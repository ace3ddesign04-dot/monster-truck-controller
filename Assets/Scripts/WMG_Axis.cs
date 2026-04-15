using System.Collections.Generic;
using UnityEngine;

public class WMG_Axis : WMG_GUI_Functions
{
	public enum labelTypes
	{
		ticks,
		ticks_center,
		groups,
		manual
	}

	public delegate string AxisLabelLabeler(WMG_Axis axis, int labelIndex);

	public WMG_Axis_Graph graph;

	[SerializeField]
	private List<string> _axisLabels;

	public WMG_List<string> axisLabels = new WMG_List<string>();

	public GameObject AxisTitle;

	public GameObject GridLines;

	public GameObject AxisTicks;

	public GameObject AxisLine;

	public GameObject AxisArrowUR;

	public GameObject AxisArrowDL;

	public GameObject AxisObj;

	public GameObject AxisLabelObjs;

	[SerializeField]
	private float _AxisMinValue;

	[SerializeField]
	private float _AxisMaxValue;

	[SerializeField]
	private int _AxisNumTicks;

	[SerializeField]
	private bool _MinAutoGrow;

	[SerializeField]
	private bool _MaxAutoGrow;

	[SerializeField]
	private bool _MinAutoShrink;

	[SerializeField]
	private bool _MaxAutoShrink;

	[SerializeField]
	private float _AxisLinePadding;

	[SerializeField]
	private bool _AxisUseNonTickPercent;

	[SerializeField]
	private float _AxisNonTickPercent;

	[SerializeField]
	private bool _AxisArrowTopRight;

	[SerializeField]
	private bool _AxisArrowBotLeft;

	[SerializeField]
	private bool _AxisTicksRightAbove;

	[SerializeField]
	private int _AxisTick;

	[SerializeField]
	private bool _hideTick;

	[SerializeField]
	private labelTypes _LabelType;

	[SerializeField]
	private int _AxisLabelSkipStart;

	[SerializeField]
	private int _AxisLabelSkipInterval;

	[SerializeField]
	private float _AxisLabelRotation;

	[SerializeField]
	private bool _SetLabelsUsingMaxMin;

	[SerializeField]
	private int _AxisLabelSize;

	[SerializeField]
	private int _numDecimalsAxisLabels;

	[SerializeField]
	private bool _hideLabels;

	[SerializeField]
	private float _AxisLabelSpaceOffset;

	[SerializeField]
	private float _autoFitRotation;

	[SerializeField]
	private float _autoFitMaxBorder;

	[SerializeField]
	private float _AxisLabelSpacing;

	[SerializeField]
	private float _AxisLabelDistBetween;

	[SerializeField]
	private bool _hideGrid;

	[SerializeField]
	private bool _hideTicks;

	[SerializeField]
	private string _AxisTitleString;

	[SerializeField]
	private Vector2 _AxisTitleOffset;

	[SerializeField]
	private int _AxisTitleFontSize;

	private float GridLineLength;

	private float AxisLinePaddingTot;

	private float AxisPercentagePosition;

	private bool hasInit;

	private WMG_Axis otherAxis;

	public AxisLabelLabeler axisLabelLabeler;

	private List<WMG_Change_Obj> changeObjs = new List<WMG_Change_Obj>();

	private WMG_Change_Obj graphC = new WMG_Change_Obj();

	private WMG_Change_Obj seriesC = new WMG_Change_Obj();

	public float AxisMinValue
	{
		get
		{
			return _AxisMinValue;
		}
		set
		{
			if (_AxisMinValue != value)
			{
				_AxisMinValue = value;
				graphC.Changed();
				seriesC.Changed();
			}
		}
	}

	public float AxisMaxValue
	{
		get
		{
			return _AxisMaxValue;
		}
		set
		{
			if (_AxisMaxValue != value)
			{
				_AxisMaxValue = value;
				graphC.Changed();
				seriesC.Changed();
			}
		}
	}

	public int AxisNumTicks
	{
		get
		{
			return _AxisNumTicks;
		}
		set
		{
			if (_AxisNumTicks != value)
			{
				_AxisNumTicks = value;
				graphC.Changed();
			}
		}
	}

	public bool MinAutoGrow
	{
		get
		{
			return _MinAutoGrow;
		}
		set
		{
			if (_MinAutoGrow != value)
			{
				_MinAutoGrow = value;
				graphC.Changed();
				seriesC.Changed();
			}
		}
	}

	public bool MaxAutoGrow
	{
		get
		{
			return _MaxAutoGrow;
		}
		set
		{
			if (_MaxAutoGrow != value)
			{
				_MaxAutoGrow = value;
				graphC.Changed();
				seriesC.Changed();
			}
		}
	}

	public bool MinAutoShrink
	{
		get
		{
			return _MinAutoShrink;
		}
		set
		{
			if (_MinAutoShrink != value)
			{
				_MinAutoShrink = value;
				graphC.Changed();
				seriesC.Changed();
			}
		}
	}

	public bool MaxAutoShrink
	{
		get
		{
			return _MaxAutoShrink;
		}
		set
		{
			if (_MaxAutoShrink != value)
			{
				_MaxAutoShrink = value;
				graphC.Changed();
				seriesC.Changed();
			}
		}
	}

	public float AxisLinePadding
	{
		get
		{
			return _AxisLinePadding;
		}
		set
		{
			if (_AxisLinePadding != value)
			{
				_AxisLinePadding = value;
				graphC.Changed();
			}
		}
	}

	public bool AxisUseNonTickPercent
	{
		get
		{
			return _AxisUseNonTickPercent;
		}
		set
		{
			if (_AxisUseNonTickPercent != value)
			{
				_AxisUseNonTickPercent = value;
				graphC.Changed();
			}
		}
	}

	public float AxisNonTickPercent
	{
		get
		{
			return _AxisNonTickPercent;
		}
		set
		{
			if (_AxisNonTickPercent != value)
			{
				_AxisNonTickPercent = value;
				graphC.Changed();
			}
		}
	}

	public bool AxisArrowTopRight
	{
		get
		{
			return _AxisArrowTopRight;
		}
		set
		{
			if (_AxisArrowTopRight != value)
			{
				_AxisArrowTopRight = value;
				graphC.Changed();
			}
		}
	}

	public bool AxisArrowBotLeft
	{
		get
		{
			return _AxisArrowBotLeft;
		}
		set
		{
			if (_AxisArrowBotLeft != value)
			{
				_AxisArrowBotLeft = value;
				graphC.Changed();
			}
		}
	}

	public bool AxisTicksRightAbove
	{
		get
		{
			return _AxisTicksRightAbove;
		}
		set
		{
			if (_AxisTicksRightAbove != value)
			{
				_AxisTicksRightAbove = value;
				graphC.Changed();
			}
		}
	}

	public int AxisTick
	{
		get
		{
			return _AxisTick;
		}
		set
		{
			if (_AxisTick != value)
			{
				_AxisTick = value;
				graphC.Changed();
			}
		}
	}

	public bool hideTick
	{
		get
		{
			return _hideTick;
		}
		set
		{
			if (_hideTick != value)
			{
				_hideTick = value;
				graphC.Changed();
			}
		}
	}

	public labelTypes LabelType
	{
		get
		{
			return _LabelType;
		}
		set
		{
			if (_LabelType != value)
			{
				_LabelType = value;
				graphC.Changed();
			}
		}
	}

