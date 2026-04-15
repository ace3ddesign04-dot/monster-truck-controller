using System;
using UnityEngine;

[Serializable]
public class IndependentFrontWheel
{
	public Transform UpperWishbone;

	public Transform UpperWishboneTarget;

	public Transform LowerWishbone;

	public Transform Knuckle;

	public Transform KnucklePos;

	public Transform BrakeDisk;

	public Transform OuterDriveshaftStartBone;

	public Transform OuterDriveshaftEndBone;

	public Transform InnerDriveshaftStartBone;

	public Transform InnerDriveshaftEndBone;

	public Transform PerchBone;

	public Transform TieRodStart;

	public Transform TieRodEnd;

	public Transform Dummy;

	public Transform Frame;

	public Transform ShockUps;

	public Transform ShockDowns;

	public Transform WheelCollidersHolder;

	public Transform[] Shocks;

	public float KnuckleHeight;

	public float HeightCorrectionRatio;

	[HideInInspector]
	public Vector3 DefBrakeDiskPosition;

	[HideInInspector]
	public float Deviation;
}
