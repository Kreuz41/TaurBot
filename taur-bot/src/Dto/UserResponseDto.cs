namespace taur_bot.src.Dto;

public class UserResponseDto
{
	public long Id { get; init; }
	public string Name { get; init; } 
	public decimal Balance { get; init; } 
	public decimal InvestedBalance { get; init; } 
	public bool IsActive { get; init; }
	public string ReferralCode { get; init; } 
	public string? Referrer { get; set; }
}
