using UnityEngine;

namespace Weapon
{
	public class WeaponAnimation : MonoBehaviour
	{
		private const string ShotTriggerName = "shot";

		[SerializeField] private Animator _weaponAnimator;
		[SerializeField] private Gun _playerGun;

		private void Start()
		{
			_playerGun.ShootOccured += OnShoot;
		}

		private void OnDestroy()
		{
			_playerGun.ShootOccured -= OnShoot;
		}

		private void OnShoot()
		{
			_weaponAnimator.SetTrigger(ShotTriggerName);
		}
	}
}