﻿using Gameplay.AI.Navigation;
using Gameplay.Feature.Audio.Behaviour;
using Gameplay.Feature.Bomb.Factory;
using Gameplay.Feature.Bonus.Factory;
using Gameplay.Feature.Camera.Factory;
using Gameplay.Feature.Enemy.AI;
using Gameplay.Feature.Enemy.Base.Factory;
using Gameplay.Feature.Enemy.Base.System;
using Gameplay.Feature.FeatureControl;
using Gameplay.Feature.FinishLevel.Factory;
using Gameplay.Feature.Hero.Factory;
using Gameplay.Feature.HUD.Factory;
using Gameplay.Feature.Map.MapController;
using Gameplay.Feature.MapGenerator.Services;
using Gameplay.Feature.MapGenerator.Services.SubGenerator;
using Gameplay.Feature.PlayerProgress.Factory;
using Gameplay.FinishLevel;
using Gameplay.FinishLevel.Condition.GameOver;
using Gameplay.FinishLevel.Condition.LevelComplete;
using Gameplay.LevelData;
using Gameplay.MapView;
using Gameplay.UI.StaticData;
using Infrastructure.Boot;
using Infrastructure.ECS;
using Infrastructure.ECS.Wrapper.Factory;
using Infrastructure.GameStatus;
using Infrastructure.GameStatus.State;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Infrastructure.Installer
{
	public sealed class LevelInstaller : MonoInstaller, IInitializable
	{
		[SerializeField] Tilemap _groundTailMap;
		[SerializeField] Tilemap _destructibleTailMap;
		[SerializeField] Tilemap _indestructibleTailMap;
		[SerializeField] WindowKitId _windowKitId = WindowKitId.Game;
		[SerializeField] NavMeshSurfaceIdDictionary _navMeshSurfaces;

		[Inject] IGameLevelData _levelData;
		[Inject] IGameStateMachine _gameStateMachine;

		public override void InstallBindings()
		{
			BindInitializable();
			BindMapView();
			BindFactories();
			BindWindowKitId();
			BindAIFunctional();
			BindMapGenerator();
			BindMapController();
			BindDevSceneRunner();
			BindNavigationSurface();
			BindFeatureController();
			BindFinishLevelService();
		}

		public void Initialize()
		{
			InitLevelData();
			Util.ResolveDiContainerDependencies(Container);
			_gameStateMachine.Enter<LaunchGame>();
		}

		void InitLevelData()
		{
			_levelData.World = Container.Resolve<EcsWorld>();
			_levelData.EcsRunner = Container.Resolve<IEcsRunner>();
			_levelData.MapController = Container.Resolve<IMapController>();
			_levelData.DevSceneRunner = Container.Resolve<IDevSceneRunner>();
			_levelData.AudioSourcePool =
				Container.Resolve<PooledAudioSource.Pool>();
		}

		void BindWindowKitId()
		{
			Container.BindInstance(_windowKitId).AsSingle();
		}

		void BindFinishLevelService()
		{
			Container.Bind<IFinishLevelService>().To<FinishLevelService>().AsSingle();
			Container.BindInterfacesTo<KillAllEnemies>().AsSingle();
			Container.BindInterfacesTo<GameTimerCondition>().AsSingle();
			Container.BindInterfacesTo<HeroHealthCondition>().AsSingle();
			Container.BindInterfacesTo<HeroEnteredIntoFinishLevelDoor>().AsSingle();
		}

		void BindMapController()
		{
			Container.Bind<IMapController>().To<StandardMapController>().AsSingle();
		}

		void BindNavigationSurface()
		{
			Container.Bind<NavMeshSurfaceIdDictionary>()
				.FromInstance(_navMeshSurfaces).AsSingle();
			Container.Bind<INavigationSurface>().To<AINavigationSurface>().AsSingle();
		}

		void BindMapView()
		{
			Container.Bind<IMapView>().FromMethod(CreateMapView).AsSingle();
		}

		void BindAIFunctional()
		{
			Container.Bind<Patrolling>().AsTransient();
			Container.Bind<FindPatrolPoints>().AsSingle();
			Container.Bind<FindPatrolVolatilePoints>().AsSingle();
		}

		void BindDevSceneRunner()
		{
			Container.Bind<IDevSceneRunner>().FromComponentInHierarchy().AsSingle();
		}

		void BindFactories()
		{
			Container.Bind<IHudFactory>().To<HudFactory>().AsSingle();
			Container.Bind<IHeroFactory>().To<HeroFactory>().AsSingle();
			Container.Bind<IBombFactory>().To<BombFactory>().AsSingle();
			Container.Bind<IBonusFactory>().To<BonusFactory>().AsSingle();
			Container.Bind<ICameraFactory>().To<CameraFactory>().AsSingle();
			Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
			Container.Bind<IFinishLevelFactory>().To<FinishLevelFactory>().AsSingle();
			Container.Bind<IEntityWrapperFactory>().To<EntityWrapperFactory>()
				.AsSingle();
			Container.Bind<IPlayerProgressFactory>().To<PlayerProgressFactory>()
				.AsSingle();
		}

		void BindMapGenerator()
		{
			Container.Bind<BonusGenerator>().AsSingle()
				.WhenInjectedInto<StandardMapGenerator>();
			Container.Bind<IMapGenerator>().To<StandardMapGenerator>().AsSingle();
		}

		void BindFeatureController()
		{
			Container.Bind<IFeatureController>().To<GameFeatureController>()
				.AsSingle();
		}

		void BindInitializable()
		{
			Container.BindInterfacesTo<LevelInstaller>().FromInstance(this)
				.AsSingle();
		}

		IMapView CreateMapView()
		{
			var gameTileMap = Container.Instantiate<MapView>(
				new[] { _groundTailMap, _destructibleTailMap, _indestructibleTailMap });
			return gameTileMap;
		}
	}
}