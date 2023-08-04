using UnityEngine;
using Weapon;

namespace Character.Enemy
{
	public class EnemyGun : Gun
	{
		public void Shoot(Vector3 position, Vector3 velocity)
		{
			var bullet = Instantiate(Bullet, position, Quaternion.identity);
			bullet.Init(velocity);

			ShootOccured?.Invoke();
		}
	}
}