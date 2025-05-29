using Gameplay.Feature.Bonus.Component;
using Gameplay.Feature.Bonus.StaticData;
using Gameplay.Feature.BonusApplication.Component;
using Gameplay.Feature.Life.Component;
using Infrastructure.ECS.Wrapper;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Zenject;

namespace Gameplay.Feature.BonusApplication.System
{
	public sealed class AddLifePointBonusSystem : IEcsRunSystem
	{
		[Inject] IBonusNames _bonusNames;
		[Inject] EntityWrapper _bonus;
		[Inject] EntityWrapper _target;

		readonly EcsFilterInject<
				Inc<BonusComponent, BonusType, BonusApplicationTarget,
					ApplyBonusRequest>>
			_bonusFilter;

		public void Run(IEcsSystems systems)
		{
			foreach (var bonusEntity in _bonusFilter.Value)
			{
				_bonus.SetEntity(bonusEntity);
				var bonusType = _bonus.BonusType();
				if (bonusType != _bonusNames.AddLifePoint)
					continue;

				if (false == Util.TryGetTargetEntity(_bonus, ref _target)
				    || false == _target.Has<LifePoints>())
					continue;

				_target.AppendChangeLifePoints(1);
				_bonus.Destroy();
			}
		}
	}
}