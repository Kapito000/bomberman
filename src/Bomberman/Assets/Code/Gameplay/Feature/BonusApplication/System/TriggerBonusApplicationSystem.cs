using Extensions;
using Gameplay.Feature.Bonus.Component;
using Gameplay.Feature.Bonus.StaticData;
using Gameplay.Feature.BonusApplication.Component;
using Gameplay.Feature.Collisions;
using Gameplay.Feature.Collisions.Component;
using Gameplay.Feature.Hero.Component;
using Infrastructure.ECS.Wrapper;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Zenject;

namespace Gameplay.Feature.BonusApplication.System
{
	public sealed class TriggerBonusApplicationSystem : IEcsRunSystem
	{
		[Inject] IBonusNames _bonusNames;
		[Inject] EntityWrapper _bonus;
		[Inject] EntityWrapper _other;

		readonly EcsFilterInject<
			Inc<BonusComponent, BonusType, TriggerEnterBuffer>,
			Exc<ApplyBonusRequest, BonusApplicationTarget>> _bonusFilter;

		public void Run(IEcsSystems systems)
		{
			foreach (var bonusEntity in _bonusFilter.Value)
			{
				_bonus.SetEntity(bonusEntity);

				var enterBuffer = _bonus.TriggerEnterBuffer();
				foreach (var otherPackedEntity in enterBuffer)
				{
					if (false == TryGetOtherEntity(otherPackedEntity, out var otherEntity)
					    || false == CanCollectBonus(otherEntity))
						continue;

					_bonus.AddBonusApplicationTarget(otherPackedEntity);
					_bonus.Add<ApplyBonusRequest>();
					break;
				}
			}
		}

		bool CanCollectBonus(int otherEntity)
		{
			_other.SetEntity(otherEntity);
			return _other.Has<HeroComponent>();
		}

		bool TryGetOtherEntity(EcsPackedEntityWithWorld otherPackedEntity,
			out int otherEntity)
		{
			if (otherPackedEntity.Unpack(out otherEntity))
				return true;

			Common.Logger.Error.CannotUnpackEntity();
			return false;
		}
	}
}