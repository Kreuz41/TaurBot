using taur_bot_api.Api.Enums;

namespace taur_bot_api.src.Api.RequestDto
{
	public class WithdrawCreateDto
	{
		public decimal Sum { get; set; }
		public string Network { get; set; } = WalletType.Trc20.ToString();
	}
}
