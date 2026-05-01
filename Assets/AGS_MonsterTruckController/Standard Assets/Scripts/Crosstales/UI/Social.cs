using UnityEngine;

namespace Crosstales.UI
{
	public class Social : MonoBehaviour
	{
		public void Facebook()
		{
			Application.OpenURL("https://www.facebook.com/crosstales/");
		}

		public void Twitter()
		{
			Application.OpenURL("https://twitter.com/crosstales");
		}

		public void LinkedIn()
		{
			Application.OpenURL("https://www.linkedin.com/company/crosstales");
		}

		public void Xing()
		{
			Application.OpenURL("https://www.xing.com/companies/crosstales");
		}

		public void Youtube()
		{
			Application.OpenURL("https://www.youtube.com/c/Crosstales");
		}
	}
}
