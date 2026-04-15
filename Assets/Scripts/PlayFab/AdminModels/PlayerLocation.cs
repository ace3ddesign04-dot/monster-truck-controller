using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class PlayerLocation
	{
		public string City;

		public ContinentCode ContinentCode;

		public CountryCode CountryCode;

		public double? Latitude;

		public double? Longitude;
	}
}
