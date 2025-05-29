using System.Globalization;
using System.Linq;
using Common.Logger;
using Common.Util.Enum;
using Gameplay.UI.Shop.Factory;
using Gameplay.UI.Shop.ShopContent;
using Gameplay.Windows;
using MetaSystem.Shop;
using MetaSystem.Shop.StaticData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ContentLists =
	System.Collections.Generic.IReadOnlyDictionary<
		Gameplay.UI.Shop.ShopContent.ShopContentMode,
		Gameplay.UI.Shop.ShopContent.ShopContentViewsList>;

namespace Gameplay.UI.Shop.Window
{
	public class ShopWindow : BaseWindow, IShopWindow
	{
		[SerializeField] Button _putToCartButton;
		[SerializeField] TMP_Text _costText;
		[SerializeField] TMP_Text _descriptionText;

		[Inject] ContentLists _contentLists;
		[Inject] IShopUiFactory _shopUiFactory;
		[Inject] IShoppingCartService _shoppingCartService;
		[Inject] IPurchaseItemProvider _purchaseItemProvider;

		PurchasableItem _selectedItem;
		ShopContentMode _currentContentMode;

		public override WindowId Id => WindowId.Shop;

		protected override void Initialize()
		{
			_putToCartButton.onClick.AddListener(OnPutToCartButtonClick);
			ShowContent(ShopContentMode.Bombs);
		}

		protected override void OnCleanup()
		{
			_putToCartButton.onClick.RemoveListener(OnPutToCartButtonClick);
		}

		public void SwitchContent(ShopContentMode modeTarget)
		{
			if (false == AllValues<ShopContentMode>.Values.Contains(modeTarget))
			{
				Debug.LogError($"The shop content mode {modeTarget} is not supported.");
				return;
			}

			HideAllContentView();
			ShowContent(modeTarget);
		}

		public void ProcessSelectedItem(string id)
		{
			if (TryGetPurchasable(id, out _selectedItem) == false)
				return;

			_costText.text = _selectedItem.Cost.ToString(CultureInfo.CurrentCulture);
			_descriptionText.text = _selectedItem.Description;
		}

		protected override void OnShowed()
		{
			ShowContent(_currentContentMode);
		}

		void OnPutToCartButtonClick()
		{
			if (_selectedItem == null)
				return;

			_shoppingCartService.Put(_selectedItem.Id);
		}

		void ShowContent(ShopContentMode mode)
		{
			if (_contentLists.ContainsKey(mode) == false)
			{
				Error.CannotFind(
					$"The shop content mode: \"{mode}\" is not supported.");
				return;
			}

			_currentContentMode = mode;

			ClearView();
			_contentLists[mode]
				.Refresh(mode)
				.Show();
		}

		void HideAllContentView()
		{
			foreach (var view in _contentLists)
				view.Value.Hide();
		}

		bool TryGetPurchasable(string id, out PurchasableItem item)
		{
			if (_purchaseItemProvider.TryGet(id, out item) == false)
			{
				Error.CannotFind(id);
				return false;
			}

			return true;
		}

		void ResetSelectedItem()
		{
			_selectedItem = default;
		}

		void ClearView()
		{
			ResetSelectedItem();
			_costText.text = _descriptionText.text = string.Empty;
		}
	}
}