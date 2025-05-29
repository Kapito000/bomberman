using Gameplay.Feature.MainMenu.Component;
using Gameplay.Windows;
using Infrastructure.ECS.Wrapper;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Zenject;

namespace Gameplay.Feature.MainMenu.System
{
	public sealed class WindowRegistrationSystem : IEcsRunSystem
	{
		[Inject] EntityWrapper _window;
		[Inject] IWindowService _windowService;

		readonly EcsFilterInject<Inc<WindowComponent, BaseWindowComponent>>
			_windowFilter;

		public void Run(IEcsSystems systems)
		{
			foreach (var windowEntity in _windowFilter.Value)
			{
				_window.SetEntity(windowEntity);
				var windowMono = _window.BaseWindow();
				if (_windowService.TryRegistry(windowMono) == false)
				{
					Debug.LogError($"Cannot to registry window \"{windowMono.Id}\".");
					continue;
				}

				if (windowMono.gameObject.activeSelf)
				{
					_windowService.Open(windowMono.Id);
				}
			}
		}
	}
}