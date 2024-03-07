using Dapr.Workflow;
using NET.Dapr.Domains.Workflows.LeaveRequest.Activities;
using NET.Dapr.Domains.Workflows.LeaveRequest.Models;

namespace NET.Dapr.Domains.Workflows.LeaveRequest
{
    public class LeaveRequestWorkflow : Workflow<LRStartWorkflowPayload, LRStartWorkflowResult>
    {
        public override async Task<LRStartWorkflowResult> RunAsync(WorkflowContext context, LRStartWorkflowPayload input)
        {
            bool isSuccess = false;
            List<string> messages = [];
            if (string.IsNullOrEmpty(input.DivisionCode))
            {
                messages.Add("DivisionCode is required");
                return new LRStartWorkflowResult(context.InstanceId, isSuccess, messages);
            }

            var getApproverResponse =  await context.CallActivityAsync<GetApproverAndCreateTaskResponse>(
                 nameof(LR_GetApproverAndCreateTaskActivity),
                 new GetApproverAndCreateTaskRequest(
                     input.DivisionCode,input.EmployeeCode,
                     input.EmployeeName,input.DateOffFrom,
                     input.DateOffTo,input.TransactionId)
                );

            if (getApproverResponse is not null)
            {
                var sendEmailReviewerRs =  await context.CallActivityAsync<LRSendEmailNotifyResponse>(
                 nameof(LR_SendEmailNotifyActivity),
                 new LRSendEmailNotifyRequest(
                     TransactionId:input.TransactionId,
                     Subject:$"LR[{input.TransactionId}][Review for approval] Leave Request {input.EmployeeCode} - {input.EmployeeName}",
                     Body:$"Dear {getApproverResponse.Name} <br/>" +
                        $"Please review and approve for leave request of {input.EmployeeName}. <br/>" +
                        $"Leave from {input.DateOffFrom:dd/MM/yyyy} to {input.DateOffTo:dd/MM/yyyy}. <br/><br/>" +
                        $"Thanks and best regards.",
                     To:getApproverResponse.Email,
                     Cc:input.Cc,
                     Bcc:input.Bcc));

                ManagerApprovalResult managerApprovalResultResp = default;
                context.CallActivityAsync(nameof(LR_ReminderApprovalTaskActivity), new ReminderRequest(getApproverResponse.TaskId, 3, 2));
                try
                {
                    managerApprovalResultResp = await context.WaitForExternalEventAsync<ManagerApprovalResult>(
                          eventName: "ManagerApproval",
                          timeout: TimeSpan.FromMinutes(10));
                }
                catch (TaskCanceledException)
                {
                    // An approval timeout results in automatic order cancellation
                    var processTimeoutResonse = await context.CallActivityAsync<LRTimeoutResponse>(
                        nameof(LR_ApprovalTimeoutActivity),
                        new LRTimeoutRequest(TransactionId:input.TransactionId));

                    await context.CallActivityAsync(
                        nameof(LR_SendEmailNotifyActivity),
                       new LRSendEmailNotifyRequest(
                              TransactionId: input.TransactionId,
                               Subject: $"LR[{input.TransactionId}][Review approval result] Leave request - {input.EmployeeCode} - {input.EmployeeName}",
                               Body: $"Dear {getApproverResponse.Name} <br/>" +
                                  $"Leave request has been timeout. Processing automatically cancelled. <br/>" +
                                  $"Thanks and best regards.",
                               To: getApproverResponse.Email,
                               Cc: input.Cc,
                               Bcc: input.Bcc
                               ));
                    return new LRStartWorkflowResult(context.InstanceId, isSuccess, messages);
                }

                var approvalProcessResult = await context.CallActivityAsync<LRProcessApproveTransactionResponse>(
                        nameof(LR_AfterManagerApprovalActivity),
                        new LRProcessApproveTransactionRequest(
                            managerApprovalResultResp.TaskId,
                            managerApprovalResultResp.IsApproved,
                            managerApprovalResultResp.Comment));

                string approveType = managerApprovalResultResp.IsApproved?"APPROVED":"REJECTED";
                await context.CallActivityAsync<LRSendEmailNotifyResponse>(
                       nameof(LR_SendEmailNotifyActivity),
                       new LRSendEmailNotifyRequest(
                           TransactionId: input.TransactionId,
                           Subject: $"LR[{input.TransactionId}][Review approval result][{approveType}] Leave request {input.EmployeeCode} - {input.EmployeeName}",
                           Body: $"Dear {input.EmployeeName} <br/>" +
                              $"Your leave request has been {approveType} by {getApproverResponse.Name} <br/>" +
                              $"Comment: '{managerApprovalResultResp.Comment}' <br/>" +
                              $"Thanks and best regards.",
                           To: getApproverResponse.Email,
                           Cc: input.Cc,
                           Bcc: input.Bcc
                           ));

                messages.AddRange(approvalProcessResult.Messages);
            }
            else
            {
                messages.Add($"Get Approver Error. Not found approver with division code {input.DivisionCode}");
            }

            return new LRStartWorkflowResult(context.InstanceId, isSuccess, messages);
        }
    }
    public record LRStartWorkflowPayload
    {
        public long TransactionId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DivisionCode { get; set; }
        public DateTime? DateOffFrom { get; set; }
        public DateTime? DateOffTo { get; set; }
        public string Reason { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
    };
    public record LRStartWorkflowResult(string WorkflowInstaceId, bool IsSuccess, List<string> Messages);
}
