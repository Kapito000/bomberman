using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure;

namespace MetaSystem.Shop
{
	public interface IShopService : IService
	{
		event Action PurchaseStarting;
		event Action PurchaseEnded;

		bool PurchaseAvailableByPriceAmount(
			IReadOnlyList<ShopItemInfo> infos);

		bool PurchaseAvailableByPriceAmount(IReadOnlyList<ShopItemInfo> infos,
			out Func<UniTask<bool[]>> tryPurchaseAsync);
	}
}