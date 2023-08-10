using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
	public abstract class WeaponArmory<T> : MonoBehaviour where T : Gun
	{
		[SerializeField] private List<T> _weapons;
		[SerializeField] private WeaponsProvider _weaponsProvider;

		public int ActiveWeaponIndex { get; private set; }

		protected T ActiveGun => _weapons[ActiveWeaponIndex];

		public bool TrySwitchToNext()
		{
			return TryChangeWeapon(ActiveWeaponIndex + 1);
		}

		protected bool TryChangeWeapon(int weaponIndex)
		{
			var nextIndex = weaponIndex;

			if (nextIndex >= _weapons.Count)
			{
				nextIndex = 0;
			}

			if (nextIndex != ActiveWeaponIndex)
			{
				ActiveGun.Hide();
				ActiveWeaponIndex = nextIndex;
				ActiveGun.Show();
				return true;
			}

			return false;
		}

		private void Awake()
		{
			_weaponsProvider.SetupWeapons(_weapons);
		}
	}
}