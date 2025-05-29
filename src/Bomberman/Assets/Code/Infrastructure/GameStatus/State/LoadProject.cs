using System;
using Cysharp.Threading.Tasks;
using Gameplay.Feature.Bomb.StaticData;
using Gameplay.Feature.Bonus.StaticData;
using Gameplay.PlayersBombCollection.StaticData;
using Gameplay.SaveLoad;
using Gameplay.StaticData.LevelData;
using RemotePlugin.UserDataService;
using UnityEngine;
using Zenject;
using RemoteServices = RemotePlugin.Remote;

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
			Init().Forget();
		}

		public void Exit()
		{ }

		async UniTaskVoid Init()
		{
			InitStaticData();
			try
			{
				await InitRemotePluginAsync();
			}
			catch (Exception e)
			{
				Debug.LogError($"Cannot to init remote plugin.\n{e.Message}");
			}

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

		async UniTask InitRemotePluginAsync()
		{
			var userDataLoader = new RemoteServices.UserDataLoader();
			var userData = await userDataLoader.LoadUserDataAsync();
			var exceptionHandler = new RemoteServices.ConsoleExceptionHandler(false);
			var userDataService =
				new RemoteUserDataService(userData, exceptionHandler);

			RemoteServices.Services.Init(exceptionHandler, userDataService);
		}
	}
}