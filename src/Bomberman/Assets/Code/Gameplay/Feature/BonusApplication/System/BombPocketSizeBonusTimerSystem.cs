using Gameplay.Feature.Bomb.Component;
using Gameplay.Feature.BonusApplication.Component;
using Gameplay.PlayersBombCollection;
using Infrastructure.ECS.Wrapper;
using Infrastructure.TimeService;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Zenject;

namespace Gameplay.Feature.BonusApplication.System
{
	public sealed class BombPocketSizeBonusTimerSystem : IEcsRunSystem
	{
		[Inject] ITimeService _timeService;
		[Inject] EntityWrapper _bonusCarrier;
		[Inject] IBombCollectionService _bombCollectionService;

		readonly EcsFilterInject<
				Inc<BombStackSize, BombCollectionComponent, BombPocketSizeBonusTimer>>
			_bonusCarrierFilter;

		public void Run(IEcsSystems systems)
		{
			foreach (var carrierEntity in _bonusCarrierFilter.Value)
			{
				_bonusCarrier.SetEntity(carrierEntity);
				var timerEndMoment = _bonusCarrier.BombPocketSizeBonusTimer();

				if (_timeService.GameTime() < timerEndMoment)
					continue;

				var size = _bombCollectionService.DefaultBombPocketSize();
				_bonusCarrier.SetBombStackSize(size);

				_bonusCarrier.Remove<BombPocketSizeBonusTimer>();

				var bombCollection = _bonusCarrier.BombCollectionComponent();
				var bombType = bombCollection.DefaultBombType();
				bombCollection.SetBombCount(bombType, size);
			}
		}
	}
}