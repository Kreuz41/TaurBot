using taur_bot_api.Database.Models;

namespace taur_bot_api.Api.ResponseDto.Responses;

public class UserProfileDto(User user)
{
    public long Id { get; init; } = user.Id;
    public string Name { get; init; } = user.Name;
    public decimal Balance { get; init; } = user.Balance;
    public decimal InvestedBalance { get; init; } = user.InvestedBalance;
    public bool IsActive { get; init; } = user.IsActive;
    public string ReferralCode { get; init; } = user.ReferralCode;
    public string? Referrer { get; set; }
}