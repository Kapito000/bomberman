using Gameplay.Feature.Bomb;

namespace Gameplay.Input.Character
{
	public interface ICharacterInputActionProvider
	{
		void CallPutBomb(BombType bombType);
	}
}