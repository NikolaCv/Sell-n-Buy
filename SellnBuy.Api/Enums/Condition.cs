using System.ComponentModel;

namespace SellnBuy.Api.Enums
{
	public enum Condition
		{
			New,
			[Description("Second Hand But Unused")]
			SecondHandButUnused,
			Used,
			[Description("Damaged But Somewhat Working")]
			DamagedButSomewhatWorking,
			[Description("Damaged And Not Working")]
			DamagedAndNotWorking
		}
}