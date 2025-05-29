using Gameplay.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Popup.Window
{
	public sealed class PopupWindow : BaseWindow, IPopupWindow
	{
		[SerializeField] Button _closeButton;
		[SerializeField] TMP_Text _title;
		[SerializeField] TMP_Text _message;

		public override WindowId Id => WindowId.Popup;

		public void Refresh(RefreshData data)
		{
			_title.text = data.Title;
			_message.text = data.Message;
			_closeButton.gameObject.SetActive(data.CloseButton);
		}
	}
}