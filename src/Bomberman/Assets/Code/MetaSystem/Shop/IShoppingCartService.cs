using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure;

namespace MetaSystem.Shop
{
	public interface IShoppingCartService : IService
	{
		void Put(string itemId);
		IReadOnlyList<CartItemContainer> Items { get; }
		event Action DataChanged;
		void Pop(string itemId, int quantity = 1);
		UniTask RequestPurchaseAsync();
	}
}