	public int AxisLabelSkipInterval
	{
		get
		{
			return _AxisLabelSkipInterval;
		}
		set
		{
			if (_AxisLabelSkipInterval != value)
			{
				_AxisLabelSkipInterval = value;
				graphC.Changed();
			}
		}
	}

	public int AxisLabelSkipStart
	{
		get
		{
			return _AxisLabelSkipStart;
		}
		set
		{
			if (_AxisLabelSkipStart != value)
			{
				_AxisLabelSkipStart = value;
				graphC.Changed();
			}
		}
	}

	public float AxisLabelRotation
	{
		get
		{
			return _AxisLabelRotation;
		}
		set
		{
			if (_AxisLabelRotation != value)
			{
				_AxisLabelRotation = value;
				graphC.Changed();
			}
		}
	}

	public bool SetLabelsUsingMaxMin
	{
		get
		{
			return _SetLabelsUsingMaxMin;
		}
		set
		{
			if (_SetLabelsUsingMaxMin != value)
			{
				_SetLabelsUsingMaxMin = value;
				graphC.Changed();
			}
		}
	}

	public int AxisLabelSize
	{
		get
		{
			return _AxisLabelSize;
		}
		set
		{
			if (_AxisLabelSize != value)
			{
				_AxisLabelSize = value;
				graphC.Changed();
			}
		}
	}

	public int numDecimalsAxisLabels
	{
		get
		{
			return _numDecimalsAxisLabels;
		}
		set
		{
			if (_numDecimalsAxisLabels != value)
			{
				_numDecimalsAxisLabels = value;
				graphC.Changed();
			}
		}
	}

	public bool hideLabels
	{
		get
		{
			return _hideLabels;
		}
		set
		{
			if (_hideLabels != value)
			{
				_hideLabels = value;
				graphC.Changed();
			}
		}
	}

	public float AxisLabelSpaceOffset
	{
		get
		{
			return _AxisLabelSpaceOffset;
		}
		set
		{
			if (_AxisLabelSpaceOffset != value)
			{
				_AxisLabelSpaceOffset = value;
				graphC.Changed();
			}
		}
	}

	public float autoFitRotation
	{
		get
		{
			return _autoFitRotation;
		}
		set
		{
			if (_autoFitRotation != value)
			{
				_autoFitRotation = value;
				graphC.Changed();
			}
		}
	}

	public float autoFitMaxBorder
	{
		get
		{
			return _autoFitMaxBorder;
		}
		set
		{
			if (_autoFitMaxBorder != value)
			{
				_autoFitMaxBorder = value;
				graphC.Changed();
			}
		}
	}

	public float AxisLabelSpacing
	{
		get
		{
			return _AxisLabelSpacing;
		}
		set
		{
			if (_AxisLabelSpacing != value)
			{
				_AxisLabelSpacing = value;
				graphC.Changed();
			}
		}
	}

	public float AxisLabelDistBetween
	{
		get
		{
			return _AxisLabelDistBetween;
		}
		set
		{
			if (_AxisLabelDistBetween != value)
			{
				_AxisLabelDistBetween = value;
				graphC.Changed();
			}
		}
	}

	public bool hideGrid
	{
		get
		{
			return _hideGrid;
		}
		set
		{
			if (_hideGrid != value)
			{
				_hideGrid = value;
				graphC.Changed();
			}
		}
	}

	public bool hideTicks
	{
		get
		{
			return _hideTicks;
		}
		set
		{
			if (_hideTicks != value)
			{
				_hideTicks = value;
				graphC.Changed();
			}
		}
	}

	public string AxisTitleString
	{
		get
		{
			return _AxisTitleString;
		}
		set
		{
			if (_AxisTitleString != value)
			{
				_AxisTitleString = value;
				graphC.Changed();
			}
		}
	}

	public Vector2 AxisTitleOffset
	{
		get
		{
			return _AxisTitleOffset;
		}
		set
		{
			if (_AxisTitleOffset != value)
			{
				_AxisTitleOffset = value;
				graphC.Changed();
			}
		}
	}

	public int AxisTitleFontSize
	{
		get
		{
			return _AxisTitleFontSize;
		}
		set
		{
			if (_AxisTitleFontSize != value)
			{
				_AxisTitleFontSize = value;
				graphC.Changed();
			}
		}
	}

	public float AxisLength
	{
		get
		{
			if (isY)
			{
				return graph.yAxisLength;
			}
			return graph.xAxisLength;
		}
	}

	public int origAxisLabelSize
	{
		get;
		private set;
	}

	public float origAxisLabelSpaceOffset
	{
		get;
		private set;
	}

	public int origAxisTitleFontSize
	{
		get;
		private set;
	}

	public float origAxisLinePadding
	{
		get;
		private set;
	}

	public Vector2 origAxisArrowSize
	{
		get;
		private set;
	}

	public bool isY
	{
		get;
		private set;
	}

	public void Init(WMG_Axis otherAxis, bool isY)
	{
		if (!hasInit)
		{
			hasInit = true;
			changeObjs.Add(graphC);
			changeObjs.Add(seriesC);
			this.otherAxis = otherAxis;
			this.isY = isY;
			axisLabels.SetList(_axisLabels);
			axisLabels.Changed += axisLabelsChanged;
			graphC.OnChange += GraphChanged;
			seriesC.OnChange += SeriesChanged;
			axisLabelLabeler = defaultAxisLabelLabeler;
			setOriginalPropertyValues();
			PauseCallbacks();
		}
	}

	public void PauseCallbacks()
	{
		for (int i = 0; i < changeObjs.Count; i++)
		{
			changeObjs[i].changesPaused = true;
			changeObjs[i].changePaused = false;
		}
	}

	public void ResumeCallbacks()
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

	private void GraphChanged()
	{
		graph.graphC.Changed();
	}

	private void SeriesChanged()
	{
		graph.seriesNoCountC.Changed();
	}

	private void axisLabelsChanged(bool editorChange, bool countChanged, bool oneValChanged, int index)
	{
		WMG_Util.listChanged(editorChange, ref axisLabels, ref _axisLabels, oneValChanged, index);
		graphC.Changed();
	}

	public void setOriginalPropertyValues()
	{
		origAxisLabelSize = AxisLabelSize;
		origAxisTitleFontSize = AxisTitleFontSize;
		origAxisLabelSpaceOffset = AxisLabelSpaceOffset;
		origAxisLinePadding = AxisLinePadding;
		origAxisArrowSize = getSpriteSize(AxisArrowDL);
	}

	public void setAxisTopRight(bool rightAbove)
	{
		_AxisArrowTopRight = true;
		_AxisArrowBotLeft = false;
		otherAxis.setOtherHideTick(val: false);
		otherAxis.setOtherAxisTick(0);
		otherAxis.setOtherAxisNonTickPercent(0f);
		_AxisTicksRightAbove = rightAbove;
	}

	public void setAxisBotLeft(bool rightAbove)
	{
		_AxisArrowTopRight = false;
		_AxisArrowBotLeft = true;
		otherAxis.setOtherHideTick(val: false);
		otherAxis.setOtherAxisTick(AxisNumTicks - 1);
		otherAxis.setOtherAxisNonTickPercent(1f);
		_AxisTicksRightAbove = rightAbove;
	}

	public void setAxisMiddle(bool rightAbove)
	{
		_AxisArrowTopRight = true;
		_AxisArrowBotLeft = true;
		otherAxis.setOtherHideTick(val: true);
		otherAxis.setOtherAxisTick(AxisNumTicks / 2);
		otherAxis.setOtherAxisNonTickPercent(0.5f);
		_AxisTicksRightAbove = rightAbove;
	}

