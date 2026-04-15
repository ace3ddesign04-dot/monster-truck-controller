using System;
using UnityEngine;
using UnityEngine.UI;

public class CUIColorPicker : MonoBehaviour
{
	private Color _color = Color.red;

	private Action _update;

	public Action ColorChangedCallback;

	private MenuManager menuManager;

	public Color Color
	{
		get
		{
			return _color;
		}
		set
		{
			Setup(value);
		}
	}

	private void Start()
	{
		menuManager = MenuManager.Instance;
	}

	private static void RGBToHSV(Color color, out float h, out float s, out float v)
	{
		float num = Mathf.Min(color.r, color.g, color.b);
		float num2 = Mathf.Max(color.r, color.g, color.b);
		float num3 = num2 - num;
		if (num3 == 0f)
		{
			h = 0f;
		}
		else if (num2 == color.r)
		{
			h = Mathf.Repeat((color.g - color.b) / num3, 6f);
		}
		else if (num2 == color.g)
		{
			h = (color.b - color.r) / num3 + 2f;
		}
		else
		{
			h = (color.r - color.g) / num3 + 4f;
		}
		s = ((num2 != 0f) ? (num3 / num2) : 0f);
		v = num2;
	}

	private static bool GetLocalMouse(GameObject go, out Vector2 result)
	{
		RectTransform rectTransform = (RectTransform)go.transform;
		Vector3 point = rectTransform.InverseTransformPoint(UnityEngine.Input.mousePosition);
		float x = point.x;
		Vector2 min = rectTransform.rect.min;
		float x2 = min.x;
		Vector2 max = rectTransform.rect.max;
		result.x = Mathf.Clamp(x, x2, max.x);
		float y = point.y;
		Vector2 min2 = rectTransform.rect.min;
		float y2 = min2.y;
		Vector2 max2 = rectTransform.rect.max;
		result.y = Mathf.Clamp(y, y2, max2.y);
		return rectTransform.rect.Contains(point);
	}

	private static Vector2 GetWidgetSize(GameObject go)
	{
		RectTransform rectTransform = (RectTransform)go.transform;
		return rectTransform.rect.size;
	}

	private GameObject GO(string name)
	{
		return base.transform.Find(name).gameObject;
	}

	private void Setup(Color inputColor)
	{
		GameObject satvalGO = GO("SaturationValue");
		GameObject satvalKnob = GO("SaturationValue/Knob");
		GameObject hueGO = GO("Hue");
		GameObject hueKnob = GO("Hue/Knob");
		GameObject result = GO("Result");
		Color[] hueColors = new Color[6]
		{
			Color.red,
			Color.yellow,
			Color.green,
			Color.cyan,
			Color.blue,
			Color.magenta
		};
		Color[] satvalColors = new Color[4]
		{
			new Color(0f, 0f, 0f),
			new Color(0f, 0f, 0f),
			new Color(1f, 1f, 1f),
			hueColors[0]
		};
		Texture2D texture2D = new Texture2D(1, 7);
		for (int i = 0; i < 7; i++)
		{
			texture2D.SetPixel(0, i, hueColors[i % 6]);
		}
		texture2D.Apply();
		hueGO.GetComponent<Image>().sprite = Sprite.Create(texture2D, new Rect(0f, 0.5f, 1f, 6f), new Vector2(0.5f, 0.5f));
		Vector2 hueSz = GetWidgetSize(hueGO);
		Texture2D satvalTex = new Texture2D(2, 2);
		satvalGO.GetComponent<Image>().sprite = Sprite.Create(satvalTex, new Rect(0.5f, 0.5f, 1f, 1f), new Vector2(0.5f, 0.5f));
		Action resetSatValTexture = delegate
		{
			for (int j = 0; j < 2; j++)
			{
				for (int k = 0; k < 2; k++)
				{
					satvalTex.SetPixel(k, j, satvalColors[k + j * 2]);
				}
			}
			satvalTex.Apply();
		};
		Vector2 satvalSz = GetWidgetSize(satvalGO);
		RGBToHSV(inputColor, out float Hue, out float Saturation, out float Value);
		Action applyHue = delegate
		{
			int num = Mathf.Clamp((int)Hue, 0, 5);
			int num2 = (num + 1) % 6;
			Color color2 = Color.Lerp(hueColors[num], hueColors[num2], Hue - (float)num);
			satvalColors[3] = color2;
			resetSatValTexture();
		};
		Action applySaturationValue = delegate
		{
			Vector2 vector = new Vector2(Saturation, Value);
			Vector2 vector2 = new Vector2(1f - vector.x, 1f - vector.y);
			Color a = vector2.x * vector2.y * satvalColors[0];
			Color b = vector.x * vector2.y * satvalColors[1];
			Color b2 = vector2.x * vector.y * satvalColors[2];
			Color b3 = vector.x * vector.y * satvalColors[3];
			Color color = a + b + b2 + b3;
			Image component = result.GetComponent<Image>();
			component.color = color;
			if (_color != color)
			{
				_color = color;
				if (ColorChangedCallback != null)
				{
					ColorChangedCallback();
				}
			}
		};
		applyHue();
		applySaturationValue();
		satvalKnob.transform.localPosition = new Vector2(Saturation * satvalSz.x, Value * satvalSz.y);
		Transform transform = hueKnob.transform;
		Vector3 localPosition = hueKnob.transform.localPosition;
		transform.localPosition = new Vector2(localPosition.x, Hue / 6f * satvalSz.y);
		Action dragH = null;
		Action dragSV = null;
		Action idle = delegate
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (GetLocalMouse(hueGO, out Vector2 result4))
				{
					_update = dragH;
				}
				else if (GetLocalMouse(satvalGO, out result4))
				{
					_update = dragSV;
				}
			}
		};
		dragH = delegate
		{
			GetLocalMouse(hueGO, out Vector2 result3);
			Hue = result3.y / hueSz.y * 6f;
			applyHue();
			applySaturationValue();
			Transform transform2 = hueKnob.transform;
			Vector3 localPosition2 = hueKnob.transform.localPosition;
			transform2.localPosition = new Vector2(localPosition2.x, result3.y);
			if (Input.GetMouseButtonUp(0))
			{
				_update = idle;
			}
		};
		dragSV = delegate
		{
			GetLocalMouse(satvalGO, out Vector2 result2);
			Saturation = result2.x / satvalSz.x;
			Value = result2.y / satvalSz.y;
			applySaturationValue();
			satvalKnob.transform.localPosition = result2;
			if (Input.GetMouseButtonUp(0))
			{
				_update = idle;
			}
		};
		_update = idle;
	}

	public void SetRandomColor()
	{
		System.Random random = new System.Random();
		float r = (float)(random.Next() % 1000) / 1000f;
		float g = (float)(random.Next() % 1000) / 1000f;
		float b = (float)(random.Next() % 1000) / 1000f;
		Color = new Color(r, g, b);
	}

	private void Awake()
	{
		Color = Color.red;
	}

	private void Update()
	{
		_update();
	}
}
