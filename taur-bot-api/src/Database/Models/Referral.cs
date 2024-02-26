namespace taur_bot_api.Database.Models;

public class ReferralNode
{
    public long Id { get; set; }
    public long ReferralId { get; set; }
    public User Referral { get; set; } = null!;
    public int Inline { get; set; }
    public long ReferrerId { get; set; }
    public User Referrer { get; set; } = null!;
}