using System;
using UnityEngine;

[Serializable]
public class AssetRearWheel
{
	public Transform BrakeDisk;

	public Transform Axle;

	public Transform ControlArmStart;

	public Transform ControlArmEnd;

	public Transform Dummy;

	public Transform SteeringAxis;

	public Transform ShockUps;

	public Transform ShockDowns;

	public Transform WheelColliderHolder;

	public Transform[] Shocks;
}
