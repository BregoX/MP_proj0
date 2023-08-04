using UnityEngine;

namespace Character.Enemy
{
	public class EnemyCharacter : Character
	{
		public Vector3 TargetPosition { get; private set; }

		private float _velocityMagnitude;

		public void SetSpeed(float speed)
		{
			Speed = speed;
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

		public void SetupMoveInfo(in Vector3 position, in Vector3 velocity, in float averageTime)
		{
			var pos = position + velocity * averageTime;

			TargetPosition = pos;
			_velocityMagnitude = velocity.magnitude;
			Velocity = velocity;
		}
	}
}