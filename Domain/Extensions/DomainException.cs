namespace Domain.Extensions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}