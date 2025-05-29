﻿using Common.Component;
using Gameplay.Feature.Hero.Component;
using Infrastructure.ECS;
using Infrastructure.ECS.Wrapper;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Zenject;

namespace Gameplay.Feature.Hero.System
{
	public sealed class HeroAnimationSystem : IEcsRunSystem
	{
		[Inject] EntityWrapper _hero;

		readonly EcsFilterInject<
			Inc<HeroComponent, HeroAnimatorComponent, MovementDirection>> _filter;

		public void Run(IEcsSystems systems)
		{
			foreach (var hero in _filter.Value)
			{
				_hero.SetEntity(hero);
				var direction = _hero.MoveDirection();
				var heroAnimator = _hero.HeroAnimator();

				if (_hero.IsMoving())
					heroAnimator.SetMoveDirection(direction);
				else
					heroAnimator.Stop();
			}
		}
	}
}