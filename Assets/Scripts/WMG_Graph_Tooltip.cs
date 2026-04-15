using UnityEngine;
using UnityEngine.UI;

public class WMG_Graph_Tooltip : WMG_GUI_Functions
{
	public delegate string TooltipLabeler(WMG_Series series, WMG_Node node);

	public TooltipLabeler tooltipLabeler;

	public WMG_Axis_Graph theGraph;

	private Canvas _canvas;

	private void Start()
	{
		_canvas = theGraph.toolTipPanel.GetComponent<Graphic>().canvas;
	}

	private void Update()
	{
		if (theGraph.tooltipEnabled && !isTooltipObjectNull() && getControlVisibility(theGraph.toolTipPanel))
		{
			repositionTooltip();
		}
	}

	public void subscribeToEvents(bool val)
	{
		if (val)
		{
			theGraph.WMG_MouseEnter += TooltipNodeMouseEnter;
			theGraph.WMG_MouseEnter_Leg += TooltipLegendNodeMouseEnter;
			theGraph.WMG_Link_MouseEnter_Leg += TooltipLegendLinkMouseEnter;
			tooltipLabeler = defaultTooltipLabeler;
		}
		else
		{
			theGraph.WMG_MouseEnter -= TooltipNodeMouseEnter;
			theGraph.WMG_MouseEnter_Leg -= TooltipLegendNodeMouseEnter;
			theGraph.WMG_Link_MouseEnter_Leg -= TooltipLegendLinkMouseEnter;
		}
	}

	private bool isTooltipObjectNull()
	{
		if (theGraph.toolTipPanel == null)
		{
			return true;
		}
		if (theGraph.toolTipLabel == null)
		{
			return true;
		}
		return false;
	}

	private void repositionTooltip()
	{
		RectTransform component = theGraph.toolTipPanel.GetComponent<RectTransform>();
		Vector3 mousePosition = UnityEngine.Input.mousePosition;
		float x = mousePosition.x;
		Vector3 mousePosition2 = UnityEngine.Input.mousePosition;
		RectTransformUtility.ScreenPointToWorldPointInRectangle(component, new Vector2(x, mousePosition2.y), (_canvas.renderMode != 0) ? _canvas.worldCamera : null, out Vector3 worldPoint);
		float x2 = theGraph.tooltipOffset.x;
		float y = theGraph.tooltipOffset.y;
		theGraph.toolTipPanel.transform.localPosition = theGraph.toolTipPanel.transform.parent.InverseTransformPoint(worldPoint) + new Vector3(x2, y + 13f, 0f);
		EnsureTooltipStaysOnScreen(worldPoint, x2, y);
	}

	private void EnsureTooltipStaysOnScreen(Vector3 position, float offsetX, float offsetY)
	{
		Vector3 position2 = theGraph.toolTipPanel.transform.position;
		Vector3[] array = new Vector3[4];
		((RectTransform)theGraph.toolTipPanel.transform).GetWorldCorners(array);
		float num = array[2].x - array[0].x;
		float num2 = array[1].y - array[0].y;
		float num3 = position.x + offsetX + num - (float)Screen.width;
		if (num3 > 0f)
		{
			position2 = new Vector3(position.x - num3 + offsetX, position2.y, position2.z);
		}
		else
		{
			num3 = position.x + offsetX;
			if (num3 < 0f)
			{
				position2 = new Vector3(position.x - num3 + offsetX, position2.y, position2.z);
			}
		}
		float num4 = position.y + offsetY + num2 - (float)Screen.height;
		if (num4 > 0f)
		{
			position2 = new Vector3(position2.x, position.y - num4 + offsetY + num2 / 2f, position2.z);
		}
		else
		{
			num4 = position.y + offsetY;
			if (num4 < 0f)
			{
				position2 = new Vector3(position2.x, position.y - num4 + offsetY + num2 / 2f, position2.z);
			}
		}
		theGraph.toolTipPanel.transform.position = position2;
	}

