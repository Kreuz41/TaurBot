using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using taur_bot_api.Api.RequestDto;
using taur_bot_api.Database.Models;

namespace taur_bot_api.Database.Repositories;

public class InvestmentRepository(AppDbContext context) : Repository(context)
{
    private readonly AppDbContext _context = context;

    public async Task<string> CreateInvestment(InvestmentCreateDto dto, long userId)
    {
        var response = "Ok";
        
        var investment = new Investment
        {
            DealSum = dto.DealSum,
            UserId = userId,
            InvestmentTypeId = dto.InvestmentType
        };

        _context.Investments.Add(investment);
        var res = await SaveChangesAsync();
        if (!res) response = "Internal error";
        
        return response;
    }

    public async Task<InvestmentType?> GetInvestTypeById(int id) => await _context.InvestmentType.FirstOrDefaultAsync(i => i.Id == id);

    public async Task<List<Investment>> GetAllInvestments() =>
        await _context.Investments.Where(i => !i.IsEnded).ToListAsync();
    public async Task<Investment?> GetInvestment(long id) => await _context.Investments.FindAsync(id);
    public async Task CloseInvestment(long id)
    {
        var investment = await _context.Investments.FindAsync(id);
        if (investment == null) return;
        investment.IsEnded = true;
        await SaveChangesAsync();
    }
}