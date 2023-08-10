using DefaultNamespace;
using Weapon;

namespace Character.Player
{
	public class PlayerWeaponArmory : WeaponArmory<PlayerGun>
	{
		public bool TryShoot(out ShotInfo shotInfo)
		{
			return ActiveGun.TryShoot(out shotInfo);
		}
	}
}