using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common;
public abstract class BaseValueObject<T> : ValueObject
{
    public T Id { get; set; }

}
