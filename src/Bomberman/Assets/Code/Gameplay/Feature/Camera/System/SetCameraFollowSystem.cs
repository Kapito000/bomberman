﻿using Extensions;
using Gameplay.Feature.Camera.Component;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Zenject;

namespace Gameplay.Feature.Camera.System
{
	public sealed class SetCameraFollowSystem : IEcsRunSystem
	{
		[Inject] EcsWorld _world;
		
		readonly EcsFilterInject<Inc<VirtualCamera, FollowTarget>> _filter;

		public void Run(IEcsSystems systems)
		{
			foreach (var e in _filter.Value)
			{
				var target = _world.FollowTarget(e);
				_world.VirtualCamera(e).Follow = target;
				_world.RemoveComponent<FollowTarget>(e);
			}
		}
	}
}