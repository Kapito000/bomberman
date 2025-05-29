using System;
using Common.Dictionary;
using Gameplay.UI.Shop.ShopContent;

namespace Gameplay.UI.Shop.Installer
{
	[Serializable]
	public sealed class ShopContentListDict
		: SerializedDictionary<ShopContentMode, ShopContentViewsList>
	{ }
}