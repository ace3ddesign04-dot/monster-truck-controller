using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BodyPartsData
{
	public float Dirtiness;

	public PartGroupData[] partGroupsData;

	public List<Wrap> Wraps;

	public int WrapLayerCount;

	public Color BodyColor;

	public Color WrapColor;

	public Color FRimsColor;

	public Color FBeadlocksColor;

	public Color RRimsColor;

	public Color RBeadlocksColor;

	public int WrapID;

	public Vector4 WrapCoords;

	public bool GlossyPaint;

	public bool GlossyPaintPurchased;
}
