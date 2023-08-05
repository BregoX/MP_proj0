using UnityEngine;

namespace Character
{
	public class Character : MonoBehaviour
	{
		[field: SerializeField] public float Speed { get; protected set; } = 2f;
		public Vector3 Velocity { get; protected set; }
		public bool IsSit { get; protected set; }
	}
}