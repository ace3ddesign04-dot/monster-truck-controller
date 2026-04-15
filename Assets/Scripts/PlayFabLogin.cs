using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
	public static PlayFabLogin Instance;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	public void Login()
	{
		PlayFabSettings.TitleId = "433F";
		if (!PlayFabClientAPI.IsClientLoggedIn())
		{
			LoginWithAndroidDeviceIDRequest loginWithAndroidDeviceIDRequest = new LoginWithAndroidDeviceIDRequest();
			loginWithAndroidDeviceIDRequest.AndroidDeviceId = SystemInfo.deviceUniqueIdentifier;
			loginWithAndroidDeviceIDRequest.CreateAccount = true;
			LoginWithAndroidDeviceIDRequest request = loginWithAndroidDeviceIDRequest;
			PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginSuccess, OnLoginFailure);
		}
	}

	private void OnLoginSuccess(LoginResult result)
	{
		UnityEngine.Debug.Log("Logged into PlayFab.");
		UpdateDisplayName();
	}

	private void OnLoginFailure(PlayFabError error)
	{
		UnityEngine.Debug.Log("Login Error:");
		UnityEngine.Debug.Log(error.GenerateErrorReport());
	}

	public void UpdateDisplayName()
	{
		UpdateUserTitleDisplayNameRequest updateUserTitleDisplayNameRequest = new UpdateUserTitleDisplayNameRequest();
		updateUserTitleDisplayNameRequest.DisplayName = GameState.playerName;
		PlayFabClientAPI.UpdateUserTitleDisplayName(updateUserTitleDisplayNameRequest, delegate
		{
		}, delegate(PlayFabError error)
		{
			UnityEngine.Debug.LogError(error.GenerateErrorReport());
		});
	}
}
