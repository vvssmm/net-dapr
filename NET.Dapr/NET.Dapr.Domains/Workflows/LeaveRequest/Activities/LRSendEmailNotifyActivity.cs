using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Dapr.Domains.Workflows.LeaveRequest.Activities
{
    internal record LRSendEmailNotifyRequest(string EmailCode,object Data);
    internal record LRSendEmailNotifyResponse(bool IsSuccess,string Message);
    internal class LRSendEmailNotifyActivity:
    {
    }
}
