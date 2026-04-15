using CustomVP;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class LightsController : MonoBehaviour
{
	private PhotonTransformView photonTransformView;

	private CarController carController;

	private Camera mainCamera;

	public LensFlare[] HeadLights;

	public LensFlare[] BrakeLights;

	public LensFlare[] RoofLights;

	public LensFlare[] PoliceLights;

	public LensFlare FLTurnLight;

	public LensFlare FRTurnLight;

	public LensFlare RLTurnLight;

	public LensFlare RRTurnLight;

	public SpriteRenderer[] LightBars;

	private float MasterBrightnessMultiplier;

	private float FrontVisiblityMultiplier;

	private float RearVisiblityMultiplier;

	public float HeadlightsBrightness = 5f;

	public float BrakeBrightness = 5f;

	public float RooflightsBrightness = 5f;

	public float TurnlightsBrightness = 5f;

	public bool LightsOn;

	private float minSize;

	public float LightsState
	{
		get
		{
			return LightsOn ? 1 : 0;
		}
		set
		{
			LightsOn = (value == 1f);
		}
	}

	[ContextMenu("Shut all lights")]
	private void Shut()
	{
		LensFlare[] componentsInChildren = GetComponentsInChildren<LensFlare>(includeInactive: true);
		foreach (LensFlare lensFlare in componentsInChildren)
		{
			lensFlare.brightness = 0f;
		}
	}

	private void Start()
	{
		mainCamera = Camera.main;
		photonTransformView = GetComponent<PhotonTransformView>();
		carController = GetComponent<CarController>();
		LensFlare[] componentsInChildren = GetComponentsInChildren<LensFlare>();
		foreach (LensFlare lensFlare in componentsInChildren)
		{
			lensFlare.fadeSpeed = 100f;
		}
	}

	private void OnDestroy()
	{
		if (FLTurnLight != null)
		{
			FLTurnLight.enabled = false;
		}
		if (FRTurnLight != null)
		{
			FRTurnLight.enabled = false;
		}
		if (RLTurnLight != null)
		{
			RLTurnLight.enabled = false;
		}
		if (RRTurnLight != null)
		{
			RRTurnLight.enabled = false;
		}
		LensFlare[] brakeLights = BrakeLights;
		foreach (LensFlare lensFlare in brakeLights)
		{
			lensFlare.enabled = false;
		}
		LensFlare[] roofLights = RoofLights;
		foreach (LensFlare lensFlare2 in roofLights)
		{
			lensFlare2.enabled = false;
		}
		SpriteRenderer[] lightBars = LightBars;
		foreach (SpriteRenderer spriteRenderer in lightBars)
		{
			spriteRenderer.enabled = false;
		}
		LensFlare[] headLights = HeadLights;
		foreach (LensFlare lensFlare3 in headLights)
		{
			lensFlare3.enabled = false;
		}
		for (int m = 0; m < PoliceLights.Length; m++)
		{
			PoliceLights[m].enabled = false;
		}
	}

	private void Update()
	{
		if (mainCamera == null)
		{
			return;
		}
		float num = Vector3.Distance(base.transform.position, mainCamera.transform.position);
		float target = 0f;
		if (HeadLights.Length > 0)
		{
			target = ((Vector3.Angle(base.transform.forward, mainCamera.transform.position - HeadLights[0].transform.position) < 90f) ? 1 : 0);
		}
		else if (RoofLights.Length > 0)
		{
			target = ((Vector3.Angle(base.transform.forward, mainCamera.transform.position - RoofLights[0].transform.position) < 90f) ? 1 : 0);
		}
		float target2 = 0f;
		if (BrakeLights.Length > 0)
		{
			target2 = ((Vector3.Angle(-base.transform.forward, mainCamera.transform.position - BrakeLights[0].transform.position) < 90f) ? 1 : 0);
		}
		FrontVisiblityMultiplier = Mathf.MoveTowards(FrontVisiblityMultiplier, target, Time.deltaTime * 5f);
		RearVisiblityMultiplier = Mathf.MoveTowards(RearVisiblityMultiplier, target2, Time.deltaTime * 5f);
		float num2 = 1f;
		if (Physics.Raycast(mainCamera.transform.position, base.transform.position - mainCamera.transform.position, out RaycastHit hitInfo) && hitInfo.collider.transform.root != base.transform)
		{
			num2 = 0f;
		}
		float target3 = LightsOn ? 1 : 0;
		MasterBrightnessMultiplier = Mathf.MoveTowards(MasterBrightnessMultiplier, target3, Time.deltaTime * 5f) * num2;
		if (HeadLights.Length > 0)
		{
			LensFlare[] headLights = HeadLights;
			foreach (LensFlare lensFlare in headLights)
			{
				lensFlare.brightness = Mathf.Max(minSize, HeadlightsBrightness / num) * FrontVisiblityMultiplier * MasterBrightnessMultiplier;
			}
		}
		float num3 = 0f;
		num3 = ((!(carController != null)) ? photonTransformView.lastSteeringAngle : carController.Steering);
		float num4 = (num3 < -15f && Mathf.PingPong(Time.time, 0.5f) > 0.25f) ? 1 : 0;
		float num5 = (num3 > 15f && Mathf.PingPong(Time.time, 0.5f) > 0.25f) ? 1 : 0;
		if (FLTurnLight != null)
		{
			FLTurnLight.brightness = Mathf.Max(minSize, TurnlightsBrightness / num) * FrontVisiblityMultiplier * MasterBrightnessMultiplier * num4;
		}
		if (FRTurnLight != null)
		{
			FRTurnLight.brightness = Mathf.Max(minSize, TurnlightsBrightness / num) * FrontVisiblityMultiplier * MasterBrightnessMultiplier * num5;
		}
		if (RLTurnLight != null)
		{
			RLTurnLight.brightness = Mathf.Max(minSize, TurnlightsBrightness / num) * RearVisiblityMultiplier * MasterBrightnessMultiplier * num4;
		}
		if (RRTurnLight != null)
		{
			RRTurnLight.brightness = Mathf.Max(minSize, TurnlightsBrightness / num) * RearVisiblityMultiplier * MasterBrightnessMultiplier * num5;
		}
		float num6 = 0.5f;
		if (carController != null)
		{
			num6 = 0.5f + carController.Braking / 2f;
		}
		if (BrakeLights.Length > 0)
		{
			LensFlare[] brakeLights = BrakeLights;
			foreach (LensFlare lensFlare2 in brakeLights)
			{
				lensFlare2.brightness = Mathf.Max(minSize, BrakeBrightness / num) * RearVisiblityMultiplier * MasterBrightnessMultiplier * num6;
			}
		}
		if (RoofLights.Length > 0)
		{
			LensFlare[] roofLights = RoofLights;
			foreach (LensFlare lensFlare3 in roofLights)
			{
				lensFlare3.brightness = Mathf.Max(minSize, RooflightsBrightness / num) * FrontVisiblityMultiplier * MasterBrightnessMultiplier * 0.5f;
			}
		}
		if (PoliceLights.Length > 0)
		{
			for (int l = 0; l < PoliceLights.Length; l++)
			{
				PoliceLights[l].brightness = Mathf.Max(minSize, RooflightsBrightness / num) * MasterBrightnessMultiplier * Mathf.Abs((float)l - Mathf.PingPong(Time.time * 10f, 1f));
			}
		}
		SpriteRenderer[] lightBars = LightBars;
		foreach (SpriteRenderer spriteRenderer in lightBars)
		{
			spriteRenderer.enabled = LightsOn;
		}
		if (CrossPlatformInputManager.GetButtonDown("ToggleLights") && carController != null)
		{
			LightsOn = !LightsOn;
			if (photonTransformView.enabled)
			{
				photonTransformView.SendLightsChangingEvent(LightsState);
			}
		}
	}
}
