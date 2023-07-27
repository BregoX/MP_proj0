using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	[SerializeField] private EnemyCharacter _character;

	public void OnChange(List<DataChange> changes)
	{
		var position = transform.position;
		foreach (var change in changes)
		{
			switch (change.Field)
			{
				case "x":
					position.x = (float)change.Value;
					break;
				case "y":
					position.z = (float)change.Value;
					break;
				default:
					Debug.Log($"Unknown field {change.Field} with value {change.Value}");
					break;
			}
		}

		_character.SetPosition(position);
	}
}