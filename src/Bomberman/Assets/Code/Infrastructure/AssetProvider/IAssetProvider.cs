using System.Collections.Generic;
using Cinemachine;
using Common.HUD;
using Gameplay.Feature.Bomb;
using Gameplay.Feature.HUD.Feature.Timer.Behaviour;
using Gameplay.UI.Shop;
using Gameplay.UI.Shop.ShopContent;
using Gameplay.Windows;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Infrastructure.AssetProvider
{
	public interface IAssetProvider : IService
	{
		Camera Camera();
		CinemachineVirtualCamera VirtualCamera();
		GameObject Hero();
		GameObject HeroSpawnPoint();
		GameObject Bomb(BombType bombType);
		Canvas UiRoot();
		Canvas HudRoot();
		GameObject CharacterJoystick();
		GameObject PutBombButtonPanel();
		EventSystem EventSystem();
		GameObject Explosion();
		GameObject UpperPanel();
		IntegerDisplay LifePointsPanel();
		IntegerDisplay BombCounterPanel();
		BaseWindow WindowPrefab(WindowId id);
		GameObject BaseEnemy();
		GameObject DestructibleTile();
		GameObject WindowsRoot();
		GameTimerDisplay GameTimerDisplay();
		GameObject FinishLevelDoor();
		Dictionary<WindowId, BaseWindow> AllWindows { get; }
		GameObject GameMusic();
		GameObject FinishLevelMusic();
		GameObject MainMenuUpperPanel();
		IntegerDisplay EnemyCounterDisplay();
		GameObject Bonus();
		PurchasableItemView ShopItemView();
		Gameplay.UI.ShoppingCart.PurchasableItemView ShoppingCartItemView();
	}
}