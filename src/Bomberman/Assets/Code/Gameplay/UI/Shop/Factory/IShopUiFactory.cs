using Gameplay.UI.Shop.ShopContent;
using UnityEngine;

namespace Gameplay.UI.Shop.Factory
{
	public interface IShopUiFactory
	{
		PurchasableItemView CreateShopItemView(Transform parent);

		ShoppingCart.PurchasableItemView CreateShoppingCartItemView(
			Transform parent);
	}
}