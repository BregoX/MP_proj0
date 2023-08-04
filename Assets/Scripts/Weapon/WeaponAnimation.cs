using UnityEngine;

namespace Weapon
{
	public class WeaponAnimation : MonoBehaviour
	{
		private const string ShotTriggerName = "shot";

		[SerializeField] private Animator _weaponAnimator;
		[SerializeField] private Weapon _weapon;

		private void Start()
		{
			_weapon.ShootOccured += OnShoot;
		}

		private void OnDestroy()
		{
			_weapon.ShootOccured -= OnShoot;
		}

		private void OnShoot()
		{
			_weaponAnimator.SetTrigger(ShotTriggerName);
		}
	}
}