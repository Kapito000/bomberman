using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using RemotePlugin.Remote;
using RemotePlugin.UserDataService;
using UnityEngine;

namespace MetaSystem.Shop
{
	public sealed class ShopService : IShopService
	{
		public event Action PurchaseStarting;
		public event Action PurchaseEnded;

		public bool PurchaseAvailableByPriceAmount(
			IReadOnlyList<ShopItemInfo> infos)
		{
			foreach (var info in infos)
			{
				var priceItemCount = UserDataService().GetItemCount(info.PriceId);
				if (info.PriceAmount > priceItemCount)
					return false;
			}

			return true;
		}

		public bool PurchaseAvailableByPriceAmount(
			IReadOnlyList<ShopItemInfo> infos,
			out Func<UniTask<bool[]>> tryPurchaseAsync)
		{
			if (PurchaseAvailableByPriceAmount(infos))
			{
				tryPurchaseAsync = () => TryPurchaseAsync(infos);
				return true;
			}

			tryPurchaseAsync = null;
			return false;
		}

		async UniTask<bool[]> TryPurchaseAsync(
			IReadOnlyList<ShopItemInfo> purchasableDataset)
		{
			var tasks = CreatePurchaseTasks(purchasableDataset);
			PurchaseStarting?.Invoke();
			var results = await ProcessPurchasesTasksAsync(tasks);
			PurchaseEnded?.Invoke();

			return results;
		}

		async Task<bool[]> ProcessPurchasesTasksAsync(UniTask<bool>[] tasks)
		{
			try
			{
				return await UniTask.WhenAll(tasks);
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
				var results = new bool[tasks.Length];
				for (var i = 0; i < tasks.Length; i++)
					results[i] = tasks[i].Status != UniTaskStatus.Faulted;

				return results;
			}
		}

		UniTask<bool>[] CreatePurchaseTasks(
			IReadOnlyList<ShopItemInfo> purchasableDataset)
		{
			var tasks = new UniTask<bool>[purchasableDataset.Count];

			for (var i = 0; i < purchasableDataset.Count; i++)
			{
				var info = purchasableDataset[i];
				var task = TryPurchaseAsync(info);
				tasks[i] = task;
			}

			return tasks;
		}

		async UniTask<bool> TryPurchaseAsync(ShopItemInfo info)
		{
			bool result;
			if (info.Unique)
				result = await Services.UserDataService.TryPurchaseUniqueItem(
					info.Id, info.PriceId, info.PriceAmount);
			else
				result = await Services.UserDataService.TryPurchaseCountableItem(
					info.Id, info.Amount, info.PriceId, info.PriceAmount);

			return result;
		}

		IUserDataService UserDataService() =>
			Services.UserDataService;
	}
}