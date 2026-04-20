using UnityEngine;
using UnityEngine.UI;

public class AdjustmentSlider : MonoBehaviour
{
	public Text ValueNameText;

	public Slider slider;

	public Image MinClampImage;

	public Image MaxClampImage;

	public bool SnapToInterval;

	public float Interval;

	private string ValueName;

	private bool SliderInitialized;

	private float minClamp;

	private float maxClamp;

	private string GetDecimalPlaces()
	{
		if (slider.wholeNumbers)
		{
			return "F0";
		}
		if (maxClamp <= 1f)
		{
			return "F2";
		}
		if (maxClamp > 1f && maxClamp <= 10f)
		{
			return "F1";
		}
		if (maxClamp > 10f)
		{
			return "F0";
		}
		return "F2";
	}

	public void SliderValueChanged()
	{
		if (!SliderInitialized)
		{
			return;
		}
		if (!slider.wholeNumbers)
		{
			if (slider.value > maxClamp)
			{
				slider.value = maxClamp;
				return;
			}
			if (slider.value < minClamp)
			{
				slider.value = minClamp;
				return;
			}
			if (SnapToInterval)
			{
				slider.value = Mathf.Round(slider.value / Interval) * Interval;
			}
		}
		ValueNameText.text = ValueName + ": " + slider.value.ToString(GetDecimalPlaces());
	}

	public void SetupFloatValue(string valueName, float MinValue, float MaxValue, float MinClamp, float MaxClamp, float CurrentValue)
	{
		SliderInitialized = false;
		slider.wholeNumbers = false;
		ValueName = valueName;
		slider.minValue = MinValue;
		slider.maxValue = MaxValue;
		slider.value = CurrentValue;
		minClamp = MinClamp;
		maxClamp = MaxClamp;
		MinClampImage.fillAmount = Mathf.InverseLerp(slider.minValue, slider.maxValue, MinClamp);
		MaxClampImage.fillAmount = 1f - Mathf.InverseLerp(slider.minValue, slider.maxValue, MaxClamp);
		ValueNameText.text = ValueName + ": " + slider.value.ToString(GetDecimalPlaces());
		SliderInitialized = true;
	}

	public void SetupIntValue(string valueName, int MinValue, int MaxValue, int MinClamp, int MaxClamp, int CurrentValue)
	{
		SliderInitialized = false;
		slider.wholeNumbers = true;
		ValueName = valueName;
		slider.minValue = MinValue;
		slider.maxValue = MaxValue;
		slider.value = CurrentValue;
		minClamp = MinClamp;
		maxClamp = MaxClamp;
		MinClampImage.fillAmount = 0f;
		MaxClampImage.fillAmount = 0f;
		ValueNameText.text = ValueName + ": " + slider.value.ToString(GetDecimalPlaces());
		SliderInitialized = true;
	}
}
