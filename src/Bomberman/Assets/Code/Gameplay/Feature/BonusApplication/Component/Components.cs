using Leopotam.EcsLite;

namespace Gameplay.Feature.BonusApplication.Component
{
	public struct ApplyBonusRequest { }
	public struct BonusApplicationTarget { public EcsPackedEntityWithWorld Value; }
	
	public struct BombPocketSizeBonusTimer { public float Value; }
	
	public struct IncreaseMovementSpeedBonusTimer { public float Value; }
}