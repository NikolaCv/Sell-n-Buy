using System.ComponentModel.DataAnnotations;

namespace SellnBuy.Api.Entities.Validation;

public class IntIdRequiredAttribute : ValidationAttribute
{
	public override bool IsValid(object? value)
	{
		return value != null && (int)value > 0;
	}
}

public class StringIdRequiredAttribute : ValidationAttribute
{
	public override bool IsValid(object? value)
	{
		return value != null && (string)value != "";
	}
}
