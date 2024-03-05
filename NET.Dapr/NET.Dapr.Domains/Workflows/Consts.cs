namespace NET.Dapr.Domains.Workflows
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
            internal const string WorkflowCode = "LeaveRequestWf";
            internal static class WfSteps
            {
                internal const string WaitingForApproval = "WaitingForApproval";
            }
        }
        internal const string DefaultSchema="DAPR";
    }
}
