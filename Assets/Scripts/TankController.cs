using CustomVP;
using UnityEngine;

public class TankController : MonoBehaviour
{
	public Transform COM;

	public WheelComponent[] LeftWheelColliders;

	public WheelComponent[] RightWheelColliders;

	public Transform[] LeftWheels;

	public Transform[] RightWheels;

	public Transform FLDriveWheel;

	public Transform FRDriveWheel;

	public Transform RLDriveWheel;

	public Transform RRDriveWheel;

	public Transform[] LeftTrackBones;

	public Transform[] RightTrackBones;

	public float torque;

	public float RollingStopTorque;

	private float xInput;

	private float yInput;

	private float smoothYinput;

	public Material TankTracksMaterial;

	public Renderer leftTracks;

	public Renderer rightTracks;

	private Material _leftmaterial;

	private Material _rightmaterial;

	private void Start()
	{
		GetComponent<Rigidbody>().centerOfMass = base.transform.InverseTransformPoint(COM.position);
		_leftmaterial = leftTracks.materials[0];
		_rightmaterial = rightTracks.materials[0];
	}

	private void Update()
	{
		xInput = UnityEngine.Input.GetAxis("Horizontal");
		yInput = UnityEngine.Input.GetAxis("Vertical");
		if (xInput != 0f)
		{
			yInput = 0f;
		}
		smoothYinput = Mathf.MoveTowards(smoothYinput, yInput, Time.deltaTime);
		if (Mathf.Abs(yInput) < Mathf.Abs(smoothYinput))
		{
			smoothYinput = yInput;
		}
		float value = torque * (smoothYinput + xInput * 2f);
		value = Mathf.Clamp(value, 0f - torque, torque);
		float value2 = torque * (smoothYinput - xInput * 2f);
		value2 = Mathf.Clamp(value2, 0f - torque, torque);
		WheelComponent[] leftWheelColliders = LeftWheelColliders;
		foreach (WheelComponent wheelComponent in leftWheelColliders)
		{
			wheelComponent.MotorTorque = value;
			if (value == 0f || xInput != 0f)
			{
				wheelComponent.BrakeTorque = RollingStopTorque;
			}
			else
			{
				wheelComponent.BrakeTorque = 0f;
			}
			wheelComponent.wheelCollider.rpmLimit = 30f;
		}
		WheelComponent[] rightWheelColliders = RightWheelColliders;
		foreach (WheelComponent wheelComponent2 in rightWheelColliders)
		{
			wheelComponent2.MotorTorque = value2;
			if (value2 == 0f || xInput != 0f)
			{
				wheelComponent2.BrakeTorque = RollingStopTorque;
			}
			else
			{
				wheelComponent2.BrakeTorque = 0f;
			}
			wheelComponent2.wheelCollider.rpmLimit = 30f;
		}
		FLDriveWheel.Rotate((0f - LeftWheelColliders[2].rpm) * 0.05f, 0f, 0f);
		RLDriveWheel.Rotate((0f - LeftWheelColliders[2].rpm) * 0.05f, 0f, 0f);
		for (int k = 0; k < LeftWheels.Length; k++)
		{
			LeftWheels[k].Rotate((0f - LeftWheelColliders[2].rpm) * 0.05f, 0f, 0f);
		}
		FRDriveWheel.Rotate((0f - RightWheelColliders[2].rpm) * 0.05f, 0f, 0f);
		RRDriveWheel.Rotate((0f - RightWheelColliders[2].rpm) * 0.05f, 0f, 0f);
		for (int l = 0; l < RightWheels.Length; l++)
		{
			RightWheels[l].Rotate((0f - RightWheelColliders[2].rpm) * 0.05f, 0f, 0f);
		}
		for (int m = 0; m < LeftTrackBones.Length; m++)
		{
			Vector3 localPosition = LeftTrackBones[m].localPosition;
			Vector3 vector = LeftTrackBones[m].parent.InverseTransformPoint(LeftWheelColliders[m].GetVisualWheelPosition());
			localPosition.z = vector.z;
			LeftTrackBones[m].localPosition = localPosition;
		}
		for (int n = 0; n < LeftWheels.Length; n++)
		{
			Vector3 localPosition2 = LeftWheels[n].localPosition;
			Vector3 vector2 = LeftWheels[n].parent.InverseTransformPoint(LeftWheelColliders[n].GetVisualWheelPosition());
			localPosition2.z = vector2.z;
			LeftWheels[n].localPosition = localPosition2;
		}
		for (int num = 0; num < RightTrackBones.Length; num++)
		{
			Vector3 localPosition3 = RightTrackBones[num].localPosition;
			Vector3 vector3 = RightTrackBones[num].parent.InverseTransformPoint(RightWheelColliders[num].GetVisualWheelPosition());
			localPosition3.z = vector3.z;
			RightTrackBones[num].localPosition = localPosition3;
		}
		for (int num2 = 0; num2 < RightWheels.Length; num2++)
		{
			Vector3 localPosition4 = RightWheels[num2].localPosition;
			Vector3 vector4 = RightWheels[num2].parent.InverseTransformPoint(RightWheelColliders[num2].GetVisualWheelPosition());
			localPosition4.z = vector4.z;
			RightWheels[num2].localPosition = localPosition4;
		}
		_leftmaterial.mainTextureOffset += new Vector2(0f, LeftWheelColliders[2].rpm * 0.0002f);
		_rightmaterial.mainTextureOffset += new Vector2(0f, RightWheelColliders[2].rpm * 0.0002f);
	}
}
