using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	[SerializeField] private float _speed = 2;
	[SerializeField] private Rigidbody _rigidbody;
	[SerializeField] private Transform _head;

	private Vector3 _velocity;

	public void SetVelocity(Vector3 velocity)
	{
		_velocity = velocity;
	}

	private void FixedUpdate()
	{
		Move(_velocity);
	}

	private void Move(Vector3 velocity)
	{
		_rigidbody.velocity = (transform.forward * velocity.z + transform.right * velocity.x).normalized * _speed;
	}

	public void GetMoveInfo(out Vector3 position, out Vector3 velocity)
	{
		position = transform.position;
		velocity = _rigidbody.velocity;
	}
}