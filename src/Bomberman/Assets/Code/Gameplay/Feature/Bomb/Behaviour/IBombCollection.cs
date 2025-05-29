namespace Gameplay.Feature.Bomb.Behaviour
{
	public interface IBombCollection
	{
		void AddBomb(BombType bombType);
		bool TryGetCount(BombType bombType, out int count);
		int DefaultBombCount();
		void AddDefaultBomb();
		void SetBombCount(BombType bombType, int count);
		BombType DefaultBombType();
		void DecrementBomb(BombType bombType);
	}
}