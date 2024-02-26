namespace taur_bot_api.Database.Models;

public class InvestmentType
{
    public int Id { get; set; }
    public decimal MinValue { get; set; }
    public ICollection<Investment> Investments { get; set; } = new List<Investment>();
}