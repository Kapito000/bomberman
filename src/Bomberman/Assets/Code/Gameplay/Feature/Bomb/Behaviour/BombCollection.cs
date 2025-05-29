using System.Linq;
using Common.Dictionary;
using Common.Util.Enum;
using Gameplay.PlayersBombCollection;
using UnityEngine;
using Zenject;

namespace Gameplay.Feature.Bomb.Behaviour
{
	public class BombCollection : MonoBehaviour, IBombCollection
	{
		[SerializeField] BombTypeIntDictionary _collection;

		[Inject] IBombCollectionService _bombCollectionService;

		public bool TryGetCount(BombType bombType, out int count)
		{
			if (ExistBombTypeWithDebug(bombType) == false)
			{
				count = default;
				return false;
			}

			return _collection.TryGetValue(bombType, out count);
		}

		public void AddBomb(BombType bombType)
		{
			if (ExistBombTypeWithDebug(bombType) == false)
				return;


			if (_collection.ContainsKey(bombType) == false)
			{
				_collection.Add(bombType, 1);
				return;
			}

			_collection[bombType] += 1;
		}

		public void SetBombCount(BombType bombType, int count)
		{
			if (ExistBombTypeWithDebug(bombType) == false)
				return;

			if (_collection.ContainsKey(bombType) == false)
				return;

			_collection[bombType] = Mathf.Max(0, count);
		}

		public void DecrementBomb(BombType bombType)
		{
			if (TryGetCount(bombType, out var count) == false)
				return;

			SetBombCount(bombType, count - 1);
		}

		public void AddDefaultBomb() =>
			AddBomb(DefaultBombType());

		public int DefaultBombCount() =>
			_collection[DefaultBombType()];

		public BombType DefaultBombType() =>
			_bombCollectionService.DefaultBombType();

		bool ExistBombTypeWithDebug(BombType bombType)
		{
			if (AllValues<BombType>.Values.Contains(bombType))
				return true;

			Debug.LogError($"An attempt to add non-exist bomb: {bombType}.");
			return false;
		}
	}
}