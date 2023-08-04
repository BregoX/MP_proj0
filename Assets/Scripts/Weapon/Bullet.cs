using UnityEngine;

namespace Weapon
{
	public class Bullet : MonoBehaviour
	{
		[SerializeField] private Rigidbody _rigidbody;

		public void Init(Vector3 direction, float speed)
		{
			_rigidbody.velocity = direction * speed;
			Destroy(gameObject, 3f);
		}
	}
}