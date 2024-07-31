using System.ComponentModel.DataAnnotations.Schema;

namespace DataBase.Common;
public abstract class BaseEntity<T>
{
    public T Id { get; set; }
}
