using System.Collections;
using UnityEngine;

namespace Weapon
{
	public class Bullet : MonoBehaviour
	{
		[SerializeField] private Rigidbody _rigidbody;
		[SerializeField] private float _lifeTime = 3f;

		public void Init(Vector3 velocity)
		{
			_rigidbody.velocity = velocity;
			StartCoroutine(DelayDestroy());
		}

		private void OnCollisionEnter(Collision other)
		{
			DestroyBullet();
		}

		private IEnumerator DelayDestroy()
		{
			yield return new WaitForSecondsRealtime(_lifeTime);
		}

		private void DestroyBullet()
		{
			Destroy(gameObject);
		}
	}
}