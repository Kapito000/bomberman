using Gameplay.Feature.Bomb.Component;
using Gameplay.PlayersBombCollection;
using Infrastructure.ECS;
using Infrastructure.ECS.Wrapper;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Zenject;

namespace Gameplay.Feature.Bomb.System
{
	public sealed class BombReplenishmentSystem : IEcsRunSystem
	{
		[Inject] EntityWrapper _bombCarrier;
		[Inject] IBombCollectionService _bombCollectionService;

		readonly EcsFilterInject<
				Inc<BombCarrier, BombCollectionComponent, BombStackSize>>
			_bombCarrierFilter;

		readonly EcsFilterInject<Inc<BombComponent, BombExplosion>> _bombFilter;

		public void Run(IEcsSystems systems)
		{
			foreach (var bombEntity in _bombFilter.Value)
			foreach (var carrierEntity in _bombCarrierFilter.Value)
			{
				_bombCarrier.SetEntity(carrierEntity);
				var bombCollection = _bombCarrier.BombCollectionComponent();
				var defaultBombCount = bombCollection.DefaultBombCount();
				var bombStackSize = _bombCarrier.BombStackSize();

				if (defaultBombCount >= bombStackSize)
					continue;

				bombCollection.AddDefaultBomb();
			}
		}
	}
}