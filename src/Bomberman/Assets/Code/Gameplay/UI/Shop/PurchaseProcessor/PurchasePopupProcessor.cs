using Gameplay.UI.Popup;
using Gameplay.UI.Popup.Window;
using Gameplay.Windows;
using MetaSystem.Shop;
using UnityEngine;
using Zenject;

namespace Gameplay.UI.Shop.PurchaseProcessor
{
	public sealed class PurchasePopupProcessor : IPurchasePopupProcessor
	{
		[Inject] IShopService _shopService;
		[Inject] IWindowService _windowService;
		[Inject] IWindowDistributor _windowDistributor;

		const WindowId c_windowId = WindowId.Popup;

		public void Init()
		{
			Subscribe();
		}

		public void Dispose()
		{
			Unsubscribe();
		}

		void Subscribe()
		{
			_shopService.PurchaseStarting += OnPurchaseStarting;
			_shopService.PurchaseEnded += OnPurchaseEnded;
		}

		void Unsubscribe()
		{
			_shopService.PurchaseStarting -= OnPurchaseStarting;
			_shopService.PurchaseEnded -= OnPurchaseEnded;
		}

		void OnPurchaseStarting()
		{
			if (false == _windowDistributor
				    .TryGetWindow(c_windowId, out IPopupWindow popup))
			{
				Debug.LogWarning("Cannot to get popup window.");
				return;
			}

			popup.Refresh(RefreshData.ProcessPurchaseData());
			_windowService.Open(c_windowId);
		}

		void OnPurchaseEnded()
		{
			_windowService.Close(c_windowId);
		}
	}
}