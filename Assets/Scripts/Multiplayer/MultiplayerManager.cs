using Colyseus;
using Generated;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
	[SerializeField] private GameObject _playerPrefab;
	[SerializeField] private GameObject _enemyPrefab;

	private ColyseusRoom<State> _room;

	protected override void Awake()
	{
		base.Awake();

		Instance.InitializeClient();
		Connect();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

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

		var player = state.players[_room.SessionId];
		InstantiatePrefab(_playerPrefab, player);

		state.players.ForEach(InitializeEnemy);
	}

	private Vector3 GetPosition(Player player)
	{
		return new Vector3(player.x - 200, 0, player.y - 200) / 40;
	}

	private void InitializeEnemy(string key, Player player)
	{
		if (key == _room.SessionId) return;
		InstantiatePrefab(_enemyPrefab, player);
	}

	private void InstantiatePrefab(GameObject prefab, Player player)
	{
		Instantiate(prefab, GetPosition(player), Quaternion.identity);
	}
}