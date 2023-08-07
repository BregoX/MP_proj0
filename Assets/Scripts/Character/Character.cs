using UnityEngine;

namespace Character
{
	public class Character : MonoBehaviour
	{
		[SerializeField] protected Health Health;

		[field: SerializeField] public float Speed { get; protected set; } = 2f;
		[field: SerializeField] public int MaxHealth { get; protected set; } = 10;

		public Vector3 Velocity { get; protected set; }
		public bool IsSit { get; protected set; }
	}
}