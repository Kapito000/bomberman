using System;
using System.Collections.Generic;
using System.Linq;
using Common.Component;
using Gameplay.Feature.Bomb;
using Gameplay.Feature.Bonus.Component;
using Gameplay.Feature.Bonus.StaticData;
using Gameplay.Progress;
using Infrastructure.ECS.Wrapper;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Zenject;

namespace Gameplay.Feature.MapGenerator.System
{
	public sealed class AdjustBombBonusesSystem : IEcsRunSystem
	{
		[Inject] IBonusNames _bonusNames;
		[Inject] EntityWrapper _bonus;
		[Inject] IProgressService _progress;

		readonly EcsFilterInject<Inc<BonusComponent, BonusType, FirstBreath>>
			_bonusFilterInject;

		public void Run(IEcsSystems systems)
		{
			if (TryGetBombDataList(out var bombBonusesDataList) == false ||
			    bombBonusesDataList.Count == 0)
				return;

			foreach (var bonusEntity in _bonusFilterInject.Value)
			{
				_bonus.SetEntity(bonusEntity);
				var bonusType = _bonus.BonusType();

				var bombType = bombBonusesDataList[0];
				bombBonusesDataList.RemoveAt(0);
				_bonus.AddBombBonusType(bombType);
			}
		}

		bool TryGetBombDataList(out List<BombType> list)
		{
			
			list = Enum.GetValues(typeof(BombType)).Cast<BombType>().ToList();
			return true;
		}
	}
}