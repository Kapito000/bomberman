using Common.Component;
using Gameplay.Feature.BonusApplication.Component;
using Gameplay.Feature.Hero.StaticData;
using Infrastructure.ECS.Wrapper;
using Infrastructure.TimeService;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Zenject;

namespace Gameplay.Feature.BonusApplication.System
{
	public sealed class IncreaseMovementSpeedBonusTimerSystem : IEcsRunSystem
	{
		[Inject] IHeroData _heroData;
		[Inject] ITimeService _timeService;
		[Inject] EntityWrapper _bonusCarrier;

		readonly EcsFilterInject<
				Inc<IncreaseMovementSpeedBonusTimer, MoveSpeed>>
			_bonusCarrierFilter;

		public void Run(IEcsSystems systems)
		{
			foreach (var carrierEntity in _bonusCarrierFilter.Value)
			{
				_bonusCarrier.SetEntity(carrierEntity);
				var timerEndMoment = _bonusCarrier.IncreaseMovementSpeedBonusTimer();

				if (_timeService.GameTime() < timerEndMoment)
					continue;

				_bonusCarrier.Remove<IncreaseMovementSpeedBonusTimer>();

				_bonusCarrier.SetMoveSpeed(_heroData.MovementSpeed);
			}
		}
	}
}