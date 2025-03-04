using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Common
{
    public interface IEntity
    {
    }

    public abstract class BaseEntity<TKey> : IEntity 
    {
        public TKey Id { get; set;}
        public bool IsDeleted { get; set;} = false;
        public DateTime? DeletedAt { get; set;}
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public abstract class BaseEntity : BaseEntity<int> { }
}
