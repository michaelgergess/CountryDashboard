using System;

namespace CountryDashboard.Domain.Common
{
    public abstract class BaseEntity
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
