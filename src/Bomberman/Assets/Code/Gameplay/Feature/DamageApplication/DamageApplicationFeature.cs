﻿using Gameplay.Feature.DamageApplication.System;
using Infrastructure.Factory.SystemFactory;

namespace Gameplay.Feature.DamageApplication
{
	public sealed class DamageApplicationFeature : Infrastructure.ECS.Feature
	{
		public DamageApplicationFeature(ISystemFactory systemFactory) : base(systemFactory)
		{
			AddUpdate<UpdateDamageBufferSystem>();
			AddUpdate<DamageBufferToDamageSystem>();
			
			AddUpdate<ApplyDamageSystem>();
			
			AddUpdate<ApplyTakenDamageEffectSystem>();
			AddUpdate<DamageEffectProcessSystem>();
			AddUpdate<DamageEffectDurationTimerSystem>();
			AddUpdate<TakenDamageAudioEffectSystem>();
			
			AddCleanup<CleanupSystem>();
		}
	}
}