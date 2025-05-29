using System;
using Gameplay.Feature.Bomb;
using UnityEngine;

namespace Gameplay.Input.Character
{
	public interface ICharacterInput : IInput
	{
		Vector2 Movement { get; }
		event Action<BombType> PutBomb;
		event Action<Vector2> ScreenClick;
	}
}