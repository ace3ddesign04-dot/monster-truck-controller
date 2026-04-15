using System;
using UnityEngine;

[Serializable]
public class LightSet
{
	private float Brightness;

	private LightState lightState;

	public LensFlare[] PreStageLights;

	public LensFlare[] StageLights;

	public LensFlare Countdown3Light;

	public LensFlare Countdown2Light;

	public LensFlare Countdown1Light;

	public LensFlare StartLight;

	public void SetLightState(LightState state)
	{
		lightState = state;
		UpdateLights();
	}

	public void SetBrightness(float value)
	{
		Brightness = value;
		UpdateLights();
	}

	public void UpdateLights()
	{
		LensFlare[] preStageLights = PreStageLights;
		foreach (LensFlare lensFlare in preStageLights)
		{
			lensFlare.brightness = ((lightState <= LightState.ShutAll) ? 0f : Brightness);
		}
		LensFlare[] stageLights = StageLights;
		foreach (LensFlare lensFlare2 in stageLights)
		{
			lensFlare2.brightness = ((lightState <= LightState.PreStage) ? 0f : Brightness);
		}
		Countdown3Light.brightness = ((lightState <= LightState.Stage || lightState >= LightState.Start) ? 0f : Brightness);
		Countdown2Light.brightness = ((lightState <= LightState.Countdown3 || lightState >= LightState.Start) ? 0f : Brightness);
		Countdown1Light.brightness = ((lightState <= LightState.Countdown2 || lightState >= LightState.Start) ? 0f : Brightness);
		StartLight.brightness = ((lightState <= LightState.Countdown1) ? 0f : Brightness);
	}

	public void ShutAllLights()
	{
		SetLightState(LightState.ShutAll);
	}
}
