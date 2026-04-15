using System;
using System.IO;
using UnityEngine;

namespace Gaia
{
	public class ScreenShotter : MonoBehaviour
	{
		public KeyCode m_screenShotKey = KeyCode.F12;

		public GaiaConstants.StorageFormat m_imageFormat = GaiaConstants.StorageFormat.JPG;

		public string m_targetDirectory = "Screenshots";

		public int m_targetWidth = 1900;

		public int m_targetHeight = 1200;

		public bool m_useScreenSize = true;

		public Camera m_mainCamera;

		private bool m_takeShot;

		private bool m_refreshAssetDB;

		public Texture2D m_watermark;

		private void OnEnable()
		{
			if (m_mainCamera == null)
			{
				m_mainCamera = Camera.main;
			}
			string path = Path.Combine(Application.dataPath, m_targetDirectory);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		private void OnDisable()
		{
			if (m_refreshAssetDB)
			{
				m_refreshAssetDB = false;
			}
		}

		private string ScreenShotName(int width, int height)
		{
			string text = Path.Combine(Application.dataPath, m_targetDirectory);
			text = text.Replace('\\', '/');
			if (text[text.Length - 1] == '/')
			{
				text = text.Substring(0, text.Length - 1);
			}
			if (m_imageFormat == GaiaConstants.StorageFormat.JPG)
			{
				object[] obj = new object[8]
				{
					text,
					DateTime.Now.ToString("yyyyMMddHHmmss"),
					width,
					height,
					null,
					null,
					null,
					null
				};
				Vector3 position = m_mainCamera.transform.position;
				obj[4] = (int)position.x;
				Vector3 position2 = m_mainCamera.transform.position;
				obj[5] = (int)position2.y;
				Vector3 position3 = m_mainCamera.transform.position;
				obj[6] = (int)position3.z;
				Vector3 eulerAngles = m_mainCamera.transform.rotation.eulerAngles;
				obj[7] = (int)eulerAngles.y;
				return string.Format("{0}/Grab {1} w{2}h{3} x{4}y{5}z{6}r{7}.jpg", obj);
			}
			object[] obj2 = new object[8]
			{
				text,
				DateTime.Now.ToString("yyyyMMdd HHmmss"),
				width,
				height,
				null,
				null,
				null,
				null
			};
			Vector3 position4 = m_mainCamera.transform.position;
			obj2[4] = (int)position4.x;
			Vector3 position5 = m_mainCamera.transform.position;
			obj2[5] = (int)position5.y;
			Vector3 position6 = m_mainCamera.transform.position;
			obj2[6] = (int)position6.z;
			Vector3 eulerAngles2 = m_mainCamera.transform.rotation.eulerAngles;
			obj2[7] = (int)eulerAngles2.y;
			return string.Format("{0}/Grab {1} w{2}h{3} x{4}y{5}z{6}r{7}.png", obj2);
		}

		public void TakeHiResShot()
		{
			m_takeShot = true;
		}

		private void LateUpdate()
		{
			if (UnityEngine.Input.GetKeyDown(m_screenShotKey) || m_takeShot)
			{
				if (m_useScreenSize)
				{
					m_targetWidth = Screen.width;
					m_targetHeight = Screen.height;
				}
				m_refreshAssetDB = true;
				RenderTexture renderTexture = new RenderTexture(m_targetWidth, m_targetHeight, 24);
				m_mainCamera.targetTexture = renderTexture;
				Texture2D texture2D = new Texture2D(m_targetWidth, m_targetHeight, TextureFormat.RGB24, mipChain: false);
				m_mainCamera.Render();
				RenderTexture.active = renderTexture;
				texture2D.ReadPixels(new Rect(0f, 0f, m_targetWidth, m_targetHeight), 0, 0);
				m_mainCamera.targetTexture = null;
				RenderTexture.active = null;
				UnityEngine.Object.Destroy(renderTexture);
				if (m_watermark != null)
				{
					Utils.MakeTextureReadable(m_watermark);
					texture2D = AddWatermark(texture2D, m_watermark);
				}
				byte[] bytes = texture2D.EncodeToJPG();
				string text = ScreenShotName(m_targetWidth, m_targetHeight);
				Utils.WriteAllBytes(text, bytes);
				m_takeShot = false;
				UnityEngine.Debug.Log($"Took screenshot to: {text}");
			}
		}

		public Texture2D AddWatermark(Texture2D background, Texture2D watermark)
		{
			int num = background.width - watermark.width - 10;
			int num2 = num + watermark.width;
			int num3 = 8;
			int num4 = num3 + watermark.height;
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					Color pixel = background.GetPixel(i, j);
					Color pixel2 = watermark.GetPixel(i - num, j - num3);
					Color color = Color.Lerp(pixel, pixel2, pixel2.a / 1f);
					background.SetPixel(i, j, color);
				}
			}
			background.Apply();
			return background;
		}
	}
}
