using UnityEngine;

namespace Character
{
	public class Health : MonoBehaviour
	{
		[SerializeField] private int _current;
		[SerializeField] private int _max;

		[SerializeField] private HealthUI _healthUI;

		public void SetMax(int max)
		{
			_max = max;
			UpdateUI();
		}

		public void SetCurrent(int current)
		{
			_current = current;
			UpdateUI();
		}

		public void ApplyDamage(int damage)
		{
			_current -= damage;
			if (_current <= 0)
			{
				_current = 0;
			}

			UpdateUI();
		}

		private void UpdateUI()
		{
			_healthUI.UpdateHealth(_current, _max);
		}
	}
}