using CustomVP;
using Photon;
using UnityEngine;

public class NetworkMovement : Photon.MonoBehaviour
{
	[SerializeField]
	protected Transform target;

	[Header("Setup")]
	[Range(0f, 10f)]
	public int SendRate = 2;

	[Range(0f, 2f)]
	public float movementThreshold = 0.2f;

	[Range(0f, 30f)]
	public float angleThreshold = 5f;

	[Range(0f, 10f)]
	public float distanceBeforeSnap = 4f;

	[Range(0f, 90f)]
	public float angleBeforeSnap = 40f;

	[Header("Interpolation")]
	[Range(0f, 1f)]
	public float movementInterpolation = 0.1f;

	[Range(0f, 1f)]
	public float rotationInterpolation = 0.1f;

	public float thresholdMovementPrediction = 0.7f;

	public float thresholdRotationPrediction = 15f;

	protected Vector3 lastDirectionPerFrame = Vector3.zero;

	protected Vector3 lastPositionSent = Vector3.zero;

	protected Quaternion lastRotationSent = Quaternion.identity;

	protected Quaternion lastRotationDirectionPerFrame = Quaternion.identity;

	protected float lastSteeringAngle;

	protected bool send;

	protected bool sending;

	protected int count;

	private CarController carController;

	private SuspensionController suspensionController;

	private BodyPartsSwitcher partsSwitcher;

	private void Start()
	{
		target = base.transform;
		carController = GetComponent<CarController>();
		suspensionController = GetComponent<SuspensionController>();
		partsSwitcher = GetComponent<BodyPartsSwitcher>();
		if (!base.photonView.isMine)
		{
			if (suspensionController != null)
			{
				suspensionController.TurnToMultiplayerCar();
			}
			if (carController != null)
			{
				carController.enabled = false;
			}
		}
	}

	private void FixedUpdate()
	{
		if (base.photonView.isMine)
		{
			sendInfo();
		}
		else
		{
			recontiliation();
		}
	}

	protected void sendInfo()
	{
		if (send)
		{
			if (count == SendRate)
			{
				count = 0;
				send = false;
				Vector3 position = target.position;
				Quaternion rotation = target.rotation;
				float dirtiness = 0f;
				float wetness = 0f;
				float steeringAngle = 0f;
				if (partsSwitcher != null)
				{
					dirtiness = partsSwitcher.Dirtiness;
					wetness = partsSwitcher.MudWetness;
				}
				if (carController != null)
				{
					steeringAngle = carController.Steering;
				}
				CmdSendPosition(position, rotation, steeringAngle, dirtiness, wetness);
			}
			else
			{
				count++;
			}
		}
		else
		{
			checkIfSend();
		}
	}

	protected void checkIfSend()
	{
		if (sending)
		{
			send = true;
			sending = false;
			return;
		}
		Vector3 position = target.position;
		Quaternion rotation = target.rotation;
		float num = Vector3.Distance(lastPositionSent, position);
		float num2 = Quaternion.Angle(lastRotationSent, rotation);
		float num3 = 0f;
		if (carController != null)
		{
			num3 = carController.Steering;
		}
		send = true;
		sending = true;
	}

	protected void recontiliation()
	{
		Vector3 position = target.position;
		Quaternion rotation = target.rotation;
		float num = Vector3.Distance(lastPositionSent, position);
		float num2 = Vector3.Angle(lastRotationSent.eulerAngles, rotation.eulerAngles);
		if (num > distanceBeforeSnap)
		{
			target.position = lastPositionSent;
		}
		if (num2 > angleBeforeSnap)
		{
			target.rotation = lastRotationSent;
		}
		position += lastDirectionPerFrame;
		rotation *= lastRotationDirectionPerFrame;
		Vector3 position2 = Vector3.Lerp(position, lastPositionSent, movementInterpolation);
		Quaternion rotation2 = Quaternion.Lerp(rotation, lastRotationSent, rotationInterpolation);
		target.position = position2;
		target.rotation = rotation2;
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!(target == null))
		{
			if (stream.isWriting)
			{
				Vector3 obj = target.position;
				Quaternion obj2 = target.rotation;
				stream.Serialize(ref obj);
				stream.Serialize(ref obj2);
			}
			else
			{
				Vector3 obj3 = Vector3.zero;
				Quaternion obj4 = Quaternion.identity;
				stream.Serialize(ref obj3);
				stream.Serialize(ref obj4);
				target.position = obj3;
				target.rotation = obj4;
			}
		}
	}

	protected void CmdSendPosition(Vector3 newPos, Quaternion newRot, float steeringAngle, float dirtiness, float wetness)
	{
		RpcReceivePosition(newPos, newRot, steeringAngle, dirtiness, wetness);
	}

	protected void RpcReceivePosition(Vector3 newPos, Quaternion newRot, float steeringAngle, float dirtiness, float wetness)
	{
		int num = SendRate + 1;
		lastDirectionPerFrame = newPos - lastPositionSent;
		lastDirectionPerFrame /= (float)num;
		if (lastDirectionPerFrame.magnitude > thresholdMovementPrediction)
		{
			lastDirectionPerFrame = Vector3.zero;
		}
		Vector3 eulerAngles = lastRotationSent.eulerAngles;
		Vector3 eulerAngles2 = newRot.eulerAngles;
		if (Quaternion.Angle(lastRotationDirectionPerFrame, newRot) < thresholdRotationPrediction)
		{
			lastRotationDirectionPerFrame = Quaternion.Euler((eulerAngles2 - eulerAngles) / num);
		}
		else
		{
			lastRotationDirectionPerFrame = Quaternion.identity;
		}
		lastPositionSent = newPos;
		lastRotationSent = newRot;
		lastSteeringAngle = steeringAngle;
		if (partsSwitcher != null)
		{
			partsSwitcher.Dirtiness = dirtiness;
			partsSwitcher.MudWetness = wetness;
			partsSwitcher.UpdateDirtiness();
		}
	}
}
