using Gameplay.Feature.FeatureControl;
using Gameplay.Feature.MainMenu.Factory;
using Gameplay.LevelData;
using Gameplay.UI.Shop.Factory;
using Gameplay.UI.Shop.PurchaseProcessor;
using Gameplay.UI.StaticData;
using Infrastructure.Boot;
using Infrastructure.ECS;
using Infrastructure.GameStatus;
using Infrastructure.GameStatus.State;
using MetaSystem.MainMenu;
using MetaSystem.Shop;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installer
{
	public sealed class MainMenuInstaller : MonoInstaller, IInitializable
	{
		[SerializeField] WindowKitId _windowKitId = WindowKitId.MainMenu;

		[Inject] ILevelData _levelData;
		[Inject] IGameStateMachine _gameStateMachine;

		public override void InstallBindings()
		{
			BindInitializable();
			BindFactories();
			BindUIServices();
			BindWindowKitId();
			BindDevSceneRunner();
			BindFeatureController();
			BindShoppingCartService();
		}

		public void Initialize()
		{
			InitLevelData();
			Util.ResolveDiContainerDependencies(Container);
			_gameStateMachine.Enter<MainMenu>();
		}

		void InitLevelData()
		{
			_levelData.EcsRunner = Container.Resolve<IEcsRunner>();
			_levelData.DevSceneRunner = Container.Resolve<IDevSceneRunner>();
			_levelData.PurchasePopupProcessor = Container
				.Resolve<IPurchasePopupProcessor>();
		}

		void BindShoppingCartService()
		{
			Container.Bind<IShoppingCartService>().To<ShoppingCartService>()
				.AsSingle();
		}

		void BindWindowKitId()
		{
			Container.BindInstance(_windowKitId).AsSingle();
		}

		void BindFactories()
		{
			Container.Bind<IShopUiFactory>().To<ShopUiFactory>().AsSingle();
			Container.Bind<IMainMenuFactory>().To<MainMenuFactory>().AsSingle();
		}

		void BindFeatureController()
		{
			Container.Bind<IFeatureController>().To<MainMenuFeatureController>()
				.AsSingle();
		}

		void BindDevSceneRunner()
		{
			Container.Bind<IDevSceneRunner>().FromComponentInHierarchy().AsSingle();
		}

		void BindUIServices()
		{
			Container.Bind<IMainMenuService>().To<MainMenuService>().AsSingle();
		}

		void BindInitializable()
		{
			Container.Bind<IInitializable>().FromInstance(this).AsSingle();
		}
	}
}