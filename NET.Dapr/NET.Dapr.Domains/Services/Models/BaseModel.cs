﻿namespace NET.Dapr.Domains.Services.Models
{
    public class BaseModel
    {
        public long Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
