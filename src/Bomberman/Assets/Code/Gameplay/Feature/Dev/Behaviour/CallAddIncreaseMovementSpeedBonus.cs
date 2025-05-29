#if UNITY_EDITOR
using Common.Component;
using Gameplay.Feature.Bonus.Component;
using Gameplay.Feature.Bonus.StaticData;
using Gameplay.Feature.BonusApplication.Component;
using Gameplay.Feature.Hero.Component;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using Zenject;

namespace Gameplay.Feature.Dev.Behaviour
{
	public class CallAddIncreaseMovementSpeedBonus : CreateNewEntity
	{
		[Inject] EcsWorld _world;
		[Inject] IBonusNames _bonusNames;

		[Button]
		public void Create()
		{
			var targetFilter = _world
				.Filter<HeroComponent>()
				.Inc<MoveSpeed>()
				.End();
			
			foreach (var targetEntity in targetFilter)
			{
				var targetPack = _world.PackEntityWithWorld(targetEntity);

				var wrapper = CreateDevEntity();
				wrapper
					.Add<BonusComponent>()
					.AddBonusType(_bonusNames.IncreaseSpeed)
					.Add<ApplyBonusRequest>()
					.AddBonusApplicationTarget(targetPack)
					;
			}
		}
	}
}
#endif