using System;
using UnityEngine;

public class WMG_Link : WMG_GUI_Functions
{
	public int id;

	public GameObject fromNode;

	public GameObject toNode;

	public GameObject objectToScale;

	public GameObject objectToColor;

	public GameObject objectToLabel;

	public bool weightIsLength;

	public bool updateLabelWithLength;

	public bool isSelected;

	public bool wasSelected;

	public float weight;

	public void Setup(GameObject fromNode, GameObject toNode, int linkId, bool repos)
	{
		this.fromNode = fromNode;
		this.toNode = toNode;
		SetId(linkId);
		WMG_Node component = fromNode.GetComponent<WMG_Node>();
		WMG_Node component2 = toNode.GetComponent<WMG_Node>();
		base.name = "WMG_Link_" + component.id + "_" + component2.id;
		if (repos)
		{
			Reposition();
		}
	}

	public void Reposition()
	{
		float num = getSpritePositionX(toNode) - getSpritePositionX(fromNode);
		float num2 = getSpritePositionY(toNode) - getSpritePositionY(fromNode);
		float num3 = Mathf.Atan2(num2, num) * 57.29578f + 90f;
		WMG_Node component = fromNode.GetComponent<WMG_Node>();
		WMG_Node component2 = toNode.GetComponent<WMG_Node>();
		SetNodeAngles(num3, component, component2);
		float num4 = component.radius + component2.radius;
		float num5 = Mathf.Sqrt(Mathf.Pow(num2, 2f) + Mathf.Pow(num, 2f)) - num4;
		if (num5 < 0f)
		{
			num5 = 0f;
		}
		float num6 = (component.radius - component2.radius) / 2f * Mathf.Cos((float)Math.PI / 180f * (num3 - 90f));
		float num7 = (component.radius - component2.radius) / 2f * Mathf.Sin((float)Math.PI / 180f * (num3 - 90f));
		float squareCircleOffsetLength = getSquareCircleOffsetLength(component, num3, isFrom: true);
		float squareCircleOffsetLength2 = getSquareCircleOffsetLength(component2, num3, isFrom: false);
		num5 = num5 - squareCircleOffsetLength - squareCircleOffsetLength2;
		float num8 = (squareCircleOffsetLength - squareCircleOffsetLength2) / 2f * Mathf.Cos((float)Math.PI / 180f * (num3 - 90f));
		float num9 = (squareCircleOffsetLength - squareCircleOffsetLength2) / 2f * Mathf.Sin((float)Math.PI / 180f * (num3 - 90f));
		if (weightIsLength)
		{
			weight = num5;
		}
		if (updateLabelWithLength && objectToLabel != null)
		{
			changeLabelText(objectToLabel, Mathf.Round(num5).ToString());
			objectToLabel.transform.localEulerAngles = new Vector3(0f, 0f, 360f - num3);
		}
		Transform transform = base.transform;
		float num10 = getSpriteFactorY2(objectToScale) * num;
		Vector3 localPosition = fromNode.transform.localPosition;
		float x = num10 + localPosition.x + num6 + num8;
		float num11 = getSpriteFactorY2(objectToScale) * num2;
		Vector3 localPosition2 = fromNode.transform.localPosition;
		float y = num11 + localPosition2.y + num7 + num9;
		Vector3 localPosition3 = base.transform.localPosition;
		transform.localPosition = new Vector3(x, y, localPosition3.z);
		changeSpriteHeight(objectToScale, Mathf.RoundToInt(num5));
		base.transform.localEulerAngles = new Vector3(0f, 0f, num3);
	}

	public void SetId(int linkId)
	{
		id = linkId;
	}

	private void SetNodeAngles(float angle, WMG_Node fromN, WMG_Node toN)
	{
		for (int i = 0; i < fromN.numLinks; i++)
		{
			WMG_Link component = fromN.links[i].GetComponent<WMG_Link>();
			if (component.id == id)
			{
				fromN.linkAngles[i] = angle - 90f;
			}
		}
		for (int j = 0; j < toN.numLinks; j++)
		{
			WMG_Link component2 = toN.links[j].GetComponent<WMG_Link>();
			if (component2.id == id)
			{
				toN.linkAngles[j] = angle + 90f;
			}
		}
	}

	private float getSquareCircleOffsetLength(WMG_Node theNode, float angle, bool isFrom)
	{
		if (theNode.isSquare)
		{
			int squareCircleOffsetAngle = getSquareCircleOffsetAngle(angle, isFrom);
			float num = theNode.radius - theNode.radius * Mathf.Cos((float)Math.PI / 180f * (float)squareCircleOffsetAngle);
			float num2 = num * Mathf.Tan((float)Math.PI / 180f * (float)squareCircleOffsetAngle);
			return Mathf.Sqrt(num * num + num2 * num2);
		}
		return 0f;
	}

	private int getSquareCircleOffsetAngle(float angle, bool isFrom)
	{
		int num = 0;
		num = ((!isFrom) ? ((Mathf.RoundToInt(angle) + 90) % 90) : ((Mathf.RoundToInt(angle) - 90) % 90));
		if (Mathf.Abs(num) > 45)
		{
			num = ((num <= 0) ? (num - 2 * (num + 45)) : (num - 2 * (num - 45)));
		}
		return num;
	}
}
