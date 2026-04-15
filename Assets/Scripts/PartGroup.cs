using System;
using UnityEngine;

[Serializable]
public class PartGroup
{
	public string GroupName;

	public PartType partType;

	public GameObject[] Parts;

	public bool Paintable;

	public Color color;

	public bool HideRearFenders;

	[HideInInspector]
	public int InstalledPart;

	public PartGroupData returnData()
	{
		PartGroupData partGroupData = new PartGroupData();
		partGroupData.GroupName = GroupName;
		partGroupData.InstalledPart = InstalledPart;
		partGroupData.color = color;
		return partGroupData;
	}

	public PartGroup DeepCopy()
	{
		PartGroup partGroup = new PartGroup();
		partGroup.GroupName = GroupName;
		partGroup.partType = partType;
		partGroup.InstalledPart = InstalledPart;
		partGroup.color = color;
		partGroup.Parts = new GameObject[Parts.Length];
		for (int i = 0; i < Parts.Length; i++)
		{
			partGroup.Parts[i] = Parts[i];
		}
		return partGroup;
	}

	public void PaintPart()
	{
		if (!Paintable || color == Color.clear)
		{
			return;
		}
		GameObject[] parts = Parts;
		foreach (GameObject gameObject in parts)
		{
			if (!(gameObject != null))
			{
				continue;
			}
			MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
			if (componentsInChildren.Length <= 0)
			{
				continue;
			}
			MeshRenderer[] array = componentsInChildren;
			foreach (MeshRenderer meshRenderer in array)
			{
				Material[] materials = meshRenderer.materials;
				foreach (Material material in materials)
				{
					material.SetColor("_BaseColor", color);
				}
			}
		}
	}
}
