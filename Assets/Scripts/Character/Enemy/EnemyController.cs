using System;
using System.Collections.Generic;
using System.Linq;
using Character.Enemy;
using Colyseus.Schema;
using DefaultNamespace;
using Generated;
using Multiplayer;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	[SerializeField] private EnemyCharacter _character;
	[SerializeField] private EnemyWeaponArmory _enemyWeaponArmory;

	public event Action<int> LossCountChanged;
	
	private readonly List<float> _receiveTimeInterval = new() { 0, 0, 0, 0, 0 };

	private float _lastReceivedTime;
	private Player _player;
	private string _sessionId;

	private MultiplayerManager MultiplayerManager => MultiplayerManager.Instance;

	public void Init(string key, Player player)
	{
		_sessionId = key;
		_player = player;

		_character.SetSpeed(_player.speed);
		_character.SetMaxHealth(_player.mHP);
		_character.DamageTaken += OnDamageTaken;

		_player.OnChange += OnChange;
	}

	private void OnDamageTaken(int damage)
	{
		MultiplayerManager.SendDamageInfo(_sessionId, damage);
	}

	public void Destroy()
	{
		_player.OnChange -= OnChange;
		
		Destroy(gameObject);
	}

	public void Shoot(in ShotInfo shotInfo)
	{
		var position = new Vector3(shotInfo.pX, shotInfo.pY, shotInfo.pZ);
		var velocity = new Vector3(shotInfo.dX, shotInfo.dY, shotInfo.dZ);

		_enemyWeaponArmory.Shoot(position, velocity);
	}

	public void ChangeWeapon(in WeaponInfo weaponInfo)
	{
		_enemyWeaponArmory.ChangeWeapon(weaponInfo.i);
	}

	private void UpdateReceiveIntervals()
	{
		var time = Time.time;
		var delta = time - _lastReceivedTime;
		_receiveTimeInterval.Add(delta);
		_receiveTimeInterval.RemoveAt(0);
		_lastReceivedTime = time;
	}

	private void OnChange(List<DataChange> changes)
	{
		UpdateReceiveIntervals();
		var position = _character.TargetPosition;
		var velocity = _character.Velocity;
		float? rotateX = null;
		float? rotateY = null;
		bool? isSit = null;
		int? currentHP = null;
		int? lossCount = null;

		foreach (var change in changes)
		{
			switch (change.Field)
			{
				case "cHP":
					var previous = (sbyte)change.PreviousValue;
					var value = (sbyte)change.Value;

					currentHP = value > previous ? value : null;
					break;
				case "pX":
					position.x = (float)change.Value;
					break;
				case "pY":
					position.y = (float)change.Value;
					break;
				case "pZ":
					position.z = (float)change.Value;
					break;
				case "vX":
					velocity.x = (float)change.Value;
					break;
				case "vY":
					velocity.y = (float)change.Value;
					break;
				case "vZ":
					velocity.z = (float)change.Value;
					break;
				case "rX":
					rotateX = (float)change.Value;
					break;
				case "rY":
					rotateY = (float)change.Value;
					break;
				case "sit":
					isSit = (bool)change.Value;
					break;
				case "loss":
					lossCount = (byte)change.Value;
					break;
				default:
					Debug.Log($"Unknown field {change.Field} with value {change.Value}");
					break;
			}
		}

		_character.SetupMoveInfo(position, velocity, _receiveTimeInterval.Average(), rotateX, rotateY, isSit);
		_character.SetupHealth(currentHP);

		if (lossCount.HasValue)
		{
			LossCountChanged?.Invoke(lossCount.Value);
		}
	}

	private void OnDestroy()
	{
		_player.OnChange -= OnChange;
	}
}