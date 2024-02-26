using taur_bot_api.src.Api.Enums;

namespace taur_bot_api.src.Api.RequestDto
{
	public class OperationCreateDto
	{
		public OperationsType Type { get; set; }
		public decimal Sum { get; set; }
	}
}
