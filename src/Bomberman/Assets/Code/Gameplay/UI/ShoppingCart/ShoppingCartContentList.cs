using System.Collections.Generic;
using System.Linq;
using Common.Logger;
using Extensions;
using Gameplay.UI.Shop.Factory;
using Gameplay.UI.Shop.ShopContent;
using MetaSystem.Shop;
using MetaSystem.Shop.StaticData;
using UnityEngine;
using Zenject;

namespace Gameplay.UI.ShoppingCart
{
	public sealed class ShoppingCartContentList
		: ViewsList<PurchasableItemView, PurchasableItemView.RefreshData>
	{
		[SerializeField] Transform _contentParent;

		[Inject] IShopUiFactory _shopUiFactory;
		[Inject] IShoppingCartService _shoppingCartService;
		[Inject] IPurchaseItemProvider _purchaseItemProvider;

		public void Refresh()
		{
			var items = ContentRefreshData();
			Refresh(items);
		}

		public void Show()
		{
			gameObject.SetActive(true);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		public void BlockItems(bool block = true)
		{
			foreach (var itemView in ItemsViews()) 
				itemView.Block(block);
		}

		protected override void HideItemView(int index) =>
			ItemsViews()[index].Hide();

		protected override void ShowItemView(int index) =>
			ItemsViews()[index].Show();

		protected override void RefreshItemView(int index,
			PurchasableItemView.RefreshData item) =>
			ItemsViews()[index].Refresh(item);

		protected override PurchasableItemView CreateItemView()
		{
			var shopItemView =
				_shopUiFactory.CreateShoppingCartItemView(_contentParent);
			shopItemView.Init();
			return shopItemView;
		}

		PurchasableItemView.RefreshData[] ContentRefreshData()
		{
			var result = new List<PurchasableItemView.RefreshData>();
			var itemsIds = _shoppingCartService
				.Items
				.Select(x => x.ItemId)
				.ToArray();
			foreach (var purchasableItem in _purchaseItemProvider.Get(itemsIds))
			{
				if (false == _shoppingCartService.Items
					    .TryGet(x => x.ItemId == purchasableItem.Id, out var cartItem))
				{
					Debug.LogError("Cannot to get shopping cart item.");
					continue;
				}

				var refreshData = new PurchasableItemView.RefreshData()
				{
					Item = purchasableItem,
					Count = cartItem.Quantity,
				};
				result.Add(refreshData);
			}

			return result.ToArray();
		}
	}
}