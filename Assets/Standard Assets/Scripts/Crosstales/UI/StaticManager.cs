using UnityEngine;

namespace Crosstales.UI
{
	public class StaticManager : MonoBehaviour
	{
		public string AssetstoreURL;

		public void Quit()
		{
			Application.Quit();
		}

		public void OpenCrosstales()
		{
			Application.OpenURL("https://crosstales.com/");
		}

		public void OpenAssetstore()
		{
			Application.OpenURL(AssetstoreURL);
		}
	}
}
