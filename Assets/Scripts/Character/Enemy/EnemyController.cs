using System.Collections.Generic;
using System.Linq;
using Character.Enemy;
using Colyseus.Schema;
using DefaultNamespace;
using Generated;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	[SerializeField] private EnemyCharacter _character;
	[SerializeField] private EnemyGun _enemyGun;

	private readonly List<float> _receiveTimeInterval = new() { 0, 0, 0, 0, 0 };

	private float _lastReceivedTime;
	private Player _player;

	public void Init(Player player)
	{
		_player = player;

		_player.OnChange += OnChange;
		_character.SetSpeed(player.speed);
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
		
		_enemyGun.Shoot(position, velocity);
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

		foreach (var change in changes)
		{
			switch (change.Field)
			{
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
				default:
					Debug.Log($"Unknown field {change.Field} with value {change.Value}");
					break;
			}
		}

		_character.SetupMoveInfo(position, velocity, _receiveTimeInterval.Average(), rotateX, rotateY);
	}

	private void OnDestroy()
	{
		_player.OnChange += OnChange;
	}
}