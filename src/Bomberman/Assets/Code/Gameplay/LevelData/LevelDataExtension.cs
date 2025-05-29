using Infrastructure.ECS;
using Infrastructure.ECS.Wrapper;

namespace Gameplay.LevelData
{
	public static class LevelDataExtension
	{
		public static EntityWrapper NewEntityWrapper(this ILevelData levelData) => 
			new(levelData.World);
	}
}