using System;
using UnityEngine;

namespace Weapon
{
	public class Weapon : MonoBehaviour
	{
		[SerializeField] private Bullet _bullet;
		[SerializeField] private Transform _bulletPoint;
		[SerializeField] private float _bulletSpeed = 5;
		[SerializeField] private float _reloadTime = 0.5f;

		private float _lastShotTime;

		public Action ShootOccured;

		public void Shoot()
		{
			if (Time.time - _lastShotTime < _reloadTime) return;
			
			var bullet = Instantiate(_bullet, _bulletPoint.position, _bulletPoint.rotation);
			bullet.Init(_bulletPoint.forward, _bulletSpeed);
			_lastShotTime = Time.time;
			
			ShootOccured?.Invoke();
		}
	}
}