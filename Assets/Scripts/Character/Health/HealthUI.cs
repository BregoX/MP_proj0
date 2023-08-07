using UnityEngine;

namespace Character
{
	public class HealthUI : MonoBehaviour
	{
		[SerializeField] private RectTransform _healthImage;

		private float _defaultWidth;

		private void Awake()
		{
			_defaultWidth = _healthImage.sizeDelta.x;
		}

		public void UpdateHealth(int current, int max)
		{
			var progress = (float)current / max;
			_healthImage.sizeDelta = new Vector2(_defaultWidth * progress, _healthImage.sizeDelta.y);
		}
	}
}