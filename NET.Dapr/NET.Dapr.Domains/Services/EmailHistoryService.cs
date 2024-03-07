using NET.Dapr.Domains.Entities;
using NET.Dapr.Domains.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NET.Dapr.Domains.Consts;

namespace NET.Dapr.Domains.Services
{
    internal interface IEmailHistoryService { 
        Task<bool> AddEmailHistory(EmailHistoryModel input);
    }
    internal class EmailHistoryService(IUnitOfWork unitOfWork): IEmailHistoryService
    {
       readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> AddEmailHistory(EmailHistoryModel input,long transactionId)
        {
            var emailHistoryDbSet = _unitOfWork.GetDbSet<EmailHistories>();
            var emailHistory = new EmailHistories()
            {
                Subject = input.Subject,
                Content = input.Body,
                ToEmail = input.To,
                CcEmail = input.Cc,
                BccEmail = input.Bcc,
                Status = (int)LREmailStatus.Sent,
                TransactionID = input.TransactionId
            };
            await emailHistoryDbSet.AddAsync(emailHistory);
        }

        int impactRows=await _unitOfWork.SaveChangesAsync();
        bool isSuccess = impactRows > 0;
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
