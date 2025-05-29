using Common.Dictionary;

namespace Infrastructure.AssetProvider.SpriteLibraries
{
	public interface ISpriteLibraryProvider
	{
		StringSpriteLibraryDictionary BombSkins { get; }
	}
}