using taur_bot_api.Database.Models;
using taur_bot_api.src.Api.Enums;

namespace taur_bot_api.src.Database.Models;

public class Operation
{
	public long Id { get; set; }
	public decimal Sum { get; set; }
	public DateTime CreationTime { get; set; } = DateTime.UtcNow;
	public string Type { get; set; } = OperationsType.Deposit.ToString();
	public long UserId { get; set; }
	public User User { get; set; } = null!;
}
