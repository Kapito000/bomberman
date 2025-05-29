using Gameplay.Feature.Bomb;
using Gameplay.PlayersBombCollection.StaticData;
using Gameplay.Progress;
using UnityEngine;
using Zenject;

namespace Gameplay.PlayersBombCollection
{
	public sealed class BombCollectionService : IBombCollectionService
	{
		[Inject] IProgressService _progressService;
		[Inject] IBombPocketBonusForLevels _bombPocketBonusForLevels;

		public BombType DefaultBombType() => 
			BombType.Usual;

		public int DefaultBombPocketSize()
		{
			const int c_defaultBombPocketSize = 1;
			return c_defaultBombPocketSize;
		}

		public int BombPocketSizeBonusForCurrentLevel()
		{
			var level = _progressService.ReachedLevel;
			if (false == _bombPocketBonusForLevels
				    .TryGetPocketSize(level, out var pocketSize))
			{
				Debug.LogError("Cannot to get pocket size.");
				const int c_defaultBonus = 2;
				return c_defaultBonus;
			}

			return pocketSize;
		}
	}
}