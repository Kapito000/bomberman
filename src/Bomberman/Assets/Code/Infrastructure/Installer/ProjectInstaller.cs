﻿using Gameplay.Audio.ClipProvider;
using Gameplay.Audio.Factory;
using Gameplay.Audio.MixerGroupProvider;
using Gameplay.Audio.Player;
using Gameplay.Audio.Service;
using Gameplay.Collisions;
using Gameplay.Difficult;
using Gameplay.Feature.Audio.Behaviour;
using Gameplay.Feature.Bomb.StaticData;
using Gameplay.Feature.Bonus.Service;
using Gameplay.Feature.Bonus.StaticData;
using Gameplay.Feature.Enemy.Base.StaticData;
using Gameplay.Feature.GameMusic.Factory;
using Gameplay.Feature.Hero.StaticData;
using Gameplay.Feature.MapGenerator.StaticData;
using Gameplay.Feature.Timer.StaticData;
using Gameplay.Input;
using Gameplay.Input.Character;
using Gameplay.LevelData;
using Gameplay.MapTile.TileProvider;
using Gameplay.Physics;
using Gameplay.Progress;
using Gameplay.Reskin;
using Gameplay.Reskin.StaticData;
using Gameplay.SaveLoad;
using Gameplay.StaticData.LevelData;
using Gameplay.StaticData.SceneNames;
using Gameplay.UI.Factory;
using Gameplay.UI.StaticData;
using Gameplay.Windows;
using Gameplay.Windows.Factory;
using Infrastructure.AssetProvider;
using Infrastructure.AssetProvider.SpriteLibraries;
using Infrastructure.ECS;
using Infrastructure.ECS.Wrapper;
using Infrastructure.Factory.EntityBehaviourFactory;
using Infrastructure.Factory.Kit;
using Infrastructure.Factory.SystemFactory;
using Infrastructure.GameStatus;
using Infrastructure.GameStatus.State;
using Infrastructure.InstantiateService;
using Infrastructure.SceneLoader;
using Infrastructure.TimeService;
using Leopotam.EcsLite;
using MetaSystem.GameSettings;
using MetaSystem.GameSettings.Audio;
using MetaSystem.GameSettings.StaticData;
using MetaSystem.GameSettings.StaticData.Audio;
using UnityEngine;
using Zenject;
using AudioSettings = MetaSystem.GameSettings.Audio.AudioSettings;

namespace Infrastructure.Installer
{
	public class ProjectInstaller : MonoInstaller
	{
		[SerializeField] AIData _aiData;
		[SerializeField] MapData _mapData;
		[SerializeField] HeroData _heroData;
		[SerializeField] EnemyList _enemyList;
		[SerializeField] BonusNames _bonusNames;
		[SerializeField] SkinLibrary _skinLibrary;
		[SerializeField] BonusSprites _bonusSprites;
		[SerializeField] GameTimerData _gameTimerData;
		[SerializeField] WindowKitData _windowKitData;
		[SerializeField] SceneNamesData _sceneNamesData;
		[SerializeField] TileCollection _tileCollection;
		[SerializeField] AudioClipProvider _audioClipProvider;
		[SerializeField] DirectLinkProvider _assetProvider;
		[SerializeField] AudioMixerProvider _audioMixerProvider;
		[SerializeField] SpriteLibraryProvider _spriteLibraryProvider;
		[SerializeField] IncreaseSpeedBonusData _increaseSpeedBonusData;
		[SerializeField] PocketResizeBonusSettings _pocketResizeBonusSettings;

		public override void InstallBindings()
		{
			BindWorld();
			BindPools();
			BindEcsRunner();
			BindLevelData();
			BindFactories();
			BindStaticData();
			BindFactoryKit();
			BindTimeService();
			BindSceneLoader();
			BindUIFactories();
			BindAudioService();
			BindInputService();
			BindMusicFactory();
			BindReskinService();
			BindGameTimerData();
			BindWindowService();
			BindSystemFactory();
			BindEntityWrapper();
			BindAssetProvider();
			BindPhysicsService();
			BindSaveLoadService();
			BindProgressService();
			BindDifficultService();
			BindGameStateMachine();
			BindAudioClipProvider();
			BindCollisionRegistry();
			BindAudioMixerProvider();
			BindInstantiateService();
			BindGameSettingsService();
			BindEntityBehaviourFactory();
			BindIncreaseSpeedBonusModificator();
		}

