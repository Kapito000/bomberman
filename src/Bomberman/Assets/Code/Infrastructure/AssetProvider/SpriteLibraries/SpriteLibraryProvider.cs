using UnityEngine;
using StrLibraries = Common.Dictionary.StringSpriteLibraryDictionary;
using Menu = Constant.CreateAssetMenu.Path;

namespace Infrastructure.AssetProvider.SpriteLibraries
{
	[CreateAssetMenu(menuName =
		Menu.c_StaticData + nameof(SpriteLibraryProvider))]
	public sealed class SpriteLibraryProvider : ScriptableObject,
		ISpriteLibraryProvider
	{
		[field: SerializeField]
		public StrLibraries BombSkins { get; private set; }
	}
}