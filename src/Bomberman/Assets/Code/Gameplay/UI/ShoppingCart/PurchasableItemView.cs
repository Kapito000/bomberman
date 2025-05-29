using MetaSystem.Shop;
using MetaSystem.Shop.StaticData;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Menu = Constant.AddComponentMenu;
using NotImplementedException = System.NotImplementedException;

namespace Gameplay.UI.ShoppingCart
{
	[AddComponentMenu(c_addComponentMenu)]
	public sealed class PurchasableItemView : MonoBehaviour
	{
		const string c_addComponentMenu =
			Menu.c_ShoppingCart + Menu.c_UI + nameof(PurchasableItemView);

		[ReadOnly]
		[SerializeField] string _itemId;
		[Space]
		[SerializeField] Image _icon;
		[SerializeField] TMP_Text _name;
		[SerializeField] TMP_Text _count;
		[SerializeField] Button _incrementItemsButton;
		[SerializeField] Button _decrementItemsButton;
		
		[Inject] IShoppingCartService _shoppingCartService;

		public struct RefreshData
		{
			public PurchasableItem Item;
			public int Count;
		}

		public void Init()
		{
			_incrementItemsButton.onClick.AddListener(OnIncrementItemsClick);
			_decrementItemsButton.onClick.AddListener(OnDecrementItemsClick);
		}

		void OnDestroy()
		{
			_incrementItemsButton.onClick.RemoveListener(OnIncrementItemsClick);
			_decrementItemsButton.onClick.RemoveListener(OnDecrementItemsClick);
		}

		public void Show()
		{
			gameObject.SetActive(true);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		public void Refresh(RefreshData data)
		{
			_itemId = data.Item.Id;
			_name.text = data.Item.Name;
			_count.text = data.Count.ToString();
		}
		
		void OnIncrementItemsClick()
		{
			_shoppingCartService.Put(_itemId);
		}
		
		void OnDecrementItemsClick()
		{
			_shoppingCartService.Pop(_itemId);
		}

		public void Block(bool block)
		{
			_incrementItemsButton.interactable = !block;
		}
	}
}