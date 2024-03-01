namespace NET.Dapr.Domains.Workflows
{
    internal static class Consts
    {
        internal enum WorkflowStatus
        {
            GetApprover,
            WaitingForApproval,
            Approved,
            Rejected,
            Timeout
        }
        internal enum TaskStatus
        {
            New,
            Completed,
            Terminated
        }
        internal enum EmailHistoryCode
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
    }
}
