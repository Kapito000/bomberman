using Gameplay.Feature.Bomb;

namespace Gameplay.PlayersBombCollection
{
	public interface IBombCollectionService
	{
		int BombPocketSizeBonusForCurrentLevel();
		int DefaultBombPocketSize();
		BombType DefaultBombType();
	}
}