using Gameplay.Feature.Bomb.StaticData;
using Gameplay.Feature.Bonus.StaticData;
using Gameplay.PlayersBombCollection.StaticData;
using Gameplay.SaveLoad;
using Gameplay.StaticData.LevelData;
using Zenject;

namespace Infrastructure.GameStatus.State
{
	public sealed class LoadProject : State, IState
	{
		[Inject] IBombDataService _bombData;
		[Inject] ISaveLoadService _saveLoadService;

		[Inject] IBonusesForLevel _bonusesForLevel;
		[Inject] IEnemiesAtLevelsData _enemiesAtLevelsData;
		[Inject] IAdditionalBombBonuses _additionalBombBonuses;
		[Inject] IBombPocketBonusForLevels _bombPocketBonusForLevels;

		public LoadProject(IGameStateMachine gameStateMachine) : base(
			gameStateMachine)
		{ }

		public string FirstScene { get; set; }

		public void Enter()
		{
			Init();
		}

		public void Exit()
		{ }

		void Init()
		{
			InitStaticData();

			_saveLoadService.Load();
			_gameStateMachine.EnterToLoadScene(FirstScene);
		}

		void InitStaticData()
		{
			_bombData.Init();
			_enemiesAtLevelsData.Init();
			InitBonusesStaticData();
		}

		void InitBonusesStaticData()
		{
			_bonusesForLevel.Init();
			_additionalBombBonuses.Init();
			_bombPocketBonusForLevels.Init();
		}
	}
}