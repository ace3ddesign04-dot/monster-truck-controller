using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WMG_Axis_Graph : WMG_Graph_Manager
{
	public enum graphTypes
	{
		line,
		line_stacked,
		bar_side,
		bar_stacked,
		bar_stacked_percent,
		combo
	}

	public enum orientationTypes
	{
		vertical,
		horizontal
	}

	public enum axesTypes
	{
		MANUAL,
		CENTER,
		AUTO_ORIGIN,
		AUTO_ORIGIN_X,
		AUTO_ORIGIN_Y,
		I,
		II,
		III,
		IV,
		I_II,
		III_IV,
		II_III,
		I_IV
	}

	[Flags]
	public enum ResizeProperties
	{
		SeriesPointSize = 0x1,
		SeriesLineWidth = 0x2,
		SeriesDataLabelSize = 0x4,
		SeriesDataLabelOffset = 0x8,
		LegendFontSize = 0x10,
		LegendEntrySize = 0x20,
		LegendOffset = 0x40,
		AxesWidth = 0x80,
		AxesLabelSize = 0x100,
		AxesLabelOffset = 0x200,
		AxesTitleSize = 0x400,
		AxesLinePadding = 0x800,
		AxesArrowSize = 0x1000,
		AutofitPadding = 0x2000,
		BorderPadding = 0x4000,
		TickSize = 0x8000
	}

	public delegate void GraphBackgroundChangedHandler(WMG_Axis_Graph aGraph);

	[SerializeField]
	public WMG_Axis yAxis;

	[SerializeField]
	public WMG_Axis xAxis;

	[SerializeField]
	private List<string> _groups;

	public WMG_List<string> groups = new WMG_List<string>();

	public Vector2 tooltipOffset;

	public int tooltipNumberDecimals;

	public bool tooltipDisplaySeriesName;

	public bool tooltipAnimationsEnabled;

	public Ease tooltipAnimationsEasetype;

	public float tooltipAnimationsDuration;

	public Ease autoAnimationsEasetype;

	public float autoAnimationsDuration;

	public List<GameObject> lineSeries;

	public List<UnityEngine.Object> pointPrefabs;

	public List<UnityEngine.Object> linkPrefabs;

	public UnityEngine.Object barPrefab;

	public UnityEngine.Object seriesPrefab;

	public WMG_Legend legend;

	public GameObject graphTitle;

	public GameObject graphBackground;

	public GameObject anchoredParent;

	public GameObject seriesParent;

	public GameObject toolTipPanel;

	public GameObject toolTipLabel;

	[SerializeField]
	private graphTypes _graphType;

	[SerializeField]
	private orientationTypes _orientationType;

	[SerializeField]
	private axesTypes _axesType;

	[SerializeField]
	private bool _resizeEnabled;

	[WMG_EnumFlag]
	[SerializeField]
	private ResizeProperties _resizeProperties;

	[SerializeField]
	private bool _useGroups;

	[SerializeField]
	private Vector2 _paddingLeftRight;

	[SerializeField]
	private Vector2 _paddingTopBottom;

	[SerializeField]
	private Vector2 _theOrigin;

	[SerializeField]
	private float _barWidth;

	[SerializeField]
	private float _barAxisValue;

	[SerializeField]
	private bool _autoUpdateOrigin;

	[SerializeField]
	private bool _autoUpdateBarWidth;

	[SerializeField]
	private float _autoUpdateBarWidthSpacing;

	[SerializeField]
	private bool _autoUpdateSeriesAxisSpacing;

	[SerializeField]
	private bool _autoUpdateBarAxisValue;

	[SerializeField]
	private int _axisWidth;

	[SerializeField]
	private float _autoShrinkAtPercent;

	[SerializeField]
	private float _autoGrowAndShrinkByPercent;

	[SerializeField]
	private bool _tooltipEnabled;

	[SerializeField]
	private bool _autoAnimationsEnabled;

	[SerializeField]
	private bool _autoFitLabels;

	[SerializeField]
	private float _autoFitPadding;

	[SerializeField]
	private Vector2 _tickSize;

	[SerializeField]
	private string _graphTitleString;

	[SerializeField]
	private Vector2 _graphTitleOffset;

	private List<float> totalPointValues = new List<float>();

	private int maxSeriesPointCount;

	private int maxSeriesBarCount;

	private int numComboBarSeries;

	private float origWidth;

	private float origHeight;

	private float origBarWidth;

	private float origAxisWidth;

	private float origAutoFitPadding;

	private Vector2 origTickSize;

	private Vector2 origPaddingLeftRight;

	private Vector2 origPaddingTopBottom;

	private float cachedContainerWidth;

	private float cachedContainerHeight;

	public WMG_Graph_Tooltip theTooltip;

	private WMG_Graph_Auto_Anim autoAnim;

	private bool hasInit;

	private List<WMG_Change_Obj> changeObjs = new List<WMG_Change_Obj>();

	public WMG_Change_Obj graphC = new WMG_Change_Obj();

	public WMG_Change_Obj resizeC = new WMG_Change_Obj();

	public WMG_Change_Obj seriesCountC = new WMG_Change_Obj();

	public WMG_Change_Obj seriesNoCountC = new WMG_Change_Obj();

	private WMG_Change_Obj tooltipEnabledC = new WMG_Change_Obj();

	private WMG_Change_Obj autoAnimEnabledC = new WMG_Change_Obj();

	private WMG_Change_Obj orientationC = new WMG_Change_Obj();

	private WMG_Change_Obj graphTypeC = new WMG_Change_Obj();

	public graphTypes graphType
	{
		get
		{
			return _graphType;
		}
		set
		{
			if (_graphType != value)
			{
				_graphType = value;
				graphTypeC.Changed();
				graphC.Changed();
				seriesCountC.Changed();
				legend.legendC.Changed();
			}
		}
	}

	public orientationTypes orientationType
	{
		get
		{
			return _orientationType;
		}
		set
		{
			if (_orientationType != value)
			{
				_orientationType = value;
				orientationC.Changed();
				graphC.Changed();
				seriesNoCountC.Changed();
			}
		}
	}

	public axesTypes axesType
	{
		get
		{
			return _axesType;
		}
		set
		{
			if (_axesType != value)
			{
				_axesType = value;
				graphC.Changed();
				seriesNoCountC.Changed();
			}
		}
	}

	public bool resizeEnabled
	{
		get
		{
			return _resizeEnabled;
		}
		set
		{
			if (_resizeEnabled != value)
			{
				_resizeEnabled = value;
				resizeC.Changed();
			}
		}
	}

	public ResizeProperties resizeProperties
	{
		get
		{
			return _resizeProperties;
		}
		set
		{
			if (_resizeProperties != value)
			{
				_resizeProperties = value;
				resizeC.Changed();
			}
		}
	}

	public bool useGroups
	{
		get
		{
			return _useGroups;
		}
		set
		{
			if (_useGroups != value)
			{
				_useGroups = value;
				graphC.Changed();
			}
		}
	}

	public Vector2 paddingLeftRight
	{
		get
		{
			return _paddingLeftRight;
		}
		set
		{
			if (_paddingLeftRight != value)
			{
				_paddingLeftRight = value;
				graphC.Changed();
				seriesCountC.Changed();
				legend.legendC.Changed();
			}
		}
	}

	public Vector2 paddingTopBottom
	{
		get
		{
			return _paddingTopBottom;
		}
		set
		{
			if (_paddingTopBottom != value)
			{
				_paddingTopBottom = value;
				graphC.Changed();
				seriesCountC.Changed();
				legend.legendC.Changed();
			}
		}
	}

	public Vector2 theOrigin
	{
		get
		{
			return _theOrigin;
		}
		set
		{
			if (_theOrigin != value)
			{
				_theOrigin = value;
				graphC.Changed();
				seriesNoCountC.Changed();
			}
		}
	}

	public float barWidth
	{
		get
		{
			return _barWidth;
		}
		set
		{
			if (_barWidth != value)
			{
				_barWidth = value;
				graphC.Changed();
				seriesNoCountC.Changed();
			}
		}
	}

	public float barAxisValue
	{
		get
		{
			return _barAxisValue;
		}
		set
		{
			if (_barAxisValue != value)
			{
				_barAxisValue = value;
				graphC.Changed();
				seriesNoCountC.Changed();
			}
		}
	}

	public bool autoUpdateOrigin
	{
		get
		{
			return _autoUpdateOrigin;
		}
		set
		{
			if (_autoUpdateOrigin != value)
			{
				_autoUpdateOrigin = value;
				graphC.Changed();
			}
		}
	}

	public bool autoUpdateBarWidth
	{
		get
		{
			return _autoUpdateBarWidth;
		}
		set
		{
			if (_autoUpdateBarWidth != value)
			{
				_autoUpdateBarWidth = value;
				graphC.Changed();
				seriesNoCountC.Changed();
			}
		}
	}

	public float autoUpdateBarWidthSpacing
	{
		get
		{
			return _autoUpdateBarWidthSpacing;
		}
		set
		{
			if (_autoUpdateBarWidthSpacing != value)
			{
				_autoUpdateBarWidthSpacing = value;
				graphC.Changed();
				seriesNoCountC.Changed();
			}
		}
	}

	public bool autoUpdateSeriesAxisSpacing
	{
		get
		{
			return _autoUpdateSeriesAxisSpacing;
		}
		set
		{
			if (_autoUpdateSeriesAxisSpacing != value)
			{
				_autoUpdateSeriesAxisSpacing = value;
				graphC.Changed();
				seriesNoCountC.Changed();
			}
		}
	}

	public bool autoUpdateBarAxisValue
	{
		get
		{
			return _autoUpdateBarAxisValue;
		}
		set
		{
			if (_autoUpdateBarAxisValue != value)
			{
				_autoUpdateBarAxisValue = value;
				graphC.Changed();
				seriesNoCountC.Changed();
			}
		}
	}

	public int axisWidth
	{
		get
		{
			return _axisWidth;
		}
		set
		{
			if (_axisWidth != value)
			{
				_axisWidth = value;
				graphC.Changed();
			}
		}
	}

	public float autoShrinkAtPercent
	{
		get
		{
			return _autoShrinkAtPercent;
		}
		set
		{
			if (_autoShrinkAtPercent != value)
			{
				_autoShrinkAtPercent = value;
				graphC.Changed();
			}
		}
	}

	public float autoGrowAndShrinkByPercent
	{
		get
		{
			return _autoGrowAndShrinkByPercent;
		}
		set
		{
			if (_autoGrowAndShrinkByPercent != value)
			{
				_autoGrowAndShrinkByPercent = value;
				graphC.Changed();
			}
		}
	}

	public bool tooltipEnabled
	{
		get
		{
			return _tooltipEnabled;
		}
		set
		{
			if (_tooltipEnabled != value)
			{
				_tooltipEnabled = value;
				tooltipEnabledC.Changed();
			}
		}
	}

	public bool autoAnimationsEnabled
	{
		get
		{
			return _autoAnimationsEnabled;
		}
		set
		{
			if (_autoAnimationsEnabled != value)
			{
				_autoAnimationsEnabled = value;
				autoAnimEnabledC.Changed();
			}
		}
	}

	public bool autoFitLabels
	{
		get
		{
			return _autoFitLabels;
		}
		set
		{
			if (_autoFitLabels != value)
			{
				_autoFitLabels = value;
				graphC.Changed();
			}
		}
	}

	public float autoFitPadding
	{
		get
		{
			return _autoFitPadding;
		}
		set
		{
			if (_autoFitPadding != value)
			{
				_autoFitPadding = value;
				graphC.Changed();
			}
		}
	}

	public Vector2 tickSize
	{
		get
		{
			return _tickSize;
		}
		set
		{
			if (_tickSize != value)
			{
				_tickSize = value;
				graphC.Changed();
			}
		}
	}

	public string graphTitleString
	{
		get
		{
			return _graphTitleString;
		}
		set
		{
			if (_graphTitleString != value)
			{
				_graphTitleString = value;
				graphC.Changed();
			}
		}
	}

	public Vector2 graphTitleOffset
	{
		get
		{
			return _graphTitleOffset;
		}
		set
		{
			if (_graphTitleOffset != value)
			{
				_graphTitleOffset = value;
				graphC.Changed();
			}
		}
	}

	public float xAxisLength
	{
		get
		{
			float spriteWidth = getSpriteWidth(base.gameObject);
			Vector2 paddingLeftRight = this.paddingLeftRight;
			float num = spriteWidth - paddingLeftRight.x;
			Vector2 paddingLeftRight2 = this.paddingLeftRight;
			return num - paddingLeftRight2.y;
		}
	}

	public float yAxisLength
	{
		get
		{
			float spriteHeight = getSpriteHeight(base.gameObject);
			Vector2 paddingTopBottom = this.paddingTopBottom;
			float num = spriteHeight - paddingTopBottom.x;
			Vector2 paddingTopBottom2 = this.paddingTopBottom;
			return num - paddingTopBottom2.y;
		}
	}

	public bool IsStacked => graphType == graphTypes.bar_stacked || graphType == graphTypes.bar_stacked_percent || graphType == graphTypes.line_stacked;

	public bool _autoFitting
	{
		get;
		set;
	}

	public List<float> TotalPointValues => totalPointValues;

	public event GraphBackgroundChangedHandler GraphBackgroundChanged;

	public int NumComboBarSeries()
	{
		return numComboBarSeries;
	}

	protected virtual void OnGraphBackgroundChanged()
	{
		this.GraphBackgroundChanged?.Invoke(this);
	}

	[ContextMenu("start")]
	public void Start()
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
			changeObjs.Add(orientationC);
			changeObjs.Add(graphTypeC);
			changeObjs.Add(graphC);
			changeObjs.Add(resizeC);
			changeObjs.Add(seriesCountC);
			changeObjs.Add(seriesNoCountC);
			changeObjs.Add(tooltipEnabledC);
			changeObjs.Add(autoAnimEnabledC);
			legend.Init();
			xAxis.Init(yAxis, isY: false);
			yAxis.Init(xAxis, isY: true);
			for (int i = 0; i < lineSeries.Count; i++)
			{
				WMG_Series component = lineSeries[i].GetComponent<WMG_Series>();
				component.Init(i);
			}
			theTooltip = base.gameObject.AddComponent<WMG_Graph_Tooltip>();
			theTooltip.hideFlags = HideFlags.HideInInspector;
			theTooltip.theGraph = this;
			if (tooltipEnabled)
			{
				theTooltip.subscribeToEvents(val: true);
			}
			autoAnim = base.gameObject.AddComponent<WMG_Graph_Auto_Anim>();
			autoAnim.hideFlags = HideFlags.HideInInspector;
			autoAnim.theGraph = this;
			if (autoAnimationsEnabled)
			{
				autoAnim.subscribeToEvents(val: true);
			}
			groups.SetList(_groups);
			groups.Changed += groupsChanged;
			graphTypeC.OnChange += GraphTypeChanged;
			tooltipEnabledC.OnChange += TooltipEnabledChanged;
			autoAnimEnabledC.OnChange += AutoAnimationsEnabledChanged;
			orientationC.OnChange += OrientationChanged;
			resizeC.OnChange += ResizeChanged;
			graphC.OnChange += GraphChanged;
			seriesCountC.OnChange += SeriesCountChanged;
			seriesNoCountC.OnChange += SeriesNoCountChanged;
			setOriginalPropertyValues();
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

	public void ManualResize()
	{
		PauseCallbacks();
		resizeEnabled = true;
		UpdateFromContainer();
		resizeEnabled = false;
		ResumeCallbacks();
	}

	private void PauseCallbacks()
	{
		yAxis.PauseCallbacks();
		xAxis.PauseCallbacks();
		for (int i = 0; i < changeObjs.Count; i++)
		{
			changeObjs[i].changesPaused = true;
			changeObjs[i].changePaused = false;
		}
		for (int j = 0; j < lineSeries.Count; j++)
		{
			WMG_Series component = lineSeries[j].GetComponent<WMG_Series>();
			component.PauseCallbacks();
		}
		legend.PauseCallbacks();
	}

	private void ResumeCallbacks()
	{
		yAxis.ResumeCallbacks();
		xAxis.ResumeCallbacks();
		for (int i = 0; i < changeObjs.Count; i++)
		{
			changeObjs[i].changesPaused = false;
			if (changeObjs[i].changePaused)
			{
				changeObjs[i].Changed();
			}
		}
		for (int j = 0; j < lineSeries.Count; j++)
		{
			WMG_Series component = lineSeries[j].GetComponent<WMG_Series>();
			component.ResumeCallbacks();
		}
		legend.ResumeCallbacks();
	}

	private void updateFromResize()
	{
		bool flag = false;
		updateCacheAndFlag(ref cachedContainerWidth, getSpriteWidth(base.gameObject), ref flag);
		updateCacheAndFlag(ref cachedContainerHeight, getSpriteHeight(base.gameObject), ref flag);
		if (flag)
		{
			resizeC.Changed();
			graphC.Changed();
			seriesNoCountC.Changed();
			legend.legendC.Changed();
		}
	}

	private void updateFromDataSource()
	{
		for (int i = 0; i < lineSeries.Count; i++)
		{
			WMG_Series component = lineSeries[i].GetComponent<WMG_Series>();
			component.UpdateFromDataSource();
			component.RealTimeUpdate();
		}
	}

	private void OrientationChanged()
	{
		UpdateOrientation();
	}

	private void TooltipEnabledChanged()
	{
		UpdateTooltip();
	}

	private void AutoAnimationsEnabledChanged()
	{
		UpdateAutoAnimEvents();
	}

	private void ResizeChanged()
	{
		UpdateFromContainer();
	}

	private void AllChanged()
	{
		graphC.Changed();
		seriesCountC.Changed();
		legend.legendC.Changed();
	}

	private void GraphTypeChanged()
	{
		for (int i = 0; i < lineSeries.Count; i++)
		{
			WMG_Series component = lineSeries[i].GetComponent<WMG_Series>();
			component.prefabC.Changed();
		}
	}

	public void SeriesChanged(bool countChanged, bool instant)
	{
		for (int i = 0; i < lineSeries.Count; i++)
		{
			WMG_Series component = lineSeries[i].GetComponent<WMG_Series>();
			if (countChanged)
			{
				if (instant)
				{
					component.pointValuesCountChanged();
				}
				else
				{
					component.pointValuesCountC.Changed();
				}
			}
			else if (instant)
			{
				component.pointValuesChanged();
			}
			else
			{
				component.pointValuesC.Changed();
			}
		}
	}

	private void SeriesCountChanged()
	{
		SeriesChanged(countChanged: true, instant: false);
	}

	private void SeriesNoCountChanged()
	{
		SeriesChanged(countChanged: false, instant: false);
	}

	public void aSeriesPointsChanged()
	{
		if (Application.isPlaying)
		{
			UpdateTotals();
			UpdateBarWidth();
			UpdateAxesMinMaxValues();
		}
	}

	public void GraphChanged()
	{
		UpdateTotals();
		UpdateBarWidth();
		UpdateAxesMinMaxValues();
		UpdateAxesType();
		UpdateAxesGridsAndTicks();
		UpdateAxesLabels();
		UpdateSeriesParentPositions();
		UpdateBG();
		UpdateTitles();
	}

	private void groupsChanged(bool editorChange, bool countChanged, bool oneValChanged, int index)
	{
		WMG_Util.listChanged(editorChange, ref groups, ref _groups, oneValChanged, index);
		graphC.Changed();
		if (oneValChanged)
		{
			seriesNoCountC.Changed();
		}
		else
		{
			seriesCountC.Changed();
		}
	}

	public void setOriginalPropertyValues()
	{
		cachedContainerWidth = getSpriteWidth(base.gameObject);
		cachedContainerHeight = getSpriteHeight(base.gameObject);
		origWidth = getSpriteWidth(base.gameObject);
		origHeight = getSpriteHeight(base.gameObject);
		origBarWidth = barWidth;
		origAxisWidth = axisWidth;
		origAutoFitPadding = autoFitPadding;
		origTickSize = tickSize;
		origPaddingLeftRight = paddingLeftRight;
		origPaddingTopBottom = paddingTopBottom;
	}

	private void UpdateOrientation()
	{
		yAxis.ChangeOrientation();
		for (int i = 0; i < lineSeries.Count; i++)
		{
			WMG_Series component = lineSeries[i].GetComponent<WMG_Series>();
			WMG_Series wMG_Series = component;
			Vector2 origDataLabelOffset = component.origDataLabelOffset;
			float y = origDataLabelOffset.y;
			Vector2 origDataLabelOffset2 = component.origDataLabelOffset;
			wMG_Series.origDataLabelOffset = new Vector2(y, origDataLabelOffset2.x);
			WMG_Series wMG_Series2 = component;
			Vector2 dataLabelsOffset = component.dataLabelsOffset;
			float y2 = dataLabelsOffset.y;
			Vector2 dataLabelsOffset2 = component.dataLabelsOffset;
			wMG_Series2.dataLabelsOffset = new Vector2(y2, dataLabelsOffset2.x);
			component.setAnimatingFromPreviousData();
		}
	}

	private void UpdateAxesType()
	{
		if (axesType == axesTypes.MANUAL)
		{
			return;
		}
		if (axesType == axesTypes.AUTO_ORIGIN || axesType == axesTypes.AUTO_ORIGIN_X || axesType == axesTypes.AUTO_ORIGIN_Y)
		{
			updateAxesRelativeToOrigin();
			return;
		}
		updateOriginRelativeToAxes();
		if (axesType == axesTypes.I || axesType == axesTypes.II || axesType == axesTypes.III || axesType == axesTypes.IV)
		{
			if (axesType == axesTypes.I)
			{
				setAxesQuadrant1();
			}
			else if (axesType == axesTypes.II)
			{
				setAxesQuadrant2();
			}
			else if (axesType == axesTypes.III)
			{
				setAxesQuadrant3();
			}
			else if (axesType == axesTypes.IV)
			{
				setAxesQuadrant4();
			}
			return;
		}
		if (axesType == axesTypes.CENTER)
		{
			setAxesQuadrant1_2_3_4();
		}
		else if (axesType == axesTypes.I_II)
		{
			setAxesQuadrant1_2();
		}
		else if (axesType == axesTypes.III_IV)
		{
			setAxesQuadrant3_4();
		}
		else if (axesType == axesTypes.II_III)
		{
			setAxesQuadrant2_3();
		}
		else if (axesType == axesTypes.I_IV)
		{
			setAxesQuadrant1_4();
		}
		yAxis.possiblyHideTickBasedOnPercent();
		xAxis.possiblyHideTickBasedOnPercent();
	}

	private void updateOriginRelativeToAxes()
	{
		if (autoUpdateOrigin)
		{
			if (axesType == axesTypes.I)
			{
				_theOrigin = new Vector2(xAxis.AxisMinValue, yAxis.AxisMinValue);
			}
			else if (axesType == axesTypes.II)
			{
				_theOrigin = new Vector2(xAxis.AxisMaxValue, yAxis.AxisMinValue);
			}
			else if (axesType == axesTypes.III)
			{
				_theOrigin = new Vector2(xAxis.AxisMaxValue, yAxis.AxisMaxValue);
			}
			else if (axesType == axesTypes.IV)
			{
				_theOrigin = new Vector2(xAxis.AxisMinValue, yAxis.AxisMaxValue);
			}
			else if (axesType == axesTypes.CENTER)
			{
				_theOrigin = new Vector2((xAxis.AxisMaxValue + xAxis.AxisMinValue) / 2f, (yAxis.AxisMaxValue + yAxis.AxisMinValue) / 2f);
			}
			else if (axesType == axesTypes.I_II)
			{
				_theOrigin = new Vector2((xAxis.AxisMaxValue + xAxis.AxisMinValue) / 2f, yAxis.AxisMinValue);
			}
			else if (axesType == axesTypes.III_IV)
			{
				_theOrigin = new Vector2((xAxis.AxisMaxValue + xAxis.AxisMinValue) / 2f, yAxis.AxisMaxValue);
			}
			else if (axesType == axesTypes.II_III)
			{
				_theOrigin = new Vector2(xAxis.AxisMaxValue, (yAxis.AxisMaxValue + yAxis.AxisMinValue) / 2f);
			}
			else if (axesType == axesTypes.I_IV)
			{
				_theOrigin = new Vector2(xAxis.AxisMinValue, (yAxis.AxisMaxValue + yAxis.AxisMinValue) / 2f);
			}
		}
		if (autoUpdateBarAxisValue)
		{
			if (orientationType == orientationTypes.vertical)
			{
				Vector2 theOrigin = this.theOrigin;
				_barAxisValue = theOrigin.y;
			}
			else
			{
				Vector2 theOrigin2 = this.theOrigin;
				_barAxisValue = theOrigin2.x;
			}
		}
	}

	private void updateAxesRelativeToOrigin()
	{
		WMG_Axis wMG_Axis = yAxis;
		Vector2 theOrigin = this.theOrigin;
		wMG_Axis.updateAxesRelativeToOrigin(theOrigin.x);
		WMG_Axis wMG_Axis2 = xAxis;
		Vector2 theOrigin2 = this.theOrigin;
		wMG_Axis2.updateAxesRelativeToOrigin(theOrigin2.y);
		if (autoUpdateBarAxisValue)
		{
			if (orientationType == orientationTypes.vertical)
			{
				Vector2 theOrigin3 = this.theOrigin;
				_barAxisValue = theOrigin3.y;
			}
			else
			{
				Vector2 theOrigin4 = this.theOrigin;
				_barAxisValue = theOrigin4.x;
			}
		}
	}

	private void UpdateAxesMinMaxValues()
	{
		yAxis.UpdateAxesMinMaxValues();
		xAxis.UpdateAxesMinMaxValues();
	}

	private void UpdateAxesGridsAndTicks()
	{
		yAxis.UpdateAxesGridsAndTicks();
		xAxis.UpdateAxesGridsAndTicks();
	}

	private void UpdateAxesLabels()
	{
		yAxis.UpdateAxesLabels();
		xAxis.UpdateAxesLabels();
		yAxis.AutofitAxesLabels();
		xAxis.AutofitAxesLabels();
	}

	private void UpdateSeriesParentPositions()
	{
		int num = -1;
		bool flag = false;
		if (graphType == graphTypes.combo)
		{
			for (int i = 0; i < lineSeries.Count; i++)
			{
				WMG_Series component = lineSeries[i].GetComponent<WMG_Series>();
				if (component.comboType == WMG_Series.comboTypes.bar)
				{
					flag = true;
					break;
				}
			}
		}
		for (int j = 0; j < lineSeries.Count; j++)
		{
			WMG_Series component2 = lineSeries[j].GetComponent<WMG_Series>();
			Vector2 axesOffsetFactor = getAxesOffsetFactor();
			axesOffsetFactor = new Vector2((float)(-axisWidth / 2) * axesOffsetFactor.x, (float)(-axisWidth / 2) * axesOffsetFactor.y);
			if (component2.seriesIsLine)
			{
				changeSpritePositionTo(lineSeries[j], new Vector3(0f, 0f, 0f));
			}
			else if (orientationType == orientationTypes.vertical)
			{
				changeSpritePositionTo(lineSeries[j], new Vector3(axesOffsetFactor.x, axesOffsetFactor.y, 0f));
			}
			else
			{
				changeSpritePositionTo(lineSeries[j], new Vector3(axesOffsetFactor.x, axesOffsetFactor.y + barWidth, 0f));
			}
			if (graphType == graphTypes.bar_side)
			{
				if (j > 0)
				{
					if (orientationType == orientationTypes.vertical)
					{
						changeSpritePositionRelativeToObjByX(lineSeries[j], lineSeries[j - 1], barWidth);
					}
					else
					{
						changeSpritePositionRelativeToObjByY(lineSeries[j], lineSeries[j - 1], barWidth);
					}
				}
			}
			else if (graphType == graphTypes.combo)
			{
				if (j <= 0)
				{
					continue;
				}
				if (lineSeries[j - 1].GetComponent<WMG_Series>().comboType == WMG_Series.comboTypes.bar)
				{
					num = j - 1;
				}
				if (num > -1 && lineSeries[j].GetComponent<WMG_Series>().comboType == WMG_Series.comboTypes.bar)
				{
					if (orientationType == orientationTypes.vertical)
					{
						changeSpritePositionRelativeToObjByX(lineSeries[j], lineSeries[num], barWidth);
					}
					else
					{
						changeSpritePositionRelativeToObjByY(lineSeries[j], lineSeries[num], barWidth);
					}
				}
				if (flag && lineSeries[j].GetComponent<WMG_Series>().comboType == WMG_Series.comboTypes.line)
				{
					changeSpritePositionRelativeToObjByX(lineSeries[j], lineSeries[0], barWidth / 2f);
				}
			}
			else if (j > 0)
			{
				if (orientationType == orientationTypes.vertical)
				{
					changeSpritePositionRelativeToObjByX(lineSeries[j], lineSeries[0], 0f);
				}
				else
				{
					changeSpritePositionRelativeToObjByY(lineSeries[j], lineSeries[0], 0f);
				}
			}
		}
	}

	public void UpdateBG()
	{
		changeSpriteSize(graphBackground, Mathf.RoundToInt(getSpriteWidth(base.gameObject)), Mathf.RoundToInt(getSpriteHeight(base.gameObject)));
		GameObject obj = graphBackground;
		Vector2 paddingLeftRight = this.paddingLeftRight;
		float x = 0f - paddingLeftRight.x;
		Vector2 paddingTopBottom = this.paddingTopBottom;
		changeSpritePositionTo(obj, new Vector3(x, 0f - paddingTopBottom.y, 0f));
		changeSpriteSize(anchoredParent, Mathf.RoundToInt(getSpriteWidth(base.gameObject)), Mathf.RoundToInt(getSpriteHeight(base.gameObject)));
		GameObject obj2 = anchoredParent;
		Vector2 paddingLeftRight2 = this.paddingLeftRight;
		float x2 = 0f - paddingLeftRight2.x;
		Vector2 paddingTopBottom2 = this.paddingTopBottom;
		changeSpritePositionTo(obj2, new Vector3(x2, 0f - paddingTopBottom2.y, 0f));
		UpdateBGandSeriesParentPositions(cachedContainerWidth, cachedContainerHeight);
		OnGraphBackgroundChanged();
	}

	public void UpdateBGandSeriesParentPositions(float x, float y)
	{
		Vector2 spritePivot = getSpritePivot(base.gameObject);
		float num = (0f - x) * spritePivot.x;
		Vector2 paddingLeftRight = this.paddingLeftRight;
		float x2 = num + paddingLeftRight.x;
		float num2 = (0f - y) * spritePivot.y;
		Vector2 paddingTopBottom = this.paddingTopBottom;
		Vector3 newPos = new Vector3(x2, num2 + paddingTopBottom.y);
		changeSpritePositionTo(graphBackground.transform.parent.gameObject, newPos);
		changeSpritePositionTo(seriesParent, newPos);
	}

	private void UpdateTotals()
	{
		int num = 0;
		int num2 = 0;
		numComboBarSeries = 0;
		for (int i = 0; i < lineSeries.Count; i++)
		{
			WMG_Series component = lineSeries[i].GetComponent<WMG_Series>();
			if (num < component.pointValues.Count)
			{
				num = component.pointValues.Count;
			}
			if (graphType == graphTypes.combo && component.comboType == WMG_Series.comboTypes.bar)
			{
				numComboBarSeries++;
				if (num2 < component.pointValues.Count)
				{
					num2 = component.pointValues.Count;
				}
			}
		}
		maxSeriesPointCount = num;
		maxSeriesBarCount = num2;
		for (int j = 0; j < num; j++)
		{
			if (totalPointValues.Count <= j)
			{
				totalPointValues.Add(0f);
			}
			totalPointValues[j] = 0f;
			for (int k = 0; k < lineSeries.Count; k++)
			{
				WMG_Series component2 = lineSeries[k].GetComponent<WMG_Series>();
				if (component2.pointValues.Count > j)
				{
					if (orientationType == orientationTypes.vertical)
					{
						List<float> list;
						List<float> list2 = list = totalPointValues;
						int index;
						int index2 = index = j;
						float num3 = list[index];
						Vector2 vector = component2.pointValues[j];
						list2[index2] = num3 + (vector.y - yAxis.AxisMinValue);
					}
					else
					{
						List<float> list;
						List<float> list3 = list = totalPointValues;
						int index3;
						int index4 = index3 = j;
						float num4 = list[index3];
						Vector2 vector2 = component2.pointValues[j];
						list3[index4] = num4 + (vector2.y - xAxis.AxisMinValue);
					}
				}
			}
		}
	}

	private void UpdateBarWidth()
	{
		if (autoUpdateBarWidth)
		{
			if (graphType == graphTypes.line || graphType == graphTypes.line_stacked)
			{
				return;
			}
			float num = xAxisLength;
			if (orientationType == orientationTypes.horizontal)
			{
				num = yAxisLength;
			}
			int num2 = maxSeriesPointCount * lineSeries.Count + 1;
			if (graphType == graphTypes.combo)
			{
				num2 = maxSeriesBarCount * numComboBarSeries + 1;
			}
			if (graphType == graphTypes.bar_stacked || graphType == graphTypes.bar_stacked_percent)
			{
				num2 = maxSeriesPointCount;
			}
			autoUpdateBarWidthSpacing = Mathf.Clamp01(autoUpdateBarWidthSpacing);
			barWidth = (1f - autoUpdateBarWidthSpacing) * (num - (float)maxSeriesPointCount) / (float)num2;
		}
		for (int i = 0; i < lineSeries.Count; i++)
		{
			WMG_Series component = lineSeries[i].GetComponent<WMG_Series>();
			component.updateXdistBetween();
			component.updateExtraXSpace();
		}
		UpdateSeriesParentPositions();
	}

	private void UpdateTitles()
	{
		if (graphTitle != null)
		{
			changeLabelText(graphTitle, graphTitleString);
			GameObject obj = graphTitle;
			float num = xAxisLength / 2f;
			Vector2 graphTitleOffset = this.graphTitleOffset;
			float x = num + graphTitleOffset.x;
			float yAxisLength = this.yAxisLength;
			Vector2 graphTitleOffset2 = this.graphTitleOffset;
			changeSpritePositionTo(obj, new Vector3(x, yAxisLength + graphTitleOffset2.y));
		}
		yAxis.UpdateTitle();
		xAxis.UpdateTitle();
	}

	private void UpdateTooltip()
	{
		theTooltip.subscribeToEvents(tooltipEnabled);
	}

	private void UpdateAutoAnimEvents()
	{
		autoAnim.subscribeToEvents(autoAnimationsEnabled);
	}

	public float getDistBetween(int pointsCount, float theAxisLength)
	{
		float num = 0f;
		if (pointsCount - 1 <= 0)
		{
			num = xAxisLength;
			if (graphType == graphTypes.bar_side)
			{
				num -= (float)lineSeries.Count * barWidth;
			}
			else if (graphType == graphTypes.combo)
			{
				num -= (float)numComboBarSeries * barWidth;
			}
			else if (graphType == graphTypes.bar_stacked)
			{
				num -= barWidth;
			}
			else if (graphType == graphTypes.bar_stacked_percent)
			{
				num -= barWidth;
			}
		}
		else
		{
			int num2 = pointsCount - 1;
			if (graphType != 0 && graphType != graphTypes.line_stacked)
			{
				num2++;
			}
			num = theAxisLength / (float)num2;
			if (graphType == graphTypes.bar_side)
			{
				num -= (float)lineSeries.Count * barWidth / (float)num2;
			}
			else if (graphType == graphTypes.combo)
			{
				num -= (float)numComboBarSeries * barWidth / (float)num2;
			}
			else if (graphType == graphTypes.bar_stacked)
			{
				num -= barWidth / (float)num2;
			}
			else if (graphType == graphTypes.bar_stacked_percent)
			{
				num -= barWidth / (float)num2;
			}
		}
		return num;
	}

	[Obsolete("Use xAxis.GetAxisTickNodes")]
	public List<WMG_Node> getXAxisTicks()
	{
		return xAxis.GetAxisTickNodes();
	}

	[Obsolete("Use xAxis.GetAxisLabelNodes")]
	public List<WMG_Node> getXAxisLabels()
	{
		return xAxis.GetAxisLabelNodes();
	}

	[Obsolete("Use yAxis.GetAxisTickNodes")]
	public List<WMG_Node> getYAxisTicks()
	{
		return yAxis.GetAxisTickNodes();
	}

	[Obsolete("Use yAxis.GetAxisLabelNodes")]
	public List<WMG_Node> getYAxisLabels()
	{
		return yAxis.GetAxisLabelNodes();
	}

	public void changeAllLinePivots(WMGpivotTypes newPivot)
	{
		for (int i = 0; i < lineSeries.Count; i++)
		{
			WMG_Series component = lineSeries[i].GetComponent<WMG_Series>();
			List<GameObject> lines = component.getLines();
			for (int j = 0; j < lines.Count; j++)
			{
				changeSpritePivot(lines[j], newPivot);
				WMG_Link component2 = lines[j].GetComponent<WMG_Link>();
				component2.Reposition();
			}
		}
	}

	public List<Vector3> getSeriesScaleVectors(bool useLineWidthForX, float x, float y)
	{
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < lineSeries.Count; i++)
		{
			WMG_Series component = lineSeries[i].GetComponent<WMG_Series>();
			if (useLineWidthForX)
			{
				list.Add(new Vector3(component.lineScale, y, 1f));
			}
			else
			{
				list.Add(new Vector3(x, y, 1f));
			}
		}
		return list;
	}

	public float getMaxPointSize()
	{
		if (graphType == graphTypes.line || graphType == graphTypes.line_stacked || (graphType == graphTypes.combo && numComboBarSeries == 0))
		{
			float num = 0f;
			for (int i = 0; i < lineSeries.Count; i++)
			{
				WMG_Series component = lineSeries[i].GetComponent<WMG_Series>();
				if (component.pointWidthHeight > num)
				{
					num = component.pointWidthHeight;
				}
			}
			return num;
		}
		float num2 = barWidth;
		if (graphType == graphTypes.combo)
		{
			for (int j = 0; j < lineSeries.Count; j++)
			{
				WMG_Series component2 = lineSeries[j].GetComponent<WMG_Series>();
				if (component2.comboType == WMG_Series.comboTypes.line && component2.pointWidthHeight > num2)
				{
					num2 = component2.pointWidthHeight;
				}
			}
		}
		return num2;
	}

	public int getMaxNumPoints()
	{
		return maxSeriesPointCount;
	}

	public void setAxesQuadrant1()
	{
		xAxis.setAxisTopRight(rightAbove: false);
		yAxis.setAxisTopRight(rightAbove: false);
	}

	public void setAxesQuadrant2()
	{
		xAxis.setAxisBotLeft(rightAbove: false);
		yAxis.setAxisTopRight(rightAbove: true);
	}

	public void setAxesQuadrant3()
	{
		xAxis.setAxisBotLeft(rightAbove: true);
		yAxis.setAxisBotLeft(rightAbove: true);
	}

	public void setAxesQuadrant4()
	{
		xAxis.setAxisTopRight(rightAbove: true);
		yAxis.setAxisBotLeft(rightAbove: false);
	}

	public void setAxesQuadrant1_2_3_4()
	{
		xAxis.setAxisMiddle(rightAbove: false);
		yAxis.setAxisMiddle(rightAbove: false);
	}

	public void setAxesQuadrant1_2()
	{
		xAxis.setAxisMiddle(rightAbove: false);
		yAxis.setAxisTopRight(rightAbove: false);
	}

	public void setAxesQuadrant3_4()
	{
		xAxis.setAxisMiddle(rightAbove: true);
		yAxis.setAxisBotLeft(rightAbove: false);
	}

	public void setAxesQuadrant2_3()
	{
		xAxis.setAxisBotLeft(rightAbove: false);
		yAxis.setAxisMiddle(rightAbove: true);
	}

	public void setAxesQuadrant1_4()
	{
		xAxis.setAxisTopRight(rightAbove: false);
		yAxis.setAxisMiddle(rightAbove: false);
	}

	private Vector2 getAxesOffsetFactor()
	{
		if (axesType == axesTypes.I)
		{
			return new Vector2(-1f, -1f);
		}
		if (axesType == axesTypes.II)
		{
			return new Vector2(1f, -1f);
		}
		if (axesType == axesTypes.III)
		{
			return new Vector2(1f, 1f);
		}
		if (axesType == axesTypes.IV)
		{
			return new Vector2(-1f, 1f);
		}
		if (axesType == axesTypes.CENTER)
		{
			return new Vector2(0f, 0f);
		}
		if (axesType == axesTypes.I_II)
		{
			return new Vector2(0f, -1f);
		}
		if (axesType == axesTypes.III_IV)
		{
			return new Vector2(0f, 1f);
		}
		if (axesType == axesTypes.II_III)
		{
			return new Vector2(1f, 0f);
		}
		if (axesType == axesTypes.I_IV)
		{
			return new Vector2(-1f, 0f);
		}
		if (axesType == axesTypes.AUTO_ORIGIN || axesType == axesTypes.AUTO_ORIGIN_X || axesType == axesTypes.AUTO_ORIGIN_Y)
		{
			float x = 0f;
			float y = 0f;
			if (axesType == axesTypes.AUTO_ORIGIN || axesType == axesTypes.AUTO_ORIGIN_Y)
			{
				float axisMinValue = xAxis.AxisMinValue;
				Vector2 theOrigin = this.theOrigin;
				if (axisMinValue >= theOrigin.x)
				{
					y = -1f;
				}
				else
				{
					float axisMaxValue = xAxis.AxisMaxValue;
					Vector2 theOrigin2 = this.theOrigin;
					if (axisMaxValue <= theOrigin2.x)
					{
						y = 1f;
					}
				}
			}
			if (axesType == axesTypes.AUTO_ORIGIN || axesType == axesTypes.AUTO_ORIGIN_X)
			{
				float axisMinValue2 = yAxis.AxisMinValue;
				Vector2 theOrigin3 = this.theOrigin;
				if (axisMinValue2 >= theOrigin3.y)
				{
					x = -1f;
				}
				else
				{
					float axisMaxValue2 = yAxis.AxisMaxValue;
					Vector2 theOrigin4 = this.theOrigin;
					if (axisMaxValue2 <= theOrigin4.y)
					{
						x = 1f;
					}
				}
			}
			return new Vector2(x, y);
		}
		return new Vector2(0f, 0f);
	}

	public void animScaleAllAtOnce(bool isPoint, float duration, float delay, Ease anEaseType, List<Vector3> before, List<Vector3> after)
	{
		for (int i = 0; i < lineSeries.Count; i++)
		{
			WMG_Series component = lineSeries[i].GetComponent<WMG_Series>();
			List<GameObject> list = (!isPoint) ? component.getLines() : component.getPoints();
			for (int j = 0; j < list.Count; j++)
			{
				list[j].transform.localScale = before[i];
				WMG_Anim.animScale(list[j], duration, anEaseType, after[i], delay);
			}
		}
	}

	public void animScaleBySeries(bool isPoint, float duration, float delay, Ease anEaseType, List<Vector3> before, List<Vector3> after)
	{
		Sequence seq = DOTween.Sequence();
		float num = duration / (float)lineSeries.Count;
		float num2 = delay / (float)lineSeries.Count;
		for (int i = 0; i < lineSeries.Count; i++)
		{
			WMG_Series component = lineSeries[i].GetComponent<WMG_Series>();
			List<GameObject> list = (!isPoint) ? component.getLines() : component.getPoints();
			float insTime = (float)i * num + (float)(i + 1) * num2;
			for (int j = 0; j < list.Count; j++)
			{
				list[j].transform.localScale = before[i];
				WMG_Anim.animScaleSeqInsert(ref seq, insTime, list[j], num, anEaseType, after[i], num2);
			}
		}
		seq.Play();
	}

	public void animScaleOneByOne(bool isPoint, float duration, float delay, Ease anEaseType, List<Vector3> before, List<Vector3> after, int loopDir)
	{
		for (int i = 0; i < lineSeries.Count; i++)
		{
			Sequence seq = DOTween.Sequence();
			WMG_Series component = lineSeries[i].GetComponent<WMG_Series>();
			List<GameObject> list = (!isPoint) ? component.getLines() : component.getPoints();
			float duration2 = duration / (float)list.Count;
			float delay2 = delay / (float)list.Count;
			switch (loopDir)
			{
			case 0:
				for (int j = 0; j < list.Count; j++)
				{
					list[j].transform.localScale = before[i];
					WMG_Anim.animScaleSeqAppend(ref seq, list[j], duration2, anEaseType, after[i], delay2);
				}
				break;
			case 1:
				for (int num5 = list.Count - 1; num5 >= 0; num5--)
				{
					list[num5].transform.localScale = before[i];
					WMG_Anim.animScaleSeqAppend(ref seq, list[num5], duration2, anEaseType, after[i], delay2);
				}
				break;
			case 2:
			{
				int num = list.Count - 1;
				int num2 = num / 2;
				int num3 = -1;
				int num4 = 0;
				bool flag = false;
				bool flag2 = false;
				while (!flag || !flag2)
				{
					if (num2 >= 0 && num2 <= num)
					{
						list[num2].transform.localScale = before[i];
						WMG_Anim.animScaleSeqAppend(ref seq, list[num2], duration2, anEaseType, after[i], delay2);
					}
					num4++;
					num3 *= -1;
					num2 += num3 * num4;
					if (num2 < 0)
					{
						flag = true;
					}
					if (num2 > num)
					{
						flag2 = true;
					}
				}
				break;
			}
			}
			seq.Play();
		}
	}

	public WMG_Series addSeries()
	{
		return addSeriesAt(lineSeries.Count);
	}

	public void deleteSeries()
	{
		deleteSeriesAt(lineSeries.Count - 1);
	}

	public WMG_Series addSeriesAt(int index)
	{
		if (Application.isPlaying)
		{
			Init();
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(seriesPrefab) as GameObject;
		gameObject.name = "Series" + (index + 1);
		changeSpriteParent(gameObject, seriesParent);
		gameObject.transform.localScale = Vector3.one;
		WMG_Series component = gameObject.GetComponent<WMG_Series>();
		if (autoAnimationsEnabled)
		{
			autoAnim.addSeriesForAutoAnim(component);
		}
		component.theGraph = this;
		lineSeries.Insert(index, gameObject);
		component.Init(index);
		return gameObject.GetComponent<WMG_Series>();
	}

	public void deleteSeriesAt(int index)
	{
		if (Application.isPlaying)
		{
			Init();
		}
		GameObject gameObject = lineSeries[index];
		WMG_Series component = gameObject.GetComponent<WMG_Series>();
		lineSeries.Remove(gameObject);
		if (Application.isPlaying)
		{
			component.deleteAllNodesFromGraphManager();
			legend.deleteLegendEntry(index);
		}
		UnityEngine.Object.DestroyImmediate(gameObject);
		graphC.Changed();
		if (graphType != 0 && graphType != graphTypes.line_stacked)
		{
			seriesNoCountC.Changed();
		}
		legend.legendC.Changed();
	}

	private void UpdateFromContainer()
	{
		if (!resizeEnabled)
		{
			return;
		}
		bool flag = true;
		Vector2 vector = new Vector2(cachedContainerWidth / origWidth, cachedContainerHeight / origHeight);
		Vector2 vector2 = vector;
		if (orientationType == orientationTypes.horizontal)
		{
			vector2 = new Vector2(vector.y, vector.x);
		}
		float num = vector.x;
		if (vector.y < num)
		{
			num = vector.y;
		}
		if ((resizeProperties & ResizeProperties.BorderPadding) == ResizeProperties.BorderPadding)
		{
			if (autoFitLabels)
			{
				if (xAxis.AxisTicksRightAbove)
				{
					float newResizeVariable = getNewResizeVariable(num, origPaddingLeftRight.x);
					Vector2 paddingLeftRight = this.paddingLeftRight;
					this.paddingLeftRight = new Vector2(newResizeVariable, paddingLeftRight.y);
				}
				else
				{
					Vector2 paddingLeftRight2 = this.paddingLeftRight;
					this.paddingLeftRight = new Vector2(paddingLeftRight2.x, getNewResizeVariable(num, origPaddingLeftRight.y));
				}
				if (yAxis.AxisTicksRightAbove)
				{
					Vector2 paddingTopBottom = this.paddingTopBottom;
					this.paddingTopBottom = new Vector2(paddingTopBottom.x, getNewResizeVariable(num, origPaddingTopBottom.y));
				}
				else
				{
					float newResizeVariable2 = getNewResizeVariable(num, origPaddingTopBottom.x);
					Vector2 paddingTopBottom2 = this.paddingTopBottom;
					this.paddingTopBottom = new Vector2(newResizeVariable2, paddingTopBottom2.y);
				}
			}
			else
			{
				this.paddingLeftRight = new Vector2(getNewResizeVariable(num, origPaddingLeftRight.x), getNewResizeVariable(num, origPaddingLeftRight.y));
				this.paddingTopBottom = new Vector2(getNewResizeVariable(num, origPaddingTopBottom.x), getNewResizeVariable(num, origPaddingTopBottom.y));
			}
		}
		if ((resizeProperties & ResizeProperties.AutofitPadding) == ResizeProperties.AutofitPadding)
		{
			autoFitPadding = getNewResizeVariable(num, origAutoFitPadding);
		}
		if ((resizeProperties & ResizeProperties.TickSize) == ResizeProperties.TickSize)
		{
			tickSize = new Vector2(getNewResizeVariable(num, origTickSize.x), getNewResizeVariable(num, origTickSize.y));
		}
		if ((resizeProperties & ResizeProperties.AxesWidth) == ResizeProperties.AxesWidth)
		{
			axisWidth = Mathf.RoundToInt(getNewResizeVariable(num, origAxisWidth));
		}
		if ((resizeProperties & ResizeProperties.AxesLabelSize) == ResizeProperties.AxesLabelSize)
		{
			if (flag)
			{
				yAxis.setLabelScales(getNewResizeVariable(num, 1f));
				xAxis.setLabelScales(getNewResizeVariable(num, 1f));
			}
			else
			{
				yAxis.AxisLabelSize = Mathf.RoundToInt(getNewResizeVariable(num, yAxis.origAxisLabelSize));
				xAxis.AxisLabelSize = Mathf.RoundToInt(getNewResizeVariable(num, xAxis.origAxisLabelSize));
			}
		}
		if ((resizeProperties & ResizeProperties.AxesLabelOffset) == ResizeProperties.AxesLabelOffset)
		{
			yAxis.AxisLabelSpaceOffset = Mathf.RoundToInt(getNewResizeVariable(num, yAxis.origAxisLabelSpaceOffset));
			xAxis.AxisLabelSpaceOffset = Mathf.RoundToInt(getNewResizeVariable(num, xAxis.origAxisLabelSpaceOffset));
		}
		if ((resizeProperties & ResizeProperties.AxesLabelOffset) == ResizeProperties.AxesLabelOffset)
		{
			yAxis.AxisTitleFontSize = Mathf.RoundToInt(getNewResizeVariable(num, yAxis.origAxisTitleFontSize));
			xAxis.AxisTitleFontSize = Mathf.RoundToInt(getNewResizeVariable(num, xAxis.origAxisTitleFontSize));
		}
		if ((resizeProperties & ResizeProperties.AxesLinePadding) == ResizeProperties.AxesLinePadding)
		{
			yAxis.AxisLinePadding = getNewResizeVariable(num, yAxis.origAxisLinePadding);
			xAxis.AxisLinePadding = getNewResizeVariable(num, xAxis.origAxisLinePadding);
		}
		if ((resizeProperties & ResizeProperties.AxesArrowSize) == ResizeProperties.AxesArrowSize)
		{
			float sizeFactor = num;
			Vector2 origAxisArrowSize = yAxis.origAxisArrowSize;
			float newResizeVariable3 = getNewResizeVariable(sizeFactor, origAxisArrowSize.x);
			float sizeFactor2 = num;
			Vector2 origAxisArrowSize2 = yAxis.origAxisArrowSize;
			Vector2 vector3 = new Vector2(newResizeVariable3, getNewResizeVariable(sizeFactor2, origAxisArrowSize2.y));
			changeSpriteSize(yAxis.AxisArrowDL, Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y));
			changeSpriteSize(yAxis.AxisArrowUR, Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y));
			float sizeFactor3 = num;
			Vector2 origAxisArrowSize3 = xAxis.origAxisArrowSize;
			float newResizeVariable4 = getNewResizeVariable(sizeFactor3, origAxisArrowSize3.x);
			float sizeFactor4 = num;
			Vector2 origAxisArrowSize4 = xAxis.origAxisArrowSize;
			Vector2 vector4 = new Vector2(newResizeVariable4, getNewResizeVariable(sizeFactor4, origAxisArrowSize4.y));
			changeSpriteSize(xAxis.AxisArrowDL, Mathf.RoundToInt(vector4.x), Mathf.RoundToInt(vector4.y));
			changeSpriteSize(xAxis.AxisArrowUR, Mathf.RoundToInt(vector4.x), Mathf.RoundToInt(vector4.y));
		}
		if ((resizeProperties & ResizeProperties.LegendFontSize) == ResizeProperties.LegendFontSize)
		{
			if (flag)
			{
				legend.setLabelScales(getNewResizeVariable(num, 1f));
			}
			else
			{
				legend.legendEntryFontSize = Mathf.RoundToInt(getNewResizeVariable(num, legend.origLegendEntryFontSize));
			}
		}
		if ((resizeProperties & ResizeProperties.LegendEntrySize) == ResizeProperties.LegendEntrySize)
		{
			if (!legend.setWidthFromLabels)
			{
				legend.legendEntryWidth = getNewResizeVariable(num, legend.origLegendEntryWidth);
			}
			legend.legendEntryHeight = getNewResizeVariable(num, legend.origLegendEntryHeight);
		}
		if ((resizeProperties & ResizeProperties.LegendOffset) == ResizeProperties.LegendOffset)
		{
			legend.offset = getNewResizeVariable(num, legend.origOffset);
		}
		if ((resizeProperties & ResizeProperties.SeriesPointSize) == ResizeProperties.SeriesPointSize)
		{
			legend.legendEntryLinkSpacing = getNewResizeVariable(num, legend.origLegendEntryLinkSpacing);
			legend.legendEntrySpacing = getNewResizeVariable(num, legend.origLegendEntrySpacing);
		}
		if ((resizeProperties & ResizeProperties.SeriesPointSize) == ResizeProperties.SeriesPointSize)
		{
			barWidth = getNewResizeVariable(vector2.x, origBarWidth);
		}
		if ((resizeProperties & ResizeProperties.SeriesPointSize) != ResizeProperties.SeriesPointSize && (resizeProperties & ResizeProperties.SeriesLineWidth) != ResizeProperties.SeriesLineWidth && (resizeProperties & ResizeProperties.SeriesDataLabelSize) != ResizeProperties.SeriesDataLabelSize && (resizeProperties & ResizeProperties.SeriesDataLabelOffset) != ResizeProperties.SeriesDataLabelOffset)
		{
			return;
		}
		for (int i = 0; i < lineSeries.Count; i++)
		{
			if (activeInHierarchy(lineSeries[i]))
			{
				WMG_Series component = lineSeries[i].GetComponent<WMG_Series>();
				if ((resizeProperties & ResizeProperties.SeriesPointSize) == ResizeProperties.SeriesPointSize)
				{
					component.pointWidthHeight = getNewResizeVariable(num, component.origPointWidthHeight);
				}
				if ((resizeProperties & ResizeProperties.SeriesLineWidth) == ResizeProperties.SeriesLineWidth)
				{
					component.lineScale = getNewResizeVariable(num, component.origLineScale);
				}
				if ((resizeProperties & ResizeProperties.SeriesDataLabelSize) == ResizeProperties.SeriesDataLabelSize)
				{
					component.dataLabelsFontSize = Mathf.RoundToInt(getNewResizeVariable(num, component.origDataLabelsFontSize));
				}
				if ((resizeProperties & ResizeProperties.SeriesDataLabelOffset) == ResizeProperties.SeriesDataLabelOffset)
				{
					WMG_Series wMG_Series = component;
					float sizeFactor5 = num;
					Vector2 origDataLabelOffset = component.origDataLabelOffset;
					float newResizeVariable5 = getNewResizeVariable(sizeFactor5, origDataLabelOffset.x);
					float sizeFactor6 = num;
					Vector2 origDataLabelOffset2 = component.origDataLabelOffset;
					wMG_Series.dataLabelsOffset = new Vector2(newResizeVariable5, getNewResizeVariable(sizeFactor6, origDataLabelOffset2.y));
				}
			}
		}
	}

	private float getNewResizeVariable(float sizeFactor, float variable)
	{
		return variable + (sizeFactor - 1f) * variable;
	}
}
