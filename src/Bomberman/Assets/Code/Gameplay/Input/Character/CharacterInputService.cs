using System;
using Gameplay.Feature.Bomb;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.Input.Character
{
	public sealed class CharacterInputService : ICharacterInput, IDisposable,
		ICharacterInputActionProvider
	{
		[Inject] Controls _controls;

		public Vector2 Movement { get; private set; }
		public event Action<BombType> PutBomb;
		public event Action<Vector2> ScreenClick;

		[Inject]
		void Construct()
		{
			SubscribeOnPutBombInputEvents();

			_controls.Character.Movement.performed += OnMovePerformed;
			_controls.Character.ScreenTap.performed += OnScreenTapPerformed;
		}

		public void Enable() =>
			_controls.Character.Enable();

		public void Disable() =>
			_controls.Character.Disable();

		public void Dispose()
		{
			UnsubscribeFromPutBombInputEvents();
			_controls.Character.Movement.performed -= OnMovePerformed;
			_controls.Character.ScreenTap.performed -= OnScreenTapPerformed;
		}

		void SubscribeOnPutBombInputEvents()
		{
			_controls.Character.PutBigBomb.performed += OnPutBigBombPerformed;
			_controls.Character.PutUsualBomb.performed += OnPutUsualBombPerformed;
			_controls.Character.PutHunterBomb.performed += OnPutHunterBombPerformed;
			_controls.Character.PutTimeDelayBomb.performed +=
				OnPutTimeDelayBombPerformed;
			_controls.Character.PutRemoteDetonationBomb.performed +=
				OnPutRemoteDetonationBombPerformed;
		}

		void UnsubscribeFromPutBombInputEvents()
		{
			_controls.Character.PutBigBomb.performed -= OnPutBigBombPerformed;
			_controls.Character.PutUsualBomb.performed -= OnPutUsualBombPerformed;
			_controls.Character.PutHunterBomb.performed -= OnPutHunterBombPerformed;
			_controls.Character.PutTimeDelayBomb.performed -=
				OnPutTimeDelayBombPerformed;
			_controls.Character.PutRemoteDetonationBomb.performed -=
				OnPutRemoteDetonationBombPerformed;
		}

		void OnPutRemoteDetonationBombPerformed(
			InputAction.CallbackContext context) =>
			CallPutBombEvent(BombType.RemoteDetonation);

		void OnPutHunterBombPerformed(InputAction.CallbackContext context) =>
			CallPutBombEvent(BombType.Hunter);

		void OnPutUsualBombPerformed(InputAction.CallbackContext context) =>
			CallPutBombEvent(BombType.Usual);

		void OnPutBigBombPerformed(InputAction.CallbackContext context) =>
			CallPutBombEvent(BombType.Big);

		void OnPutTimeDelayBombPerformed(InputAction.CallbackContext context) =>
			CallPutBombEvent(BombType.TimeDelay);

		void ICharacterInputActionProvider.CallPutBomb(BombType bombType) =>
			CallPutBombEvent(bombType);

		void OnMovePerformed(InputAction.CallbackContext context) =>
			Movement = context.ReadValue<Vector2>();

		void OnScreenTapPerformed(InputAction.CallbackContext context)
		{
			var screenPos = _controls.Character.MousePos.ReadValue<Vector2>();
			ScreenClick?.Invoke(screenPos);
		}

		void CallPutBombEvent(BombType bombType) =>
			PutBomb?.Invoke(bombType);
	}
}