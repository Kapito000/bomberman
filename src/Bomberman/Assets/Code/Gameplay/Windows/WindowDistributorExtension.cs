using Gameplay.UI.Popup.Window;
using UnityEngine;

namespace Gameplay.Windows
{
	public static class WindowDistributorExtension
	{
		public static bool TryGetWindowWithDebug<TWindow>(
			this IWindowDistributor distributor, WindowId id, out TWindow window)
			where TWindow : class, IWindow
		{
			if (distributor.TryGetWindow(id, out window) == false)
			{
				Debug.LogError($"Cannot to get \"{nameof(id)}\" window.");
				return false;
			}

			return true;
		}

		public static bool TryGetPopupWindowWithDebug(this IWindowDistributor distributor,
			out IPopupWindow popupWindow)
		{
			if (distributor.TryGetWindowWithDebug(WindowId.Popup, out popupWindow))
				return true;

			return false;
		}
	}
}