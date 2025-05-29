using System;
using Cysharp.Threading.Tasks;
using RemotePlugin.Remote;
using RemotePlugin.Remote.Data;

namespace RemotePlugin.UserDataService
{
	public interface IUserDataService
	{
		IExceptionHandler ExceptionHandler { get; }
		TelegramData TelegramData { get; }

		bool HasItem(string id);
		bool HasUnique(string id);
		bool HasCountable(string id);
		int GetItemCount(string id);
		int GetItemSpentAmount(string id);

		UniTask<bool> TryStartRound();
		UniTask<bool> TryFinishRound();
		UniTask<bool> TryPurchaseCountableItem(string id, int amount,
			string priceId, double priceAmount);
		
		UniTask<bool> TryPurchaseUniqueItem(string id, string priceId,
			double priceAmount);
		
		UniTask<LeaderboardResponse> GetLeaderboard(bool askPlayer, DateTime since,
			int limit, int offset = 0);
	}
}