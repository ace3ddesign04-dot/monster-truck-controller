using PlayFab.Internal;
using PlayFab.Json;
using PlayFab.ServerModels;
using System;
using System.Collections.Generic;

namespace PlayFab
{
	public static class PlayFabServerAPI
	{
		static PlayFabServerAPI()
		{
		}

		public static void ForgetAllCredentials()
		{
			PlayFabHttp.ForgetAllCredentials();
		}

		public static void AddCharacterVirtualCurrency(AddCharacterVirtualCurrencyRequest request, Action<ModifyCharacterVirtualCurrencyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/AddCharacterVirtualCurrency", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void AddFriend(AddFriendRequest request, Action<EmptyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/AddFriend", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void AddPlayerTag(AddPlayerTagRequest request, Action<AddPlayerTagResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/AddPlayerTag", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void AddSharedGroupMembers(AddSharedGroupMembersRequest request, Action<AddSharedGroupMembersResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/AddSharedGroupMembers", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void AddUserVirtualCurrency(AddUserVirtualCurrencyRequest request, Action<ModifyUserVirtualCurrencyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/AddUserVirtualCurrency", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void AuthenticateSessionTicket(AuthenticateSessionTicketRequest request, Action<AuthenticateSessionTicketResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/AuthenticateSessionTicket", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void AwardSteamAchievement(AwardSteamAchievementRequest request, Action<AwardSteamAchievementResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/AwardSteamAchievement", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void BanUsers(BanUsersRequest request, Action<BanUsersResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/BanUsers", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void ConsumeItem(ConsumeItemRequest request, Action<ConsumeItemResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/ConsumeItem", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void CreateSharedGroup(CreateSharedGroupRequest request, Action<CreateSharedGroupResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/CreateSharedGroup", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void DeleteCharacterFromUser(DeleteCharacterFromUserRequest request, Action<DeleteCharacterFromUserResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/DeleteCharacterFromUser", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void DeleteSharedGroup(DeleteSharedGroupRequest request, Action<EmptyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/DeleteSharedGroup", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void DeleteUsers(DeleteUsersRequest request, Action<DeleteUsersResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/DeleteUsers", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void DeregisterGame(DeregisterGameRequest request, Action<DeregisterGameResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/DeregisterGame", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void EvaluateRandomResultTable(EvaluateRandomResultTableRequest request, Action<EvaluateRandomResultTableResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/EvaluateRandomResultTable", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void ExecuteCloudScript(ExecuteCloudScriptServerRequest request, Action<ExecuteCloudScriptResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/ExecuteCloudScript", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void ExecuteCloudScript<TOut>(ExecuteCloudScriptServerRequest request, Action<ExecuteCloudScriptResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			Action<ExecuteCloudScriptResult> resultCallback2 = delegate(ExecuteCloudScriptResult wrappedResult)
			{
				string text = JsonWrapper.SerializeObject(wrappedResult.FunctionResult);
				try
				{
					wrappedResult.FunctionResult = JsonWrapper.DeserializeObject<TOut>(text);
				}
				catch (Exception)
				{
					wrappedResult.FunctionResult = text;
					wrappedResult.Logs.Add(new LogStatement
					{
						Level = "Warning",
						Data = text,
						Message = "Sdk Message: Could not deserialize result as: " + typeof(TOut).Name
					});
				}
				resultCallback(wrappedResult);
			};
			ExecuteCloudScript(request, resultCallback2, errorCallback, customData, extraHeaders);
		}

		public static void GetAllSegments(GetAllSegmentsRequest request, Action<GetAllSegmentsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetAllSegments", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetAllUsersCharacters(ListUsersCharactersRequest request, Action<ListUsersCharactersResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetAllUsersCharacters", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetCatalogItems(GetCatalogItemsRequest request, Action<GetCatalogItemsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetCatalogItems", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetCharacterData(GetCharacterDataRequest request, Action<GetCharacterDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetCharacterData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetCharacterInternalData(GetCharacterDataRequest request, Action<GetCharacterDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetCharacterInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetCharacterInventory(GetCharacterInventoryRequest request, Action<GetCharacterInventoryResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetCharacterInventory", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetCharacterLeaderboard(GetCharacterLeaderboardRequest request, Action<GetCharacterLeaderboardResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetCharacterLeaderboard", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetCharacterReadOnlyData(GetCharacterDataRequest request, Action<GetCharacterDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetCharacterReadOnlyData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetCharacterStatistics(GetCharacterStatisticsRequest request, Action<GetCharacterStatisticsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetCharacterStatistics", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetContentDownloadUrl(GetContentDownloadUrlRequest request, Action<GetContentDownloadUrlResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetContentDownloadUrl", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetFriendLeaderboard(GetFriendLeaderboardRequest request, Action<GetLeaderboardResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetFriendLeaderboard", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetFriendsList(GetFriendsListRequest request, Action<GetFriendsListResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetFriendsList", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetLeaderboard(GetLeaderboardRequest request, Action<GetLeaderboardResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetLeaderboard", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetLeaderboardAroundCharacter(GetLeaderboardAroundCharacterRequest request, Action<GetLeaderboardAroundCharacterResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetLeaderboardAroundCharacter", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetLeaderboardAroundUser(GetLeaderboardAroundUserRequest request, Action<GetLeaderboardAroundUserResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetLeaderboardAroundUser", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetLeaderboardForUserCharacters(GetLeaderboardForUsersCharactersRequest request, Action<GetLeaderboardForUsersCharactersResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetLeaderboardForUserCharacters", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetPlayerCombinedInfo(GetPlayerCombinedInfoRequest request, Action<GetPlayerCombinedInfoResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetPlayerCombinedInfo", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetPlayerProfile(GetPlayerProfileRequest request, Action<GetPlayerProfileResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetPlayerProfile", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetPlayerSegments(GetPlayersSegmentsRequest request, Action<GetPlayerSegmentsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetPlayerSegments", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetPlayersInSegment(GetPlayersInSegmentRequest request, Action<GetPlayersInSegmentResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetPlayersInSegment", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetPlayerStatistics(GetPlayerStatisticsRequest request, Action<GetPlayerStatisticsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetPlayerStatistics", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetPlayerStatisticVersions(GetPlayerStatisticVersionsRequest request, Action<GetPlayerStatisticVersionsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetPlayerStatisticVersions", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetPlayerTags(GetPlayerTagsRequest request, Action<GetPlayerTagsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetPlayerTags", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetPlayFabIDsFromFacebookIDs(GetPlayFabIDsFromFacebookIDsRequest request, Action<GetPlayFabIDsFromFacebookIDsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetPlayFabIDsFromFacebookIDs", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetPlayFabIDsFromSteamIDs(GetPlayFabIDsFromSteamIDsRequest request, Action<GetPlayFabIDsFromSteamIDsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetPlayFabIDsFromSteamIDs", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetPublisherData(GetPublisherDataRequest request, Action<GetPublisherDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetPublisherData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetRandomResultTables(GetRandomResultTablesRequest request, Action<GetRandomResultTablesResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetRandomResultTables", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetSharedGroupData(GetSharedGroupDataRequest request, Action<GetSharedGroupDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetSharedGroupData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetTime(GetTimeRequest request, Action<GetTimeResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetTime", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetTitleData(GetTitleDataRequest request, Action<GetTitleDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetTitleData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetTitleInternalData(GetTitleDataRequest request, Action<GetTitleDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetTitleInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetTitleNews(GetTitleNewsRequest request, Action<GetTitleNewsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetTitleNews", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetUserAccountInfo(GetUserAccountInfoRequest request, Action<GetUserAccountInfoResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetUserAccountInfo", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetUserBans(GetUserBansRequest request, Action<GetUserBansResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetUserBans", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetUserData(GetUserDataRequest request, Action<GetUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetUserData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetUserInternalData(GetUserDataRequest request, Action<GetUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetUserInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetUserInventory(GetUserInventoryRequest request, Action<GetUserInventoryResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetUserInventory", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetUserPublisherData(GetUserDataRequest request, Action<GetUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetUserPublisherData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetUserPublisherInternalData(GetUserDataRequest request, Action<GetUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetUserPublisherInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetUserPublisherReadOnlyData(GetUserDataRequest request, Action<GetUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetUserPublisherReadOnlyData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GetUserReadOnlyData(GetUserDataRequest request, Action<GetUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GetUserReadOnlyData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GrantCharacterToUser(GrantCharacterToUserRequest request, Action<GrantCharacterToUserResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GrantCharacterToUser", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GrantItemsToCharacter(GrantItemsToCharacterRequest request, Action<GrantItemsToCharacterResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GrantItemsToCharacter", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GrantItemsToUser(GrantItemsToUserRequest request, Action<GrantItemsToUserResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GrantItemsToUser", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void GrantItemsToUsers(GrantItemsToUsersRequest request, Action<GrantItemsToUsersResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/GrantItemsToUsers", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void ModifyItemUses(ModifyItemUsesRequest request, Action<ModifyItemUsesResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/ModifyItemUses", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void MoveItemToCharacterFromCharacter(MoveItemToCharacterFromCharacterRequest request, Action<MoveItemToCharacterFromCharacterResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/MoveItemToCharacterFromCharacter", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void MoveItemToCharacterFromUser(MoveItemToCharacterFromUserRequest request, Action<MoveItemToCharacterFromUserResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/MoveItemToCharacterFromUser", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void MoveItemToUserFromCharacter(MoveItemToUserFromCharacterRequest request, Action<MoveItemToUserFromCharacterResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/MoveItemToUserFromCharacter", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void NotifyMatchmakerPlayerLeft(NotifyMatchmakerPlayerLeftRequest request, Action<NotifyMatchmakerPlayerLeftResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/NotifyMatchmakerPlayerLeft", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void RedeemCoupon(RedeemCouponRequest request, Action<RedeemCouponResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/RedeemCoupon", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void RedeemMatchmakerTicket(RedeemMatchmakerTicketRequest request, Action<RedeemMatchmakerTicketResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/RedeemMatchmakerTicket", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void RefreshGameServerInstanceHeartbeat(RefreshGameServerInstanceHeartbeatRequest request, Action<RefreshGameServerInstanceHeartbeatResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/RefreshGameServerInstanceHeartbeat", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void RegisterGame(RegisterGameRequest request, Action<RegisterGameResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/RegisterGame", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void RemoveFriend(RemoveFriendRequest request, Action<EmptyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/RemoveFriend", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void RemovePlayerTag(RemovePlayerTagRequest request, Action<RemovePlayerTagResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/RemovePlayerTag", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void RemoveSharedGroupMembers(RemoveSharedGroupMembersRequest request, Action<RemoveSharedGroupMembersResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/RemoveSharedGroupMembers", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void ReportPlayer(ReportPlayerServerRequest request, Action<ReportPlayerServerResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/ReportPlayer", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void RevokeAllBansForUser(RevokeAllBansForUserRequest request, Action<RevokeAllBansForUserResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/RevokeAllBansForUser", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void RevokeBans(RevokeBansRequest request, Action<RevokeBansResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/RevokeBans", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void RevokeInventoryItem(RevokeInventoryItemRequest request, Action<RevokeInventoryResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/RevokeInventoryItem", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void SendCustomAccountRecoveryEmail(SendCustomAccountRecoveryEmailRequest request, Action<SendCustomAccountRecoveryEmailResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/SendCustomAccountRecoveryEmail", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void SendEmailFromTemplate(SendEmailFromTemplateRequest request, Action<SendEmailFromTemplateResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/SendEmailFromTemplate", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void SendPushNotification(SendPushNotificationRequest request, Action<SendPushNotificationResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/SendPushNotification", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void SetFriendTags(SetFriendTagsRequest request, Action<EmptyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/SetFriendTags", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void SetGameServerInstanceData(SetGameServerInstanceDataRequest request, Action<SetGameServerInstanceDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/SetGameServerInstanceData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void SetGameServerInstanceState(SetGameServerInstanceStateRequest request, Action<SetGameServerInstanceStateResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/SetGameServerInstanceState", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void SetGameServerInstanceTags(SetGameServerInstanceTagsRequest request, Action<SetGameServerInstanceTagsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/SetGameServerInstanceTags", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void SetPlayerSecret(SetPlayerSecretRequest request, Action<SetPlayerSecretResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/SetPlayerSecret", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void SetPublisherData(SetPublisherDataRequest request, Action<SetPublisherDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/SetPublisherData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void SetTitleData(SetTitleDataRequest request, Action<SetTitleDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/SetTitleData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void SetTitleInternalData(SetTitleDataRequest request, Action<SetTitleDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/SetTitleInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void SubtractCharacterVirtualCurrency(SubtractCharacterVirtualCurrencyRequest request, Action<ModifyCharacterVirtualCurrencyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/SubtractCharacterVirtualCurrency", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void SubtractUserVirtualCurrency(SubtractUserVirtualCurrencyRequest request, Action<ModifyUserVirtualCurrencyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/SubtractUserVirtualCurrency", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void UnlockContainerInstance(UnlockContainerInstanceRequest request, Action<UnlockContainerItemResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/UnlockContainerInstance", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void UnlockContainerItem(UnlockContainerItemRequest request, Action<UnlockContainerItemResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/UnlockContainerItem", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void UpdateAvatarUrl(UpdateAvatarUrlRequest request, Action<EmptyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/UpdateAvatarUrl", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void UpdateBans(UpdateBansRequest request, Action<UpdateBansResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/UpdateBans", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void UpdateCharacterData(UpdateCharacterDataRequest request, Action<UpdateCharacterDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/UpdateCharacterData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void UpdateCharacterInternalData(UpdateCharacterDataRequest request, Action<UpdateCharacterDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/UpdateCharacterInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void UpdateCharacterReadOnlyData(UpdateCharacterDataRequest request, Action<UpdateCharacterDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/UpdateCharacterReadOnlyData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void UpdateCharacterStatistics(UpdateCharacterStatisticsRequest request, Action<UpdateCharacterStatisticsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/UpdateCharacterStatistics", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void UpdatePlayerStatistics(UpdatePlayerStatisticsRequest request, Action<UpdatePlayerStatisticsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/UpdatePlayerStatistics", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void UpdateSharedGroupData(UpdateSharedGroupDataRequest request, Action<UpdateSharedGroupDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/UpdateSharedGroupData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void UpdateUserData(UpdateUserDataRequest request, Action<UpdateUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/UpdateUserData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void UpdateUserInternalData(UpdateUserInternalDataRequest request, Action<UpdateUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/UpdateUserInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void UpdateUserInventoryItemCustomData(UpdateUserInventoryItemDataRequest request, Action<EmptyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/UpdateUserInventoryItemCustomData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void UpdateUserPublisherData(UpdateUserDataRequest request, Action<UpdateUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/UpdateUserPublisherData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void UpdateUserPublisherInternalData(UpdateUserInternalDataRequest request, Action<UpdateUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/UpdateUserPublisherInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void UpdateUserPublisherReadOnlyData(UpdateUserDataRequest request, Action<UpdateUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/UpdateUserPublisherReadOnlyData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void UpdateUserReadOnlyData(UpdateUserDataRequest request, Action<UpdateUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/UpdateUserReadOnlyData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void WriteCharacterEvent(WriteServerCharacterEventRequest request, Action<WriteEventResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/WriteCharacterEvent", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void WritePlayerEvent(WriteServerPlayerEventRequest request, Action<WriteEventResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/WritePlayerEvent", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}

		public static void WriteTitleEvent(WriteTitleEventRequest request, Action<WriteEventResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
		{
			if (PlayFabSettings.DeveloperSecretKey == null)
			{
				throw new Exception("Must have PlayFabSettings.DeveloperSecretKey set to call this method");
			}
			PlayFabHttp.MakeApiCall("/Server/WriteTitleEvent", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders);
		}
	}
}
