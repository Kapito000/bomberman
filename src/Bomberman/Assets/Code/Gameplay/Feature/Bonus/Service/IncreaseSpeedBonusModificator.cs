using Gameplay.Feature.Bonus.StaticData;
using Gameplay.Feature.Hero.StaticData;
using Zenject;

namespace Gameplay.Feature.Bonus.Service
{
	public sealed class
		IncreaseSpeedBonusModificator : IIncreaseSpeedBonusModificator
	{
		[Inject] IHeroData _heroData;
		[Inject] IIncreaseSpeedBonusData _data;

		public float IncreasedSpeed() =>
			_heroData.MovementSpeed * (1 + _data.IncreaseSpeedAsPercentage);
	}
}