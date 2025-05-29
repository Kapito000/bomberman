using Extensions;
using Gameplay.Feature.Bomb.Component;
using Gameplay.Feature.Bonus.Component;
using Gameplay.Feature.Bonus.StaticData;
using Gameplay.Feature.BonusApplication.Component;
using Gameplay.PlayersBombCollection;
using Infrastructure.ECS.Wrapper;
using Infrastructure.ECS.Wrapper.Factory;
using Infrastructure.TimeService;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Zenject;

namespace Gameplay.Feature.BonusApplication.System
{
	public sealed class AddExtendBombPocketBonusSystem : IEcsRunSystem
	{
		[Inject] IBonusNames _bonusNames;
		[Inject] ITimeService _timeService;
		[Inject] EntityWrapper _bonus;
		[Inject] IEntityWrapperFactory _entityWrapperFactory;
		[Inject] IBombCollectionService _bombCollectionService;
		[Inject] IPocketResizeBonusSettings _pocketResizeBonusSettings;

		readonly EcsFilterInject<
				Inc<BonusComponent, BonusType, BonusApplicationTarget,
					ApplyBonusRequest>>
			_bonusFilter;

		public void Run(IEcsSystems systems)
		{
			foreach (var bonusEntity in _bonusFilter.Value)
			{
				_bonus.SetEntity(bonusEntity);

				if (CanApplyBonus(_bonus, out var target) == false)
					continue;

				var size = SetBombPocketSize(target);
				CreateBonusTimer(target);
				AddBombsToCollection(target, size);

				_bonus.Destroy();
			}
		}

		void AddBombsToCollection(EntityWrapper target, int size)
		{
			var bombCollection = target.BombCollectionComponent();
			bombCollection.SetBombCount(bombCollection.DefaultBombType(), size);
		}

		void CreateBonusTimer(EntityWrapper target)
		{
			var endTimerMoment =
				_timeService.GameTime() + _pocketResizeBonusSettings.Timer;
			target.ReplaceBombPocketSizeBonusTimer(endTimerMoment);
		}

		int SetBombPocketSize(EntityWrapper target)
		{
			var size = _bombCollectionService.BombPocketSizeBonusForCurrentLevel();
			target.SetBombStackSize(size);
			return size;
		}

		bool CanApplyBonus(EntityWrapper bonus, out EntityWrapper target)
		{
			var bonusType = bonus.BonusType();
			if (bonusType != _bonusNames.ExtendBombPocket)
			{
				target = default;
				return false;
			}

			if (false == TryGetTargetEntity(_bonus, out target)
			    || false == target.Has<BombStackSize, BombCollectionComponent>())
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