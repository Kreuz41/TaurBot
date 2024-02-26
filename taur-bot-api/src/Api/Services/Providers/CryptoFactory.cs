using taur_bot_api.Api.Enums;
using taur_bot_api.Api.Services.Providers.Interfaces;

namespace taur_bot_api.Api.Services.Providers;

public class CryptoFactory : ICryptoFactory
{
    private readonly IEnumerable<ICryptoApiProvider> _networks;
    public IEnumerable<ICryptoApiProvider> Networks { get => _networks; }
    
    public CryptoFactory(IEnumerable<ICryptoApiProvider> networks)
    {
        _networks = networks;
    }
    
    public ICryptoApiProvider GetNetwork(WalletType type)
    {
        return _networks.First(n => n.Type == type);
    }
}