		void BindIncreaseSpeedBonusModificator()
		{
			Container.Bind<IIncreaseSpeedBonusModificator>()
				.To<IncreaseSpeedBonusModificator>().AsSingle();
		}

		void BindSaveLoadService()
		{
			Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
		}

		void BindReskinService()
		{
			Container.Bind<IReskinService>().To<ReskinService>().AsSingle();
		}

		void BindDifficultService()
		{
			Container.Bind<IDifficultService>().To<DifficultService>().AsSingle();
		}

		void BindProgressService()
		{
			Container.BindInterfacesTo<ProgressService>().AsSingle();
		}

		void BindGameSettingsService()
		{
			Container.Bind<IGameSettings>().To<GameSettingsService>().AsSingle();
			Container.Bind<IAudioSetting>().To<AudioSettings>().AsSingle()
				.WhenInjectedInto<IGameSettings>();
		}

		void BindPools()
		{
			BindAudioSourcePool();
		}

		void BindAudioSourcePool()
		{
			Container.BindMemoryPool<PooledAudioSource, PooledAudioSource.Pool>()
				.FromNewComponentOnNewGameObject()
				.WithGameObjectName(Constant.ObjectName.c_PooledAudioSource)
				.UnderTransformGroup(Constant.ObjectName.c_PooledAudioSourcesParent)
				.AsSingle()
				.MoveIntoDirectSubContainers();
		}

		void BindFactories()
		{
			Container.Bind<IGameMusicFactory>().To<GameMusicFactory>()
				.AsSingle()
				.MoveIntoDirectSubContainers();
		}

		void BindMusicFactory()
		{
			Container.Bind<IMusicFactory>().To<MusicFactory>()
				.AsSingle()
				.MoveIntoDirectSubContainers();
		}

		void BindWorld()
		{
			Container.Bind<EcsWorld>().FromMethod(CreateNewWorld).AsSingle()
				.MoveIntoDirectSubContainers();

			EcsWorld CreateNewWorld() => new();
		}

		void BindEcsRunner()
		{
			Container.Bind<IEcsRunner>().To<EcsRunner>()
				.FromNewComponentOnNewGameObject()
				.WithGameObjectName(Constant.ObjectName.c_EcsRunnerName)
				.UnderTransformGroup(Constant.ObjectName.c_Services)
				.AsSingle()
				.OnInstantiated<EcsRunner>((c, runner) => runner.enabled = false)
				.MoveIntoDirectSubContainers();
		}

		void BindSystemFactory()
		{
			Container.Bind<ISystemFactory>().To<StandardSystemFactory>()
				.AsSingle()
				.MoveIntoDirectSubContainers();
		}

		void BindAudioClipProvider()
		{
			Container.Bind<IAudioClipProvider>().FromInstance(_audioClipProvider)
				.AsSingle();
		}

		void BindAudioMixerProvider()
		{
			Container.Bind<IAudioMixerProvider>().FromInstance(_audioMixerProvider)
				.AsSingle();
		}

		void BindAudioService()
		{
			Container.Bind<IAudioService>().To<AudioService>().AsSingle();
			Container.Bind<IAudioPlayer>().To<AudioPlayer>()
				.AsSingle()
				.WhenInjectedInto<IAudioService>();
		}

		void BindGameTimerData()
		{
			Container.Bind<IGameTimerData>().FromInstance(_gameTimerData).AsSingle();
		}

		void BindEntityBehaviourFactory()
		{
			Container.Bind<IEntityBehaviourFactory>().To<EntityBehaviourFactory>()
				.AsSingle()
				.MoveIntoDirectSubContainers();
		}

		void BindCollisionRegistry()
		{
			Container.Bind<ICollisionRegistry>().To<CollisionRegistry>().AsSingle();
		}

		void BindPhysicsService()
		{
			Container.Bind<IPhysicsService>().To<PhysicsService>().AsSingle();
		}

		void BindTimeService()
		{
			Container.Bind<ITimeService>().To<StandardTimeService>().AsSingle();
		}

