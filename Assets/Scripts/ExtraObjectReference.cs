using System;
using UnityEngine;

[Serializable]
public class ExtraObjectReference
{
	public int arrayID;

	[Range(0f, 1f)]
	public float density;

	public bool onlyByEdges;

	public ExtraObjectReference DeepCopy()
	{
		ExtraObjectReference extraObjectReference = new ExtraObjectReference();
		extraObjectReference.arrayID = arrayID;
		extraObjectReference.density = density;
		extraObjectReference.onlyByEdges = onlyByEdges;
		return extraObjectReference;
	}
}
