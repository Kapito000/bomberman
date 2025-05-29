using Gameplay.StaticData;

namespace Gameplay.PlayersBombCollection.StaticData
{
	public interface IBombPocketBonusForLevels : IStaticData
	{
		void Init();
		bool TryGetPocketSize(int level, out int value);
	}
}