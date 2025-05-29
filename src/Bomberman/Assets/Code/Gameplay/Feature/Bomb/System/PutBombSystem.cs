using Common.Component;
using Gameplay.Feature.Bomb.Behaviour;
using Gameplay.Feature.Bomb.Component;
using Gameplay.Feature.Bomb.Factory;
using Gameplay.Feature.Map.MapController;
using Gameplay.PlayersBombCollection;
using Infrastructure.ECS.Wrapper;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Zenject;

namespace Gameplay.Feature.Bomb.System
{
	public sealed class PutBombSystem : IEcsRunSystem
	{
		[Inject] IBombFactory _bombFactory;
		[Inject] EntityWrapper _bombParent;
		[Inject] EntityWrapper _bombCarrier;
		[Inject] IMapController _mapController;
		[Inject] IBombCollectionService _bombCollectionService;

		readonly EcsFilterInject<
				Inc<BombCarrier, BombCollectionComponent, PutBombRequest,
					TransformComponent>>
			_putBombRequestFilter;
		readonly EcsFilterInject<Inc<BombParent, TransformComponent>>
			_bombParentFilter;

		public void Run(IEcsSystems systems)
		{
			foreach (var parentEntity in _bombParentFilter.Value)
			foreach (var requestEntity in _putBombRequestFilter.Value)
			{
				_bombParent.SetEntity(parentEntity);
				_bombCarrier.SetEntity(requestEntity);

				var bombType = _bombCarrier.PutBombRequest();
				var bombCollection = _bombCarrier.BombCollectionComponent();

				if (CanPutBomb(bombCollection, bombType) == false)
					continue;

				bombCollection.DecrementBomb(bombType);

				SpawnBomb(bombType, _bombCarrier, _bombParent);
			}
		}

		void SpawnBomb(BombType bombType,
			EntityWrapper bombCarrier, EntityWrapper bombParent)
		{
			var bombCarrierPos = bombCarrier.TransformPos();
			var parent = bombParent.Transform();
			var cell = _mapController.WorldToCell(bombCarrierPos);
			_bombFactory.CreateBomb(bombType, cell, parent);
		}

		bool CanPutBomb(IBombCollection bombCollection, BombType bombType) =>
			bombCollection.TryGetCount(bombType, out var count)
			&& count > 0;

		BombType DefaultBombType() =>
			_bombCollectionService.DefaultBombType();
	}
}