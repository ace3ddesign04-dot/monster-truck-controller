using System.Collections.Generic;
using UnityEngine;

public class LightTree : MonoBehaviour
{
	public float Brightness = 1f;

	public LightSet Player1Lights;

	public LightSet Player2Lights;

	private List<LensFlare> AllLights = new List<LensFlare>();

	private Transform cam;

	private void Awake()
	{
		Player1Lights.ShutAllLights();
		Player2Lights.ShutAllLights();
		AllLights.Add(Player1Lights.Countdown1Light);
		AllLights.Add(Player1Lights.Countdown2Light);
		AllLights.Add(Player1Lights.Countdown3Light);
		AllLights.Add(Player1Lights.StartLight);
		AllLights.Add(Player1Lights.StageLights[0]);
		AllLights.Add(Player1Lights.StageLights[1]);
		AllLights.Add(Player1Lights.PreStageLights[0]);
		AllLights.Add(Player1Lights.PreStageLights[1]);
		AllLights.Add(Player2Lights.Countdown1Light);
		AllLights.Add(Player2Lights.Countdown2Light);
		AllLights.Add(Player2Lights.Countdown3Light);
		AllLights.Add(Player2Lights.StartLight);
		AllLights.Add(Player2Lights.StageLights[0]);
		AllLights.Add(Player2Lights.StageLights[1]);
		AllLights.Add(Player2Lights.PreStageLights[0]);
		AllLights.Add(Player2Lights.PreStageLights[1]);
		cam = Camera.main.transform;
	}

	private void Update()
	{
		float brightness = Brightness / Vector3.Distance(base.transform.position, cam.position);
		Player1Lights.SetBrightness(brightness);
		Player2Lights.SetBrightness(brightness);
		if (UnityEngine.Input.GetKeyDown(KeyCode.Keypad0))
		{
			Player1Lights.SetLightState(LightState.ShutAll);
			Player2Lights.SetLightState(LightState.ShutAll);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Keypad1))
		{
			Player1Lights.SetLightState(LightState.PreStage);
			Player2Lights.SetLightState(LightState.PreStage);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Keypad2))
		{
			Player1Lights.SetLightState(LightState.Stage);
			Player2Lights.SetLightState(LightState.Stage);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Keypad3))
		{
			Player1Lights.SetLightState(LightState.Countdown3);
			Player2Lights.SetLightState(LightState.Countdown3);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Keypad4))
		{
			Player1Lights.SetLightState(LightState.Countdown2);
			Player2Lights.SetLightState(LightState.Countdown2);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Keypad5))
		{
			Player1Lights.SetLightState(LightState.Countdown1);
			Player2Lights.SetLightState(LightState.Countdown1);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Keypad6))
		{
			Player1Lights.SetLightState(LightState.Start);
			Player2Lights.SetLightState(LightState.Start);
		}
	}
}
