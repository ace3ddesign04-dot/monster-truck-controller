using CustomVP;
using System.Collections.Generic;
using UnityEngine;

public class TankTracksController : MonoBehaviour
{
	public Transform MasterWheel;

	public Transform Body;

	public Transform[] littleWheels;

	public Transform SteeringHelper;

	[HideInInspector]
	public WheelComponent wc;

	[HideInInspector]
	public Transform wheelHolder;

	public Material TracksMaterial;

	private List<Material> tracksMats = new List<Material>();

	private float xAngle;

	private void Start()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in componentsInChildren)
		{
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				if (material.name.Contains(TracksMaterial.name))
				{
					tracksMats.Add(material);
				}
			}
		}
	}

	private void FixedUpdate()
	{
		if (!(wc.wheelCollider == null))
		{
			SteeringHelper.localEulerAngles = new Vector3(0f, wc.wheelCollider.steeringAngle, 0f);
			Body.position = MasterWheel.position;
			float steeringAngle = wc.wheelCollider.steeringAngle;
			Vector3 hitPoint = wc.wheelCollider.hitPoint;
			Vector3 position = wc.transform.parent.InverseTransformPoint(hitPoint);
			Vector3 localPosition = wc.transform.localPosition;
			position.x = localPosition.x;
			Vector3 a = wc.transform.parent.TransformPoint(position);
			float b = Vector3.SignedAngle(-wc.transform.up, a - wc.transform.position, wc.transform.right);
			if (wc.IsGrounded)
			{
				xAngle = Mathf.Lerp(xAngle, b, Time.fixedDeltaTime * 5f);
			}
			float num = 0f;
			Vector3 lhs = MasterWheel.right;
			Vector3 localPosition2 = base.transform.localPosition;
			if (localPosition2.x < 0f)
			{
				lhs = -MasterWheel.right;
			}
			Vector3 from = Vector3.Cross(lhs, base.transform.forward);
			float num2 = Vector3.SignedAngle(from, base.transform.up, base.transform.forward);
			num = 0f - num2;
			Body.localEulerAngles = new Vector3(xAngle, steeringAngle, num);
			Transform[] array = littleWheels;
			foreach (Transform transform in array)
			{
				transform.Rotate((0f - wc.rpm) * 0.1f, 0f, 0f);
			}
			foreach (Material tracksMat in tracksMats)
			{
				Vector2 textureOffset = tracksMat.GetTextureOffset("_DecalLayer1");
				textureOffset += new Vector2(0f, wc.rpm * 0.0005f);
				tracksMat.SetTextureOffset("_DecalLayer1", textureOffset);
				tracksMat.SetTextureOffset("_DirtLayer1", textureOffset);
			}
		}
	}
}
