using System.Net;
using System.Text.Json;
using taur_bot_api.Api.Enums;
using taur_bot_api.Api.Services.Providers.Interfaces;
using taur_bot_api.Database.Models;

namespace taur_bot_api.Api.Services.Providers.Networks;

public class Trc20Provider(IHttpClientFactory httpClientFactory) : ICryptoApiProvider
{
	private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

		public async Task<CryptoCreatedWallet?> CreateWallet()
		{
			var httpClient = httpClientFactory.CreateClient("CryptoApi");
			using var response = await httpClient.GetAsync("trc20/create_wallet");
			if (response.StatusCode != HttpStatusCode.OK) return null;
			return await response.Content.ReadFromJsonAsync<CryptoCreatedWallet>();
		}

		public WalletType Type => WalletType.Trc20;

		public async Task<decimal?> GetWalletBalance(string walletAddress)
		{
			var httpClient = httpClientFactory.CreateClient("CryptoApi");
			using var response = await httpClient.GetAsync($"trc20/get_balance_usdt/{walletAddress}");
			if (response.StatusCode != HttpStatusCode.OK) return null;
			var updated = await response.Content.ReadFromJsonAsync<CryptoWalletBalance>();
			return updated?.Balance;
		}
		
		public async Task<bool> TransferToken(Wallet fromWallet, Wallet toWallet, decimal amount)
		{
			if (!await TransferGasToWallet(toWallet, fromWallet.Address, 0.001m)) return false;
			return await TransferTokenNoFee(fromWallet, toWallet.Address, amount);
		}
		
		public async Task<bool> TransferTokenNoFee(Wallet fromWallet, string toWalletAddress, decimal amount)
		{
			try
			{
				var httpClient = httpClientFactory.CreateClient("CryptoApi");

				var transferRequest = new
				{
					from = new { address = fromWallet.Address, privateKey = fromWallet.PrivateKey },
					to = new {address = toWalletAddress},
					amount
				};
				
				using var response = await httpClient.PostAsJsonAsync("trc20/transfer_usdt", transferRequest);
				return response.StatusCode == HttpStatusCode.OK;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async Task<bool> TransferGasToWallet(Wallet fromWallet, string toWalletAddress, decimal amount)
		{
			try
			{
				var httpClient = httpClientFactory.CreateClient("CryptoApi");

				var transferRequest = new
				{
					from = new { address = fromWallet.Address, privateKey = fromWallet.PrivateKey },
					to = new {address = toWalletAddress},
					amount
				};
				using var response = await httpClient.PostAsJsonAsync("trc20/transfer_gas", transferRequest);
				return response.StatusCode == HttpStatusCode.OK;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
}