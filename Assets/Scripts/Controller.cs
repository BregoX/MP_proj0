using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
	[SerializeField] private PlayerCharacter _playerCharacter;

	private void Update()
	{
		var h = Input.GetAxis("Horizontal");
		var v = Input.GetAxis("Vertical");

		_playerCharacter.SetTranslation(new Vector3(h, 0, v));

		SendMove();
	}

	private void SendMove()
	{
		_playerCharacter.GetMoveInfo(out var position);
		MultiplayerManager.Instance.UpdateRemotePosition(new Dictionary<string, object>
		{
			{ "x", position.x },
			{ "y", position.z },
		});
	}
}