using UnityEngine;

namespace Character.Player
{
	public class PlayerCharacter : Character
	{
		[SerializeField] private Rigidbody _rigidbody;
		[SerializeField] private Transform _head;
		[SerializeField] private Transform _cameraHolder;
		[SerializeField] private float _minAngle = -45;
		[SerializeField] private float _maxAngle = 45;
		[SerializeField] private float _jumpForce = 5;
		[SerializeField] private CheckFly _checkFly;
		[SerializeField] private float _jumpDelay;

		private float _inputX;
		private float _inputZ;
		private float _rotateY;
		private float _currentRotationY;
		private float _jumpTime;

		private void Start()
		{
			var camera = Camera.main.transform;
			camera.parent = _cameraHolder;
			camera.localPosition = Vector3.zero;
			camera.localRotation = Quaternion.identity;
		}

		public void SetInput(
			float inputX,
			float inputZ,
			float rotationY,
			bool isSit)
		{
			_inputX = inputX;
			_inputZ = inputZ;
			_rotateY += rotationY;
			IsSit = isSit;
		}

		public void RotateX(float value)
		{
			_currentRotationY = Mathf.Clamp(_currentRotationY + value, _minAngle, _maxAngle);
			_head.localEulerAngles = new Vector3(_currentRotationY, 0, 0);
		}

		public void Jump()
		{
			if (_checkFly.IsFly || (Time.time - _jumpTime) < _jumpDelay) return;

			_jumpTime = Time.time;
			_rigidbody.AddForce(0, _jumpForce, 0, ForceMode.VelocityChange);
		}

		public void GetMoveInfo(
			out Vector3 position,
			out Vector3 velocity,
			out float rotateX,
			out float rotateY,
			out bool isSit)
		{
			position = transform.position;
			velocity = _rigidbody.velocity;

			rotateX = _head.localEulerAngles.x;
			rotateY = transform.localEulerAngles.y;
			isSit = IsSit;
		}

		public void SetupRestartPosition(float positionX, float positionZ)
		{
			SetInput(0, 0, 0, false);

			_rigidbody.velocity = Vector3.zero;
			transform.position = new Vector3(positionX, 0, positionZ);
		}

		private void FixedUpdate()
		{
			Move();
			RotateY();
		}

		private void RotateY()
		{
			_rigidbody.angularVelocity = new Vector3(0, _rotateY, 0);
			_rotateY = 0;
		}

		private void Move()
		{
			var newVelocity = (transform.forward * _inputZ + transform.right * _inputX).normalized * Speed;
			newVelocity.y = _rigidbody.velocity.y;
			Velocity = newVelocity;

			_rigidbody.velocity = Velocity;
		}

		public void UpdateHealth(int? currentHp)
		{
			if (!currentHp.HasValue) return;

			Health.SetCurrent(currentHp.Value);
		}
	}
}