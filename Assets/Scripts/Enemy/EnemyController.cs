using System.Collections.Generic;
using System.Linq;
using Colyseus.Schema;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	[SerializeField] private EnemyCharacter _character;
	private readonly List<float> _receiveTimeInterval = new() { 0, 0, 0, 0, 0 };

	private float _lastReceivedTime;

	private void UpdateReceiveIntervals()
	{
		var time = Time.time;
		var delta = time - _lastReceivedTime;
		_receiveTimeInterval.Add(delta);
		_receiveTimeInterval.RemoveAt(0);
		_lastReceivedTime = time;
	}

	public void OnChange(List<DataChange> changes)
	{
		UpdateReceiveIntervals();
		var position = _character.TargetPosition;
		var velocity = Vector3.zero;

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
				default:
					Debug.Log($"Unknown field {change.Field} with value {change.Value}");
					break;
			}
		}

		_character.SetupMoveInfo(position, velocity, _receiveTimeInterval.Average());
	}
}