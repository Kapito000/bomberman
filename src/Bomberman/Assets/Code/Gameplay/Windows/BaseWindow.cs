using UnityEngine;
using Zenject;

namespace Gameplay.Windows
{
	public abstract class BaseWindow : MonoBehaviour
	{
		[Inject] IWindowService _windowService;

		public abstract WindowId Id { get; }

		public void Init()
		{
			Initialize();
			SubscribeUpdates();
		}

		void OnDestroy() =>
			Cleanup();

		public virtual void Show()
		{
			gameObject.SetActive(true);
			OnShowed();
		}

		public virtual void Hide()
		{
			gameObject.SetActive(false);
		}

		protected virtual void Initialize()
		{ }

		protected virtual void SubscribeUpdates()
		{ }

		protected virtual void UnsubscribeUpdates()
		{ }

		protected virtual void OnCleanup()
		{ }

		protected virtual void OnShowed()
		{ }

		void Cleanup()
		{
			UnsubscribeUpdates();
			OnCleanup();
		}
	}
}