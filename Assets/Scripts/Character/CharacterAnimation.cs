using System;
using UnityEngine;

namespace Character
{
	public class CharacterAnimation : MonoBehaviour
	{
		private const string Grounded = "Grounded";
		private const string Speed = "Speed";

		[SerializeField] private CheckFly _checkFly;
		[SerializeField] private Animator _animator;
		[SerializeField] private Character _character;

		private void Update()
		{
			var localVelocity = _character.transform.InverseTransformVector(_character.Velocity);
			var speed = localVelocity.magnitude / _character.Speed * Mathf.Sign(localVelocity.z);
			
			_animator.SetBool(Grounded, !_checkFly.IsFly);
			_animator.SetFloat(Speed, speed);
		}
	}
}