using System.Collections.Generic;

namespace Gameplay.Feature.Bonus.StaticData
{
	public interface IBonusNames : IEnumerable<string>
	{
		string AddLifePoint { get; }
		string IncreaseSpeed { get; }
	}
}