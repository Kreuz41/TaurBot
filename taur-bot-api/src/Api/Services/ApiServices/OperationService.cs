
using taur_bot_api.src.Api.Enums;
using taur_bot_api.src.Api.RequestDto;
using taur_bot_api.src.Database.Models;
using taur_bot_api.src.Database.Repositories;

namespace taur_bot_api.src.Api.Services.ApiServices;

public class OperationService(OperationsRepository repository)
{
	public async Task CreateOperation( decimal sum, long userId, OperationsType type = OperationsType.Deposit)
	{
		var dto = new OperationCreateDto
		{
			Type = type,
			Sum = sum,
		};
		await repository.CreateOperation(dto, userId); 
	}

	public async Task<IEnumerable<Operation>> GetUserOperations(long userId, int offset) => await repository.GetOperationsByUserId(userId, offset);

}
