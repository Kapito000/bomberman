using System;
using Gameplay.Feature.Bomb;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Common.Dictionary
{
	[Serializable]
	public sealed class StringIntegerDictionary : SerializedDictionary<string, int>
	{ }

	[Serializable]
	public sealed class StringSpriteDictionary : SerializedDictionary<string, Sprite>
	{ }

	[Serializable]
	public sealed class BombTypeIntDictionary : SerializedDictionary<BombType, int>
	{ }

	[Serializable]
	public sealed class StringSpriteLibraryDictionary : SerializedDictionary<string, SpriteLibraryAsset>
	{ }
}