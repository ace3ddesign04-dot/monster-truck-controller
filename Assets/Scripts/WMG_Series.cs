using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WMG_Series : MonoBehaviour
{
	public enum comboTypes
	{
		line,
		bar
	}

	public enum areaShadingTypes
	{
		None,
		Solid,
		Gradient
	}

	public delegate string SeriesDataLabeler(WMG_Series series, float val);

	public delegate void SeriesDataChangedHandler(WMG_Series aSeries);

	[SerializeField]
	private List<Vector2> _pointValues;

	public WMG_List<Vector2> pointValues = new WMG_List<Vector2>();

	[SerializeField]
	private List<Color> _pointColors;

	public WMG_List<Color> pointColors = new WMG_List<Color>();

	public UnityEngine.Object dataLabelPrefab;

	public GameObject dataLabelsParent;

	public Material areaShadingMatSolid;

	public Material areaShadingMatGradient;

	public GameObject areaShadingParent;

	public UnityEngine.Object areaShadingPrefab;

	public WMG_Axis_Graph theGraph;

	public WMG_Data_Source realTimeDataSource;

	public WMG_Data_Source pointValuesDataSource;

	public UnityEngine.Object legendEntryPrefab;

	public GameObject linkParent;

	public GameObject nodeParent;

	public WMG_Legend_Entry legendEntry;

	[SerializeField]
	private comboTypes _comboType;

	[SerializeField]
	private string _seriesName;

	[SerializeField]
	private float _pointWidthHeight;

	[SerializeField]
	private float _lineScale;

	[SerializeField]
	private Color _pointColor;

	[SerializeField]
	private bool _usePointColors;

	[SerializeField]
	private Color _lineColor;

	[SerializeField]
	private bool _UseXDistBetweenToSpace;

	[SerializeField]
	private bool _AutoUpdateXDistBetween;

	[SerializeField]
	private float _xDistBetweenPoints;

	[SerializeField]
	private float _extraXSpace;

	[SerializeField]
	private bool _hidePoints;

	[SerializeField]
	private bool _hideLines;

	[SerializeField]
	private bool _connectFirstToLast;

	[SerializeField]
	private float _linePadding;

	[SerializeField]
	private bool _dataLabelsEnabled;

	[SerializeField]
	private int _dataLabelsNumDecimals;

	[SerializeField]
	private int _dataLabelsFontSize;

	[SerializeField]
	private Vector2 _dataLabelsOffset;

	[SerializeField]
	private areaShadingTypes _areaShadingType;

	[SerializeField]
	private Color _areaShadingColor;

	[SerializeField]
	private float _areaShadingAxisValue;

	[SerializeField]
	private int _pointPrefab;

	[SerializeField]
	private int _linkPrefab;

	private UnityEngine.Object nodePrefab;

	private List<GameObject> points = new List<GameObject>();

	private List<GameObject> lines = new List<GameObject>();

	private List<GameObject> areaShadingRects = new List<GameObject>();

	private List<GameObject> dataLabels = new List<GameObject>();

	private List<bool> barIsNegative = new List<bool>();

	private List<int> changedValIndices = new List<int>();

	private WMG_Axis_Graph.graphTypes cachedSeriesType;

	private bool realTimeRunning;

	private float realTimeLoopVar;

	private float realTimeOrigMax;

	private bool animatingFromPreviousData;

	private List<Vector2> afterPositions = new List<Vector2>();

	private List<int> afterWidths = new List<int>();

	private List<int> afterHeights = new List<int>();

	private List<WMG_Change_Obj> changeObjs = new List<WMG_Change_Obj>();

	public WMG_Change_Obj pointValuesC = new WMG_Change_Obj();

	public WMG_Change_Obj pointValuesCountC = new WMG_Change_Obj();

	private WMG_Change_Obj pointValuesValC = new WMG_Change_Obj();

	private WMG_Change_Obj lineScaleC = new WMG_Change_Obj();

	private WMG_Change_Obj pointWidthHeightC = new WMG_Change_Obj();

	private WMG_Change_Obj dataLabelsC = new WMG_Change_Obj();

	private WMG_Change_Obj lineColorC = new WMG_Change_Obj();

	private WMG_Change_Obj pointColorC = new WMG_Change_Obj();

	private WMG_Change_Obj hideLineC = new WMG_Change_Obj();

	private WMG_Change_Obj hidePointC = new WMG_Change_Obj();

	private WMG_Change_Obj seriesNameC = new WMG_Change_Obj();

	private WMG_Change_Obj linePaddingC = new WMG_Change_Obj();

	private WMG_Change_Obj areaShadingTypeC = new WMG_Change_Obj();

	private WMG_Change_Obj areaShadingC = new WMG_Change_Obj();

	public WMG_Change_Obj prefabC = new WMG_Change_Obj();

	private WMG_Change_Obj connectFirstToLastC = new WMG_Change_Obj();

	private bool hasInit;

	public SeriesDataLabeler seriesDataLabeler;

	public comboTypes comboType
	{
		get
		{
			return _comboType;
		}
		set
		{
			if (_comboType != value)
			{
				_comboType = value;
				prefabC.Changed();
			}
		}
	}

	public string seriesName
	{
		get
		{
			return _seriesName;
		}
		set
		{
			if (_seriesName != value)
			{
				_seriesName = value;
				seriesNameC.Changed();
			}
		}
	}

	public float pointWidthHeight
	{
		get
		{
			return _pointWidthHeight;
		}
		set
		{
			if (_pointWidthHeight != value)
			{
				_pointWidthHeight = value;
				pointWidthHeightC.Changed();
			}
		}
	}

	public float lineScale
	{
		get
		{
			return _lineScale;
		}
		set
		{
			if (_lineScale != value)
			{
				_lineScale = value;
				lineScaleC.Changed();
			}
		}
	}

	public Color pointColor
	{
		get
		{
			return _pointColor;
		}
		set
		{
			if (_pointColor != value)
			{
				_pointColor = value;
				pointColorC.Changed();
			}
		}
	}

	public bool usePointColors
	{
		get
		{
			return _usePointColors;
		}
		set
		{
			if (_usePointColors != value)
			{
				_usePointColors = value;
				pointColorC.Changed();
			}
		}
	}

	public Color lineColor
	{
		get
		{
			return _lineColor;
		}
		set
		{
			if (_lineColor != value)
			{
				_lineColor = value;
				lineColorC.Changed();
			}
		}
	}

	public bool UseXDistBetweenToSpace
	{
		get
		{
			return _UseXDistBetweenToSpace;
		}
		set
		{
			if (_UseXDistBetweenToSpace != value)
			{
				_UseXDistBetweenToSpace = value;
				pointValuesC.Changed();
			}
		}
	}

	public bool AutoUpdateXDistBetween
	{
		get
		{
			return _AutoUpdateXDistBetween;
		}
		set
		{
			if (_AutoUpdateXDistBetween != value)
			{
				_AutoUpdateXDistBetween = value;
				pointValuesC.Changed();
			}
		}
	}

	public float xDistBetweenPoints
	{
		get
		{
			return _xDistBetweenPoints;
		}
		set
		{
			if (_xDistBetweenPoints != value)
			{
				_xDistBetweenPoints = value;
				pointValuesC.Changed();
			}
		}
	}

	public float extraXSpace
	{
		get
		{
			return _extraXSpace;
		}
		set
		{
			if (_extraXSpace != value)
			{
				_extraXSpace = value;
				pointValuesC.Changed();
			}
		}
	}

	public bool hidePoints
	{
		get
		{
			return _hidePoints;
		}
		set
		{
			if (_hidePoints != value)
			{
				_hidePoints = value;
				hidePointC.Changed();
			}
		}
	}

	public bool hideLines
	{
		get
		{
			return _hideLines;
		}
		set
		{
			if (_hideLines != value)
			{
				_hideLines = value;
				hideLineC.Changed();
			}
		}
	}

	public bool connectFirstToLast
	{
		get
		{
			return _connectFirstToLast;
		}
		set
		{
			if (_connectFirstToLast != value)
			{
				_connectFirstToLast = value;
				connectFirstToLastC.Changed();
				lineScaleC.Changed();
				linePaddingC.Changed();
				hideLineC.Changed();
				lineColorC.Changed();
			}
		}
	}

	public float linePadding
	{
		get
		{
			return _linePadding;
		}
		set
		{
			if (_linePadding != value)
			{
				_linePadding = value;
				linePaddingC.Changed();
			}
		}
	}

	public bool dataLabelsEnabled
	{
		get
		{
			return _dataLabelsEnabled;
		}
		set
		{
			if (_dataLabelsEnabled != value)
			{
				_dataLabelsEnabled = value;
				dataLabelsC.Changed();
			}
		}
	}

	public int dataLabelsNumDecimals
	{
		get
		{
			return _dataLabelsNumDecimals;
		}
		set
		{
			if (_dataLabelsNumDecimals != value)
			{
				_dataLabelsNumDecimals = value;
				dataLabelsC.Changed();
			}
		}
	}

	public int dataLabelsFontSize
	{
		get
		{
			return _dataLabelsFontSize;
		}
		set
		{
			if (_dataLabelsFontSize != value)
			{
				_dataLabelsFontSize = value;
				dataLabelsC.Changed();
			}
		}
	}

	public Vector2 dataLabelsOffset
	{
		get
		{
			return _dataLabelsOffset;
		}
		set
		{
			if (_dataLabelsOffset != value)
			{
				_dataLabelsOffset = value;
				dataLabelsC.Changed();
			}
		}
	}

	public areaShadingTypes areaShadingType
	{
		get
		{
			return _areaShadingType;
		}
		set
		{
			if (_areaShadingType != value)
			{
				_areaShadingType = value;
				areaShadingTypeC.Changed();
			}
		}
	}

	public Color areaShadingColor
	{
		get
		{
			return _areaShadingColor;
		}
		set
		{
			if (_areaShadingColor != value)
			{
				_areaShadingColor = value;
				areaShadingC.Changed();
			}
		}
	}

	public float areaShadingAxisValue
	{
		get
		{
			return _areaShadingAxisValue;
		}
		set
		{
			if (_areaShadingAxisValue != value)
			{
				_areaShadingAxisValue = value;
				areaShadingC.Changed();
			}
		}
	}

	public int pointPrefab
	{
		get
		{
			return _pointPrefab;
		}
		set
		{
			if (_pointPrefab != value)
			{
				_pointPrefab = value;
				prefabC.Changed();
			}
		}
	}

	public int linkPrefab
	{
		get
		{
			return _linkPrefab;
		}
		set
		{
			if (_linkPrefab != value)
			{
				_linkPrefab = value;
				prefabC.Changed();
			}
		}
	}

	public bool seriesIsLine => theGraph.graphType == WMG_Axis_Graph.graphTypes.line || theGraph.graphType == WMG_Axis_Graph.graphTypes.line_stacked || (theGraph.graphType == WMG_Axis_Graph.graphTypes.combo && comboType == comboTypes.line);

	public bool IsLast => theGraph.lineSeries[theGraph.lineSeries.Count - 1].GetComponent<WMG_Series>() == this;

	public float origPointWidthHeight
	{
		get;
		private set;
	}

	public float origLineScale
	{
		get;
		private set;
	}

	public int origDataLabelsFontSize
	{
		get;
		private set;
	}

	public Vector2 origDataLabelOffset
	{
		get;
		set;
	}

	public bool currentlyAnimating
	{
		get;
		set;
	}

	public event SeriesDataChangedHandler SeriesDataChanged;

	public string formatSeriesDataLabel(WMG_Series series, float val)
	{
		float num = Mathf.Pow(10f, series.dataLabelsNumDecimals);
		return (Mathf.Round(val * num) / num).ToString();
	}

	protected virtual void OnSeriesDataChanged()
	{
		this.SeriesDataChanged?.Invoke(this);
	}

	[ContextMenu("Init")]
	public void Init(int index)
	{
		if (!hasInit)
		{
			hasInit = true;
			changeObjs.Add(pointValuesCountC);
			changeObjs.Add(pointValuesC);
			changeObjs.Add(pointValuesValC);
			changeObjs.Add(connectFirstToLastC);
			changeObjs.Add(lineScaleC);
			changeObjs.Add(pointWidthHeightC);
			changeObjs.Add(dataLabelsC);
			changeObjs.Add(lineColorC);
			changeObjs.Add(pointColorC);
			changeObjs.Add(hideLineC);
			changeObjs.Add(hidePointC);
			changeObjs.Add(seriesNameC);
			changeObjs.Add(linePaddingC);
			changeObjs.Add(areaShadingTypeC);
			changeObjs.Add(areaShadingC);
			changeObjs.Add(prefabC);
			if (seriesIsLine)
			{
				nodePrefab = theGraph.pointPrefabs[pointPrefab];
			}
			else
			{
				nodePrefab = theGraph.barPrefab;
			}
			legendEntry = theGraph.legend.createLegendEntry(legendEntryPrefab, this, index);
			createLegendSwatch();
			theGraph.legend.updateLegend();
			pointValues.SetList(_pointValues);
			pointValues.Changed += pointValuesListChanged;
			pointColors.SetList(_pointColors);
			pointColors.Changed += pointColorsListChanged;
			pointValuesCountC.OnChange += PointValuesCountChanged;
			pointValuesC.OnChange += PointValuesChanged;
			pointValuesValC.OnChange += PointValuesValChanged;
			lineScaleC.OnChange += LineScaleChanged;
			pointWidthHeightC.OnChange += PointWidthHeightChanged;
			dataLabelsC.OnChange += DataLabelsChanged;
			lineColorC.OnChange += LineColorChanged;
			pointColorC.OnChange += PointColorChanged;
			hideLineC.OnChange += HideLinesChanged;
			hidePointC.OnChange += HidePointsChanged;
			seriesNameC.OnChange += SeriesNameChanged;
			linePaddingC.OnChange += LinePaddingChanged;
			areaShadingTypeC.OnChange += AreaShadingTypeChanged;
			areaShadingC.OnChange += AreaShadingChanged;
			prefabC.OnChange += PrefabChanged;
			connectFirstToLastC.OnChange += ConnectFirstToLastChanged;
			seriesDataLabeler = formatSeriesDataLabel;
			setOriginalPropertyValues();
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

	public void pointColorsListChanged(bool editorChange, bool countChanged, bool oneValChanged, int index)
	{
		WMG_Util.listChanged(editorChange, ref pointColors, ref _pointColors, oneValChanged, index);
		pointColorC.Changed();
	}

	public void pointValuesListAboutToChange()
	{
	}

	public void pointValuesListChanged(bool editorChange, bool countChanged, bool oneValChanged, int index)
	{
		WMG_Util.listChanged(editorChange, ref pointValues, ref _pointValues, oneValChanged, index);
		if (countChanged)
		{
			pointValuesCountC.Changed();
			return;
		}
		setAnimatingFromPreviousData();
		if (oneValChanged)
		{
			changedValIndices.Add(index);
			pointValuesValC.Changed();
		}
		else
		{
			pointValuesC.Changed();
		}
	}

	public void PrefabChanged()
	{
		UpdatePrefabType();
		pointValuesCountC.Changed();
	}

	[ContextMenu("Values changed")]
	public void pointValuesChanged()
	{
		theGraph.aSeriesPointsChanged();
		UpdateNullVisibility();
		UpdateSprites();
	}

	public void pointValuesCountChanged()
	{
		theGraph.aSeriesPointsChanged();
		CreateOrDeleteSpritesBasedOnPointValues();
		UpdateLineColor();
		UpdatePointColor();
		UpdateLineScale();
		UpdatePointWidthHeight();
		UpdateHideLines();
		UpdateHidePoints();
		UpdateNullVisibility();
		UpdateLinePadding();
		UpdateSprites();
	}

	public void pointValuesValChanged(int index)
	{
		theGraph.aSeriesPointsChanged();
		UpdateNullVisibility();
		UpdateSprites();
	}

	public void PointValuesChanged()
	{
		if (theGraph.graphType == WMG_Axis_Graph.graphTypes.bar_stacked_percent || (theGraph.IsStacked && !IsLast))
		{
			theGraph.aSeriesPointsChanged();
			theGraph.SeriesChanged(countChanged: false, instant: true);
		}
		else
		{
			pointValuesChanged();
		}
	}

	public void PointValuesCountChanged()
	{
		if (theGraph.graphType == WMG_Axis_Graph.graphTypes.bar_stacked_percent || (theGraph.IsStacked && !IsLast))
		{
			theGraph.aSeriesPointsChanged();
			theGraph.SeriesChanged(countChanged: true, instant: true);
		}
		else
		{
			pointValuesCountChanged();
		}
	}

	public void PointValuesValChanged()
	{
		if (changedValIndices.Count != 1)
		{
			PointValuesChanged();
			return;
		}
		if (theGraph.graphType == WMG_Axis_Graph.graphTypes.bar_stacked_percent || (theGraph.IsStacked && !IsLast))
		{
			theGraph.aSeriesPointsChanged();
			theGraph.SeriesChanged(countChanged: false, instant: true);
		}
		else
		{
			pointValuesValChanged(changedValIndices[0]);
		}
		changedValIndices.Clear();
	}

	public void LineColorChanged()
	{
		UpdateLineColor();
	}

	public void ConnectFirstToLastChanged()
	{
		createOrDeletePoints(pointValues.Count);
	}

	public void PointColorChanged()
	{
		UpdatePointColor();
	}

	public void LineScaleChanged()
	{
		UpdateLineScale();
	}

	public void PointWidthHeightChanged()
	{
		UpdatePointWidthHeight();
	}

	public void HideLinesChanged()
	{
		UpdateHideLines();
		UpdateNullVisibility();
	}

	public void HidePointsChanged()
	{
		UpdateHidePoints();
		UpdateNullVisibility();
	}

	public void SeriesNameChanged()
	{
		UpdateSeriesName();
	}

	public void LinePaddingChanged()
	{
		UpdateLinePadding();
	}

	public void AreaShadingTypeChanged()
	{
		createOrDeleteAreaShading(pointValues.Count);
	}

	public void AreaShadingChanged()
	{
		updateAreaShading();
	}

	public void DataLabelsChanged()
	{
		createOrDeleteLabels(pointValues.Count);
		updateDataLabels();
	}

	public void UpdateFromDataSource()
	{
		if (pointValuesDataSource != null)
		{
			List<Vector2> list = pointValuesDataSource.getData<Vector2>();
			if (theGraph.useGroups)
			{
				list = sanitizeGroupData(list);
			}
			pointValues.SetList(list);
		}
	}

	public void RealTimeUpdate()
	{
		if (realTimeRunning)
		{
			DoRealTimeUpdate();
		}
	}

	public List<Vector2> AfterPositions()
	{
		return afterPositions;
	}

	public List<int> AfterHeights()
	{
		return afterHeights;
	}

	public List<int> AfterWidths()
	{
		return afterWidths;
	}

	public bool AnimatingFromPreviousData()
	{
		return animatingFromPreviousData;
	}

	public void setAnimatingFromPreviousData()
	{
		if (!realTimeRunning && !theGraph.IsStacked && theGraph.autoAnimationsEnabled)
		{
			animatingFromPreviousData = true;
		}
	}

	public void setOriginalPropertyValues()
	{
		origPointWidthHeight = pointWidthHeight;
		origLineScale = lineScale;
		origDataLabelsFontSize = dataLabelsFontSize;
		origDataLabelOffset = dataLabelsOffset;
	}

	public List<GameObject> getPoints()
	{
		return points;
	}

	public GameObject getLastPoint()
	{
		return points[points.Count - 1];
	}

	public GameObject getFirstPoint()
	{
		return points[0];
	}

	public List<GameObject> getLines()
	{
		return lines;
	}

	public List<GameObject> getDataLabels()
	{
		return dataLabels;
	}

	public bool getBarIsNegative(int i)
	{
		return barIsNegative[i];
	}

	public Vector2 getNodeValue(WMG_Node aNode)
	{
		for (int i = 0; i < pointValues.Count; i++)
		{
			if (points[i].GetComponent<WMG_Node>() == aNode)
			{
				return pointValues[i];
			}
		}
		return Vector2.zero;
	}

	public void UpdateHidePoints()
	{
		for (int i = 0; i < points.Count; i++)
		{
			theGraph.SetActive(points[i], !hidePoints);
		}
		theGraph.SetActive(legendEntry.swatchNode, !hidePoints);
		StartCoroutine(SetDelayedAreaShadingChanged());
	}

	public void UpdateNullVisibility()
	{
		if (theGraph.useGroups)
		{
			for (int i = 0; i < points.Count; i++)
			{
				WMG_Axis_Graph wMG_Axis_Graph = theGraph;
				GameObject obj = points[i];
				Vector2 vector = pointValues[i];
				wMG_Axis_Graph.SetActive(obj, vector.x > 0f);
			}
			if (seriesIsLine)
			{
				for (int j = 0; j < lines.Count; j++)
				{
					theGraph.SetActive(lines[j], state: true);
				}
				for (int k = 0; k < points.Count; k++)
				{
					Vector2 vector2 = pointValues[k];
					if (vector2.x < 0f)
					{
						WMG_Node component = points[k].GetComponent<WMG_Node>();
						for (int l = 0; l < component.links.Count; l++)
						{
							theGraph.SetActive(component.links[l], state: false);
						}
					}
				}
			}
			StartCoroutine(SetDelayedAreaShadingChanged());
		}
		if (hidePoints)
		{
			for (int m = 0; m < points.Count; m++)
			{
				theGraph.SetActive(points[m], state: false);
			}
		}
		if (hideLines || !seriesIsLine)
		{
			for (int n = 0; n < lines.Count; n++)
			{
				theGraph.SetActive(lines[n], state: false);
			}
		}
	}

	public void UpdateHideLines()
	{
		for (int i = 0; i < lines.Count; i++)
		{
			if (hideLines || !seriesIsLine)
			{
				theGraph.SetActive(lines[i], state: false);
			}
			else
			{
				theGraph.SetActive(lines[i], state: true);
			}
		}
		if (hideLines || !seriesIsLine)
		{
			theGraph.SetActive(legendEntry.line, state: false);
		}
		else
		{
			theGraph.SetActive(legendEntry.line, state: true);
		}
		StartCoroutine(SetDelayedAreaShadingChanged());
	}

	public void UpdateLineColor()
	{
		for (int i = 0; i < lines.Count; i++)
		{
			WMG_Link component = lines[i].GetComponent<WMG_Link>();
			theGraph.changeSpriteColor(component.objectToColor, lineColor);
		}
		WMG_Link component2 = legendEntry.line.GetComponent<WMG_Link>();
		theGraph.changeSpriteColor(component2.objectToColor, lineColor);
	}

	public void UpdatePointColor()
	{
		for (int i = 0; i < points.Count; i++)
		{
			WMG_Node component = points[i].GetComponent<WMG_Node>();
			if (usePointColors)
			{
				if (i < pointColors.Count)
				{
					theGraph.changeSpriteColor(component.objectToColor, pointColors[i]);
				}
			}
			else
			{
				theGraph.changeSpriteColor(component.objectToColor, pointColor);
			}
		}
		WMG_Node component2 = legendEntry.swatchNode.GetComponent<WMG_Node>();
		theGraph.changeSpriteColor(component2.objectToColor, pointColor);
	}

	public void UpdateLineScale()
	{
		for (int i = 0; i < lines.Count; i++)
		{
			WMG_Link component = lines[i].GetComponent<WMG_Link>();
			Transform transform = component.objectToScale.transform;
			float lineScale = this.lineScale;
			Vector3 localScale = component.objectToScale.transform.localScale;
			float y = localScale.y;
			Vector3 localScale2 = component.objectToScale.transform.localScale;
			transform.localScale = new Vector3(lineScale, y, localScale2.z);
		}
		WMG_Link component2 = legendEntry.line.GetComponent<WMG_Link>();
		Transform transform2 = component2.objectToScale.transform;
		float lineScale2 = this.lineScale;
		Vector3 localScale3 = component2.objectToScale.transform.localScale;
		float y2 = localScale3.y;
		Vector3 localScale4 = component2.objectToScale.transform.localScale;
		transform2.localScale = new Vector3(lineScale2, y2, localScale4.z);
	}

	public void UpdatePointWidthHeight()
	{
		if (seriesIsLine)
		{
			for (int i = 0; i < points.Count; i++)
			{
				WMG_Node component = points[i].GetComponent<WMG_Node>();
				theGraph.changeSpriteHeight(component.objectToColor, Mathf.RoundToInt(pointWidthHeight));
				theGraph.changeSpriteWidth(component.objectToColor, Mathf.RoundToInt(pointWidthHeight));
			}
		}
		WMG_Node component2 = legendEntry.swatchNode.GetComponent<WMG_Node>();
		theGraph.changeSpriteHeight(component2.objectToColor, Mathf.RoundToInt(pointWidthHeight));
		theGraph.changeSpriteWidth(component2.objectToColor, Mathf.RoundToInt(pointWidthHeight));
	}

	public void UpdatePrefabType()
	{
		if (seriesIsLine)
		{
			nodePrefab = theGraph.pointPrefabs[pointPrefab];
		}
		else
		{
			nodePrefab = theGraph.barPrefab;
		}
		for (int num = points.Count - 1; num >= 0; num--)
		{
			if (points[num] != null)
			{
				WMG_Node component = points[num].GetComponent<WMG_Node>();
				foreach (GameObject link in component.links)
				{
					lines.Remove(link);
				}
				theGraph.DeleteNode(component);
				points.RemoveAt(num);
			}
		}
		if (legendEntry.swatchNode != null)
		{
			theGraph.DeleteNode(legendEntry.swatchNode.GetComponent<WMG_Node>());
			theGraph.DeleteLink(legendEntry.line.GetComponent<WMG_Link>());
		}
	}

	public void UpdateSeriesName()
	{
		theGraph.legend.LegendChanged();
	}

	public void UpdateLinePadding()
	{
		for (int i = 0; i < points.Count; i++)
		{
			points[i].GetComponent<WMG_Node>().radius = -1f * linePadding;
		}
		RepositionLines();
	}

	public void RepositionLines()
	{
		for (int i = 0; i < lines.Count; i++)
		{
			lines[i].GetComponent<WMG_Link>().Reposition();
		}
	}

	public void CreateOrDeleteSpritesBasedOnPointValues()
	{
		if (theGraph.useGroups)
		{
			pointValues.SetListNoCb(sanitizeGroupData(pointValues.list), ref _pointValues);
		}
		int count = pointValues.Count;
		createOrDeletePoints(count);
		createOrDeleteLabels(count);
		createOrDeleteAreaShading(count);
	}

	private List<Vector2> sanitizeGroupData(List<Vector2> groupData)
	{
		for (int num = groupData.Count - 1; num >= 0; num--)
		{
			Vector2 vector = groupData[num];
			int num2 = Mathf.RoundToInt(vector.x);
			float num3 = num2;
			Vector2 vector2 = groupData[num];
			if (num3 - vector2.x != 0f)
			{
				groupData.RemoveAt(num);
			}
			else if (Mathf.Abs(num2) > theGraph.groups.Count)
			{
				groupData.RemoveAt(num);
			}
			else if (num2 == 0)
			{
				groupData.RemoveAt(num);
			}
		}
		groupData.Sort((Vector2 vec1, Vector2 vec2) => vec1.x.CompareTo(vec2.x));
		List<Vector2> list = new List<Vector2>();
		bool flag = true;
		for (int i = 0; i < groupData.Count; i++)
		{
			if (flag)
			{
				list.Add(groupData[i]);
				flag = false;
			}
			else
			{
				Vector2 vector3 = list[list.Count - 1];
				List<Vector2> list2 = list;
				int index = list.Count - 1;
				float x = vector3.x;
				float y = vector3.y;
				Vector2 vector4 = groupData[i];
				list2[index] = new Vector2(x, y + vector4.y);
			}
			if (i < groupData.Count - 1)
			{
				Vector2 vector5 = groupData[i];
				float x2 = vector5.x;
				Vector2 vector6 = groupData[i + 1];
				if (x2 != vector6.x)
				{
					flag = true;
				}
			}
		}
		if (list.Count < theGraph.groups.Count)
		{
			int num4 = theGraph.groups.Count - list.Count;
			for (int j = 0; j < num4; j++)
			{
				list.Insert(0, new Vector2(-1f, 0f));
			}
		}
		if (list.Count > theGraph.groups.Count)
		{
			int num5 = list.Count - theGraph.groups.Count;
			for (int k = 0; k < num5; k++)
			{
				list.RemoveAt(0);
			}
		}
		List<int> list3 = new List<int>();
		for (int l = 0; l < theGraph.groups.Count; l++)
		{
			list3.Add(l + 1);
		}
		for (int num6 = list.Count - 1; num6 >= 0; num6--)
		{
			Vector2 vector7 = list[num6];
			if (vector7.x > 0f)
			{
				List<int> list4 = list3;
				Vector2 vector8 = list[num6];
				list4.Remove(Mathf.RoundToInt(vector8.x));
			}
		}
		for (int m = 0; m < list3.Count; m++)
		{
			list[m] = new Vector2(-1 * list3[m], 0f);
		}
		list.Sort((Vector2 vec1, Vector2 vec2) => Mathf.Abs(vec1.x).CompareTo(Mathf.Abs(vec2.x)));
		return list;
	}

	private void createOrDeletePoints(int pointValuesCount)
	{
		for (int i = 0; i < pointValuesCount; i++)
		{
			if (points.Count <= i)
			{
				GameObject gameObject = theGraph.CreateNode(nodePrefab, nodeParent);
				theGraph.addNodeClickEvent(gameObject);
				theGraph.addNodeMouseEnterEvent(gameObject);
				theGraph.addNodeMouseLeaveEvent(gameObject);
				gameObject.GetComponent<WMG_Node>().radius = -1f * linePadding;
				theGraph.SetActive(gameObject, state: false);
				points.Add(gameObject);
				barIsNegative.Add(item: false);
				if (i > 0)
				{
					WMG_Node component = points[i - 1].GetComponent<WMG_Node>();
					gameObject = theGraph.CreateLink(component, gameObject, theGraph.linkPrefabs[linkPrefab], linkParent);
					theGraph.addLinkClickEvent(gameObject);
					theGraph.addLinkMouseEnterEvent(gameObject);
					theGraph.addLinkMouseLeaveEvent(gameObject);
					theGraph.SetActive(gameObject, state: false);
					lines.Add(gameObject);
				}
			}
		}
		for (int num = points.Count - 1; num >= 0; num--)
		{
			if (points[num] != null && num >= pointValuesCount)
			{
				WMG_Node component2 = points[num].GetComponent<WMG_Node>();
				foreach (GameObject link3 in component2.links)
				{
					lines.Remove(link3);
				}
				theGraph.DeleteNode(component2);
				points.RemoveAt(num);
				barIsNegative.RemoveAt(num);
			}
			if (num > 1 && num < pointValuesCount - 1)
			{
				WMG_Node component3 = points[0].GetComponent<WMG_Node>();
				WMG_Node component4 = points[num].GetComponent<WMG_Node>();
				WMG_Link link = theGraph.GetLink(component3, component4);
				if (link != null)
				{
					lines.Remove(link.gameObject);
					theGraph.DeleteLink(link);
				}
			}
		}
		if (points.Count > 2)
		{
			WMG_Node component5 = points[0].GetComponent<WMG_Node>();
			WMG_Node component6 = points[points.Count - 1].GetComponent<WMG_Node>();
			WMG_Link link2 = theGraph.GetLink(component5, component6);
			if (connectFirstToLast && link2 == null)
			{
				GameObject gameObject2 = theGraph.CreateLink(component5, component6.gameObject, theGraph.linkPrefabs[linkPrefab], linkParent);
				theGraph.addLinkClickEvent(gameObject2);
				theGraph.addLinkMouseEnterEvent(gameObject2);
				theGraph.addLinkMouseLeaveEvent(gameObject2);
				theGraph.SetActive(gameObject2, state: false);
				lines.Add(gameObject2);
			}
			if (!connectFirstToLast && link2 != null)
			{
				lines.Remove(link2.gameObject);
				theGraph.DeleteLink(link2);
			}
		}
		if (legendEntry.swatchNode == null)
		{
			createLegendSwatch();
		}
	}

	private void createLegendSwatch()
	{
		legendEntry.swatchNode = theGraph.CreateNode(nodePrefab, legendEntry.gameObject);
		theGraph.addNodeClickEvent_Leg(legendEntry.swatchNode);
		theGraph.addNodeMouseEnterEvent_Leg(legendEntry.swatchNode);
		theGraph.addNodeMouseLeaveEvent_Leg(legendEntry.swatchNode);
		WMG_Node component = legendEntry.swatchNode.GetComponent<WMG_Node>();
		theGraph.changeSpritePivot(component.objectToColor, WMG_Text_Functions.WMGpivotTypes.Center);
		component.Reposition(0f, 0f);
		legendEntry.line = theGraph.CreateLink(legendEntry.nodeRight.GetComponent<WMG_Node>(), legendEntry.nodeLeft, theGraph.linkPrefabs[linkPrefab], legendEntry.gameObject);
		theGraph.addLinkClickEvent_Leg(legendEntry.line);
		theGraph.addLinkMouseEnterEvent_Leg(legendEntry.line);
		theGraph.addLinkMouseLeaveEvent_Leg(legendEntry.line);
		theGraph.bringSpriteToFront(legendEntry.swatchNode);
	}

	private void createOrDeleteLabels(int pointValuesCount)
	{
		if (!(dataLabelPrefab != null) || !(dataLabelsParent != null))
		{
			return;
		}
		if (dataLabelsEnabled)
		{
			for (int i = 0; i < pointValuesCount; i++)
			{
				if (dataLabels.Count <= i)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(dataLabelPrefab) as GameObject;
					theGraph.changeSpriteParent(gameObject, dataLabelsParent);
					gameObject.transform.localScale = Vector3.one;
					dataLabels.Add(gameObject);
					gameObject.name = "Data_Label_" + dataLabels.Count;
				}
			}
		}
		int num = pointValuesCount;
		if (!dataLabelsEnabled)
		{
			num = 0;
		}
		else if (theGraph.IsStacked && theGraph.graphType != WMG_Axis_Graph.graphTypes.line_stacked)
		{
			num = 0;
			dataLabelsEnabled = false;
		}
		for (int num2 = dataLabels.Count - 1; num2 >= 0; num2--)
		{
			if (dataLabels[num2] != null && num2 >= num)
			{
				UnityEngine.Object.DestroyImmediate(dataLabels[num2]);
				dataLabels.RemoveAt(num2);
			}
		}
		StartCoroutine(SetDelayedAreaShadingChanged());
	}

	private void createOrDeleteAreaShading(int pointValuesCount)
	{
		if (areaShadingPrefab == null || areaShadingParent == null)
		{
			return;
		}
		if (areaShadingType != 0)
		{
			for (int i = 0; i < pointValuesCount - 1; i++)
			{
				if (areaShadingRects.Count <= i)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(areaShadingPrefab) as GameObject;
					theGraph.changeSpriteParent(gameObject, areaShadingParent);
					gameObject.transform.localScale = Vector3.one;
					areaShadingRects.Add(gameObject);
					gameObject.name = "Area_Shading_" + areaShadingRects.Count;
					StartCoroutine(SetDelayedAreaShadingChanged());
				}
			}
		}
		int num = pointValuesCount - 1;
		if (areaShadingType == areaShadingTypes.None)
		{
			num = 0;
		}
		for (int num2 = areaShadingRects.Count - 1; num2 >= 0; num2--)
		{
			if (areaShadingRects[num2] != null && num2 >= num)
			{
				UnityEngine.Object.DestroyImmediate(areaShadingRects[num2]);
				areaShadingRects.RemoveAt(num2);
				StartCoroutine(SetDelayedAreaShadingChanged());
			}
		}
		Material aMat = areaShadingMatSolid;
		if (areaShadingType == areaShadingTypes.Gradient)
		{
			aMat = areaShadingMatGradient;
		}
		for (int j = 0; j < areaShadingRects.Count; j++)
		{
			theGraph.setTextureMaterial(areaShadingRects[j], aMat);
			StartCoroutine(SetDelayedAreaShadingChanged());
		}
	}

	private IEnumerator SetDelayedAreaShadingChanged()
	{
		yield return new WaitForEndOfFrame();
		AreaShadingChanged();
		yield return new WaitForEndOfFrame();
		AreaShadingChanged();
	}

	public void UpdateSprites()
	{
		List<GameObject> prevPoints = null;
		if (theGraph.IsStacked)
		{
			for (int i = 1; i < theGraph.lineSeries.Count; i++)
			{
				WMG_Series component = theGraph.lineSeries[i].GetComponent<WMG_Series>();
				if (component == this)
				{
					WMG_Series component2 = theGraph.lineSeries[i - 1].GetComponent<WMG_Series>();
					prevPoints = component2.getPoints();
				}
			}
		}
		updatePointSprites(prevPoints);
		updateDataLabels();
		updateAreaShading();
	}

	public void updateXdistBetween()
	{
		if (AutoUpdateXDistBetween)
		{
			_xDistBetweenPoints = theGraph.getDistBetween(points.Count, (theGraph.orientationType != WMG_Axis_Graph.orientationTypes.horizontal) ? theGraph.xAxisLength : theGraph.yAxisLength);
		}
	}

	public void updateExtraXSpace()
	{
		if (theGraph.autoUpdateSeriesAxisSpacing)
		{
			if (theGraph.graphType == WMG_Axis_Graph.graphTypes.line || theGraph.graphType == WMG_Axis_Graph.graphTypes.line_stacked)
			{
				_extraXSpace = 0f;
			}
			else
			{
				_extraXSpace = xDistBetweenPoints / 2f;
			}
		}
	}

	private void updatePointSprites(List<GameObject> prevPoints)
	{
		if (points.Count == 0)
		{
			return;
		}
		float val = theGraph.xAxisLength;
		float val2 = theGraph.yAxisLength;
		float val3 = theGraph.xAxis.AxisMaxValue;
		float val4 = theGraph.yAxis.AxisMaxValue;
		float val5 = theGraph.xAxis.AxisMinValue;
		float val6 = theGraph.yAxis.AxisMinValue;
		if (theGraph.orientationType == WMG_Axis_Graph.orientationTypes.horizontal)
		{
			theGraph.SwapVals(ref val, ref val2);
			theGraph.SwapVals(ref val3, ref val4);
			theGraph.SwapVals(ref val5, ref val6);
		}
		updateXdistBetween();
		updateExtraXSpace();
		List<Vector2> list = new List<Vector2>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		for (int i = 0; i < points.Count && i < pointValues.Count; i++)
		{
			float num = 0f;
			Vector2 vector = pointValues[i];
			float val7 = (vector.y - val6) / (val4 - val6) * val2;
			if (!theGraph.useGroups && UseXDistBetweenToSpace)
			{
				if (i > 0)
				{
					Vector2 vector2 = list[i - 1];
					float num2 = vector2.x;
					float num3 = 0f;
					if (theGraph.orientationType == WMG_Axis_Graph.orientationTypes.horizontal)
					{
						Vector2 vector3 = list[i - 1];
						num2 = vector3.y;
						num3 = theGraph.barWidth;
					}
					num = num2 + this.xDistBetweenPoints;
					if (!seriesIsLine)
					{
						num += num3;
					}
				}
				else
				{
					num = this.extraXSpace;
				}
			}
			else if (theGraph.useGroups)
			{
				float extraXSpace = this.extraXSpace;
				float xDistBetweenPoints = this.xDistBetweenPoints;
				Vector2 vector4 = pointValues[i];
				num = extraXSpace + xDistBetweenPoints * (Mathf.Abs(vector4.x) - 1f);
			}
			else
			{
				Vector2 vector5 = pointValues[i];
				num = (vector5.x - val5) / (val3 - val5) * val;
			}
			if (theGraph.orientationType == WMG_Axis_Graph.orientationTypes.horizontal)
			{
				theGraph.SwapVals(ref num, ref val7);
			}
			int num4 = 0;
			int num5 = 0;
			if (seriesIsLine)
			{
				num4 = Mathf.RoundToInt(pointWidthHeight);
				num5 = Mathf.RoundToInt(pointWidthHeight);
				if (theGraph.graphType == WMG_Axis_Graph.graphTypes.line_stacked)
				{
					if (theGraph.orientationType == WMG_Axis_Graph.orientationTypes.vertical)
					{
						if (prevPoints != null && i < prevPoints.Count)
						{
							val7 += theGraph.getSpritePositionY(prevPoints[i]);
						}
					}
					else if (prevPoints != null && i < prevPoints.Count)
					{
						num += theGraph.getSpritePositionX(prevPoints[i]);
					}
				}
			}
			else
			{
				if (theGraph.graphType == WMG_Axis_Graph.graphTypes.bar_stacked_percent && theGraph.TotalPointValues.Count > i)
				{
					if (theGraph.orientationType == WMG_Axis_Graph.orientationTypes.vertical)
					{
						Vector2 vector6 = pointValues[i];
						val7 = (vector6.y - val6) / theGraph.TotalPointValues[i] * val2;
					}
					else
					{
						Vector2 vector7 = pointValues[i];
						num = (vector7.y - val6) / theGraph.TotalPointValues[i] * val2;
					}
				}
				if (theGraph.orientationType == WMG_Axis_Graph.orientationTypes.vertical)
				{
					num4 = Mathf.RoundToInt(theGraph.barWidth);
					num5 = Mathf.RoundToInt(val7);
					int num6 = 0;
					if (theGraph.graphType == WMG_Axis_Graph.graphTypes.bar_side || (theGraph.graphType == WMG_Axis_Graph.graphTypes.combo && comboType == comboTypes.bar))
					{
						num6 = Mathf.RoundToInt((theGraph.barAxisValue - val6) / (val4 - val6) * val2);
					}
					num5 -= num6;
					val7 -= (float)num5;
					barIsNegative[i] = false;
					if (num5 < 0)
					{
						num5 *= -1;
						val7 -= (float)num5;
						barIsNegative[i] = true;
					}
					if (prevPoints != null && i < prevPoints.Count)
					{
						val7 += theGraph.getSpritePositionY(prevPoints[i]) + theGraph.getSpriteHeight(prevPoints[i]);
					}
				}
				else
				{
					num4 = Mathf.RoundToInt(num);
					num5 = Mathf.RoundToInt(theGraph.barWidth);
					int num7 = 0;
					if (theGraph.graphType == WMG_Axis_Graph.graphTypes.bar_side || (theGraph.graphType == WMG_Axis_Graph.graphTypes.combo && comboType == comboTypes.bar))
					{
						num7 = Mathf.RoundToInt((theGraph.barAxisValue - val6) / (val4 - val6) * val2);
					}
					num4 -= num7;
					num = num7;
					val7 -= theGraph.barWidth;
					barIsNegative[i] = false;
					if (num4 < 0)
					{
						num4 *= -1;
						num -= (float)num4;
						barIsNegative[i] = true;
					}
					if (prevPoints != null && i < prevPoints.Count)
					{
						num += theGraph.getSpritePositionX(prevPoints[i]) + theGraph.getSpriteWidth(prevPoints[i]);
					}
				}
			}
			list2.Add(num4);
			list3.Add(num5);
			list.Add(new Vector2(num, val7));
		}
		if (animatingFromPreviousData)
		{
			if (seriesIsLine)
			{
				for (int j = 0; j < points.Count && j < pointValues.Count; j++)
				{
					list[j] = theGraph.getChangeSpritePositionTo(points[j], list[j]);
				}
			}
			afterPositions = new List<Vector2>(list);
			afterWidths = new List<int>(list2);
			afterHeights = new List<int>(list3);
			OnSeriesDataChanged();
			animatingFromPreviousData = false;
			return;
		}
		for (int k = 0; k < points.Count && k < pointValues.Count; k++)
		{
			if (!seriesIsLine)
			{
				WMG_Node component = points[k].GetComponent<WMG_Node>();
				theGraph.changeBarWidthHeight(component.objectToColor, list2[k], list3[k]);
			}
			WMG_Axis_Graph wMG_Axis_Graph = theGraph;
			GameObject obj = points[k];
			Vector2 vector8 = list[k];
			float x = vector8.x;
			Vector2 vector9 = list[k];
			wMG_Axis_Graph.changeSpritePositionTo(obj, new Vector3(x, vector9.y, 0f));
		}
		RepositionLines();
	}

	private void updateDataLabels()
	{
		if (!dataLabelsEnabled)
		{
			return;
		}
		for (int i = 0; i < dataLabels.Count; i++)
		{
			Vector2 vector = new Vector2(theGraph.getSpritePositionX(points[i]), theGraph.getSpritePositionY(points[i]));
			theGraph.changeLabelFontSize(dataLabels[i], dataLabelsFontSize);
			WMG_Axis_Graph wMG_Axis_Graph = theGraph;
			GameObject obj = dataLabels[i];
			SeriesDataLabeler obj2 = seriesDataLabeler;
			Vector2 vector2 = pointValues[i];
			wMG_Axis_Graph.changeLabelText(obj, obj2(this, vector2.y));
			if (seriesIsLine)
			{
				WMG_Axis_Graph wMG_Axis_Graph2 = theGraph;
				GameObject obj3 = dataLabels[i];
				Vector2 dataLabelsOffset = this.dataLabelsOffset;
				float x = dataLabelsOffset.x + vector.x;
				Vector2 dataLabelsOffset2 = this.dataLabelsOffset;
				wMG_Axis_Graph2.changeSpritePositionTo(obj3, new Vector3(x, dataLabelsOffset2.y + vector.y, 0f));
				continue;
			}
			if (theGraph.orientationType == WMG_Axis_Graph.orientationTypes.vertical)
			{
				Vector2 dataLabelsOffset3 = this.dataLabelsOffset;
				float y = dataLabelsOffset3.y + vector.y + theGraph.getSpriteHeight(points[i]);
				if (barIsNegative[i])
				{
					Vector2 dataLabelsOffset4 = this.dataLabelsOffset;
					y = 0f - dataLabelsOffset4.y - theGraph.getSpriteHeight(points[i]) + (float)Mathf.RoundToInt((theGraph.barAxisValue - theGraph.yAxis.AxisMinValue) / (theGraph.yAxis.AxisMaxValue - theGraph.yAxis.AxisMinValue) * theGraph.yAxisLength);
				}
				WMG_Axis_Graph wMG_Axis_Graph3 = theGraph;
				GameObject obj4 = dataLabels[i];
				Vector2 dataLabelsOffset5 = this.dataLabelsOffset;
				wMG_Axis_Graph3.changeSpritePositionTo(obj4, new Vector3(dataLabelsOffset5.x + vector.x + theGraph.barWidth / 2f, y, 0f));
				continue;
			}
			Vector2 dataLabelsOffset6 = this.dataLabelsOffset;
			float num = dataLabelsOffset6.x + vector.x + theGraph.getSpriteWidth(points[i]);
			if (barIsNegative[i])
			{
				Vector2 dataLabelsOffset7 = this.dataLabelsOffset;
				num = 0f - dataLabelsOffset7.x - theGraph.getSpriteWidth(points[i]) + (float)Mathf.RoundToInt((theGraph.barAxisValue - theGraph.xAxis.AxisMinValue) / (theGraph.xAxis.AxisMaxValue - theGraph.xAxis.AxisMinValue) * theGraph.xAxisLength);
			}
			WMG_Axis_Graph wMG_Axis_Graph4 = theGraph;
			GameObject obj5 = dataLabels[i];
			float x2 = num;
			Vector2 dataLabelsOffset8 = this.dataLabelsOffset;
			wMG_Axis_Graph4.changeSpritePositionTo(obj5, new Vector3(x2, dataLabelsOffset8.y + vector.y + theGraph.barWidth / 2f, 0f));
		}
	}

	public void updateAreaShading()
	{
		if (areaShadingType == areaShadingTypes.None)
		{
			return;
		}
		float num = float.NegativeInfinity;
		for (int i = 0; i < points.Count && i < pointValues.Count; i++)
		{
			Vector2 vector = pointValues[i];
			if (vector.y > num)
			{
				Vector2 vector2 = pointValues[i];
				num = vector2.y;
			}
		}
		for (int j = 0; j < points.Count - 1 && j < pointValues.Count; j++)
		{
			int num2 = 180;
			Vector2 vector3 = new Vector2(theGraph.getSpritePositionX(points[j]), theGraph.getSpritePositionY(points[j]));
			Vector2 vector4 = new Vector2(theGraph.getSpritePositionX(points[j + 1]), theGraph.getSpritePositionY(points[j + 1]));
			float num3 = theGraph.yAxisLength / (theGraph.yAxis.AxisMaxValue - theGraph.yAxis.AxisMinValue);
			float num4 = (areaShadingAxisValue - theGraph.yAxis.AxisMinValue) * num3;
			if (theGraph.orientationType == WMG_Axis_Graph.orientationTypes.horizontal)
			{
				num2 = 90;
				vector3 = new Vector2(theGraph.getSpritePositionY(points[j]), theGraph.getSpritePositionX(points[j]));
				vector4 = new Vector2(theGraph.getSpritePositionY(points[j + 1]), theGraph.getSpritePositionX(points[j + 1]));
				num3 = theGraph.xAxisLength / (theGraph.xAxis.AxisMaxValue - theGraph.xAxis.AxisMinValue);
				num4 = (areaShadingAxisValue - theGraph.xAxis.AxisMinValue) * num3;
			}
			areaShadingRects[j].transform.localEulerAngles = new Vector3(0f, 0f, num2);
			float num5 = Mathf.Max(vector4.y, vector3.y);
			float num6 = Mathf.Min(vector4.y, vector3.y);
			int num7 = Mathf.RoundToInt(vector3.x);
			int num8 = Mathf.RoundToInt(vector4.x - vector3.x);
			float num9 = num5 - num6;
			Vector2 vector5 = pointValues[j + 1];
			float y = vector5.y;
			Vector2 vector6 = pointValues[j];
			float num10 = num9 + (Mathf.Min(y, vector6.y) - areaShadingAxisValue) * num3;
			if (num6 < num4)
			{
				float num11 = (vector4.y - vector3.y) / (vector4.x - vector3.x);
				if (vector4.y > vector3.y)
				{
					float num12 = num4 - num6;
					int num13 = Mathf.RoundToInt(num12 / num11);
					num8 -= num13;
					num7 += num13;
				}
				else
				{
					float num14 = num4 - num6;
					int num15 = Mathf.RoundToInt(num14 / num11 * -1f);
					num8 -= num15;
				}
			}
			if (theGraph.orientationType == WMG_Axis_Graph.orientationTypes.horizontal)
			{
				theGraph.changeSpritePositionTo(areaShadingRects[j], new Vector3(num5, num7 + num8, 0f));
			}
			else
			{
				theGraph.changeSpritePositionTo(areaShadingRects[j], new Vector3(num7, num5, 0f));
			}
			theGraph.changeSpriteSizeFloat(areaShadingRects[j], num8, num10);
			if (j > 0)
			{
				if (theGraph.orientationType == WMG_Axis_Graph.orientationTypes.horizontal)
				{
					int num16 = Mathf.RoundToInt(theGraph.getSpritePositionY(areaShadingRects[j])) - Mathf.RoundToInt(theGraph.getSpriteWidth(areaShadingRects[j]));
					int num17 = Mathf.RoundToInt(theGraph.getSpritePositionY(areaShadingRects[j - 1]));
					if (num16 > num17)
					{
						theGraph.changeSpriteWidth(areaShadingRects[j], Mathf.RoundToInt(theGraph.getSpriteWidth(areaShadingRects[j]) + 1f));
					}
					if (num16 < num17)
					{
						theGraph.changeSpriteWidth(areaShadingRects[j], Mathf.RoundToInt(theGraph.getSpriteWidth(areaShadingRects[j]) - 1f));
					}
				}
				else
				{
					int num18 = Mathf.RoundToInt(theGraph.getSpriteWidth(areaShadingRects[j - 1])) + Mathf.RoundToInt(theGraph.getSpritePositionX(areaShadingRects[j - 1]));
					if (num18 > Mathf.RoundToInt(theGraph.getSpritePositionX(areaShadingRects[j])))
					{
						theGraph.changeSpriteWidth(areaShadingRects[j - 1], Mathf.RoundToInt(theGraph.getSpriteWidth(areaShadingRects[j - 1]) - 1f));
					}
					if (num18 < Mathf.RoundToInt(theGraph.getSpritePositionX(areaShadingRects[j])))
					{
						theGraph.changeSpriteWidth(areaShadingRects[j - 1], Mathf.RoundToInt(theGraph.getSpriteWidth(areaShadingRects[j - 1]) + 1f));
					}
				}
			}
			Material textureMaterial = theGraph.getTextureMaterial(areaShadingRects[j]);
			if (!(textureMaterial == null))
			{
				if (theGraph.orientationType == WMG_Axis_Graph.orientationTypes.horizontal)
				{
					textureMaterial.SetFloat("_Slope", (0f - (vector4.y - vector3.y)) / num10);
				}
				else
				{
					textureMaterial.SetFloat("_Slope", (vector4.y - vector3.y) / num10);
				}
				textureMaterial.SetColor("_Color", this.areaShadingColor);
				Material material = textureMaterial;
				Color areaShadingColor = this.areaShadingColor;
				material.SetFloat("_Transparency", 1f - areaShadingColor.a);
				Material material2 = textureMaterial;
				Vector2 vector7 = pointValues[j + 1];
				float y2 = vector7.y;
				Vector2 vector8 = pointValues[j];
				material2.SetFloat("_GradientScale", (Mathf.Max(y2, vector8.y) - areaShadingAxisValue) / (num - areaShadingAxisValue));
			}
		}
	}

	public void StartRealTimeUpdate()
	{
		if (!realTimeRunning && realTimeDataSource != null)
		{
			realTimeRunning = true;
			pointValues.SetListNoCb(new List<Vector2>(), ref _pointValues);
			pointValues.AddNoCb(new Vector2(0f, realTimeDataSource.getDatum<float>()), ref _pointValues);
			realTimeLoopVar = 0f;
			if (theGraph.orientationType == WMG_Axis_Graph.orientationTypes.vertical)
			{
				realTimeOrigMax = theGraph.xAxis.AxisMaxValue;
			}
			else
			{
				realTimeOrigMax = theGraph.yAxis.AxisMaxValue;
			}
		}
	}

	public void StopRealTimeUpdate()
	{
		realTimeRunning = false;
	}

	public void ResumeRealTimeUpdate()
	{
		realTimeRunning = true;
	}

	private void DoRealTimeUpdate()
	{
		float num = 0.0166f;
		realTimeLoopVar += num;
		float datum = realTimeDataSource.getDatum<float>();
		int num2 = 2;
		if (pointValues.Count >= 2)
		{
			float num3 = 0.3f;
			float num4 = (theGraph.yAxis.AxisMaxValue - theGraph.yAxis.AxisMinValue) / (theGraph.xAxis.AxisMaxValue - theGraph.xAxis.AxisMinValue);
			float[] array = new float[num2];
			Vector2 vector = new Vector2(realTimeLoopVar, datum);
			for (int i = 0; i < array.Length; i++)
			{
				Vector2 vector2 = pointValues[pointValues.Count - (i + 1)];
				array[i] = (vector.y - vector2.y) / (vector.x - vector2.x) / num4;
			}
			if (Mathf.Abs(array[0] - array[1]) <= num3)
			{
				pointValues[pointValues.Count - 1] = new Vector2(realTimeLoopVar, datum);
			}
			else
			{
				pointValues.Add(new Vector2(realTimeLoopVar, datum));
			}
		}
		else
		{
			pointValues.Add(new Vector2(realTimeLoopVar, datum));
		}
		if (pointValues.Count <= 1)
		{
			return;
		}
		Vector2 vector3 = pointValues[pointValues.Count - 1];
		if (vector3.x > realTimeOrigMax)
		{
			if (theGraph.orientationType == WMG_Axis_Graph.orientationTypes.vertical)
			{
				theGraph.xAxis.AxisMinValue = realTimeLoopVar - realTimeOrigMax;
				theGraph.xAxis.AxisMaxValue = realTimeLoopVar;
			}
			else
			{
				theGraph.yAxis.AxisMinValue = realTimeLoopVar - realTimeOrigMax;
				theGraph.yAxis.AxisMaxValue = realTimeLoopVar;
			}
			Vector2 vector4 = pointValues[0];
			float x = vector4.x;
			Vector2 vector5 = pointValues[1];
			float x2 = vector5.x;
			Vector2 vector6 = pointValues[0];
			float y = vector6.y;
			Vector2 vector7 = pointValues[1];
			float y2 = vector7.y;
			if (Mathf.Approximately(x + num, x2))
			{
				pointValues.RemoveAt(0);
			}
			else
			{
				pointValues[0] = new Vector2(x + num, y + (y2 - y) / (x2 - x) * num);
			}
		}
	}

	public void deleteAllNodesFromGraphManager()
	{
		for (int num = points.Count - 1; num >= 0; num--)
		{
			theGraph.DeleteNode(points[num].GetComponent<WMG_Node>());
		}
		theGraph.DeleteNode(legendEntry.nodeLeft.GetComponent<WMG_Node>());
		theGraph.DeleteNode(legendEntry.nodeRight.GetComponent<WMG_Node>());
		theGraph.DeleteNode(legendEntry.swatchNode.GetComponent<WMG_Node>());
	}
}
