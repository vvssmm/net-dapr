namespace NET.Dapr.Domains
{
    internal static class Consts
    {
        internal enum LRWorkflowStatus
        {
            GetApprover,
            WaitingForApproval,
            Approved,
            Rejected,
            Timeout
        }
        internal enum LRTaskStatus
        {
            New,
            Completed,
            Terminated
        }
        internal enum LREmailStatus
        {
            New,
            Sent,
            Error
        }
        internal enum LREmailHistoryCode
        {
            ReviewAndApprovalNotification,
            ApprovalReminderNotification,
            ApprovalResultNotification,
        }
        internal static class WfConfig
        {
            internal const string WorkflowComponet = "dapr";
            internal static class LeaveRequest
            {
                internal const string WorkflowName = "LeaveRequestWorkflow";
                internal const string WorkflowApprovalEventName = "ManagerApproval";
            }
        }
        internal const string DefaultSchema = "DAPR";
    }
}
