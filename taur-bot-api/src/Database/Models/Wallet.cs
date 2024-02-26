namespace taur_bot_api.Database.Models;

public class Wallet
{
    public long Id { get; set; }
    public string Address { get; set; } = null!;
    public string PrivateKey { get; set; } = null!;
    public string NetworkType { get; set; } = null!;
    public long UserId { get; set; }
    public User User { get; set; } = null!;
}