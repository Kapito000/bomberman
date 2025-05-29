﻿using Infrastructure;

namespace Gameplay.FinishLevel
{
	public interface IFinishLevelService : IService
	{
		bool GameOver(int observerEntity);
		bool LevelComplete(int observerEntity);
		void SwitchGameToMainMenu();
		void LaunchGamePause();
		void LaunchNextLevel();
		void RestartThisLevel();
	}
}