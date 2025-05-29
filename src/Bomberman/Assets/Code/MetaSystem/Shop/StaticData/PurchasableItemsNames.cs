using UnityEngine;
using Menu = Constant.CreateAssetMenu.Path;

namespace MetaSystem.Shop.StaticData
{
	[CreateAssetMenu(menuName = c_createAssetMenuName)]
	public sealed class PurchasableItemsNames : ScriptableObject
	{
		const string c_createAssetMenuName =
			Menu.c_StaticData + Menu.c_Purchasable + nameof(PurchasableItemsNames);

		[field: SerializeField] public string[] Names { get; private set; }
	}
}