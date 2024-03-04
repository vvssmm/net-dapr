using Dapr.Workflow;
using NET.Dapr.Domains.Workflows.LeaveRequest.Activities;
using NET.Dapr.Domains.Workflows.LeaveRequest.Models;
using static NET.Dapr.Domains.Workflows.Consts;

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
            var getApproverResponse =  await context.CallActivityAsync<GetApproverResponse>(
                 nameof(LRGetApproverActivity),
                 new GetApproverRequest(input.DivisionCode)
                );

            if (getApproverResponse is not null)
            {
                var sendEmailReviewerRs =  await context.CallActivityAsync<LRSendEmailNotifyResponse>(
                 nameof(LRSendEmailNotifyActivity),
                 new LRSendEmailNotifyRequest(
                     EmailHistoryCode.ReviewAndApprovalNotification.ToString(),
                     new
                     {
                         getApproverResponse.Name,
                         getApproverResponse.Email,
                         getApproverResponse.Code
                     }));

                ManagerApprovalResult managerApprovalResultResp = default;
                context.SetCustomStatus(WorkflowStatus.WaitingForApproval.ToString());

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
                        nameof(LRProcessTimeoutTransactionActivity),
                        new LRTimeoutRequest());

                    await context.CallActivityAsync(
                        nameof(LRSendEmailNotifyActivity),
                        new LRSendEmailNotifyRequest(
                            EmailHistoryCode.ApprovalResultNotification.ToString(),
                            new
                            {
                                getApproverResponse.Name,
                                getApproverResponse.Email,
                                getApproverResponse.Code
                            }));

                    context.SetCustomStatus(WorkflowStatus.Timeout.ToString());
                    return new LRStartWorkflowResult(context.InstanceId, isSuccess, messages);
                }

                var approvalProcessResult = await context.CallActivityAsync<LRProcessApproveTransactionResponse>(
                        nameof(LRProcessApproveTransactionActivity),
                        new LRProcessApproveTransactionRequest(
                            DateTime.Now,
                            managerApprovalResultResp.IsApproved
                            ,managerApprovalResultResp.Comment));

                await context.CallActivityAsync(
                        nameof(LRSendEmailNotifyActivity),
                        new LRSendEmailNotifyRequest(
                            EmailHistoryCode.ApprovalResultNotification.ToString(),
                            new
                            {
                                getApproverResponse.Name,
                                getApproverResponse.Email,
                                getApproverResponse.Code
                            }));

                isSuccess = approvalProcessResult.IsSuccess;

                if(isSuccess) context.SetCustomStatus(WorkflowStatus.Approved.ToString());
                context.SetCustomStatus(WorkflowStatus.Rejected.ToString());

                messages.AddRange(approvalProcessResult.Messages);
            }
            else
            {
                messages.Add($"Get Approver Error. Not found approver with division code {input.DivisionCode}");
            }

            return new LRStartWorkflowResult(context.InstanceId, isSuccess, messages);
        }
    }
    public record LRStartWorkflowPayload(
    string EmployeeCode, string EmployeeName, string DivisionCode, DateTime? DateOffFrom,
    DateTime? DateOffTo, string LeaveRequestType, string Reason);
    public record LRStartWorkflowResult(string WorkflowInstaceId, bool IsSuccess, List<string> Messages);
}
