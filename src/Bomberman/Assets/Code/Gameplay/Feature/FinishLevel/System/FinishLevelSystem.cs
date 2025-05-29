﻿using Gameplay.Feature.FinishLevel.Component;
using Gameplay.FinishLevel;
using Gameplay.Windows;
using Infrastructure.ECS;
using Infrastructure.ECS.Wrapper;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Zenject;

namespace Gameplay.Feature.FinishLevel.System
{
	public sealed class FinishLevelSystem : IEcsRunSystem
	{
		[Inject] EcsWorld _world;
		[Inject] EntityWrapper _observer;
		[Inject] IWindowService _windowService;
		[Inject] IFinishLevelService _finishLevelService;

		readonly EcsFilterInject<
			Inc<FinishLevelObserver>, Exc<LevelFinished>> _finishLevelObserver;

		public void Run(IEcsSystems systems)
		{
			foreach (var observerEntity in _finishLevelObserver.Value)
			{
				if (ProcessGameOver(observerEntity))
					continue;
				if (ProcessLevelComplete(observerEntity))
					continue;
			}
		}

		bool ProcessGameOver(int observerEntity)
		{
			if (_finishLevelService.GameOver(observerEntity) == false)
				return false;

			_observer.SetEntity(observerEntity);
			_observer
				.Add<LevelFinished>()
				.Add<GameOver>()
				.Add<LevelFinishedProcessor>()
				;
			return true;
		}

		bool ProcessLevelComplete(int observerEntity)
		{
			if (_finishLevelService.LevelComplete(observerEntity) == false)
				return false;

			_observer.SetEntity(observerEntity);
			_observer
				.Add<LevelFinished>()
				.Add<LevelComplete>()
				.Add<LevelFinishedProcessor>()
				;
			return true;
		}
	}
}