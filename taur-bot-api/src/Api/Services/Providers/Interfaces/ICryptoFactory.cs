using taur_bot_api.Api.Enums;
using taur_bot_api.Api.Services.Providers.Interfaces;

namespace taur_bot_api.Api.Services.Providers;

public interface ICryptoFactory
{
    ICryptoApiProvider GetNetwork(WalletType type);
    public IEnumerable<ICryptoApiProvider> Networks { get; }
}