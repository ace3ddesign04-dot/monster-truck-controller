using CustomVP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRigidbodyView : MonoBehaviour, IPunObservable
{
	[HideInInspector]
	public Vector3 Velocity;

	[HideInInspector]
	public Vector3 AngularVelocity;

	private Rigidbody rb;

	private CarController carController;

	private void Awake()
	{
		if (!PhotonNetwork.inRoom || SceneManager.GetActiveScene().name.ToLower() == "menu")
		{
			base.enabled = false;
		}
	}

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		carController = GetComponent<CarController>();
		Invoke("CheckForces", 10f);
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(rb.velocity);
			stream.SendNext(rb.angularVelocity);
		}
		else if (stream.isReading)
		{
			if (stream.PeekNext() != null)
			{
				Velocity = (Vector3)stream.ReceiveNext();
			}
			if (stream.PeekNext() != null)
			{
				AngularVelocity = (Vector3)stream.ReceiveNext();
			}
		}
	}

	private void Update()
	{
		if (rb == null)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	private void CheckForces()
	{
		if (Velocity == Vector3.zero && AngularVelocity == Vector3.zero && carController == null)
		{
			SwitchToOldApproach();
		}
	}

	private void SwitchToOldApproach()
	{
		UnityEngine.Object.Destroy(GetComponent<Rigidbody>());
		UnityEngine.Object.Destroy(this);
	}
}
