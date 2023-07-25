using UnityEngine;

public class Controller : MonoBehaviour
{
	[SerializeField] private Player _player;

	private void Update()
	{
		var h = Input.GetAxis("Horizontal");
		var v = Input.GetAxis("Vertical");

		_player.SetDirection(new Vector3(h, 0, v));
	}
}