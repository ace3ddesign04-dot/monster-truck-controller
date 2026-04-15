using System;
using UnityEngine;

[Serializable]
public class RearTrophyWheel
{
	public Transform BrakeDisk;

	public Transform TrackBarStartBone;

	public Transform TrackBarEndBone;

	public Transform TrailingArm;

	public Transform TrailingArmTarget;

	public Transform TrailingArmMount;

	public Transform Dummy;

	public Transform Helper;

	public Transform SteeringAxle;

	public Transform ShockUps;

	public Transform ShockDowns;

	public Transform WheelColliderHolder;

	public Transform[] Shocks;

	[HideInInspector]
	public Vector3 DummyDefPos;

	[HideInInspector]
	public float Deviation;
}
