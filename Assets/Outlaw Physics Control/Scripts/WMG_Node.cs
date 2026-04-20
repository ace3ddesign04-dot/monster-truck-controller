using System;
using System.Collections.Generic;
using UnityEngine;

public class WMG_Node : WMG_GUI_Functions
{
	public int id;

	public float radius;

	public bool isSquare;

	public int numLinks;

	public List<GameObject> links = new List<GameObject>();

	public List<float> linkAngles = new List<float>();

	public GameObject objectToScale;

	public GameObject objectToColor;

	public GameObject objectToLabel;

	public bool isSelected;

	public bool wasSelected;

	public bool BFS_mark;

	public int BFS_depth;

	public float Dijkstra_depth;

	public WMG_Series seriesRef;

	public GameObject CreateLink(GameObject target, UnityEngine.Object prefabLink, int linkId, GameObject parent, bool repos)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(prefabLink) as GameObject;
		Vector3 localPosition = gameObject.transform.localPosition;
		GameObject parent2 = parent;
		if (parent == null)
		{
			parent2 = target.transform.parent.gameObject;
		}
		changeSpriteParent(gameObject, parent2);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = localPosition;
		WMG_Link component = gameObject.GetComponent<WMG_Link>();
		links.Add(gameObject);
		linkAngles.Add(0f);
		WMG_Node component2 = target.GetComponent<WMG_Node>();
		component2.links.Add(gameObject);
		component2.linkAngles.Add(0f);
		component2.numLinks++;
		numLinks++;
		component.Setup(base.gameObject, target, linkId, repos);
		return gameObject;
	}

	public void Reposition(float x, float y)
	{
		changeSpritePositionTo(base.gameObject, new Vector3(x, y, 1f));
		for (int i = 0; i < numLinks; i++)
		{
			WMG_Link component = links[i].GetComponent<WMG_Link>();
			component.Reposition();
		}
	}

	public void SetID(int newID)
	{
		id = newID;
		base.name = "WMG_Node_" + id;
	}

	public void RepositionRelativeToNode(WMG_Node fromNode, bool fixAngle, int degreeStep, float lengthStep)
	{
		Vector3 localPosition = base.transform.localPosition;
		float x = localPosition.x;
		Vector3 localPosition2 = fromNode.transform.localPosition;
		float num = x - localPosition2.x;
		Vector3 localPosition3 = base.transform.localPosition;
		float y = localPosition3.y;
		Vector3 localPosition4 = fromNode.transform.localPosition;
		float num2 = y - localPosition4.y;
		float num3 = Mathf.Atan2(num2, num) * 57.29578f;
		if (num3 < 0f)
		{
			num3 += 360f;
		}
		float num4 = Mathf.Sqrt(Mathf.Pow(num2, 2f) + Mathf.Pow(num, 2f));
		if (num4 < 0f)
		{
			num4 = 0f;
		}
		float num5 = num3;
		if (fixAngle)
		{
			num5 = 0f;
			for (int i = 0; i < 360 / degreeStep; i++)
			{
				if (num3 >= (float)(i * degreeStep) - 0.5f * (float)degreeStep && num3 < (float)((i + 1) * degreeStep) - 0.5f * (float)degreeStep)
				{
					num5 = i * degreeStep;
				}
			}
		}
		else
		{
			float num6 = num4 % lengthStep;
			num4 -= num6;
			if (lengthStep - num6 < lengthStep / 2f)
			{
				num4 += lengthStep;
			}
		}
		Transform transform = base.transform;
		Vector3 localPosition5 = fromNode.transform.localPosition;
		float x2 = localPosition5.x + num4 * Mathf.Cos((float)Math.PI / 180f * num5);
		Vector3 localPosition6 = fromNode.transform.localPosition;
		float y2 = localPosition6.y + num4 * Mathf.Sin((float)Math.PI / 180f * num5);
		Vector3 localPosition7 = base.transform.localPosition;
		transform.localPosition = new Vector3(x2, y2, localPosition7.z);
		for (int j = 0; j < numLinks; j++)
		{
			WMG_Link component = links[j].GetComponent<WMG_Link>();
			component.Reposition();
		}
	}
}
