using System;
using Gameplay.Feature.Bomb.Behaviour;
using UnityEngine;

namespace Gameplay.Feature.Bomb.Component
{
	public struct BombStackSize { public int Value; }
	public struct BombCollectionComponent { public IBombCollection Value; }
	
	public struct BombParent { }
	public struct BombCarrier { }
	public struct BombComponent { }
	public struct BombExplosion { }
	public struct ExplosionTimer { public float Value; }
	public struct PutBombRequest { public BombType Value; }
	public struct ExplosionRadius { public int Value; }
	
	public struct BombHunter { }
	public struct BombHunterTarget { public Transform Value; }
	
	public struct BombRemoteDetonation { }
	
	public struct ExplosionPartComponent { public ExplosionPart Value; }
	
	public struct CallExplosion { }
	
	public struct Explosion { }
	public struct CreateExplosionRequest { }
	public struct BlowUpDestructible { }
	[Serializable] public struct BombAnimatorComponent { public BombAnimator Value; }
}