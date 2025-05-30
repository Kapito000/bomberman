using System.Collections.Generic;

namespace Gameplay.Feature.Bonus.StaticData
{
	internal interface IBonusesForLevel
	{
		bool TryGetBonuses(int level,
			out IReadOnlyDictionary<string, int> bonuses);
	}
}