using NET.Dapr.Domains.Infra;

namespace NET.Dapr.Domains.Services
{
    internal interface IEmailHistoryService { 
        Task<bool> AddEmailHistory(EmailHistoryModel input);
    }
    internal class EmailHistoryService(IUnitOfWork unitOfWork) : IEmailHistoryService
    {
       readonly IUnitOfWork _unitOfWork = unitOfWork;

        public Task<bool> AddEmailHistory(EmailHistoryModel input)
        {
            throw new NotImplementedException();
        }
    }
    internal class EmailHistoryModel
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
    }
}
