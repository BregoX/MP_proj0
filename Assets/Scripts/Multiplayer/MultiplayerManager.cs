using System.Collections.Generic;
using Colyseus;
using Generated;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
	[SerializeField] private GameObject _playerPrefab;
	[SerializeField] private EnemyController _enemyController;

	private ColyseusRoom<State> _room;

	public void UpdateRemotePosition(Dictionary<string, object> message)
	{
		SendMessage("move", message);
	}

	protected override void Awake()
	{
		base.Awake();

		Instance.InitializeClient();
		Connect();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		_room.State.players.OnAdd -= OnAddPlayer;
		_room.State.players.OnRemove -= OnRemovePlayer;
		_room.Leave();
	}

	private async void Connect()
	{
		_room = await Instance.client.JoinOrCreate<State>("state_handler");
		_room.OnStateChange += OnStateChange;
	}

	private void OnStateChange(State state, bool isFirstState)
	{
		if (!isFirstState)
		{
			return;
		}

		state.players.ForEach(InitializePlayer);
		_room.State.players.OnAdd += OnAddPlayer;
		_room.State.players.OnRemove += OnRemovePlayer;
	}

	private void OnAddPlayer(string key, Player player)
	{
		InitializeEnemy(player);
	}

	private void OnRemovePlayer(string key, Player value)
	{
	}

	private void InitializePlayer(string key, Player player)
	{
		if (key == _room.SessionId)
		{
			Instantiate(_playerPrefab, GetPosition(player), Quaternion.identity);
		}
		else
		{
			InitializeEnemy(player);
		}
	}

	private void InitializeEnemy(Player player)
	{
		var enemy = Instantiate(_enemyController, GetPosition(player), Quaternion.identity);
		player.OnChange += enemy.OnChange;
	}

	private void SendMessage(string key, Dictionary<string, object> message)
	{
		_room.Send(key, message);
	}

	private static Vector3 GetPosition(Player player)
	{
		return new Vector3(player.x, 0, player.y);
	}
}