		void BindWindowService()
		{
			Container.BindInterfacesTo<WindowService>().AsSingle()
				.MoveIntoDirectSubContainers();
		}

		void BindUIFactories()
		{
			Container.Bind<IUiFactory>().To<UiFactory>().AsSingle()
				.MoveIntoDirectSubContainers();
			Container.Bind<IWindowFactory>().To<WindowFactory>().AsSingle()
				.MoveIntoDirectSubContainers();
		}

		void BindEntityWrapper()
		{
			Container.Bind<EntityWrapper>().AsTransient()
				.MoveIntoDirectSubContainers();
		}

		void BindInputService()
		{
			Container.Bind<Controls>().AsSingle();
			Container.Bind<IInput>().To<CommonInput>().AsSingle();
			Container
				.Bind(typeof(ICharacterInput), typeof(ICharacterInputActionProvider))
				.To<CharacterInputService>().AsSingle();
		}

		void BindAssetProvider()
		{
			Container.Bind<IAssetProvider>().FromInstance(_assetProvider).AsSingle();
		}

		void BindLevelData()
		{
			Container.BindInterfacesTo<StandardLevelData>().AsSingle();
		}

		void BindSceneLoader()
		{
			Container.Bind<ISceneLoader>().To<StandardLoader>().AsSingle();
		}

		void BindStaticData()
		{
			Container.Bind<IHeroData>().FromInstance(_heroData).AsSingle();
			Container.Bind<IBonusNames>().FromInstance(_bonusNames).AsSingle();
			Container.Bind<IBonusSprites>().FromInstance(_bonusSprites).AsSingle();
			Container.Bind<ISceneNameData>().FromInstance(_sceneNamesData)
				.AsSingle();
			Container.Bind<IAIData>().FromInstance(_aiData).AsSingle();
			Container.Bind<IMapData>().FromInstance(_mapData).AsSingle();
			Container.Bind<IEnemyList>().FromInstance(_enemyList).AsSingle();
			Container.Bind<ITileProvider>().FromInstance(_tileCollection).AsSingle();
			Container.Bind<IEnemiesAtLevelsData>().To<EnemiesAtEnemiesAtLevelsData>()
				.AsSingle();
			Container.Bind<IWindowKitData>().FromInstance(_windowKitData).AsSingle();
			Container.Bind<IBombDataService>().To<BombDataService>().AsSingle();
			Container.Bind<ISpriteLibraryProvider>()
				.FromInstance(_spriteLibraryProvider).AsSingle();
			Container.Bind<IIncreaseSpeedBonusData>()
				.FromInstance(_increaseSpeedBonusData).AsSingle();
			Container.Bind<IPocketResizeBonusSettings>()
				.FromInstance(_pocketResizeBonusSettings).AsSingle();
			Container.Bind<IGameSettingsStartValueData>().To<GameSettingsStartValue>()
				.AsSingle();
			Container.Bind<IAudioStartValueData>().To<AudioStartValueData>()
				.AsSingle().WhenInjectedInto<IGameSettingsStartValueData>();
			Container.Bind<ISkinLibrary>().FromInstance(_skinLibrary).AsSingle();
			Container.Bind<IBonusesForLevel>().To<BonusesForLevel>().AsSingle();
		}

		void BindGameStateMachine()
		{
			Container.Bind<Game>().FromNew().AsSingle();

			Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
			Container.Bind<IState>().To<LoadProject>().AsSingle();
			Container.Bind<IState>().To<LoadScene>().AsSingle();
			Container.Bind<IState>().To<MainMenu>().AsSingle();
			Container.Bind<IState>().To<LaunchGame>().AsSingle();
			Container.Bind<IState>().To<GameLoop>().AsSingle();
			Container.Bind<IState>().To<GamePause>().AsSingle();
			Container.Bind<IState>().To<GameExit>().AsSingle();
		}

		void BindInstantiateService()
		{
			Container
				.Bind(typeof(IInstantiateService), typeof(IDiContainerDependence))
				.To<StandardInstantiateService>()
				.AsSingle()
				.MoveIntoDirectSubContainers();
		}

		void BindFactoryKit()
		{
			Container.Bind<IFactoryKit>().To<FactoryKit>()
				.AsSingle()
				.MoveIntoDirectSubContainers();
		}
	}
}