using System.Linq;
using Gameplay.UI.Shop.Factory;
using Gameplay.UI.Shop.Window;
using MetaSystem.Shop.StaticData;
using UnityEngine;
using Zenject;
using RemoteServices = RemotePlugin.Remote;
using PurchasableItem = MetaSystem.Shop.StaticData.PurchasableItem;

namespace Gameplay.UI.Shop.ShopContent
{
	public sealed class ShopContentViewsList
		: ViewsList<PurchasableItemView, PurchasableItem>
	{
		[SerializeField] Transform _contentParent;

		[Inject] IShopWindow _shopWindow;
		[Inject] IShopUiFactory _shopUiFactory;
		[Inject] IPurchaseItemProvider _purchaseItemProvider;

		public ShopContentViewsList Refresh(ShopContentMode mode)
		{
			var items = _purchaseItemProvider.Enumerate(mode).ToArray();

			Refresh(items);
			DeselectAll();

			return this;
		}

		public void Show()
		{
			gameObject.SetActive(true);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		public void Select(PurchasableItemView view)
		{
			if (IsNull(view))
				return;

			DeselectAll();
			view.Select();

			_shopWindow.ProcessSelectedItem(view.ItemId);
		}

		protected override PurchasableItemView CreateItemView()
		{
			var shopItemView = _shopUiFactory.CreateShopItemView(_contentParent);
			shopItemView.Init(this);
			return shopItemView;
		}

		protected override void HideItemView(int index) => 
			ItemsViews()[index].Hide();

		protected override void ShowItemView(int index) => 
			ItemsViews()[index].Show();

		protected override void RefreshItemView(int index, PurchasableItem item)
		{
			bool purchased = RemoteServices.Services.UserDataService
				.HasUnique(item.Id);
			ItemsViews()[index].Refresh(item, purchased);
		}

		void DeselectAll()
		{
			foreach (var view in ItemsViews())
			{
				if (IsNull(view))
					continue;

				view.Deselect();
			}
		}

		bool IsNull(PurchasableItemView view)
		{
			if (view == null)
			{
				Debug.LogError($"{nameof(view)} is null.");
				return true;
			}
			return false;
		}
	}
}