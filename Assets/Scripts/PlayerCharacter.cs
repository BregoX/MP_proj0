using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	[SerializeField] private float _speed = 2;

	private Vector3 _direction;

	public void SetDirection(Vector3 direction)
	{
		_direction = direction;
	}

	private void Update()
	{
		Move(_direction);
	}

	private void Move(Vector3 direction)
	{
		transform.position += direction * (Time.deltaTime * _speed);
	}
}