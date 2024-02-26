using taur_bot_api.Api.Enums;
using taur_bot_api.Database.Models;
using taur_bot_api.src.Api.Enums;

namespace taur_bot_api.src.Database.Models
{
	public class Withdraw
	{
		public long Id { get; set; }
		public DateTime CreationDate { get; set; } = DateTime.UtcNow;
		public decimal Sum { get; set; }
		public long UserId { get; set; }
		public User User { get; set; } = null!;
		public string Network {  get; set; } = WalletType.Trc20.ToString();

		public string Status { get; set; } = WithdrawStatus.Waiting.ToString();
	}
}
