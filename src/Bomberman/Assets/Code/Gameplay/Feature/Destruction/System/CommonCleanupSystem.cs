﻿using Common.Component;
using Infrastructure.ECS;
using Infrastructure.ECS.Wrapper;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Zenject;

namespace Gameplay.Feature.Destruction.System
{
	public sealed class CommonCleanupSystem : IEcsRunSystem
	{
		readonly EcsFilterInject<Inc<FirstBreath>> _firstBreathFilter;
		readonly EcsFilterInject<Inc<ObjectFirstBreath>> _objectFirstBreathFilter;

		[Inject] EntityWrapper _entity;

		public void Run(IEcsSystems systems)
		{
			foreach (var e in _firstBreathFilter.Value)
			{
				_entity.SetEntity(e);
				_entity.Remove<FirstBreath>();
			}
			
			foreach (var e in _objectFirstBreathFilter.Value)
			{
				_entity.SetEntity(e);
				_entity.Remove<ObjectFirstBreath>();
			}
		}
	}
}