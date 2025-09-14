using System.Collections.Generic;
using Zenject;

namespace Gameplay.Feature.Bonus.StaticData
{
	public sealed class BonusesForLevel : IBonusesForLevel
	{
		const int _bounsCoutForLevel = 10;

		[Inject] IBonusNames _bonusNames;

		public bool TryGetBonuses(int level,
			out IReadOnlyDictionary<string, int> bonuses)
		{
			var result = new Dictionary<string, int>();
			bonuses = result;

			foreach (var bonusName in _bonusNames)
				result.Add(bonusName, _bounsCoutForLevel);

			return true;
		}
	}
}