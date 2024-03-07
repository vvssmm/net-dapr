using AutoMapper;
using NET.Dapr.Domains.Entities;
using NET.Dapr.Domains.Services;
using NET.Dapr.Domains.Workflows.LeaveRequest;

namespace NET.Dapr.Domains
{
    public class MapperProfiles:Profile
    {
        public MapperProfiles()
        {
            CreateMap<LRTransaction, LRBaseModel>().ReverseMap();
            CreateMap<LRTransaction, LRDataModel>().ReverseMap();
            CreateMap<LRTransaction, LRStartWorkflowPayload>().ReverseMap();
            CreateMap<LRTasks, LRTaskApiModel>().ReverseMap();
        }
    }
}
