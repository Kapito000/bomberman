using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logger;
using Cysharp.Threading.Tasks;
using Extensions;
using Gameplay.UI.Popup;
using Gameplay.UI.Popup.Window;
using Gameplay.Windows;
using MetaSystem.Shop.StaticData;
using UnityEngine;
using Zenject;

namespace MetaSystem.Shop
{
	public sealed class ShoppingCartService : IShoppingCartService
	{
		[Inject] IShopService _shopService;
		[Inject] IWindowService _windowService;
		[Inject] IWindowDistributor _windowDistributor;
		[Inject] IPurchaseItemProvider _purchaseItemProvider;

		List<CartItemContainer> _items = new();

		public event Action DataChanged;

		public IReadOnlyList<CartItemContainer> Items => _items;

		public void Put(string itemId)
		{
			if (IsItemIdCorrect(itemId, out var purchaseItem) == false)
				return;

			if (purchaseItem.Unique)
				AddUniqueItem(itemId);
			else
				AddCoutableItem(itemId);

			OnDataChanged();
		}

		public void Pop(string itemId, int quantity)
		{
			if (IsItemIdCorrect(itemId, out var purchaseItem) == false)
				return;
			
			if (TryPop(itemId, quantity))
			{
				OnDataChanged();
			}
		}

		public async UniTask RequestPurchaseAsync()
		{
			var infos = ShopItemInfos();

			if (false == _shopService
				    .PurchaseAvailableByPriceAmount(infos, out var tryPurchaseAsync))
			{
				ProcessNotEnoughCurrency();
				return;
			}

			var results = await tryPurchaseAsync.Invoke();
			for (int i = 0; i < infos.Length; i++)
			{
				if (results[i])
					Pop(infos[i].Id, infos[i].Amount);
			}

			ProcessPurchaseFail();
		}

		bool TryPop(string itemId, int quantity)
		{
			quantity = quantity < 1 ? 1 : quantity;

			if (false == _items.TryGetIndex(x => x.ItemId == itemId, out var index))
				return false;

			if (_items[index].Unique == false
			    || _items[index].Quantity > 1)
			{
				var item = _items[index];
				item.Quantity -= quantity;
				if (item.Quantity < 1)
				{
					_items.RemoveAt(index);
					return true;
				}

				_items[index] = item;
				return true;
			}

			_items.RemoveAt(index);
			return true;
		}

		ShopItemInfo[] ShopItemInfos()
		{
			var infos = new ShopItemInfo[_items.Count];
			for (var i = 0; i < _items.Count; i++)
				infos[i] = ConvertToShopItemInfo(_items[i]);
			return infos;
		}

		void AddCoutableItem(string itemId)
		{
			var index = _items.FindIndex(x => x.ItemId == itemId);
			if (index == -1)
				CreateCartItemContainer(itemId);
			else
				IncrementItemQuantity(index);
		}

		void AddUniqueItem(string itemId)
		{
			if (_items.Any(x => x.ItemId == itemId) == false)
				CreateCartItemContainer(itemId, true);
		}

		bool IsItemIdCorrect(string itemId, out PurchasableItem purchaseItem)
		{
			if (string.IsNullOrEmpty(itemId)
			    || false == _purchaseItemProvider.TryGet(itemId, out purchaseItem))
			{
				Error.Incorrect($"{nameof(itemId)}: \"{itemId}\".");
				purchaseItem = default;
				return false;
			}

			return true;
		}

		void IncrementItemQuantity(int index)
		{
			var cartItem = _items[index];
			cartItem.Quantity++;
			_items[index] = cartItem;
		}

		void CreateCartItemContainer(string itemId, bool unique = false)
		{
			_items.Add(new CartItemContainer()
			{
				ItemId = itemId,
				Quantity = 1,
				Unique = unique,
			});
		}

		void OnDataChanged()
		{
			DataChanged?.Invoke();
		}

		ShopItemInfo ConvertToShopItemInfo(CartItemContainer item)
		{
			if (_purchaseItemProvider.TryGet(item.ItemId, out var info) == false)
				Debug.LogError($"Cannot to find shop item with id: {item.ItemId}");

			var priceAmount = info.Cost;

			return new ShopItemInfo
			{
				Id = item.ItemId,
				Unique = item.Unique,
				Amount = item.Quantity,
				PriceId = Constant.Purchases.c_CurrencyId,
				PriceAmount = priceAmount,
			};
		}

		void ProcessNotEnoughCurrency()
		{
			if (_windowDistributor.TryGetPopupWindowWithDebug(out var popup) == false)
				return;

			popup.Refresh(RefreshData.NotEnoughCurrency());
			_windowService.Open(WindowId.Popup);
		}

		void ProcessPurchaseFail()
		{
			if (_items.Count <= 0) return;

			if (_windowDistributor.TryGetPopupWindowWithDebug(out var popup) == false)
				return;

			var ids = Items
				.Select(x => x.ItemId)
				.ToArray();
			var purchasableNames = _purchaseItemProvider
				.Get(ids)
				.Select(x => x.Name)
				.ToArray();

			popup.Refresh(RefreshData.PurchaseFailData(purchasableNames));
			_windowService.Open(WindowId.Popup);
		}
	}
}