	private void TooltipNodeMouseEnter(WMG_Series aSeries, WMG_Node aNode, bool state)
	{
		if (isTooltipObjectNull())
		{
			return;
		}
		if (state)
		{
			changeLabelText(theGraph.toolTipLabel, tooltipLabeler(aSeries, aNode));
			changeSpriteWidth(theGraph.toolTipPanel, Mathf.RoundToInt(getSpriteWidth(theGraph.toolTipLabel)) + 24);
			repositionTooltip();
			showControl(theGraph.toolTipPanel);
			bringSpriteToFront(theGraph.toolTipPanel);
			Vector3 newScale = new Vector3(2f, 2f, 1f);
			if (!aSeries.seriesIsLine)
			{
				newScale = ((theGraph.orientationType == WMG_Axis_Graph.orientationTypes.vertical) ? new Vector3(1f, 1.1f, 1f) : new Vector3(1.1f, 1f, 1f));
			}
			performTooltipAnimation(aNode.transform, newScale);
		}
		else
		{
			hideControl(theGraph.toolTipPanel);
			sendSpriteToBack(theGraph.toolTipPanel);
			performTooltipAnimation(aNode.transform, new Vector3(1f, 1f, 1f));
		}
	}

	private string defaultTooltipLabeler(WMG_Series aSeries, WMG_Node aNode)
	{
		Vector2 nodeValue = aSeries.getNodeValue(aNode);
		float num = Mathf.Pow(10f, aSeries.theGraph.tooltipNumberDecimals);
		string text = (Mathf.Round(nodeValue.x * num) / num).ToString();
		string text2 = (Mathf.Round(nodeValue.y * num) / num).ToString();
		string text3 = (!aSeries.seriesIsLine) ? text2 : ("(" + text + ", " + text2 + ")");
		if (aSeries.theGraph.tooltipDisplaySeriesName)
		{
			text3 = aSeries.seriesName + ": " + text3;
		}
		return text3;
	}

	private void TooltipLegendNodeMouseEnter(WMG_Series aSeries, WMG_Node aNode, bool state)
	{
		if (!isTooltipObjectNull())
		{
			if (state)
			{
				changeLabelText(theGraph.toolTipLabel, aSeries.seriesName);
				changeSpriteWidth(theGraph.toolTipPanel, Mathf.RoundToInt(getSpriteWidth(theGraph.toolTipLabel)) + 24);
				repositionTooltip();
				showControl(theGraph.toolTipPanel);
				bringSpriteToFront(theGraph.toolTipPanel);
				performTooltipAnimation(aNode.transform, new Vector3(2f, 2f, 1f));
			}
			else
			{
				hideControl(theGraph.toolTipPanel);
				sendSpriteToBack(theGraph.toolTipPanel);
				performTooltipAnimation(aNode.transform, new Vector3(1f, 1f, 1f));
			}
		}
	}

	private void TooltipLegendLinkMouseEnter(WMG_Series aSeries, WMG_Link aLink, bool state)
	{
		if (!isTooltipObjectNull() && aSeries.hidePoints)
		{
			if (state)
			{
				changeLabelText(theGraph.toolTipLabel, aSeries.seriesName);
				changeSpriteWidth(theGraph.toolTipPanel, Mathf.RoundToInt(getSpriteWidth(theGraph.toolTipLabel)) + 24);
				repositionTooltip();
				showControl(theGraph.toolTipPanel);
				bringSpriteToFront(theGraph.toolTipPanel);
				performTooltipAnimation(aLink.transform, new Vector3(2f, 1.05f, 1f));
			}
			else
			{
				hideControl(theGraph.toolTipPanel);
				sendSpriteToBack(theGraph.toolTipPanel);
				performTooltipAnimation(aLink.transform, new Vector3(1f, 1f, 1f));
			}
		}
	}

	private void performTooltipAnimation(Transform trans, Vector3 newScale)
	{
		if (theGraph.tooltipAnimationsEnabled)
		{
			WMG_Anim.animScale(trans.gameObject, theGraph.tooltipAnimationsDuration, theGraph.tooltipAnimationsEasetype, newScale, 0f);
		}
	}
}
