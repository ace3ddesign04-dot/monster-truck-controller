using System;
using UnityEngine;

[Serializable]
public class FordAFrontWheel
{
	public Transform LowerArmStart;

	public Transform LowerArmEnd;

	public Transform UpperArmStart;

	public Transform UpperArmEnd;

	public Transform Knuckle;

	public Transform KnucklePos;

	public Transform TieRodStartBone;

	public Transform TieRodEndBone;

	public Transform BrakeDisk;

	public Transform Dummy;

	public Transform ShockUps;

	public Transform ShockDowns;

	public Transform WheelColliderHolder;

	public Transform[] Shocks;
}
