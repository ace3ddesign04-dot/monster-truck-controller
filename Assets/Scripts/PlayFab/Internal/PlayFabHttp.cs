using PlayFab.ClientModels;
using PlayFab.Json;
using PlayFab.Public;
using PlayFab.SharedModels;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PlayFab.Internal
{
	public class PlayFabHttp : SingletonMonoBehaviour<PlayFabHttp>
	{
		public delegate void ApiProcessingEvent<in TEventArgs>(TEventArgs e);

		public delegate void ApiProcessErrorEvent(PlayFabRequestCommon request, PlayFabError error);

		private static IPlayFabHttp _internalHttp;

		private static List<CallRequestContainer> _apiCallQueue = new List<CallRequestContainer>();

		public static readonly Dictionary<string, string> GlobalHeaderInjection = new Dictionary<string, string>();

		private static IPlayFabLogger _logger;

		public static event ApiProcessingEvent<ApiProcessingEventArgs> ApiProcessingEventHandler;

		public static event ApiProcessErrorEvent ApiProcessingErrorEventHandler;

		public static int GetPendingMessages()
		{
			return (_internalHttp != null) ? _internalHttp.GetPendingMessages() : 0;
		}

		public static void SetHttp<THttpObject>(THttpObject httpObj) where THttpObject : IPlayFabHttp
		{
			_internalHttp = httpObj;
		}

		public static void SetAuthKey(string authKey)
		{
			_internalHttp.AuthKey = authKey;
		}

		public static void InitializeHttp()
		{
			if (_internalHttp == null)
			{
				Application.runInBackground = true;
				if (PlayFabSettings.RequestType == WebRequestType.HttpWebRequest)
				{
					_internalHttp = new PlayFabWebRequest();
				}
				if (_internalHttp == null)
				{
					_internalHttp = new PlayFabWww();
				}
				_internalHttp.InitializeHttp();
				SingletonMonoBehaviour<PlayFabHttp>.CreateInstance();
			}
		}

		public static void InitializeLogger(IPlayFabLogger setLogger = null)
		{
			if (_logger != null)
			{
				throw new Exception("Once initialized, the logger cannot be reset.");
			}
			if (setLogger == null)
			{
				setLogger = new PlayFabLogger();
			}
			_logger = setLogger;
		}

		protected internal static void MakeApiCall<TResult>(string apiEndpoint, PlayFabRequestCommon request, AuthType authType, Action<TResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null, bool allowQueueing = false) where TResult : PlayFabResultCommon
		{
			InitializeHttp();
			SendEvent(apiEndpoint, request, null, ApiProcessingEventType.Pre);
			CallRequestContainer reqContainer = new CallRequestContainer
			{
				ApiEndpoint = apiEndpoint,
				FullUrl = PlayFabSettings.GetFullUrl(apiEndpoint),
				CustomData = customData,
				Payload = Encoding.UTF8.GetBytes(JsonWrapper.SerializeObject(request)),
				ApiRequest = request,
				ErrorCallback = errorCallback,
				RequestHeaders = (extraHeaders ?? new Dictionary<string, string>())
			};
			foreach (KeyValuePair<string, string> item in GlobalHeaderInjection)
			{
				if (!reqContainer.RequestHeaders.ContainsKey(item.Key))
				{
					reqContainer.RequestHeaders[item.Key] = item.Value;
				}
			}
			reqContainer.RequestHeaders["X-ReportErrorAsSuccess"] = "true";
			reqContainer.RequestHeaders["X-PlayFabSDK"] = "UnitySDK-2.33.171127";
			switch (authType)
			{
			case AuthType.DevSecretKey:
				reqContainer.RequestHeaders["X-SecretKey"] = PlayFabSettings.DeveloperSecretKey;
				break;
			case AuthType.LoginSession:
				reqContainer.RequestHeaders["X-Authorization"] = _internalHttp.AuthKey;
				break;
			case AuthType.EntityToken:
				reqContainer.RequestHeaders["X-EntityToken"] = _internalHttp.EntityToken;
				break;
			}
			reqContainer.DeserializeResultJson = delegate
			{
				reqContainer.ApiResult = JsonWrapper.DeserializeObject<TResult>(reqContainer.JsonResponse);
			};
			reqContainer.InvokeSuccessCallback = delegate
			{
				if (resultCallback != null)
				{
					resultCallback((TResult)reqContainer.ApiResult);
				}
			};
			if (allowQueueing && _apiCallQueue != null && !_internalHttp.SessionStarted)
			{
				for (int num = _apiCallQueue.Count - 1; num >= 0; num--)
				{
					if (_apiCallQueue[num].ApiEndpoint == apiEndpoint)
					{
						_apiCallQueue.RemoveAt(num);
					}
				}
				_apiCallQueue.Add(reqContainer);
			}
			else
			{
				_internalHttp.MakeApiCall(reqContainer);
			}
		}

		internal void OnPlayFabApiResult(PlayFabResultCommon result)
		{
			LoginResult loginResult = result as LoginResult;
			RegisterPlayFabUserResult registerPlayFabUserResult = result as RegisterPlayFabUserResult;
			if (loginResult != null)
			{
				_internalHttp.AuthKey = loginResult.SessionTicket;
			}
			else if (registerPlayFabUserResult != null)
			{
				_internalHttp.AuthKey = registerPlayFabUserResult.SessionTicket;
			}
		}

		private void OnEnable()
		{
			if (_logger != null)
			{
				_logger.OnEnable();
			}
		}

		private void OnDisable()
		{
			if (_logger != null)
			{
				_logger.OnDisable();
			}
		}

		private void OnDestroy()
		{
			if (_internalHttp != null)
			{
				_internalHttp.OnDestroy();
			}
			if (_logger != null)
			{
				_logger.OnDestroy();
			}
		}

		private void Update()
		{
			if (_internalHttp != null)
			{
				if (!_internalHttp.SessionStarted && _apiCallQueue != null)
				{
					foreach (CallRequestContainer item in _apiCallQueue)
					{
						_internalHttp.MakeApiCall(item);
					}
					_apiCallQueue = null;
				}
				_internalHttp.Update();
			}
		}

		public static bool IsClientLoggedIn()
		{
			return _internalHttp != null && !string.IsNullOrEmpty(_internalHttp.AuthKey);
		}

		public static void ForgetAllCredentials()
		{
			if (_internalHttp != null)
			{
				_internalHttp.AuthKey = null;
				_internalHttp.EntityToken = null;
			}
		}

		protected internal static PlayFabError GeneratePlayFabError(string json, object customData)
		{
			JsonObject jsonObject = null;
			Dictionary<string, List<string>> errorDetails = null;
			try
			{
				jsonObject = JsonWrapper.DeserializeObject<JsonObject>(json);
			}
			catch (Exception)
			{
			}
			try
			{
				if (jsonObject != null && jsonObject.ContainsKey("errorDetails"))
				{
					errorDetails = JsonWrapper.DeserializeObject<Dictionary<string, List<string>>>(jsonObject["errorDetails"].ToString());
				}
			}
			catch (Exception)
			{
			}
			PlayFabError playFabError = new PlayFabError();
			playFabError.HttpCode = ((jsonObject == null || !jsonObject.ContainsKey("code")) ? 400 : Convert.ToInt32(jsonObject["code"]));
			playFabError.HttpStatus = ((jsonObject == null || !jsonObject.ContainsKey("status")) ? "BadRequest" : ((string)jsonObject["status"]));
			playFabError.Error = (PlayFabErrorCode)((jsonObject == null || !jsonObject.ContainsKey("errorCode")) ? 1123 : Convert.ToInt32(jsonObject["errorCode"]));
			playFabError.ErrorMessage = ((jsonObject == null || !jsonObject.ContainsKey("errorMessage")) ? json : ((string)jsonObject["errorMessage"]));
			playFabError.ErrorDetails = errorDetails;
			playFabError.CustomData = customData;
			return playFabError;
		}

		protected internal static void SendErrorEvent(PlayFabRequestCommon request, PlayFabError error)
		{
			if (PlayFabHttp.ApiProcessingErrorEventHandler != null)
			{
				try
				{
					PlayFabHttp.ApiProcessingErrorEventHandler(request, error);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
			}
		}

		protected internal static void SendEvent(string apiEndpoint, PlayFabRequestCommon request, PlayFabResultCommon result, ApiProcessingEventType eventType)
		{
			if (PlayFabHttp.ApiProcessingEventHandler != null)
			{
				try
				{
					PlayFabHttp.ApiProcessingEventHandler(new ApiProcessingEventArgs
					{
						ApiEndpoint = apiEndpoint,
						EventType = eventType,
						Request = request,
						Result = result
					});
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
			}
		}

		protected internal static void ClearAllEvents()
		{
			PlayFabHttp.ApiProcessingEventHandler = null;
			PlayFabHttp.ApiProcessingErrorEventHandler = null;
		}
	}
}
