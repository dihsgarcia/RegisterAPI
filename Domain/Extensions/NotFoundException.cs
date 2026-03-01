namespace Domain.Extensions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}