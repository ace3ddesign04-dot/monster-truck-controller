using PlayFab.ClientModels;
using PlayFab.SharedModels;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PlayFab.Internal
{
	public static class PlayFabDeviceUtil
	{
		private class DeviceInfoRequest : PlayFabRequestCommon
		{
			public PlayFabDataGatherer Info;
		}

		private static bool _needsAttribution;

		private static bool _gatherInfo;

		[CompilerGenerated]
		private static Action<AttributeInstallResult> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static Action<EmptyResult> _003C_003Ef__mg_0024cache1;

		[CompilerGenerated]
		private static Action<PlayFabError> _003C_003Ef__mg_0024cache2;

		private static void DoAttributeInstall()
		{
			if (_needsAttribution && !PlayFabSettings.DisableAdvertising)
			{
				AttributeInstallRequest attributeInstallRequest = new AttributeInstallRequest();
				switch (PlayFabSettings.AdvertisingIdType)
				{
				case "Adid":
					attributeInstallRequest.Adid = PlayFabSettings.AdvertisingIdValue;
					break;
				case "Idfa":
					attributeInstallRequest.Idfa = PlayFabSettings.AdvertisingIdValue;
					break;
				}
				PlayFabClientAPI.AttributeInstall(attributeInstallRequest, OnAttributeInstall, null);
			}
		}

		private static void OnAttributeInstall(AttributeInstallResult result)
		{
			PlayFabSettings.AdvertisingIdType += "_Successful";
		}

		private static void SendDeviceInfoToPlayFab()
		{
			if (!PlayFabSettings.DisableDeviceInfo && _gatherInfo)
			{
				DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
				deviceInfoRequest.Info = new PlayFabDataGatherer();
				DeviceInfoRequest request = deviceInfoRequest;
				PlayFabHttp.MakeApiCall<EmptyResult>("/Client/ReportDeviceInfo", request, AuthType.LoginSession, OnGatherSuccess, OnGatherFail);
			}
		}

		private static void OnGatherSuccess(EmptyResult result)
		{
			UnityEngine.Debug.Log("OnGatherSuccess");
		}

		private static void OnGatherFail(PlayFabError error)
		{
			UnityEngine.Debug.Log("OnGatherFail: " + error.GenerateErrorReport());
		}

		public static void OnPlayFabLogin(PlayFabResultCommon result)
		{
			LoginResult loginResult = result as LoginResult;
			RegisterPlayFabUserResult registerPlayFabUserResult = result as RegisterPlayFabUserResult;
			if (loginResult != null || registerPlayFabUserResult != null)
			{
				_needsAttribution = false;
				_gatherInfo = false;
				if (loginResult != null && loginResult.SettingsForUser != null)
				{
					_needsAttribution = loginResult.SettingsForUser.NeedsAttribution;
				}
				else if (registerPlayFabUserResult != null && registerPlayFabUserResult.SettingsForUser != null)
				{
					_needsAttribution = registerPlayFabUserResult.SettingsForUser.NeedsAttribution;
				}
				if (loginResult != null && loginResult.SettingsForUser != null)
				{
					_gatherInfo = loginResult.SettingsForUser.GatherDeviceInfo;
				}
				else if (registerPlayFabUserResult != null && registerPlayFabUserResult.SettingsForUser != null)
				{
					_gatherInfo = registerPlayFabUserResult.SettingsForUser.GatherDeviceInfo;
				}
				if (PlayFabSettings.AdvertisingIdType != null && PlayFabSettings.AdvertisingIdValue != null)
				{
					DoAttributeInstall();
				}
				else
				{
					GetAdvertIdFromUnity();
				}
				SendDeviceInfoToPlayFab();
			}
		}

		private static void GetAdvertIdFromUnity()
		{
			Application.RequestAdvertisingIdentifierAsync(delegate(string advertisingId, bool trackingEnabled, string error)
			{
				PlayFabSettings.DisableAdvertising = !trackingEnabled;
				if (trackingEnabled)
				{
					PlayFabSettings.AdvertisingIdType = "Adid";
					PlayFabSettings.AdvertisingIdValue = advertisingId;
					DoAttributeInstall();
				}
			});
		}
	}
}
