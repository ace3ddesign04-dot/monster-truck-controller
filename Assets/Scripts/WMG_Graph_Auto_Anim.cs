using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class WMG_Graph_Auto_Anim : MonoBehaviour
{
	public WMG_Axis_Graph theGraph;

	public void subscribeToEvents(bool val)
	{
		for (int i = 0; i < theGraph.lineSeries.Count; i++)
		{
			if (theGraph.activeInHierarchy(theGraph.lineSeries[i]))
			{
				WMG_Series component = theGraph.lineSeries[i].GetComponent<WMG_Series>();
				if (val)
				{
					component.SeriesDataChanged += SeriesDataChangedMethod;
				}
				else
				{
					component.SeriesDataChanged -= SeriesDataChangedMethod;
				}
			}
		}
	}

	public void addSeriesForAutoAnim(WMG_Series aSeries)
	{
		aSeries.SeriesDataChanged += SeriesDataChangedMethod;
	}

	private void SeriesDataChangedMethod(WMG_Series aSeries)
	{
		List<GameObject> points = aSeries.getPoints();
		for (int i = 0; i < points.Count; i++)
		{
			if (aSeries.seriesIsLine)
			{
				GameObject go = points[i];
				string text = aSeries.GetHashCode() + "autoAnim" + i;
				bool isLast = i == points.Count - 1;
				if (aSeries.currentlyAnimating)
				{
					DOTween.Kill(text);
					animateLinkCallback(aSeries, go, isLast);
				}
				GameObject obj = go;
				float autoAnimationsDuration = theGraph.autoAnimationsDuration;
				Ease autoAnimationsEasetype = theGraph.autoAnimationsEasetype;
				Vector2 vector = aSeries.AfterPositions()[i];
				float x = vector.x;
				Vector2 vector2 = aSeries.AfterPositions()[i];
				WMG_Anim.animPositionCallbacks(obj, autoAnimationsDuration, autoAnimationsEasetype, new Vector3(x, vector2.y), delegate
				{
					animateLinkCallback(aSeries, go, isLast);
				}, delegate
				{
					animateLinkCallbackEnd(aSeries, isLast);
				}, text);
			}
			else
			{
				WMG_Axis_Graph wMG_Axis_Graph = theGraph;
				GameObject obj2 = points[i];
				Vector2 vector3 = aSeries.AfterPositions()[i];
				float x2 = vector3.x;
				Vector2 vector4 = aSeries.AfterPositions()[i];
				Vector2 changeSpritePositionTo = wMG_Axis_Graph.getChangeSpritePositionTo(obj2, new Vector2(x2, vector4.y));
				WMG_Anim.animPosition(points[i], theGraph.autoAnimationsDuration, theGraph.autoAnimationsEasetype, new Vector3(changeSpritePositionTo.x, changeSpritePositionTo.y));
				WMG_Anim.animSize(points[i], theGraph.autoAnimationsDuration, theGraph.autoAnimationsEasetype, new Vector2(aSeries.AfterWidths()[i], aSeries.AfterHeights()[i]));
			}
		}
		List<GameObject> dataLabels = aSeries.getDataLabels();
		for (int j = 0; j < dataLabels.Count; j++)
		{
			if (aSeries.seriesIsLine)
			{
				Vector2 dataLabelsOffset = aSeries.dataLabelsOffset;
				float x3 = dataLabelsOffset.x;
				Vector2 dataLabelsOffset2 = aSeries.dataLabelsOffset;
				float y = dataLabelsOffset2.y;
				Vector2 vector5 = theGraph.getChangeSpritePositionTo(dataLabels[j], new Vector2(x3, y));
				float x4 = vector5.x;
				Vector2 vector6 = aSeries.AfterPositions()[j];
				float x5 = x4 + vector6.x;
				float y2 = vector5.y;
				Vector2 vector7 = aSeries.AfterPositions()[j];
				vector5 = new Vector2(x5, y2 + vector7.y);
				WMG_Anim.animPosition(dataLabels[j], theGraph.autoAnimationsDuration, theGraph.autoAnimationsEasetype, new Vector3(vector5.x, vector5.y));
				continue;
			}
			Vector2 dataLabelsOffset3 = aSeries.dataLabelsOffset;
			float y3 = dataLabelsOffset3.y;
			Vector2 vector8 = aSeries.AfterPositions()[j];
			float y4 = y3 + vector8.y + theGraph.barWidth / 2f;
			Vector2 dataLabelsOffset4 = aSeries.dataLabelsOffset;
			float x6 = dataLabelsOffset4.x;
			Vector2 vector9 = aSeries.AfterPositions()[j];
			float x7 = x6 + vector9.x + (float)aSeries.AfterWidths()[j];
			if (aSeries.getBarIsNegative(j))
			{
				Vector2 dataLabelsOffset5 = aSeries.dataLabelsOffset;
				x7 = 0f - dataLabelsOffset5.x - (float)aSeries.AfterWidths()[j] + (float)Mathf.RoundToInt((theGraph.barAxisValue - theGraph.xAxis.AxisMinValue) / (theGraph.xAxis.AxisMaxValue - theGraph.xAxis.AxisMinValue) * theGraph.xAxisLength);
			}
			if (theGraph.orientationType == WMG_Axis_Graph.orientationTypes.vertical)
			{
				Vector2 dataLabelsOffset6 = aSeries.dataLabelsOffset;
				float y5 = dataLabelsOffset6.y;
				Vector2 vector10 = aSeries.AfterPositions()[j];
				y4 = y5 + vector10.y + (float)aSeries.AfterHeights()[j];
				Vector2 dataLabelsOffset7 = aSeries.dataLabelsOffset;
				float x8 = dataLabelsOffset7.x;
				Vector2 vector11 = aSeries.AfterPositions()[j];
				x7 = x8 + vector11.x + theGraph.barWidth / 2f;
				if (aSeries.getBarIsNegative(j))
				{
					Vector2 dataLabelsOffset8 = aSeries.dataLabelsOffset;
					y4 = 0f - dataLabelsOffset8.y - (float)aSeries.AfterHeights()[j] + (float)Mathf.RoundToInt((theGraph.barAxisValue - theGraph.yAxis.AxisMinValue) / (theGraph.yAxis.AxisMaxValue - theGraph.yAxis.AxisMinValue) * theGraph.yAxisLength);
				}
			}
			Vector2 changeSpritePositionTo2 = theGraph.getChangeSpritePositionTo(dataLabels[j], new Vector2(x7, y4));
			WMG_Anim.animPosition(dataLabels[j], theGraph.autoAnimationsDuration, theGraph.autoAnimationsEasetype, new Vector3(changeSpritePositionTo2.x, changeSpritePositionTo2.y));
		}
		if (!aSeries.currentlyAnimating)
		{
			aSeries.currentlyAnimating = true;
		}
	}

	private void animateLinkCallback(WMG_Series aSeries, GameObject aGO, bool isLast)
	{
		WMG_Node component = aGO.GetComponent<WMG_Node>();
		if (component.links.Count != 0)
		{
			WMG_Link component2 = component.links[component.links.Count - 1].GetComponent<WMG_Link>();
			component2.Reposition();
		}
		if (isLast)
		{
			aSeries.updateAreaShading();
		}
		if (aSeries.connectFirstToLast)
		{
			component = aSeries.getPoints()[0].GetComponent<WMG_Node>();
			WMG_Link component3 = component.links[0].GetComponent<WMG_Link>();
			component3.Reposition();
		}
	}

	private void animateLinkCallbackEnd(WMG_Series aSeries, bool isLast)
	{
		aSeries.RepositionLines();
		if (isLast)
		{
			aSeries.updateAreaShading();
		}
		aSeries.currentlyAnimating = false;
	}
}
