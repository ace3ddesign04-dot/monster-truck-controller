using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class WMG_Ring_Graph : WMG_Graph_Manager
{
	[SerializeField]
	private List<Color> _bandColors;

	public WMG_List<Color> bandColors = new WMG_List<Color>();

	[SerializeField]
	private List<float> _values;

	public WMG_List<float> values = new WMG_List<float>();

	[SerializeField]
	private List<string> _labels;

	public WMG_List<string> labels = new WMG_List<string>();

	[SerializeField]
	private List<string> _ringIDs;

	public WMG_List<string> ringIDs = new WMG_List<string>();

	public bool animateData;

	public float animDuration;

	public Ease animEaseType;

	public Object ringPrefab;

	public GameObject extraRing;

	public GameObject background;

	public GameObject zeroLine;

	public GameObject zeroLineText;

	public GameObject ringsParent;

	public GameObject ringLabelsParent;

	public GameObject contentParent;

	public WMG_Data_Source valuesDataSource;

	public WMG_Data_Source labelsDataSource;

	public WMG_Data_Source ringIDsDataSource;

	public Sprite labelLineSprite;

	public Sprite botLeftCorners;

	public Sprite botRightCorners;

	[SerializeField]
	private bool _bandMode;

	[SerializeField]
	private float _innerRadiusPercentage;

	[SerializeField]
	private float _degrees;

	[SerializeField]
	private float _minValue;

	[SerializeField]
	private float _maxValue;

	[SerializeField]
	private Color _bandColor;

	[SerializeField]
	private bool _autoUpdateBandAlpha;

	[SerializeField]
	private Color _ringColor;

	[SerializeField]
	private float _ringWidth;

	[SerializeField]
	private float _ringPointWidthFactor;

	[SerializeField]
	private float _bandPadding;

	[SerializeField]
	private float _labelLinePadding;

	[SerializeField]
	private Vector2 _leftRightPadding;

	[SerializeField]
	private Vector2 _topBotPadding;

	[SerializeField]
	private bool _antiAliasing;

	[SerializeField]
	private float _antiAliasingStrength;

	private float origGraphWidth;

	private float containerWidthCached;

	private float containerHeightCached;

	private Sprite extraRingSprite;

	private Color[] extraRingColors;

	private int ringTexSize;

	private List<WMG_Change_Obj> changeObjs = new List<WMG_Change_Obj>();

	private WMG_Change_Obj numberRingsC = new WMG_Change_Obj();

	private WMG_Change_Obj bandColorC = new WMG_Change_Obj();

	private WMG_Change_Obj ringColorC = new WMG_Change_Obj();

	private WMG_Change_Obj labelsC = new WMG_Change_Obj();

	private WMG_Change_Obj degreesC = new WMG_Change_Obj();

	private WMG_Change_Obj radiusC = new WMG_Change_Obj();

	private WMG_Change_Obj textureC = new WMG_Change_Obj();

	private bool hasInit;

	public bool bandMode
	{
		get
		{
			return _bandMode;
		}
		set
		{
			if (_bandMode != value)
			{
				_bandMode = value;
				textureC.Changed();
			}
		}
	}

	public float innerRadiusPercentage
	{
		get
		{
			return _innerRadiusPercentage;
		}
		set
		{
			if (_innerRadiusPercentage != value)
			{
				_innerRadiusPercentage = value;
				textureC.Changed();
			}
		}
	}

	public float degrees
	{
		get
		{
			return _degrees;
		}
		set
		{
			if (_degrees != value)
			{
				_degrees = value;
				degreesC.Changed();
			}
		}
	}

	public float minValue
	{
		get
		{
			return _minValue;
		}
		set
		{
			if (_minValue != value)
			{
				_minValue = value;
				degreesC.Changed();
			}
		}
	}

	public float maxValue
	{
		get
		{
			return _maxValue;
		}
		set
		{
			if (_maxValue != value)
			{
				_maxValue = value;
				degreesC.Changed();
			}
		}
	}

	public Color bandColor
	{
		get
		{
			return _bandColor;
		}
		set
		{
			if (_bandColor != value)
			{
				_bandColor = value;
				bandColorC.Changed();
			}
		}
	}

	public bool autoUpdateBandAlpha
	{
		get
		{
			return _autoUpdateBandAlpha;
		}
		set
		{
			if (_autoUpdateBandAlpha != value)
			{
				_autoUpdateBandAlpha = value;
				bandColorC.Changed();
			}
		}
	}

	public Color ringColor
	{
		get
		{
			return _ringColor;
		}
		set
		{
			if (_ringColor != value)
			{
				_ringColor = value;
				ringColorC.Changed();
			}
		}
	}

	public float ringWidth
	{
		get
		{
			return _ringWidth;
		}
		set
		{
			if (_ringWidth != value)
			{
				_ringWidth = value;
				textureC.Changed();
			}
		}
	}

	public float ringPointWidthFactor
	{
		get
		{
			return _ringPointWidthFactor;
		}
		set
		{
			if (_ringPointWidthFactor != value)
			{
				_ringPointWidthFactor = value;
				textureC.Changed();
			}
		}
	}

	public float bandPadding
	{
		get
		{
			return _bandPadding;
		}
		set
		{
			if (_bandPadding != value)
			{
				_bandPadding = value;
				textureC.Changed();
			}
		}
	}

	public float labelLinePadding
	{
		get
		{
			return _labelLinePadding;
		}
		set
		{
			if (_labelLinePadding != value)
			{
				_labelLinePadding = value;
				radiusC.Changed();
			}
		}
	}

	public Vector2 leftRightPadding
	{
		get
		{
			return _leftRightPadding;
		}
		set
		{
			if (_leftRightPadding != value)
			{
				_leftRightPadding = value;
				radiusC.Changed();
			}
		}
	}

	public Vector2 topBotPadding
	{
		get
		{
			return _topBotPadding;
		}
		set
		{
			if (_topBotPadding != value)
			{
				_topBotPadding = value;
				radiusC.Changed();
			}
		}
	}

	public bool antiAliasing
	{
		get
		{
			return _antiAliasing;
		}
		set
		{
			if (_antiAliasing != value)
			{
				_antiAliasing = value;
				textureC.Changed();
			}
		}
	}

	public float antiAliasingStrength
	{
		get
		{
			return _antiAliasingStrength;
		}
		set
		{
			if (_antiAliasingStrength != value)
			{
				_antiAliasingStrength = value;
				textureC.Changed();
			}
		}
	}

	public float outerRadius
	{
		get
		{
			float spriteWidth = getSpriteWidth(base.gameObject);
			Vector2 leftRightPadding = this.leftRightPadding;
			float num = spriteWidth - leftRightPadding.x;
			Vector2 leftRightPadding2 = this.leftRightPadding;
			float a = (num - leftRightPadding2.y) / 2f;
			float spriteHeight = getSpriteHeight(base.gameObject);
			Vector2 topBotPadding = this.topBotPadding;
			float num2 = spriteHeight - topBotPadding.x;
			Vector2 topBotPadding2 = this.topBotPadding;
			return Mathf.Min(a, (num2 - topBotPadding2.y) / 2f);
		}
	}

	public float RingWidthFactor => (1f - innerRadiusPercentage) * outerRadius / origGraphWidth;

	public List<WMG_Ring> rings
	{
		get;
		private set;
	}

	private void Start()
	{
		Init();
		PauseCallbacks();
		AllChanged();
	}

	public void Init()
	{
		if (!hasInit)
		{
			hasInit = true;
			changeObjs.Add(numberRingsC);
			changeObjs.Add(textureC);
			changeObjs.Add(degreesC);
			changeObjs.Add(ringColorC);
			changeObjs.Add(bandColorC);
			changeObjs.Add(radiusC);
			changeObjs.Add(labelsC);
			extraRingSprite = WMG_Util.createSprite(getTexture(extraRing));
			ringTexSize = extraRingSprite.texture.width;
			extraRingColors = new Color[ringTexSize * ringTexSize];
			setTexture(extraRing, extraRingSprite);
			rings = new List<WMG_Ring>();
			origGraphWidth = (1f - innerRadiusPercentage) * outerRadius;
			bandColors.SetList(_bandColors);
			bandColors.Changed += bandColorsChanged;
			values.SetList(_values);
			values.Changed += valuesChanged;
			labels.SetList(_labels);
			labels.Changed += labelsChanged;
			ringIDs.SetList(_ringIDs);
			ringIDs.Changed += ringIDsChanged;
			numberRingsC.OnChange += NumberRingsChanged;
			bandColorC.OnChange += BandColorChanged;
			ringColorC.OnChange += RingColorChanged;
			labelsC.OnChange += LabelsChanged;
			degreesC.OnChange += DegreesChanged;
			radiusC.OnChange += RadiusChanged;
			textureC.OnChange += TextureChanged;
			PauseCallbacks();
		}
	}

	private void Update()
	{
		updateFromDataSource();
		updateFromResize();
		Refresh();
	}

	public void Refresh()
	{
		ResumeCallbacks();
		PauseCallbacks();
	}

	private void PauseCallbacks()
	{
		for (int i = 0; i < changeObjs.Count; i++)
		{
			changeObjs[i].changesPaused = true;
			changeObjs[i].changePaused = false;
		}
	}

	private void ResumeCallbacks()
	{
		for (int i = 0; i < changeObjs.Count; i++)
		{
			changeObjs[i].changesPaused = false;
			if (changeObjs[i].changePaused)
			{
				changeObjs[i].Changed();
			}
		}
	}

	private void updateFromResize()
	{
		bool flag = false;
		updateCacheAndFlag(ref containerWidthCached, getSpriteWidth(base.gameObject), ref flag);
		updateCacheAndFlag(ref containerHeightCached, getSpriteHeight(base.gameObject), ref flag);
		if (flag)
		{
			radiusC.Changed();
		}
	}

	private void updateFromDataSource()
	{
		if (valuesDataSource != null)
		{
			values.SetList(valuesDataSource.getData<float>());
		}
		if (labelsDataSource != null)
		{
			labels.SetList(labelsDataSource.getData<string>());
		}
		if (ringIDsDataSource != null)
		{
			ringIDs.SetList(ringIDsDataSource.getData<string>());
		}
	}

	private void NumberRingsChanged()
	{
		updateNumberRings();
	}

	private void TextureChanged()
	{
		updateRingsAndBands();
		updateOuterRadius();
	}

	private void DegreesChanged()
	{
		updateDegrees();
	}

	private void RingColorChanged()
	{
		updateRingColors();
	}

	private void BandColorChanged()
	{
		updateBandColors();
	}

	private void RadiusChanged()
	{
		updateOuterRadius();
	}

	private void LabelsChanged()
	{
		updateLabelsText();
	}

	private void AllChanged()
	{
		numberRingsC.Changed();
		textureC.Changed();
		degreesC.Changed();
		ringColorC.Changed();
		bandColorC.Changed();
		radiusC.Changed();
		labelsC.Changed();
	}

	public void bandColorsChanged(bool editorChange, bool countChanged, bool oneValChanged, int index)
	{
		WMG_Util.listChanged(editorChange, ref bandColors, ref _bandColors, oneValChanged, index);
		bandColorC.Changed();
	}

	public void valuesChanged(bool editorChange, bool countChanged, bool oneValChanged, int index)
	{
		WMG_Util.listChanged(editorChange, ref values, ref _values, oneValChanged, index);
		if (countChanged)
		{
			AllChanged();
		}
		else
		{
			degreesC.Changed();
		}
	}

	public void labelsChanged(bool editorChange, bool countChanged, bool oneValChanged, int index)
	{
		WMG_Util.listChanged(editorChange, ref labels, ref _labels, oneValChanged, index);
		labelsC.Changed();
	}

	public void ringIDsChanged(bool editorChange, bool countChanged, bool oneValChanged, int index)
	{
		WMG_Util.listChanged(editorChange, ref ringIDs, ref _ringIDs, oneValChanged, index);
	}

	private void updateNumberRings()
	{
		for (int i = 0; i < values.Count; i++)
		{
			if (labels.Count <= i)
			{
				labels.AddNoCb("Ring " + (i + 1), ref _labels);
			}
			if (bandColors.Count <= i)
			{
				bandColors.AddNoCb(bandColor, ref _bandColors);
			}
			if (rings.Count <= i)
			{
				GameObject gameObject = Object.Instantiate(ringPrefab) as GameObject;
				changeSpriteParent(gameObject, ringsParent);
				WMG_Ring component = gameObject.GetComponent<WMG_Ring>();
				component.initialize(this);
				rings.Add(component);
			}
		}
		for (int num = rings.Count - 1; num >= 0; num--)
		{
			if (rings[num] != null && num >= values.Count)
			{
				UnityEngine.Object.Destroy(rings[num].label);
				UnityEngine.Object.Destroy(rings[num].gameObject);
				rings.RemoveAt(num);
			}
		}
	}

	private void updateOuterRadius()
	{
		int num = Mathf.RoundToInt(outerRadius * 2f);
		changeSpriteSize(extraRing, num, num);
		for (int i = 0; i < rings.Count; i++)
		{
			changeSpriteSize(rings[i].ring, num, num);
			changeSpriteSize(rings[i].band, num, num);
			changeSpriteHeight(rings[i].label, Mathf.RoundToInt(outerRadius + labelLinePadding));
		}
		changeSpriteHeight(zeroLine, Mathf.RoundToInt(outerRadius + labelLinePadding));
		for (int j = 0; j < rings.Count; j++)
		{
			rings[j].updateRingPoint(j);
		}
		Vector2 leftRightPadding = this.leftRightPadding;
		float y = leftRightPadding.y;
		Vector2 leftRightPadding2 = this.leftRightPadding;
		float x = (y - leftRightPadding2.x) / 2f;
		Vector2 topBotPadding = this.topBotPadding;
		float y2 = topBotPadding.y;
		Vector2 topBotPadding2 = this.topBotPadding;
		Vector3 newPos = new Vector3(x, (y2 - topBotPadding2.x) / 2f);
		changeSpritePositionTo(contentParent, newPos);
	}

	private void updateLabelsText()
	{
		for (int i = 0; i < rings.Count; i++)
		{
			changeLabelText(rings[i].labelText, labels[i]);
			forceUpdateUI();
			changeSpriteHeight(rings[i].textLine, Mathf.RoundToInt(getSpriteWidth(rings[i].labelBackground) + 10f));
		}
	}

	private void updateRingsAndBands()
	{
		if (bandMode)
		{
			SetActive(extraRing, state: true);
			float ringRadius = getRingRadius(rings.Count);
			WMG_Util.updateBandColors(ref extraRingColors, outerRadius * 2f, ringRadius - ringWidth, ringRadius, antiAliasing, antiAliasingStrength);
			extraRingSprite.texture.SetPixels(extraRingColors);
			extraRingSprite.texture.Apply();
		}
		else
		{
			SetActive(extraRing, state: false);
		}
		for (int i = 0; i < rings.Count; i++)
		{
			rings[i].updateRing(i);
		}
	}

	public float getRingRadius(int index)
	{
		int num = rings.Count - 1;
		if (bandMode)
		{
			num++;
		}
		if (num == 0)
		{
			return outerRadius;
		}
		float num2 = (1f - innerRadiusPercentage) * outerRadius / (float)num;
		return innerRadiusPercentage * outerRadius + (float)index * num2;
	}

	private void updateDegrees()
	{
		Vector3 vector = new Vector3(0f, 0f, (0f - degrees) / 2f);
		float num = (360f - degrees) / 360f;
		changeRadialSpriteRotation(extraRing, vector);
		changeSpriteFill(extraRing, num);
		bool flag = false;
		for (int i = 0; i < rings.Count; i++)
		{
			WMG_Ring ring = rings[i];
			changeRadialSpriteRotation(rings[i].ring, vector);
			changeSpriteFill(rings[i].ring, num);
			float num2 = values[i] / (maxValue - minValue);
			changeRadialSpriteRotation(rings[i].band, vector);
			changeSpriteFill(rings[i].band, 0f);
			if (animateData)
			{
				WMG_Anim.animFill(rings[i].band, animDuration, animEaseType, num * num2);
			}
			else
			{
				changeSpriteFill(rings[i].band, num * num2);
			}
			if (num2 == 0f)
			{
				flag = true;
			}
			int num3 = 0;
			for (int num4 = i - 1; num4 >= 0; num4--)
			{
				float num5 = values[num4] / (maxValue - minValue);
				if (Mathf.Abs(num2 - num5) < 0.01f)
				{
					num3++;
					num2 = num5;
				}
			}
			changeSpriteWidth(ring.textLine, 2 + 20 * num3);
			bool labelsOverlap = num3 > 0;
			if (!labelsOverlap)
			{
				SetActiveImage(ring.line, state: true);
				changeSpritePivot(ring.textLine, WMGpivotTypes.Bottom);
				setTexture(ring.textLine, labelLineSprite);
				setAnchor(ring.labelBackground, new Vector2(0f, 1f), new Vector2(1f, 0f), Vector2.zero);
			}
			else
			{
				SetActiveImage(ring.line, state: false);
			}
			Vector3 vector2 = new Vector3(0f, 0f, (0f - num2) * (360f - degrees));
			if (animateData)
			{
				if (DOTween.IsTweening(rings[i].label.transform))
				{
					labelRotationUpdated(ring, 0f, labelsOverlap);
					float degOffset = 90f;
					Vector3 localEulerAngles = ring.label.transform.localEulerAngles;
					if (localEulerAngles.z < 180f)
					{
						degOffset *= -1f;
					}
					WMG_Anim.animRotation(rings[i].label, animDuration, animEaseType, vector2 + new Vector3(0f, 0f, 360f) + vector, relative: false);
					WMG_Anim.animRotationCallbackU(rings[i].textLine, animDuration, animEaseType, -vector2 - vector + new Vector3(0f, 0f, degOffset), relative: false, delegate
					{
						labelRotationUpdated(ring, degOffset, labelsOverlap);
					});
				}
				else
				{
					rings[i].label.transform.localEulerAngles = vector;
					rings[i].textLine.transform.localEulerAngles = -vector + new Vector3(0f, 0f, 90f);
					WMG_Anim.animRotation(rings[i].label, animDuration, animEaseType, vector2, relative: true);
					WMG_Anim.animRotationCallbackU(rings[i].textLine, animDuration, animEaseType, -vector2, relative: true, delegate
					{
						labelRotationUpdated(ring, 0f, labelsOverlap);
					});
				}
			}
			else
			{
				rings[i].label.transform.localEulerAngles = vector2 + vector;
				rings[i].textLine.transform.localEulerAngles = -vector2 - vector + new Vector3(0f, 0f, 90f);
				labelRotationUpdated(ring, 0f, labelsOverlap);
			}
		}
		zeroLine.transform.localEulerAngles = vector;
		zeroLineText.transform.localEulerAngles = -vector;
		SetActive(zeroLine, !flag);
	}

	private void labelRotationUpdated(WMG_Ring ring, float degOffset, bool labelsOverlap)
	{
		Vector3 localEulerAngles = ring.label.transform.localEulerAngles;
		if (localEulerAngles.z < 180f)
		{
			if (labelsOverlap)
			{
				changeSpritePivot(ring.textLine, WMGpivotTypes.BottomLeft);
				setTexture(ring.textLine, botRightCorners);
				setAnchor(ring.labelBackground, Vector2.one, new Vector2(1f, 0f), Vector2.zero);
			}
			if (degOffset == 0f || degOffset == 90f)
			{
				Transform transform = ring.textLine.transform;
				Vector3 localEulerAngles2 = ring.textLine.transform.localEulerAngles;
				float x = localEulerAngles2.x;
				Vector3 localEulerAngles3 = ring.textLine.transform.localEulerAngles;
				float y = localEulerAngles3.y;
				Vector3 localEulerAngles4 = ring.textLine.transform.localEulerAngles;
				transform.localEulerAngles = new Vector3(x, y, localEulerAngles4.z - 180f);
			}
			ring.labelBackground.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
			changeSpritePivot(ring.labelBackground, WMGpivotTypes.BottomRight);
		}
		else
		{
			if (labelsOverlap)
			{
				changeSpritePivot(ring.textLine, WMGpivotTypes.BottomRight);
				setTexture(ring.textLine, botLeftCorners);
				setAnchor(ring.labelBackground, new Vector2(0f, 1f), new Vector2(1f, 0f), Vector2.zero);
			}
			if (degOffset == -90f)
			{
				Transform transform2 = ring.textLine.transform;
				Vector3 localEulerAngles5 = ring.textLine.transform.localEulerAngles;
				float x2 = localEulerAngles5.x;
				Vector3 localEulerAngles6 = ring.textLine.transform.localEulerAngles;
				float y2 = localEulerAngles6.y;
				Vector3 localEulerAngles7 = ring.textLine.transform.localEulerAngles;
				transform2.localEulerAngles = new Vector3(x2, y2, localEulerAngles7.z + 180f);
			}
			ring.labelBackground.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
			changeSpritePivot(ring.labelBackground, WMGpivotTypes.BottomLeft);
		}
	}

	public List<int> getRingsSortedByValue()
	{
		List<float> list = new List<float>(values);
		list.Sort();
		List<int> list2 = new List<int>();
		for (int i = 0; i < list.Count; i++)
		{
			for (int j = 0; j < values.Count; j++)
			{
				if (Mathf.Approximately(values[j], list[i]))
				{
					list2.Add(j);
					break;
				}
			}
		}
		return list2;
	}

	private void updateRingColors()
	{
		changeSpriteColor(extraRing, ringColor);
		for (int i = 0; i < rings.Count; i++)
		{
			changeSpriteColor(rings[i].ring, ringColor);
		}
	}

	private void updateBandColors()
	{
		for (int i = 0; i < rings.Count; i++)
		{
			if (autoUpdateBandAlpha)
			{
				WMG_List<Color> wMG_List = bandColors;
				int index = i;
				Color color = bandColors[i];
				float r = color.r;
				Color color2 = bandColors[i];
				float g = color2.g;
				Color color3 = bandColors[i];
				wMG_List.SetValNoCb(index, new Color(r, g, color3.b, ((float)i + 1f) / (float)rings.Count), ref _bandColors);
			}
			changeSpriteColor(rings[i].band, bandColors[i]);
		}
	}

	public WMG_Ring getRing(string id)
	{
		for (int i = 0; i < ringIDs.Count; i++)
		{
			if (id == ringIDs[i])
			{
				return rings[i];
			}
		}
		UnityEngine.Debug.LogError("No ring found with id: " + id);
		return null;
	}

	public void HighlightRing(string id)
	{
		for (int i = 0; i < rings.Count; i++)
		{
			GameObject band = rings[i].band;
			Color bandColor = this.bandColor;
			float r = bandColor.r;
			Color bandColor2 = this.bandColor;
			float g = bandColor2.g;
			Color bandColor3 = this.bandColor;
			changeSpriteColor(band, new Color(r, g, bandColor3.b, 0f));
		}
		GameObject band2 = getRing(id).band;
		Color bandColor4 = this.bandColor;
		float r2 = bandColor4.r;
		Color bandColor5 = this.bandColor;
		float g2 = bandColor5.g;
		Color bandColor6 = this.bandColor;
		changeSpriteColor(band2, new Color(r2, g2, bandColor6.b, 1f));
	}

	public void RemoveHighlights()
	{
		bandColorC.Changed();
	}
}