	public void setOtherAxisNonTickPercent(float val)
	{
		_AxisNonTickPercent = val;
	}

	public void setOtherAxisTick(int val)
	{
		_AxisTick = val;
	}

	public void setOtherHideTick(bool val)
	{
		_hideTick = val;
	}

	public void setOtherRightAbove(bool val)
	{
		_AxisTicksRightAbove = val;
	}

	public void possiblyHideTickBasedOnPercent()
	{
		if (otherAxis.AxisUseNonTickPercent && AxisNumTicks % 2 == 0)
		{
			_hideTick = false;
		}
	}

	public void ChangeOrientation()
	{
		labelTypes labelType = LabelType;
		float axisMaxValue = AxisMaxValue;
		float axisMinValue = AxisMinValue;
		int axisNumTicks = AxisNumTicks;
		int numDecimalsAxisLabels = this.numDecimalsAxisLabels;
		bool minAutoGrow = MinAutoGrow;
		bool maxAutoGrow = MaxAutoGrow;
		bool minAutoShrink = MinAutoShrink;
		bool maxAutoShrink = MaxAutoShrink;
		bool setLabelsUsingMaxMin = SetLabelsUsingMaxMin;
		float axisLabelSpacing = AxisLabelSpacing;
		string axisTitleString = AxisTitleString;
		bool hideTicks = this.hideTicks;
		List<string> tLabels = new List<string>(axisLabels);
		LabelType = otherAxis.LabelType;
		AxisMaxValue = otherAxis.AxisMaxValue;
		AxisMinValue = otherAxis.AxisMinValue;
		AxisNumTicks = otherAxis.AxisNumTicks;
		this.hideTicks = otherAxis.hideTicks;
		this.numDecimalsAxisLabels = otherAxis.numDecimalsAxisLabels;
		MinAutoGrow = otherAxis.MinAutoGrow;
		MaxAutoGrow = otherAxis.MaxAutoGrow;
		MinAutoShrink = otherAxis.MinAutoShrink;
		MaxAutoShrink = otherAxis.MaxAutoShrink;
		SetLabelsUsingMaxMin = otherAxis.SetLabelsUsingMaxMin;
		AxisLabelSpacing = otherAxis.AxisLabelSpacing;
		AxisTitleString = otherAxis.AxisTitleString;
		axisLabels.SetList(otherAxis.axisLabels);
		otherAxis.ChangeOrientationEnd(labelType, axisMaxValue, axisMinValue, axisNumTicks, numDecimalsAxisLabels, minAutoGrow, maxAutoGrow, minAutoShrink, maxAutoShrink, setLabelsUsingMaxMin, axisLabelSpacing, axisTitleString, tLabels, hideTicks);
	}

	public void ChangeOrientationEnd(labelTypes tLabelType, float tAxisMaxValue, float tAxisMinValue, int tAxisNumTicks, int tnumDecimalsAxisLabels, bool tMinAutoGrow, bool tMaxAutoGrow, bool tMinAutoShrink, bool tMaxAutoShrink, bool tSetLabelsUsingMaxMin, float tAxisLabelSpacing, string tAxisTitleString, List<string> tLabels, bool tHideTicks)
	{
		LabelType = tLabelType;
		AxisMaxValue = tAxisMaxValue;
		AxisMinValue = tAxisMinValue;
		AxisNumTicks = tAxisNumTicks;
		hideTicks = tHideTicks;
		numDecimalsAxisLabels = tnumDecimalsAxisLabels;
		MinAutoGrow = tMinAutoGrow;
		MaxAutoGrow = tMaxAutoGrow;
		MinAutoShrink = tMinAutoShrink;
		MaxAutoShrink = tMaxAutoShrink;
		SetLabelsUsingMaxMin = tSetLabelsUsingMaxMin;
		AxisLabelSpacing = tAxisLabelSpacing;
		AxisTitleString = tAxisTitleString;
		axisLabels.SetList(tLabels);
	}

	public void updateAxesRelativeToOrigin(float originVal)
	{
		if (graph.axesType == WMG_Axis_Graph.axesTypes.AUTO_ORIGIN || graph.axesType == (WMG_Axis_Graph.axesTypes)((!isY) ? 3 : 4))
		{
			bool axisTicksRightAbove = otherAxis.AxisTicksRightAbove;
			if (originVal >= otherAxis.AxisMaxValue)
			{
				otherAxis.setAxisBotLeft(rightAbove: false);
				_AxisTicksRightAbove = true;
			}
			else if (originVal <= otherAxis.AxisMinValue)
			{
				otherAxis.setAxisTopRight(rightAbove: false);
				_AxisTicksRightAbove = false;
			}
			else
			{
				otherAxis.setAxisMiddle(rightAbove: false);
				_AxisTicksRightAbove = false;
				_AxisTick = Mathf.RoundToInt((originVal - otherAxis.AxisMinValue) / (otherAxis.AxisMaxValue - otherAxis.AxisMinValue) * (float)(otherAxis.AxisNumTicks - 1));
				_AxisNonTickPercent = (originVal - otherAxis.AxisMinValue) / (otherAxis.AxisMaxValue - otherAxis.AxisMinValue);
			}
			otherAxis.setOtherRightAbove(axisTicksRightAbove);
		}
	}

