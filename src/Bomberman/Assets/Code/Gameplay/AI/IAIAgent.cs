using Infrastructure.ECS;
using Infrastructure.ECS.Wrapper;

namespace Gameplay.AI
{
	public interface IAIAgent
	{
		public EntityWrapper Entity { get; }
	}
}