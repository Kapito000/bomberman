using Gameplay.Feature.MainMenu.Component;
using Infrastructure.ECS.Wrapper;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Zenject;

namespace Gameplay.Feature.MainMenu.System
{
	public sealed class IntiWindowsSystem : IEcsRunSystem
	{
		[Inject] EntityWrapper _window;

		readonly EcsFilterInject<Inc<WindowComponent, BaseWindowComponent>>
			_windowFilter;

		public void Run(IEcsSystems systems)
		{
			foreach (var windowEntity in _windowFilter.Value)
			{
				_window.SetEntity(windowEntity);
				var windowMono = _window.BaseWindow();
				windowMono.Init();
			}
		}
	}
}