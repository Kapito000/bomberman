using Gameplay.Feature.FinishLevel.Component;
using Infrastructure.ECS;
using Infrastructure.ECS.Wrapper;
using Zenject;

namespace Gameplay.FinishLevel.Condition.GameOver
{
	public sealed class GameTimerCondition : IGameOverCondition
	{
		[Inject] EntityWrapper _observer;

		public bool Check(int observerEntity)
		{
			_observer.SetEntity(observerEntity);
			return _observer.Has<GameTimerOver>();
		}
	}
}