using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
	public class WeaponsProvider : MonoBehaviour
	{
		public IReadOnlyList<Gun> Weapons { get; private set; }

		public void SetupWeapons(IReadOnlyList<Gun> weapons)
		{
			Weapons = weapons;
		}
	}
}