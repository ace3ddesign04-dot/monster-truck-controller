using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
	private Slider slider;

	public float step;

	public Text valueDisplayText;

	public int digitsAfterPoint = 1;

	private void Start()
	{
		slider = GetComponent<Slider>();
		UpdateValueDisplay();
	}

	public void IncreaseValue()
	{
		if (!(slider == null))
		{
			slider.value += step;
		}
	}

	public void DecreaseValue()
	{
		if (!(slider == null))
		{
			slider.value -= step;
		}
	}

	public void UpdateValueDisplay()
	{
		if (!(slider == null) && !(valueDisplayText == null))
		{
			string text = (Mathf.Round(slider.value * Mathf.Pow(10f, digitsAfterPoint)) / Mathf.Pow(10f, digitsAfterPoint)).ToString();
			valueDisplayText.text = text;
		}
	}
}
