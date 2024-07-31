using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common;
public abstract class BaseEntity<T>
{
    public T Id { get; set; }
}
