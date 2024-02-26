namespace taur_bot_api.Api.RequestDto;

public class UserCreateDto
{
    public string Name { get; set; } = null!;
    public string? ReferrerCode { get; set; } = null;
    public int Local { get; set; } = 0;
    public long TelegramId { get; set; } = 0;
}