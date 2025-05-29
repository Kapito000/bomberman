using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.UI.Popup;
using Gameplay.UI.Popup.Window;
using Gameplay.Windows;
using MetaSystem.Shop;
using MetaSystem.Shop.StaticData;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI.ShoppingCart.Window
{
	public sealed class ShoppingCartWindow : BaseWindow, IShoppingCartWindow
	{
		[SerializeField] ShoppingCartContentList _contentList;
		[Space]
		[SerializeField] Button _buyButton;

		[Inject] IShopService _shopService;
		[Inject] IWindowService _windowService;
		[Inject] IWindowDistributor _windowDistributor;
		[Inject] IShoppingCartService _shoppingCartService;
		[Inject] IPurchaseItemProvider _purchaseItemProvider;

		public override WindowId Id => WindowId.ShoppingCart;

		protected override void Initialize()
		{
			_buyButton.onClick.AddListener(OnBuyButtonClick);
		}

		protected override void SubscribeUpdates()
		{
			_shoppingCartService.DataChanged += OnShoppingCartServiceDataChanged;
		}

		protected override void UnsubscribeUpdates()
		{
			_shoppingCartService.DataChanged -= OnShoppingCartServiceDataChanged;
		}

		protected override void OnCleanup()
		{
			_buyButton.onClick.RemoveListener(OnBuyButtonClick);
		}

		public override void Show()
		{
			gameObject.SetActive(true);
			Refresh();
		}

		public override void Hide()
		{
			gameObject.SetActive(false);
		}

		void Refresh()
		{
			_contentList.Refresh();
		}

		void OnBuyButtonClick()
		{
			LaunchPurchasePrecess().Forget();
		}

		void OnShoppingCartServiceDataChanged()
		{
			Refresh();
		}

		void BlockBuyButton(bool block = true) =>
			_buyButton.interactable = !block;

		async UniTaskVoid LaunchPurchasePrecess()
		{
			BlockBuyButton();
			_contentList.BlockItems();

			await _shoppingCartService.RequestPurchaseAsync();
			Refresh();

			_contentList.BlockItems(false);
			BlockBuyButton(false);
		}
	}
}