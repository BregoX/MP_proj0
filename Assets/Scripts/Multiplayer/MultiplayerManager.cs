using System.Collections.Generic;
using Character.Player;
using Colyseus;
using Generated;
using UnityEngine;

namespace Multiplayer
{
	public class MultiplayerManager : ColyseusManager<MultiplayerManager>
	{
		private const string StateHandlerEndpoint = "state_handler";
		private const string MoveEndpoint = "move";

		[SerializeField] private PlayerCharacter _playerPrefab;
		[SerializeField] private EnemyController _enemyController;

		private ColyseusRoom<State> _room;

		public void UpdateRemotePosition(Dictionary<string, object> message)
		{
			SendMessage(MoveEndpoint, message);
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
			var data = new Dictionary<string, object> { { "speed", _playerPrefab.Speed } };

			_room = await Instance.client.JoinOrCreate<State>(StateHandlerEndpoint, data);
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
			enemy.Init(player);
			
		}

		private void SendMessage(string key, Dictionary<string, object> message)
		{
			_room.Send(key, message);
		}

		private static Vector3 GetPosition(Player player)
		{
			return new Vector3(player.pX, player.pY, player.pZ);
		}
	}
}