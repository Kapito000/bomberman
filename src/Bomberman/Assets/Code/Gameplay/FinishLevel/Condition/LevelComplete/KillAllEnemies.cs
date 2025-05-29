using Gameplay.Feature.FinishLevel.Component;
using Infrastructure.ECS;
using Infrastructure.ECS.Wrapper;
using Zenject;

namespace Gameplay.FinishLevel.Condition.LevelComplete
{
	public sealed class KillAllEnemies : ILevelCompleteCondition
	{
		[Inject] EntityWrapper _observer;

		public bool Check(int observerEntity)
		{
			_observer.SetEntity(observerEntity);
			return _observer.Has<AllEnemiesKilled>();
		}
	}
}