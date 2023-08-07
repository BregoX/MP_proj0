using DefaultNamespace;
using UnityEngine;

namespace Weapon
{
	public class PlayerGun : Gun
	{
		[SerializeField] private Transform _bulletPoint;
		[SerializeField] private float _bulletSpeed = 5;
		[SerializeField] private float _reloadTime = 0.5f;
		[SerializeField] private int _damage = 1;

		private float _lastShotTime;

		public bool TryShoot(out ShotInfo shotInfo)
		{
			shotInfo = new ShotInfo();

			if (Time.time - _lastShotTime < _reloadTime) return false;

			var direction = _bulletPoint.forward;
			var bullet = Instantiate(Bullet, _bulletPoint.position, _bulletPoint.rotation);
			bullet.Init(direction * _bulletSpeed, _damage);
			_lastShotTime = Time.time;

			ShootOccured?.Invoke();

			direction *= _bulletSpeed;

			shotInfo.pX = _bulletPoint.position.x;
			shotInfo.pY = _bulletPoint.position.y;
			shotInfo.pZ = _bulletPoint.position.z;

			shotInfo.dX = direction.x;
			shotInfo.dY = direction.y;
			shotInfo.dZ = direction.z;

			return true;
		}
	}
}