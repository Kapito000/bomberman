using System.Collections.Generic;
using Gameplay.Windows.Factory;
using UnityEngine;
using Zenject;

namespace Gameplay.Windows
{
	public sealed class WindowService : IWindowService, IWindowDistributor
	{
		[Inject] IWindowFactory _windowFactory;

		readonly List<WindowId> _closeList = new();
		readonly List<WindowId> _openedWindows = new();
		readonly Dictionary<WindowId, BaseWindow> _poolWindows = new();

		public void Create(Transform parent, params WindowId[] ids) =>
			Create(parent, (IEnumerable<WindowId>)ids);

		public void Create(Transform parent, IEnumerable<WindowId> ids)
		{
			foreach (var id in ids)
				TryCreate(id, parent);
		}

		public bool TryRegistry(BaseWindow window)
		{
			if (_poolWindows.TryAdd(window.Id, window))
				return true;

			Debug.LogError($"The window \"{window.Id}\" already registered.");
			return false;
		}

		public bool Opened(WindowId windowId) =>
			_openedWindows.Contains(windowId);

		public void Open(WindowId windowId)
		{
			if (_openedWindows.Contains(windowId))
				return;

			if (_poolWindows.TryGetValue(windowId, out var window) == false)
			{
				Debug.LogWarning($"Cannot open \"{windowId}\" windows. " +
					$"The window has not been added to the pool.");
				return;
			}

			window.Show();
			_openedWindows.Add(windowId);
		}

		public void OpenOnly(WindowId windowId)
		{
			CloseAll();
			Open(windowId);
		}

		public void Close(WindowId windowId)
		{
			if (_poolWindows.TryGetValue(windowId, out var window) == false)
				return;

			window.Hide();
			_openedWindows.Remove(windowId);
		}

		public void CloseAll()
		{
			_closeList.Clear();

			foreach (var windowId in _openedWindows)
				_closeList.Add(windowId);

			_closeList.ForEach(Close);
		}

		public bool TryGetWindow<TWindow>(WindowId id, out TWindow window)
			where TWindow : class, IWindow
		{
			if (_poolWindows.TryGetValue(id, out var pooledWindow) == false)
			{
				window = default;
				return false;
			}

			window = pooledWindow as TWindow;
			if (window == null)
				return false;

			return true;
		}

		bool TryCreate(WindowId id, Transform parent)
		{
			if (_poolWindows.ContainsKey(id))
			{
				Debug.LogWarning($"The try to create window \"{id}\" twice.");
				return false;
			}

			var window = _windowFactory.CreateWindow(id, parent);
			_poolWindows[id] = window;
			window.Hide();
			return true;
		}

		void DestroyWindow(BaseWindow window)
		{
			_poolWindows.Remove(window.Id);
			Object.Destroy(window);
		}

		void DestroyAllWindows()
		{
			var windowIds = _poolWindows.Keys;
			foreach (var windowId in windowIds)
				DestroyWindow(_poolWindows[windowId]);
		}
	}
}