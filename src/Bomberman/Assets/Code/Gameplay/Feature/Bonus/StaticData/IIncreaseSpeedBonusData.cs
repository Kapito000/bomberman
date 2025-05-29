using Gameplay.StaticData;

namespace Gameplay.Feature.Bonus.StaticData
{
	public interface IIncreaseSpeedBonusData : IStaticData
	{
		float IncreaseSpeedTimer { get; }
		float IncreaseSpeedAsPercentage { get; }
	}
}