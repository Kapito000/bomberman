using Gameplay.Feature.Bomb;
using Gameplay.Input.Character;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Feature.Input.Behavior
{
	[RequireComponent(typeof(Button))]
	public class PutBombButton : MonoBehaviour
	{
		[SerializeField] BombType _bombType;
		
		[Inject] ICharacterInputActionProvider _input;
		
		Button _button;

		void Awake()
		{
			_button = GetComponent<Button>();
			_button.onClick.AddListener(OnButtonClick);
		}

		void OnButtonClick()
		{
			_input.CallPutBomb(_bombType);
		}
	}
}