namespace DataBase.Common;
public abstract class BaseValueObject<T> : ValueObject
{
    public T Id { get; set; }

}
