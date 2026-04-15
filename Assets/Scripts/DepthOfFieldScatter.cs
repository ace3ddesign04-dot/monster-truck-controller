using System;
using UnityEngine;

[Serializable]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Depth of Field (HDR, Scatter, Lens Blur)")]
public class DepthOfFieldScatter : PostEffectsBase
{
	[Serializable]
	public enum BlurQuality
	{
		Low,
		Medium,
		High
	}

	[Serializable]
	public enum BlurResolution
	{
		High,
		Low
	}

	public bool visualizeFocus;

	public float focalLength;

	public float focalSize;

	public float aperture;

	public Transform focalTransform;

	public float maxBlurSize;

	public BlurQuality blurQuality;

	public BlurResolution blurResolution;

	public bool foregroundBlur;

	public float foregroundOverlap;

	public Shader dofHdrShader;

	private float focalDistance01;

	private Material dofHdrMaterial;

	public DepthOfFieldScatter()
	{
		focalLength = 10f;
		focalSize = 0.05f;
		aperture = 10f;
		maxBlurSize = 2f;
		blurQuality = BlurQuality.Medium;
		blurResolution = BlurResolution.Low;
		foregroundOverlap = 0.55f;
		focalDistance01 = 10f;
	}

	public override bool CheckResources()
	{
		CheckSupport(needDepth: true);
		dofHdrMaterial = CheckShaderAndCreateMaterial(dofHdrShader, dofHdrMaterial);
		if (!isSupported)
		{
			ReportAutoDisable();
		}
		return isSupported;
	}

	public float FocalDistance01(float worldDist)
	{
		Vector3 vector = GetComponent<Camera>().WorldToViewportPoint((worldDist - GetComponent<Camera>().nearClipPlane) * GetComponent<Camera>().transform.forward + GetComponent<Camera>().transform.position);
		return vector.z / (GetComponent<Camera>().farClipPlane - GetComponent<Camera>().nearClipPlane);
	}

	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!CheckResources())
		{
			Graphics.Blit(source, destination);
			return;
		}
		int num = 0;
		float num2 = maxBlurSize;
		int num3 = (blurResolution == BlurResolution.High) ? 1 : 2;
		if (!(aperture >= 0f))
		{
			aperture = 0f;
		}
		if (!(maxBlurSize >= 0f))
		{
			maxBlurSize = 0f;
		}
		focalSize = Mathf.Clamp(focalSize, 0f, 0.3f);
		float num4;
		if ((bool)focalTransform)
		{
			Vector3 vector = GetComponent<Camera>().WorldToViewportPoint(focalTransform.position);
			num4 = vector.z / GetComponent<Camera>().farClipPlane;
		}
		else
		{
			num4 = FocalDistance01(focalLength);
		}
		focalDistance01 = num4;
		bool flag = source.format == RenderTextureFormat.ARGBHalf;
		RenderTexture renderTexture = (num3 <= 1) ? null : RenderTexture.GetTemporary(source.width / num3, source.height / num3, 0, source.format);
		if ((bool)renderTexture)
		{
			renderTexture.filterMode = FilterMode.Bilinear;
		}
		RenderTexture temporary = RenderTexture.GetTemporary(source.width / (2 * num3), source.height / (2 * num3), 0, source.format);
		RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / (2 * num3), source.height / (2 * num3), 0, source.format);
		if ((bool)temporary)
		{
			temporary.filterMode = FilterMode.Bilinear;
		}
		if ((bool)temporary2)
		{
			temporary2.filterMode = FilterMode.Bilinear;
		}
		dofHdrMaterial.SetVector("_CurveParams", new Vector4(0f, focalSize, aperture / 10f, focalDistance01));
		if (foregroundBlur)
		{
			RenderTexture temporary3 = RenderTexture.GetTemporary(source.width / (2 * num3), source.height / (2 * num3), 0, source.format);
			Graphics.Blit(source, temporary2, dofHdrMaterial, 4);
			dofHdrMaterial.SetTexture("_FgOverlap", temporary2);
			float num5 = num2 * foregroundOverlap * 0.225f;
			dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, num5, 0f, num5));
			Graphics.Blit(temporary2, temporary3, dofHdrMaterial, 2);
			dofHdrMaterial.SetVector("_Offsets", new Vector4(num5, 0f, 0f, num5));
			Graphics.Blit(temporary3, temporary, dofHdrMaterial, 2);
			dofHdrMaterial.SetTexture("_FgOverlap", null);
			Graphics.Blit(temporary, source, dofHdrMaterial, 7);
			RenderTexture.ReleaseTemporary(temporary3);
		}
		else
		{
			dofHdrMaterial.SetTexture("_FgOverlap", null);
		}
		Graphics.Blit(source, source, dofHdrMaterial, foregroundBlur ? 3 : 0);
		RenderTexture renderTexture2 = source;
		if (num3 > 1)
		{
			Graphics.Blit(source, renderTexture, dofHdrMaterial, 6);
			renderTexture2 = renderTexture;
		}
		Graphics.Blit(renderTexture2, temporary2, dofHdrMaterial, 6);
		Graphics.Blit(temporary2, renderTexture2, dofHdrMaterial, 8);
		int pass = 10;
		switch (blurQuality)
		{
		case BlurQuality.Low:
			pass = ((num3 <= 1) ? 10 : 13);
			break;
		case BlurQuality.Medium:
			pass = ((num3 <= 1) ? 11 : 12);
			break;
		case BlurQuality.High:
			pass = ((num3 <= 1) ? 14 : 15);
			break;
		default:
			UnityEngine.Debug.Log("DOF couldn't find valid blur quality setting", transform);
			break;
		}
		if (visualizeFocus)
		{
			Graphics.Blit(source, destination, dofHdrMaterial, 1);
		}
		else
		{
			dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, 0f, 0f, num2));
			dofHdrMaterial.SetTexture("_LowRez", renderTexture2);
			Graphics.Blit(source, destination, dofHdrMaterial, pass);
		}
		if ((bool)temporary)
		{
			RenderTexture.ReleaseTemporary(temporary);
		}
		if ((bool)temporary2)
		{
			RenderTexture.ReleaseTemporary(temporary2);
		}
		if ((bool)renderTexture)
		{
			RenderTexture.ReleaseTemporary(renderTexture);
		}
	}

	public override void Main()
	{
	}
}
