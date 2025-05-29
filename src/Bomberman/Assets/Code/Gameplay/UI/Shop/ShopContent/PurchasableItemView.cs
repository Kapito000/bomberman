using MetaSystem.Shop.StaticData;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Menu = Constant.AddComponentMenu;

namespace Gameplay.UI.Shop.ShopContent
{
	[AddComponentMenu(c_addComponentMenu)]
	public sealed class PurchasableItemView : MonoBehaviour
	{
		const string c_addComponentMenu =
			Menu.c_Shop + Menu.c_UI + nameof(PurchasableItemView);

		[ReadOnly]
		[SerializeField] string _itemId;
		[Space]
		[SerializeField] Button _selectButton;
		[SerializeField] TMP_Text _name;
		[SerializeField] GameObject _selectMarker;
		[SerializeField] GameObject _purchasedMarker;

		ShopContentViewsList _shopContentViewsList;

		bool _isSelected;

		public bool IsSelected => _isSelected;
		public string ItemId => _itemId;

		void Awake()
		{
			_selectButton.onClick.AddListener(OnButtonClick);
		}

		void OnDestroy()
		{
			_selectButton.onClick.RemoveListener(OnButtonClick);
		}

		public void Init(ShopContentViewsList selectItemSystem)
		{
			_shopContentViewsList = selectItemSystem;
		}

		public void Refresh(PurchasableItem item, bool purchased = false)
		{
			_itemId = item.Id;
			_name.text = item.Name;

			ProcessPurchaseState(purchased);
		}

		public void Show()
		{
			gameObject.SetActive(true);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		public void Select()
		{
			_isSelected = true;
			_selectMarker.SetActive(true);
		}

		public void Deselect()
		{
			_isSelected = false;
			_selectMarker.SetActive(false);
		}

		void ProcessPurchaseState(bool purchased)
		{
			_selectButton.interactable = !purchased;
			_purchasedMarker.gameObject.SetActive(purchased);
		}

		void OnButtonClick()
		{
			_shopContentViewsList.Select(this);
		}
	}
}