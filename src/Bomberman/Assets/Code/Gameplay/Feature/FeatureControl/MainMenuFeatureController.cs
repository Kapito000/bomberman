﻿using Gameplay.Feature.MainMenu;
using Infrastructure.Factory.SystemFactory;

namespace Gameplay.Feature.FeatureControl
{
	public sealed class MainMenuFeatureController : FeatureController
	{
		public MainMenuFeatureController(ISystemFactory systemFactory)
			: base(systemFactory)
		{
			Add<MainMenuFeature>();
		}
	}
}