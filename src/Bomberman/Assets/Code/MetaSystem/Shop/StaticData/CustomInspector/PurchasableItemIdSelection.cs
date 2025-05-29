#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace MetaSystem.Shop.StaticData
{
	public partial class PurchasableItem
	{
		const string c_itemIdSelectionBoxGroup = "Item id selection";

		[BoxGroup(c_itemIdSelectionBoxGroup)]
		[SerializeField] PurchasableItemsNames _namesObj;

		[ShowInInspector]
		[PropertyOrder(0)]
		[LabelText("Id")]
		[ValueDropdown(nameof(ItemIdsEnumerable))]
		string Id_Editor
		{
			get => _id;
			set
			{
				Undo.RecordObject(this, "Set Id to " + value);
				_id = value;
				EditorUtility.SetDirty(this);
			}
		}

		IEnumerable<string> ItemIdsEnumerable()
		{
			if (_namesObj == null)
				yield break;

			foreach (var itemId in _namesObj.Names)
				yield return itemId;
		}
	}
}
#endif