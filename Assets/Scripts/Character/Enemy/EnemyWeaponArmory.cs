using UnityEngine;
using Weapon;

namespace Character.Enemy
{
	public class EnemyWeaponArmory : WeaponArmory<EnemyGun>
	{
		public void Shoot(Vector3 position, Vector3 velocity)
		{
			ActiveGun.Shoot(position, velocity);
		}

		public void ChangeWeapon(int weaponInfo)
		{
			TryChangeWeapon(weaponInfo);
		}
	}
}