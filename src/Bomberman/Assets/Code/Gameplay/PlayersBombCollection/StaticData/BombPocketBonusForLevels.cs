using StaticTableData;
using UnityEngine;
using Zenject;

namespace Gameplay.PlayersBombCollection.StaticData
{
	public class BombPocketBonusForLevels : IBombPocketBonusForLevels
	{
		[Inject(Id = Constant.TsvDataId.c_BombPocketSizeBonus)]
		TextAsset _tsv;

		IFloatTable _table;

		public void Init()
		{
			var navigationType = IFloatTable.NavigationType.OnlyIndices;
			_table = TableFactory.ParseXSV(_tsv.text,
				SimpleFloatTable.SeparatorType.Tab, navigationType);
		}

		public bool TryGetPocketSize(int level, out int value)
		{
			const int c_columnIndex = 0;

			if (level > _table.RowCount)
				level = _table.RowCount - 1;

			if (_table.TryGetValue(c_columnIndex, level, out var result))
			{
				value = (int)result;
				return true;
			}

			value = default;
			return false;
		}
	}
}