	public void UpdateAxesGridsAndTicks()
	{
		if (AxisNumTicks <= 1)
		{
			_AxisNumTicks = 1;
			GridLineLength = 0f;
		}
		else
		{
			GridLineLength = AxisLength / (float)(AxisNumTicks - 1);
		}
		if (AxisUseNonTickPercent)
		{
			AxisPercentagePosition = AxisNonTickPercent;
		}
		else if (otherAxis.AxisNumTicks == 1)
		{
			AxisPercentagePosition = 1f;
		}
		else
		{
			AxisPercentagePosition = (float)AxisTick / ((float)otherAxis.AxisNumTicks - 1f);
		}
		SetActive(GridLines, !hideGrid);
		if (!hideGrid)
		{
			WMG_Grid component = GridLines.GetComponent<WMG_Grid>();
			if (isY)
			{
				component.gridNumNodesY = AxisNumTicks;
				component.gridLinkLengthY = GridLineLength;
				component.gridLinkLengthX = otherAxis.AxisLength;
			}
			else
			{
				component.gridNumNodesX = AxisNumTicks;
				component.gridLinkLengthX = GridLineLength;
				component.gridLinkLengthY = otherAxis.AxisLength;
			}
			component.Refresh();
		}
		SetActive(AxisTicks, !hideTicks);
		if (!hideTicks)
		{
			WMG_Grid component2 = AxisTicks.GetComponent<WMG_Grid>();
			if (isY)
			{
				component2.gridNumNodesY = AxisNumTicks;
				component2.gridLinkLengthY = GridLineLength;
			}
			else
			{
				component2.gridNumNodesX = AxisNumTicks;
				component2.gridLinkLengthX = GridLineLength;
			}
			component2.Refresh();
			if (!AxisTicksRightAbove)
			{
				if (isY)
				{
					GameObject axisTicks = AxisTicks;
					float num = AxisPercentagePosition * otherAxis.AxisLength - (float)(graph.axisWidth / 2);
					Vector2 tickSize = graph.tickSize;
					changeSpritePositionToX(axisTicks, num - tickSize.y / 2f);
				}
				else
				{
					GameObject axisTicks2 = AxisTicks;
					float num2 = AxisPercentagePosition * otherAxis.AxisLength - (float)(graph.axisWidth / 2);
					Vector2 tickSize2 = graph.tickSize;
					changeSpritePositionToY(axisTicks2, num2 - tickSize2.y / 2f);
				}
			}
			else if (isY)
			{
				GameObject axisTicks3 = AxisTicks;
				float num3 = AxisPercentagePosition * otherAxis.AxisLength + (float)(graph.axisWidth / 2);
				Vector2 tickSize3 = graph.tickSize;
				changeSpritePositionToX(axisTicks3, num3 + tickSize3.y / 2f);
			}
			else
			{
				GameObject axisTicks4 = AxisTicks;
				float num4 = AxisPercentagePosition * otherAxis.AxisLength + (float)(graph.axisWidth / 2);
				Vector2 tickSize4 = graph.tickSize;
				changeSpritePositionToY(axisTicks4, num4 + tickSize4.y / 2f);
			}
			foreach (WMG_Node axisTickNode in GetAxisTickNodes())
			{
				GameObject objectToScale = axisTickNode.objectToScale;
				float f;
				if (isY)
				{
					Vector2 tickSize5 = graph.tickSize;
					f = tickSize5.y;
				}
				else
				{
					Vector2 tickSize6 = graph.tickSize;
					f = tickSize6.x;
				}
				int aWidth = Mathf.RoundToInt(f);
				float f2;
				if (isY)
				{
					Vector2 tickSize7 = graph.tickSize;
					f2 = tickSize7.x;
				}
				else
				{
					Vector2 tickSize8 = graph.tickSize;
					f2 = tickSize8.y;
				}
				changeSpriteSize(objectToScale, aWidth, Mathf.RoundToInt(f2));
			}
		}
		AxisLinePaddingTot = 2f * AxisLinePadding;
		float num5 = 0f;
		if (!AxisArrowTopRight)
		{
			AxisLinePaddingTot -= AxisLinePadding;
		}
		else
		{
			num5 += AxisLinePadding / 2f;
		}
		if (!AxisArrowBotLeft)
		{
			AxisLinePaddingTot -= AxisLinePadding;
		}
		else
		{
			num5 -= AxisLinePadding / 2f;
		}
		if (isY)
		{
			changeSpriteSize(AxisLine, graph.axisWidth, Mathf.RoundToInt(AxisLength + AxisLinePaddingTot));
			changeSpritePositionTo(AxisLine, new Vector3(0f, num5 + AxisLength / 2f, 0f));
			changeSpritePositionToX(AxisObj, AxisPercentagePosition * otherAxis.AxisLength);
		}
		else
		{
			changeSpriteSize(AxisLine, Mathf.RoundToInt(AxisLength + AxisLinePaddingTot), graph.axisWidth);
			changeSpritePositionTo(AxisLine, new Vector3(num5 + AxisLength / 2f, 0f, 0f));
			changeSpritePositionToY(AxisObj, AxisPercentagePosition * otherAxis.AxisLength);
		}
		SetActiveAnchoredSprite(AxisArrowUR, AxisArrowTopRight);
		SetActiveAnchoredSprite(AxisArrowDL, AxisArrowBotLeft);
	}

	public void UpdateTitle()
	{
		if (AxisTitle != null)
		{
			changeLabelText(AxisTitle, AxisTitleString);
			if (isY)
			{
				GameObject axisTitle = AxisTitle;
				Vector2 axisTitleOffset = AxisTitleOffset;
				float x = axisTitleOffset.x;
				float num = AxisLength / 2f;
				Vector2 axisTitleOffset2 = AxisTitleOffset;
				changeSpritePositionTo(axisTitle, new Vector3(x, num + axisTitleOffset2.y));
			}
			else
			{
				GameObject axisTitle2 = AxisTitle;
				Vector2 axisTitleOffset3 = AxisTitleOffset;
				float x2 = axisTitleOffset3.x + AxisLength / 2f;
				Vector2 axisTitleOffset4 = AxisTitleOffset;
				changeSpritePositionTo(axisTitle2, new Vector3(x2, axisTitleOffset4.y));
			}
			changeLabelFontSize(AxisTitle, AxisTitleFontSize);
		}
	}

	public void UpdateAxesMinMaxValues()
	{
		if (!MinAutoGrow && !MaxAutoGrow && !MinAutoShrink && !MaxAutoShrink)
		{
			return;
		}
		float num = float.PositiveInfinity;
		float num2 = float.NegativeInfinity;
		for (int i = 0; i < graph.lineSeries.Count; i++)
		{
			if (!activeInHierarchy(graph.lineSeries[i]))
			{
				continue;
			}
			WMG_Series component = graph.lineSeries[i].GetComponent<WMG_Series>();
			if (graph.orientationType == WMG_Axis_Graph.orientationTypes.vertical)
			{
				for (int j = 0; j < component.pointValues.Count; j++)
				{
					if (isY)
					{
						Vector2 vector = component.pointValues[j];
						if (vector.y < num)
						{
							Vector2 vector2 = component.pointValues[j];
							num = vector2.y;
						}
						Vector2 vector3 = component.pointValues[j];
						if (vector3.y > num2)
						{
							Vector2 vector4 = component.pointValues[j];
							num2 = vector4.y;
						}
						if ((graph.graphType == WMG_Axis_Graph.graphTypes.bar_stacked || graph.graphType == WMG_Axis_Graph.graphTypes.line_stacked) && graph.TotalPointValues[j] + AxisMinValue > num2)
						{
							num2 = graph.TotalPointValues[j] + AxisMinValue;
						}
					}
					else
					{
						Vector2 vector5 = component.pointValues[j];
						if (vector5.x < num)
						{
							Vector2 vector6 = component.pointValues[j];
							num = vector6.x;
						}
						Vector2 vector7 = component.pointValues[j];
						if (vector7.x > num2)
						{
							Vector2 vector8 = component.pointValues[j];
							num2 = vector8.x;
						}
					}
				}
				continue;
			}
			for (int k = 0; k < component.pointValues.Count; k++)
			{
				if (isY)
				{
					Vector2 vector9 = component.pointValues[k];
					if (vector9.x < num)
					{
						Vector2 vector10 = component.pointValues[k];
						num = vector10.x;
					}
					Vector2 vector11 = component.pointValues[k];
					if (vector11.x > num2)
					{
						Vector2 vector12 = component.pointValues[k];
						num2 = vector12.x;
					}
					continue;
				}
				Vector2 vector13 = component.pointValues[k];
				if (vector13.y < num)
				{
					Vector2 vector14 = component.pointValues[k];
					num = vector14.y;
				}
				Vector2 vector15 = component.pointValues[k];
				if (vector15.y > num2)
				{
					Vector2 vector16 = component.pointValues[k];
					num2 = vector16.y;
				}
				if ((graph.graphType == WMG_Axis_Graph.graphTypes.bar_stacked || graph.graphType == WMG_Axis_Graph.graphTypes.line_stacked) && graph.TotalPointValues[k] + AxisMinValue > num2)
				{
					num2 = graph.TotalPointValues[k] + AxisMinValue;
				}
			}
		}
		if ((MinAutoGrow || MaxAutoGrow || MinAutoShrink || MaxAutoShrink) && num != num2 && num != float.PositiveInfinity && num2 != float.NegativeInfinity)
		{
			float axisMaxValue = AxisMaxValue;
			float axisMinValue = AxisMinValue;
			if (MaxAutoGrow && num2 > axisMaxValue)
			{
				AutoSetAxisMinMax(num2, num, max: true, grow: true, axisMinValue, axisMaxValue);
			}
			if (MinAutoGrow && num < axisMinValue)
			{
				AutoSetAxisMinMax(num, num2, max: false, grow: true, axisMinValue, axisMaxValue);
			}
			if (MaxAutoShrink && graph.autoShrinkAtPercent > (num2 - axisMinValue) / (axisMaxValue - axisMinValue))
			{
				AutoSetAxisMinMax(num2, num, max: true, grow: false, axisMinValue, axisMaxValue);
			}
			if (MinAutoShrink && graph.autoShrinkAtPercent > (axisMaxValue - num) / (axisMaxValue - axisMinValue))
			{
				AutoSetAxisMinMax(num, num2, max: false, grow: false, axisMinValue, axisMaxValue);
			}
		}
	}

