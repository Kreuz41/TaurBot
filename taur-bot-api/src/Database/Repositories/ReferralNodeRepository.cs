using Microsoft.EntityFrameworkCore;
using taur_bot_api.Database.Models;

namespace taur_bot_api.Database.Repositories;

public class ReferralNodeRepository(AppDbContext context) : Repository(context)
{
    public async Task CreateNode(long referrerId, long referralId, int inline)
    {
        var refNode = new ReferralNode
        {
            ReferralId = referralId,
            ReferrerId = referrerId,
            Inline = inline
        };

        await context.ReferralNodes.AddAsync(refNode);
        await SaveChangesAsync();
    }
	public async Task<List<ReferralNode>> GetTree(long userId, int offset)
	{
		int pageSize = 10;
		var treePage = await context.ReferralNodes
			.Include(r => r.Referral)
			.Where(r => r.ReferrerId == userId)
			.OrderBy(r => r.Id)
			.Skip(offset * pageSize)
			.Take(pageSize)
			.ToListAsync();

		return treePage;
	}
	public async Task<List<ReferralNode>> GetFullTree(long userId) => await context.ReferralNodes.Include(r => r.Referral).Where(r => r.ReferrerId == userId).ToListAsync();
	public async Task<int> GetLine(long referrerId, long referralId)
    {
        var refInfo = await context.ReferralNodes.FirstOrDefaultAsync(r => r.ReferrerId == referrerId && r.ReferralId == referralId);
        return refInfo == null ? 0 : refInfo.Inline;
    }
}