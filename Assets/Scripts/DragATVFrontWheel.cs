using System;
using UnityEngine;

[Serializable]
public class DragATVFrontWheel
{
	public Transform BrakeDisk;

	public Transform Knuckle;

	public Transform Dummy;

	public Transform UpperArm;

	public Transform LowerArm;

	public Transform UpperArmTarget;

	public Transform Perch;

	public Transform Frame;

	public Transform TieRod;

	public Transform ShockUps;

	public Transform ShockDowns;

	public Transform KnucklePosition;

	public Transform WheelColliderHolder;

	public Transform[] Shocks;

	[HideInInspector]
	public float Deviation;

	[HideInInspector]
	public Vector3 KnuckleDefPos;
}
