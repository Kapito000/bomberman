using Common.Component;
using Extensions;
using Gameplay.Feature.Bonus.Component;
using Gameplay.Feature.Bonus.Service;
using Gameplay.Feature.Bonus.StaticData;
using Gameplay.Feature.BonusApplication.Component;
using Gameplay.Feature.Hero.Component;
using Infrastructure.ECS.Wrapper;
using Infrastructure.ECS.Wrapper.Factory;
using Infrastructure.TimeService;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Zenject;

namespace Gameplay.Feature.BonusApplication.System
{
	public sealed class AddIncreaseMovementSpeedBonusSystem : IEcsRunSystem
	{
		[Inject] IBonusNames _bonusNames;
		[Inject] ITimeService _timeService;
		[Inject] EntityWrapper _bonus;
		[Inject] IEntityWrapperFactory _entityWrapperFactory;
		[Inject] IIncreaseSpeedBonusData _increaseSpeedBonusData;
		[Inject] IIncreaseSpeedBonusModificator _increaseSpeedBonusModificator;

		readonly EcsFilterInject<
				Inc<BonusComponent, BonusType, BonusApplicationTarget,
					ApplyBonusRequest>,
				Exc<IncreaseMovementSpeedBonusTimer>>
			_bonusFilter;

		public void Run(IEcsSystems systems)
		{
			foreach (var bonusEntity in _bonusFilter.Value)
			{
				_bonus.SetEntity(bonusEntity);

				if (CanApplyBonus(_bonus, out var target) == false)
					continue;

				var increasedSpeed = _increaseSpeedBonusModificator.IncreasedSpeed();
				target.SetMoveSpeed(increasedSpeed);
				var timer = _increaseSpeedBonusData.IncreaseSpeedTimer;
				var endTimerMoment = _timeService.GameTime() + timer;
				target.ReplaceIncreaseMovementSpeedBonusTimer(endTimerMoment);

				_bonus.Destroy();
			}
		}

		bool CanApplyBonus(EntityWrapper bonus, out EntityWrapper target)
		{
			var bonusType = bonus.BonusType();
			if (bonusType != _bonusNames.IncreaseSpeed)
			{
				target = default;
				return false;
			}

			if (false == TryGetTargetEntity(_bonus, out target)
			    || false == target.Has<HeroComponent, MoveSpeed>())
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