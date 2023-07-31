using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
	[SerializeField] private PlayerCharacter _playerCharacter;

	private void Update()
	{
		var h = Input.GetAxis("Horizontal");
		var v = Input.GetAxis("Vertical");

		_playerCharacter.SetVelocity(new Vector3(h, 0, v));

		SendMove();
	}

	private void SendMove()
	{
		_playerCharacter.GetMoveInfo(out var position, out var velocity);
		MultiplayerManager.Instance.UpdateRemotePosition(new Dictionary<string, object>
		{
			{ "pX", position.x },
			{ "pY", position.y },
			{ "pZ", position.z },
			{ "vX", velocity.x },
			{ "vY", velocity.y },
			{ "vZ", velocity.z }
		});
	}
}