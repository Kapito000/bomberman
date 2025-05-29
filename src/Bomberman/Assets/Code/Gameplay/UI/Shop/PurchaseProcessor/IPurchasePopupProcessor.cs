using System;
using Infrastructure;

namespace Gameplay.UI.Shop.PurchaseProcessor
{
	public interface IPurchasePopupProcessor : IService, IDisposable
	{
		void Init();
	}
}