using System;
using UnityEngine;

namespace Weapon
{
	public abstract class Gun : MonoBehaviour
	{
		[SerializeField] protected Bullet Bullet;

		public Action ShootOccured;
	}
}