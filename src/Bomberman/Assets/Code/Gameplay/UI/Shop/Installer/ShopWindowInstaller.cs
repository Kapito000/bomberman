using System.Collections.Generic;
using Gameplay.UI.Shop.ShopContent;
using Gameplay.UI.Shop.Window;
using Gameplay.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.UI.Shop.Installer
{
	public sealed class ShopWindowInstaller : MonoInstaller
	{
		[SerializeField] ShopWindow _shopWindow;
		[SerializeField] ShopContentListDict shopContentListDict;

		[Inject] IWindowService _windowService;

		public override void InstallBindings()
		{
			BindShopWindow();
			BindShopContentListDict();
		}

		void BindShopWindow()
		{
			Container.BindInterfacesTo<ShopWindow>().FromInstance(_shopWindow)
				.AsSingle();
		}

		void BindShopContentListDict()
		{
			Container
				.Bind<IReadOnlyDictionary<ShopContentMode, ShopContentViewsList>>()
				.FromInstance(shopContentListDict).AsSingle();
		}
	}
}