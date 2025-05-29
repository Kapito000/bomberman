using UnityEngine;
using Menu = Constant.CreateAssetMenu.Path;

namespace Gameplay.Feature.Bonus.StaticData
{
	[CreateAssetMenu(menuName =
		Menu.c_StaticData + nameof(PocketResizeBonusSettings))]
	public sealed class PocketResizeBonusSettings : ScriptableObject,
		IPocketResizeBonusSettings
	{
		[field: SerializeField] public float Timer { get; private set; } = 10f;
	}
}