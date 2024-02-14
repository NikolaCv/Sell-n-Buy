using System.ComponentModel.DataAnnotations;

namespace SellnBuy.Api.Entities.Validation;

public class IdRequiredAttribute : ValidationAttribute
{
	public override bool IsValid(object? value)
	{
		return value != null && (int)value > 0;
	}
}