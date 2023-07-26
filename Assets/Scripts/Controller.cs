using UnityEngine;

public class Controller : MonoBehaviour
{
	[SerializeField] private PlayerCharacter _playerCharacter;

	private void Update()
	{
		var h = Input.GetAxis("Horizontal");
		var v = Input.GetAxis("Vertical");

		_playerCharacter.SetDirection(new Vector3(h, 0, v));
	}
}