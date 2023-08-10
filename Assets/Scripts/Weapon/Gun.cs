using System;
using UnityEngine;

namespace Weapon
{
	public class Gun : MonoBehaviour
	{
		[SerializeField] protected Bullet Bullet;

		public Action ShootOccured;

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		public void Show()
		{
			gameObject.SetActive(true);
		}
	}
}