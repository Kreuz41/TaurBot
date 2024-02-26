using taur_bot_api.src.Database.Models;

namespace taur_bot_api.Database.Models;

public class User
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Balance { get; set; }
    public decimal InvestedBalance { get; set; }
    public bool IsActive { get; set; }
    public bool IsAdmin { get; set; }
    public string ReferralCode { get; set; } = string.Empty;
    public string WithdrawWallet { get; set; } = string.Empty;
    public int LocalId { get; set; }
    public int State { get; set; }
    public long TelegramId { get; set; }
    public string? Nickname { get; set; }
    public long? ReferrerId { get; set; }
    public User? Referrer { get; set; } 
    public ICollection<Investment> Investments { get; set; } = new List<Investment>();
    public ICollection<ReferralNode> ReferralNodes { get; set; } = new List<ReferralNode>();
    public ICollection<ReferralNode> ReferrerNodes { get; set; } = new List<ReferralNode>();
    public ICollection<User> Referrals { get; set; } = new List<User>();
    public ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
    public ICollection<Withdraw> Withdraws { get; set; } = new List<Withdraw>();
    public ICollection<Operation> Operations { get; set; } = new List<Operation>();
}