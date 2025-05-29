using Infrastructure.Factory.Kit;
using UnityEngine;
using Zenject;
using ShopPurchasableItemView =
	Gameplay.UI.Shop.ShopContent.PurchasableItemView;
using ShoppingCartPurchasableItemView =
	Gameplay.UI.ShoppingCart.PurchasableItemView;

namespace Gameplay.UI.Shop.Factory
{
	public class ShopUiFactory : IShopUiFactory
	{
		[Inject] IFactoryKit _kit;

		public ShopPurchasableItemView CreateShopItemView(Transform parent)
		{
			var prefab = _kit.AssetProvider.ShopItemView();
			var instance = _kit.InstantiateService
				.Instantiate<ShopPurchasableItemView>(prefab, parent);
			return instance;
		}

		public ShoppingCartPurchasableItemView CreateShoppingCartItemView(
			Transform parent)
		{
			var prefab = _kit.AssetProvider.ShoppingCartItemView();
			var instance = _kit.InstantiateService
				.Instantiate<ShoppingCartPurchasableItemView>(prefab, parent);
			return instance;
		}
	}
}