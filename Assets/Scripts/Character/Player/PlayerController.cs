using System.Collections.Generic;
using Character.Player;
using Multiplayer;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private PlayerCharacter _playerCharacter;
	[SerializeField] private float _mouseSensitivity = 2f;

	private void Update()
	{
		var h = Input.GetAxisRaw("Horizontal");
		var v = Input.GetAxisRaw("Vertical");

		var mX = Input.GetAxis("Mouse X");
		var my = Input.GetAxis("Mouse Y");
		var isJump = Input.GetKeyDown(KeyCode.Space);

		_playerCharacter.SetInput(h, v, mX * _mouseSensitivity);
		_playerCharacter.RotateX(-my * _mouseSensitivity);

		if (isJump)
		{
			_playerCharacter.Jump();
		}

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