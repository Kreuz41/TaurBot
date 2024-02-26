using taur_bot_api.Api.RequestDto;
using taur_bot_api.Database.Models;
using taur_bot_api.Database.Repositories;
using taur_bot_api.Database;
using taur_bot_api.src.Api.Enums;
using taur_bot_api.src.Database.Models;
using Microsoft.EntityFrameworkCore;
using taur_bot_api.src.Api.RequestDto;

namespace taur_bot_api.src.Database.Repositories;

public class OperationsRepository(AppDbContext context) : Repository(context)
{
	private readonly AppDbContext _context = context;

	public async Task<string> CreateOperation(OperationCreateDto dto, long userId)
	{
		var response = "Ok";

		var investment = new Operation
		{
			Sum = dto.Sum,
			UserId = userId,
			Type = dto.Type.ToString(),
		};

		_context.Operations.Add(investment);
		var res = await SaveChangesAsync();
		if (!res) response = "Internal error";

		return response;
	}

	public async Task<IEnumerable<Operation>> GetOperationsByUserId(long id, int offset) => await _context.Operations.Where(o => o.UserId == id).OrderBy(o=>o.Id).Skip(offset*10).Take(10).ToListAsync();

}
