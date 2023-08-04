using UnityEngine;

namespace Character
{
	public class CheckFly : MonoBehaviour
	{
		[SerializeField] private float _radius = 0.15f;
		[SerializeField] private LayerMask _ground;
		[SerializeField] private float _coyoteTime = 0.2f;

		private float _flyTimer;

		public bool IsFly { get; private set; }

		private void Update()
		{
			if (!Physics.CheckSphere(transform.position, _radius, _ground))
			{
				if (_flyTimer >= _coyoteTime)
				{
					IsFly = true;
				}
				else
				{
					_flyTimer += Time.deltaTime;
				}
			}
			else
			{
				IsFly = false;
				_flyTimer = 0;
			}
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere(transform.position, _radius);
		}
#endif
	}
}