	private void AutoSetAxisMinMax(float val, float val2, bool max, bool grow, float aMin, float aMax)
	{
		int num = 0;
		num = AxisNumTicks - 1;
		float num2 = 1f + graph.autoGrowAndShrinkByPercent;
		float num3 = 0f;
		num3 = (max ? ((!grow) ? (num2 * (val - val2) / (float)num) : (num2 * (val - aMin) / (float)num)) : ((!grow) ? (num2 * (val2 - val) / (float)num) : (num2 * (aMax - val) / (float)num)));
		if (num3 == 0f || aMax <= aMin)
		{
			return;
		}
		float num4 = num3;
		int num5 = 0;
		if (Mathf.Abs(num4) > 1f)
		{
			while (Mathf.Abs(num4) > 10f)
			{
				num5++;
				num4 /= 10f;
			}
		}
		else
		{
			while (Mathf.Abs(num4) < 0.1f)
			{
				num5--;
				num4 *= 10f;
			}
		}
		float num6 = Mathf.Pow(10f, num5 - 1);
		num4 = num3 - num3 % num6 + num6;
		float num7 = 0f;
		num7 = (max ? ((!grow) ? ((float)num * num4 + val2) : ((float)num * num4 + aMin)) : ((!grow) ? (val2 - (float)num * num4) : (aMax - (float)num * num4)));
		if (max)
		{
			AxisMaxValue = num7;
		}
		else
		{
			AxisMinValue = num7;
		}
	}

	public void UpdateAxesLabels()
	{
		int num = 0;
		num = ((LabelType == labelTypes.ticks) ? AxisNumTicks : ((LabelType == labelTypes.ticks_center) ? (AxisNumTicks - 1) : ((LabelType != labelTypes.groups) ? axisLabels.Count : graph.groups.Count)));
		float distBetween = graph.getDistBetween(graph.groups.Count, AxisLength);
		if (LabelType == labelTypes.ticks)
		{
			_AxisLabelDistBetween = AxisLength / (float)(num - 1);
		}
		else if (LabelType == labelTypes.ticks_center)
		{
			_AxisLabelDistBetween = AxisLength / (float)num;
		}
		else if (LabelType == labelTypes.groups)
		{
			_AxisLabelDistBetween = distBetween;
		}
		WMG_Grid component = AxisLabelObjs.GetComponent<WMG_Grid>();
		if (isY)
		{
			component.gridNumNodesY = num;
			component.gridLinkLengthY = AxisLabelDistBetween;
		}
		else
		{
			component.gridNumNodesX = num;
			component.gridLinkLengthX = AxisLabelDistBetween;
		}
		component.Refresh();
		for (int i = 0; i < num; i++)
		{
			if (axisLabels.Count <= i)
			{
				axisLabels.AddNoCb(string.Empty, ref _axisLabels);
			}
		}
		for (int num2 = axisLabels.Count - 1; num2 >= 0; num2--)
		{
			if (num2 >= num)
			{
				axisLabels.RemoveAtNoCb(num2, ref _axisLabels);
			}
		}
		if (LabelType == labelTypes.ticks)
		{
			_AxisLabelSpacing = 0f;
		}
		else if (LabelType == labelTypes.ticks_center)
		{
			if (AxisNumTicks == 1)
			{
				_AxisLabelSpacing = 0f;
			}
			else
			{
				_AxisLabelSpacing = AxisLength / (float)(AxisNumTicks - 1) / 2f;
			}
		}
		else if (LabelType == labelTypes.groups)
		{
			if (graph.graphType == WMG_Axis_Graph.graphTypes.line || graph.graphType == WMG_Axis_Graph.graphTypes.line_stacked)
			{
				_AxisLabelSpacing = 0f;
			}
			else
			{
				_AxisLabelSpacing = distBetween / 2f;
				if (graph.graphType == WMG_Axis_Graph.graphTypes.bar_side)
				{
					_AxisLabelSpacing += (float)graph.lineSeries.Count * graph.barWidth / 2f;
				}
				else if (graph.graphType == WMG_Axis_Graph.graphTypes.bar_stacked)
				{
					_AxisLabelSpacing += graph.barWidth / 2f;
				}
				else if (graph.graphType == WMG_Axis_Graph.graphTypes.bar_stacked_percent)
				{
					_AxisLabelSpacing += graph.barWidth / 2f;
				}
				else if (graph.graphType == WMG_Axis_Graph.graphTypes.combo)
				{
					_AxisLabelSpacing += (float)graph.NumComboBarSeries() * graph.barWidth / 2f;
				}
				if (isY)
				{
					_AxisLabelSpacing += 2f;
				}
			}
		}
		float num3 = 0f;
		if (LabelType == labelTypes.ticks || (LabelType == labelTypes.groups && AxisNumTicks == graph.groups.Count))
		{
			Vector2 tickSize = graph.tickSize;
			num3 = tickSize.y;
		}
		if (isY)
		{
			if (!AxisTicksRightAbove)
			{
				changeSpritePositionToX(AxisLabelObjs, AxisPercentagePosition * otherAxis.AxisLength - num3 - (float)(graph.axisWidth / 2));
			}
			else
			{
				changeSpritePositionToX(AxisLabelObjs, AxisPercentagePosition * otherAxis.AxisLength + num3 + (float)(graph.axisWidth / 2));
			}
		}
		else if (!AxisTicksRightAbove)
		{
			changeSpritePositionToY(AxisLabelObjs, AxisPercentagePosition * otherAxis.AxisLength - num3 - (float)(graph.axisWidth / 2));
		}
		else
		{
			changeSpritePositionToY(AxisLabelObjs, AxisPercentagePosition * otherAxis.AxisLength + num3 + (float)(graph.axisWidth / 2));
		}
		List<WMG_Node> axisLabelNodes = GetAxisLabelNodes();
		if (axisLabelNodes == null)
		{
			return;
		}
		for (int j = 0; j < axisLabels.Count && j < axisLabelNodes.Count; j++)
		{
			SetActive(axisLabelNodes[j].gameObject, !hideLabels);
			if (LabelType == labelTypes.ticks && hideTick && j == otherAxis.AxisTick)
			{
				SetActive(axisLabelNodes[otherAxis.AxisTick].gameObject, state: false);
			}
			if (!graph._autoFitting)
			{
				axisLabelNodes[j].objectToLabel.transform.localEulerAngles = new Vector3(0f, 0f, AxisLabelRotation);
			}
			if (!isY && !graph.autoFitLabels)
			{
				if (AxisLabelRotation > 0f)
				{
					if (!AxisTicksRightAbove)
					{
						changeSpritePivot(axisLabelNodes[j].objectToLabel, WMGpivotTypes.TopRight);
					}
					else
					{
						changeSpritePivot(axisLabelNodes[j].objectToLabel, WMGpivotTypes.BottomLeft);
					}
				}
				else if (!AxisTicksRightAbove)
				{
					changeSpritePivot(axisLabelNodes[j].objectToLabel, WMGpivotTypes.Top);
				}
				else
				{
					changeSpritePivot(axisLabelNodes[j].objectToLabel, WMGpivotTypes.Bottom);
				}
			}
			if (isY)
			{
				if (!AxisTicksRightAbove)
				{
					changeSpritePivot(axisLabelNodes[j].objectToLabel, WMGpivotTypes.Right);
					changeSpritePositionTo(axisLabelNodes[j].objectToLabel, new Vector3(0f - AxisLabelSpaceOffset, AxisLabelSpacing, 0f));
				}
				else
				{
					changeSpritePivot(axisLabelNodes[j].objectToLabel, WMGpivotTypes.Left);
					changeSpritePositionTo(axisLabelNodes[j].objectToLabel, new Vector3(AxisLabelSpaceOffset, AxisLabelSpacing, 0f));
				}
			}
			else if (!AxisTicksRightAbove)
			{
				changeSpritePositionTo(axisLabelNodes[j].objectToLabel, new Vector3(AxisLabelSpacing, 0f - AxisLabelSpaceOffset, 0f));
			}
			else
			{
				changeSpritePositionTo(axisLabelNodes[j].objectToLabel, new Vector3(AxisLabelSpacing, AxisLabelSpaceOffset, 0f));
			}
			if (!graph._autoFitting)
			{
				changeLabelFontSize(axisLabelNodes[j].objectToLabel, AxisLabelSize);
			}
			axisLabels.SetValNoCb(j, axisLabelLabeler(this, j), ref _axisLabels);
			changeLabelText(axisLabelNodes[j].objectToLabel, axisLabels[j]);
		}
	}

