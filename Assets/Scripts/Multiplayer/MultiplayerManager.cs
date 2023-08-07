using System.Collections.Generic;
using Character.Player;
using Colyseus;
using DefaultNamespace;
using Generated;
using UnityEngine;

namespace Multiplayer
{
	public class MultiplayerManager : ColyseusManager<MultiplayerManager>
	{
		private const string StateHandlerEndpoint = "state_handler";
		private const string MoveEndpoint = "move";
		private const string ShootEndpoint = "shoot";
		private const string ShootMessageFromServer = "SHOOT";

		[SerializeField] private PlayerCharacter _playerPrefab;
		[SerializeField] private EnemyController _enemyController;

		private Dictionary<string, EnemyController> _enemys = new();

		private ColyseusRoom<State> _room;

		public void UpdateRemotePosition(Dictionary<string, object> message)
		{
			SendMessage(MoveEndpoint, message);
		}

		public void SendShotInfo(ref ShotInfo shotInfo)
		{
			shotInfo.key = _room.SessionId;
			var message = JsonUtility.ToJson(shotInfo);
			SendMessage(ShootEndpoint, message);
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
			var data = new Dictionary<string, object>
			{
				{ "speed", _playerPrefab.Speed },
				{ "hp", _playerPrefab.MaxHealth },
			};

			_room = await Instance.client.JoinOrCreate<State>(StateHandlerEndpoint, data);
			_room.OnStateChange += OnStateChange;

			_room.OnMessage<string>(ShootMessageFromServer, OnShootReceive);
		}

		private void OnShootReceive(string shotInfoMessage)
		{
			var shotInfo = JsonUtility.FromJson<ShotInfo>(shotInfoMessage);

			if (_enemys.TryGetValue(shotInfo.key, out var enemy))
			{
				enemy.Shoot(shotInfo);
			}
			else
			{
				Debug.Log("No enemy on shoot " + shotInfoMessage);
			}
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
			InitializeEnemy(key, player);
		}

		private void OnRemovePlayer(string key, Player value)
		{
			if (_enemys.ContainsKey(key))
			{
				var enemy = _enemys[key];
				enemy.Destroy();

				_enemys.Remove(key);
			}
		}

		private void InitializePlayer(string key, Player player)
		{
			if (key == _room.SessionId)
			{
				Instantiate(_playerPrefab, GetPosition(player), Quaternion.identity);
			}
			else
			{
				InitializeEnemy(key, player);
			}
		}

		private void InitializeEnemy(string key, Player player)
		{
			var enemy = Instantiate(_enemyController, GetPosition(player), Quaternion.identity);
			enemy.Init(player);
			_enemys.Add(key, enemy);
		}

		private void SendMessage(string key, Dictionary<string, object> message)
		{
			_room.Send(key, message);
		}

		private void SendMessage(string key, string message)
		{
			_room.Send(key, message);
		}

		private static Vector3 GetPosition(Player player)
		{
			return new Vector3(player.pX, player.pY, player.pZ);
		}
	}
}