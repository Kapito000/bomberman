using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Menu = Constant.CreateAssetMenu.Path;

namespace Gameplay.Feature.Bonus.StaticData
{
	[CreateAssetMenu(menuName = Menu.c_StaticData + nameof(BonusNames))]
	public sealed class BonusNames : ScriptableObject, IBonusNames
	{
		[field: SerializeField] public string AddLifePoint { get; private set; }
		[field: SerializeField] public string IncreaseSpeed { get; private set; }

		IEnumerable<string> _names;

		public IEnumerator<string> GetEnumerator()
		{
			_names ??= new List<string>
			{
				AddLifePoint,
				IncreaseSpeed,
			};

			return _names.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}