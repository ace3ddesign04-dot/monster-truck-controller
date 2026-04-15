using System;
using UnityEngine;

namespace PlayFab
{
	public static class PlayFabSettings
	{
		public const string AD_TYPE_IDFA = "Idfa";

		public const string AD_TYPE_ANDROID_ID = "Adid";

		public static string AdvertisingIdType;

		public static string AdvertisingIdValue;

		public static bool DisableAdvertising;

		public static bool DisableDeviceInfo;

		private static PlayFabSharedSettings _playFabShared;

		public const string SdkVersion = "2.33.171127";

		public const string BuildIdentifier = "jbuild_unitysdk__0";

		public const string VersionString = "UnitySDK-2.33.171127";

		private const string DefaultPlayFabApiUrlPrivate = ".playfabapi.com";

		private static PlayFabSharedSettings PlayFabSharedPrivate
		{
			get
			{
				if (_playFabShared == null)
				{
					_playFabShared = GetSharedSettingsObjectPrivate();
				}
				return _playFabShared;
			}
		}

		[Obsolete("This field will become private after Mar 1, 2017", false)]
		public static PlayFabSharedSettings PlayFabShared
		{
			get
			{
				if (_playFabShared == null)
				{
					_playFabShared = GetSharedSettingsObjectPrivate();
				}
				return _playFabShared;
			}
		}

		[Obsolete("This field will become private after Mar 1, 2017", false)]
		public static string DefaultPlayFabApiUrl => ".playfabapi.com";

		public static string DeveloperSecretKey
		{
			internal get
			{
				return PlayFabSharedPrivate.DeveloperSecretKey;
			}
			set
			{
				PlayFabSharedPrivate.DeveloperSecretKey = value;
			}
		}

		public static string DeviceUniqueIdentifier
		{
			get
			{
				string empty = string.Empty;
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getContentResolver", new object[0]);
				AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.provider.Settings$Secure");
				return androidJavaClass2.CallStatic<string>("getString", new object[2]
				{
					androidJavaObject,
					"android_id"
				});
			}
		}

		private static string ProductionEnvironmentUrlPrivate
		{
			get
			{
				return string.IsNullOrEmpty(PlayFabSharedPrivate.ProductionEnvironmentUrl) ? ".playfabapi.com" : PlayFabSharedPrivate.ProductionEnvironmentUrl;
			}
			set
			{
				PlayFabSharedPrivate.ProductionEnvironmentUrl = value;
			}
		}

		[Obsolete("This field will become private after Mar 1, 2017", false)]
		public static string ProductionEnvironmentUrl
		{
			get
			{
				return ProductionEnvironmentUrlPrivate;
			}
			set
			{
				ProductionEnvironmentUrlPrivate = value;
			}
		}

		public static string TitleId
		{
			get
			{
				return PlayFabSharedPrivate.TitleId;
			}
			set
			{
				PlayFabSharedPrivate.TitleId = value;
			}
		}

		public static PlayFabLogLevel LogLevel
		{
			get
			{
				return PlayFabSharedPrivate.LogLevel;
			}
			set
			{
				PlayFabSharedPrivate.LogLevel = value;
			}
		}

		public static WebRequestType RequestType
		{
			get
			{
				return PlayFabSharedPrivate.RequestType;
			}
			set
			{
				PlayFabSharedPrivate.RequestType = value;
			}
		}

		public static int RequestTimeout
		{
			get
			{
				return PlayFabSharedPrivate.RequestTimeout;
			}
			set
			{
				PlayFabSharedPrivate.RequestTimeout = value;
			}
		}

		public static bool RequestKeepAlive
		{
			get
			{
				return PlayFabSharedPrivate.RequestKeepAlive;
			}
			set
			{
				PlayFabSharedPrivate.RequestKeepAlive = value;
			}
		}

		public static bool CompressApiData
		{
			get
			{
				return PlayFabSharedPrivate.CompressApiData;
			}
			set
			{
				PlayFabSharedPrivate.CompressApiData = value;
			}
		}

		public static string LoggerHost
		{
			get
			{
				return PlayFabSharedPrivate.LoggerHost;
			}
			set
			{
				PlayFabSharedPrivate.LoggerHost = value;
			}
		}

		public static int LoggerPort
		{
			get
			{
				return PlayFabSharedPrivate.LoggerPort;
			}
			set
			{
				PlayFabSharedPrivate.LoggerPort = value;
			}
		}

		public static bool EnableRealTimeLogging
		{
			get
			{
				return PlayFabSharedPrivate.EnableRealTimeLogging;
			}
			set
			{
				PlayFabSharedPrivate.EnableRealTimeLogging = value;
			}
		}

		public static int LogCapLimit
		{
			get
			{
				return PlayFabSharedPrivate.LogCapLimit;
			}
			set
			{
				PlayFabSharedPrivate.LogCapLimit = value;
			}
		}

		static PlayFabSettings()
		{
		}

		private static PlayFabSharedSettings GetSharedSettingsObjectPrivate()
		{
			PlayFabSharedSettings[] array = Resources.LoadAll<PlayFabSharedSettings>("PlayFabSharedSettings");
			if (array.Length != 1)
			{
				throw new Exception("The number of PlayFabSharedSettings objects should be 1: " + array.Length);
			}
			return array[0];
		}

		[Obsolete("This field will become private after Mar 1, 2017", false)]
		public static PlayFabSharedSettings GetSharedSettingsObject()
		{
			return GetSharedSettingsObjectPrivate();
		}

		public static string GetFullUrl(string apiCall)
		{
			string productionEnvironmentUrlPrivate = ProductionEnvironmentUrlPrivate;
			if (productionEnvironmentUrlPrivate.StartsWith("http"))
			{
				return productionEnvironmentUrlPrivate + apiCall;
			}
			return "https://" + TitleId + productionEnvironmentUrlPrivate + apiCall;
		}
	}
}
