using Gameplay.Feature.Bomb.StaticData;
using Gameplay.Feature.Bonus.StaticData;
using Gameplay.SaveLoad;
using Gameplay.StaticData.LevelData;
using Zenject;

namespace Infrastructure.GameStatus.State
{
	public sealed class LoadProject : State, IState
	{
		[Inject] IBombDataService _bombData;
		[Inject] ISaveLoadService _saveLoadService;

		[Inject] IEnemiesAtLevelsData _enemiesAtLevelsData;
		
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
		}
	}
}