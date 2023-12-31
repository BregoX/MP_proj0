using System;
using System.Collections;
using System.Collections.Generic;
using Character.Player;
using Colyseus.Schema;
using DefaultNamespace;
using Generated;
using Multiplayer;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private PlayerCharacter _playerCharacter;

	[SerializeField] private float _mouseSensitivity = 2f;

	// [SerializeField] private PlayerGun _playerGun;
	[SerializeField] private PlayerWeaponArmory _weaponArmory;
	[SerializeField] private float _restartDelay = 3f;

	public event Action<int> LossCountChanged;

	private Player _player;

	private MultiplayerManager MultiplayerManager => MultiplayerManager.Instance;
	private bool _isHold;
	public float Speed => _playerCharacter.Speed;
	public float MaxHealth => _playerCharacter.MaxHealth;

	public void Init(Player player)
	{
		_player = player;
		_player.OnChange += OnChange;
	}

	private void OnChange(List<DataChange> changes)
	{
		int? currentHP = null;
		int? lossCount = null;

		foreach (var change in changes)
		{
			switch (change.Field)
			{
				case "cHP":
					currentHP = (sbyte)change.Value;
					break;
				case "loss":
					lossCount = (byte)change.Value;
					break;

				default:
					Debug.Log($"Unknown field {change.Field} with value {change.Value}");
					break;
			}
		}

		_playerCharacter.UpdateHealth(currentHP);

		if (lossCount.HasValue)
		{
			LossCountChanged?.Invoke(lossCount.Value);
		}
	}


	private void Update()
	{
		if (_isHold)
		{
			SendMove();
			return;
		}

		var h = Input.GetAxisRaw("Horizontal");
		var v = Input.GetAxisRaw("Vertical");

		var mX = Input.GetAxis("Mouse X");
		var my = Input.GetAxis("Mouse Y");
		var isJump = Input.GetKeyDown(KeyCode.Space);
		var isShoot = Input.GetMouseButton(0);
		var isSit = Input.GetKey(KeyCode.LeftShift);
		var isSwitchWeapon = Input.GetKeyDown(KeyCode.Q);

		_playerCharacter.SetInput(h, v, mX * _mouseSensitivity, isSit);
		_playerCharacter.RotateX(-my * _mouseSensitivity);

		if (isJump)
		{
			_playerCharacter.Jump();
		}

		if (isShoot && _weaponArmory.TryShoot(out var shotInfo))
		{
			MultiplayerManager.SendShotInfo(ref shotInfo);
		}

		if (isSwitchWeapon && _weaponArmory.TrySwitchToNext())
		{
			MultiplayerManager.SendWeaponInfo(_weaponArmory.ActiveWeaponIndex);
		}


		SendMove();
	}

	private void SendMove()
	{
		_playerCharacter.GetMoveInfo(out var position,
			out var velocity,
			out var rotateX,
			out var rotateY,
			out var isSit);

		MultiplayerManager.UpdateRemotePosition(new Dictionary<string, object>
		{
			{ "pX", position.x },
			{ "pY", position.y },
			{ "pZ", position.z },
			{ "vX", velocity.x },
			{ "vY", velocity.y },
			{ "vZ", velocity.z },
			{ "rX", rotateX },
			{ "rY", rotateY },
			{ "sit", isSit }
		});
	}

	private void OnDestroy()
	{
		_player.OnChange -= OnChange;
	}

	public void Restart(RestartInfo restartInfo)
	{
		_playerCharacter.SetupRestartPosition(restartInfo.pX, restartInfo.pZ);
		StartCoroutine(Hold());
	}

	private IEnumerator Hold()
	{
		_isHold = true;
		yield return new WaitForSecondsRealtime(_restartDelay);
		_isHold = false;
	}
}