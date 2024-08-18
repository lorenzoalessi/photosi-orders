using System.Diagnostics.CodeAnalysis;

namespace PhotosiOrders.Exceptions;

[ExcludeFromCodeCoverage]
public class OrderException : Exception
{
    public OrderException()
    {
    }

    public OrderException(string message) : base(message)
    {
    }

    public OrderException(Exception exception)
    {
    }
}