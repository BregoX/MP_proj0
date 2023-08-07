using System.Collections;
using Character.Enemy;
using UnityEngine;

namespace Weapon
{
	public class Bullet : MonoBehaviour
	{
		[SerializeField] private Rigidbody _rigidbody;
		[SerializeField] private float _lifeTime = 3f;

		private int _damage;

		public void Init(Vector3 velocity, int damage = 0)
		{
			_rigidbody.velocity = velocity;
			_damage = damage;
			StartCoroutine(DelayDestroy());
		}

		private void OnCollisionEnter(Collision other)
		{
			if (other.collider.TryGetComponent<EnemyCharacter>(out var enemyCharacter))
			{
				enemyCharacter.ApplyDamage(_damage);
			}

			DestroyBullet();
		}

		private IEnumerator DelayDestroy()
		{
			yield return new WaitForSecondsRealtime(_lifeTime);
			DestroyBullet();
		}

		private void DestroyBullet()
		{
			Destroy(gameObject);
		}
	}
}