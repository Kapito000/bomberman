using Gameplay.UI.Shop.Window;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI.Shop.ShopContent
{
	[RequireComponent(typeof(Button))]
	public sealed class SwitchContentButton : MonoBehaviour
	{
		[SerializeField] ShopContentMode _modeTarget;
		
		[Inject] IShopWindow _shopWindow;

		Button _button;

		void Awake()
		{
			_button = GetComponent<Button>();
			_button.onClick.AddListener(OnButtonClick);
		}

		void OnButtonClick()
		{
			_shopWindow.SwitchContent(_modeTarget);
		}
	}
}