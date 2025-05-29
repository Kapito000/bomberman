using Gameplay.StaticData;
using Gameplay.UI.Shop.ShopContent;
using UnityEngine;
using Menu = Constant.CreateAssetMenu.Path;

#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

namespace MetaSystem.Shop.StaticData
{
	[CreateAssetMenu(menuName = c_createAssetMenuName)]
	public sealed partial class PurchasableItem : ScriptableObject, IStaticData
	{
		const string c_createAssetMenuName = Menu.c_StaticData +
			Menu.c_Purchasable + nameof(PurchasableItem);

#if UNITY_EDITOR
		[HideInInspector]
#endif
		[SerializeField] string _id;
		[SerializeField] float _cost;
		[SerializeField] bool _unique;
		[SerializeField] ShopContentMode _shopCategory;
		[SerializeField] string _name;
		[SerializeField] string _description;

		public string Id => _id;
		public float Cost => _cost;
		public ShopContentMode ShopCategory => _shopCategory;
		public string Name => _name;
		public string Description => _description;
		public bool Unique => _unique;
	}
}