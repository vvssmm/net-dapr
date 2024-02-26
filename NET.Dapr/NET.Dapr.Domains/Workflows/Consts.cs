namespace NET.Dapr.Domains.Workflows
{
    internal static class Consts
    {
        internal enum WORKFLOW_STATUS
        {
            SUBMITTED,
            WAITING_FOR_APPROVAL,
            APPROVED,
            REJECTED
        }

        internal enum TASK_STATUS
        {
            NEW,
            COMPLETED,
            TERMINATED
        }
    }
}
