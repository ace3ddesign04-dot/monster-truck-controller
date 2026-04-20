using System;
using System.Collections.Generic;
using UnityEngine;

public class WMG_Random_Graph : WMG_Graph_Manager
{
	public UnityEngine.Object nodePrefab;

	public UnityEngine.Object linkPrefab;

	public int numNodes;

	public float minAngle;

	public float minAngleRange;

	public float maxAngleRange;

	public int minRandomNumberNeighbors;

	public int maxRandomNumberNeighbors;

	public float minRandomLinkLength;

	public float maxRandomLinkLength;

	public bool centerPropogate;

	public bool noLinkIntersection;

	public bool noNodeIntersection;

	public float noNodeIntersectionRadiusPadding;

	public int maxNeighborAttempts;

	public bool noLinkNodeIntersection;

	public float noLinkNodeIntersectionRadiusPadding;

	public bool createOnStart;

	public bool debugRandomGraph;

	private void Awake()
	{
		if (createOnStart)
		{
			GenerateGraph();
		}
	}

	public List<GameObject> GenerateGraph()
	{
		GameObject gameObject = CreateNode(nodePrefab, null);
		WMG_Node component = gameObject.GetComponent<WMG_Node>();
		return GenerateGraphFromNode(component);
	}

	public List<GameObject> GenerateGraphFromNode(WMG_Node fromNode)
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(fromNode.gameObject);
		GameObject[] array = new GameObject[numNodes];
		bool[] array2 = new bool[numNodes];
		GameObject gameObject = fromNode.gameObject;
		int num = 0;
		int num2 = 0;
		int num3 = base.NodesParent.Count - 1;
		array[num] = gameObject;
		while (base.NodesParent.Count - num3 < numNodes)
		{
			WMG_Node component = array[num].GetComponent<WMG_Node>();
			int num4 = UnityEngine.Random.Range(minRandomNumberNeighbors, maxRandomNumberNeighbors);
			if (debugRandomGraph)
			{
				UnityEngine.Debug.Log("Processesing Node: " + component.id + " with " + num4 + " neighbors.");
			}
			for (int i = 0; i < num4; i++)
			{
				int num5 = 0;
				while (num5 < maxNeighborAttempts)
				{
					float num6 = UnityEngine.Random.Range(minAngleRange, maxAngleRange);
					float num7 = UnityEngine.Random.Range(minRandomLinkLength, maxRandomLinkLength);
					bool flag = false;
					if (debugRandomGraph)
					{
						UnityEngine.Debug.Log("Neighbor: " + i + " Attempt: " + num5 + " angle: " + Mathf.Round(num6));
					}
					if (minAngle > 0f)
					{
						for (int j = 0; j < component.numLinks; j++)
						{
							float num8 = Mathf.Abs(component.linkAngles[j] - num6);
							if (num8 > 180f)
							{
								num8 = Mathf.Abs(num8 - 360f);
							}
							if (num8 < minAngle)
							{
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						if (debugRandomGraph)
						{
							UnityEngine.Debug.Log("Failed: Angle within minAngle of existing neighbor");
						}
						num5++;
						continue;
					}
					if (noLinkIntersection)
					{
						Vector3 localPosition = component.transform.localPosition;
						float p1y = localPosition.y + (num7 + component.radius) * Mathf.Sin((float)Math.PI / 180f * num6);
						Vector3 localPosition2 = component.transform.localPosition;
						float p1x = localPosition2.x + (num7 + component.radius) * Mathf.Cos((float)Math.PI / 180f * num6);
						Vector3 localPosition3 = component.transform.localPosition;
						float p2y = localPosition3.y + component.radius * Mathf.Sin((float)Math.PI / 180f * num6);
						Vector3 localPosition4 = component.transform.localPosition;
						float p2x = localPosition4.x + component.radius * Mathf.Cos((float)Math.PI / 180f * num6);
						foreach (GameObject item in base.LinksParent)
						{
							WMG_Link component2 = item.GetComponent<WMG_Link>();
							if (component2.id != -1)
							{
								WMG_Node component3 = component2.fromNode.GetComponent<WMG_Node>();
								WMG_Node component4 = component2.toNode.GetComponent<WMG_Node>();
								Vector3 localPosition5 = component3.transform.localPosition;
								float y = localPosition5.y;
								Vector3 localPosition6 = component3.transform.localPosition;
								float x = localPosition6.x;
								Vector3 localPosition7 = component4.transform.localPosition;
								float y2 = localPosition7.y;
								Vector3 localPosition8 = component4.transform.localPosition;
								float x2 = localPosition8.x;
								if (WMG_Util.LineSegmentsIntersect(p1x, p1y, p2x, p2y, x, y, x2, y2))
								{
									if (debugRandomGraph)
									{
										UnityEngine.Debug.Log("Failed: Link intersected with existing link: " + component2.id);
									}
									flag = true;
									break;
								}
							}
						}
					}
					if (flag)
					{
						num5++;
						continue;
					}
					if (noNodeIntersection)
					{
						Vector3 localPosition9 = component.transform.localPosition;
						float num9 = localPosition9.y + num7 * Mathf.Sin((float)Math.PI / 180f * num6);
						Vector3 localPosition10 = component.transform.localPosition;
						float num10 = localPosition10.x + num7 * Mathf.Cos((float)Math.PI / 180f * num6);
						foreach (GameObject item2 in base.NodesParent)
						{
							WMG_Node component5 = item2.GetComponent<WMG_Node>();
							if (component5.id != -1)
							{
								float num11 = num10;
								Vector3 localPosition11 = item2.transform.localPosition;
								float num12 = Mathf.Pow(num11 - localPosition11.x, 2f);
								float num13 = num9;
								Vector3 localPosition12 = item2.transform.localPosition;
								if (num12 + Mathf.Pow(num13 - localPosition12.y, 2f) <= Mathf.Pow(2f * (component.radius + noNodeIntersectionRadiusPadding), 2f))
								{
									if (debugRandomGraph)
									{
										UnityEngine.Debug.Log("Failed: Node intersected with existing node: " + component5.id);
									}
									flag = true;
									break;
								}
							}
						}
					}
					if (flag)
					{
						num5++;
						continue;
					}
					if (noLinkNodeIntersection)
					{
						Vector3 localPosition13 = component.transform.localPosition;
						float num14 = localPosition13.y + (num7 + component.radius) * Mathf.Sin((float)Math.PI / 180f * num6);
						Vector3 localPosition14 = component.transform.localPosition;
						float num15 = localPosition14.x + (num7 + component.radius) * Mathf.Cos((float)Math.PI / 180f * num6);
						Vector3 localPosition15 = component.transform.localPosition;
						float num16 = localPosition15.y + component.radius * Mathf.Sin((float)Math.PI / 180f * num6);
						Vector3 localPosition16 = component.transform.localPosition;
						float num17 = localPosition16.x + component.radius * Mathf.Cos((float)Math.PI / 180f * num6);
						foreach (GameObject item3 in base.NodesParent)
						{
							WMG_Node component6 = item3.GetComponent<WMG_Node>();
							if (component.id != component6.id)
							{
								float x3 = num15;
								float y3 = num14;
								float x4 = num17;
								float y4 = num16;
								Vector3 localPosition17 = item3.transform.localPosition;
								float x5 = localPosition17.x;
								Vector3 localPosition18 = item3.transform.localPosition;
								if (WMG_Util.LineIntersectsCircle(x3, y3, x4, y4, x5, localPosition18.y, component6.radius + noLinkNodeIntersectionRadiusPadding))
								{
									if (debugRandomGraph)
									{
										UnityEngine.Debug.Log("Failed: Link intersected with existing node: " + component6.id);
									}
									flag = true;
									break;
								}
							}
						}
					}
					if (flag)
					{
						num5++;
						continue;
					}
					if (noLinkNodeIntersection)
					{
						Vector3 localPosition19 = component.transform.localPosition;
						float y5 = localPosition19.y + (num7 + 2f * component.radius) * Mathf.Sin((float)Math.PI / 180f * num6);
						Vector3 localPosition20 = component.transform.localPosition;
						float x6 = localPosition20.x + (num7 + 2f * component.radius) * Mathf.Cos((float)Math.PI / 180f * num6);
						foreach (GameObject item4 in base.LinksParent)
						{
							WMG_Link component7 = item4.GetComponent<WMG_Link>();
							if (component7.id != -1)
							{
								WMG_Node component8 = component7.fromNode.GetComponent<WMG_Node>();
								WMG_Node component9 = component7.toNode.GetComponent<WMG_Node>();
								Vector3 localPosition21 = component8.transform.localPosition;
								float y6 = localPosition21.y;
								Vector3 localPosition22 = component8.transform.localPosition;
								float x7 = localPosition22.x;
								Vector3 localPosition23 = component9.transform.localPosition;
								float y7 = localPosition23.y;
								Vector3 localPosition24 = component9.transform.localPosition;
								float x8 = localPosition24.x;
								if (WMG_Util.LineIntersectsCircle(x7, y6, x8, y7, x6, y5, component.radius + noLinkNodeIntersectionRadiusPadding))
								{
									if (debugRandomGraph)
									{
										UnityEngine.Debug.Log("Failed: Node intersected with existing link: " + component7.id);
									}
									flag = true;
									break;
								}
							}
						}
					}
					if (flag)
					{
						num5++;
						continue;
					}
					gameObject = CreateNode(nodePrefab, fromNode.transform.parent.gameObject);
					list.Add(gameObject);
					array[base.NodesParent.Count - num3 - 1] = gameObject;
					float num18 = Mathf.Cos((float)Math.PI / 180f * num6) * num7;
					float num19 = Mathf.Sin((float)Math.PI / 180f * num6) * num7;
					Transform transform = gameObject.transform;
					Vector3 localPosition25 = component.transform.localPosition;
					float x9 = localPosition25.x + num18;
					Vector3 localPosition26 = component.transform.localPosition;
					transform.localPosition = new Vector3(x9, localPosition26.y + num19, 0f);
					list.Add(CreateLink(component, gameObject, linkPrefab, null));
					break;
				}
				if (base.NodesParent.Count - num3 == numNodes)
				{
					break;
				}
			}
			array2[num] = true;
			num2++;
			if (centerPropogate)
			{
				num++;
			}
			else
			{
				int num20 = base.NodesParent.Count - num3 - num2;
				if (num20 > 0)
				{
					int[] array3 = new int[num20];
					int num21 = 0;
					for (int k = 0; k < numNodes; k++)
					{
						if (!array2[k] && k < base.NodesParent.Count - num3)
						{
							array3[num21] = k;
							num21++;
						}
					}
					num = array3[UnityEngine.Random.Range(0, num21 - 1)];
				}
			}
			if (base.NodesParent.Count - num3 == num2)
			{
				UnityEngine.Debug.Log("WMG - Warning: Only generated " + (base.NodesParent.Count - num3 - 1) + " nodes with the given parameters.");
				break;
			}
		}
		return list;
	}
}
