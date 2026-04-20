using UnityEngine;

public class FlagPoint : MonoBehaviour
{
	public int FlagPointID;

	public GameObject CurrentRay;

	private Material CurrentMaterial;

	public Color CurrentColor
	{
		get
		{
			if (CurrentMaterial == null)
			{
				CurrentMaterial = CurrentRay.GetComponent<MeshRenderer>().material;
			}
			return CurrentMaterial.color;
		}
	}

	private void Start()
	{
		CurrentMaterial = CurrentRay.GetComponent<MeshRenderer>().material;
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		UnityEngine.Debug.Log("Collided!");
		if (!(other.transform != null) || !(other.transform.parent != null))
		{
			return;
		}
		VehicleDataManager component = other.transform.parent.GetComponent<VehicleDataManager>();
		PhotonView component2 = other.transform.parent.GetComponent<PhotonView>();
		PhotonTransformView component3 = other.transform.parent.GetComponent<PhotonTransformView>();
		if (component != null && component2.isMine)
		{
			SwitchColor((component.Team != PunTeams.Team.blue) ? Color.red : Color.blue);
			if (component.Team == PunTeams.Team.blue)
			{
				component3.SendFlagCapturedBlue(FlagPointID);
			}
			if (component.Team == PunTeams.Team.red)
			{
				component3.SendFlagCapturedRed(FlagPointID);
			}
		}
	}

	public void SwitchColor(Color color)
	{
		CurrentMaterial.color = color;
	}
}
