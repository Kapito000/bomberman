using Extensions;
using Infrastructure.ECS.Wrapper;

namespace Gameplay.Feature.BonusApplication
{
	public static class Util
	{
		public static bool TryGetTargetEntity(EntityWrapper bonus, ref EntityWrapper target)
		{
			var targetPack = bonus.BonusApplicationTarget();
			if (targetPack.Unpack(out var targetEntity))
			{
				target.SetEntity(targetEntity);
				return true;
			}

			target = default;
			Common.Logger.Error.CannotUnpackEntity();
			return false;
		}

	}
}