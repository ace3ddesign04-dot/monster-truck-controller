using UnityEngine;

public class PlayerRouteCheckpoint : MonoBehaviour
{
	public int checkpointID;

	public GameObject checkpointTapTarget;

	public void ToggleTapTarget(bool on)
	{
		checkpointTapTarget.SetActive(on);
	}
}
