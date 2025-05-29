#if UNITY_EDITOR
using Infrastructure.Factory.SystemFactory;

namespace Gameplay.Feature.Dev
{
	public sealed class DevFeature : Infrastructure.ECS.Feature
	{
		public DevFeature(ISystemFactory systemFactory) : base(systemFactory)
		{ }
	}
}
#endif