	private string defaultAxisLabelLabeler(WMG_Axis axis, int labelIndex)
	{
		if (axis.LabelType == labelTypes.groups)
		{
			return ((labelIndex - axis.AxisLabelSkipStart) % (axis.AxisLabelSkipInterval + 1) != 0) ? string.Empty : ((labelIndex < axis.AxisLabelSkipStart) ? string.Empty : axis.graph.groups[labelIndex]);
		}
		if (axis.SetLabelsUsingMaxMin)
		{
			float num = axis.AxisMinValue + (float)labelIndex * (axis.AxisMaxValue - axis.AxisMinValue) / (float)(axis.axisLabels.Count - 1);
			if (labelIndex == 0)
			{
				num = axis.AxisMinValue;
			}
			if (axis.graph.graphType == WMG_Axis_Graph.graphTypes.bar_stacked_percent && ((axis.isY && axis.graph.orientationType == WMG_Axis_Graph.orientationTypes.vertical) || (!axis.isY && axis.graph.orientationType == WMG_Axis_Graph.orientationTypes.horizontal)))
			{
				num = (float)labelIndex / ((float)axis.axisLabels.Count - 1f) * 100f;
			}
			float num2 = Mathf.Pow(10f, axis.numDecimalsAxisLabels);
			string text = ((labelIndex - axis.AxisLabelSkipStart) % (axis.AxisLabelSkipInterval + 1) != 0) ? string.Empty : ((labelIndex < axis.AxisLabelSkipStart) ? string.Empty : (Mathf.Round(num * num2) / num2).ToString());
			if (axis.graph.graphType == WMG_Axis_Graph.graphTypes.bar_stacked_percent && ((axis.isY && axis.graph.orientationType == WMG_Axis_Graph.orientationTypes.vertical) || (!axis.isY && axis.graph.orientationType == WMG_Axis_Graph.orientationTypes.horizontal)))
			{
				return (!string.IsNullOrEmpty(text)) ? (text + "%") : string.Empty;
			}
			return text;
		}
		return axis.axisLabels[labelIndex];
	}

