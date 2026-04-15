using System;
using System.Net;

namespace Crosstales.BWF.Util
{
	public class CTWebClient : WebClient
	{
		public int Timeout
		{
			get;
			set;
		}

		public CTWebClient()
			: this(5000)
		{
		}

		public CTWebClient(int timeout)
		{
			Timeout = timeout;
		}

		protected override WebRequest GetWebRequest(Uri uri)
		{
			WebRequest webRequest = base.GetWebRequest(uri);
			if (webRequest != null)
			{
				webRequest.Timeout = Timeout;
			}
			return webRequest;
		}
	}
}
