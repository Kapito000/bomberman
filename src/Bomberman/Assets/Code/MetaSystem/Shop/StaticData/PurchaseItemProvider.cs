using System.Collections.Generic;
using System.Linq;
using Gameplay.UI.Shop.ShopContent;
using UnityEngine;
using Menu = Constant.CreateAssetMenu.Path;

namespace MetaSystem.Shop.StaticData
{
	[CreateAssetMenu(menuName = c_createAssetMenu)]
	public sealed class PurchaseItemProvider : ScriptableObject,
		IPurchaseItemProvider
	{
		const string c_createAssetMenu = Menu.c_StaticData + Menu.c_Purchasable +
			nameof(PurchaseItemProvider);

		[SerializeField] PurchasableItem[] _items;

		public bool TryGet(string id, out PurchasableItem itemData)
		{
			foreach (var item in _items)
			{
				if (item.Id == id)
				{
					itemData = item;
					return true;
				}
			}

			itemData = default;
			return false;
		}

		public IEnumerable<PurchasableItem> Get(params string[] ids) =>
			_items.Where(item => ids.Any(id => id == item.Id));

		public IEnumerable<PurchasableItem> Enumerate(ShopContentMode category)
		{
			foreach (var item in _items)
				if (item.ShopCategory == category)
					yield return item;
		}
	}
}