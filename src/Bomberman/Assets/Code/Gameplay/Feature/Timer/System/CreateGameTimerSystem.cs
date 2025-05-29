﻿using Gameplay.Feature.Timer.Component;
using Gameplay.Feature.Timer.StaticData;
using Infrastructure.ECS;
using Infrastructure.ECS.Wrapper;
using Leopotam.EcsLite;
using Zenject;

namespace Gameplay.Feature.Timer.System
{
	public sealed class CreateGameTimerSystem : IEcsRunSystem
	{
		[Inject] EntityWrapper _timer;
		[Inject] IGameTimerData _gameTimerData;
		
		public void Run(IEcsSystems systems)
		{
			var timerValue = _gameTimerData.Value;
			_timer.NewEntity()
				.AddGameTimer(timerValue)
				.Add<RunningGameTimer>()
				;
		}
	}
}