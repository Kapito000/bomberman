using System.Collections.Generic;
using Gameplay.StaticData;
using Gameplay.UI.Shop.ShopContent;

namespace MetaSystem.Shop.StaticData
{
	public interface IPurchaseItemProvider : IStaticData
	{
		bool TryGet(string id, out PurchasableItem item);
		IEnumerable<PurchasableItem> Enumerate(ShopContentMode category);
		IEnumerable<PurchasableItem> Get(params string[] ids);
	}
}