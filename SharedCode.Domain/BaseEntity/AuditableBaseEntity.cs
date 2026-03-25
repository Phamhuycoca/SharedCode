using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Domain.BaseEntity
{
    public abstract class AuditableBaseEntity
    {
        public virtual Guid id { get; set; }
        public string? created_by { get; set; } = string.Empty;
        public DateTime created { get; set; }
        public string? updated_by { get; set; } = string.Empty;
        public DateTime? updated { get; set; }
    }
}
