using System.Collections.Generic;
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
		private const string WeaponEndpoint = "weapon";
		private const string ShootMessageFromServer = "SHOOT";
		private const string WeaponMessageFromServer = "WEAPON";
		private const string RestartMessageFromServer = "Restart";
		private const string DamageMessage = "damage";

		[SerializeField] private PlayerController _playerControllerPrototype;
		[SerializeField] private EnemyController _enemyControllerPrototype;
		[SerializeField] private LossCounter _lossCounter;

		private Dictionary<string, EnemyController> _enemys = new();
		private PlayerController _playerController;

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

		public void SendWeaponInfo(int weaponIndex)
		{
			var weaponInfo = new WeaponInfo { i = weaponIndex, key = _room.SessionId };
			var message = JsonUtility.ToJson(weaponInfo);
			SendMessage(WeaponEndpoint, message);
		}

		public void SendDamageInfo(string enemySessionId, int damage)
		{
			var data = new Dictionary<string, object>()
			{
				{ "enemySessionId", enemySessionId },
				{ "value", damage }
			};

			SendMessage(DamageMessage, data);
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
				{ "speed", _playerControllerPrototype.Speed },
				{ "mHP", _playerControllerPrototype.MaxHealth }
			};

			_room = await Instance.client.JoinOrCreate<State>(StateHandlerEndpoint, data);
			_room.OnStateChange += OnStateChange;

			_room.OnMessage<string>(ShootMessageFromServer, OnShootReceive);
			_room.OnMessage<string>(RestartMessageFromServer, OnRestartReceive);
			_room.OnMessage<string>(WeaponMessageFromServer, OnWeaponReceive);
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

		private void OnRestartReceive(string restartMessage)
		{
			var restartInfo = JsonUtility.FromJson<RestartInfo>(restartMessage);
			_playerController.Restart(restartInfo);
		}

		private void OnWeaponReceive(string weaponMessage)
		{
			var weaponInfo = JsonUtility.FromJson<WeaponInfo>(weaponMessage);

			if (_enemys.TryGetValue(weaponInfo.key, out var enemy))
			{
				enemy.ChangeWeapon(weaponInfo);
			}
			else
			{
				Debug.Log("No enemy on change weapon " + weaponMessage);
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
			InitializeEnemyController(key, player);
		}

		private void OnRemovePlayer(string key, Player value)
		{
			if (_enemys.ContainsKey(key))
			{
				var enemy = _enemys[key];
				enemy.Destroy();
				enemy.LossCountChanged -= OnEnemyLossCountChanged;
				_enemys.Remove(key);
			}
		}

		private void InitializePlayer(string key, Player player)
		{
			if (key == _room.SessionId)
			{
				InitializePlayerController(player);
			}
			else
			{
				InitializeEnemyController(key, player);
			}
		}

		private void InitializePlayerController(Player player)
		{
			_playerController = Instantiate(_playerControllerPrototype, GetPosition(player), Quaternion.identity);
			_playerController.Init(player);
			_playerController.LossCountChanged += OnPlayerLossCountChanged;
		}

		private void OnPlayerLossCountChanged(int lossCount)
		{
			_lossCounter.SetupMyLoses(lossCount);
		}

		private void OnEnemyLossCountChanged(int lossCount)
		{
			_lossCounter.SetupOpponentLoses(lossCount);
		}

		private void InitializeEnemyController(string key, Player player)
		{
			var enemy = Instantiate(_enemyControllerPrototype, GetPosition(player), Quaternion.identity);
			enemy.Init(key, player);
			enemy.LossCountChanged += OnEnemyLossCountChanged;
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