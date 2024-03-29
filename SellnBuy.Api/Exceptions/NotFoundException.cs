namespace SellnBuy.Api.Exceptions;

public class NotFoundException : Exception
{
	public NotFoundException() { }

	public NotFoundException(string message) : base(message) { }

	public NotFoundException(string message, Exception innerException) : base(message, innerException) { }

	public NotFoundException(Type entityType) : base($"The {entityType.Name} with requested ID does not exist.") { }

}
