namespace PhotosiOrders.Exceptions;

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