	public void AutofitAxesLabels()
	{
		if (!graph.autoFitLabels || graph._autoFitting)
		{
			return;
		}
		graph._autoFitting = true;
		List<WMG_Node> axisLabelNodes = GetAxisLabelNodes();
		float num = graph.autoFitPadding;
		float num2 = graph.autoFitPadding;
		float num3 = graph.autoFitPadding;
		float num4 = graph.autoFitPadding;
		if (!graph.legend.hideLegend && graph.legend.offset >= 0f)
		{
			if (graph.legend.legendType == WMG_Legend.legendTypes.Bottom)
			{
				if (graph.legend.oppositeSideLegend)
				{
					num3 += (float)graph.legend.LegendHeight + graph.legend.offset;
				}
				else
				{
					num4 += (float)graph.legend.LegendHeight + graph.legend.offset;
				}
			}
			else if (graph.legend.oppositeSideLegend)
			{
				num += (float)graph.legend.LegendWidth + graph.legend.offset;
			}
			else
			{
				num2 += (float)graph.legend.LegendWidth + graph.legend.offset;
			}
		}
		float autoFitMaxBorder = this.autoFitMaxBorder;
		Vector2 zero = Vector2.zero;
		Vector2 vector2;
		if (isY)
		{
			zero = getLabelsMaxDiff(axisLabelNodes, AxisTicksRightAbove, AxisTicksRightAbove, num, num2, num3, num4);
			if (Mathf.Abs(zero.x) > 1f || Mathf.Abs(zero.y) > 1f)
			{
				if (AxisTicksRightAbove)
				{
					WMG_Axis_Graph wMG_Axis_Graph = graph;
					Vector2 paddingLeftRight = graph.paddingLeftRight;
					float x = paddingLeftRight.x;
					Vector2 paddingLeftRight2 = graph.paddingLeftRight;
					wMG_Axis_Graph.paddingLeftRight = new Vector2(x, paddingLeftRight2.y - zero.x);
					WMG_Axis_Graph wMG_Axis_Graph2 = graph;
					Vector2 paddingTopBottom = graph.paddingTopBottom;
					float x2 = paddingTopBottom.x - zero.y;
					Vector2 paddingTopBottom2 = graph.paddingTopBottom;
					wMG_Axis_Graph2.paddingTopBottom = new Vector2(x2, paddingTopBottom2.y);
				}
				else
				{
					WMG_Axis_Graph wMG_Axis_Graph3 = graph;
					Vector2 paddingLeftRight3 = graph.paddingLeftRight;
					float x3 = paddingLeftRight3.x - zero.x;
					Vector2 paddingLeftRight4 = graph.paddingLeftRight;
					wMG_Axis_Graph3.paddingLeftRight = new Vector2(x3, paddingLeftRight4.y);
					WMG_Axis_Graph wMG_Axis_Graph4 = graph;
					Vector2 paddingTopBottom3 = graph.paddingTopBottom;
					float x4 = paddingTopBottom3.x;
					Vector2 paddingTopBottom4 = graph.paddingTopBottom;
					wMG_Axis_Graph4.paddingTopBottom = new Vector2(x4, paddingTopBottom4.y - zero.y);
				}
				Vector2 vector;
				if (AxisTicksRightAbove)
				{
					Vector2 paddingLeftRight5 = graph.paddingLeftRight;
					vector = new Vector2(paddingLeftRight5.x, autoFitMaxBorder * getSpriteWidth(graph.gameObject) + num2);
				}
				else
				{
					float x5 = autoFitMaxBorder * getSpriteWidth(graph.gameObject) + num;
					Vector2 paddingLeftRight6 = graph.paddingLeftRight;
					vector = new Vector2(x5, paddingLeftRight6.y);
				}
				vector2 = vector;
				if (AxisTicksRightAbove)
				{
					Vector2 paddingLeftRight7 = graph.paddingLeftRight;
					if (paddingLeftRight7.y > vector2.y)
					{
						goto IL_03ad;
					}
				}
				if (!AxisTicksRightAbove)
				{
					Vector2 paddingLeftRight8 = graph.paddingLeftRight;
					if (paddingLeftRight8.x > vector2.x)
					{
						goto IL_03ad;
					}
				}
				goto IL_0413;
			}
			goto IL_0bb7;
		}
		bool flag = false;
		bool flag2 = false;
		if (otherAxis.AxisTicksRightAbove)
		{
			flag2 = true;
		}
		else
		{
			flag = true;
		}
		bool flag3 = true;
		for (int i = 1; i < axisLabelNodes.Count; i++)
		{
			flag3 = (flag3 && !rectIntersectRect(axisLabelNodes[i - 1].objectToLabel, axisLabelNodes[i].objectToLabel));
		}
		if (!flag3)
		{
			setLabelRotations(axisLabelNodes, autoFitRotation);
		}
		WMGpivotTypes wMGpivotTypes = WMGpivotTypes.Top;
		if (axisLabelNodes.Count > 0)
		{
			Vector3 localEulerAngles = axisLabelNodes[0].objectToLabel.transform.localEulerAngles;
			if (localEulerAngles.z > 0f)
			{
				wMGpivotTypes = (AxisTicksRightAbove ? WMGpivotTypes.BottomLeft : WMGpivotTypes.TopRight);
				goto IL_063d;
			}
		}
		wMGpivotTypes = ((!AxisTicksRightAbove) ? WMGpivotTypes.Top : WMGpivotTypes.Bottom);
		goto IL_063d;
		IL_0a19:
		Vector2 vector3;
		if (AxisTicksRightAbove)
		{
			WMG_Axis_Graph wMG_Axis_Graph5 = graph;
			Vector2 paddingLeftRight9 = graph.paddingLeftRight;
			wMG_Axis_Graph5.paddingLeftRight = new Vector2(paddingLeftRight9.x, vector3.y);
		}
		else
		{
			WMG_Axis_Graph wMG_Axis_Graph6 = graph;
			float x6 = vector3.x;
			Vector2 paddingLeftRight10 = graph.paddingLeftRight;
			wMG_Axis_Graph6.paddingLeftRight = new Vector2(x6, paddingLeftRight10.y);
		}
		goto IL_0a7f;
		IL_0b46:
		Vector2 vector4;
		if (AxisTicksRightAbove)
		{
			WMG_Axis_Graph wMG_Axis_Graph7 = graph;
			float x7 = vector4.x;
			Vector2 paddingTopBottom5 = graph.paddingTopBottom;
			wMG_Axis_Graph7.paddingTopBottom = new Vector2(x7, paddingTopBottom5.y);
		}
		else
		{
			WMG_Axis_Graph wMG_Axis_Graph8 = graph;
			Vector2 paddingTopBottom6 = graph.paddingTopBottom;
			wMG_Axis_Graph8.paddingTopBottom = new Vector2(paddingTopBottom6.x, vector4.y);
		}
		goto IL_0bac;
		IL_03ad:
		if (AxisTicksRightAbove)
		{
			WMG_Axis_Graph wMG_Axis_Graph9 = graph;
			Vector2 paddingLeftRight11 = graph.paddingLeftRight;
			wMG_Axis_Graph9.paddingLeftRight = new Vector2(paddingLeftRight11.x, vector2.y);
		}
		else
		{
			WMG_Axis_Graph wMG_Axis_Graph10 = graph;
			float x8 = vector2.x;
			Vector2 paddingLeftRight12 = graph.paddingLeftRight;
			wMG_Axis_Graph10.paddingLeftRight = new Vector2(x8, paddingLeftRight12.y);
		}
		goto IL_0413;
		IL_0413:
		Vector2 vector5;
		if (!AxisTicksRightAbove)
		{
			Vector2 paddingTopBottom7 = graph.paddingTopBottom;
			vector5 = new Vector2(paddingTopBottom7.x, autoFitMaxBorder * getSpriteHeight(graph.gameObject) + num4);
		}
		else
		{
			float x9 = autoFitMaxBorder * getSpriteHeight(graph.gameObject) + num3;
			Vector2 paddingTopBottom8 = graph.paddingTopBottom;
			vector5 = new Vector2(x9, paddingTopBottom8.y);
		}
		Vector2 vector6 = vector5;
		if (!AxisTicksRightAbove)
		{
			Vector2 paddingTopBottom9 = graph.paddingTopBottom;
			if (paddingTopBottom9.y > vector6.y)
			{
				goto IL_04da;
			}
		}
		if (AxisTicksRightAbove)
		{
			Vector2 paddingTopBottom10 = graph.paddingTopBottom;
			if (paddingTopBottom10.x > vector6.x)
			{
				goto IL_04da;
			}
		}
		goto IL_0540;
		IL_04da:
		if (AxisTicksRightAbove)
		{
			WMG_Axis_Graph wMG_Axis_Graph11 = graph;
			float x10 = vector6.x;
			Vector2 paddingTopBottom11 = graph.paddingTopBottom;
			wMG_Axis_Graph11.paddingTopBottom = new Vector2(x10, paddingTopBottom11.y);
		}
		else
		{
			WMG_Axis_Graph wMG_Axis_Graph12 = graph;
			Vector2 paddingTopBottom12 = graph.paddingTopBottom;
			wMG_Axis_Graph12.paddingTopBottom = new Vector2(paddingTopBottom12.x, vector6.y);
		}
		goto IL_0540;
		IL_0540:
		graph.UpdateBG();
		goto IL_0bb7;
		IL_0bb7:
		graph.GraphChanged();
		graph._autoFitting = false;
		return;
		IL_0bac:
		graph.UpdateBG();
		goto IL_0bb7;
		IL_063d:
		foreach (WMG_Node item in axisLabelNodes)
		{
			changeSpritePivot(item.objectToLabel, wMGpivotTypes);
		}
		zero = getLabelsMaxDiff(axisLabelNodes, AxisTicksRightAbove, AxisTicksRightAbove, num, num2, num3, num4);
		if (Mathf.Abs(zero.x) > 1f || Mathf.Abs(zero.y) > 1f)
		{
			if (AxisTicksRightAbove)
			{
				if (flag2)
				{
					WMG_Axis_Graph wMG_Axis_Graph13 = graph;
					Vector2 paddingLeftRight13 = graph.paddingLeftRight;
					float x11 = paddingLeftRight13.x;
					Vector2 paddingLeftRight14 = graph.paddingLeftRight;
					float a = paddingLeftRight14.y - zero.x;
					Vector2 paddingLeftRight15 = graph.paddingLeftRight;
					wMG_Axis_Graph13.paddingLeftRight = new Vector2(x11, Mathf.Max(a, paddingLeftRight15.y));
					WMG_Axis_Graph wMG_Axis_Graph14 = graph;
					Vector2 paddingTopBottom13 = graph.paddingTopBottom;
					float a2 = paddingTopBottom13.x - zero.y;
					Vector2 paddingTopBottom14 = graph.paddingTopBottom;
					float x12 = Mathf.Max(a2, paddingTopBottom14.x);
					Vector2 paddingTopBottom15 = graph.paddingTopBottom;
					wMG_Axis_Graph14.paddingTopBottom = new Vector2(x12, paddingTopBottom15.y);
				}
				else
				{
					WMG_Axis_Graph wMG_Axis_Graph15 = graph;
					Vector2 paddingLeftRight16 = graph.paddingLeftRight;
					float x13 = paddingLeftRight16.x;
					Vector2 paddingLeftRight17 = graph.paddingLeftRight;
					wMG_Axis_Graph15.paddingLeftRight = new Vector2(x13, paddingLeftRight17.y - zero.x);
					WMG_Axis_Graph wMG_Axis_Graph16 = graph;
					Vector2 paddingTopBottom16 = graph.paddingTopBottom;
					float x14 = paddingTopBottom16.x - zero.y;
					Vector2 paddingTopBottom17 = graph.paddingTopBottom;
					wMG_Axis_Graph16.paddingTopBottom = new Vector2(x14, paddingTopBottom17.y);
				}
			}
			else if (flag)
			{
				WMG_Axis_Graph wMG_Axis_Graph17 = graph;
				Vector2 paddingLeftRight18 = graph.paddingLeftRight;
				float a3 = paddingLeftRight18.x - zero.x;
				Vector2 paddingLeftRight19 = graph.paddingLeftRight;
				float x15 = Mathf.Max(a3, paddingLeftRight19.x);
				Vector2 paddingLeftRight20 = graph.paddingLeftRight;
				wMG_Axis_Graph17.paddingLeftRight = new Vector2(x15, paddingLeftRight20.y);
				WMG_Axis_Graph wMG_Axis_Graph18 = graph;
				Vector2 paddingTopBottom18 = graph.paddingTopBottom;
				float x16 = paddingTopBottom18.x;
				Vector2 paddingTopBottom19 = graph.paddingTopBottom;
				float a4 = paddingTopBottom19.y - zero.y;
				Vector2 paddingTopBottom20 = graph.paddingTopBottom;
				wMG_Axis_Graph18.paddingTopBottom = new Vector2(x16, Mathf.Max(a4, paddingTopBottom20.y));
			}
			else
			{
				WMG_Axis_Graph wMG_Axis_Graph19 = graph;
				Vector2 paddingLeftRight21 = graph.paddingLeftRight;
				float x17 = paddingLeftRight21.x - zero.x;
				Vector2 paddingLeftRight22 = graph.paddingLeftRight;
				wMG_Axis_Graph19.paddingLeftRight = new Vector2(x17, paddingLeftRight22.y);
				WMG_Axis_Graph wMG_Axis_Graph20 = graph;
				Vector2 paddingTopBottom21 = graph.paddingTopBottom;
				float x18 = paddingTopBottom21.x;
				Vector2 paddingTopBottom22 = graph.paddingTopBottom;
				wMG_Axis_Graph20.paddingTopBottom = new Vector2(x18, paddingTopBottom22.y - zero.y);
			}
			Vector2 vector7;
			if (AxisTicksRightAbove)
			{
				Vector2 paddingLeftRight23 = graph.paddingLeftRight;
				vector7 = new Vector2(paddingLeftRight23.x, autoFitMaxBorder * getSpriteWidth(graph.gameObject) + num2);
			}
			else
			{
				float x19 = autoFitMaxBorder * getSpriteWidth(graph.gameObject) + num;
				Vector2 paddingLeftRight24 = graph.paddingLeftRight;
				vector7 = new Vector2(x19, paddingLeftRight24.y);
			}
			vector3 = vector7;
			if (AxisTicksRightAbove)
			{
				Vector2 paddingLeftRight25 = graph.paddingLeftRight;
				if (paddingLeftRight25.y > vector3.y)
				{
					goto IL_0a19;
				}
			}
			if (!AxisTicksRightAbove)
			{
				Vector2 paddingLeftRight26 = graph.paddingLeftRight;
				if (paddingLeftRight26.x > vector3.x)
				{
					goto IL_0a19;
				}
			}
			goto IL_0a7f;
		}
		goto IL_0bb7;
		IL_0a7f:
		Vector2 vector8;
		if (!AxisTicksRightAbove)
		{
			Vector2 paddingTopBottom23 = graph.paddingTopBottom;
			vector8 = new Vector2(paddingTopBottom23.x, autoFitMaxBorder * getSpriteHeight(graph.gameObject) + num4);
		}
		else
		{
			float x20 = autoFitMaxBorder * getSpriteHeight(graph.gameObject) + num3;
			Vector2 paddingTopBottom24 = graph.paddingTopBottom;
			vector8 = new Vector2(x20, paddingTopBottom24.y);
		}
		vector4 = vector8;
		if (!AxisTicksRightAbove)
		{
			Vector2 paddingTopBottom25 = graph.paddingTopBottom;
			if (paddingTopBottom25.y > vector4.y)
			{
				goto IL_0b46;
			}
		}
		if (AxisTicksRightAbove)
		{
			Vector2 paddingTopBottom26 = graph.paddingTopBottom;
			if (paddingTopBottom26.x > vector4.x)
			{
				goto IL_0b46;
			}
		}
		goto IL_0bac;
	}

