using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	[SerializeField] private float _speed = 2;

	private Vector3 _translation;

	public void SetTranslation(Vector3 translation)
	{
		_translation = translation;
	}

	private void Update()
	{
		Move(_translation);
	}

	private void Move(Vector3 translation)
	{
		transform.position += translation * (Time.deltaTime * _speed);
	}

	public void GetMoveInfo(out Vector3 position)
	{
		position = transform.position;
	}
}