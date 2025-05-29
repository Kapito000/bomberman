using Extensions;
using Gameplay.Feature.Bomb.Component;
using Gameplay.Feature.Bonus.Component;
using Gameplay.Feature.Bonus.StaticData;
using Gameplay.Feature.BonusApplication.Component;
using Infrastructure.ECS.Wrapper;
using Infrastructure.ECS.Wrapper.Factory;
using Infrastructure.TimeService;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Zenject;

namespace Gameplay.Feature.BonusApplication.System
{
	public sealed class AddBonusBombSystem : IEcsRunSystem
	{
		[Inject] IBonusNames _bonusNames;
		[Inject] ITimeService _timeService;
		[Inject] EntityWrapper _bonus;
		[Inject] IEntityWrapperFactory _entityWrapperFactory;

		readonly EcsFilterInject<
				Inc<BonusComponent, BonusType, BombBonusType,
					BonusApplicationTarget, ApplyBonusRequest>>
			_bonusFilter;

		public void Run(IEcsSystems systems)
		{
			foreach (var bonusEntity in _bonusFilter.Value)
			{
				_bonus.SetEntity(bonusEntity);

				if (CanApplyBonus(_bonus, out var target) == false)
					continue;

				var bombCollection = target.BombCollectionComponent();
				var bombType = _bonus.BombBonusType();
				bombCollection.AddBomb(bombType);

				_bonus.Destroy();
			}
		}

		bool CanApplyBonus(EntityWrapper bonus, out EntityWrapper target)
		{
			var bonusType = bonus.BonusType();
			if (bonusType != _bonusNames.Bomb)
			{
				target = default;
				return false;
			}

			if (false == TryGetTargetEntity(_bonus, out target)
			    || false == target.Has<BombCollectionComponent>())
				return false;

			return true;
		}

		bool TryGetTargetEntity(EntityWrapper bonus, out EntityWrapper target)
		{
			var targetPack = bonus.BonusApplicationTarget();
			if (targetPack.Unpack(out var targetEntity))
			{
				target = _entityWrapperFactory.CreateWrapper(targetEntity);
				return true;
			}

			target = default;
			Common.Logger.Error.CannotUnpackEntity();
			return false;
		}
	}
}