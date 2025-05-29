﻿using Gameplay.Feature.Audio;
using Gameplay.Feature.Bomb;
using Gameplay.Feature.Bonus;
using Gameplay.Feature.BonusApplication;
using Gameplay.Feature.Camera;
using Gameplay.Feature.Collisions;
using Gameplay.Feature.DamageApplication;
using Gameplay.Feature.Destruction;
using Gameplay.Feature.Enemy;
using Gameplay.Feature.FinishLevel;
using Gameplay.Feature.GameMusic;
using Gameplay.Feature.GameUI;
using Gameplay.Feature.Hero;
using Gameplay.Feature.HUD;
using Gameplay.Feature.Input;
using Gameplay.Feature.Life;
using Gameplay.Feature.Map;
using Gameplay.Feature.MapGenerator;
using Gameplay.Feature.PlayerProgress;
using Gameplay.Feature.Timer;
using Gameplay.Navigation;
using Infrastructure.Factory.SystemFactory;

namespace Gameplay.Feature.FeatureControl
{
	public sealed class GameFeatureController : FeatureController
	{
		public GameFeatureController(ISystemFactory systemFactory)
			: base(systemFactory)
		{
			Add<InputFeature>();
			Add<MapGenerationFeature>();
			Add<CollisionsFeature>();
			Add<GameMusicFeature>();

			Add<BonusApplicationFeature>();

			Add<HeroFeature>();
			Add<EnemyFeature>();
			Add<BombFeature>();
			Add<DamageApplicationFeature>();
			Add<LifeFeature>();
			Add<NavigationFeature>();

			Add<SpawnBonusFeature>();

			Add<MapFeature>();
			Add<TimerFeature>();
			Add<AudioFeature>();
			Add<CameraFeature>();
			Add<GameUiFeature>();
			Add<FinishLevelFeature>();
			Add<HudFeature>();
			Add<PlayerProgressFeature>();
			Add<DestructionFeature>();

#if UNITY_EDITOR
			Add<Dev.DevFeature>();
#endif
		}
	}
}