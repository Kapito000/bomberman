using Gameplay.Windows;

namespace Gameplay.UI.Popup.Window
{
	public interface IPopupWindow : IWindow
	{
		void Refresh(RefreshData data);
	}
}