#if UNITY_EDITOR
using Gameplay.Feature.Bomb.Component;
using Gameplay.Feature.Bonus.Component;
using Gameplay.Feature.Bonus.StaticData;
using Gameplay.Feature.BonusApplication.Component;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using Zenject;

namespace Gameplay.Feature.Dev.Behaviour
{
	public class CallAddExtendBombPocketBonus : CreateNewEntity
	{
		[Inject] EcsWorld _world;
		[Inject] IBonusNames _bonusNames;

		[Button]
		public void Create()
		{
			var targetFilter = _world
				.Filter<BombCollectionComponent>()
				.Inc<BombStackSize>()
				.End();
			
			foreach (var targetEntity in targetFilter)
			{
				var targetPack = _world.PackEntityWithWorld(targetEntity);

				var wrapper = CreateDevEntity();
				wrapper
					.Add<BonusComponent>()
					.AddBonusType(_bonusNames.ExtendBombPocket)
					.Add<ApplyBonusRequest>()
					.AddBonusApplicationTarget(targetPack)
					;
			}
		}
	}
}
#endif