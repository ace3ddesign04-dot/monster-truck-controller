using System.IO;
using UnityEngine;

public class ScreenshotWithAlpha : MonoBehaviour
{
	public string screenshotName;

	private static Texture2D Screenshot()
	{
		int pixelWidth = Camera.main.pixelWidth;
		int pixelHeight = Camera.main.pixelHeight;
		Camera main = Camera.main;
		CameraClearFlags clearFlags = main.clearFlags;
		main.clearFlags = CameraClearFlags.Depth;
		RenderTexture renderTexture2 = main.targetTexture = new RenderTexture(pixelWidth, pixelHeight, 32);
		Texture2D texture2D = new Texture2D(pixelWidth, pixelHeight, TextureFormat.ARGB32, mipChain: false);
		main.Render();
		RenderTexture.active = renderTexture2;
		texture2D.ReadPixels(new Rect(0f, 0f, pixelWidth, pixelHeight), 0, 0);
		texture2D.Apply();
		main.targetTexture = null;
		RenderTexture.active = null;
		UnityEngine.Object.Destroy(renderTexture2);
		main.clearFlags = clearFlags;
		return texture2D;
	}

	[ContextMenu("Take screenshot")]
	public void SaveScreenshotToFile()
	{
		string path = screenshotName + ".png";
		Texture2D tex = Screenshot();
		byte[] bytes = tex.EncodeToPNG();
		File.WriteAllBytes(path, bytes);
	}
}
