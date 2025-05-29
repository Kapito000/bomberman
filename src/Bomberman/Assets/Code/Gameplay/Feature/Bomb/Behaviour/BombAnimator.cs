using Infrastructure.ECS;
using Infrastructure.ECS.Wrapper;
using UnityEngine;
using Zenject;

namespace Gameplay.Feature.Bomb.Behaviour
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Animator))]
	public sealed class BombAnimator : EntityDependantBehavior
	{
		[Inject] EntityWrapper _bomb;
		
		Animator _animator;
		
		void Awake()
		{
			_animator = GetComponent<Animator>();
		}
	}
}