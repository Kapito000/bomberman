using Infrastructure;

namespace Gameplay.Feature.Bonus.Service
{
	public interface IIncreaseSpeedBonusModificator : IService
	{
		float IncreasedSpeed();
	}
}