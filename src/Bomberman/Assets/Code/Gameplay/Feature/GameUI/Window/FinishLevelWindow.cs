using Common.Logger;
using Gameplay.FinishLevel;
using Gameplay.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Feature.GameUI.Window
{
	public class FinishLevelWindow : BaseWindow
	{
		[SerializeField] WindowId _windowId;
		[Space]
		[SerializeField] Button _goToMainMenuButton;

		[Inject] IFinishLevelService _finishLevelService;

		public override WindowId Id => _windowId;

		protected override void Initialize()
		{
			Error.NoImplementation();
		}

		protected override void OnCleanup()
		{
			Error.NoImplementation();
		}
	}
}