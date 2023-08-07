using TMPro;
using UnityEngine;

public class LossCounter : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _lossCounterText;

	private int _myLoses;
	private int _oponentsLoses;


	public void SetupMyLoses(int loses)
	{
		_myLoses = loses;
		UpdateUI();
	}

	public void SetupOpponentLoses(int loses)
	{
		_oponentsLoses = loses;
		UpdateUI();
	}

	private void Awake()
	{
		UpdateUI();
	}

	private void UpdateUI()
	{
		_lossCounterText.SetText($"{_myLoses} : {_oponentsLoses}");
	}
}