namespace Domain.Extensions;

public class BusinessException : Exception
{
    public BusinessException(string message) : base(message) { }
}