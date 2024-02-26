namespace taur_bot_api.Database.Models;

public class Investment
{
    public long Id { get; set; }
    public decimal TotalPercent { get; set; }
    public decimal DealSum { get; set; }
    public int InvestmentTypeId { get; set; }
    public InvestmentType InvestmentType { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public bool IsEnded { get; set; } = false;
    public bool HaveFirstAccrual { get; set; } = false;
    public long UserId { get; set; }
    public User User { get; set; } = null!;
}