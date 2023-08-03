using System;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	[SerializeField] private float _speed = 2;
	[SerializeField] private Rigidbody _rigidbody;
	[SerializeField] private Transform _head;
	[SerializeField] private Transform _cameraHolder;

	private float _inputX;
	private float _inputZ;
	private float _rotateY;

	private void Start()
	{
		var camera = Camera.main.transform;
		camera.parent = _cameraHolder;
		camera.localRotation = Quaternion.identity;
		camera.localPosition = Vector3.zero;
		
	}

	public void SetInput(float inputX, float inputZ, float rotationY)
	{
		_inputX = inputX;
		_inputZ = inputZ;
		_rotateY += rotationY;
	}

	public void RotateX(float value)
	{
		_head.Rotate(value, 0, 0);
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
		_rigidbody.velocity = (transform.forward * _inputZ + transform.right * _inputX).normalized * _speed;
	}

	public void GetMoveInfo(out Vector3 position, out Vector3 velocity)
	{
		position = transform.position;
		velocity = _rigidbody.velocity;
	}
}