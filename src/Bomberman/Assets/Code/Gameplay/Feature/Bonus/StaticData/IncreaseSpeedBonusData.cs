using UnityEngine;
using Menu = Constant.CreateAssetMenu.Path;

namespace Gameplay.Feature.Bonus.StaticData
{
	[CreateAssetMenu(menuName =
		Menu.c_StaticData + nameof(IncreaseSpeedBonusData))]
	public class IncreaseSpeedBonusData : ScriptableObject,
		IIncreaseSpeedBonusData
	{
		[field: SerializeField] public float IncreaseSpeedTimer { get; private set; }
		[field: SerializeField] public float IncreaseSpeedAsPercentage { get; private set; }
	}
}