using taur_bot_api.Api.Enums;
using taur_bot_api.Database.Models;

namespace taur_bot_api.Api.Services.Providers.Interfaces;

public interface ICryptoApiProvider
{
    public Task<CryptoCreatedWallet?> CreateWallet();
    public WalletType Type { get; }
    public Task<decimal?> GetWalletBalance(string walletAddress);
    public Task<bool> TransferToken(Wallet fromWallet, Wallet toWallet, decimal amount);
    public Task<bool> TransferTokenNoFee(Wallet fromWallet, string toWalletAddress, decimal amount);
    public Task<bool> TransferGasToWallet(Wallet fromWallet, string toWalletAddress, decimal amount);
}