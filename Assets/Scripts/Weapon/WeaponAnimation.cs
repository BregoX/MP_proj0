using UnityEngine;

namespace Weapon
{
	public class WeaponAnimation : MonoBehaviour
	{
		private const string ShotTriggerName = "shot";

		[SerializeField] private Animator _weaponAnimator;
		[SerializeField] private WeaponsProvider _weaponsProvider;

		private void Start()
		{
			foreach (var weapon in _weaponsProvider.Weapons)
			{
				weapon.ShootOccured += OnShoot;
			}
		}

		private void OnDestroy()
		{
			foreach (var weapon in _weaponsProvider.Weapons)
			{
				weapon.ShootOccured -= OnShoot;
			}
		}

		private void OnShoot()
		{
			_weaponAnimator.SetTrigger(ShotTriggerName);
		}
	}
}