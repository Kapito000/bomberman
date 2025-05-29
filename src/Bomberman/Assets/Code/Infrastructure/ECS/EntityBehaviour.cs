﻿using AB_Utility.FromSceneToEntityConverter;
using Extensions;
using Gameplay.Collisions;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Infrastructure.ECS
{
	public partial class EntityBehaviour : MonoBehaviour, IEntityView
	{
		[Inject] EcsWorld _world;
		[Inject] ICollisionRegistry _collisionRegistry;

		EcsPackedEntity _packedEntity;

		void OnDestroy()
		{
			Dispose();
			OnDestroyed();
		}

		public void AddToEcs(out int entity)
		{
			entity = _world.NewEntity();
			Init(entity);
		}

		public void AttachEntity(int entity)
		{
			Init(entity);
		}

		public bool TryGetEntity(out int entity)
		{
			if (_packedEntity.Unpack(_world, out entity) == false)
			{
				Debug.LogError($"Cannot to get entity: {entity}.");
				return false;
			}

			return true;
		}

		public void Dispose()
		{
			foreach (var collider2d in GetComponentsInChildren<Collider2D>(
				         includeInactive: true))
				_collisionRegistry.Unregister(collider2d.GetInstanceID());
		}

		protected virtual void OnDestroyed()
		{ }

		void Init(int entity)
		{
			SetEntity(entity);
			RegisterColliders(entity);
			ConvertConverters(_world, entity);
			ResolveEntityDependant();
			InitPartial(entity);
		}

		void SetEntity(int entity)
		{
			_packedEntity = _world.PackEntity(entity);
			_world.AddView(entity, this);
		}

		void RegisterColliders(int entity)
		{
			foreach (Collider2D collider2d in GetComponentsInChildren<Collider2D>(
				         includeInactive: true))
				_collisionRegistry.Register(collider2d.GetInstanceID(), entity);
		}

		void ConvertConverters(EcsWorld world, int entity)
		{
			if (false == TryGetComponent<ComponentsContainer>(out var container))
				return;

			var destroyAfterConversion = container.DestroyAfterConversion;
			var packedEntity = world.PackEntityWithWorld(entity);

			for (int j = 0; j < container.Converters.Length; j++)
			{
				var converter = container.Converters[j];
				converter.Convert(packedEntity);

				if (destroyAfterConversion)
					Destroy(converter);
			}

			if (destroyAfterConversion)
				Destroy(container);
		}

		void ResolveEntityDependant()
		{
			var dependants = GetComponents<EntityDependantBehavior>();
			foreach (var dependant in dependants)
			{
				dependant.Init(this);
			}
		}
	}
}