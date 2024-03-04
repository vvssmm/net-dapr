using NET.Dapr.Domains.Services.Models;

namespace NET.Dapr.Domains.Services
{
    public interface ILeaveRequestService
    {
        Task<LRDataModel> SubmitForm(LRBaseModel postModel);
        Task<LRDataModel> GetLeaveRequestByWfInstanceId(string wfInstanceId);
    }
    public class LeaveRequestService (PgDbContext) : ILeaveRequestService
    {
        public Task<LRDataModel> GetLeaveRequestByWfInstanceId(string wfInstanceId)
        {
            throw new NotImplementedException();
        }

        public Task<LRDataModel> SubmitForm(LRBaseModel postModel)
        {
            throw new NotImplementedException();
        }
    }
}