	private Vector2 getLabelsMaxDiff(List<WMG_Node> LabelNodes, bool isRight, bool isTop, float paddingLeft, float paddingRight, float paddingTop, float paddingBot)
	{
		float num = float.PositiveInfinity;
		float num2 = float.PositiveInfinity;
		Vector2 xDif = Vector2.zero;
		Vector2 yDif = Vector2.zero;
		foreach (WMG_Node LabelNode in LabelNodes)
		{
			getRectDiffs(LabelNode.objectToLabel, graph.gameObject, ref xDif, ref yDif);
			if (isRight)
			{
				if (xDif.y < num)
				{
					num = xDif.y;
				}
			}
			else if (xDif.x < num)
			{
				num = xDif.x;
			}
			if (isTop)
			{
				if (yDif.y < num2)
				{
					num2 = yDif.y;
				}
			}
			else if (yDif.x < num2)
			{
				num2 = yDif.x;
			}
		}
		return new Vector2(num - ((!isRight) ? paddingLeft : paddingRight), num2 - ((!isTop) ? paddingBot : paddingTop));
	}

	private void setLabelRotations(List<WMG_Node> LabelNodes, float rotation)
	{
		foreach (WMG_Node LabelNode in LabelNodes)
		{
			LabelNode.objectToLabel.transform.localEulerAngles = new Vector3(0f, 0f, rotation);
		}
	}

	private void setFontSizeLabels(List<WMG_Node> LabelNodes, int newLabelSize)
	{
		foreach (WMG_Node LabelNode in LabelNodes)
		{
			changeLabelFontSize(LabelNode.objectToLabel, newLabelSize);
		}
	}

	public void setLabelScales(float newScale)
	{
		foreach (WMG_Node axisLabelNode in GetAxisLabelNodes())
		{
			axisLabelNode.objectToLabel.transform.localScale = new Vector3(newScale, newScale, 1f);
		}
	}

	public List<WMG_Node> GetAxisLabelNodes()
	{
		WMG_Grid component = AxisLabelObjs.GetComponent<WMG_Grid>();
		if (isY)
		{
			return component.getColumn(0);
		}
		return component.getRow(0);
	}

	public List<WMG_Node> GetAxisTickNodes()
	{
		WMG_Grid component = AxisTicks.GetComponent<WMG_Grid>();
		if (isY)
		{
			return component.getColumn(0);
		}
		return component.getRow(0);
	}
}
