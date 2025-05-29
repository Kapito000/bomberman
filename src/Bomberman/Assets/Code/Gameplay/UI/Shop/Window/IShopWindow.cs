using Gameplay.UI.Shop.ShopContent;
using Gameplay.Windows;

namespace Gameplay.UI.Shop.Window
{
	public interface IShopWindow : IWindow
	{
		void ProcessSelectedItem(string id);
		void SwitchContent(ShopContentMode modeTarget);
	}
}