using System.Linq;
using Gameplay.FinishLevel.Condition;
using Gameplay.StaticData.SceneNames;
using Infrastructure.GameStatus;
using Infrastructure.GameStatus.State;
using Zenject;

namespace Gameplay.FinishLevel
{
	public sealed class FinishLevelService : IFinishLevelService
	{
		[Inject] ISceneNameData _sceneNameData;
		[Inject] IGameStateMachine _gameStateMachine;

		[Inject] readonly IGameOverCondition[] _gameOverConditions;
		[Inject] readonly ILevelCompleteCondition[] _levelCompleteCondition;

		public bool GameOver(int observerEntity) =>
			_gameOverConditions.Any(x => x.Check(observerEntity));

		public bool LevelComplete(int observerEntity) =>
			_levelCompleteCondition.All(x => x.Check(observerEntity));

		public void LaunchGamePause()
		{
			_gameStateMachine.Enter<GamePause>();
		}

		public void SwitchGameToMainMenu()
		{
			_gameStateMachine.EnterToLoadScene(_sceneNameData.MainMenu);
		}

		public void LaunchNextLevel()
		{
			var gameSceneName = _sceneNameData.Game;
			_gameStateMachine.EnterToLoadScene(gameSceneName);
		}

		public void RestartThisLevel()
		{
			var gameSceneName = _sceneNameData.Game;
			_gameStateMachine.EnterToLoadScene(gameSceneName);
		}
	}
}