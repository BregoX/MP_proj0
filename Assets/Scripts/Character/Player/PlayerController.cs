using System.Collections.Generic;
using Character.Player;
using DefaultNamespace;
using Multiplayer;
using UnityEngine;
using Weapon;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private PlayerCharacter _playerCharacter;
	[SerializeField] private float _mouseSensitivity = 2f;
	[SerializeField] private PlayerGun _playerGun;

	private MultiplayerManager MultiplayerManager => MultiplayerManager.Instance;

	private void Update()
	{
		var h = Input.GetAxisRaw("Horizontal");
		var v = Input.GetAxisRaw("Vertical");

		var mX = Input.GetAxis("Mouse X");
		var my = Input.GetAxis("Mouse Y");
		var isJump = Input.GetKeyDown(KeyCode.Space);
		var isShoot = Input.GetMouseButton(0);

		_playerCharacter.SetInput(h, v, mX * _mouseSensitivity);
		_playerCharacter.RotateX(-my * _mouseSensitivity);

		if (isJump)
		{
			_playerCharacter.Jump();
		}

		if (isShoot && _playerGun.TryShoot(out var shotInfo))
		{
			MultiplayerManager.SendShotInfo(ref shotInfo);
		}

		SendMove();
	}

	private void SendMove()
	{
		_playerCharacter.GetMoveInfo(out var position, out var velocity, out float rotateX, out float rotateY);
		MultiplayerManager.UpdateRemotePosition(new Dictionary<string, object>
		{
			{ "pX", position.x },
			{ "pY", position.y },
			{ "pZ", position.z },
			{ "vX", velocity.x },
			{ "vY", velocity.y },
			{ "vZ", velocity.z },
			{ "rX", rotateX },
			{ "rY", rotateY }
		});
	}

	private void SendShot(ref ShotInfo shotInfo)
	{
		MultiplayerManager.SendShotInfo(ref shotInfo);
	}
}