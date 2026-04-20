using System;
using UnityEngine;

[Serializable]
public class TrophyFrontWheel
{
	public Transform LowerWishbone;

	public Transform UpperWishbone;

	public Transform UpperWishboneTarget;

	public Transform Knuckle;

	public Transform KnucklePos;

	public Transform TieRodStartBone;

	public Transform TieRodEndBone;

	public Transform BrakeDisk;

	public Transform FrameBone;

	public Transform PerchBone;

	public Transform Dummy;

	public Transform ShockUps;

	public Transform ShockDowns;

	public Transform WheelColliderHolder;

	public Transform[] Shocks;

	[HideInInspector]
	public Vector3 DefBrakeDiskPosition;

	[HideInInspector]
	public float Deviation;

	public float HeightCorrectionRatio;
}
