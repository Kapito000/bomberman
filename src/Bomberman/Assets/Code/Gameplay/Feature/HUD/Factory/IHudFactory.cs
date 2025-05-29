﻿using Infrastructure.Factory;
using UnityEngine;

namespace Gameplay.Feature.HUD.Factory
{
	public interface IHudFactory : IFactory
	{
		void CreatePutBombButtonPanel(Transform parent);
		void CreateCharacterJoystick(Transform parent);
		int CreateHudRoot(Transform parent);
		int CreateUpperPanel(Transform parent);
		int CreateTimerDisplay(Transform parent);
		int CreateLifePointsPanel(Transform parent);
		int CreateBombCounterPanel(Transform parent);
		int CreateEnemyCounterDisplay(Transform parent);
	}
}