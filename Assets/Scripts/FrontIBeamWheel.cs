using System;
using UnityEngine;

[Serializable]
public class FrontIBeamWheel
{
	public Transform TieRodStartBone;

	public Transform TieRodEndBone;

	public Transform TrailingArm;

	public Transform TrailingArmTarget;

	public Transform TrailingArmMountBone;

	public Transform PerchBone;

	public Transform Dummy;

	public Transform Arm;

	public Transform Knuckle;

	public Transform BrakeDisk;

	public Transform SteeringBrace;

	public Transform CamberMeasurer;

	public Transform FrameBone;

	public Transform ShockUps;

	public Transform ShockDowns;

	public Transform WheelColliderHolder;

	public Transform[] Shocks;

	public float RealCamber;

	[HideInInspector]
	public Vector3 DummyDefPos;
}
