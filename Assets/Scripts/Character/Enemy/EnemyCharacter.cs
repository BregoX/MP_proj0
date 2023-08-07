using UnityEngine;

namespace Character.Enemy
{
	public class EnemyCharacter : Character
	{
		[SerializeField] private Transform _head;
		[SerializeField] private Health _health;

		public Vector3 TargetPosition { get; private set; }

		private float _velocityMagnitude;

		public void SetSpeed(float speed)
		{
			Speed = speed;
		}

		public void SetMaxHealth(int health)
		{
			MaxHealth = health;

			_health.SetMax(health);
			_health.SetCurrent(health);
		}

		public void ApplyDamage(int damage)
		{
			_health.ApplyDamage(damage);
		}

		public void SetupMoveInfo(
			in Vector3 position,
			in Vector3 velocity,
			in float averageTime,
			in float? rotateX,
			in float? rotateY,
			in bool? isSit)
		{
			var pos = position + velocity * averageTime;

			TargetPosition = pos;
			_velocityMagnitude = velocity.magnitude;
			Velocity = velocity;

			if (rotateX.HasValue)
			{
				var eulerAngles = _head.localEulerAngles;
				_head.localEulerAngles = new Vector3(rotateX.Value, eulerAngles.y, eulerAngles.z);
			}

			if (rotateY.HasValue)
			{
				var eulerAngles = transform.localEulerAngles;
				transform.localEulerAngles = new Vector3(eulerAngles.x, rotateY.Value, eulerAngles.z);
			}

			if (isSit.HasValue)
			{
				IsSit = isSit.Value;
			}
		}

		private void Start()
		{
			TargetPosition = transform.position;
		}

		private void Update()
		{
			if (_velocityMagnitude > 0.1)
			{
				var maxDistance = _velocityMagnitude * Time.deltaTime;
				transform.position = Vector3.MoveTowards(transform.position, TargetPosition, maxDistance);
			}
			else
			{
				transform.position = TargetPosition;
			}
		}
	}
}