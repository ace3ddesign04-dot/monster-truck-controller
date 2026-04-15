using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class RammingChecker : MonoBehaviour
{
	[Serializable]
	public class RammingVehicle
	{
		public PhotonView pView;

		public int RammingCount;

		public float TimeSinceLastRamming;

		public string playerName;

		public bool AllowCollisions;

		public RammingVehicle(PhotonView view)
		{
			pView = view;
			TimeSinceLastRamming = 0f;
			RammingCount = 1;
			playerName = view.owner.CustomProperties["DisplayName"].ToString();
		}
	}

	public List<RammingVehicle> rammingVehicles;

	private RammingVehicle currentRammingVehicle;

	private bool ShowingRammingMessage;

	private PhotonTransformView tView;

	private void Start()
	{
		tView = GetComponent<PhotonTransformView>();
		if (!base.gameObject.GetPhotonView().isMine || !PhotonNetwork.inRoom)
		{
			base.enabled = false;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!base.enabled || collision.collider.gameObject.layer != 26 || ShowingRammingMessage || Vector3.Angle(collision.gameObject.transform.position - base.transform.position, base.transform.forward) < 60f)
		{
			return;
		}
		PhotonView view = collision.gameObject.GetPhotonView();
		if (view == null)
		{
			return;
		}
		RammingVehicle rammingVehicle = rammingVehicles.Find((RammingVehicle rv) => rv.pView.Equals(view));
		if (rammingVehicle == null || !rammingVehicle.AllowCollisions)
		{
			if (rammingVehicle == null)
			{
				rammingVehicles.Add(new RammingVehicle(collision.gameObject.GetPhotonView()));
			}
			else if (rammingVehicle.RammingCount == 10)
			{
				currentRammingVehicle = rammingVehicle;
				ShowBlockCollisionMessage();
			}
			else if (rammingVehicle.TimeSinceLastRamming > 1f)
			{
				rammingVehicle.RammingCount++;
				rammingVehicle.TimeSinceLastRamming = 0f;
			}
		}
	}

	private void Update()
	{
		for (int i = 0; i < rammingVehicles.Count; i++)
		{
			rammingVehicles[i].TimeSinceLastRamming += Time.deltaTime;
			if (rammingVehicles[i].TimeSinceLastRamming > 60f)
			{
				rammingVehicles.RemoveAt(i);
			}
		}
		if (CrossPlatformInputManager.GetButtonDown("BlockCollisions"))
		{
			BlockCollisions();
		}
		if (CrossPlatformInputManager.GetButtonDown("AllowCollisions"))
		{
			AllowCollisions();
		}
	}

	private void ShowBlockCollisionMessage()
	{
		ShowingRammingMessage = true;
		CarUIControl.Instance.ShowRammingWindow(currentRammingVehicle.playerName);
	}

	private void BlockCollisions()
	{
		UnityEngine.Debug.Log("Blocking collisions");
		UnityEngine.Debug.Log(currentRammingVehicle.playerName);
		Collider[] componentsInChildren = currentRammingVehicle.pView.gameObject.GetComponentsInChildren<Collider>();
		foreach (Collider collider in componentsInChildren)
		{
			collider.enabled = false;
		}
		tView.SendDisableMyCollidersEvent(currentRammingVehicle.pView.owner);
		rammingVehicles.Remove(currentRammingVehicle);
		currentRammingVehicle = null;
	}

	private void AllowCollisions()
	{
		UnityEngine.Debug.Log("Allowing collisions");
		currentRammingVehicle.AllowCollisions = true;
		currentRammingVehicle = null;
	}
}
