﻿using Common.Component;
using Gameplay.Feature.HUD.Component;
using Gameplay.Feature.HUD.Factory;
using Infrastructure.ECS;
using Infrastructure.ECS.Wrapper;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Zenject;

namespace Gameplay.Feature.HUD.Feature.Input.System
{
	public sealed class CreatePutBombButtonSystem : IEcsRunSystem
	{
		[Inject] IHudFactory _hudFactory;
		[Inject] EntityWrapper _hudRoot;
		
		readonly EcsFilterInject<Inc<HudRoot, TransformComponent>> _filter;

		public void Run(IEcsSystems systems)
		{
			foreach (var e in _filter.Value)
			{
				_hudRoot.SetEntity(e);
				var parent = _hudRoot.Transform();
				_hudFactory.CreatePutBombButtonPanel(parent);
			}
		}
	}
}