using PlayFab.AdminModels;
using PlayFab.ClientModels;
using PlayFab.Internal;
using PlayFab.MatchmakerModels;
using PlayFab.ServerModels;
using PlayFab.SharedModels;
using System;

namespace PlayFab.Events
{
	public class PlayFabEvents
	{
		public delegate void PlayFabErrorEvent(PlayFabRequestCommon request, PlayFabError error);

		public delegate void PlayFabResultEvent<in TResult>(TResult result) where TResult : PlayFabResultCommon;

		public delegate void PlayFabRequestEvent<in TRequest>(TRequest request) where TRequest : PlayFabRequestCommon;

		private static PlayFabEvents _instance;

		public event PlayFabRequestEvent<AbortTaskInstanceRequest> OnAdminAbortTaskInstanceRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.EmptyResult> OnAdminAbortTaskInstanceResultEvent;

		public event PlayFabRequestEvent<AddNewsRequest> OnAdminAddNewsRequestEvent;

		public event PlayFabResultEvent<AddNewsResult> OnAdminAddNewsResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.AddPlayerTagRequest> OnAdminAddPlayerTagRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.AddPlayerTagResult> OnAdminAddPlayerTagResultEvent;

		public event PlayFabRequestEvent<AddServerBuildRequest> OnAdminAddServerBuildRequestEvent;

		public event PlayFabResultEvent<AddServerBuildResult> OnAdminAddServerBuildResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.AddUserVirtualCurrencyRequest> OnAdminAddUserVirtualCurrencyRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.ModifyUserVirtualCurrencyResult> OnAdminAddUserVirtualCurrencyResultEvent;

		public event PlayFabRequestEvent<AddVirtualCurrencyTypesRequest> OnAdminAddVirtualCurrencyTypesRequestEvent;

		public event PlayFabResultEvent<BlankResult> OnAdminAddVirtualCurrencyTypesResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.BanUsersRequest> OnAdminBanUsersRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.BanUsersResult> OnAdminBanUsersResultEvent;

		public event PlayFabRequestEvent<CheckLimitedEditionItemAvailabilityRequest> OnAdminCheckLimitedEditionItemAvailabilityRequestEvent;

		public event PlayFabResultEvent<CheckLimitedEditionItemAvailabilityResult> OnAdminCheckLimitedEditionItemAvailabilityResultEvent;

		public event PlayFabRequestEvent<CreateActionsOnPlayerSegmentTaskRequest> OnAdminCreateActionsOnPlayersInSegmentTaskRequestEvent;

		public event PlayFabResultEvent<CreateTaskResult> OnAdminCreateActionsOnPlayersInSegmentTaskResultEvent;

		public event PlayFabRequestEvent<CreateCloudScriptTaskRequest> OnAdminCreateCloudScriptTaskRequestEvent;

		public event PlayFabResultEvent<CreateTaskResult> OnAdminCreateCloudScriptTaskResultEvent;

		public event PlayFabRequestEvent<CreatePlayerSharedSecretRequest> OnAdminCreatePlayerSharedSecretRequestEvent;

		public event PlayFabResultEvent<CreatePlayerSharedSecretResult> OnAdminCreatePlayerSharedSecretResultEvent;

		public event PlayFabRequestEvent<CreatePlayerStatisticDefinitionRequest> OnAdminCreatePlayerStatisticDefinitionRequestEvent;

		public event PlayFabResultEvent<CreatePlayerStatisticDefinitionResult> OnAdminCreatePlayerStatisticDefinitionResultEvent;

		public event PlayFabRequestEvent<DeleteContentRequest> OnAdminDeleteContentRequestEvent;

		public event PlayFabResultEvent<BlankResult> OnAdminDeleteContentResultEvent;

		public event PlayFabRequestEvent<DeletePlayerRequest> OnAdminDeletePlayerRequestEvent;

		public event PlayFabResultEvent<DeletePlayerResult> OnAdminDeletePlayerResultEvent;

		public event PlayFabRequestEvent<DeletePlayerSharedSecretRequest> OnAdminDeletePlayerSharedSecretRequestEvent;

		public event PlayFabResultEvent<DeletePlayerSharedSecretResult> OnAdminDeletePlayerSharedSecretResultEvent;

		public event PlayFabRequestEvent<DeleteStoreRequest> OnAdminDeleteStoreRequestEvent;

		public event PlayFabResultEvent<DeleteStoreResult> OnAdminDeleteStoreResultEvent;

		public event PlayFabRequestEvent<DeleteTaskRequest> OnAdminDeleteTaskRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.EmptyResult> OnAdminDeleteTaskResultEvent;

		public event PlayFabRequestEvent<DeleteTitleRequest> OnAdminDeleteTitleRequestEvent;

		public event PlayFabResultEvent<DeleteTitleResult> OnAdminDeleteTitleResultEvent;

		public event PlayFabRequestEvent<GetTaskInstanceRequest> OnAdminGetActionsOnPlayersInSegmentTaskInstanceRequestEvent;

		public event PlayFabResultEvent<GetActionsOnPlayersInSegmentTaskInstanceResult> OnAdminGetActionsOnPlayersInSegmentTaskInstanceResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetAllSegmentsRequest> OnAdminGetAllSegmentsRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetAllSegmentsResult> OnAdminGetAllSegmentsResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetCatalogItemsRequest> OnAdminGetCatalogItemsRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetCatalogItemsResult> OnAdminGetCatalogItemsResultEvent;

		public event PlayFabRequestEvent<GetCloudScriptRevisionRequest> OnAdminGetCloudScriptRevisionRequestEvent;

		public event PlayFabResultEvent<GetCloudScriptRevisionResult> OnAdminGetCloudScriptRevisionResultEvent;

		public event PlayFabRequestEvent<GetTaskInstanceRequest> OnAdminGetCloudScriptTaskInstanceRequestEvent;

		public event PlayFabResultEvent<GetCloudScriptTaskInstanceResult> OnAdminGetCloudScriptTaskInstanceResultEvent;

		public event PlayFabRequestEvent<GetCloudScriptVersionsRequest> OnAdminGetCloudScriptVersionsRequestEvent;

		public event PlayFabResultEvent<GetCloudScriptVersionsResult> OnAdminGetCloudScriptVersionsResultEvent;

		public event PlayFabRequestEvent<GetContentListRequest> OnAdminGetContentListRequestEvent;

		public event PlayFabResultEvent<GetContentListResult> OnAdminGetContentListResultEvent;

		public event PlayFabRequestEvent<GetContentUploadUrlRequest> OnAdminGetContentUploadUrlRequestEvent;

		public event PlayFabResultEvent<GetContentUploadUrlResult> OnAdminGetContentUploadUrlResultEvent;

		public event PlayFabRequestEvent<GetDataReportRequest> OnAdminGetDataReportRequestEvent;

		public event PlayFabResultEvent<GetDataReportResult> OnAdminGetDataReportResultEvent;

		public event PlayFabRequestEvent<GetMatchmakerGameInfoRequest> OnAdminGetMatchmakerGameInfoRequestEvent;

		public event PlayFabResultEvent<GetMatchmakerGameInfoResult> OnAdminGetMatchmakerGameInfoResultEvent;

		public event PlayFabRequestEvent<GetMatchmakerGameModesRequest> OnAdminGetMatchmakerGameModesRequestEvent;

		public event PlayFabResultEvent<GetMatchmakerGameModesResult> OnAdminGetMatchmakerGameModesResultEvent;

		public event PlayFabRequestEvent<GetPlayerIdFromAuthTokenRequest> OnAdminGetPlayerIdFromAuthTokenRequestEvent;

		public event PlayFabResultEvent<GetPlayerIdFromAuthTokenResult> OnAdminGetPlayerIdFromAuthTokenResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetPlayerProfileRequest> OnAdminGetPlayerProfileRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetPlayerProfileResult> OnAdminGetPlayerProfileResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetPlayersSegmentsRequest> OnAdminGetPlayerSegmentsRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetPlayerSegmentsResult> OnAdminGetPlayerSegmentsResultEvent;

		public event PlayFabRequestEvent<GetPlayerSharedSecretsRequest> OnAdminGetPlayerSharedSecretsRequestEvent;

		public event PlayFabResultEvent<GetPlayerSharedSecretsResult> OnAdminGetPlayerSharedSecretsResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetPlayersInSegmentRequest> OnAdminGetPlayersInSegmentRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetPlayersInSegmentResult> OnAdminGetPlayersInSegmentResultEvent;

		public event PlayFabRequestEvent<GetPlayerStatisticDefinitionsRequest> OnAdminGetPlayerStatisticDefinitionsRequestEvent;

		public event PlayFabResultEvent<GetPlayerStatisticDefinitionsResult> OnAdminGetPlayerStatisticDefinitionsResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetPlayerStatisticVersionsRequest> OnAdminGetPlayerStatisticVersionsRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetPlayerStatisticVersionsResult> OnAdminGetPlayerStatisticVersionsResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetPlayerTagsRequest> OnAdminGetPlayerTagsRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetPlayerTagsResult> OnAdminGetPlayerTagsResultEvent;

		public event PlayFabRequestEvent<GetPolicyRequest> OnAdminGetPolicyRequestEvent;

		public event PlayFabResultEvent<GetPolicyResponse> OnAdminGetPolicyResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetPublisherDataRequest> OnAdminGetPublisherDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetPublisherDataResult> OnAdminGetPublisherDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetRandomResultTablesRequest> OnAdminGetRandomResultTablesRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetRandomResultTablesResult> OnAdminGetRandomResultTablesResultEvent;

		public event PlayFabRequestEvent<GetServerBuildInfoRequest> OnAdminGetServerBuildInfoRequestEvent;

		public event PlayFabResultEvent<GetServerBuildInfoResult> OnAdminGetServerBuildInfoResultEvent;

		public event PlayFabRequestEvent<GetServerBuildUploadURLRequest> OnAdminGetServerBuildUploadUrlRequestEvent;

		public event PlayFabResultEvent<GetServerBuildUploadURLResult> OnAdminGetServerBuildUploadUrlResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetStoreItemsRequest> OnAdminGetStoreItemsRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetStoreItemsResult> OnAdminGetStoreItemsResultEvent;

		public event PlayFabRequestEvent<GetTaskInstancesRequest> OnAdminGetTaskInstancesRequestEvent;

		public event PlayFabResultEvent<GetTaskInstancesResult> OnAdminGetTaskInstancesResultEvent;

		public event PlayFabRequestEvent<GetTasksRequest> OnAdminGetTasksRequestEvent;

		public event PlayFabResultEvent<GetTasksResult> OnAdminGetTasksResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetTitleDataRequest> OnAdminGetTitleDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetTitleDataResult> OnAdminGetTitleDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetTitleDataRequest> OnAdminGetTitleInternalDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetTitleDataResult> OnAdminGetTitleInternalDataResultEvent;

		public event PlayFabRequestEvent<LookupUserAccountInfoRequest> OnAdminGetUserAccountInfoRequestEvent;

		public event PlayFabResultEvent<LookupUserAccountInfoResult> OnAdminGetUserAccountInfoResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetUserBansRequest> OnAdminGetUserBansRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetUserBansResult> OnAdminGetUserBansResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetUserDataRequest> OnAdminGetUserDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetUserDataResult> OnAdminGetUserDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetUserDataRequest> OnAdminGetUserInternalDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetUserDataResult> OnAdminGetUserInternalDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetUserInventoryRequest> OnAdminGetUserInventoryRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetUserInventoryResult> OnAdminGetUserInventoryResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetUserDataRequest> OnAdminGetUserPublisherDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetUserDataResult> OnAdminGetUserPublisherDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetUserDataRequest> OnAdminGetUserPublisherInternalDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetUserDataResult> OnAdminGetUserPublisherInternalDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetUserDataRequest> OnAdminGetUserPublisherReadOnlyDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetUserDataResult> OnAdminGetUserPublisherReadOnlyDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GetUserDataRequest> OnAdminGetUserReadOnlyDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GetUserDataResult> OnAdminGetUserReadOnlyDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.GrantItemsToUsersRequest> OnAdminGrantItemsToUsersRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.GrantItemsToUsersResult> OnAdminGrantItemsToUsersResultEvent;

		public event PlayFabRequestEvent<IncrementLimitedEditionItemAvailabilityRequest> OnAdminIncrementLimitedEditionItemAvailabilityRequestEvent;

		public event PlayFabResultEvent<IncrementLimitedEditionItemAvailabilityResult> OnAdminIncrementLimitedEditionItemAvailabilityResultEvent;

		public event PlayFabRequestEvent<IncrementPlayerStatisticVersionRequest> OnAdminIncrementPlayerStatisticVersionRequestEvent;

		public event PlayFabResultEvent<IncrementPlayerStatisticVersionResult> OnAdminIncrementPlayerStatisticVersionResultEvent;

		public event PlayFabRequestEvent<ListBuildsRequest> OnAdminListServerBuildsRequestEvent;

		public event PlayFabResultEvent<ListBuildsResult> OnAdminListServerBuildsResultEvent;

		public event PlayFabRequestEvent<ListVirtualCurrencyTypesRequest> OnAdminListVirtualCurrencyTypesRequestEvent;

		public event PlayFabResultEvent<ListVirtualCurrencyTypesResult> OnAdminListVirtualCurrencyTypesResultEvent;

		public event PlayFabRequestEvent<ModifyMatchmakerGameModesRequest> OnAdminModifyMatchmakerGameModesRequestEvent;

		public event PlayFabResultEvent<ModifyMatchmakerGameModesResult> OnAdminModifyMatchmakerGameModesResultEvent;

		public event PlayFabRequestEvent<ModifyServerBuildRequest> OnAdminModifyServerBuildRequestEvent;

		public event PlayFabResultEvent<ModifyServerBuildResult> OnAdminModifyServerBuildResultEvent;

		public event PlayFabRequestEvent<RefundPurchaseRequest> OnAdminRefundPurchaseRequestEvent;

		public event PlayFabResultEvent<RefundPurchaseResponse> OnAdminRefundPurchaseResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.RemovePlayerTagRequest> OnAdminRemovePlayerTagRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.RemovePlayerTagResult> OnAdminRemovePlayerTagResultEvent;

		public event PlayFabRequestEvent<RemoveServerBuildRequest> OnAdminRemoveServerBuildRequestEvent;

		public event PlayFabResultEvent<RemoveServerBuildResult> OnAdminRemoveServerBuildResultEvent;

		public event PlayFabRequestEvent<RemoveVirtualCurrencyTypesRequest> OnAdminRemoveVirtualCurrencyTypesRequestEvent;

		public event PlayFabResultEvent<BlankResult> OnAdminRemoveVirtualCurrencyTypesResultEvent;

		public event PlayFabRequestEvent<ResetCharacterStatisticsRequest> OnAdminResetCharacterStatisticsRequestEvent;

		public event PlayFabResultEvent<ResetCharacterStatisticsResult> OnAdminResetCharacterStatisticsResultEvent;

		public event PlayFabRequestEvent<ResetPasswordRequest> OnAdminResetPasswordRequestEvent;

		public event PlayFabResultEvent<ResetPasswordResult> OnAdminResetPasswordResultEvent;

		public event PlayFabRequestEvent<ResetUserStatisticsRequest> OnAdminResetUserStatisticsRequestEvent;

		public event PlayFabResultEvent<ResetUserStatisticsResult> OnAdminResetUserStatisticsResultEvent;

		public event PlayFabRequestEvent<ResolvePurchaseDisputeRequest> OnAdminResolvePurchaseDisputeRequestEvent;

		public event PlayFabResultEvent<ResolvePurchaseDisputeResponse> OnAdminResolvePurchaseDisputeResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.RevokeAllBansForUserRequest> OnAdminRevokeAllBansForUserRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.RevokeAllBansForUserResult> OnAdminRevokeAllBansForUserResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.RevokeBansRequest> OnAdminRevokeBansRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.RevokeBansResult> OnAdminRevokeBansResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.RevokeInventoryItemRequest> OnAdminRevokeInventoryItemRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.RevokeInventoryResult> OnAdminRevokeInventoryItemResultEvent;

		public event PlayFabRequestEvent<RunTaskRequest> OnAdminRunTaskRequestEvent;

		public event PlayFabResultEvent<RunTaskResult> OnAdminRunTaskResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.SendAccountRecoveryEmailRequest> OnAdminSendAccountRecoveryEmailRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.SendAccountRecoveryEmailResult> OnAdminSendAccountRecoveryEmailResultEvent;

		public event PlayFabRequestEvent<UpdateCatalogItemsRequest> OnAdminSetCatalogItemsRequestEvent;

		public event PlayFabResultEvent<UpdateCatalogItemsResult> OnAdminSetCatalogItemsResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.SetPlayerSecretRequest> OnAdminSetPlayerSecretRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.SetPlayerSecretResult> OnAdminSetPlayerSecretResultEvent;

		public event PlayFabRequestEvent<SetPublishedRevisionRequest> OnAdminSetPublishedRevisionRequestEvent;

		public event PlayFabResultEvent<SetPublishedRevisionResult> OnAdminSetPublishedRevisionResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.SetPublisherDataRequest> OnAdminSetPublisherDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.SetPublisherDataResult> OnAdminSetPublisherDataResultEvent;

		public event PlayFabRequestEvent<UpdateStoreItemsRequest> OnAdminSetStoreItemsRequestEvent;

		public event PlayFabResultEvent<UpdateStoreItemsResult> OnAdminSetStoreItemsResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.SetTitleDataRequest> OnAdminSetTitleDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.SetTitleDataResult> OnAdminSetTitleDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.SetTitleDataRequest> OnAdminSetTitleInternalDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.SetTitleDataResult> OnAdminSetTitleInternalDataResultEvent;

		public event PlayFabRequestEvent<SetupPushNotificationRequest> OnAdminSetupPushNotificationRequestEvent;

		public event PlayFabResultEvent<SetupPushNotificationResult> OnAdminSetupPushNotificationResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.SubtractUserVirtualCurrencyRequest> OnAdminSubtractUserVirtualCurrencyRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.ModifyUserVirtualCurrencyResult> OnAdminSubtractUserVirtualCurrencyResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.UpdateBansRequest> OnAdminUpdateBansRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.UpdateBansResult> OnAdminUpdateBansResultEvent;

		public event PlayFabRequestEvent<UpdateCatalogItemsRequest> OnAdminUpdateCatalogItemsRequestEvent;

		public event PlayFabResultEvent<UpdateCatalogItemsResult> OnAdminUpdateCatalogItemsResultEvent;

		public event PlayFabRequestEvent<UpdateCloudScriptRequest> OnAdminUpdateCloudScriptRequestEvent;

		public event PlayFabResultEvent<UpdateCloudScriptResult> OnAdminUpdateCloudScriptResultEvent;

		public event PlayFabRequestEvent<UpdatePlayerSharedSecretRequest> OnAdminUpdatePlayerSharedSecretRequestEvent;

		public event PlayFabResultEvent<UpdatePlayerSharedSecretResult> OnAdminUpdatePlayerSharedSecretResultEvent;

		public event PlayFabRequestEvent<UpdatePlayerStatisticDefinitionRequest> OnAdminUpdatePlayerStatisticDefinitionRequestEvent;

		public event PlayFabResultEvent<UpdatePlayerStatisticDefinitionResult> OnAdminUpdatePlayerStatisticDefinitionResultEvent;

		public event PlayFabRequestEvent<UpdatePolicyRequest> OnAdminUpdatePolicyRequestEvent;

		public event PlayFabResultEvent<UpdatePolicyResponse> OnAdminUpdatePolicyResultEvent;

		public event PlayFabRequestEvent<UpdateRandomResultTablesRequest> OnAdminUpdateRandomResultTablesRequestEvent;

		public event PlayFabResultEvent<UpdateRandomResultTablesResult> OnAdminUpdateRandomResultTablesResultEvent;

		public event PlayFabRequestEvent<UpdateStoreItemsRequest> OnAdminUpdateStoreItemsRequestEvent;

		public event PlayFabResultEvent<UpdateStoreItemsResult> OnAdminUpdateStoreItemsResultEvent;

		public event PlayFabRequestEvent<UpdateTaskRequest> OnAdminUpdateTaskRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.EmptyResult> OnAdminUpdateTaskResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.UpdateUserDataRequest> OnAdminUpdateUserDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.UpdateUserDataResult> OnAdminUpdateUserDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.UpdateUserInternalDataRequest> OnAdminUpdateUserInternalDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.UpdateUserDataResult> OnAdminUpdateUserInternalDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.UpdateUserDataRequest> OnAdminUpdateUserPublisherDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.UpdateUserDataResult> OnAdminUpdateUserPublisherDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.UpdateUserInternalDataRequest> OnAdminUpdateUserPublisherInternalDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.UpdateUserDataResult> OnAdminUpdateUserPublisherInternalDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.UpdateUserDataRequest> OnAdminUpdateUserPublisherReadOnlyDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.UpdateUserDataResult> OnAdminUpdateUserPublisherReadOnlyDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.UpdateUserDataRequest> OnAdminUpdateUserReadOnlyDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.UpdateUserDataResult> OnAdminUpdateUserReadOnlyDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.AdminModels.UpdateUserTitleDisplayNameRequest> OnAdminUpdateUserTitleDisplayNameRequestEvent;

		public event PlayFabResultEvent<PlayFab.AdminModels.UpdateUserTitleDisplayNameResult> OnAdminUpdateUserTitleDisplayNameResultEvent;

		public event PlayFabResultEvent<LoginResult> OnLoginResultEvent;

		public event PlayFabRequestEvent<AcceptTradeRequest> OnAcceptTradeRequestEvent;

		public event PlayFabResultEvent<AcceptTradeResponse> OnAcceptTradeResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.AddFriendRequest> OnAddFriendRequestEvent;

		public event PlayFabResultEvent<AddFriendResult> OnAddFriendResultEvent;

		public event PlayFabRequestEvent<AddGenericIDRequest> OnAddGenericIDRequestEvent;

		public event PlayFabResultEvent<AddGenericIDResult> OnAddGenericIDResultEvent;

		public event PlayFabRequestEvent<AddOrUpdateContactEmailRequest> OnAddOrUpdateContactEmailRequestEvent;

		public event PlayFabResultEvent<AddOrUpdateContactEmailResult> OnAddOrUpdateContactEmailResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.AddSharedGroupMembersRequest> OnAddSharedGroupMembersRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.AddSharedGroupMembersResult> OnAddSharedGroupMembersResultEvent;

		public event PlayFabRequestEvent<AddUsernamePasswordRequest> OnAddUsernamePasswordRequestEvent;

		public event PlayFabResultEvent<AddUsernamePasswordResult> OnAddUsernamePasswordResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.AddUserVirtualCurrencyRequest> OnAddUserVirtualCurrencyRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.ModifyUserVirtualCurrencyResult> OnAddUserVirtualCurrencyResultEvent;

		public event PlayFabRequestEvent<AndroidDevicePushNotificationRegistrationRequest> OnAndroidDevicePushNotificationRegistrationRequestEvent;

		public event PlayFabResultEvent<AndroidDevicePushNotificationRegistrationResult> OnAndroidDevicePushNotificationRegistrationResultEvent;

		public event PlayFabRequestEvent<AttributeInstallRequest> OnAttributeInstallRequestEvent;

		public event PlayFabResultEvent<AttributeInstallResult> OnAttributeInstallResultEvent;

		public event PlayFabRequestEvent<CancelTradeRequest> OnCancelTradeRequestEvent;

		public event PlayFabResultEvent<CancelTradeResponse> OnCancelTradeResultEvent;

		public event PlayFabRequestEvent<ConfirmPurchaseRequest> OnConfirmPurchaseRequestEvent;

		public event PlayFabResultEvent<ConfirmPurchaseResult> OnConfirmPurchaseResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.ConsumeItemRequest> OnConsumeItemRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.ConsumeItemResult> OnConsumeItemResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.CreateSharedGroupRequest> OnCreateSharedGroupRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.CreateSharedGroupResult> OnCreateSharedGroupResultEvent;

		public event PlayFabRequestEvent<ExecuteCloudScriptRequest> OnExecuteCloudScriptRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.ExecuteCloudScriptResult> OnExecuteCloudScriptResultEvent;

		public event PlayFabRequestEvent<GetAccountInfoRequest> OnGetAccountInfoRequestEvent;

		public event PlayFabResultEvent<GetAccountInfoResult> OnGetAccountInfoResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.ListUsersCharactersRequest> OnGetAllUsersCharactersRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.ListUsersCharactersResult> OnGetAllUsersCharactersResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetCatalogItemsRequest> OnGetCatalogItemsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetCatalogItemsResult> OnGetCatalogItemsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetCharacterDataRequest> OnGetCharacterDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetCharacterDataResult> OnGetCharacterDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetCharacterInventoryRequest> OnGetCharacterInventoryRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetCharacterInventoryResult> OnGetCharacterInventoryResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetCharacterLeaderboardRequest> OnGetCharacterLeaderboardRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetCharacterLeaderboardResult> OnGetCharacterLeaderboardResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetCharacterDataRequest> OnGetCharacterReadOnlyDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetCharacterDataResult> OnGetCharacterReadOnlyDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetCharacterStatisticsRequest> OnGetCharacterStatisticsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetCharacterStatisticsResult> OnGetCharacterStatisticsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetContentDownloadUrlRequest> OnGetContentDownloadUrlRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetContentDownloadUrlResult> OnGetContentDownloadUrlResultEvent;

		public event PlayFabRequestEvent<CurrentGamesRequest> OnGetCurrentGamesRequestEvent;

		public event PlayFabResultEvent<CurrentGamesResult> OnGetCurrentGamesResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetFriendLeaderboardRequest> OnGetFriendLeaderboardRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetLeaderboardResult> OnGetFriendLeaderboardResultEvent;

		public event PlayFabRequestEvent<GetFriendLeaderboardAroundPlayerRequest> OnGetFriendLeaderboardAroundPlayerRequestEvent;

		public event PlayFabResultEvent<GetFriendLeaderboardAroundPlayerResult> OnGetFriendLeaderboardAroundPlayerResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetFriendsListRequest> OnGetFriendsListRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetFriendsListResult> OnGetFriendsListResultEvent;

		public event PlayFabRequestEvent<GameServerRegionsRequest> OnGetGameServerRegionsRequestEvent;

		public event PlayFabResultEvent<GameServerRegionsResult> OnGetGameServerRegionsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetLeaderboardRequest> OnGetLeaderboardRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetLeaderboardResult> OnGetLeaderboardResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetLeaderboardAroundCharacterRequest> OnGetLeaderboardAroundCharacterRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetLeaderboardAroundCharacterResult> OnGetLeaderboardAroundCharacterResultEvent;

		public event PlayFabRequestEvent<GetLeaderboardAroundPlayerRequest> OnGetLeaderboardAroundPlayerRequestEvent;

		public event PlayFabResultEvent<GetLeaderboardAroundPlayerResult> OnGetLeaderboardAroundPlayerResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetLeaderboardForUsersCharactersRequest> OnGetLeaderboardForUserCharactersRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetLeaderboardForUsersCharactersResult> OnGetLeaderboardForUserCharactersResultEvent;

		public event PlayFabRequestEvent<GetPaymentTokenRequest> OnGetPaymentTokenRequestEvent;

		public event PlayFabResultEvent<GetPaymentTokenResult> OnGetPaymentTokenResultEvent;

		public event PlayFabRequestEvent<GetPhotonAuthenticationTokenRequest> OnGetPhotonAuthenticationTokenRequestEvent;

		public event PlayFabResultEvent<GetPhotonAuthenticationTokenResult> OnGetPhotonAuthenticationTokenResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetPlayerCombinedInfoRequest> OnGetPlayerCombinedInfoRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetPlayerCombinedInfoResult> OnGetPlayerCombinedInfoResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetPlayerProfileRequest> OnGetPlayerProfileRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetPlayerProfileResult> OnGetPlayerProfileResultEvent;

		public event PlayFabRequestEvent<GetPlayerSegmentsRequest> OnGetPlayerSegmentsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetPlayerSegmentsResult> OnGetPlayerSegmentsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetPlayerStatisticsRequest> OnGetPlayerStatisticsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetPlayerStatisticsResult> OnGetPlayerStatisticsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetPlayerStatisticVersionsRequest> OnGetPlayerStatisticVersionsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetPlayerStatisticVersionsResult> OnGetPlayerStatisticVersionsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetPlayerTagsRequest> OnGetPlayerTagsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetPlayerTagsResult> OnGetPlayerTagsResultEvent;

		public event PlayFabRequestEvent<GetPlayerTradesRequest> OnGetPlayerTradesRequestEvent;

		public event PlayFabResultEvent<GetPlayerTradesResponse> OnGetPlayerTradesResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetPlayFabIDsFromFacebookIDsRequest> OnGetPlayFabIDsFromFacebookIDsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetPlayFabIDsFromFacebookIDsResult> OnGetPlayFabIDsFromFacebookIDsResultEvent;

		public event PlayFabRequestEvent<GetPlayFabIDsFromGameCenterIDsRequest> OnGetPlayFabIDsFromGameCenterIDsRequestEvent;

		public event PlayFabResultEvent<GetPlayFabIDsFromGameCenterIDsResult> OnGetPlayFabIDsFromGameCenterIDsResultEvent;

		public event PlayFabRequestEvent<GetPlayFabIDsFromGenericIDsRequest> OnGetPlayFabIDsFromGenericIDsRequestEvent;

		public event PlayFabResultEvent<GetPlayFabIDsFromGenericIDsResult> OnGetPlayFabIDsFromGenericIDsResultEvent;

		public event PlayFabRequestEvent<GetPlayFabIDsFromGoogleIDsRequest> OnGetPlayFabIDsFromGoogleIDsRequestEvent;

		public event PlayFabResultEvent<GetPlayFabIDsFromGoogleIDsResult> OnGetPlayFabIDsFromGoogleIDsResultEvent;

		public event PlayFabRequestEvent<GetPlayFabIDsFromKongregateIDsRequest> OnGetPlayFabIDsFromKongregateIDsRequestEvent;

		public event PlayFabResultEvent<GetPlayFabIDsFromKongregateIDsResult> OnGetPlayFabIDsFromKongregateIDsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetPlayFabIDsFromSteamIDsRequest> OnGetPlayFabIDsFromSteamIDsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetPlayFabIDsFromSteamIDsResult> OnGetPlayFabIDsFromSteamIDsResultEvent;

		public event PlayFabRequestEvent<GetPlayFabIDsFromTwitchIDsRequest> OnGetPlayFabIDsFromTwitchIDsRequestEvent;

		public event PlayFabResultEvent<GetPlayFabIDsFromTwitchIDsResult> OnGetPlayFabIDsFromTwitchIDsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetPublisherDataRequest> OnGetPublisherDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetPublisherDataResult> OnGetPublisherDataResultEvent;

		public event PlayFabRequestEvent<GetPurchaseRequest> OnGetPurchaseRequestEvent;

		public event PlayFabResultEvent<GetPurchaseResult> OnGetPurchaseResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetSharedGroupDataRequest> OnGetSharedGroupDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetSharedGroupDataResult> OnGetSharedGroupDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetStoreItemsRequest> OnGetStoreItemsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetStoreItemsResult> OnGetStoreItemsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetTimeRequest> OnGetTimeRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetTimeResult> OnGetTimeResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetTitleDataRequest> OnGetTitleDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetTitleDataResult> OnGetTitleDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetTitleNewsRequest> OnGetTitleNewsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetTitleNewsResult> OnGetTitleNewsResultEvent;

		public event PlayFabRequestEvent<GetTitlePublicKeyRequest> OnGetTitlePublicKeyRequestEvent;

		public event PlayFabResultEvent<GetTitlePublicKeyResult> OnGetTitlePublicKeyResultEvent;

		public event PlayFabRequestEvent<GetTradeStatusRequest> OnGetTradeStatusRequestEvent;

		public event PlayFabResultEvent<GetTradeStatusResponse> OnGetTradeStatusResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetUserDataRequest> OnGetUserDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetUserDataResult> OnGetUserDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetUserInventoryRequest> OnGetUserInventoryRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetUserInventoryResult> OnGetUserInventoryResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetUserDataRequest> OnGetUserPublisherDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetUserDataResult> OnGetUserPublisherDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetUserDataRequest> OnGetUserPublisherReadOnlyDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetUserDataResult> OnGetUserPublisherReadOnlyDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GetUserDataRequest> OnGetUserReadOnlyDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GetUserDataResult> OnGetUserReadOnlyDataResultEvent;

		public event PlayFabRequestEvent<GetWindowsHelloChallengeRequest> OnGetWindowsHelloChallengeRequestEvent;

		public event PlayFabResultEvent<GetWindowsHelloChallengeResponse> OnGetWindowsHelloChallengeResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.GrantCharacterToUserRequest> OnGrantCharacterToUserRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.GrantCharacterToUserResult> OnGrantCharacterToUserResultEvent;

		public event PlayFabRequestEvent<LinkAndroidDeviceIDRequest> OnLinkAndroidDeviceIDRequestEvent;

		public event PlayFabResultEvent<LinkAndroidDeviceIDResult> OnLinkAndroidDeviceIDResultEvent;

		public event PlayFabRequestEvent<LinkCustomIDRequest> OnLinkCustomIDRequestEvent;

		public event PlayFabResultEvent<LinkCustomIDResult> OnLinkCustomIDResultEvent;

		public event PlayFabRequestEvent<LinkFacebookAccountRequest> OnLinkFacebookAccountRequestEvent;

		public event PlayFabResultEvent<LinkFacebookAccountResult> OnLinkFacebookAccountResultEvent;

		public event PlayFabRequestEvent<LinkGameCenterAccountRequest> OnLinkGameCenterAccountRequestEvent;

		public event PlayFabResultEvent<LinkGameCenterAccountResult> OnLinkGameCenterAccountResultEvent;

		public event PlayFabRequestEvent<LinkGoogleAccountRequest> OnLinkGoogleAccountRequestEvent;

		public event PlayFabResultEvent<LinkGoogleAccountResult> OnLinkGoogleAccountResultEvent;

		public event PlayFabRequestEvent<LinkIOSDeviceIDRequest> OnLinkIOSDeviceIDRequestEvent;

		public event PlayFabResultEvent<LinkIOSDeviceIDResult> OnLinkIOSDeviceIDResultEvent;

		public event PlayFabRequestEvent<LinkKongregateAccountRequest> OnLinkKongregateRequestEvent;

		public event PlayFabResultEvent<LinkKongregateAccountResult> OnLinkKongregateResultEvent;

		public event PlayFabRequestEvent<LinkSteamAccountRequest> OnLinkSteamAccountRequestEvent;

		public event PlayFabResultEvent<LinkSteamAccountResult> OnLinkSteamAccountResultEvent;

		public event PlayFabRequestEvent<LinkTwitchAccountRequest> OnLinkTwitchRequestEvent;

		public event PlayFabResultEvent<LinkTwitchAccountResult> OnLinkTwitchResultEvent;

		public event PlayFabRequestEvent<LinkWindowsHelloAccountRequest> OnLinkWindowsHelloRequestEvent;

		public event PlayFabResultEvent<LinkWindowsHelloAccountResponse> OnLinkWindowsHelloResultEvent;

		public event PlayFabRequestEvent<LoginWithAndroidDeviceIDRequest> OnLoginWithAndroidDeviceIDRequestEvent;

		public event PlayFabRequestEvent<LoginWithCustomIDRequest> OnLoginWithCustomIDRequestEvent;

		public event PlayFabRequestEvent<LoginWithEmailAddressRequest> OnLoginWithEmailAddressRequestEvent;

		public event PlayFabRequestEvent<LoginWithFacebookRequest> OnLoginWithFacebookRequestEvent;

		public event PlayFabRequestEvent<LoginWithGameCenterRequest> OnLoginWithGameCenterRequestEvent;

		public event PlayFabRequestEvent<LoginWithGoogleAccountRequest> OnLoginWithGoogleAccountRequestEvent;

		public event PlayFabRequestEvent<LoginWithIOSDeviceIDRequest> OnLoginWithIOSDeviceIDRequestEvent;

		public event PlayFabRequestEvent<LoginWithKongregateRequest> OnLoginWithKongregateRequestEvent;

		public event PlayFabRequestEvent<LoginWithPlayFabRequest> OnLoginWithPlayFabRequestEvent;

		public event PlayFabRequestEvent<LoginWithSteamRequest> OnLoginWithSteamRequestEvent;

		public event PlayFabRequestEvent<LoginWithTwitchRequest> OnLoginWithTwitchRequestEvent;

		public event PlayFabRequestEvent<LoginWithWindowsHelloRequest> OnLoginWithWindowsHelloRequestEvent;

		public event PlayFabRequestEvent<MatchmakeRequest> OnMatchmakeRequestEvent;

		public event PlayFabResultEvent<MatchmakeResult> OnMatchmakeResultEvent;

		public event PlayFabRequestEvent<OpenTradeRequest> OnOpenTradeRequestEvent;

		public event PlayFabResultEvent<OpenTradeResponse> OnOpenTradeResultEvent;

		public event PlayFabRequestEvent<PayForPurchaseRequest> OnPayForPurchaseRequestEvent;

		public event PlayFabResultEvent<PayForPurchaseResult> OnPayForPurchaseResultEvent;

		public event PlayFabRequestEvent<PurchaseItemRequest> OnPurchaseItemRequestEvent;

		public event PlayFabResultEvent<PurchaseItemResult> OnPurchaseItemResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.RedeemCouponRequest> OnRedeemCouponRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.RedeemCouponResult> OnRedeemCouponResultEvent;

		public event PlayFabRequestEvent<RegisterForIOSPushNotificationRequest> OnRegisterForIOSPushNotificationRequestEvent;

		public event PlayFabResultEvent<RegisterForIOSPushNotificationResult> OnRegisterForIOSPushNotificationResultEvent;

		public event PlayFabRequestEvent<RegisterPlayFabUserRequest> OnRegisterPlayFabUserRequestEvent;

		public event PlayFabResultEvent<RegisterPlayFabUserResult> OnRegisterPlayFabUserResultEvent;

		public event PlayFabRequestEvent<RegisterWithWindowsHelloRequest> OnRegisterWithWindowsHelloRequestEvent;

		public event PlayFabRequestEvent<RemoveContactEmailRequest> OnRemoveContactEmailRequestEvent;

		public event PlayFabResultEvent<RemoveContactEmailResult> OnRemoveContactEmailResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.RemoveFriendRequest> OnRemoveFriendRequestEvent;

		public event PlayFabResultEvent<RemoveFriendResult> OnRemoveFriendResultEvent;

		public event PlayFabRequestEvent<RemoveGenericIDRequest> OnRemoveGenericIDRequestEvent;

		public event PlayFabResultEvent<RemoveGenericIDResult> OnRemoveGenericIDResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.RemoveSharedGroupMembersRequest> OnRemoveSharedGroupMembersRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.RemoveSharedGroupMembersResult> OnRemoveSharedGroupMembersResultEvent;

		public event PlayFabRequestEvent<DeviceInfoRequest> OnReportDeviceInfoRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.EmptyResult> OnReportDeviceInfoResultEvent;

		public event PlayFabRequestEvent<ReportPlayerClientRequest> OnReportPlayerRequestEvent;

		public event PlayFabResultEvent<ReportPlayerClientResult> OnReportPlayerResultEvent;

		public event PlayFabRequestEvent<RestoreIOSPurchasesRequest> OnRestoreIOSPurchasesRequestEvent;

		public event PlayFabResultEvent<RestoreIOSPurchasesResult> OnRestoreIOSPurchasesResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.SendAccountRecoveryEmailRequest> OnSendAccountRecoveryEmailRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.SendAccountRecoveryEmailResult> OnSendAccountRecoveryEmailResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.SetFriendTagsRequest> OnSetFriendTagsRequestEvent;

		public event PlayFabResultEvent<SetFriendTagsResult> OnSetFriendTagsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.SetPlayerSecretRequest> OnSetPlayerSecretRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.SetPlayerSecretResult> OnSetPlayerSecretResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.StartGameRequest> OnStartGameRequestEvent;

		public event PlayFabResultEvent<StartGameResult> OnStartGameResultEvent;

		public event PlayFabRequestEvent<StartPurchaseRequest> OnStartPurchaseRequestEvent;

		public event PlayFabResultEvent<StartPurchaseResult> OnStartPurchaseResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.SubtractUserVirtualCurrencyRequest> OnSubtractUserVirtualCurrencyRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.ModifyUserVirtualCurrencyResult> OnSubtractUserVirtualCurrencyResultEvent;

		public event PlayFabRequestEvent<UnlinkAndroidDeviceIDRequest> OnUnlinkAndroidDeviceIDRequestEvent;

		public event PlayFabResultEvent<UnlinkAndroidDeviceIDResult> OnUnlinkAndroidDeviceIDResultEvent;

		public event PlayFabRequestEvent<UnlinkCustomIDRequest> OnUnlinkCustomIDRequestEvent;

		public event PlayFabResultEvent<UnlinkCustomIDResult> OnUnlinkCustomIDResultEvent;

		public event PlayFabRequestEvent<UnlinkFacebookAccountRequest> OnUnlinkFacebookAccountRequestEvent;

		public event PlayFabResultEvent<UnlinkFacebookAccountResult> OnUnlinkFacebookAccountResultEvent;

		public event PlayFabRequestEvent<UnlinkGameCenterAccountRequest> OnUnlinkGameCenterAccountRequestEvent;

		public event PlayFabResultEvent<UnlinkGameCenterAccountResult> OnUnlinkGameCenterAccountResultEvent;

		public event PlayFabRequestEvent<UnlinkGoogleAccountRequest> OnUnlinkGoogleAccountRequestEvent;

		public event PlayFabResultEvent<UnlinkGoogleAccountResult> OnUnlinkGoogleAccountResultEvent;

		public event PlayFabRequestEvent<UnlinkIOSDeviceIDRequest> OnUnlinkIOSDeviceIDRequestEvent;

		public event PlayFabResultEvent<UnlinkIOSDeviceIDResult> OnUnlinkIOSDeviceIDResultEvent;

		public event PlayFabRequestEvent<UnlinkKongregateAccountRequest> OnUnlinkKongregateRequestEvent;

		public event PlayFabResultEvent<UnlinkKongregateAccountResult> OnUnlinkKongregateResultEvent;

		public event PlayFabRequestEvent<UnlinkSteamAccountRequest> OnUnlinkSteamAccountRequestEvent;

		public event PlayFabResultEvent<UnlinkSteamAccountResult> OnUnlinkSteamAccountResultEvent;

		public event PlayFabRequestEvent<UnlinkTwitchAccountRequest> OnUnlinkTwitchRequestEvent;

		public event PlayFabResultEvent<UnlinkTwitchAccountResult> OnUnlinkTwitchResultEvent;

		public event PlayFabRequestEvent<UnlinkWindowsHelloAccountRequest> OnUnlinkWindowsHelloRequestEvent;

		public event PlayFabResultEvent<UnlinkWindowsHelloAccountResponse> OnUnlinkWindowsHelloResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.UnlockContainerInstanceRequest> OnUnlockContainerInstanceRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.UnlockContainerItemResult> OnUnlockContainerInstanceResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.UnlockContainerItemRequest> OnUnlockContainerItemRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.UnlockContainerItemResult> OnUnlockContainerItemResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.UpdateAvatarUrlRequest> OnUpdateAvatarUrlRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.EmptyResult> OnUpdateAvatarUrlResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.UpdateCharacterDataRequest> OnUpdateCharacterDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.UpdateCharacterDataResult> OnUpdateCharacterDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.UpdateCharacterStatisticsRequest> OnUpdateCharacterStatisticsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.UpdateCharacterStatisticsResult> OnUpdateCharacterStatisticsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.UpdatePlayerStatisticsRequest> OnUpdatePlayerStatisticsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.UpdatePlayerStatisticsResult> OnUpdatePlayerStatisticsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.UpdateSharedGroupDataRequest> OnUpdateSharedGroupDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.UpdateSharedGroupDataResult> OnUpdateSharedGroupDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.UpdateUserDataRequest> OnUpdateUserDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.UpdateUserDataResult> OnUpdateUserDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.UpdateUserDataRequest> OnUpdateUserPublisherDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.UpdateUserDataResult> OnUpdateUserPublisherDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.UpdateUserTitleDisplayNameRequest> OnUpdateUserTitleDisplayNameRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.UpdateUserTitleDisplayNameResult> OnUpdateUserTitleDisplayNameResultEvent;

		public event PlayFabRequestEvent<ValidateAmazonReceiptRequest> OnValidateAmazonIAPReceiptRequestEvent;

		public event PlayFabResultEvent<ValidateAmazonReceiptResult> OnValidateAmazonIAPReceiptResultEvent;

		public event PlayFabRequestEvent<ValidateGooglePlayPurchaseRequest> OnValidateGooglePlayPurchaseRequestEvent;

		public event PlayFabResultEvent<ValidateGooglePlayPurchaseResult> OnValidateGooglePlayPurchaseResultEvent;

		public event PlayFabRequestEvent<ValidateIOSReceiptRequest> OnValidateIOSReceiptRequestEvent;

		public event PlayFabResultEvent<ValidateIOSReceiptResult> OnValidateIOSReceiptResultEvent;

		public event PlayFabRequestEvent<ValidateWindowsReceiptRequest> OnValidateWindowsStoreReceiptRequestEvent;

		public event PlayFabResultEvent<ValidateWindowsReceiptResult> OnValidateWindowsStoreReceiptResultEvent;

		public event PlayFabRequestEvent<WriteClientCharacterEventRequest> OnWriteCharacterEventRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.WriteEventResponse> OnWriteCharacterEventResultEvent;

		public event PlayFabRequestEvent<WriteClientPlayerEventRequest> OnWritePlayerEventRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.WriteEventResponse> OnWritePlayerEventResultEvent;

		public event PlayFabRequestEvent<PlayFab.ClientModels.WriteTitleEventRequest> OnWriteTitleEventRequestEvent;

		public event PlayFabResultEvent<PlayFab.ClientModels.WriteEventResponse> OnWriteTitleEventResultEvent;

		public event PlayFabRequestEvent<AuthUserRequest> OnMatchmakerAuthUserRequestEvent;

		public event PlayFabResultEvent<AuthUserResponse> OnMatchmakerAuthUserResultEvent;

		public event PlayFabRequestEvent<PlayerJoinedRequest> OnMatchmakerPlayerJoinedRequestEvent;

		public event PlayFabResultEvent<PlayerJoinedResponse> OnMatchmakerPlayerJoinedResultEvent;

		public event PlayFabRequestEvent<PlayerLeftRequest> OnMatchmakerPlayerLeftRequestEvent;

		public event PlayFabResultEvent<PlayerLeftResponse> OnMatchmakerPlayerLeftResultEvent;

		public event PlayFabRequestEvent<PlayFab.MatchmakerModels.StartGameRequest> OnMatchmakerStartGameRequestEvent;

		public event PlayFabResultEvent<StartGameResponse> OnMatchmakerStartGameResultEvent;

		public event PlayFabRequestEvent<UserInfoRequest> OnMatchmakerUserInfoRequestEvent;

		public event PlayFabResultEvent<UserInfoResponse> OnMatchmakerUserInfoResultEvent;

		public event PlayFabRequestEvent<AddCharacterVirtualCurrencyRequest> OnServerAddCharacterVirtualCurrencyRequestEvent;

		public event PlayFabResultEvent<ModifyCharacterVirtualCurrencyResult> OnServerAddCharacterVirtualCurrencyResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.AddFriendRequest> OnServerAddFriendRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.EmptyResult> OnServerAddFriendResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.AddPlayerTagRequest> OnServerAddPlayerTagRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.AddPlayerTagResult> OnServerAddPlayerTagResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.AddSharedGroupMembersRequest> OnServerAddSharedGroupMembersRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.AddSharedGroupMembersResult> OnServerAddSharedGroupMembersResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.AddUserVirtualCurrencyRequest> OnServerAddUserVirtualCurrencyRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.ModifyUserVirtualCurrencyResult> OnServerAddUserVirtualCurrencyResultEvent;

		public event PlayFabRequestEvent<AuthenticateSessionTicketRequest> OnServerAuthenticateSessionTicketRequestEvent;

		public event PlayFabResultEvent<AuthenticateSessionTicketResult> OnServerAuthenticateSessionTicketResultEvent;

		public event PlayFabRequestEvent<AwardSteamAchievementRequest> OnServerAwardSteamAchievementRequestEvent;

		public event PlayFabResultEvent<AwardSteamAchievementResult> OnServerAwardSteamAchievementResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.BanUsersRequest> OnServerBanUsersRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.BanUsersResult> OnServerBanUsersResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.ConsumeItemRequest> OnServerConsumeItemRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.ConsumeItemResult> OnServerConsumeItemResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.CreateSharedGroupRequest> OnServerCreateSharedGroupRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.CreateSharedGroupResult> OnServerCreateSharedGroupResultEvent;

		public event PlayFabRequestEvent<DeleteCharacterFromUserRequest> OnServerDeleteCharacterFromUserRequestEvent;

		public event PlayFabResultEvent<DeleteCharacterFromUserResult> OnServerDeleteCharacterFromUserResultEvent;

		public event PlayFabRequestEvent<DeleteSharedGroupRequest> OnServerDeleteSharedGroupRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.EmptyResult> OnServerDeleteSharedGroupResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.DeleteUsersRequest> OnServerDeleteUsersRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.DeleteUsersResult> OnServerDeleteUsersResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.DeregisterGameRequest> OnServerDeregisterGameRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.DeregisterGameResponse> OnServerDeregisterGameResultEvent;

		public event PlayFabRequestEvent<EvaluateRandomResultTableRequest> OnServerEvaluateRandomResultTableRequestEvent;

		public event PlayFabResultEvent<EvaluateRandomResultTableResult> OnServerEvaluateRandomResultTableResultEvent;

		public event PlayFabRequestEvent<ExecuteCloudScriptServerRequest> OnServerExecuteCloudScriptRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.ExecuteCloudScriptResult> OnServerExecuteCloudScriptResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetAllSegmentsRequest> OnServerGetAllSegmentsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetAllSegmentsResult> OnServerGetAllSegmentsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.ListUsersCharactersRequest> OnServerGetAllUsersCharactersRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.ListUsersCharactersResult> OnServerGetAllUsersCharactersResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetCatalogItemsRequest> OnServerGetCatalogItemsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetCatalogItemsResult> OnServerGetCatalogItemsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetCharacterDataRequest> OnServerGetCharacterDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetCharacterDataResult> OnServerGetCharacterDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetCharacterDataRequest> OnServerGetCharacterInternalDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetCharacterDataResult> OnServerGetCharacterInternalDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetCharacterInventoryRequest> OnServerGetCharacterInventoryRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetCharacterInventoryResult> OnServerGetCharacterInventoryResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetCharacterLeaderboardRequest> OnServerGetCharacterLeaderboardRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetCharacterLeaderboardResult> OnServerGetCharacterLeaderboardResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetCharacterDataRequest> OnServerGetCharacterReadOnlyDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetCharacterDataResult> OnServerGetCharacterReadOnlyDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetCharacterStatisticsRequest> OnServerGetCharacterStatisticsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetCharacterStatisticsResult> OnServerGetCharacterStatisticsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetContentDownloadUrlRequest> OnServerGetContentDownloadUrlRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetContentDownloadUrlResult> OnServerGetContentDownloadUrlResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetFriendLeaderboardRequest> OnServerGetFriendLeaderboardRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetLeaderboardResult> OnServerGetFriendLeaderboardResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetFriendsListRequest> OnServerGetFriendsListRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetFriendsListResult> OnServerGetFriendsListResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetLeaderboardRequest> OnServerGetLeaderboardRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetLeaderboardResult> OnServerGetLeaderboardResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetLeaderboardAroundCharacterRequest> OnServerGetLeaderboardAroundCharacterRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetLeaderboardAroundCharacterResult> OnServerGetLeaderboardAroundCharacterResultEvent;

		public event PlayFabRequestEvent<GetLeaderboardAroundUserRequest> OnServerGetLeaderboardAroundUserRequestEvent;

		public event PlayFabResultEvent<GetLeaderboardAroundUserResult> OnServerGetLeaderboardAroundUserResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetLeaderboardForUsersCharactersRequest> OnServerGetLeaderboardForUserCharactersRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetLeaderboardForUsersCharactersResult> OnServerGetLeaderboardForUserCharactersResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetPlayerCombinedInfoRequest> OnServerGetPlayerCombinedInfoRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetPlayerCombinedInfoResult> OnServerGetPlayerCombinedInfoResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetPlayerProfileRequest> OnServerGetPlayerProfileRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetPlayerProfileResult> OnServerGetPlayerProfileResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetPlayersSegmentsRequest> OnServerGetPlayerSegmentsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetPlayerSegmentsResult> OnServerGetPlayerSegmentsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetPlayersInSegmentRequest> OnServerGetPlayersInSegmentRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetPlayersInSegmentResult> OnServerGetPlayersInSegmentResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetPlayerStatisticsRequest> OnServerGetPlayerStatisticsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetPlayerStatisticsResult> OnServerGetPlayerStatisticsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetPlayerStatisticVersionsRequest> OnServerGetPlayerStatisticVersionsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetPlayerStatisticVersionsResult> OnServerGetPlayerStatisticVersionsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetPlayerTagsRequest> OnServerGetPlayerTagsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetPlayerTagsResult> OnServerGetPlayerTagsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetPlayFabIDsFromFacebookIDsRequest> OnServerGetPlayFabIDsFromFacebookIDsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetPlayFabIDsFromFacebookIDsResult> OnServerGetPlayFabIDsFromFacebookIDsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetPlayFabIDsFromSteamIDsRequest> OnServerGetPlayFabIDsFromSteamIDsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetPlayFabIDsFromSteamIDsResult> OnServerGetPlayFabIDsFromSteamIDsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetPublisherDataRequest> OnServerGetPublisherDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetPublisherDataResult> OnServerGetPublisherDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetRandomResultTablesRequest> OnServerGetRandomResultTablesRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetRandomResultTablesResult> OnServerGetRandomResultTablesResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetSharedGroupDataRequest> OnServerGetSharedGroupDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetSharedGroupDataResult> OnServerGetSharedGroupDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetTimeRequest> OnServerGetTimeRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetTimeResult> OnServerGetTimeResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetTitleDataRequest> OnServerGetTitleDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetTitleDataResult> OnServerGetTitleDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetTitleDataRequest> OnServerGetTitleInternalDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetTitleDataResult> OnServerGetTitleInternalDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetTitleNewsRequest> OnServerGetTitleNewsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetTitleNewsResult> OnServerGetTitleNewsResultEvent;

		public event PlayFabRequestEvent<GetUserAccountInfoRequest> OnServerGetUserAccountInfoRequestEvent;

		public event PlayFabResultEvent<GetUserAccountInfoResult> OnServerGetUserAccountInfoResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetUserBansRequest> OnServerGetUserBansRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetUserBansResult> OnServerGetUserBansResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetUserDataRequest> OnServerGetUserDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetUserDataResult> OnServerGetUserDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetUserDataRequest> OnServerGetUserInternalDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetUserDataResult> OnServerGetUserInternalDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetUserInventoryRequest> OnServerGetUserInventoryRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetUserInventoryResult> OnServerGetUserInventoryResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetUserDataRequest> OnServerGetUserPublisherDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetUserDataResult> OnServerGetUserPublisherDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetUserDataRequest> OnServerGetUserPublisherInternalDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetUserDataResult> OnServerGetUserPublisherInternalDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetUserDataRequest> OnServerGetUserPublisherReadOnlyDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetUserDataResult> OnServerGetUserPublisherReadOnlyDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GetUserDataRequest> OnServerGetUserReadOnlyDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GetUserDataResult> OnServerGetUserReadOnlyDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GrantCharacterToUserRequest> OnServerGrantCharacterToUserRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GrantCharacterToUserResult> OnServerGrantCharacterToUserResultEvent;

		public event PlayFabRequestEvent<GrantItemsToCharacterRequest> OnServerGrantItemsToCharacterRequestEvent;

		public event PlayFabResultEvent<GrantItemsToCharacterResult> OnServerGrantItemsToCharacterResultEvent;

		public event PlayFabRequestEvent<GrantItemsToUserRequest> OnServerGrantItemsToUserRequestEvent;

		public event PlayFabResultEvent<GrantItemsToUserResult> OnServerGrantItemsToUserResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.GrantItemsToUsersRequest> OnServerGrantItemsToUsersRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.GrantItemsToUsersResult> OnServerGrantItemsToUsersResultEvent;

		public event PlayFabRequestEvent<ModifyItemUsesRequest> OnServerModifyItemUsesRequestEvent;

		public event PlayFabResultEvent<ModifyItemUsesResult> OnServerModifyItemUsesResultEvent;

		public event PlayFabRequestEvent<MoveItemToCharacterFromCharacterRequest> OnServerMoveItemToCharacterFromCharacterRequestEvent;

		public event PlayFabResultEvent<MoveItemToCharacterFromCharacterResult> OnServerMoveItemToCharacterFromCharacterResultEvent;

		public event PlayFabRequestEvent<MoveItemToCharacterFromUserRequest> OnServerMoveItemToCharacterFromUserRequestEvent;

		public event PlayFabResultEvent<MoveItemToCharacterFromUserResult> OnServerMoveItemToCharacterFromUserResultEvent;

		public event PlayFabRequestEvent<MoveItemToUserFromCharacterRequest> OnServerMoveItemToUserFromCharacterRequestEvent;

		public event PlayFabResultEvent<MoveItemToUserFromCharacterResult> OnServerMoveItemToUserFromCharacterResultEvent;

		public event PlayFabRequestEvent<NotifyMatchmakerPlayerLeftRequest> OnServerNotifyMatchmakerPlayerLeftRequestEvent;

		public event PlayFabResultEvent<NotifyMatchmakerPlayerLeftResult> OnServerNotifyMatchmakerPlayerLeftResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.RedeemCouponRequest> OnServerRedeemCouponRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.RedeemCouponResult> OnServerRedeemCouponResultEvent;

		public event PlayFabRequestEvent<RedeemMatchmakerTicketRequest> OnServerRedeemMatchmakerTicketRequestEvent;

		public event PlayFabResultEvent<RedeemMatchmakerTicketResult> OnServerRedeemMatchmakerTicketResultEvent;

		public event PlayFabRequestEvent<RefreshGameServerInstanceHeartbeatRequest> OnServerRefreshGameServerInstanceHeartbeatRequestEvent;

		public event PlayFabResultEvent<RefreshGameServerInstanceHeartbeatResult> OnServerRefreshGameServerInstanceHeartbeatResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.RegisterGameRequest> OnServerRegisterGameRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.RegisterGameResponse> OnServerRegisterGameResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.RemoveFriendRequest> OnServerRemoveFriendRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.EmptyResult> OnServerRemoveFriendResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.RemovePlayerTagRequest> OnServerRemovePlayerTagRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.RemovePlayerTagResult> OnServerRemovePlayerTagResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.RemoveSharedGroupMembersRequest> OnServerRemoveSharedGroupMembersRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.RemoveSharedGroupMembersResult> OnServerRemoveSharedGroupMembersResultEvent;

		public event PlayFabRequestEvent<ReportPlayerServerRequest> OnServerReportPlayerRequestEvent;

		public event PlayFabResultEvent<ReportPlayerServerResult> OnServerReportPlayerResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.RevokeAllBansForUserRequest> OnServerRevokeAllBansForUserRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.RevokeAllBansForUserResult> OnServerRevokeAllBansForUserResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.RevokeBansRequest> OnServerRevokeBansRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.RevokeBansResult> OnServerRevokeBansResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.RevokeInventoryItemRequest> OnServerRevokeInventoryItemRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.RevokeInventoryResult> OnServerRevokeInventoryItemResultEvent;

		public event PlayFabRequestEvent<SendCustomAccountRecoveryEmailRequest> OnServerSendCustomAccountRecoveryEmailRequestEvent;

		public event PlayFabResultEvent<SendCustomAccountRecoveryEmailResult> OnServerSendCustomAccountRecoveryEmailResultEvent;

		public event PlayFabRequestEvent<SendEmailFromTemplateRequest> OnServerSendEmailFromTemplateRequestEvent;

		public event PlayFabResultEvent<SendEmailFromTemplateResult> OnServerSendEmailFromTemplateResultEvent;

		public event PlayFabRequestEvent<SendPushNotificationRequest> OnServerSendPushNotificationRequestEvent;

		public event PlayFabResultEvent<SendPushNotificationResult> OnServerSendPushNotificationResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.SetFriendTagsRequest> OnServerSetFriendTagsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.EmptyResult> OnServerSetFriendTagsResultEvent;

		public event PlayFabRequestEvent<SetGameServerInstanceDataRequest> OnServerSetGameServerInstanceDataRequestEvent;

		public event PlayFabResultEvent<SetGameServerInstanceDataResult> OnServerSetGameServerInstanceDataResultEvent;

		public event PlayFabRequestEvent<SetGameServerInstanceStateRequest> OnServerSetGameServerInstanceStateRequestEvent;

		public event PlayFabResultEvent<SetGameServerInstanceStateResult> OnServerSetGameServerInstanceStateResultEvent;

		public event PlayFabRequestEvent<SetGameServerInstanceTagsRequest> OnServerSetGameServerInstanceTagsRequestEvent;

		public event PlayFabResultEvent<SetGameServerInstanceTagsResult> OnServerSetGameServerInstanceTagsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.SetPlayerSecretRequest> OnServerSetPlayerSecretRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.SetPlayerSecretResult> OnServerSetPlayerSecretResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.SetPublisherDataRequest> OnServerSetPublisherDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.SetPublisherDataResult> OnServerSetPublisherDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.SetTitleDataRequest> OnServerSetTitleDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.SetTitleDataResult> OnServerSetTitleDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.SetTitleDataRequest> OnServerSetTitleInternalDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.SetTitleDataResult> OnServerSetTitleInternalDataResultEvent;

		public event PlayFabRequestEvent<SubtractCharacterVirtualCurrencyRequest> OnServerSubtractCharacterVirtualCurrencyRequestEvent;

		public event PlayFabResultEvent<ModifyCharacterVirtualCurrencyResult> OnServerSubtractCharacterVirtualCurrencyResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.SubtractUserVirtualCurrencyRequest> OnServerSubtractUserVirtualCurrencyRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.ModifyUserVirtualCurrencyResult> OnServerSubtractUserVirtualCurrencyResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.UnlockContainerInstanceRequest> OnServerUnlockContainerInstanceRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.UnlockContainerItemResult> OnServerUnlockContainerInstanceResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.UnlockContainerItemRequest> OnServerUnlockContainerItemRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.UnlockContainerItemResult> OnServerUnlockContainerItemResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.UpdateAvatarUrlRequest> OnServerUpdateAvatarUrlRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.EmptyResult> OnServerUpdateAvatarUrlResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.UpdateBansRequest> OnServerUpdateBansRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.UpdateBansResult> OnServerUpdateBansResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.UpdateCharacterDataRequest> OnServerUpdateCharacterDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.UpdateCharacterDataResult> OnServerUpdateCharacterDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.UpdateCharacterDataRequest> OnServerUpdateCharacterInternalDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.UpdateCharacterDataResult> OnServerUpdateCharacterInternalDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.UpdateCharacterDataRequest> OnServerUpdateCharacterReadOnlyDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.UpdateCharacterDataResult> OnServerUpdateCharacterReadOnlyDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.UpdateCharacterStatisticsRequest> OnServerUpdateCharacterStatisticsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.UpdateCharacterStatisticsResult> OnServerUpdateCharacterStatisticsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.UpdatePlayerStatisticsRequest> OnServerUpdatePlayerStatisticsRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.UpdatePlayerStatisticsResult> OnServerUpdatePlayerStatisticsResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.UpdateSharedGroupDataRequest> OnServerUpdateSharedGroupDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.UpdateSharedGroupDataResult> OnServerUpdateSharedGroupDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.UpdateUserDataRequest> OnServerUpdateUserDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.UpdateUserDataResult> OnServerUpdateUserDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.UpdateUserInternalDataRequest> OnServerUpdateUserInternalDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.UpdateUserDataResult> OnServerUpdateUserInternalDataResultEvent;

		public event PlayFabRequestEvent<UpdateUserInventoryItemDataRequest> OnServerUpdateUserInventoryItemCustomDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.EmptyResult> OnServerUpdateUserInventoryItemCustomDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.UpdateUserDataRequest> OnServerUpdateUserPublisherDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.UpdateUserDataResult> OnServerUpdateUserPublisherDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.UpdateUserInternalDataRequest> OnServerUpdateUserPublisherInternalDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.UpdateUserDataResult> OnServerUpdateUserPublisherInternalDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.UpdateUserDataRequest> OnServerUpdateUserPublisherReadOnlyDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.UpdateUserDataResult> OnServerUpdateUserPublisherReadOnlyDataResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.UpdateUserDataRequest> OnServerUpdateUserReadOnlyDataRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.UpdateUserDataResult> OnServerUpdateUserReadOnlyDataResultEvent;

		public event PlayFabRequestEvent<WriteServerCharacterEventRequest> OnServerWriteCharacterEventRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.WriteEventResponse> OnServerWriteCharacterEventResultEvent;

		public event PlayFabRequestEvent<WriteServerPlayerEventRequest> OnServerWritePlayerEventRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.WriteEventResponse> OnServerWritePlayerEventResultEvent;

		public event PlayFabRequestEvent<PlayFab.ServerModels.WriteTitleEventRequest> OnServerWriteTitleEventRequestEvent;

		public event PlayFabResultEvent<PlayFab.ServerModels.WriteEventResponse> OnServerWriteTitleEventResultEvent;

		public event PlayFabErrorEvent OnGlobalErrorEvent;

		private PlayFabEvents()
		{
		}

		public static PlayFabEvents Init()
		{
			if (_instance == null)
			{
				_instance = new PlayFabEvents();
			}
			PlayFabHttp.ApiProcessingEventHandler += _instance.OnProcessingEvent;
			PlayFabHttp.ApiProcessingErrorEventHandler += _instance.OnProcessingErrorEvent;
			return _instance;
		}

		public void UnregisterInstance(object instance)
		{
			if (this.OnLoginResultEvent != null)
			{
				Delegate[] invocationList = this.OnLoginResultEvent.GetInvocationList();
				foreach (Delegate @delegate in invocationList)
				{
					if (object.ReferenceEquals(@delegate.Target, instance))
					{
						OnLoginResultEvent -= (PlayFabResultEvent<LoginResult>)@delegate;
					}
				}
			}
			if (this.OnAdminAbortTaskInstanceRequestEvent != null)
			{
				Delegate[] invocationList2 = this.OnAdminAbortTaskInstanceRequestEvent.GetInvocationList();
				foreach (Delegate delegate2 in invocationList2)
				{
					if (object.ReferenceEquals(delegate2.Target, instance))
					{
						OnAdminAbortTaskInstanceRequestEvent -= (PlayFabRequestEvent<AbortTaskInstanceRequest>)delegate2;
					}
				}
			}
			if (this.OnAdminAbortTaskInstanceResultEvent != null)
			{
				Delegate[] invocationList3 = this.OnAdminAbortTaskInstanceResultEvent.GetInvocationList();
				foreach (Delegate delegate3 in invocationList3)
				{
					if (object.ReferenceEquals(delegate3.Target, instance))
					{
						OnAdminAbortTaskInstanceResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.EmptyResult>)delegate3;
					}
				}
			}
			if (this.OnAdminAddNewsRequestEvent != null)
			{
				Delegate[] invocationList4 = this.OnAdminAddNewsRequestEvent.GetInvocationList();
				foreach (Delegate delegate4 in invocationList4)
				{
					if (object.ReferenceEquals(delegate4.Target, instance))
					{
						OnAdminAddNewsRequestEvent -= (PlayFabRequestEvent<AddNewsRequest>)delegate4;
					}
				}
			}
			if (this.OnAdminAddNewsResultEvent != null)
			{
				Delegate[] invocationList5 = this.OnAdminAddNewsResultEvent.GetInvocationList();
				foreach (Delegate delegate5 in invocationList5)
				{
					if (object.ReferenceEquals(delegate5.Target, instance))
					{
						OnAdminAddNewsResultEvent -= (PlayFabResultEvent<AddNewsResult>)delegate5;
					}
				}
			}
			if (this.OnAdminAddPlayerTagRequestEvent != null)
			{
				Delegate[] invocationList6 = this.OnAdminAddPlayerTagRequestEvent.GetInvocationList();
				foreach (Delegate delegate6 in invocationList6)
				{
					if (object.ReferenceEquals(delegate6.Target, instance))
					{
						OnAdminAddPlayerTagRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.AddPlayerTagRequest>)delegate6;
					}
				}
			}
			if (this.OnAdminAddPlayerTagResultEvent != null)
			{
				Delegate[] invocationList7 = this.OnAdminAddPlayerTagResultEvent.GetInvocationList();
				foreach (Delegate delegate7 in invocationList7)
				{
					if (object.ReferenceEquals(delegate7.Target, instance))
					{
						OnAdminAddPlayerTagResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.AddPlayerTagResult>)delegate7;
					}
				}
			}
			if (this.OnAdminAddServerBuildRequestEvent != null)
			{
				Delegate[] invocationList8 = this.OnAdminAddServerBuildRequestEvent.GetInvocationList();
				foreach (Delegate delegate8 in invocationList8)
				{
					if (object.ReferenceEquals(delegate8.Target, instance))
					{
						OnAdminAddServerBuildRequestEvent -= (PlayFabRequestEvent<AddServerBuildRequest>)delegate8;
					}
				}
			}
			if (this.OnAdminAddServerBuildResultEvent != null)
			{
				Delegate[] invocationList9 = this.OnAdminAddServerBuildResultEvent.GetInvocationList();
				foreach (Delegate delegate9 in invocationList9)
				{
					if (object.ReferenceEquals(delegate9.Target, instance))
					{
						OnAdminAddServerBuildResultEvent -= (PlayFabResultEvent<AddServerBuildResult>)delegate9;
					}
				}
			}
			if (this.OnAdminAddUserVirtualCurrencyRequestEvent != null)
			{
				Delegate[] invocationList10 = this.OnAdminAddUserVirtualCurrencyRequestEvent.GetInvocationList();
				foreach (Delegate delegate10 in invocationList10)
				{
					if (object.ReferenceEquals(delegate10.Target, instance))
					{
						OnAdminAddUserVirtualCurrencyRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.AddUserVirtualCurrencyRequest>)delegate10;
					}
				}
			}
			if (this.OnAdminAddUserVirtualCurrencyResultEvent != null)
			{
				Delegate[] invocationList11 = this.OnAdminAddUserVirtualCurrencyResultEvent.GetInvocationList();
				foreach (Delegate delegate11 in invocationList11)
				{
					if (object.ReferenceEquals(delegate11.Target, instance))
					{
						OnAdminAddUserVirtualCurrencyResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.ModifyUserVirtualCurrencyResult>)delegate11;
					}
				}
			}
			if (this.OnAdminAddVirtualCurrencyTypesRequestEvent != null)
			{
				Delegate[] invocationList12 = this.OnAdminAddVirtualCurrencyTypesRequestEvent.GetInvocationList();
				foreach (Delegate delegate12 in invocationList12)
				{
					if (object.ReferenceEquals(delegate12.Target, instance))
					{
						OnAdminAddVirtualCurrencyTypesRequestEvent -= (PlayFabRequestEvent<AddVirtualCurrencyTypesRequest>)delegate12;
					}
				}
			}
			if (this.OnAdminAddVirtualCurrencyTypesResultEvent != null)
			{
				Delegate[] invocationList13 = this.OnAdminAddVirtualCurrencyTypesResultEvent.GetInvocationList();
				foreach (Delegate delegate13 in invocationList13)
				{
					if (object.ReferenceEquals(delegate13.Target, instance))
					{
						OnAdminAddVirtualCurrencyTypesResultEvent -= (PlayFabResultEvent<BlankResult>)delegate13;
					}
				}
			}
			if (this.OnAdminBanUsersRequestEvent != null)
			{
				Delegate[] invocationList14 = this.OnAdminBanUsersRequestEvent.GetInvocationList();
				foreach (Delegate delegate14 in invocationList14)
				{
					if (object.ReferenceEquals(delegate14.Target, instance))
					{
						OnAdminBanUsersRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.BanUsersRequest>)delegate14;
					}
				}
			}
			if (this.OnAdminBanUsersResultEvent != null)
			{
				Delegate[] invocationList15 = this.OnAdminBanUsersResultEvent.GetInvocationList();
				foreach (Delegate delegate15 in invocationList15)
				{
					if (object.ReferenceEquals(delegate15.Target, instance))
					{
						OnAdminBanUsersResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.BanUsersResult>)delegate15;
					}
				}
			}
			if (this.OnAdminCheckLimitedEditionItemAvailabilityRequestEvent != null)
			{
				Delegate[] invocationList16 = this.OnAdminCheckLimitedEditionItemAvailabilityRequestEvent.GetInvocationList();
				foreach (Delegate delegate16 in invocationList16)
				{
					if (object.ReferenceEquals(delegate16.Target, instance))
					{
						OnAdminCheckLimitedEditionItemAvailabilityRequestEvent -= (PlayFabRequestEvent<CheckLimitedEditionItemAvailabilityRequest>)delegate16;
					}
				}
			}
			if (this.OnAdminCheckLimitedEditionItemAvailabilityResultEvent != null)
			{
				Delegate[] invocationList17 = this.OnAdminCheckLimitedEditionItemAvailabilityResultEvent.GetInvocationList();
				foreach (Delegate delegate17 in invocationList17)
				{
					if (object.ReferenceEquals(delegate17.Target, instance))
					{
						OnAdminCheckLimitedEditionItemAvailabilityResultEvent -= (PlayFabResultEvent<CheckLimitedEditionItemAvailabilityResult>)delegate17;
					}
				}
			}
			if (this.OnAdminCreateActionsOnPlayersInSegmentTaskRequestEvent != null)
			{
				Delegate[] invocationList18 = this.OnAdminCreateActionsOnPlayersInSegmentTaskRequestEvent.GetInvocationList();
				foreach (Delegate delegate18 in invocationList18)
				{
					if (object.ReferenceEquals(delegate18.Target, instance))
					{
						OnAdminCreateActionsOnPlayersInSegmentTaskRequestEvent -= (PlayFabRequestEvent<CreateActionsOnPlayerSegmentTaskRequest>)delegate18;
					}
				}
			}
			if (this.OnAdminCreateActionsOnPlayersInSegmentTaskResultEvent != null)
			{
				Delegate[] invocationList19 = this.OnAdminCreateActionsOnPlayersInSegmentTaskResultEvent.GetInvocationList();
				foreach (Delegate delegate19 in invocationList19)
				{
					if (object.ReferenceEquals(delegate19.Target, instance))
					{
						OnAdminCreateActionsOnPlayersInSegmentTaskResultEvent -= (PlayFabResultEvent<CreateTaskResult>)delegate19;
					}
				}
			}
			if (this.OnAdminCreateCloudScriptTaskRequestEvent != null)
			{
				Delegate[] invocationList20 = this.OnAdminCreateCloudScriptTaskRequestEvent.GetInvocationList();
				foreach (Delegate delegate20 in invocationList20)
				{
					if (object.ReferenceEquals(delegate20.Target, instance))
					{
						OnAdminCreateCloudScriptTaskRequestEvent -= (PlayFabRequestEvent<CreateCloudScriptTaskRequest>)delegate20;
					}
				}
			}
			if (this.OnAdminCreateCloudScriptTaskResultEvent != null)
			{
				Delegate[] invocationList21 = this.OnAdminCreateCloudScriptTaskResultEvent.GetInvocationList();
				foreach (Delegate delegate21 in invocationList21)
				{
					if (object.ReferenceEquals(delegate21.Target, instance))
					{
						OnAdminCreateCloudScriptTaskResultEvent -= (PlayFabResultEvent<CreateTaskResult>)delegate21;
					}
				}
			}
			if (this.OnAdminCreatePlayerSharedSecretRequestEvent != null)
			{
				Delegate[] invocationList22 = this.OnAdminCreatePlayerSharedSecretRequestEvent.GetInvocationList();
				foreach (Delegate delegate22 in invocationList22)
				{
					if (object.ReferenceEquals(delegate22.Target, instance))
					{
						OnAdminCreatePlayerSharedSecretRequestEvent -= (PlayFabRequestEvent<CreatePlayerSharedSecretRequest>)delegate22;
					}
				}
			}
			if (this.OnAdminCreatePlayerSharedSecretResultEvent != null)
			{
				Delegate[] invocationList23 = this.OnAdminCreatePlayerSharedSecretResultEvent.GetInvocationList();
				foreach (Delegate delegate23 in invocationList23)
				{
					if (object.ReferenceEquals(delegate23.Target, instance))
					{
						OnAdminCreatePlayerSharedSecretResultEvent -= (PlayFabResultEvent<CreatePlayerSharedSecretResult>)delegate23;
					}
				}
			}
			if (this.OnAdminCreatePlayerStatisticDefinitionRequestEvent != null)
			{
				Delegate[] invocationList24 = this.OnAdminCreatePlayerStatisticDefinitionRequestEvent.GetInvocationList();
				foreach (Delegate delegate24 in invocationList24)
				{
					if (object.ReferenceEquals(delegate24.Target, instance))
					{
						OnAdminCreatePlayerStatisticDefinitionRequestEvent -= (PlayFabRequestEvent<CreatePlayerStatisticDefinitionRequest>)delegate24;
					}
				}
			}
			if (this.OnAdminCreatePlayerStatisticDefinitionResultEvent != null)
			{
				Delegate[] invocationList25 = this.OnAdminCreatePlayerStatisticDefinitionResultEvent.GetInvocationList();
				foreach (Delegate delegate25 in invocationList25)
				{
					if (object.ReferenceEquals(delegate25.Target, instance))
					{
						OnAdminCreatePlayerStatisticDefinitionResultEvent -= (PlayFabResultEvent<CreatePlayerStatisticDefinitionResult>)delegate25;
					}
				}
			}
			if (this.OnAdminDeleteContentRequestEvent != null)
			{
				Delegate[] invocationList26 = this.OnAdminDeleteContentRequestEvent.GetInvocationList();
				foreach (Delegate delegate26 in invocationList26)
				{
					if (object.ReferenceEquals(delegate26.Target, instance))
					{
						OnAdminDeleteContentRequestEvent -= (PlayFabRequestEvent<DeleteContentRequest>)delegate26;
					}
				}
			}
			if (this.OnAdminDeleteContentResultEvent != null)
			{
				Delegate[] invocationList27 = this.OnAdminDeleteContentResultEvent.GetInvocationList();
				foreach (Delegate delegate27 in invocationList27)
				{
					if (object.ReferenceEquals(delegate27.Target, instance))
					{
						OnAdminDeleteContentResultEvent -= (PlayFabResultEvent<BlankResult>)delegate27;
					}
				}
			}
			if (this.OnAdminDeletePlayerRequestEvent != null)
			{
				Delegate[] invocationList28 = this.OnAdminDeletePlayerRequestEvent.GetInvocationList();
				foreach (Delegate delegate28 in invocationList28)
				{
					if (object.ReferenceEquals(delegate28.Target, instance))
					{
						OnAdminDeletePlayerRequestEvent -= (PlayFabRequestEvent<DeletePlayerRequest>)delegate28;
					}
				}
			}
			if (this.OnAdminDeletePlayerResultEvent != null)
			{
				Delegate[] invocationList29 = this.OnAdminDeletePlayerResultEvent.GetInvocationList();
				foreach (Delegate delegate29 in invocationList29)
				{
					if (object.ReferenceEquals(delegate29.Target, instance))
					{
						OnAdminDeletePlayerResultEvent -= (PlayFabResultEvent<DeletePlayerResult>)delegate29;
					}
				}
			}
			if (this.OnAdminDeletePlayerSharedSecretRequestEvent != null)
			{
				Delegate[] invocationList30 = this.OnAdminDeletePlayerSharedSecretRequestEvent.GetInvocationList();
				foreach (Delegate delegate30 in invocationList30)
				{
					if (object.ReferenceEquals(delegate30.Target, instance))
					{
						OnAdminDeletePlayerSharedSecretRequestEvent -= (PlayFabRequestEvent<DeletePlayerSharedSecretRequest>)delegate30;
					}
				}
			}
			if (this.OnAdminDeletePlayerSharedSecretResultEvent != null)
			{
				Delegate[] invocationList31 = this.OnAdminDeletePlayerSharedSecretResultEvent.GetInvocationList();
				foreach (Delegate delegate31 in invocationList31)
				{
					if (object.ReferenceEquals(delegate31.Target, instance))
					{
						OnAdminDeletePlayerSharedSecretResultEvent -= (PlayFabResultEvent<DeletePlayerSharedSecretResult>)delegate31;
					}
				}
			}
			if (this.OnAdminDeleteStoreRequestEvent != null)
			{
				Delegate[] invocationList32 = this.OnAdminDeleteStoreRequestEvent.GetInvocationList();
				foreach (Delegate delegate32 in invocationList32)
				{
					if (object.ReferenceEquals(delegate32.Target, instance))
					{
						OnAdminDeleteStoreRequestEvent -= (PlayFabRequestEvent<DeleteStoreRequest>)delegate32;
					}
				}
			}
			if (this.OnAdminDeleteStoreResultEvent != null)
			{
				Delegate[] invocationList33 = this.OnAdminDeleteStoreResultEvent.GetInvocationList();
				foreach (Delegate delegate33 in invocationList33)
				{
					if (object.ReferenceEquals(delegate33.Target, instance))
					{
						OnAdminDeleteStoreResultEvent -= (PlayFabResultEvent<DeleteStoreResult>)delegate33;
					}
				}
			}
			if (this.OnAdminDeleteTaskRequestEvent != null)
			{
				Delegate[] invocationList34 = this.OnAdminDeleteTaskRequestEvent.GetInvocationList();
				foreach (Delegate delegate34 in invocationList34)
				{
					if (object.ReferenceEquals(delegate34.Target, instance))
					{
						OnAdminDeleteTaskRequestEvent -= (PlayFabRequestEvent<DeleteTaskRequest>)delegate34;
					}
				}
			}
			if (this.OnAdminDeleteTaskResultEvent != null)
			{
				Delegate[] invocationList35 = this.OnAdminDeleteTaskResultEvent.GetInvocationList();
				foreach (Delegate delegate35 in invocationList35)
				{
					if (object.ReferenceEquals(delegate35.Target, instance))
					{
						OnAdminDeleteTaskResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.EmptyResult>)delegate35;
					}
				}
			}
			if (this.OnAdminDeleteTitleRequestEvent != null)
			{
				Delegate[] invocationList36 = this.OnAdminDeleteTitleRequestEvent.GetInvocationList();
				foreach (Delegate delegate36 in invocationList36)
				{
					if (object.ReferenceEquals(delegate36.Target, instance))
					{
						OnAdminDeleteTitleRequestEvent -= (PlayFabRequestEvent<DeleteTitleRequest>)delegate36;
					}
				}
			}
			if (this.OnAdminDeleteTitleResultEvent != null)
			{
				Delegate[] invocationList37 = this.OnAdminDeleteTitleResultEvent.GetInvocationList();
				foreach (Delegate delegate37 in invocationList37)
				{
					if (object.ReferenceEquals(delegate37.Target, instance))
					{
						OnAdminDeleteTitleResultEvent -= (PlayFabResultEvent<DeleteTitleResult>)delegate37;
					}
				}
			}
			if (this.OnAdminGetActionsOnPlayersInSegmentTaskInstanceRequestEvent != null)
			{
				Delegate[] invocationList38 = this.OnAdminGetActionsOnPlayersInSegmentTaskInstanceRequestEvent.GetInvocationList();
				foreach (Delegate delegate38 in invocationList38)
				{
					if (object.ReferenceEquals(delegate38.Target, instance))
					{
						OnAdminGetActionsOnPlayersInSegmentTaskInstanceRequestEvent -= (PlayFabRequestEvent<GetTaskInstanceRequest>)delegate38;
					}
				}
			}
			if (this.OnAdminGetActionsOnPlayersInSegmentTaskInstanceResultEvent != null)
			{
				Delegate[] invocationList39 = this.OnAdminGetActionsOnPlayersInSegmentTaskInstanceResultEvent.GetInvocationList();
				foreach (Delegate delegate39 in invocationList39)
				{
					if (object.ReferenceEquals(delegate39.Target, instance))
					{
						OnAdminGetActionsOnPlayersInSegmentTaskInstanceResultEvent -= (PlayFabResultEvent<GetActionsOnPlayersInSegmentTaskInstanceResult>)delegate39;
					}
				}
			}
			if (this.OnAdminGetAllSegmentsRequestEvent != null)
			{
				Delegate[] invocationList40 = this.OnAdminGetAllSegmentsRequestEvent.GetInvocationList();
				foreach (Delegate delegate40 in invocationList40)
				{
					if (object.ReferenceEquals(delegate40.Target, instance))
					{
						OnAdminGetAllSegmentsRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetAllSegmentsRequest>)delegate40;
					}
				}
			}
			if (this.OnAdminGetAllSegmentsResultEvent != null)
			{
				Delegate[] invocationList41 = this.OnAdminGetAllSegmentsResultEvent.GetInvocationList();
				foreach (Delegate delegate41 in invocationList41)
				{
					if (object.ReferenceEquals(delegate41.Target, instance))
					{
						OnAdminGetAllSegmentsResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetAllSegmentsResult>)delegate41;
					}
				}
			}
			if (this.OnAdminGetCatalogItemsRequestEvent != null)
			{
				Delegate[] invocationList42 = this.OnAdminGetCatalogItemsRequestEvent.GetInvocationList();
				foreach (Delegate delegate42 in invocationList42)
				{
					if (object.ReferenceEquals(delegate42.Target, instance))
					{
						OnAdminGetCatalogItemsRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetCatalogItemsRequest>)delegate42;
					}
				}
			}
			if (this.OnAdminGetCatalogItemsResultEvent != null)
			{
				Delegate[] invocationList43 = this.OnAdminGetCatalogItemsResultEvent.GetInvocationList();
				foreach (Delegate delegate43 in invocationList43)
				{
					if (object.ReferenceEquals(delegate43.Target, instance))
					{
						OnAdminGetCatalogItemsResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetCatalogItemsResult>)delegate43;
					}
				}
			}
			if (this.OnAdminGetCloudScriptRevisionRequestEvent != null)
			{
				Delegate[] invocationList44 = this.OnAdminGetCloudScriptRevisionRequestEvent.GetInvocationList();
				foreach (Delegate delegate44 in invocationList44)
				{
					if (object.ReferenceEquals(delegate44.Target, instance))
					{
						OnAdminGetCloudScriptRevisionRequestEvent -= (PlayFabRequestEvent<GetCloudScriptRevisionRequest>)delegate44;
					}
				}
			}
			if (this.OnAdminGetCloudScriptRevisionResultEvent != null)
			{
				Delegate[] invocationList45 = this.OnAdminGetCloudScriptRevisionResultEvent.GetInvocationList();
				foreach (Delegate delegate45 in invocationList45)
				{
					if (object.ReferenceEquals(delegate45.Target, instance))
					{
						OnAdminGetCloudScriptRevisionResultEvent -= (PlayFabResultEvent<GetCloudScriptRevisionResult>)delegate45;
					}
				}
			}
			if (this.OnAdminGetCloudScriptTaskInstanceRequestEvent != null)
			{
				Delegate[] invocationList46 = this.OnAdminGetCloudScriptTaskInstanceRequestEvent.GetInvocationList();
				foreach (Delegate delegate46 in invocationList46)
				{
					if (object.ReferenceEquals(delegate46.Target, instance))
					{
						OnAdminGetCloudScriptTaskInstanceRequestEvent -= (PlayFabRequestEvent<GetTaskInstanceRequest>)delegate46;
					}
				}
			}
			if (this.OnAdminGetCloudScriptTaskInstanceResultEvent != null)
			{
				Delegate[] invocationList47 = this.OnAdminGetCloudScriptTaskInstanceResultEvent.GetInvocationList();
				foreach (Delegate delegate47 in invocationList47)
				{
					if (object.ReferenceEquals(delegate47.Target, instance))
					{
						OnAdminGetCloudScriptTaskInstanceResultEvent -= (PlayFabResultEvent<GetCloudScriptTaskInstanceResult>)delegate47;
					}
				}
			}
			if (this.OnAdminGetCloudScriptVersionsRequestEvent != null)
			{
				Delegate[] invocationList48 = this.OnAdminGetCloudScriptVersionsRequestEvent.GetInvocationList();
				foreach (Delegate delegate48 in invocationList48)
				{
					if (object.ReferenceEquals(delegate48.Target, instance))
					{
						OnAdminGetCloudScriptVersionsRequestEvent -= (PlayFabRequestEvent<GetCloudScriptVersionsRequest>)delegate48;
					}
				}
			}
			if (this.OnAdminGetCloudScriptVersionsResultEvent != null)
			{
				Delegate[] invocationList49 = this.OnAdminGetCloudScriptVersionsResultEvent.GetInvocationList();
				foreach (Delegate delegate49 in invocationList49)
				{
					if (object.ReferenceEquals(delegate49.Target, instance))
					{
						OnAdminGetCloudScriptVersionsResultEvent -= (PlayFabResultEvent<GetCloudScriptVersionsResult>)delegate49;
					}
				}
			}
			if (this.OnAdminGetContentListRequestEvent != null)
			{
				Delegate[] invocationList50 = this.OnAdminGetContentListRequestEvent.GetInvocationList();
				foreach (Delegate delegate50 in invocationList50)
				{
					if (object.ReferenceEquals(delegate50.Target, instance))
					{
						OnAdminGetContentListRequestEvent -= (PlayFabRequestEvent<GetContentListRequest>)delegate50;
					}
				}
			}
			if (this.OnAdminGetContentListResultEvent != null)
			{
				Delegate[] invocationList51 = this.OnAdminGetContentListResultEvent.GetInvocationList();
				foreach (Delegate delegate51 in invocationList51)
				{
					if (object.ReferenceEquals(delegate51.Target, instance))
					{
						OnAdminGetContentListResultEvent -= (PlayFabResultEvent<GetContentListResult>)delegate51;
					}
				}
			}
			if (this.OnAdminGetContentUploadUrlRequestEvent != null)
			{
				Delegate[] invocationList52 = this.OnAdminGetContentUploadUrlRequestEvent.GetInvocationList();
				foreach (Delegate delegate52 in invocationList52)
				{
					if (object.ReferenceEquals(delegate52.Target, instance))
					{
						OnAdminGetContentUploadUrlRequestEvent -= (PlayFabRequestEvent<GetContentUploadUrlRequest>)delegate52;
					}
				}
			}
			if (this.OnAdminGetContentUploadUrlResultEvent != null)
			{
				Delegate[] invocationList53 = this.OnAdminGetContentUploadUrlResultEvent.GetInvocationList();
				foreach (Delegate delegate53 in invocationList53)
				{
					if (object.ReferenceEquals(delegate53.Target, instance))
					{
						OnAdminGetContentUploadUrlResultEvent -= (PlayFabResultEvent<GetContentUploadUrlResult>)delegate53;
					}
				}
			}
			if (this.OnAdminGetDataReportRequestEvent != null)
			{
				Delegate[] invocationList54 = this.OnAdminGetDataReportRequestEvent.GetInvocationList();
				foreach (Delegate delegate54 in invocationList54)
				{
					if (object.ReferenceEquals(delegate54.Target, instance))
					{
						OnAdminGetDataReportRequestEvent -= (PlayFabRequestEvent<GetDataReportRequest>)delegate54;
					}
				}
			}
			if (this.OnAdminGetDataReportResultEvent != null)
			{
				Delegate[] invocationList55 = this.OnAdminGetDataReportResultEvent.GetInvocationList();
				foreach (Delegate delegate55 in invocationList55)
				{
					if (object.ReferenceEquals(delegate55.Target, instance))
					{
						OnAdminGetDataReportResultEvent -= (PlayFabResultEvent<GetDataReportResult>)delegate55;
					}
				}
			}
			if (this.OnAdminGetMatchmakerGameInfoRequestEvent != null)
			{
				Delegate[] invocationList56 = this.OnAdminGetMatchmakerGameInfoRequestEvent.GetInvocationList();
				foreach (Delegate delegate56 in invocationList56)
				{
					if (object.ReferenceEquals(delegate56.Target, instance))
					{
						OnAdminGetMatchmakerGameInfoRequestEvent -= (PlayFabRequestEvent<GetMatchmakerGameInfoRequest>)delegate56;
					}
				}
			}
			if (this.OnAdminGetMatchmakerGameInfoResultEvent != null)
			{
				Delegate[] invocationList57 = this.OnAdminGetMatchmakerGameInfoResultEvent.GetInvocationList();
				foreach (Delegate delegate57 in invocationList57)
				{
					if (object.ReferenceEquals(delegate57.Target, instance))
					{
						OnAdminGetMatchmakerGameInfoResultEvent -= (PlayFabResultEvent<GetMatchmakerGameInfoResult>)delegate57;
					}
				}
			}
			if (this.OnAdminGetMatchmakerGameModesRequestEvent != null)
			{
				Delegate[] invocationList58 = this.OnAdminGetMatchmakerGameModesRequestEvent.GetInvocationList();
				foreach (Delegate delegate58 in invocationList58)
				{
					if (object.ReferenceEquals(delegate58.Target, instance))
					{
						OnAdminGetMatchmakerGameModesRequestEvent -= (PlayFabRequestEvent<GetMatchmakerGameModesRequest>)delegate58;
					}
				}
			}
			if (this.OnAdminGetMatchmakerGameModesResultEvent != null)
			{
				Delegate[] invocationList59 = this.OnAdminGetMatchmakerGameModesResultEvent.GetInvocationList();
				foreach (Delegate delegate59 in invocationList59)
				{
					if (object.ReferenceEquals(delegate59.Target, instance))
					{
						OnAdminGetMatchmakerGameModesResultEvent -= (PlayFabResultEvent<GetMatchmakerGameModesResult>)delegate59;
					}
				}
			}
			if (this.OnAdminGetPlayerIdFromAuthTokenRequestEvent != null)
			{
				Delegate[] invocationList60 = this.OnAdminGetPlayerIdFromAuthTokenRequestEvent.GetInvocationList();
				foreach (Delegate delegate60 in invocationList60)
				{
					if (object.ReferenceEquals(delegate60.Target, instance))
					{
						OnAdminGetPlayerIdFromAuthTokenRequestEvent -= (PlayFabRequestEvent<GetPlayerIdFromAuthTokenRequest>)delegate60;
					}
				}
			}
			if (this.OnAdminGetPlayerIdFromAuthTokenResultEvent != null)
			{
				Delegate[] invocationList61 = this.OnAdminGetPlayerIdFromAuthTokenResultEvent.GetInvocationList();
				foreach (Delegate delegate61 in invocationList61)
				{
					if (object.ReferenceEquals(delegate61.Target, instance))
					{
						OnAdminGetPlayerIdFromAuthTokenResultEvent -= (PlayFabResultEvent<GetPlayerIdFromAuthTokenResult>)delegate61;
					}
				}
			}
			if (this.OnAdminGetPlayerProfileRequestEvent != null)
			{
				Delegate[] invocationList62 = this.OnAdminGetPlayerProfileRequestEvent.GetInvocationList();
				foreach (Delegate delegate62 in invocationList62)
				{
					if (object.ReferenceEquals(delegate62.Target, instance))
					{
						OnAdminGetPlayerProfileRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetPlayerProfileRequest>)delegate62;
					}
				}
			}
			if (this.OnAdminGetPlayerProfileResultEvent != null)
			{
				Delegate[] invocationList63 = this.OnAdminGetPlayerProfileResultEvent.GetInvocationList();
				foreach (Delegate delegate63 in invocationList63)
				{
					if (object.ReferenceEquals(delegate63.Target, instance))
					{
						OnAdminGetPlayerProfileResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetPlayerProfileResult>)delegate63;
					}
				}
			}
			if (this.OnAdminGetPlayerSegmentsRequestEvent != null)
			{
				Delegate[] invocationList64 = this.OnAdminGetPlayerSegmentsRequestEvent.GetInvocationList();
				foreach (Delegate delegate64 in invocationList64)
				{
					if (object.ReferenceEquals(delegate64.Target, instance))
					{
						OnAdminGetPlayerSegmentsRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetPlayersSegmentsRequest>)delegate64;
					}
				}
			}
			if (this.OnAdminGetPlayerSegmentsResultEvent != null)
			{
				Delegate[] invocationList65 = this.OnAdminGetPlayerSegmentsResultEvent.GetInvocationList();
				foreach (Delegate delegate65 in invocationList65)
				{
					if (object.ReferenceEquals(delegate65.Target, instance))
					{
						OnAdminGetPlayerSegmentsResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetPlayerSegmentsResult>)delegate65;
					}
				}
			}
			if (this.OnAdminGetPlayerSharedSecretsRequestEvent != null)
			{
				Delegate[] invocationList66 = this.OnAdminGetPlayerSharedSecretsRequestEvent.GetInvocationList();
				foreach (Delegate delegate66 in invocationList66)
				{
					if (object.ReferenceEquals(delegate66.Target, instance))
					{
						OnAdminGetPlayerSharedSecretsRequestEvent -= (PlayFabRequestEvent<GetPlayerSharedSecretsRequest>)delegate66;
					}
				}
			}
			if (this.OnAdminGetPlayerSharedSecretsResultEvent != null)
			{
				Delegate[] invocationList67 = this.OnAdminGetPlayerSharedSecretsResultEvent.GetInvocationList();
				foreach (Delegate delegate67 in invocationList67)
				{
					if (object.ReferenceEquals(delegate67.Target, instance))
					{
						OnAdminGetPlayerSharedSecretsResultEvent -= (PlayFabResultEvent<GetPlayerSharedSecretsResult>)delegate67;
					}
				}
			}
			if (this.OnAdminGetPlayersInSegmentRequestEvent != null)
			{
				Delegate[] invocationList68 = this.OnAdminGetPlayersInSegmentRequestEvent.GetInvocationList();
				foreach (Delegate delegate68 in invocationList68)
				{
					if (object.ReferenceEquals(delegate68.Target, instance))
					{
						OnAdminGetPlayersInSegmentRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetPlayersInSegmentRequest>)delegate68;
					}
				}
			}
			if (this.OnAdminGetPlayersInSegmentResultEvent != null)
			{
				Delegate[] invocationList69 = this.OnAdminGetPlayersInSegmentResultEvent.GetInvocationList();
				foreach (Delegate delegate69 in invocationList69)
				{
					if (object.ReferenceEquals(delegate69.Target, instance))
					{
						OnAdminGetPlayersInSegmentResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetPlayersInSegmentResult>)delegate69;
					}
				}
			}
			if (this.OnAdminGetPlayerStatisticDefinitionsRequestEvent != null)
			{
				Delegate[] invocationList70 = this.OnAdminGetPlayerStatisticDefinitionsRequestEvent.GetInvocationList();
				foreach (Delegate delegate70 in invocationList70)
				{
					if (object.ReferenceEquals(delegate70.Target, instance))
					{
						OnAdminGetPlayerStatisticDefinitionsRequestEvent -= (PlayFabRequestEvent<GetPlayerStatisticDefinitionsRequest>)delegate70;
					}
				}
			}
			if (this.OnAdminGetPlayerStatisticDefinitionsResultEvent != null)
			{
				Delegate[] invocationList71 = this.OnAdminGetPlayerStatisticDefinitionsResultEvent.GetInvocationList();
				foreach (Delegate delegate71 in invocationList71)
				{
					if (object.ReferenceEquals(delegate71.Target, instance))
					{
						OnAdminGetPlayerStatisticDefinitionsResultEvent -= (PlayFabResultEvent<GetPlayerStatisticDefinitionsResult>)delegate71;
					}
				}
			}
			if (this.OnAdminGetPlayerStatisticVersionsRequestEvent != null)
			{
				Delegate[] invocationList72 = this.OnAdminGetPlayerStatisticVersionsRequestEvent.GetInvocationList();
				foreach (Delegate delegate72 in invocationList72)
				{
					if (object.ReferenceEquals(delegate72.Target, instance))
					{
						OnAdminGetPlayerStatisticVersionsRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetPlayerStatisticVersionsRequest>)delegate72;
					}
				}
			}
			if (this.OnAdminGetPlayerStatisticVersionsResultEvent != null)
			{
				Delegate[] invocationList73 = this.OnAdminGetPlayerStatisticVersionsResultEvent.GetInvocationList();
				foreach (Delegate delegate73 in invocationList73)
				{
					if (object.ReferenceEquals(delegate73.Target, instance))
					{
						OnAdminGetPlayerStatisticVersionsResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetPlayerStatisticVersionsResult>)delegate73;
					}
				}
			}
			if (this.OnAdminGetPlayerTagsRequestEvent != null)
			{
				Delegate[] invocationList74 = this.OnAdminGetPlayerTagsRequestEvent.GetInvocationList();
				foreach (Delegate delegate74 in invocationList74)
				{
					if (object.ReferenceEquals(delegate74.Target, instance))
					{
						OnAdminGetPlayerTagsRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetPlayerTagsRequest>)delegate74;
					}
				}
			}
			if (this.OnAdminGetPlayerTagsResultEvent != null)
			{
				Delegate[] invocationList75 = this.OnAdminGetPlayerTagsResultEvent.GetInvocationList();
				foreach (Delegate delegate75 in invocationList75)
				{
					if (object.ReferenceEquals(delegate75.Target, instance))
					{
						OnAdminGetPlayerTagsResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetPlayerTagsResult>)delegate75;
					}
				}
			}
			if (this.OnAdminGetPolicyRequestEvent != null)
			{
				Delegate[] invocationList76 = this.OnAdminGetPolicyRequestEvent.GetInvocationList();
				foreach (Delegate delegate76 in invocationList76)
				{
					if (object.ReferenceEquals(delegate76.Target, instance))
					{
						OnAdminGetPolicyRequestEvent -= (PlayFabRequestEvent<GetPolicyRequest>)delegate76;
					}
				}
			}
			if (this.OnAdminGetPolicyResultEvent != null)
			{
				Delegate[] invocationList77 = this.OnAdminGetPolicyResultEvent.GetInvocationList();
				foreach (Delegate delegate77 in invocationList77)
				{
					if (object.ReferenceEquals(delegate77.Target, instance))
					{
						OnAdminGetPolicyResultEvent -= (PlayFabResultEvent<GetPolicyResponse>)delegate77;
					}
				}
			}
			if (this.OnAdminGetPublisherDataRequestEvent != null)
			{
				Delegate[] invocationList78 = this.OnAdminGetPublisherDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate78 in invocationList78)
				{
					if (object.ReferenceEquals(delegate78.Target, instance))
					{
						OnAdminGetPublisherDataRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetPublisherDataRequest>)delegate78;
					}
				}
			}
			if (this.OnAdminGetPublisherDataResultEvent != null)
			{
				Delegate[] invocationList79 = this.OnAdminGetPublisherDataResultEvent.GetInvocationList();
				foreach (Delegate delegate79 in invocationList79)
				{
					if (object.ReferenceEquals(delegate79.Target, instance))
					{
						OnAdminGetPublisherDataResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetPublisherDataResult>)delegate79;
					}
				}
			}
			if (this.OnAdminGetRandomResultTablesRequestEvent != null)
			{
				Delegate[] invocationList80 = this.OnAdminGetRandomResultTablesRequestEvent.GetInvocationList();
				foreach (Delegate delegate80 in invocationList80)
				{
					if (object.ReferenceEquals(delegate80.Target, instance))
					{
						OnAdminGetRandomResultTablesRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetRandomResultTablesRequest>)delegate80;
					}
				}
			}
			if (this.OnAdminGetRandomResultTablesResultEvent != null)
			{
				Delegate[] invocationList81 = this.OnAdminGetRandomResultTablesResultEvent.GetInvocationList();
				foreach (Delegate delegate81 in invocationList81)
				{
					if (object.ReferenceEquals(delegate81.Target, instance))
					{
						OnAdminGetRandomResultTablesResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetRandomResultTablesResult>)delegate81;
					}
				}
			}
			if (this.OnAdminGetServerBuildInfoRequestEvent != null)
			{
				Delegate[] invocationList82 = this.OnAdminGetServerBuildInfoRequestEvent.GetInvocationList();
				foreach (Delegate delegate82 in invocationList82)
				{
					if (object.ReferenceEquals(delegate82.Target, instance))
					{
						OnAdminGetServerBuildInfoRequestEvent -= (PlayFabRequestEvent<GetServerBuildInfoRequest>)delegate82;
					}
				}
			}
			if (this.OnAdminGetServerBuildInfoResultEvent != null)
			{
				Delegate[] invocationList83 = this.OnAdminGetServerBuildInfoResultEvent.GetInvocationList();
				foreach (Delegate delegate83 in invocationList83)
				{
					if (object.ReferenceEquals(delegate83.Target, instance))
					{
						OnAdminGetServerBuildInfoResultEvent -= (PlayFabResultEvent<GetServerBuildInfoResult>)delegate83;
					}
				}
			}
			if (this.OnAdminGetServerBuildUploadUrlRequestEvent != null)
			{
				Delegate[] invocationList84 = this.OnAdminGetServerBuildUploadUrlRequestEvent.GetInvocationList();
				foreach (Delegate delegate84 in invocationList84)
				{
					if (object.ReferenceEquals(delegate84.Target, instance))
					{
						OnAdminGetServerBuildUploadUrlRequestEvent -= (PlayFabRequestEvent<GetServerBuildUploadURLRequest>)delegate84;
					}
				}
			}
			if (this.OnAdminGetServerBuildUploadUrlResultEvent != null)
			{
				Delegate[] invocationList85 = this.OnAdminGetServerBuildUploadUrlResultEvent.GetInvocationList();
				foreach (Delegate delegate85 in invocationList85)
				{
					if (object.ReferenceEquals(delegate85.Target, instance))
					{
						OnAdminGetServerBuildUploadUrlResultEvent -= (PlayFabResultEvent<GetServerBuildUploadURLResult>)delegate85;
					}
				}
			}
			if (this.OnAdminGetStoreItemsRequestEvent != null)
			{
				Delegate[] invocationList86 = this.OnAdminGetStoreItemsRequestEvent.GetInvocationList();
				foreach (Delegate delegate86 in invocationList86)
				{
					if (object.ReferenceEquals(delegate86.Target, instance))
					{
						OnAdminGetStoreItemsRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetStoreItemsRequest>)delegate86;
					}
				}
			}
			if (this.OnAdminGetStoreItemsResultEvent != null)
			{
				Delegate[] invocationList87 = this.OnAdminGetStoreItemsResultEvent.GetInvocationList();
				foreach (Delegate delegate87 in invocationList87)
				{
					if (object.ReferenceEquals(delegate87.Target, instance))
					{
						OnAdminGetStoreItemsResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetStoreItemsResult>)delegate87;
					}
				}
			}
			if (this.OnAdminGetTaskInstancesRequestEvent != null)
			{
				Delegate[] invocationList88 = this.OnAdminGetTaskInstancesRequestEvent.GetInvocationList();
				foreach (Delegate delegate88 in invocationList88)
				{
					if (object.ReferenceEquals(delegate88.Target, instance))
					{
						OnAdminGetTaskInstancesRequestEvent -= (PlayFabRequestEvent<GetTaskInstancesRequest>)delegate88;
					}
				}
			}
			if (this.OnAdminGetTaskInstancesResultEvent != null)
			{
				Delegate[] invocationList89 = this.OnAdminGetTaskInstancesResultEvent.GetInvocationList();
				foreach (Delegate delegate89 in invocationList89)
				{
					if (object.ReferenceEquals(delegate89.Target, instance))
					{
						OnAdminGetTaskInstancesResultEvent -= (PlayFabResultEvent<GetTaskInstancesResult>)delegate89;
					}
				}
			}
			if (this.OnAdminGetTasksRequestEvent != null)
			{
				Delegate[] invocationList90 = this.OnAdminGetTasksRequestEvent.GetInvocationList();
				foreach (Delegate delegate90 in invocationList90)
				{
					if (object.ReferenceEquals(delegate90.Target, instance))
					{
						OnAdminGetTasksRequestEvent -= (PlayFabRequestEvent<GetTasksRequest>)delegate90;
					}
				}
			}
			if (this.OnAdminGetTasksResultEvent != null)
			{
				Delegate[] invocationList91 = this.OnAdminGetTasksResultEvent.GetInvocationList();
				foreach (Delegate delegate91 in invocationList91)
				{
					if (object.ReferenceEquals(delegate91.Target, instance))
					{
						OnAdminGetTasksResultEvent -= (PlayFabResultEvent<GetTasksResult>)delegate91;
					}
				}
			}
			if (this.OnAdminGetTitleDataRequestEvent != null)
			{
				Delegate[] invocationList92 = this.OnAdminGetTitleDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate92 in invocationList92)
				{
					if (object.ReferenceEquals(delegate92.Target, instance))
					{
						OnAdminGetTitleDataRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetTitleDataRequest>)delegate92;
					}
				}
			}
			if (this.OnAdminGetTitleDataResultEvent != null)
			{
				Delegate[] invocationList93 = this.OnAdminGetTitleDataResultEvent.GetInvocationList();
				foreach (Delegate delegate93 in invocationList93)
				{
					if (object.ReferenceEquals(delegate93.Target, instance))
					{
						OnAdminGetTitleDataResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetTitleDataResult>)delegate93;
					}
				}
			}
			if (this.OnAdminGetTitleInternalDataRequestEvent != null)
			{
				Delegate[] invocationList94 = this.OnAdminGetTitleInternalDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate94 in invocationList94)
				{
					if (object.ReferenceEquals(delegate94.Target, instance))
					{
						OnAdminGetTitleInternalDataRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetTitleDataRequest>)delegate94;
					}
				}
			}
			if (this.OnAdminGetTitleInternalDataResultEvent != null)
			{
				Delegate[] invocationList95 = this.OnAdminGetTitleInternalDataResultEvent.GetInvocationList();
				foreach (Delegate delegate95 in invocationList95)
				{
					if (object.ReferenceEquals(delegate95.Target, instance))
					{
						OnAdminGetTitleInternalDataResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetTitleDataResult>)delegate95;
					}
				}
			}
			if (this.OnAdminGetUserAccountInfoRequestEvent != null)
			{
				Delegate[] invocationList96 = this.OnAdminGetUserAccountInfoRequestEvent.GetInvocationList();
				foreach (Delegate delegate96 in invocationList96)
				{
					if (object.ReferenceEquals(delegate96.Target, instance))
					{
						OnAdminGetUserAccountInfoRequestEvent -= (PlayFabRequestEvent<LookupUserAccountInfoRequest>)delegate96;
					}
				}
			}
			if (this.OnAdminGetUserAccountInfoResultEvent != null)
			{
				Delegate[] invocationList97 = this.OnAdminGetUserAccountInfoResultEvent.GetInvocationList();
				foreach (Delegate delegate97 in invocationList97)
				{
					if (object.ReferenceEquals(delegate97.Target, instance))
					{
						OnAdminGetUserAccountInfoResultEvent -= (PlayFabResultEvent<LookupUserAccountInfoResult>)delegate97;
					}
				}
			}
			if (this.OnAdminGetUserBansRequestEvent != null)
			{
				Delegate[] invocationList98 = this.OnAdminGetUserBansRequestEvent.GetInvocationList();
				foreach (Delegate delegate98 in invocationList98)
				{
					if (object.ReferenceEquals(delegate98.Target, instance))
					{
						OnAdminGetUserBansRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetUserBansRequest>)delegate98;
					}
				}
			}
			if (this.OnAdminGetUserBansResultEvent != null)
			{
				Delegate[] invocationList99 = this.OnAdminGetUserBansResultEvent.GetInvocationList();
				foreach (Delegate delegate99 in invocationList99)
				{
					if (object.ReferenceEquals(delegate99.Target, instance))
					{
						OnAdminGetUserBansResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetUserBansResult>)delegate99;
					}
				}
			}
			if (this.OnAdminGetUserDataRequestEvent != null)
			{
				Delegate[] invocationList100 = this.OnAdminGetUserDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate100 in invocationList100)
				{
					if (object.ReferenceEquals(delegate100.Target, instance))
					{
						OnAdminGetUserDataRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetUserDataRequest>)delegate100;
					}
				}
			}
			if (this.OnAdminGetUserDataResultEvent != null)
			{
				Delegate[] invocationList101 = this.OnAdminGetUserDataResultEvent.GetInvocationList();
				foreach (Delegate delegate101 in invocationList101)
				{
					if (object.ReferenceEquals(delegate101.Target, instance))
					{
						OnAdminGetUserDataResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetUserDataResult>)delegate101;
					}
				}
			}
			if (this.OnAdminGetUserInternalDataRequestEvent != null)
			{
				Delegate[] invocationList102 = this.OnAdminGetUserInternalDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate102 in invocationList102)
				{
					if (object.ReferenceEquals(delegate102.Target, instance))
					{
						OnAdminGetUserInternalDataRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetUserDataRequest>)delegate102;
					}
				}
			}
			if (this.OnAdminGetUserInternalDataResultEvent != null)
			{
				Delegate[] invocationList103 = this.OnAdminGetUserInternalDataResultEvent.GetInvocationList();
				foreach (Delegate delegate103 in invocationList103)
				{
					if (object.ReferenceEquals(delegate103.Target, instance))
					{
						OnAdminGetUserInternalDataResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetUserDataResult>)delegate103;
					}
				}
			}
			if (this.OnAdminGetUserInventoryRequestEvent != null)
			{
				Delegate[] invocationList104 = this.OnAdminGetUserInventoryRequestEvent.GetInvocationList();
				foreach (Delegate delegate104 in invocationList104)
				{
					if (object.ReferenceEquals(delegate104.Target, instance))
					{
						OnAdminGetUserInventoryRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetUserInventoryRequest>)delegate104;
					}
				}
			}
			if (this.OnAdminGetUserInventoryResultEvent != null)
			{
				Delegate[] invocationList105 = this.OnAdminGetUserInventoryResultEvent.GetInvocationList();
				foreach (Delegate delegate105 in invocationList105)
				{
					if (object.ReferenceEquals(delegate105.Target, instance))
					{
						OnAdminGetUserInventoryResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetUserInventoryResult>)delegate105;
					}
				}
			}
			if (this.OnAdminGetUserPublisherDataRequestEvent != null)
			{
				Delegate[] invocationList106 = this.OnAdminGetUserPublisherDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate106 in invocationList106)
				{
					if (object.ReferenceEquals(delegate106.Target, instance))
					{
						OnAdminGetUserPublisherDataRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetUserDataRequest>)delegate106;
					}
				}
			}
			if (this.OnAdminGetUserPublisherDataResultEvent != null)
			{
				Delegate[] invocationList107 = this.OnAdminGetUserPublisherDataResultEvent.GetInvocationList();
				foreach (Delegate delegate107 in invocationList107)
				{
					if (object.ReferenceEquals(delegate107.Target, instance))
					{
						OnAdminGetUserPublisherDataResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetUserDataResult>)delegate107;
					}
				}
			}
			if (this.OnAdminGetUserPublisherInternalDataRequestEvent != null)
			{
				Delegate[] invocationList108 = this.OnAdminGetUserPublisherInternalDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate108 in invocationList108)
				{
					if (object.ReferenceEquals(delegate108.Target, instance))
					{
						OnAdminGetUserPublisherInternalDataRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetUserDataRequest>)delegate108;
					}
				}
			}
			if (this.OnAdminGetUserPublisherInternalDataResultEvent != null)
			{
				Delegate[] invocationList109 = this.OnAdminGetUserPublisherInternalDataResultEvent.GetInvocationList();
				foreach (Delegate delegate109 in invocationList109)
				{
					if (object.ReferenceEquals(delegate109.Target, instance))
					{
						OnAdminGetUserPublisherInternalDataResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetUserDataResult>)delegate109;
					}
				}
			}
			if (this.OnAdminGetUserPublisherReadOnlyDataRequestEvent != null)
			{
				Delegate[] invocationList110 = this.OnAdminGetUserPublisherReadOnlyDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate110 in invocationList110)
				{
					if (object.ReferenceEquals(delegate110.Target, instance))
					{
						OnAdminGetUserPublisherReadOnlyDataRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetUserDataRequest>)delegate110;
					}
				}
			}
			if (this.OnAdminGetUserPublisherReadOnlyDataResultEvent != null)
			{
				Delegate[] invocationList111 = this.OnAdminGetUserPublisherReadOnlyDataResultEvent.GetInvocationList();
				foreach (Delegate delegate111 in invocationList111)
				{
					if (object.ReferenceEquals(delegate111.Target, instance))
					{
						OnAdminGetUserPublisherReadOnlyDataResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetUserDataResult>)delegate111;
					}
				}
			}
			if (this.OnAdminGetUserReadOnlyDataRequestEvent != null)
			{
				Delegate[] invocationList112 = this.OnAdminGetUserReadOnlyDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate112 in invocationList112)
				{
					if (object.ReferenceEquals(delegate112.Target, instance))
					{
						OnAdminGetUserReadOnlyDataRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GetUserDataRequest>)delegate112;
					}
				}
			}
			if (this.OnAdminGetUserReadOnlyDataResultEvent != null)
			{
				Delegate[] invocationList113 = this.OnAdminGetUserReadOnlyDataResultEvent.GetInvocationList();
				foreach (Delegate delegate113 in invocationList113)
				{
					if (object.ReferenceEquals(delegate113.Target, instance))
					{
						OnAdminGetUserReadOnlyDataResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GetUserDataResult>)delegate113;
					}
				}
			}
			if (this.OnAdminGrantItemsToUsersRequestEvent != null)
			{
				Delegate[] invocationList114 = this.OnAdminGrantItemsToUsersRequestEvent.GetInvocationList();
				foreach (Delegate delegate114 in invocationList114)
				{
					if (object.ReferenceEquals(delegate114.Target, instance))
					{
						OnAdminGrantItemsToUsersRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.GrantItemsToUsersRequest>)delegate114;
					}
				}
			}
			if (this.OnAdminGrantItemsToUsersResultEvent != null)
			{
				Delegate[] invocationList115 = this.OnAdminGrantItemsToUsersResultEvent.GetInvocationList();
				foreach (Delegate delegate115 in invocationList115)
				{
					if (object.ReferenceEquals(delegate115.Target, instance))
					{
						OnAdminGrantItemsToUsersResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.GrantItemsToUsersResult>)delegate115;
					}
				}
			}
			if (this.OnAdminIncrementLimitedEditionItemAvailabilityRequestEvent != null)
			{
				Delegate[] invocationList116 = this.OnAdminIncrementLimitedEditionItemAvailabilityRequestEvent.GetInvocationList();
				foreach (Delegate delegate116 in invocationList116)
				{
					if (object.ReferenceEquals(delegate116.Target, instance))
					{
						OnAdminIncrementLimitedEditionItemAvailabilityRequestEvent -= (PlayFabRequestEvent<IncrementLimitedEditionItemAvailabilityRequest>)delegate116;
					}
				}
			}
			if (this.OnAdminIncrementLimitedEditionItemAvailabilityResultEvent != null)
			{
				Delegate[] invocationList117 = this.OnAdminIncrementLimitedEditionItemAvailabilityResultEvent.GetInvocationList();
				foreach (Delegate delegate117 in invocationList117)
				{
					if (object.ReferenceEquals(delegate117.Target, instance))
					{
						OnAdminIncrementLimitedEditionItemAvailabilityResultEvent -= (PlayFabResultEvent<IncrementLimitedEditionItemAvailabilityResult>)delegate117;
					}
				}
			}
			if (this.OnAdminIncrementPlayerStatisticVersionRequestEvent != null)
			{
				Delegate[] invocationList118 = this.OnAdminIncrementPlayerStatisticVersionRequestEvent.GetInvocationList();
				foreach (Delegate delegate118 in invocationList118)
				{
					if (object.ReferenceEquals(delegate118.Target, instance))
					{
						OnAdminIncrementPlayerStatisticVersionRequestEvent -= (PlayFabRequestEvent<IncrementPlayerStatisticVersionRequest>)delegate118;
					}
				}
			}
			if (this.OnAdminIncrementPlayerStatisticVersionResultEvent != null)
			{
				Delegate[] invocationList119 = this.OnAdminIncrementPlayerStatisticVersionResultEvent.GetInvocationList();
				foreach (Delegate delegate119 in invocationList119)
				{
					if (object.ReferenceEquals(delegate119.Target, instance))
					{
						OnAdminIncrementPlayerStatisticVersionResultEvent -= (PlayFabResultEvent<IncrementPlayerStatisticVersionResult>)delegate119;
					}
				}
			}
			if (this.OnAdminListServerBuildsRequestEvent != null)
			{
				Delegate[] invocationList120 = this.OnAdminListServerBuildsRequestEvent.GetInvocationList();
				foreach (Delegate delegate120 in invocationList120)
				{
					if (object.ReferenceEquals(delegate120.Target, instance))
					{
						OnAdminListServerBuildsRequestEvent -= (PlayFabRequestEvent<ListBuildsRequest>)delegate120;
					}
				}
			}
			if (this.OnAdminListServerBuildsResultEvent != null)
			{
				Delegate[] invocationList121 = this.OnAdminListServerBuildsResultEvent.GetInvocationList();
				foreach (Delegate delegate121 in invocationList121)
				{
					if (object.ReferenceEquals(delegate121.Target, instance))
					{
						OnAdminListServerBuildsResultEvent -= (PlayFabResultEvent<ListBuildsResult>)delegate121;
					}
				}
			}
			if (this.OnAdminListVirtualCurrencyTypesRequestEvent != null)
			{
				Delegate[] invocationList122 = this.OnAdminListVirtualCurrencyTypesRequestEvent.GetInvocationList();
				foreach (Delegate delegate122 in invocationList122)
				{
					if (object.ReferenceEquals(delegate122.Target, instance))
					{
						OnAdminListVirtualCurrencyTypesRequestEvent -= (PlayFabRequestEvent<ListVirtualCurrencyTypesRequest>)delegate122;
					}
				}
			}
			if (this.OnAdminListVirtualCurrencyTypesResultEvent != null)
			{
				Delegate[] invocationList123 = this.OnAdminListVirtualCurrencyTypesResultEvent.GetInvocationList();
				foreach (Delegate delegate123 in invocationList123)
				{
					if (object.ReferenceEquals(delegate123.Target, instance))
					{
						OnAdminListVirtualCurrencyTypesResultEvent -= (PlayFabResultEvent<ListVirtualCurrencyTypesResult>)delegate123;
					}
				}
			}
			if (this.OnAdminModifyMatchmakerGameModesRequestEvent != null)
			{
				Delegate[] invocationList124 = this.OnAdminModifyMatchmakerGameModesRequestEvent.GetInvocationList();
				foreach (Delegate delegate124 in invocationList124)
				{
					if (object.ReferenceEquals(delegate124.Target, instance))
					{
						OnAdminModifyMatchmakerGameModesRequestEvent -= (PlayFabRequestEvent<ModifyMatchmakerGameModesRequest>)delegate124;
					}
				}
			}
			if (this.OnAdminModifyMatchmakerGameModesResultEvent != null)
			{
				Delegate[] invocationList125 = this.OnAdminModifyMatchmakerGameModesResultEvent.GetInvocationList();
				foreach (Delegate delegate125 in invocationList125)
				{
					if (object.ReferenceEquals(delegate125.Target, instance))
					{
						OnAdminModifyMatchmakerGameModesResultEvent -= (PlayFabResultEvent<ModifyMatchmakerGameModesResult>)delegate125;
					}
				}
			}
			if (this.OnAdminModifyServerBuildRequestEvent != null)
			{
				Delegate[] invocationList126 = this.OnAdminModifyServerBuildRequestEvent.GetInvocationList();
				foreach (Delegate delegate126 in invocationList126)
				{
					if (object.ReferenceEquals(delegate126.Target, instance))
					{
						OnAdminModifyServerBuildRequestEvent -= (PlayFabRequestEvent<ModifyServerBuildRequest>)delegate126;
					}
				}
			}
			if (this.OnAdminModifyServerBuildResultEvent != null)
			{
				Delegate[] invocationList127 = this.OnAdminModifyServerBuildResultEvent.GetInvocationList();
				foreach (Delegate delegate127 in invocationList127)
				{
					if (object.ReferenceEquals(delegate127.Target, instance))
					{
						OnAdminModifyServerBuildResultEvent -= (PlayFabResultEvent<ModifyServerBuildResult>)delegate127;
					}
				}
			}
			if (this.OnAdminRefundPurchaseRequestEvent != null)
			{
				Delegate[] invocationList128 = this.OnAdminRefundPurchaseRequestEvent.GetInvocationList();
				foreach (Delegate delegate128 in invocationList128)
				{
					if (object.ReferenceEquals(delegate128.Target, instance))
					{
						OnAdminRefundPurchaseRequestEvent -= (PlayFabRequestEvent<RefundPurchaseRequest>)delegate128;
					}
				}
			}
			if (this.OnAdminRefundPurchaseResultEvent != null)
			{
				Delegate[] invocationList129 = this.OnAdminRefundPurchaseResultEvent.GetInvocationList();
				foreach (Delegate delegate129 in invocationList129)
				{
					if (object.ReferenceEquals(delegate129.Target, instance))
					{
						OnAdminRefundPurchaseResultEvent -= (PlayFabResultEvent<RefundPurchaseResponse>)delegate129;
					}
				}
			}
			if (this.OnAdminRemovePlayerTagRequestEvent != null)
			{
				Delegate[] invocationList130 = this.OnAdminRemovePlayerTagRequestEvent.GetInvocationList();
				foreach (Delegate delegate130 in invocationList130)
				{
					if (object.ReferenceEquals(delegate130.Target, instance))
					{
						OnAdminRemovePlayerTagRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.RemovePlayerTagRequest>)delegate130;
					}
				}
			}
			if (this.OnAdminRemovePlayerTagResultEvent != null)
			{
				Delegate[] invocationList131 = this.OnAdminRemovePlayerTagResultEvent.GetInvocationList();
				foreach (Delegate delegate131 in invocationList131)
				{
					if (object.ReferenceEquals(delegate131.Target, instance))
					{
						OnAdminRemovePlayerTagResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.RemovePlayerTagResult>)delegate131;
					}
				}
			}
			if (this.OnAdminRemoveServerBuildRequestEvent != null)
			{
				Delegate[] invocationList132 = this.OnAdminRemoveServerBuildRequestEvent.GetInvocationList();
				foreach (Delegate delegate132 in invocationList132)
				{
					if (object.ReferenceEquals(delegate132.Target, instance))
					{
						OnAdminRemoveServerBuildRequestEvent -= (PlayFabRequestEvent<RemoveServerBuildRequest>)delegate132;
					}
				}
			}
			if (this.OnAdminRemoveServerBuildResultEvent != null)
			{
				Delegate[] invocationList133 = this.OnAdminRemoveServerBuildResultEvent.GetInvocationList();
				foreach (Delegate delegate133 in invocationList133)
				{
					if (object.ReferenceEquals(delegate133.Target, instance))
					{
						OnAdminRemoveServerBuildResultEvent -= (PlayFabResultEvent<RemoveServerBuildResult>)delegate133;
					}
				}
			}
			if (this.OnAdminRemoveVirtualCurrencyTypesRequestEvent != null)
			{
				Delegate[] invocationList134 = this.OnAdminRemoveVirtualCurrencyTypesRequestEvent.GetInvocationList();
				foreach (Delegate delegate134 in invocationList134)
				{
					if (object.ReferenceEquals(delegate134.Target, instance))
					{
						OnAdminRemoveVirtualCurrencyTypesRequestEvent -= (PlayFabRequestEvent<RemoveVirtualCurrencyTypesRequest>)delegate134;
					}
				}
			}
			if (this.OnAdminRemoveVirtualCurrencyTypesResultEvent != null)
			{
				Delegate[] invocationList135 = this.OnAdminRemoveVirtualCurrencyTypesResultEvent.GetInvocationList();
				foreach (Delegate delegate135 in invocationList135)
				{
					if (object.ReferenceEquals(delegate135.Target, instance))
					{
						OnAdminRemoveVirtualCurrencyTypesResultEvent -= (PlayFabResultEvent<BlankResult>)delegate135;
					}
				}
			}
			if (this.OnAdminResetCharacterStatisticsRequestEvent != null)
			{
				Delegate[] invocationList136 = this.OnAdminResetCharacterStatisticsRequestEvent.GetInvocationList();
				foreach (Delegate delegate136 in invocationList136)
				{
					if (object.ReferenceEquals(delegate136.Target, instance))
					{
						OnAdminResetCharacterStatisticsRequestEvent -= (PlayFabRequestEvent<ResetCharacterStatisticsRequest>)delegate136;
					}
				}
			}
			if (this.OnAdminResetCharacterStatisticsResultEvent != null)
			{
				Delegate[] invocationList137 = this.OnAdminResetCharacterStatisticsResultEvent.GetInvocationList();
				foreach (Delegate delegate137 in invocationList137)
				{
					if (object.ReferenceEquals(delegate137.Target, instance))
					{
						OnAdminResetCharacterStatisticsResultEvent -= (PlayFabResultEvent<ResetCharacterStatisticsResult>)delegate137;
					}
				}
			}
			if (this.OnAdminResetPasswordRequestEvent != null)
			{
				Delegate[] invocationList138 = this.OnAdminResetPasswordRequestEvent.GetInvocationList();
				foreach (Delegate delegate138 in invocationList138)
				{
					if (object.ReferenceEquals(delegate138.Target, instance))
					{
						OnAdminResetPasswordRequestEvent -= (PlayFabRequestEvent<ResetPasswordRequest>)delegate138;
					}
				}
			}
			if (this.OnAdminResetPasswordResultEvent != null)
			{
				Delegate[] invocationList139 = this.OnAdminResetPasswordResultEvent.GetInvocationList();
				foreach (Delegate delegate139 in invocationList139)
				{
					if (object.ReferenceEquals(delegate139.Target, instance))
					{
						OnAdminResetPasswordResultEvent -= (PlayFabResultEvent<ResetPasswordResult>)delegate139;
					}
				}
			}
			if (this.OnAdminResetUserStatisticsRequestEvent != null)
			{
				Delegate[] invocationList140 = this.OnAdminResetUserStatisticsRequestEvent.GetInvocationList();
				foreach (Delegate delegate140 in invocationList140)
				{
					if (object.ReferenceEquals(delegate140.Target, instance))
					{
						OnAdminResetUserStatisticsRequestEvent -= (PlayFabRequestEvent<ResetUserStatisticsRequest>)delegate140;
					}
				}
			}
			if (this.OnAdminResetUserStatisticsResultEvent != null)
			{
				Delegate[] invocationList141 = this.OnAdminResetUserStatisticsResultEvent.GetInvocationList();
				foreach (Delegate delegate141 in invocationList141)
				{
					if (object.ReferenceEquals(delegate141.Target, instance))
					{
						OnAdminResetUserStatisticsResultEvent -= (PlayFabResultEvent<ResetUserStatisticsResult>)delegate141;
					}
				}
			}
			if (this.OnAdminResolvePurchaseDisputeRequestEvent != null)
			{
				Delegate[] invocationList142 = this.OnAdminResolvePurchaseDisputeRequestEvent.GetInvocationList();
				foreach (Delegate delegate142 in invocationList142)
				{
					if (object.ReferenceEquals(delegate142.Target, instance))
					{
						OnAdminResolvePurchaseDisputeRequestEvent -= (PlayFabRequestEvent<ResolvePurchaseDisputeRequest>)delegate142;
					}
				}
			}
			if (this.OnAdminResolvePurchaseDisputeResultEvent != null)
			{
				Delegate[] invocationList143 = this.OnAdminResolvePurchaseDisputeResultEvent.GetInvocationList();
				foreach (Delegate delegate143 in invocationList143)
				{
					if (object.ReferenceEquals(delegate143.Target, instance))
					{
						OnAdminResolvePurchaseDisputeResultEvent -= (PlayFabResultEvent<ResolvePurchaseDisputeResponse>)delegate143;
					}
				}
			}
			if (this.OnAdminRevokeAllBansForUserRequestEvent != null)
			{
				Delegate[] invocationList144 = this.OnAdminRevokeAllBansForUserRequestEvent.GetInvocationList();
				foreach (Delegate delegate144 in invocationList144)
				{
					if (object.ReferenceEquals(delegate144.Target, instance))
					{
						OnAdminRevokeAllBansForUserRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.RevokeAllBansForUserRequest>)delegate144;
					}
				}
			}
			if (this.OnAdminRevokeAllBansForUserResultEvent != null)
			{
				Delegate[] invocationList145 = this.OnAdminRevokeAllBansForUserResultEvent.GetInvocationList();
				foreach (Delegate delegate145 in invocationList145)
				{
					if (object.ReferenceEquals(delegate145.Target, instance))
					{
						OnAdminRevokeAllBansForUserResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.RevokeAllBansForUserResult>)delegate145;
					}
				}
			}
			if (this.OnAdminRevokeBansRequestEvent != null)
			{
				Delegate[] invocationList146 = this.OnAdminRevokeBansRequestEvent.GetInvocationList();
				foreach (Delegate delegate146 in invocationList146)
				{
					if (object.ReferenceEquals(delegate146.Target, instance))
					{
						OnAdminRevokeBansRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.RevokeBansRequest>)delegate146;
					}
				}
			}
			if (this.OnAdminRevokeBansResultEvent != null)
			{
				Delegate[] invocationList147 = this.OnAdminRevokeBansResultEvent.GetInvocationList();
				foreach (Delegate delegate147 in invocationList147)
				{
					if (object.ReferenceEquals(delegate147.Target, instance))
					{
						OnAdminRevokeBansResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.RevokeBansResult>)delegate147;
					}
				}
			}
			if (this.OnAdminRevokeInventoryItemRequestEvent != null)
			{
				Delegate[] invocationList148 = this.OnAdminRevokeInventoryItemRequestEvent.GetInvocationList();
				foreach (Delegate delegate148 in invocationList148)
				{
					if (object.ReferenceEquals(delegate148.Target, instance))
					{
						OnAdminRevokeInventoryItemRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.RevokeInventoryItemRequest>)delegate148;
					}
				}
			}
			if (this.OnAdminRevokeInventoryItemResultEvent != null)
			{
				Delegate[] invocationList149 = this.OnAdminRevokeInventoryItemResultEvent.GetInvocationList();
				foreach (Delegate delegate149 in invocationList149)
				{
					if (object.ReferenceEquals(delegate149.Target, instance))
					{
						OnAdminRevokeInventoryItemResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.RevokeInventoryResult>)delegate149;
					}
				}
			}
			if (this.OnAdminRunTaskRequestEvent != null)
			{
				Delegate[] invocationList150 = this.OnAdminRunTaskRequestEvent.GetInvocationList();
				foreach (Delegate delegate150 in invocationList150)
				{
					if (object.ReferenceEquals(delegate150.Target, instance))
					{
						OnAdminRunTaskRequestEvent -= (PlayFabRequestEvent<RunTaskRequest>)delegate150;
					}
				}
			}
			if (this.OnAdminRunTaskResultEvent != null)
			{
				Delegate[] invocationList151 = this.OnAdminRunTaskResultEvent.GetInvocationList();
				foreach (Delegate delegate151 in invocationList151)
				{
					if (object.ReferenceEquals(delegate151.Target, instance))
					{
						OnAdminRunTaskResultEvent -= (PlayFabResultEvent<RunTaskResult>)delegate151;
					}
				}
			}
			if (this.OnAdminSendAccountRecoveryEmailRequestEvent != null)
			{
				Delegate[] invocationList152 = this.OnAdminSendAccountRecoveryEmailRequestEvent.GetInvocationList();
				foreach (Delegate delegate152 in invocationList152)
				{
					if (object.ReferenceEquals(delegate152.Target, instance))
					{
						OnAdminSendAccountRecoveryEmailRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.SendAccountRecoveryEmailRequest>)delegate152;
					}
				}
			}
			if (this.OnAdminSendAccountRecoveryEmailResultEvent != null)
			{
				Delegate[] invocationList153 = this.OnAdminSendAccountRecoveryEmailResultEvent.GetInvocationList();
				foreach (Delegate delegate153 in invocationList153)
				{
					if (object.ReferenceEquals(delegate153.Target, instance))
					{
						OnAdminSendAccountRecoveryEmailResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.SendAccountRecoveryEmailResult>)delegate153;
					}
				}
			}
			if (this.OnAdminSetCatalogItemsRequestEvent != null)
			{
				Delegate[] invocationList154 = this.OnAdminSetCatalogItemsRequestEvent.GetInvocationList();
				foreach (Delegate delegate154 in invocationList154)
				{
					if (object.ReferenceEquals(delegate154.Target, instance))
					{
						OnAdminSetCatalogItemsRequestEvent -= (PlayFabRequestEvent<UpdateCatalogItemsRequest>)delegate154;
					}
				}
			}
			if (this.OnAdminSetCatalogItemsResultEvent != null)
			{
				Delegate[] invocationList155 = this.OnAdminSetCatalogItemsResultEvent.GetInvocationList();
				foreach (Delegate delegate155 in invocationList155)
				{
					if (object.ReferenceEquals(delegate155.Target, instance))
					{
						OnAdminSetCatalogItemsResultEvent -= (PlayFabResultEvent<UpdateCatalogItemsResult>)delegate155;
					}
				}
			}
			if (this.OnAdminSetPlayerSecretRequestEvent != null)
			{
				Delegate[] invocationList156 = this.OnAdminSetPlayerSecretRequestEvent.GetInvocationList();
				foreach (Delegate delegate156 in invocationList156)
				{
					if (object.ReferenceEquals(delegate156.Target, instance))
					{
						OnAdminSetPlayerSecretRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.SetPlayerSecretRequest>)delegate156;
					}
				}
			}
			if (this.OnAdminSetPlayerSecretResultEvent != null)
			{
				Delegate[] invocationList157 = this.OnAdminSetPlayerSecretResultEvent.GetInvocationList();
				foreach (Delegate delegate157 in invocationList157)
				{
					if (object.ReferenceEquals(delegate157.Target, instance))
					{
						OnAdminSetPlayerSecretResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.SetPlayerSecretResult>)delegate157;
					}
				}
			}
			if (this.OnAdminSetPublishedRevisionRequestEvent != null)
			{
				Delegate[] invocationList158 = this.OnAdminSetPublishedRevisionRequestEvent.GetInvocationList();
				foreach (Delegate delegate158 in invocationList158)
				{
					if (object.ReferenceEquals(delegate158.Target, instance))
					{
						OnAdminSetPublishedRevisionRequestEvent -= (PlayFabRequestEvent<SetPublishedRevisionRequest>)delegate158;
					}
				}
			}
			if (this.OnAdminSetPublishedRevisionResultEvent != null)
			{
				Delegate[] invocationList159 = this.OnAdminSetPublishedRevisionResultEvent.GetInvocationList();
				foreach (Delegate delegate159 in invocationList159)
				{
					if (object.ReferenceEquals(delegate159.Target, instance))
					{
						OnAdminSetPublishedRevisionResultEvent -= (PlayFabResultEvent<SetPublishedRevisionResult>)delegate159;
					}
				}
			}
			if (this.OnAdminSetPublisherDataRequestEvent != null)
			{
				Delegate[] invocationList160 = this.OnAdminSetPublisherDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate160 in invocationList160)
				{
					if (object.ReferenceEquals(delegate160.Target, instance))
					{
						OnAdminSetPublisherDataRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.SetPublisherDataRequest>)delegate160;
					}
				}
			}
			if (this.OnAdminSetPublisherDataResultEvent != null)
			{
				Delegate[] invocationList161 = this.OnAdminSetPublisherDataResultEvent.GetInvocationList();
				foreach (Delegate delegate161 in invocationList161)
				{
					if (object.ReferenceEquals(delegate161.Target, instance))
					{
						OnAdminSetPublisherDataResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.SetPublisherDataResult>)delegate161;
					}
				}
			}
			if (this.OnAdminSetStoreItemsRequestEvent != null)
			{
				Delegate[] invocationList162 = this.OnAdminSetStoreItemsRequestEvent.GetInvocationList();
				foreach (Delegate delegate162 in invocationList162)
				{
					if (object.ReferenceEquals(delegate162.Target, instance))
					{
						OnAdminSetStoreItemsRequestEvent -= (PlayFabRequestEvent<UpdateStoreItemsRequest>)delegate162;
					}
				}
			}
			if (this.OnAdminSetStoreItemsResultEvent != null)
			{
				Delegate[] invocationList163 = this.OnAdminSetStoreItemsResultEvent.GetInvocationList();
				foreach (Delegate delegate163 in invocationList163)
				{
					if (object.ReferenceEquals(delegate163.Target, instance))
					{
						OnAdminSetStoreItemsResultEvent -= (PlayFabResultEvent<UpdateStoreItemsResult>)delegate163;
					}
				}
			}
			if (this.OnAdminSetTitleDataRequestEvent != null)
			{
				Delegate[] invocationList164 = this.OnAdminSetTitleDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate164 in invocationList164)
				{
					if (object.ReferenceEquals(delegate164.Target, instance))
					{
						OnAdminSetTitleDataRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.SetTitleDataRequest>)delegate164;
					}
				}
			}
			if (this.OnAdminSetTitleDataResultEvent != null)
			{
				Delegate[] invocationList165 = this.OnAdminSetTitleDataResultEvent.GetInvocationList();
				foreach (Delegate delegate165 in invocationList165)
				{
					if (object.ReferenceEquals(delegate165.Target, instance))
					{
						OnAdminSetTitleDataResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.SetTitleDataResult>)delegate165;
					}
				}
			}
			if (this.OnAdminSetTitleInternalDataRequestEvent != null)
			{
				Delegate[] invocationList166 = this.OnAdminSetTitleInternalDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate166 in invocationList166)
				{
					if (object.ReferenceEquals(delegate166.Target, instance))
					{
						OnAdminSetTitleInternalDataRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.SetTitleDataRequest>)delegate166;
					}
				}
			}
			if (this.OnAdminSetTitleInternalDataResultEvent != null)
			{
				Delegate[] invocationList167 = this.OnAdminSetTitleInternalDataResultEvent.GetInvocationList();
				foreach (Delegate delegate167 in invocationList167)
				{
					if (object.ReferenceEquals(delegate167.Target, instance))
					{
						OnAdminSetTitleInternalDataResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.SetTitleDataResult>)delegate167;
					}
				}
			}
			if (this.OnAdminSetupPushNotificationRequestEvent != null)
			{
				Delegate[] invocationList168 = this.OnAdminSetupPushNotificationRequestEvent.GetInvocationList();
				foreach (Delegate delegate168 in invocationList168)
				{
					if (object.ReferenceEquals(delegate168.Target, instance))
					{
						OnAdminSetupPushNotificationRequestEvent -= (PlayFabRequestEvent<SetupPushNotificationRequest>)delegate168;
					}
				}
			}
			if (this.OnAdminSetupPushNotificationResultEvent != null)
			{
				Delegate[] invocationList169 = this.OnAdminSetupPushNotificationResultEvent.GetInvocationList();
				foreach (Delegate delegate169 in invocationList169)
				{
					if (object.ReferenceEquals(delegate169.Target, instance))
					{
						OnAdminSetupPushNotificationResultEvent -= (PlayFabResultEvent<SetupPushNotificationResult>)delegate169;
					}
				}
			}
			if (this.OnAdminSubtractUserVirtualCurrencyRequestEvent != null)
			{
				Delegate[] invocationList170 = this.OnAdminSubtractUserVirtualCurrencyRequestEvent.GetInvocationList();
				foreach (Delegate delegate170 in invocationList170)
				{
					if (object.ReferenceEquals(delegate170.Target, instance))
					{
						OnAdminSubtractUserVirtualCurrencyRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.SubtractUserVirtualCurrencyRequest>)delegate170;
					}
				}
			}
			if (this.OnAdminSubtractUserVirtualCurrencyResultEvent != null)
			{
				Delegate[] invocationList171 = this.OnAdminSubtractUserVirtualCurrencyResultEvent.GetInvocationList();
				foreach (Delegate delegate171 in invocationList171)
				{
					if (object.ReferenceEquals(delegate171.Target, instance))
					{
						OnAdminSubtractUserVirtualCurrencyResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.ModifyUserVirtualCurrencyResult>)delegate171;
					}
				}
			}
			if (this.OnAdminUpdateBansRequestEvent != null)
			{
				Delegate[] invocationList172 = this.OnAdminUpdateBansRequestEvent.GetInvocationList();
				foreach (Delegate delegate172 in invocationList172)
				{
					if (object.ReferenceEquals(delegate172.Target, instance))
					{
						OnAdminUpdateBansRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.UpdateBansRequest>)delegate172;
					}
				}
			}
			if (this.OnAdminUpdateBansResultEvent != null)
			{
				Delegate[] invocationList173 = this.OnAdminUpdateBansResultEvent.GetInvocationList();
				foreach (Delegate delegate173 in invocationList173)
				{
					if (object.ReferenceEquals(delegate173.Target, instance))
					{
						OnAdminUpdateBansResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.UpdateBansResult>)delegate173;
					}
				}
			}
			if (this.OnAdminUpdateCatalogItemsRequestEvent != null)
			{
				Delegate[] invocationList174 = this.OnAdminUpdateCatalogItemsRequestEvent.GetInvocationList();
				foreach (Delegate delegate174 in invocationList174)
				{
					if (object.ReferenceEquals(delegate174.Target, instance))
					{
						OnAdminUpdateCatalogItemsRequestEvent -= (PlayFabRequestEvent<UpdateCatalogItemsRequest>)delegate174;
					}
				}
			}
			if (this.OnAdminUpdateCatalogItemsResultEvent != null)
			{
				Delegate[] invocationList175 = this.OnAdminUpdateCatalogItemsResultEvent.GetInvocationList();
				foreach (Delegate delegate175 in invocationList175)
				{
					if (object.ReferenceEquals(delegate175.Target, instance))
					{
						OnAdminUpdateCatalogItemsResultEvent -= (PlayFabResultEvent<UpdateCatalogItemsResult>)delegate175;
					}
				}
			}
			if (this.OnAdminUpdateCloudScriptRequestEvent != null)
			{
				Delegate[] invocationList176 = this.OnAdminUpdateCloudScriptRequestEvent.GetInvocationList();
				foreach (Delegate delegate176 in invocationList176)
				{
					if (object.ReferenceEquals(delegate176.Target, instance))
					{
						OnAdminUpdateCloudScriptRequestEvent -= (PlayFabRequestEvent<UpdateCloudScriptRequest>)delegate176;
					}
				}
			}
			if (this.OnAdminUpdateCloudScriptResultEvent != null)
			{
				Delegate[] invocationList177 = this.OnAdminUpdateCloudScriptResultEvent.GetInvocationList();
				foreach (Delegate delegate177 in invocationList177)
				{
					if (object.ReferenceEquals(delegate177.Target, instance))
					{
						OnAdminUpdateCloudScriptResultEvent -= (PlayFabResultEvent<UpdateCloudScriptResult>)delegate177;
					}
				}
			}
			if (this.OnAdminUpdatePlayerSharedSecretRequestEvent != null)
			{
				Delegate[] invocationList178 = this.OnAdminUpdatePlayerSharedSecretRequestEvent.GetInvocationList();
				foreach (Delegate delegate178 in invocationList178)
				{
					if (object.ReferenceEquals(delegate178.Target, instance))
					{
						OnAdminUpdatePlayerSharedSecretRequestEvent -= (PlayFabRequestEvent<UpdatePlayerSharedSecretRequest>)delegate178;
					}
				}
			}
			if (this.OnAdminUpdatePlayerSharedSecretResultEvent != null)
			{
				Delegate[] invocationList179 = this.OnAdminUpdatePlayerSharedSecretResultEvent.GetInvocationList();
				foreach (Delegate delegate179 in invocationList179)
				{
					if (object.ReferenceEquals(delegate179.Target, instance))
					{
						OnAdminUpdatePlayerSharedSecretResultEvent -= (PlayFabResultEvent<UpdatePlayerSharedSecretResult>)delegate179;
					}
				}
			}
			if (this.OnAdminUpdatePlayerStatisticDefinitionRequestEvent != null)
			{
				Delegate[] invocationList180 = this.OnAdminUpdatePlayerStatisticDefinitionRequestEvent.GetInvocationList();
				foreach (Delegate delegate180 in invocationList180)
				{
					if (object.ReferenceEquals(delegate180.Target, instance))
					{
						OnAdminUpdatePlayerStatisticDefinitionRequestEvent -= (PlayFabRequestEvent<UpdatePlayerStatisticDefinitionRequest>)delegate180;
					}
				}
			}
			if (this.OnAdminUpdatePlayerStatisticDefinitionResultEvent != null)
			{
				Delegate[] invocationList181 = this.OnAdminUpdatePlayerStatisticDefinitionResultEvent.GetInvocationList();
				foreach (Delegate delegate181 in invocationList181)
				{
					if (object.ReferenceEquals(delegate181.Target, instance))
					{
						OnAdminUpdatePlayerStatisticDefinitionResultEvent -= (PlayFabResultEvent<UpdatePlayerStatisticDefinitionResult>)delegate181;
					}
				}
			}
			if (this.OnAdminUpdatePolicyRequestEvent != null)
			{
				Delegate[] invocationList182 = this.OnAdminUpdatePolicyRequestEvent.GetInvocationList();
				foreach (Delegate delegate182 in invocationList182)
				{
					if (object.ReferenceEquals(delegate182.Target, instance))
					{
						OnAdminUpdatePolicyRequestEvent -= (PlayFabRequestEvent<UpdatePolicyRequest>)delegate182;
					}
				}
			}
			if (this.OnAdminUpdatePolicyResultEvent != null)
			{
				Delegate[] invocationList183 = this.OnAdminUpdatePolicyResultEvent.GetInvocationList();
				foreach (Delegate delegate183 in invocationList183)
				{
					if (object.ReferenceEquals(delegate183.Target, instance))
					{
						OnAdminUpdatePolicyResultEvent -= (PlayFabResultEvent<UpdatePolicyResponse>)delegate183;
					}
				}
			}
			if (this.OnAdminUpdateRandomResultTablesRequestEvent != null)
			{
				Delegate[] invocationList184 = this.OnAdminUpdateRandomResultTablesRequestEvent.GetInvocationList();
				foreach (Delegate delegate184 in invocationList184)
				{
					if (object.ReferenceEquals(delegate184.Target, instance))
					{
						OnAdminUpdateRandomResultTablesRequestEvent -= (PlayFabRequestEvent<UpdateRandomResultTablesRequest>)delegate184;
					}
				}
			}
			if (this.OnAdminUpdateRandomResultTablesResultEvent != null)
			{
				Delegate[] invocationList185 = this.OnAdminUpdateRandomResultTablesResultEvent.GetInvocationList();
				foreach (Delegate delegate185 in invocationList185)
				{
					if (object.ReferenceEquals(delegate185.Target, instance))
					{
						OnAdminUpdateRandomResultTablesResultEvent -= (PlayFabResultEvent<UpdateRandomResultTablesResult>)delegate185;
					}
				}
			}
			if (this.OnAdminUpdateStoreItemsRequestEvent != null)
			{
				Delegate[] invocationList186 = this.OnAdminUpdateStoreItemsRequestEvent.GetInvocationList();
				foreach (Delegate delegate186 in invocationList186)
				{
					if (object.ReferenceEquals(delegate186.Target, instance))
					{
						OnAdminUpdateStoreItemsRequestEvent -= (PlayFabRequestEvent<UpdateStoreItemsRequest>)delegate186;
					}
				}
			}
			if (this.OnAdminUpdateStoreItemsResultEvent != null)
			{
				Delegate[] invocationList187 = this.OnAdminUpdateStoreItemsResultEvent.GetInvocationList();
				foreach (Delegate delegate187 in invocationList187)
				{
					if (object.ReferenceEquals(delegate187.Target, instance))
					{
						OnAdminUpdateStoreItemsResultEvent -= (PlayFabResultEvent<UpdateStoreItemsResult>)delegate187;
					}
				}
			}
			if (this.OnAdminUpdateTaskRequestEvent != null)
			{
				Delegate[] invocationList188 = this.OnAdminUpdateTaskRequestEvent.GetInvocationList();
				foreach (Delegate delegate188 in invocationList188)
				{
					if (object.ReferenceEquals(delegate188.Target, instance))
					{
						OnAdminUpdateTaskRequestEvent -= (PlayFabRequestEvent<UpdateTaskRequest>)delegate188;
					}
				}
			}
			if (this.OnAdminUpdateTaskResultEvent != null)
			{
				Delegate[] invocationList189 = this.OnAdminUpdateTaskResultEvent.GetInvocationList();
				foreach (Delegate delegate189 in invocationList189)
				{
					if (object.ReferenceEquals(delegate189.Target, instance))
					{
						OnAdminUpdateTaskResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.EmptyResult>)delegate189;
					}
				}
			}
			if (this.OnAdminUpdateUserDataRequestEvent != null)
			{
				Delegate[] invocationList190 = this.OnAdminUpdateUserDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate190 in invocationList190)
				{
					if (object.ReferenceEquals(delegate190.Target, instance))
					{
						OnAdminUpdateUserDataRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.UpdateUserDataRequest>)delegate190;
					}
				}
			}
			if (this.OnAdminUpdateUserDataResultEvent != null)
			{
				Delegate[] invocationList191 = this.OnAdminUpdateUserDataResultEvent.GetInvocationList();
				foreach (Delegate delegate191 in invocationList191)
				{
					if (object.ReferenceEquals(delegate191.Target, instance))
					{
						OnAdminUpdateUserDataResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.UpdateUserDataResult>)delegate191;
					}
				}
			}
			if (this.OnAdminUpdateUserInternalDataRequestEvent != null)
			{
				Delegate[] invocationList192 = this.OnAdminUpdateUserInternalDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate192 in invocationList192)
				{
					if (object.ReferenceEquals(delegate192.Target, instance))
					{
						OnAdminUpdateUserInternalDataRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.UpdateUserInternalDataRequest>)delegate192;
					}
				}
			}
			if (this.OnAdminUpdateUserInternalDataResultEvent != null)
			{
				Delegate[] invocationList193 = this.OnAdminUpdateUserInternalDataResultEvent.GetInvocationList();
				foreach (Delegate delegate193 in invocationList193)
				{
					if (object.ReferenceEquals(delegate193.Target, instance))
					{
						OnAdminUpdateUserInternalDataResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.UpdateUserDataResult>)delegate193;
					}
				}
			}
			if (this.OnAdminUpdateUserPublisherDataRequestEvent != null)
			{
				Delegate[] invocationList194 = this.OnAdminUpdateUserPublisherDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate194 in invocationList194)
				{
					if (object.ReferenceEquals(delegate194.Target, instance))
					{
						OnAdminUpdateUserPublisherDataRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.UpdateUserDataRequest>)delegate194;
					}
				}
			}
			if (this.OnAdminUpdateUserPublisherDataResultEvent != null)
			{
				Delegate[] invocationList195 = this.OnAdminUpdateUserPublisherDataResultEvent.GetInvocationList();
				foreach (Delegate delegate195 in invocationList195)
				{
					if (object.ReferenceEquals(delegate195.Target, instance))
					{
						OnAdminUpdateUserPublisherDataResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.UpdateUserDataResult>)delegate195;
					}
				}
			}
			if (this.OnAdminUpdateUserPublisherInternalDataRequestEvent != null)
			{
				Delegate[] invocationList196 = this.OnAdminUpdateUserPublisherInternalDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate196 in invocationList196)
				{
					if (object.ReferenceEquals(delegate196.Target, instance))
					{
						OnAdminUpdateUserPublisherInternalDataRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.UpdateUserInternalDataRequest>)delegate196;
					}
				}
			}
			if (this.OnAdminUpdateUserPublisherInternalDataResultEvent != null)
			{
				Delegate[] invocationList197 = this.OnAdminUpdateUserPublisherInternalDataResultEvent.GetInvocationList();
				foreach (Delegate delegate197 in invocationList197)
				{
					if (object.ReferenceEquals(delegate197.Target, instance))
					{
						OnAdminUpdateUserPublisherInternalDataResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.UpdateUserDataResult>)delegate197;
					}
				}
			}
			if (this.OnAdminUpdateUserPublisherReadOnlyDataRequestEvent != null)
			{
				Delegate[] invocationList198 = this.OnAdminUpdateUserPublisherReadOnlyDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate198 in invocationList198)
				{
					if (object.ReferenceEquals(delegate198.Target, instance))
					{
						OnAdminUpdateUserPublisherReadOnlyDataRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.UpdateUserDataRequest>)delegate198;
					}
				}
			}
			if (this.OnAdminUpdateUserPublisherReadOnlyDataResultEvent != null)
			{
				Delegate[] invocationList199 = this.OnAdminUpdateUserPublisherReadOnlyDataResultEvent.GetInvocationList();
				foreach (Delegate delegate199 in invocationList199)
				{
					if (object.ReferenceEquals(delegate199.Target, instance))
					{
						OnAdminUpdateUserPublisherReadOnlyDataResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.UpdateUserDataResult>)delegate199;
					}
				}
			}
			if (this.OnAdminUpdateUserReadOnlyDataRequestEvent != null)
			{
				Delegate[] invocationList200 = this.OnAdminUpdateUserReadOnlyDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate200 in invocationList200)
				{
					if (object.ReferenceEquals(delegate200.Target, instance))
					{
						OnAdminUpdateUserReadOnlyDataRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.UpdateUserDataRequest>)delegate200;
					}
				}
			}
			if (this.OnAdminUpdateUserReadOnlyDataResultEvent != null)
			{
				Delegate[] invocationList201 = this.OnAdminUpdateUserReadOnlyDataResultEvent.GetInvocationList();
				foreach (Delegate delegate201 in invocationList201)
				{
					if (object.ReferenceEquals(delegate201.Target, instance))
					{
						OnAdminUpdateUserReadOnlyDataResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.UpdateUserDataResult>)delegate201;
					}
				}
			}
			if (this.OnAdminUpdateUserTitleDisplayNameRequestEvent != null)
			{
				Delegate[] invocationList202 = this.OnAdminUpdateUserTitleDisplayNameRequestEvent.GetInvocationList();
				foreach (Delegate delegate202 in invocationList202)
				{
					if (object.ReferenceEquals(delegate202.Target, instance))
					{
						OnAdminUpdateUserTitleDisplayNameRequestEvent -= (PlayFabRequestEvent<PlayFab.AdminModels.UpdateUserTitleDisplayNameRequest>)delegate202;
					}
				}
			}
			if (this.OnAdminUpdateUserTitleDisplayNameResultEvent != null)
			{
				Delegate[] invocationList203 = this.OnAdminUpdateUserTitleDisplayNameResultEvent.GetInvocationList();
				foreach (Delegate delegate203 in invocationList203)
				{
					if (object.ReferenceEquals(delegate203.Target, instance))
					{
						OnAdminUpdateUserTitleDisplayNameResultEvent -= (PlayFabResultEvent<PlayFab.AdminModels.UpdateUserTitleDisplayNameResult>)delegate203;
					}
				}
			}
			if (this.OnMatchmakerAuthUserRequestEvent != null)
			{
				Delegate[] invocationList204 = this.OnMatchmakerAuthUserRequestEvent.GetInvocationList();
				foreach (Delegate delegate204 in invocationList204)
				{
					if (object.ReferenceEquals(delegate204.Target, instance))
					{
						OnMatchmakerAuthUserRequestEvent -= (PlayFabRequestEvent<AuthUserRequest>)delegate204;
					}
				}
			}
			if (this.OnMatchmakerAuthUserResultEvent != null)
			{
				Delegate[] invocationList205 = this.OnMatchmakerAuthUserResultEvent.GetInvocationList();
				foreach (Delegate delegate205 in invocationList205)
				{
					if (object.ReferenceEquals(delegate205.Target, instance))
					{
						OnMatchmakerAuthUserResultEvent -= (PlayFabResultEvent<AuthUserResponse>)delegate205;
					}
				}
			}
			if (this.OnMatchmakerPlayerJoinedRequestEvent != null)
			{
				Delegate[] invocationList206 = this.OnMatchmakerPlayerJoinedRequestEvent.GetInvocationList();
				foreach (Delegate delegate206 in invocationList206)
				{
					if (object.ReferenceEquals(delegate206.Target, instance))
					{
						OnMatchmakerPlayerJoinedRequestEvent -= (PlayFabRequestEvent<PlayerJoinedRequest>)delegate206;
					}
				}
			}
			if (this.OnMatchmakerPlayerJoinedResultEvent != null)
			{
				Delegate[] invocationList207 = this.OnMatchmakerPlayerJoinedResultEvent.GetInvocationList();
				foreach (Delegate delegate207 in invocationList207)
				{
					if (object.ReferenceEquals(delegate207.Target, instance))
					{
						OnMatchmakerPlayerJoinedResultEvent -= (PlayFabResultEvent<PlayerJoinedResponse>)delegate207;
					}
				}
			}
			if (this.OnMatchmakerPlayerLeftRequestEvent != null)
			{
				Delegate[] invocationList208 = this.OnMatchmakerPlayerLeftRequestEvent.GetInvocationList();
				foreach (Delegate delegate208 in invocationList208)
				{
					if (object.ReferenceEquals(delegate208.Target, instance))
					{
						OnMatchmakerPlayerLeftRequestEvent -= (PlayFabRequestEvent<PlayerLeftRequest>)delegate208;
					}
				}
			}
			if (this.OnMatchmakerPlayerLeftResultEvent != null)
			{
				Delegate[] invocationList209 = this.OnMatchmakerPlayerLeftResultEvent.GetInvocationList();
				foreach (Delegate delegate209 in invocationList209)
				{
					if (object.ReferenceEquals(delegate209.Target, instance))
					{
						OnMatchmakerPlayerLeftResultEvent -= (PlayFabResultEvent<PlayerLeftResponse>)delegate209;
					}
				}
			}
			if (this.OnMatchmakerStartGameRequestEvent != null)
			{
				Delegate[] invocationList210 = this.OnMatchmakerStartGameRequestEvent.GetInvocationList();
				foreach (Delegate delegate210 in invocationList210)
				{
					if (object.ReferenceEquals(delegate210.Target, instance))
					{
						OnMatchmakerStartGameRequestEvent -= (PlayFabRequestEvent<PlayFab.MatchmakerModels.StartGameRequest>)delegate210;
					}
				}
			}
			if (this.OnMatchmakerStartGameResultEvent != null)
			{
				Delegate[] invocationList211 = this.OnMatchmakerStartGameResultEvent.GetInvocationList();
				foreach (Delegate delegate211 in invocationList211)
				{
					if (object.ReferenceEquals(delegate211.Target, instance))
					{
						OnMatchmakerStartGameResultEvent -= (PlayFabResultEvent<StartGameResponse>)delegate211;
					}
				}
			}
			if (this.OnMatchmakerUserInfoRequestEvent != null)
			{
				Delegate[] invocationList212 = this.OnMatchmakerUserInfoRequestEvent.GetInvocationList();
				foreach (Delegate delegate212 in invocationList212)
				{
					if (object.ReferenceEquals(delegate212.Target, instance))
					{
						OnMatchmakerUserInfoRequestEvent -= (PlayFabRequestEvent<UserInfoRequest>)delegate212;
					}
				}
			}
			if (this.OnMatchmakerUserInfoResultEvent != null)
			{
				Delegate[] invocationList213 = this.OnMatchmakerUserInfoResultEvent.GetInvocationList();
				foreach (Delegate delegate213 in invocationList213)
				{
					if (object.ReferenceEquals(delegate213.Target, instance))
					{
						OnMatchmakerUserInfoResultEvent -= (PlayFabResultEvent<UserInfoResponse>)delegate213;
					}
				}
			}
			if (this.OnServerAddCharacterVirtualCurrencyRequestEvent != null)
			{
				Delegate[] invocationList214 = this.OnServerAddCharacterVirtualCurrencyRequestEvent.GetInvocationList();
				foreach (Delegate delegate214 in invocationList214)
				{
					if (object.ReferenceEquals(delegate214.Target, instance))
					{
						OnServerAddCharacterVirtualCurrencyRequestEvent -= (PlayFabRequestEvent<AddCharacterVirtualCurrencyRequest>)delegate214;
					}
				}
			}
			if (this.OnServerAddCharacterVirtualCurrencyResultEvent != null)
			{
				Delegate[] invocationList215 = this.OnServerAddCharacterVirtualCurrencyResultEvent.GetInvocationList();
				foreach (Delegate delegate215 in invocationList215)
				{
					if (object.ReferenceEquals(delegate215.Target, instance))
					{
						OnServerAddCharacterVirtualCurrencyResultEvent -= (PlayFabResultEvent<ModifyCharacterVirtualCurrencyResult>)delegate215;
					}
				}
			}
			if (this.OnServerAddFriendRequestEvent != null)
			{
				Delegate[] invocationList216 = this.OnServerAddFriendRequestEvent.GetInvocationList();
				foreach (Delegate delegate216 in invocationList216)
				{
					if (object.ReferenceEquals(delegate216.Target, instance))
					{
						OnServerAddFriendRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.AddFriendRequest>)delegate216;
					}
				}
			}
			if (this.OnServerAddFriendResultEvent != null)
			{
				Delegate[] invocationList217 = this.OnServerAddFriendResultEvent.GetInvocationList();
				foreach (Delegate delegate217 in invocationList217)
				{
					if (object.ReferenceEquals(delegate217.Target, instance))
					{
						OnServerAddFriendResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.EmptyResult>)delegate217;
					}
				}
			}
			if (this.OnServerAddPlayerTagRequestEvent != null)
			{
				Delegate[] invocationList218 = this.OnServerAddPlayerTagRequestEvent.GetInvocationList();
				foreach (Delegate delegate218 in invocationList218)
				{
					if (object.ReferenceEquals(delegate218.Target, instance))
					{
						OnServerAddPlayerTagRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.AddPlayerTagRequest>)delegate218;
					}
				}
			}
			if (this.OnServerAddPlayerTagResultEvent != null)
			{
				Delegate[] invocationList219 = this.OnServerAddPlayerTagResultEvent.GetInvocationList();
				foreach (Delegate delegate219 in invocationList219)
				{
					if (object.ReferenceEquals(delegate219.Target, instance))
					{
						OnServerAddPlayerTagResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.AddPlayerTagResult>)delegate219;
					}
				}
			}
			if (this.OnServerAddSharedGroupMembersRequestEvent != null)
			{
				Delegate[] invocationList220 = this.OnServerAddSharedGroupMembersRequestEvent.GetInvocationList();
				foreach (Delegate delegate220 in invocationList220)
				{
					if (object.ReferenceEquals(delegate220.Target, instance))
					{
						OnServerAddSharedGroupMembersRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.AddSharedGroupMembersRequest>)delegate220;
					}
				}
			}
			if (this.OnServerAddSharedGroupMembersResultEvent != null)
			{
				Delegate[] invocationList221 = this.OnServerAddSharedGroupMembersResultEvent.GetInvocationList();
				foreach (Delegate delegate221 in invocationList221)
				{
					if (object.ReferenceEquals(delegate221.Target, instance))
					{
						OnServerAddSharedGroupMembersResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.AddSharedGroupMembersResult>)delegate221;
					}
				}
			}
			if (this.OnServerAddUserVirtualCurrencyRequestEvent != null)
			{
				Delegate[] invocationList222 = this.OnServerAddUserVirtualCurrencyRequestEvent.GetInvocationList();
				foreach (Delegate delegate222 in invocationList222)
				{
					if (object.ReferenceEquals(delegate222.Target, instance))
					{
						OnServerAddUserVirtualCurrencyRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.AddUserVirtualCurrencyRequest>)delegate222;
					}
				}
			}
			if (this.OnServerAddUserVirtualCurrencyResultEvent != null)
			{
				Delegate[] invocationList223 = this.OnServerAddUserVirtualCurrencyResultEvent.GetInvocationList();
				foreach (Delegate delegate223 in invocationList223)
				{
					if (object.ReferenceEquals(delegate223.Target, instance))
					{
						OnServerAddUserVirtualCurrencyResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.ModifyUserVirtualCurrencyResult>)delegate223;
					}
				}
			}
			if (this.OnServerAuthenticateSessionTicketRequestEvent != null)
			{
				Delegate[] invocationList224 = this.OnServerAuthenticateSessionTicketRequestEvent.GetInvocationList();
				foreach (Delegate delegate224 in invocationList224)
				{
					if (object.ReferenceEquals(delegate224.Target, instance))
					{
						OnServerAuthenticateSessionTicketRequestEvent -= (PlayFabRequestEvent<AuthenticateSessionTicketRequest>)delegate224;
					}
				}
			}
			if (this.OnServerAuthenticateSessionTicketResultEvent != null)
			{
				Delegate[] invocationList225 = this.OnServerAuthenticateSessionTicketResultEvent.GetInvocationList();
				foreach (Delegate delegate225 in invocationList225)
				{
					if (object.ReferenceEquals(delegate225.Target, instance))
					{
						OnServerAuthenticateSessionTicketResultEvent -= (PlayFabResultEvent<AuthenticateSessionTicketResult>)delegate225;
					}
				}
			}
			if (this.OnServerAwardSteamAchievementRequestEvent != null)
			{
				Delegate[] invocationList226 = this.OnServerAwardSteamAchievementRequestEvent.GetInvocationList();
				foreach (Delegate delegate226 in invocationList226)
				{
					if (object.ReferenceEquals(delegate226.Target, instance))
					{
						OnServerAwardSteamAchievementRequestEvent -= (PlayFabRequestEvent<AwardSteamAchievementRequest>)delegate226;
					}
				}
			}
			if (this.OnServerAwardSteamAchievementResultEvent != null)
			{
				Delegate[] invocationList227 = this.OnServerAwardSteamAchievementResultEvent.GetInvocationList();
				foreach (Delegate delegate227 in invocationList227)
				{
					if (object.ReferenceEquals(delegate227.Target, instance))
					{
						OnServerAwardSteamAchievementResultEvent -= (PlayFabResultEvent<AwardSteamAchievementResult>)delegate227;
					}
				}
			}
			if (this.OnServerBanUsersRequestEvent != null)
			{
				Delegate[] invocationList228 = this.OnServerBanUsersRequestEvent.GetInvocationList();
				foreach (Delegate delegate228 in invocationList228)
				{
					if (object.ReferenceEquals(delegate228.Target, instance))
					{
						OnServerBanUsersRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.BanUsersRequest>)delegate228;
					}
				}
			}
			if (this.OnServerBanUsersResultEvent != null)
			{
				Delegate[] invocationList229 = this.OnServerBanUsersResultEvent.GetInvocationList();
				foreach (Delegate delegate229 in invocationList229)
				{
					if (object.ReferenceEquals(delegate229.Target, instance))
					{
						OnServerBanUsersResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.BanUsersResult>)delegate229;
					}
				}
			}
			if (this.OnServerConsumeItemRequestEvent != null)
			{
				Delegate[] invocationList230 = this.OnServerConsumeItemRequestEvent.GetInvocationList();
				foreach (Delegate delegate230 in invocationList230)
				{
					if (object.ReferenceEquals(delegate230.Target, instance))
					{
						OnServerConsumeItemRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.ConsumeItemRequest>)delegate230;
					}
				}
			}
			if (this.OnServerConsumeItemResultEvent != null)
			{
				Delegate[] invocationList231 = this.OnServerConsumeItemResultEvent.GetInvocationList();
				foreach (Delegate delegate231 in invocationList231)
				{
					if (object.ReferenceEquals(delegate231.Target, instance))
					{
						OnServerConsumeItemResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.ConsumeItemResult>)delegate231;
					}
				}
			}
			if (this.OnServerCreateSharedGroupRequestEvent != null)
			{
				Delegate[] invocationList232 = this.OnServerCreateSharedGroupRequestEvent.GetInvocationList();
				foreach (Delegate delegate232 in invocationList232)
				{
					if (object.ReferenceEquals(delegate232.Target, instance))
					{
						OnServerCreateSharedGroupRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.CreateSharedGroupRequest>)delegate232;
					}
				}
			}
			if (this.OnServerCreateSharedGroupResultEvent != null)
			{
				Delegate[] invocationList233 = this.OnServerCreateSharedGroupResultEvent.GetInvocationList();
				foreach (Delegate delegate233 in invocationList233)
				{
					if (object.ReferenceEquals(delegate233.Target, instance))
					{
						OnServerCreateSharedGroupResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.CreateSharedGroupResult>)delegate233;
					}
				}
			}
			if (this.OnServerDeleteCharacterFromUserRequestEvent != null)
			{
				Delegate[] invocationList234 = this.OnServerDeleteCharacterFromUserRequestEvent.GetInvocationList();
				foreach (Delegate delegate234 in invocationList234)
				{
					if (object.ReferenceEquals(delegate234.Target, instance))
					{
						OnServerDeleteCharacterFromUserRequestEvent -= (PlayFabRequestEvent<DeleteCharacterFromUserRequest>)delegate234;
					}
				}
			}
			if (this.OnServerDeleteCharacterFromUserResultEvent != null)
			{
				Delegate[] invocationList235 = this.OnServerDeleteCharacterFromUserResultEvent.GetInvocationList();
				foreach (Delegate delegate235 in invocationList235)
				{
					if (object.ReferenceEquals(delegate235.Target, instance))
					{
						OnServerDeleteCharacterFromUserResultEvent -= (PlayFabResultEvent<DeleteCharacterFromUserResult>)delegate235;
					}
				}
			}
			if (this.OnServerDeleteSharedGroupRequestEvent != null)
			{
				Delegate[] invocationList236 = this.OnServerDeleteSharedGroupRequestEvent.GetInvocationList();
				foreach (Delegate delegate236 in invocationList236)
				{
					if (object.ReferenceEquals(delegate236.Target, instance))
					{
						OnServerDeleteSharedGroupRequestEvent -= (PlayFabRequestEvent<DeleteSharedGroupRequest>)delegate236;
					}
				}
			}
			if (this.OnServerDeleteSharedGroupResultEvent != null)
			{
				Delegate[] invocationList237 = this.OnServerDeleteSharedGroupResultEvent.GetInvocationList();
				foreach (Delegate delegate237 in invocationList237)
				{
					if (object.ReferenceEquals(delegate237.Target, instance))
					{
						OnServerDeleteSharedGroupResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.EmptyResult>)delegate237;
					}
				}
			}
			if (this.OnServerDeleteUsersRequestEvent != null)
			{
				Delegate[] invocationList238 = this.OnServerDeleteUsersRequestEvent.GetInvocationList();
				foreach (Delegate delegate238 in invocationList238)
				{
					if (object.ReferenceEquals(delegate238.Target, instance))
					{
						OnServerDeleteUsersRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.DeleteUsersRequest>)delegate238;
					}
				}
			}
			if (this.OnServerDeleteUsersResultEvent != null)
			{
				Delegate[] invocationList239 = this.OnServerDeleteUsersResultEvent.GetInvocationList();
				foreach (Delegate delegate239 in invocationList239)
				{
					if (object.ReferenceEquals(delegate239.Target, instance))
					{
						OnServerDeleteUsersResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.DeleteUsersResult>)delegate239;
					}
				}
			}
			if (this.OnServerDeregisterGameRequestEvent != null)
			{
				Delegate[] invocationList240 = this.OnServerDeregisterGameRequestEvent.GetInvocationList();
				foreach (Delegate delegate240 in invocationList240)
				{
					if (object.ReferenceEquals(delegate240.Target, instance))
					{
						OnServerDeregisterGameRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.DeregisterGameRequest>)delegate240;
					}
				}
			}
			if (this.OnServerDeregisterGameResultEvent != null)
			{
				Delegate[] invocationList241 = this.OnServerDeregisterGameResultEvent.GetInvocationList();
				foreach (Delegate delegate241 in invocationList241)
				{
					if (object.ReferenceEquals(delegate241.Target, instance))
					{
						OnServerDeregisterGameResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.DeregisterGameResponse>)delegate241;
					}
				}
			}
			if (this.OnServerEvaluateRandomResultTableRequestEvent != null)
			{
				Delegate[] invocationList242 = this.OnServerEvaluateRandomResultTableRequestEvent.GetInvocationList();
				foreach (Delegate delegate242 in invocationList242)
				{
					if (object.ReferenceEquals(delegate242.Target, instance))
					{
						OnServerEvaluateRandomResultTableRequestEvent -= (PlayFabRequestEvent<EvaluateRandomResultTableRequest>)delegate242;
					}
				}
			}
			if (this.OnServerEvaluateRandomResultTableResultEvent != null)
			{
				Delegate[] invocationList243 = this.OnServerEvaluateRandomResultTableResultEvent.GetInvocationList();
				foreach (Delegate delegate243 in invocationList243)
				{
					if (object.ReferenceEquals(delegate243.Target, instance))
					{
						OnServerEvaluateRandomResultTableResultEvent -= (PlayFabResultEvent<EvaluateRandomResultTableResult>)delegate243;
					}
				}
			}
			if (this.OnServerExecuteCloudScriptRequestEvent != null)
			{
				Delegate[] invocationList244 = this.OnServerExecuteCloudScriptRequestEvent.GetInvocationList();
				foreach (Delegate delegate244 in invocationList244)
				{
					if (object.ReferenceEquals(delegate244.Target, instance))
					{
						OnServerExecuteCloudScriptRequestEvent -= (PlayFabRequestEvent<ExecuteCloudScriptServerRequest>)delegate244;
					}
				}
			}
			if (this.OnServerExecuteCloudScriptResultEvent != null)
			{
				Delegate[] invocationList245 = this.OnServerExecuteCloudScriptResultEvent.GetInvocationList();
				foreach (Delegate delegate245 in invocationList245)
				{
					if (object.ReferenceEquals(delegate245.Target, instance))
					{
						OnServerExecuteCloudScriptResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.ExecuteCloudScriptResult>)delegate245;
					}
				}
			}
			if (this.OnServerGetAllSegmentsRequestEvent != null)
			{
				Delegate[] invocationList246 = this.OnServerGetAllSegmentsRequestEvent.GetInvocationList();
				foreach (Delegate delegate246 in invocationList246)
				{
					if (object.ReferenceEquals(delegate246.Target, instance))
					{
						OnServerGetAllSegmentsRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetAllSegmentsRequest>)delegate246;
					}
				}
			}
			if (this.OnServerGetAllSegmentsResultEvent != null)
			{
				Delegate[] invocationList247 = this.OnServerGetAllSegmentsResultEvent.GetInvocationList();
				foreach (Delegate delegate247 in invocationList247)
				{
					if (object.ReferenceEquals(delegate247.Target, instance))
					{
						OnServerGetAllSegmentsResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetAllSegmentsResult>)delegate247;
					}
				}
			}
			if (this.OnServerGetAllUsersCharactersRequestEvent != null)
			{
				Delegate[] invocationList248 = this.OnServerGetAllUsersCharactersRequestEvent.GetInvocationList();
				foreach (Delegate delegate248 in invocationList248)
				{
					if (object.ReferenceEquals(delegate248.Target, instance))
					{
						OnServerGetAllUsersCharactersRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.ListUsersCharactersRequest>)delegate248;
					}
				}
			}
			if (this.OnServerGetAllUsersCharactersResultEvent != null)
			{
				Delegate[] invocationList249 = this.OnServerGetAllUsersCharactersResultEvent.GetInvocationList();
				foreach (Delegate delegate249 in invocationList249)
				{
					if (object.ReferenceEquals(delegate249.Target, instance))
					{
						OnServerGetAllUsersCharactersResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.ListUsersCharactersResult>)delegate249;
					}
				}
			}
			if (this.OnServerGetCatalogItemsRequestEvent != null)
			{
				Delegate[] invocationList250 = this.OnServerGetCatalogItemsRequestEvent.GetInvocationList();
				foreach (Delegate delegate250 in invocationList250)
				{
					if (object.ReferenceEquals(delegate250.Target, instance))
					{
						OnServerGetCatalogItemsRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetCatalogItemsRequest>)delegate250;
					}
				}
			}
			if (this.OnServerGetCatalogItemsResultEvent != null)
			{
				Delegate[] invocationList251 = this.OnServerGetCatalogItemsResultEvent.GetInvocationList();
				foreach (Delegate delegate251 in invocationList251)
				{
					if (object.ReferenceEquals(delegate251.Target, instance))
					{
						OnServerGetCatalogItemsResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetCatalogItemsResult>)delegate251;
					}
				}
			}
			if (this.OnServerGetCharacterDataRequestEvent != null)
			{
				Delegate[] invocationList252 = this.OnServerGetCharacterDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate252 in invocationList252)
				{
					if (object.ReferenceEquals(delegate252.Target, instance))
					{
						OnServerGetCharacterDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetCharacterDataRequest>)delegate252;
					}
				}
			}
			if (this.OnServerGetCharacterDataResultEvent != null)
			{
				Delegate[] invocationList253 = this.OnServerGetCharacterDataResultEvent.GetInvocationList();
				foreach (Delegate delegate253 in invocationList253)
				{
					if (object.ReferenceEquals(delegate253.Target, instance))
					{
						OnServerGetCharacterDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetCharacterDataResult>)delegate253;
					}
				}
			}
			if (this.OnServerGetCharacterInternalDataRequestEvent != null)
			{
				Delegate[] invocationList254 = this.OnServerGetCharacterInternalDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate254 in invocationList254)
				{
					if (object.ReferenceEquals(delegate254.Target, instance))
					{
						OnServerGetCharacterInternalDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetCharacterDataRequest>)delegate254;
					}
				}
			}
			if (this.OnServerGetCharacterInternalDataResultEvent != null)
			{
				Delegate[] invocationList255 = this.OnServerGetCharacterInternalDataResultEvent.GetInvocationList();
				foreach (Delegate delegate255 in invocationList255)
				{
					if (object.ReferenceEquals(delegate255.Target, instance))
					{
						OnServerGetCharacterInternalDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetCharacterDataResult>)delegate255;
					}
				}
			}
			if (this.OnServerGetCharacterInventoryRequestEvent != null)
			{
				Delegate[] invocationList256 = this.OnServerGetCharacterInventoryRequestEvent.GetInvocationList();
				foreach (Delegate delegate256 in invocationList256)
				{
					if (object.ReferenceEquals(delegate256.Target, instance))
					{
						OnServerGetCharacterInventoryRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetCharacterInventoryRequest>)delegate256;
					}
				}
			}
			if (this.OnServerGetCharacterInventoryResultEvent != null)
			{
				Delegate[] invocationList257 = this.OnServerGetCharacterInventoryResultEvent.GetInvocationList();
				foreach (Delegate delegate257 in invocationList257)
				{
					if (object.ReferenceEquals(delegate257.Target, instance))
					{
						OnServerGetCharacterInventoryResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetCharacterInventoryResult>)delegate257;
					}
				}
			}
			if (this.OnServerGetCharacterLeaderboardRequestEvent != null)
			{
				Delegate[] invocationList258 = this.OnServerGetCharacterLeaderboardRequestEvent.GetInvocationList();
				foreach (Delegate delegate258 in invocationList258)
				{
					if (object.ReferenceEquals(delegate258.Target, instance))
					{
						OnServerGetCharacterLeaderboardRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetCharacterLeaderboardRequest>)delegate258;
					}
				}
			}
			if (this.OnServerGetCharacterLeaderboardResultEvent != null)
			{
				Delegate[] invocationList259 = this.OnServerGetCharacterLeaderboardResultEvent.GetInvocationList();
				foreach (Delegate delegate259 in invocationList259)
				{
					if (object.ReferenceEquals(delegate259.Target, instance))
					{
						OnServerGetCharacterLeaderboardResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetCharacterLeaderboardResult>)delegate259;
					}
				}
			}
			if (this.OnServerGetCharacterReadOnlyDataRequestEvent != null)
			{
				Delegate[] invocationList260 = this.OnServerGetCharacterReadOnlyDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate260 in invocationList260)
				{
					if (object.ReferenceEquals(delegate260.Target, instance))
					{
						OnServerGetCharacterReadOnlyDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetCharacterDataRequest>)delegate260;
					}
				}
			}
			if (this.OnServerGetCharacterReadOnlyDataResultEvent != null)
			{
				Delegate[] invocationList261 = this.OnServerGetCharacterReadOnlyDataResultEvent.GetInvocationList();
				foreach (Delegate delegate261 in invocationList261)
				{
					if (object.ReferenceEquals(delegate261.Target, instance))
					{
						OnServerGetCharacterReadOnlyDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetCharacterDataResult>)delegate261;
					}
				}
			}
			if (this.OnServerGetCharacterStatisticsRequestEvent != null)
			{
				Delegate[] invocationList262 = this.OnServerGetCharacterStatisticsRequestEvent.GetInvocationList();
				foreach (Delegate delegate262 in invocationList262)
				{
					if (object.ReferenceEquals(delegate262.Target, instance))
					{
						OnServerGetCharacterStatisticsRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetCharacterStatisticsRequest>)delegate262;
					}
				}
			}
			if (this.OnServerGetCharacterStatisticsResultEvent != null)
			{
				Delegate[] invocationList263 = this.OnServerGetCharacterStatisticsResultEvent.GetInvocationList();
				foreach (Delegate delegate263 in invocationList263)
				{
					if (object.ReferenceEquals(delegate263.Target, instance))
					{
						OnServerGetCharacterStatisticsResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetCharacterStatisticsResult>)delegate263;
					}
				}
			}
			if (this.OnServerGetContentDownloadUrlRequestEvent != null)
			{
				Delegate[] invocationList264 = this.OnServerGetContentDownloadUrlRequestEvent.GetInvocationList();
				foreach (Delegate delegate264 in invocationList264)
				{
					if (object.ReferenceEquals(delegate264.Target, instance))
					{
						OnServerGetContentDownloadUrlRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetContentDownloadUrlRequest>)delegate264;
					}
				}
			}
			if (this.OnServerGetContentDownloadUrlResultEvent != null)
			{
				Delegate[] invocationList265 = this.OnServerGetContentDownloadUrlResultEvent.GetInvocationList();
				foreach (Delegate delegate265 in invocationList265)
				{
					if (object.ReferenceEquals(delegate265.Target, instance))
					{
						OnServerGetContentDownloadUrlResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetContentDownloadUrlResult>)delegate265;
					}
				}
			}
			if (this.OnServerGetFriendLeaderboardRequestEvent != null)
			{
				Delegate[] invocationList266 = this.OnServerGetFriendLeaderboardRequestEvent.GetInvocationList();
				foreach (Delegate delegate266 in invocationList266)
				{
					if (object.ReferenceEquals(delegate266.Target, instance))
					{
						OnServerGetFriendLeaderboardRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetFriendLeaderboardRequest>)delegate266;
					}
				}
			}
			if (this.OnServerGetFriendLeaderboardResultEvent != null)
			{
				Delegate[] invocationList267 = this.OnServerGetFriendLeaderboardResultEvent.GetInvocationList();
				foreach (Delegate delegate267 in invocationList267)
				{
					if (object.ReferenceEquals(delegate267.Target, instance))
					{
						OnServerGetFriendLeaderboardResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetLeaderboardResult>)delegate267;
					}
				}
			}
			if (this.OnServerGetFriendsListRequestEvent != null)
			{
				Delegate[] invocationList268 = this.OnServerGetFriendsListRequestEvent.GetInvocationList();
				foreach (Delegate delegate268 in invocationList268)
				{
					if (object.ReferenceEquals(delegate268.Target, instance))
					{
						OnServerGetFriendsListRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetFriendsListRequest>)delegate268;
					}
				}
			}
			if (this.OnServerGetFriendsListResultEvent != null)
			{
				Delegate[] invocationList269 = this.OnServerGetFriendsListResultEvent.GetInvocationList();
				foreach (Delegate delegate269 in invocationList269)
				{
					if (object.ReferenceEquals(delegate269.Target, instance))
					{
						OnServerGetFriendsListResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetFriendsListResult>)delegate269;
					}
				}
			}
			if (this.OnServerGetLeaderboardRequestEvent != null)
			{
				Delegate[] invocationList270 = this.OnServerGetLeaderboardRequestEvent.GetInvocationList();
				foreach (Delegate delegate270 in invocationList270)
				{
					if (object.ReferenceEquals(delegate270.Target, instance))
					{
						OnServerGetLeaderboardRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetLeaderboardRequest>)delegate270;
					}
				}
			}
			if (this.OnServerGetLeaderboardResultEvent != null)
			{
				Delegate[] invocationList271 = this.OnServerGetLeaderboardResultEvent.GetInvocationList();
				foreach (Delegate delegate271 in invocationList271)
				{
					if (object.ReferenceEquals(delegate271.Target, instance))
					{
						OnServerGetLeaderboardResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetLeaderboardResult>)delegate271;
					}
				}
			}
			if (this.OnServerGetLeaderboardAroundCharacterRequestEvent != null)
			{
				Delegate[] invocationList272 = this.OnServerGetLeaderboardAroundCharacterRequestEvent.GetInvocationList();
				foreach (Delegate delegate272 in invocationList272)
				{
					if (object.ReferenceEquals(delegate272.Target, instance))
					{
						OnServerGetLeaderboardAroundCharacterRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetLeaderboardAroundCharacterRequest>)delegate272;
					}
				}
			}
			if (this.OnServerGetLeaderboardAroundCharacterResultEvent != null)
			{
				Delegate[] invocationList273 = this.OnServerGetLeaderboardAroundCharacterResultEvent.GetInvocationList();
				foreach (Delegate delegate273 in invocationList273)
				{
					if (object.ReferenceEquals(delegate273.Target, instance))
					{
						OnServerGetLeaderboardAroundCharacterResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetLeaderboardAroundCharacterResult>)delegate273;
					}
				}
			}
			if (this.OnServerGetLeaderboardAroundUserRequestEvent != null)
			{
				Delegate[] invocationList274 = this.OnServerGetLeaderboardAroundUserRequestEvent.GetInvocationList();
				foreach (Delegate delegate274 in invocationList274)
				{
					if (object.ReferenceEquals(delegate274.Target, instance))
					{
						OnServerGetLeaderboardAroundUserRequestEvent -= (PlayFabRequestEvent<GetLeaderboardAroundUserRequest>)delegate274;
					}
				}
			}
			if (this.OnServerGetLeaderboardAroundUserResultEvent != null)
			{
				Delegate[] invocationList275 = this.OnServerGetLeaderboardAroundUserResultEvent.GetInvocationList();
				foreach (Delegate delegate275 in invocationList275)
				{
					if (object.ReferenceEquals(delegate275.Target, instance))
					{
						OnServerGetLeaderboardAroundUserResultEvent -= (PlayFabResultEvent<GetLeaderboardAroundUserResult>)delegate275;
					}
				}
			}
			if (this.OnServerGetLeaderboardForUserCharactersRequestEvent != null)
			{
				Delegate[] invocationList276 = this.OnServerGetLeaderboardForUserCharactersRequestEvent.GetInvocationList();
				foreach (Delegate delegate276 in invocationList276)
				{
					if (object.ReferenceEquals(delegate276.Target, instance))
					{
						OnServerGetLeaderboardForUserCharactersRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetLeaderboardForUsersCharactersRequest>)delegate276;
					}
				}
			}
			if (this.OnServerGetLeaderboardForUserCharactersResultEvent != null)
			{
				Delegate[] invocationList277 = this.OnServerGetLeaderboardForUserCharactersResultEvent.GetInvocationList();
				foreach (Delegate delegate277 in invocationList277)
				{
					if (object.ReferenceEquals(delegate277.Target, instance))
					{
						OnServerGetLeaderboardForUserCharactersResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetLeaderboardForUsersCharactersResult>)delegate277;
					}
				}
			}
			if (this.OnServerGetPlayerCombinedInfoRequestEvent != null)
			{
				Delegate[] invocationList278 = this.OnServerGetPlayerCombinedInfoRequestEvent.GetInvocationList();
				foreach (Delegate delegate278 in invocationList278)
				{
					if (object.ReferenceEquals(delegate278.Target, instance))
					{
						OnServerGetPlayerCombinedInfoRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetPlayerCombinedInfoRequest>)delegate278;
					}
				}
			}
			if (this.OnServerGetPlayerCombinedInfoResultEvent != null)
			{
				Delegate[] invocationList279 = this.OnServerGetPlayerCombinedInfoResultEvent.GetInvocationList();
				foreach (Delegate delegate279 in invocationList279)
				{
					if (object.ReferenceEquals(delegate279.Target, instance))
					{
						OnServerGetPlayerCombinedInfoResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetPlayerCombinedInfoResult>)delegate279;
					}
				}
			}
			if (this.OnServerGetPlayerProfileRequestEvent != null)
			{
				Delegate[] invocationList280 = this.OnServerGetPlayerProfileRequestEvent.GetInvocationList();
				foreach (Delegate delegate280 in invocationList280)
				{
					if (object.ReferenceEquals(delegate280.Target, instance))
					{
						OnServerGetPlayerProfileRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetPlayerProfileRequest>)delegate280;
					}
				}
			}
			if (this.OnServerGetPlayerProfileResultEvent != null)
			{
				Delegate[] invocationList281 = this.OnServerGetPlayerProfileResultEvent.GetInvocationList();
				foreach (Delegate delegate281 in invocationList281)
				{
					if (object.ReferenceEquals(delegate281.Target, instance))
					{
						OnServerGetPlayerProfileResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetPlayerProfileResult>)delegate281;
					}
				}
			}
			if (this.OnServerGetPlayerSegmentsRequestEvent != null)
			{
				Delegate[] invocationList282 = this.OnServerGetPlayerSegmentsRequestEvent.GetInvocationList();
				foreach (Delegate delegate282 in invocationList282)
				{
					if (object.ReferenceEquals(delegate282.Target, instance))
					{
						OnServerGetPlayerSegmentsRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetPlayersSegmentsRequest>)delegate282;
					}
				}
			}
			if (this.OnServerGetPlayerSegmentsResultEvent != null)
			{
				Delegate[] invocationList283 = this.OnServerGetPlayerSegmentsResultEvent.GetInvocationList();
				foreach (Delegate delegate283 in invocationList283)
				{
					if (object.ReferenceEquals(delegate283.Target, instance))
					{
						OnServerGetPlayerSegmentsResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetPlayerSegmentsResult>)delegate283;
					}
				}
			}
			if (this.OnServerGetPlayersInSegmentRequestEvent != null)
			{
				Delegate[] invocationList284 = this.OnServerGetPlayersInSegmentRequestEvent.GetInvocationList();
				foreach (Delegate delegate284 in invocationList284)
				{
					if (object.ReferenceEquals(delegate284.Target, instance))
					{
						OnServerGetPlayersInSegmentRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetPlayersInSegmentRequest>)delegate284;
					}
				}
			}
			if (this.OnServerGetPlayersInSegmentResultEvent != null)
			{
				Delegate[] invocationList285 = this.OnServerGetPlayersInSegmentResultEvent.GetInvocationList();
				foreach (Delegate delegate285 in invocationList285)
				{
					if (object.ReferenceEquals(delegate285.Target, instance))
					{
						OnServerGetPlayersInSegmentResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetPlayersInSegmentResult>)delegate285;
					}
				}
			}
			if (this.OnServerGetPlayerStatisticsRequestEvent != null)
			{
				Delegate[] invocationList286 = this.OnServerGetPlayerStatisticsRequestEvent.GetInvocationList();
				foreach (Delegate delegate286 in invocationList286)
				{
					if (object.ReferenceEquals(delegate286.Target, instance))
					{
						OnServerGetPlayerStatisticsRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetPlayerStatisticsRequest>)delegate286;
					}
				}
			}
			if (this.OnServerGetPlayerStatisticsResultEvent != null)
			{
				Delegate[] invocationList287 = this.OnServerGetPlayerStatisticsResultEvent.GetInvocationList();
				foreach (Delegate delegate287 in invocationList287)
				{
					if (object.ReferenceEquals(delegate287.Target, instance))
					{
						OnServerGetPlayerStatisticsResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetPlayerStatisticsResult>)delegate287;
					}
				}
			}
			if (this.OnServerGetPlayerStatisticVersionsRequestEvent != null)
			{
				Delegate[] invocationList288 = this.OnServerGetPlayerStatisticVersionsRequestEvent.GetInvocationList();
				foreach (Delegate delegate288 in invocationList288)
				{
					if (object.ReferenceEquals(delegate288.Target, instance))
					{
						OnServerGetPlayerStatisticVersionsRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetPlayerStatisticVersionsRequest>)delegate288;
					}
				}
			}
			if (this.OnServerGetPlayerStatisticVersionsResultEvent != null)
			{
				Delegate[] invocationList289 = this.OnServerGetPlayerStatisticVersionsResultEvent.GetInvocationList();
				foreach (Delegate delegate289 in invocationList289)
				{
					if (object.ReferenceEquals(delegate289.Target, instance))
					{
						OnServerGetPlayerStatisticVersionsResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetPlayerStatisticVersionsResult>)delegate289;
					}
				}
			}
			if (this.OnServerGetPlayerTagsRequestEvent != null)
			{
				Delegate[] invocationList290 = this.OnServerGetPlayerTagsRequestEvent.GetInvocationList();
				foreach (Delegate delegate290 in invocationList290)
				{
					if (object.ReferenceEquals(delegate290.Target, instance))
					{
						OnServerGetPlayerTagsRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetPlayerTagsRequest>)delegate290;
					}
				}
			}
			if (this.OnServerGetPlayerTagsResultEvent != null)
			{
				Delegate[] invocationList291 = this.OnServerGetPlayerTagsResultEvent.GetInvocationList();
				foreach (Delegate delegate291 in invocationList291)
				{
					if (object.ReferenceEquals(delegate291.Target, instance))
					{
						OnServerGetPlayerTagsResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetPlayerTagsResult>)delegate291;
					}
				}
			}
			if (this.OnServerGetPlayFabIDsFromFacebookIDsRequestEvent != null)
			{
				Delegate[] invocationList292 = this.OnServerGetPlayFabIDsFromFacebookIDsRequestEvent.GetInvocationList();
				foreach (Delegate delegate292 in invocationList292)
				{
					if (object.ReferenceEquals(delegate292.Target, instance))
					{
						OnServerGetPlayFabIDsFromFacebookIDsRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetPlayFabIDsFromFacebookIDsRequest>)delegate292;
					}
				}
			}
			if (this.OnServerGetPlayFabIDsFromFacebookIDsResultEvent != null)
			{
				Delegate[] invocationList293 = this.OnServerGetPlayFabIDsFromFacebookIDsResultEvent.GetInvocationList();
				foreach (Delegate delegate293 in invocationList293)
				{
					if (object.ReferenceEquals(delegate293.Target, instance))
					{
						OnServerGetPlayFabIDsFromFacebookIDsResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetPlayFabIDsFromFacebookIDsResult>)delegate293;
					}
				}
			}
			if (this.OnServerGetPlayFabIDsFromSteamIDsRequestEvent != null)
			{
				Delegate[] invocationList294 = this.OnServerGetPlayFabIDsFromSteamIDsRequestEvent.GetInvocationList();
				foreach (Delegate delegate294 in invocationList294)
				{
					if (object.ReferenceEquals(delegate294.Target, instance))
					{
						OnServerGetPlayFabIDsFromSteamIDsRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetPlayFabIDsFromSteamIDsRequest>)delegate294;
					}
				}
			}
			if (this.OnServerGetPlayFabIDsFromSteamIDsResultEvent != null)
			{
				Delegate[] invocationList295 = this.OnServerGetPlayFabIDsFromSteamIDsResultEvent.GetInvocationList();
				foreach (Delegate delegate295 in invocationList295)
				{
					if (object.ReferenceEquals(delegate295.Target, instance))
					{
						OnServerGetPlayFabIDsFromSteamIDsResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetPlayFabIDsFromSteamIDsResult>)delegate295;
					}
				}
			}
			if (this.OnServerGetPublisherDataRequestEvent != null)
			{
				Delegate[] invocationList296 = this.OnServerGetPublisherDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate296 in invocationList296)
				{
					if (object.ReferenceEquals(delegate296.Target, instance))
					{
						OnServerGetPublisherDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetPublisherDataRequest>)delegate296;
					}
				}
			}
			if (this.OnServerGetPublisherDataResultEvent != null)
			{
				Delegate[] invocationList297 = this.OnServerGetPublisherDataResultEvent.GetInvocationList();
				foreach (Delegate delegate297 in invocationList297)
				{
					if (object.ReferenceEquals(delegate297.Target, instance))
					{
						OnServerGetPublisherDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetPublisherDataResult>)delegate297;
					}
				}
			}
			if (this.OnServerGetRandomResultTablesRequestEvent != null)
			{
				Delegate[] invocationList298 = this.OnServerGetRandomResultTablesRequestEvent.GetInvocationList();
				foreach (Delegate delegate298 in invocationList298)
				{
					if (object.ReferenceEquals(delegate298.Target, instance))
					{
						OnServerGetRandomResultTablesRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetRandomResultTablesRequest>)delegate298;
					}
				}
			}
			if (this.OnServerGetRandomResultTablesResultEvent != null)
			{
				Delegate[] invocationList299 = this.OnServerGetRandomResultTablesResultEvent.GetInvocationList();
				foreach (Delegate delegate299 in invocationList299)
				{
					if (object.ReferenceEquals(delegate299.Target, instance))
					{
						OnServerGetRandomResultTablesResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetRandomResultTablesResult>)delegate299;
					}
				}
			}
			if (this.OnServerGetSharedGroupDataRequestEvent != null)
			{
				Delegate[] invocationList300 = this.OnServerGetSharedGroupDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate300 in invocationList300)
				{
					if (object.ReferenceEquals(delegate300.Target, instance))
					{
						OnServerGetSharedGroupDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetSharedGroupDataRequest>)delegate300;
					}
				}
			}
			if (this.OnServerGetSharedGroupDataResultEvent != null)
			{
				Delegate[] invocationList301 = this.OnServerGetSharedGroupDataResultEvent.GetInvocationList();
				foreach (Delegate delegate301 in invocationList301)
				{
					if (object.ReferenceEquals(delegate301.Target, instance))
					{
						OnServerGetSharedGroupDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetSharedGroupDataResult>)delegate301;
					}
				}
			}
			if (this.OnServerGetTimeRequestEvent != null)
			{
				Delegate[] invocationList302 = this.OnServerGetTimeRequestEvent.GetInvocationList();
				foreach (Delegate delegate302 in invocationList302)
				{
					if (object.ReferenceEquals(delegate302.Target, instance))
					{
						OnServerGetTimeRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetTimeRequest>)delegate302;
					}
				}
			}
			if (this.OnServerGetTimeResultEvent != null)
			{
				Delegate[] invocationList303 = this.OnServerGetTimeResultEvent.GetInvocationList();
				foreach (Delegate delegate303 in invocationList303)
				{
					if (object.ReferenceEquals(delegate303.Target, instance))
					{
						OnServerGetTimeResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetTimeResult>)delegate303;
					}
				}
			}
			if (this.OnServerGetTitleDataRequestEvent != null)
			{
				Delegate[] invocationList304 = this.OnServerGetTitleDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate304 in invocationList304)
				{
					if (object.ReferenceEquals(delegate304.Target, instance))
					{
						OnServerGetTitleDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetTitleDataRequest>)delegate304;
					}
				}
			}
			if (this.OnServerGetTitleDataResultEvent != null)
			{
				Delegate[] invocationList305 = this.OnServerGetTitleDataResultEvent.GetInvocationList();
				foreach (Delegate delegate305 in invocationList305)
				{
					if (object.ReferenceEquals(delegate305.Target, instance))
					{
						OnServerGetTitleDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetTitleDataResult>)delegate305;
					}
				}
			}
			if (this.OnServerGetTitleInternalDataRequestEvent != null)
			{
				Delegate[] invocationList306 = this.OnServerGetTitleInternalDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate306 in invocationList306)
				{
					if (object.ReferenceEquals(delegate306.Target, instance))
					{
						OnServerGetTitleInternalDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetTitleDataRequest>)delegate306;
					}
				}
			}
			if (this.OnServerGetTitleInternalDataResultEvent != null)
			{
				Delegate[] invocationList307 = this.OnServerGetTitleInternalDataResultEvent.GetInvocationList();
				foreach (Delegate delegate307 in invocationList307)
				{
					if (object.ReferenceEquals(delegate307.Target, instance))
					{
						OnServerGetTitleInternalDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetTitleDataResult>)delegate307;
					}
				}
			}
			if (this.OnServerGetTitleNewsRequestEvent != null)
			{
				Delegate[] invocationList308 = this.OnServerGetTitleNewsRequestEvent.GetInvocationList();
				foreach (Delegate delegate308 in invocationList308)
				{
					if (object.ReferenceEquals(delegate308.Target, instance))
					{
						OnServerGetTitleNewsRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetTitleNewsRequest>)delegate308;
					}
				}
			}
			if (this.OnServerGetTitleNewsResultEvent != null)
			{
				Delegate[] invocationList309 = this.OnServerGetTitleNewsResultEvent.GetInvocationList();
				foreach (Delegate delegate309 in invocationList309)
				{
					if (object.ReferenceEquals(delegate309.Target, instance))
					{
						OnServerGetTitleNewsResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetTitleNewsResult>)delegate309;
					}
				}
			}
			if (this.OnServerGetUserAccountInfoRequestEvent != null)
			{
				Delegate[] invocationList310 = this.OnServerGetUserAccountInfoRequestEvent.GetInvocationList();
				foreach (Delegate delegate310 in invocationList310)
				{
					if (object.ReferenceEquals(delegate310.Target, instance))
					{
						OnServerGetUserAccountInfoRequestEvent -= (PlayFabRequestEvent<GetUserAccountInfoRequest>)delegate310;
					}
				}
			}
			if (this.OnServerGetUserAccountInfoResultEvent != null)
			{
				Delegate[] invocationList311 = this.OnServerGetUserAccountInfoResultEvent.GetInvocationList();
				foreach (Delegate delegate311 in invocationList311)
				{
					if (object.ReferenceEquals(delegate311.Target, instance))
					{
						OnServerGetUserAccountInfoResultEvent -= (PlayFabResultEvent<GetUserAccountInfoResult>)delegate311;
					}
				}
			}
			if (this.OnServerGetUserBansRequestEvent != null)
			{
				Delegate[] invocationList312 = this.OnServerGetUserBansRequestEvent.GetInvocationList();
				foreach (Delegate delegate312 in invocationList312)
				{
					if (object.ReferenceEquals(delegate312.Target, instance))
					{
						OnServerGetUserBansRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetUserBansRequest>)delegate312;
					}
				}
			}
			if (this.OnServerGetUserBansResultEvent != null)
			{
				Delegate[] invocationList313 = this.OnServerGetUserBansResultEvent.GetInvocationList();
				foreach (Delegate delegate313 in invocationList313)
				{
					if (object.ReferenceEquals(delegate313.Target, instance))
					{
						OnServerGetUserBansResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetUserBansResult>)delegate313;
					}
				}
			}
			if (this.OnServerGetUserDataRequestEvent != null)
			{
				Delegate[] invocationList314 = this.OnServerGetUserDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate314 in invocationList314)
				{
					if (object.ReferenceEquals(delegate314.Target, instance))
					{
						OnServerGetUserDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetUserDataRequest>)delegate314;
					}
				}
			}
			if (this.OnServerGetUserDataResultEvent != null)
			{
				Delegate[] invocationList315 = this.OnServerGetUserDataResultEvent.GetInvocationList();
				foreach (Delegate delegate315 in invocationList315)
				{
					if (object.ReferenceEquals(delegate315.Target, instance))
					{
						OnServerGetUserDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetUserDataResult>)delegate315;
					}
				}
			}
			if (this.OnServerGetUserInternalDataRequestEvent != null)
			{
				Delegate[] invocationList316 = this.OnServerGetUserInternalDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate316 in invocationList316)
				{
					if (object.ReferenceEquals(delegate316.Target, instance))
					{
						OnServerGetUserInternalDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetUserDataRequest>)delegate316;
					}
				}
			}
			if (this.OnServerGetUserInternalDataResultEvent != null)
			{
				Delegate[] invocationList317 = this.OnServerGetUserInternalDataResultEvent.GetInvocationList();
				foreach (Delegate delegate317 in invocationList317)
				{
					if (object.ReferenceEquals(delegate317.Target, instance))
					{
						OnServerGetUserInternalDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetUserDataResult>)delegate317;
					}
				}
			}
			if (this.OnServerGetUserInventoryRequestEvent != null)
			{
				Delegate[] invocationList318 = this.OnServerGetUserInventoryRequestEvent.GetInvocationList();
				foreach (Delegate delegate318 in invocationList318)
				{
					if (object.ReferenceEquals(delegate318.Target, instance))
					{
						OnServerGetUserInventoryRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetUserInventoryRequest>)delegate318;
					}
				}
			}
			if (this.OnServerGetUserInventoryResultEvent != null)
			{
				Delegate[] invocationList319 = this.OnServerGetUserInventoryResultEvent.GetInvocationList();
				foreach (Delegate delegate319 in invocationList319)
				{
					if (object.ReferenceEquals(delegate319.Target, instance))
					{
						OnServerGetUserInventoryResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetUserInventoryResult>)delegate319;
					}
				}
			}
			if (this.OnServerGetUserPublisherDataRequestEvent != null)
			{
				Delegate[] invocationList320 = this.OnServerGetUserPublisherDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate320 in invocationList320)
				{
					if (object.ReferenceEquals(delegate320.Target, instance))
					{
						OnServerGetUserPublisherDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetUserDataRequest>)delegate320;
					}
				}
			}
			if (this.OnServerGetUserPublisherDataResultEvent != null)
			{
				Delegate[] invocationList321 = this.OnServerGetUserPublisherDataResultEvent.GetInvocationList();
				foreach (Delegate delegate321 in invocationList321)
				{
					if (object.ReferenceEquals(delegate321.Target, instance))
					{
						OnServerGetUserPublisherDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetUserDataResult>)delegate321;
					}
				}
			}
			if (this.OnServerGetUserPublisherInternalDataRequestEvent != null)
			{
				Delegate[] invocationList322 = this.OnServerGetUserPublisherInternalDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate322 in invocationList322)
				{
					if (object.ReferenceEquals(delegate322.Target, instance))
					{
						OnServerGetUserPublisherInternalDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetUserDataRequest>)delegate322;
					}
				}
			}
			if (this.OnServerGetUserPublisherInternalDataResultEvent != null)
			{
				Delegate[] invocationList323 = this.OnServerGetUserPublisherInternalDataResultEvent.GetInvocationList();
				foreach (Delegate delegate323 in invocationList323)
				{
					if (object.ReferenceEquals(delegate323.Target, instance))
					{
						OnServerGetUserPublisherInternalDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetUserDataResult>)delegate323;
					}
				}
			}
			if (this.OnServerGetUserPublisherReadOnlyDataRequestEvent != null)
			{
				Delegate[] invocationList324 = this.OnServerGetUserPublisherReadOnlyDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate324 in invocationList324)
				{
					if (object.ReferenceEquals(delegate324.Target, instance))
					{
						OnServerGetUserPublisherReadOnlyDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetUserDataRequest>)delegate324;
					}
				}
			}
			if (this.OnServerGetUserPublisherReadOnlyDataResultEvent != null)
			{
				Delegate[] invocationList325 = this.OnServerGetUserPublisherReadOnlyDataResultEvent.GetInvocationList();
				foreach (Delegate delegate325 in invocationList325)
				{
					if (object.ReferenceEquals(delegate325.Target, instance))
					{
						OnServerGetUserPublisherReadOnlyDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetUserDataResult>)delegate325;
					}
				}
			}
			if (this.OnServerGetUserReadOnlyDataRequestEvent != null)
			{
				Delegate[] invocationList326 = this.OnServerGetUserReadOnlyDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate326 in invocationList326)
				{
					if (object.ReferenceEquals(delegate326.Target, instance))
					{
						OnServerGetUserReadOnlyDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GetUserDataRequest>)delegate326;
					}
				}
			}
			if (this.OnServerGetUserReadOnlyDataResultEvent != null)
			{
				Delegate[] invocationList327 = this.OnServerGetUserReadOnlyDataResultEvent.GetInvocationList();
				foreach (Delegate delegate327 in invocationList327)
				{
					if (object.ReferenceEquals(delegate327.Target, instance))
					{
						OnServerGetUserReadOnlyDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GetUserDataResult>)delegate327;
					}
				}
			}
			if (this.OnServerGrantCharacterToUserRequestEvent != null)
			{
				Delegate[] invocationList328 = this.OnServerGrantCharacterToUserRequestEvent.GetInvocationList();
				foreach (Delegate delegate328 in invocationList328)
				{
					if (object.ReferenceEquals(delegate328.Target, instance))
					{
						OnServerGrantCharacterToUserRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GrantCharacterToUserRequest>)delegate328;
					}
				}
			}
			if (this.OnServerGrantCharacterToUserResultEvent != null)
			{
				Delegate[] invocationList329 = this.OnServerGrantCharacterToUserResultEvent.GetInvocationList();
				foreach (Delegate delegate329 in invocationList329)
				{
					if (object.ReferenceEquals(delegate329.Target, instance))
					{
						OnServerGrantCharacterToUserResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GrantCharacterToUserResult>)delegate329;
					}
				}
			}
			if (this.OnServerGrantItemsToCharacterRequestEvent != null)
			{
				Delegate[] invocationList330 = this.OnServerGrantItemsToCharacterRequestEvent.GetInvocationList();
				foreach (Delegate delegate330 in invocationList330)
				{
					if (object.ReferenceEquals(delegate330.Target, instance))
					{
						OnServerGrantItemsToCharacterRequestEvent -= (PlayFabRequestEvent<GrantItemsToCharacterRequest>)delegate330;
					}
				}
			}
			if (this.OnServerGrantItemsToCharacterResultEvent != null)
			{
				Delegate[] invocationList331 = this.OnServerGrantItemsToCharacterResultEvent.GetInvocationList();
				foreach (Delegate delegate331 in invocationList331)
				{
					if (object.ReferenceEquals(delegate331.Target, instance))
					{
						OnServerGrantItemsToCharacterResultEvent -= (PlayFabResultEvent<GrantItemsToCharacterResult>)delegate331;
					}
				}
			}
			if (this.OnServerGrantItemsToUserRequestEvent != null)
			{
				Delegate[] invocationList332 = this.OnServerGrantItemsToUserRequestEvent.GetInvocationList();
				foreach (Delegate delegate332 in invocationList332)
				{
					if (object.ReferenceEquals(delegate332.Target, instance))
					{
						OnServerGrantItemsToUserRequestEvent -= (PlayFabRequestEvent<GrantItemsToUserRequest>)delegate332;
					}
				}
			}
			if (this.OnServerGrantItemsToUserResultEvent != null)
			{
				Delegate[] invocationList333 = this.OnServerGrantItemsToUserResultEvent.GetInvocationList();
				foreach (Delegate delegate333 in invocationList333)
				{
					if (object.ReferenceEquals(delegate333.Target, instance))
					{
						OnServerGrantItemsToUserResultEvent -= (PlayFabResultEvent<GrantItemsToUserResult>)delegate333;
					}
				}
			}
			if (this.OnServerGrantItemsToUsersRequestEvent != null)
			{
				Delegate[] invocationList334 = this.OnServerGrantItemsToUsersRequestEvent.GetInvocationList();
				foreach (Delegate delegate334 in invocationList334)
				{
					if (object.ReferenceEquals(delegate334.Target, instance))
					{
						OnServerGrantItemsToUsersRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.GrantItemsToUsersRequest>)delegate334;
					}
				}
			}
			if (this.OnServerGrantItemsToUsersResultEvent != null)
			{
				Delegate[] invocationList335 = this.OnServerGrantItemsToUsersResultEvent.GetInvocationList();
				foreach (Delegate delegate335 in invocationList335)
				{
					if (object.ReferenceEquals(delegate335.Target, instance))
					{
						OnServerGrantItemsToUsersResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.GrantItemsToUsersResult>)delegate335;
					}
				}
			}
			if (this.OnServerModifyItemUsesRequestEvent != null)
			{
				Delegate[] invocationList336 = this.OnServerModifyItemUsesRequestEvent.GetInvocationList();
				foreach (Delegate delegate336 in invocationList336)
				{
					if (object.ReferenceEquals(delegate336.Target, instance))
					{
						OnServerModifyItemUsesRequestEvent -= (PlayFabRequestEvent<ModifyItemUsesRequest>)delegate336;
					}
				}
			}
			if (this.OnServerModifyItemUsesResultEvent != null)
			{
				Delegate[] invocationList337 = this.OnServerModifyItemUsesResultEvent.GetInvocationList();
				foreach (Delegate delegate337 in invocationList337)
				{
					if (object.ReferenceEquals(delegate337.Target, instance))
					{
						OnServerModifyItemUsesResultEvent -= (PlayFabResultEvent<ModifyItemUsesResult>)delegate337;
					}
				}
			}
			if (this.OnServerMoveItemToCharacterFromCharacterRequestEvent != null)
			{
				Delegate[] invocationList338 = this.OnServerMoveItemToCharacterFromCharacterRequestEvent.GetInvocationList();
				foreach (Delegate delegate338 in invocationList338)
				{
					if (object.ReferenceEquals(delegate338.Target, instance))
					{
						OnServerMoveItemToCharacterFromCharacterRequestEvent -= (PlayFabRequestEvent<MoveItemToCharacterFromCharacterRequest>)delegate338;
					}
				}
			}
			if (this.OnServerMoveItemToCharacterFromCharacterResultEvent != null)
			{
				Delegate[] invocationList339 = this.OnServerMoveItemToCharacterFromCharacterResultEvent.GetInvocationList();
				foreach (Delegate delegate339 in invocationList339)
				{
					if (object.ReferenceEquals(delegate339.Target, instance))
					{
						OnServerMoveItemToCharacterFromCharacterResultEvent -= (PlayFabResultEvent<MoveItemToCharacterFromCharacterResult>)delegate339;
					}
				}
			}
			if (this.OnServerMoveItemToCharacterFromUserRequestEvent != null)
			{
				Delegate[] invocationList340 = this.OnServerMoveItemToCharacterFromUserRequestEvent.GetInvocationList();
				foreach (Delegate delegate340 in invocationList340)
				{
					if (object.ReferenceEquals(delegate340.Target, instance))
					{
						OnServerMoveItemToCharacterFromUserRequestEvent -= (PlayFabRequestEvent<MoveItemToCharacterFromUserRequest>)delegate340;
					}
				}
			}
			if (this.OnServerMoveItemToCharacterFromUserResultEvent != null)
			{
				Delegate[] invocationList341 = this.OnServerMoveItemToCharacterFromUserResultEvent.GetInvocationList();
				foreach (Delegate delegate341 in invocationList341)
				{
					if (object.ReferenceEquals(delegate341.Target, instance))
					{
						OnServerMoveItemToCharacterFromUserResultEvent -= (PlayFabResultEvent<MoveItemToCharacterFromUserResult>)delegate341;
					}
				}
			}
			if (this.OnServerMoveItemToUserFromCharacterRequestEvent != null)
			{
				Delegate[] invocationList342 = this.OnServerMoveItemToUserFromCharacterRequestEvent.GetInvocationList();
				foreach (Delegate delegate342 in invocationList342)
				{
					if (object.ReferenceEquals(delegate342.Target, instance))
					{
						OnServerMoveItemToUserFromCharacterRequestEvent -= (PlayFabRequestEvent<MoveItemToUserFromCharacterRequest>)delegate342;
					}
				}
			}
			if (this.OnServerMoveItemToUserFromCharacterResultEvent != null)
			{
				Delegate[] invocationList343 = this.OnServerMoveItemToUserFromCharacterResultEvent.GetInvocationList();
				foreach (Delegate delegate343 in invocationList343)
				{
					if (object.ReferenceEquals(delegate343.Target, instance))
					{
						OnServerMoveItemToUserFromCharacterResultEvent -= (PlayFabResultEvent<MoveItemToUserFromCharacterResult>)delegate343;
					}
				}
			}
			if (this.OnServerNotifyMatchmakerPlayerLeftRequestEvent != null)
			{
				Delegate[] invocationList344 = this.OnServerNotifyMatchmakerPlayerLeftRequestEvent.GetInvocationList();
				foreach (Delegate delegate344 in invocationList344)
				{
					if (object.ReferenceEquals(delegate344.Target, instance))
					{
						OnServerNotifyMatchmakerPlayerLeftRequestEvent -= (PlayFabRequestEvent<NotifyMatchmakerPlayerLeftRequest>)delegate344;
					}
				}
			}
			if (this.OnServerNotifyMatchmakerPlayerLeftResultEvent != null)
			{
				Delegate[] invocationList345 = this.OnServerNotifyMatchmakerPlayerLeftResultEvent.GetInvocationList();
				foreach (Delegate delegate345 in invocationList345)
				{
					if (object.ReferenceEquals(delegate345.Target, instance))
					{
						OnServerNotifyMatchmakerPlayerLeftResultEvent -= (PlayFabResultEvent<NotifyMatchmakerPlayerLeftResult>)delegate345;
					}
				}
			}
			if (this.OnServerRedeemCouponRequestEvent != null)
			{
				Delegate[] invocationList346 = this.OnServerRedeemCouponRequestEvent.GetInvocationList();
				foreach (Delegate delegate346 in invocationList346)
				{
					if (object.ReferenceEquals(delegate346.Target, instance))
					{
						OnServerRedeemCouponRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.RedeemCouponRequest>)delegate346;
					}
				}
			}
			if (this.OnServerRedeemCouponResultEvent != null)
			{
				Delegate[] invocationList347 = this.OnServerRedeemCouponResultEvent.GetInvocationList();
				foreach (Delegate delegate347 in invocationList347)
				{
					if (object.ReferenceEquals(delegate347.Target, instance))
					{
						OnServerRedeemCouponResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.RedeemCouponResult>)delegate347;
					}
				}
			}
			if (this.OnServerRedeemMatchmakerTicketRequestEvent != null)
			{
				Delegate[] invocationList348 = this.OnServerRedeemMatchmakerTicketRequestEvent.GetInvocationList();
				foreach (Delegate delegate348 in invocationList348)
				{
					if (object.ReferenceEquals(delegate348.Target, instance))
					{
						OnServerRedeemMatchmakerTicketRequestEvent -= (PlayFabRequestEvent<RedeemMatchmakerTicketRequest>)delegate348;
					}
				}
			}
			if (this.OnServerRedeemMatchmakerTicketResultEvent != null)
			{
				Delegate[] invocationList349 = this.OnServerRedeemMatchmakerTicketResultEvent.GetInvocationList();
				foreach (Delegate delegate349 in invocationList349)
				{
					if (object.ReferenceEquals(delegate349.Target, instance))
					{
						OnServerRedeemMatchmakerTicketResultEvent -= (PlayFabResultEvent<RedeemMatchmakerTicketResult>)delegate349;
					}
				}
			}
			if (this.OnServerRefreshGameServerInstanceHeartbeatRequestEvent != null)
			{
				Delegate[] invocationList350 = this.OnServerRefreshGameServerInstanceHeartbeatRequestEvent.GetInvocationList();
				foreach (Delegate delegate350 in invocationList350)
				{
					if (object.ReferenceEquals(delegate350.Target, instance))
					{
						OnServerRefreshGameServerInstanceHeartbeatRequestEvent -= (PlayFabRequestEvent<RefreshGameServerInstanceHeartbeatRequest>)delegate350;
					}
				}
			}
			if (this.OnServerRefreshGameServerInstanceHeartbeatResultEvent != null)
			{
				Delegate[] invocationList351 = this.OnServerRefreshGameServerInstanceHeartbeatResultEvent.GetInvocationList();
				foreach (Delegate delegate351 in invocationList351)
				{
					if (object.ReferenceEquals(delegate351.Target, instance))
					{
						OnServerRefreshGameServerInstanceHeartbeatResultEvent -= (PlayFabResultEvent<RefreshGameServerInstanceHeartbeatResult>)delegate351;
					}
				}
			}
			if (this.OnServerRegisterGameRequestEvent != null)
			{
				Delegate[] invocationList352 = this.OnServerRegisterGameRequestEvent.GetInvocationList();
				foreach (Delegate delegate352 in invocationList352)
				{
					if (object.ReferenceEquals(delegate352.Target, instance))
					{
						OnServerRegisterGameRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.RegisterGameRequest>)delegate352;
					}
				}
			}
			if (this.OnServerRegisterGameResultEvent != null)
			{
				Delegate[] invocationList353 = this.OnServerRegisterGameResultEvent.GetInvocationList();
				foreach (Delegate delegate353 in invocationList353)
				{
					if (object.ReferenceEquals(delegate353.Target, instance))
					{
						OnServerRegisterGameResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.RegisterGameResponse>)delegate353;
					}
				}
			}
			if (this.OnServerRemoveFriendRequestEvent != null)
			{
				Delegate[] invocationList354 = this.OnServerRemoveFriendRequestEvent.GetInvocationList();
				foreach (Delegate delegate354 in invocationList354)
				{
					if (object.ReferenceEquals(delegate354.Target, instance))
					{
						OnServerRemoveFriendRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.RemoveFriendRequest>)delegate354;
					}
				}
			}
			if (this.OnServerRemoveFriendResultEvent != null)
			{
				Delegate[] invocationList355 = this.OnServerRemoveFriendResultEvent.GetInvocationList();
				foreach (Delegate delegate355 in invocationList355)
				{
					if (object.ReferenceEquals(delegate355.Target, instance))
					{
						OnServerRemoveFriendResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.EmptyResult>)delegate355;
					}
				}
			}
			if (this.OnServerRemovePlayerTagRequestEvent != null)
			{
				Delegate[] invocationList356 = this.OnServerRemovePlayerTagRequestEvent.GetInvocationList();
				foreach (Delegate delegate356 in invocationList356)
				{
					if (object.ReferenceEquals(delegate356.Target, instance))
					{
						OnServerRemovePlayerTagRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.RemovePlayerTagRequest>)delegate356;
					}
				}
			}
			if (this.OnServerRemovePlayerTagResultEvent != null)
			{
				Delegate[] invocationList357 = this.OnServerRemovePlayerTagResultEvent.GetInvocationList();
				foreach (Delegate delegate357 in invocationList357)
				{
					if (object.ReferenceEquals(delegate357.Target, instance))
					{
						OnServerRemovePlayerTagResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.RemovePlayerTagResult>)delegate357;
					}
				}
			}
			if (this.OnServerRemoveSharedGroupMembersRequestEvent != null)
			{
				Delegate[] invocationList358 = this.OnServerRemoveSharedGroupMembersRequestEvent.GetInvocationList();
				foreach (Delegate delegate358 in invocationList358)
				{
					if (object.ReferenceEquals(delegate358.Target, instance))
					{
						OnServerRemoveSharedGroupMembersRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.RemoveSharedGroupMembersRequest>)delegate358;
					}
				}
			}
			if (this.OnServerRemoveSharedGroupMembersResultEvent != null)
			{
				Delegate[] invocationList359 = this.OnServerRemoveSharedGroupMembersResultEvent.GetInvocationList();
				foreach (Delegate delegate359 in invocationList359)
				{
					if (object.ReferenceEquals(delegate359.Target, instance))
					{
						OnServerRemoveSharedGroupMembersResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.RemoveSharedGroupMembersResult>)delegate359;
					}
				}
			}
			if (this.OnServerReportPlayerRequestEvent != null)
			{
				Delegate[] invocationList360 = this.OnServerReportPlayerRequestEvent.GetInvocationList();
				foreach (Delegate delegate360 in invocationList360)
				{
					if (object.ReferenceEquals(delegate360.Target, instance))
					{
						OnServerReportPlayerRequestEvent -= (PlayFabRequestEvent<ReportPlayerServerRequest>)delegate360;
					}
				}
			}
			if (this.OnServerReportPlayerResultEvent != null)
			{
				Delegate[] invocationList361 = this.OnServerReportPlayerResultEvent.GetInvocationList();
				foreach (Delegate delegate361 in invocationList361)
				{
					if (object.ReferenceEquals(delegate361.Target, instance))
					{
						OnServerReportPlayerResultEvent -= (PlayFabResultEvent<ReportPlayerServerResult>)delegate361;
					}
				}
			}
			if (this.OnServerRevokeAllBansForUserRequestEvent != null)
			{
				Delegate[] invocationList362 = this.OnServerRevokeAllBansForUserRequestEvent.GetInvocationList();
				foreach (Delegate delegate362 in invocationList362)
				{
					if (object.ReferenceEquals(delegate362.Target, instance))
					{
						OnServerRevokeAllBansForUserRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.RevokeAllBansForUserRequest>)delegate362;
					}
				}
			}
			if (this.OnServerRevokeAllBansForUserResultEvent != null)
			{
				Delegate[] invocationList363 = this.OnServerRevokeAllBansForUserResultEvent.GetInvocationList();
				foreach (Delegate delegate363 in invocationList363)
				{
					if (object.ReferenceEquals(delegate363.Target, instance))
					{
						OnServerRevokeAllBansForUserResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.RevokeAllBansForUserResult>)delegate363;
					}
				}
			}
			if (this.OnServerRevokeBansRequestEvent != null)
			{
				Delegate[] invocationList364 = this.OnServerRevokeBansRequestEvent.GetInvocationList();
				foreach (Delegate delegate364 in invocationList364)
				{
					if (object.ReferenceEquals(delegate364.Target, instance))
					{
						OnServerRevokeBansRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.RevokeBansRequest>)delegate364;
					}
				}
			}
			if (this.OnServerRevokeBansResultEvent != null)
			{
				Delegate[] invocationList365 = this.OnServerRevokeBansResultEvent.GetInvocationList();
				foreach (Delegate delegate365 in invocationList365)
				{
					if (object.ReferenceEquals(delegate365.Target, instance))
					{
						OnServerRevokeBansResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.RevokeBansResult>)delegate365;
					}
				}
			}
			if (this.OnServerRevokeInventoryItemRequestEvent != null)
			{
				Delegate[] invocationList366 = this.OnServerRevokeInventoryItemRequestEvent.GetInvocationList();
				foreach (Delegate delegate366 in invocationList366)
				{
					if (object.ReferenceEquals(delegate366.Target, instance))
					{
						OnServerRevokeInventoryItemRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.RevokeInventoryItemRequest>)delegate366;
					}
				}
			}
			if (this.OnServerRevokeInventoryItemResultEvent != null)
			{
				Delegate[] invocationList367 = this.OnServerRevokeInventoryItemResultEvent.GetInvocationList();
				foreach (Delegate delegate367 in invocationList367)
				{
					if (object.ReferenceEquals(delegate367.Target, instance))
					{
						OnServerRevokeInventoryItemResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.RevokeInventoryResult>)delegate367;
					}
				}
			}
			if (this.OnServerSendCustomAccountRecoveryEmailRequestEvent != null)
			{
				Delegate[] invocationList368 = this.OnServerSendCustomAccountRecoveryEmailRequestEvent.GetInvocationList();
				foreach (Delegate delegate368 in invocationList368)
				{
					if (object.ReferenceEquals(delegate368.Target, instance))
					{
						OnServerSendCustomAccountRecoveryEmailRequestEvent -= (PlayFabRequestEvent<SendCustomAccountRecoveryEmailRequest>)delegate368;
					}
				}
			}
			if (this.OnServerSendCustomAccountRecoveryEmailResultEvent != null)
			{
				Delegate[] invocationList369 = this.OnServerSendCustomAccountRecoveryEmailResultEvent.GetInvocationList();
				foreach (Delegate delegate369 in invocationList369)
				{
					if (object.ReferenceEquals(delegate369.Target, instance))
					{
						OnServerSendCustomAccountRecoveryEmailResultEvent -= (PlayFabResultEvent<SendCustomAccountRecoveryEmailResult>)delegate369;
					}
				}
			}
			if (this.OnServerSendEmailFromTemplateRequestEvent != null)
			{
				Delegate[] invocationList370 = this.OnServerSendEmailFromTemplateRequestEvent.GetInvocationList();
				foreach (Delegate delegate370 in invocationList370)
				{
					if (object.ReferenceEquals(delegate370.Target, instance))
					{
						OnServerSendEmailFromTemplateRequestEvent -= (PlayFabRequestEvent<SendEmailFromTemplateRequest>)delegate370;
					}
				}
			}
			if (this.OnServerSendEmailFromTemplateResultEvent != null)
			{
				Delegate[] invocationList371 = this.OnServerSendEmailFromTemplateResultEvent.GetInvocationList();
				foreach (Delegate delegate371 in invocationList371)
				{
					if (object.ReferenceEquals(delegate371.Target, instance))
					{
						OnServerSendEmailFromTemplateResultEvent -= (PlayFabResultEvent<SendEmailFromTemplateResult>)delegate371;
					}
				}
			}
			if (this.OnServerSendPushNotificationRequestEvent != null)
			{
				Delegate[] invocationList372 = this.OnServerSendPushNotificationRequestEvent.GetInvocationList();
				foreach (Delegate delegate372 in invocationList372)
				{
					if (object.ReferenceEquals(delegate372.Target, instance))
					{
						OnServerSendPushNotificationRequestEvent -= (PlayFabRequestEvent<SendPushNotificationRequest>)delegate372;
					}
				}
			}
			if (this.OnServerSendPushNotificationResultEvent != null)
			{
				Delegate[] invocationList373 = this.OnServerSendPushNotificationResultEvent.GetInvocationList();
				foreach (Delegate delegate373 in invocationList373)
				{
					if (object.ReferenceEquals(delegate373.Target, instance))
					{
						OnServerSendPushNotificationResultEvent -= (PlayFabResultEvent<SendPushNotificationResult>)delegate373;
					}
				}
			}
			if (this.OnServerSetFriendTagsRequestEvent != null)
			{
				Delegate[] invocationList374 = this.OnServerSetFriendTagsRequestEvent.GetInvocationList();
				foreach (Delegate delegate374 in invocationList374)
				{
					if (object.ReferenceEquals(delegate374.Target, instance))
					{
						OnServerSetFriendTagsRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.SetFriendTagsRequest>)delegate374;
					}
				}
			}
			if (this.OnServerSetFriendTagsResultEvent != null)
			{
				Delegate[] invocationList375 = this.OnServerSetFriendTagsResultEvent.GetInvocationList();
				foreach (Delegate delegate375 in invocationList375)
				{
					if (object.ReferenceEquals(delegate375.Target, instance))
					{
						OnServerSetFriendTagsResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.EmptyResult>)delegate375;
					}
				}
			}
			if (this.OnServerSetGameServerInstanceDataRequestEvent != null)
			{
				Delegate[] invocationList376 = this.OnServerSetGameServerInstanceDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate376 in invocationList376)
				{
					if (object.ReferenceEquals(delegate376.Target, instance))
					{
						OnServerSetGameServerInstanceDataRequestEvent -= (PlayFabRequestEvent<SetGameServerInstanceDataRequest>)delegate376;
					}
				}
			}
			if (this.OnServerSetGameServerInstanceDataResultEvent != null)
			{
				Delegate[] invocationList377 = this.OnServerSetGameServerInstanceDataResultEvent.GetInvocationList();
				foreach (Delegate delegate377 in invocationList377)
				{
					if (object.ReferenceEquals(delegate377.Target, instance))
					{
						OnServerSetGameServerInstanceDataResultEvent -= (PlayFabResultEvent<SetGameServerInstanceDataResult>)delegate377;
					}
				}
			}
			if (this.OnServerSetGameServerInstanceStateRequestEvent != null)
			{
				Delegate[] invocationList378 = this.OnServerSetGameServerInstanceStateRequestEvent.GetInvocationList();
				foreach (Delegate delegate378 in invocationList378)
				{
					if (object.ReferenceEquals(delegate378.Target, instance))
					{
						OnServerSetGameServerInstanceStateRequestEvent -= (PlayFabRequestEvent<SetGameServerInstanceStateRequest>)delegate378;
					}
				}
			}
			if (this.OnServerSetGameServerInstanceStateResultEvent != null)
			{
				Delegate[] invocationList379 = this.OnServerSetGameServerInstanceStateResultEvent.GetInvocationList();
				foreach (Delegate delegate379 in invocationList379)
				{
					if (object.ReferenceEquals(delegate379.Target, instance))
					{
						OnServerSetGameServerInstanceStateResultEvent -= (PlayFabResultEvent<SetGameServerInstanceStateResult>)delegate379;
					}
				}
			}
			if (this.OnServerSetGameServerInstanceTagsRequestEvent != null)
			{
				Delegate[] invocationList380 = this.OnServerSetGameServerInstanceTagsRequestEvent.GetInvocationList();
				foreach (Delegate delegate380 in invocationList380)
				{
					if (object.ReferenceEquals(delegate380.Target, instance))
					{
						OnServerSetGameServerInstanceTagsRequestEvent -= (PlayFabRequestEvent<SetGameServerInstanceTagsRequest>)delegate380;
					}
				}
			}
			if (this.OnServerSetGameServerInstanceTagsResultEvent != null)
			{
				Delegate[] invocationList381 = this.OnServerSetGameServerInstanceTagsResultEvent.GetInvocationList();
				foreach (Delegate delegate381 in invocationList381)
				{
					if (object.ReferenceEquals(delegate381.Target, instance))
					{
						OnServerSetGameServerInstanceTagsResultEvent -= (PlayFabResultEvent<SetGameServerInstanceTagsResult>)delegate381;
					}
				}
			}
			if (this.OnServerSetPlayerSecretRequestEvent != null)
			{
				Delegate[] invocationList382 = this.OnServerSetPlayerSecretRequestEvent.GetInvocationList();
				foreach (Delegate delegate382 in invocationList382)
				{
					if (object.ReferenceEquals(delegate382.Target, instance))
					{
						OnServerSetPlayerSecretRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.SetPlayerSecretRequest>)delegate382;
					}
				}
			}
			if (this.OnServerSetPlayerSecretResultEvent != null)
			{
				Delegate[] invocationList383 = this.OnServerSetPlayerSecretResultEvent.GetInvocationList();
				foreach (Delegate delegate383 in invocationList383)
				{
					if (object.ReferenceEquals(delegate383.Target, instance))
					{
						OnServerSetPlayerSecretResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.SetPlayerSecretResult>)delegate383;
					}
				}
			}
			if (this.OnServerSetPublisherDataRequestEvent != null)
			{
				Delegate[] invocationList384 = this.OnServerSetPublisherDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate384 in invocationList384)
				{
					if (object.ReferenceEquals(delegate384.Target, instance))
					{
						OnServerSetPublisherDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.SetPublisherDataRequest>)delegate384;
					}
				}
			}
			if (this.OnServerSetPublisherDataResultEvent != null)
			{
				Delegate[] invocationList385 = this.OnServerSetPublisherDataResultEvent.GetInvocationList();
				foreach (Delegate delegate385 in invocationList385)
				{
					if (object.ReferenceEquals(delegate385.Target, instance))
					{
						OnServerSetPublisherDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.SetPublisherDataResult>)delegate385;
					}
				}
			}
			if (this.OnServerSetTitleDataRequestEvent != null)
			{
				Delegate[] invocationList386 = this.OnServerSetTitleDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate386 in invocationList386)
				{
					if (object.ReferenceEquals(delegate386.Target, instance))
					{
						OnServerSetTitleDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.SetTitleDataRequest>)delegate386;
					}
				}
			}
			if (this.OnServerSetTitleDataResultEvent != null)
			{
				Delegate[] invocationList387 = this.OnServerSetTitleDataResultEvent.GetInvocationList();
				foreach (Delegate delegate387 in invocationList387)
				{
					if (object.ReferenceEquals(delegate387.Target, instance))
					{
						OnServerSetTitleDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.SetTitleDataResult>)delegate387;
					}
				}
			}
			if (this.OnServerSetTitleInternalDataRequestEvent != null)
			{
				Delegate[] invocationList388 = this.OnServerSetTitleInternalDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate388 in invocationList388)
				{
					if (object.ReferenceEquals(delegate388.Target, instance))
					{
						OnServerSetTitleInternalDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.SetTitleDataRequest>)delegate388;
					}
				}
			}
			if (this.OnServerSetTitleInternalDataResultEvent != null)
			{
				Delegate[] invocationList389 = this.OnServerSetTitleInternalDataResultEvent.GetInvocationList();
				foreach (Delegate delegate389 in invocationList389)
				{
					if (object.ReferenceEquals(delegate389.Target, instance))
					{
						OnServerSetTitleInternalDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.SetTitleDataResult>)delegate389;
					}
				}
			}
			if (this.OnServerSubtractCharacterVirtualCurrencyRequestEvent != null)
			{
				Delegate[] invocationList390 = this.OnServerSubtractCharacterVirtualCurrencyRequestEvent.GetInvocationList();
				foreach (Delegate delegate390 in invocationList390)
				{
					if (object.ReferenceEquals(delegate390.Target, instance))
					{
						OnServerSubtractCharacterVirtualCurrencyRequestEvent -= (PlayFabRequestEvent<SubtractCharacterVirtualCurrencyRequest>)delegate390;
					}
				}
			}
			if (this.OnServerSubtractCharacterVirtualCurrencyResultEvent != null)
			{
				Delegate[] invocationList391 = this.OnServerSubtractCharacterVirtualCurrencyResultEvent.GetInvocationList();
				foreach (Delegate delegate391 in invocationList391)
				{
					if (object.ReferenceEquals(delegate391.Target, instance))
					{
						OnServerSubtractCharacterVirtualCurrencyResultEvent -= (PlayFabResultEvent<ModifyCharacterVirtualCurrencyResult>)delegate391;
					}
				}
			}
			if (this.OnServerSubtractUserVirtualCurrencyRequestEvent != null)
			{
				Delegate[] invocationList392 = this.OnServerSubtractUserVirtualCurrencyRequestEvent.GetInvocationList();
				foreach (Delegate delegate392 in invocationList392)
				{
					if (object.ReferenceEquals(delegate392.Target, instance))
					{
						OnServerSubtractUserVirtualCurrencyRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.SubtractUserVirtualCurrencyRequest>)delegate392;
					}
				}
			}
			if (this.OnServerSubtractUserVirtualCurrencyResultEvent != null)
			{
				Delegate[] invocationList393 = this.OnServerSubtractUserVirtualCurrencyResultEvent.GetInvocationList();
				foreach (Delegate delegate393 in invocationList393)
				{
					if (object.ReferenceEquals(delegate393.Target, instance))
					{
						OnServerSubtractUserVirtualCurrencyResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.ModifyUserVirtualCurrencyResult>)delegate393;
					}
				}
			}
			if (this.OnServerUnlockContainerInstanceRequestEvent != null)
			{
				Delegate[] invocationList394 = this.OnServerUnlockContainerInstanceRequestEvent.GetInvocationList();
				foreach (Delegate delegate394 in invocationList394)
				{
					if (object.ReferenceEquals(delegate394.Target, instance))
					{
						OnServerUnlockContainerInstanceRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.UnlockContainerInstanceRequest>)delegate394;
					}
				}
			}
			if (this.OnServerUnlockContainerInstanceResultEvent != null)
			{
				Delegate[] invocationList395 = this.OnServerUnlockContainerInstanceResultEvent.GetInvocationList();
				foreach (Delegate delegate395 in invocationList395)
				{
					if (object.ReferenceEquals(delegate395.Target, instance))
					{
						OnServerUnlockContainerInstanceResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.UnlockContainerItemResult>)delegate395;
					}
				}
			}
			if (this.OnServerUnlockContainerItemRequestEvent != null)
			{
				Delegate[] invocationList396 = this.OnServerUnlockContainerItemRequestEvent.GetInvocationList();
				foreach (Delegate delegate396 in invocationList396)
				{
					if (object.ReferenceEquals(delegate396.Target, instance))
					{
						OnServerUnlockContainerItemRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.UnlockContainerItemRequest>)delegate396;
					}
				}
			}
			if (this.OnServerUnlockContainerItemResultEvent != null)
			{
				Delegate[] invocationList397 = this.OnServerUnlockContainerItemResultEvent.GetInvocationList();
				foreach (Delegate delegate397 in invocationList397)
				{
					if (object.ReferenceEquals(delegate397.Target, instance))
					{
						OnServerUnlockContainerItemResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.UnlockContainerItemResult>)delegate397;
					}
				}
			}
			if (this.OnServerUpdateAvatarUrlRequestEvent != null)
			{
				Delegate[] invocationList398 = this.OnServerUpdateAvatarUrlRequestEvent.GetInvocationList();
				foreach (Delegate delegate398 in invocationList398)
				{
					if (object.ReferenceEquals(delegate398.Target, instance))
					{
						OnServerUpdateAvatarUrlRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.UpdateAvatarUrlRequest>)delegate398;
					}
				}
			}
			if (this.OnServerUpdateAvatarUrlResultEvent != null)
			{
				Delegate[] invocationList399 = this.OnServerUpdateAvatarUrlResultEvent.GetInvocationList();
				foreach (Delegate delegate399 in invocationList399)
				{
					if (object.ReferenceEquals(delegate399.Target, instance))
					{
						OnServerUpdateAvatarUrlResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.EmptyResult>)delegate399;
					}
				}
			}
			if (this.OnServerUpdateBansRequestEvent != null)
			{
				Delegate[] invocationList400 = this.OnServerUpdateBansRequestEvent.GetInvocationList();
				foreach (Delegate delegate400 in invocationList400)
				{
					if (object.ReferenceEquals(delegate400.Target, instance))
					{
						OnServerUpdateBansRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.UpdateBansRequest>)delegate400;
					}
				}
			}
			if (this.OnServerUpdateBansResultEvent != null)
			{
				Delegate[] invocationList401 = this.OnServerUpdateBansResultEvent.GetInvocationList();
				foreach (Delegate delegate401 in invocationList401)
				{
					if (object.ReferenceEquals(delegate401.Target, instance))
					{
						OnServerUpdateBansResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.UpdateBansResult>)delegate401;
					}
				}
			}
			if (this.OnServerUpdateCharacterDataRequestEvent != null)
			{
				Delegate[] invocationList402 = this.OnServerUpdateCharacterDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate402 in invocationList402)
				{
					if (object.ReferenceEquals(delegate402.Target, instance))
					{
						OnServerUpdateCharacterDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.UpdateCharacterDataRequest>)delegate402;
					}
				}
			}
			if (this.OnServerUpdateCharacterDataResultEvent != null)
			{
				Delegate[] invocationList403 = this.OnServerUpdateCharacterDataResultEvent.GetInvocationList();
				foreach (Delegate delegate403 in invocationList403)
				{
					if (object.ReferenceEquals(delegate403.Target, instance))
					{
						OnServerUpdateCharacterDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.UpdateCharacterDataResult>)delegate403;
					}
				}
			}
			if (this.OnServerUpdateCharacterInternalDataRequestEvent != null)
			{
				Delegate[] invocationList404 = this.OnServerUpdateCharacterInternalDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate404 in invocationList404)
				{
					if (object.ReferenceEquals(delegate404.Target, instance))
					{
						OnServerUpdateCharacterInternalDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.UpdateCharacterDataRequest>)delegate404;
					}
				}
			}
			if (this.OnServerUpdateCharacterInternalDataResultEvent != null)
			{
				Delegate[] invocationList405 = this.OnServerUpdateCharacterInternalDataResultEvent.GetInvocationList();
				foreach (Delegate delegate405 in invocationList405)
				{
					if (object.ReferenceEquals(delegate405.Target, instance))
					{
						OnServerUpdateCharacterInternalDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.UpdateCharacterDataResult>)delegate405;
					}
				}
			}
			if (this.OnServerUpdateCharacterReadOnlyDataRequestEvent != null)
			{
				Delegate[] invocationList406 = this.OnServerUpdateCharacterReadOnlyDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate406 in invocationList406)
				{
					if (object.ReferenceEquals(delegate406.Target, instance))
					{
						OnServerUpdateCharacterReadOnlyDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.UpdateCharacterDataRequest>)delegate406;
					}
				}
			}
			if (this.OnServerUpdateCharacterReadOnlyDataResultEvent != null)
			{
				Delegate[] invocationList407 = this.OnServerUpdateCharacterReadOnlyDataResultEvent.GetInvocationList();
				foreach (Delegate delegate407 in invocationList407)
				{
					if (object.ReferenceEquals(delegate407.Target, instance))
					{
						OnServerUpdateCharacterReadOnlyDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.UpdateCharacterDataResult>)delegate407;
					}
				}
			}
			if (this.OnServerUpdateCharacterStatisticsRequestEvent != null)
			{
				Delegate[] invocationList408 = this.OnServerUpdateCharacterStatisticsRequestEvent.GetInvocationList();
				foreach (Delegate delegate408 in invocationList408)
				{
					if (object.ReferenceEquals(delegate408.Target, instance))
					{
						OnServerUpdateCharacterStatisticsRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.UpdateCharacterStatisticsRequest>)delegate408;
					}
				}
			}
			if (this.OnServerUpdateCharacterStatisticsResultEvent != null)
			{
				Delegate[] invocationList409 = this.OnServerUpdateCharacterStatisticsResultEvent.GetInvocationList();
				foreach (Delegate delegate409 in invocationList409)
				{
					if (object.ReferenceEquals(delegate409.Target, instance))
					{
						OnServerUpdateCharacterStatisticsResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.UpdateCharacterStatisticsResult>)delegate409;
					}
				}
			}
			if (this.OnServerUpdatePlayerStatisticsRequestEvent != null)
			{
				Delegate[] invocationList410 = this.OnServerUpdatePlayerStatisticsRequestEvent.GetInvocationList();
				foreach (Delegate delegate410 in invocationList410)
				{
					if (object.ReferenceEquals(delegate410.Target, instance))
					{
						OnServerUpdatePlayerStatisticsRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.UpdatePlayerStatisticsRequest>)delegate410;
					}
				}
			}
			if (this.OnServerUpdatePlayerStatisticsResultEvent != null)
			{
				Delegate[] invocationList411 = this.OnServerUpdatePlayerStatisticsResultEvent.GetInvocationList();
				foreach (Delegate delegate411 in invocationList411)
				{
					if (object.ReferenceEquals(delegate411.Target, instance))
					{
						OnServerUpdatePlayerStatisticsResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.UpdatePlayerStatisticsResult>)delegate411;
					}
				}
			}
			if (this.OnServerUpdateSharedGroupDataRequestEvent != null)
			{
				Delegate[] invocationList412 = this.OnServerUpdateSharedGroupDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate412 in invocationList412)
				{
					if (object.ReferenceEquals(delegate412.Target, instance))
					{
						OnServerUpdateSharedGroupDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.UpdateSharedGroupDataRequest>)delegate412;
					}
				}
			}
			if (this.OnServerUpdateSharedGroupDataResultEvent != null)
			{
				Delegate[] invocationList413 = this.OnServerUpdateSharedGroupDataResultEvent.GetInvocationList();
				foreach (Delegate delegate413 in invocationList413)
				{
					if (object.ReferenceEquals(delegate413.Target, instance))
					{
						OnServerUpdateSharedGroupDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.UpdateSharedGroupDataResult>)delegate413;
					}
				}
			}
			if (this.OnServerUpdateUserDataRequestEvent != null)
			{
				Delegate[] invocationList414 = this.OnServerUpdateUserDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate414 in invocationList414)
				{
					if (object.ReferenceEquals(delegate414.Target, instance))
					{
						OnServerUpdateUserDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.UpdateUserDataRequest>)delegate414;
					}
				}
			}
			if (this.OnServerUpdateUserDataResultEvent != null)
			{
				Delegate[] invocationList415 = this.OnServerUpdateUserDataResultEvent.GetInvocationList();
				foreach (Delegate delegate415 in invocationList415)
				{
					if (object.ReferenceEquals(delegate415.Target, instance))
					{
						OnServerUpdateUserDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.UpdateUserDataResult>)delegate415;
					}
				}
			}
			if (this.OnServerUpdateUserInternalDataRequestEvent != null)
			{
				Delegate[] invocationList416 = this.OnServerUpdateUserInternalDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate416 in invocationList416)
				{
					if (object.ReferenceEquals(delegate416.Target, instance))
					{
						OnServerUpdateUserInternalDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.UpdateUserInternalDataRequest>)delegate416;
					}
				}
			}
			if (this.OnServerUpdateUserInternalDataResultEvent != null)
			{
				Delegate[] invocationList417 = this.OnServerUpdateUserInternalDataResultEvent.GetInvocationList();
				foreach (Delegate delegate417 in invocationList417)
				{
					if (object.ReferenceEquals(delegate417.Target, instance))
					{
						OnServerUpdateUserInternalDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.UpdateUserDataResult>)delegate417;
					}
				}
			}
			if (this.OnServerUpdateUserInventoryItemCustomDataRequestEvent != null)
			{
				Delegate[] invocationList418 = this.OnServerUpdateUserInventoryItemCustomDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate418 in invocationList418)
				{
					if (object.ReferenceEquals(delegate418.Target, instance))
					{
						OnServerUpdateUserInventoryItemCustomDataRequestEvent -= (PlayFabRequestEvent<UpdateUserInventoryItemDataRequest>)delegate418;
					}
				}
			}
			if (this.OnServerUpdateUserInventoryItemCustomDataResultEvent != null)
			{
				Delegate[] invocationList419 = this.OnServerUpdateUserInventoryItemCustomDataResultEvent.GetInvocationList();
				foreach (Delegate delegate419 in invocationList419)
				{
					if (object.ReferenceEquals(delegate419.Target, instance))
					{
						OnServerUpdateUserInventoryItemCustomDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.EmptyResult>)delegate419;
					}
				}
			}
			if (this.OnServerUpdateUserPublisherDataRequestEvent != null)
			{
				Delegate[] invocationList420 = this.OnServerUpdateUserPublisherDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate420 in invocationList420)
				{
					if (object.ReferenceEquals(delegate420.Target, instance))
					{
						OnServerUpdateUserPublisherDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.UpdateUserDataRequest>)delegate420;
					}
				}
			}
			if (this.OnServerUpdateUserPublisherDataResultEvent != null)
			{
				Delegate[] invocationList421 = this.OnServerUpdateUserPublisherDataResultEvent.GetInvocationList();
				foreach (Delegate delegate421 in invocationList421)
				{
					if (object.ReferenceEquals(delegate421.Target, instance))
					{
						OnServerUpdateUserPublisherDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.UpdateUserDataResult>)delegate421;
					}
				}
			}
			if (this.OnServerUpdateUserPublisherInternalDataRequestEvent != null)
			{
				Delegate[] invocationList422 = this.OnServerUpdateUserPublisherInternalDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate422 in invocationList422)
				{
					if (object.ReferenceEquals(delegate422.Target, instance))
					{
						OnServerUpdateUserPublisherInternalDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.UpdateUserInternalDataRequest>)delegate422;
					}
				}
			}
			if (this.OnServerUpdateUserPublisherInternalDataResultEvent != null)
			{
				Delegate[] invocationList423 = this.OnServerUpdateUserPublisherInternalDataResultEvent.GetInvocationList();
				foreach (Delegate delegate423 in invocationList423)
				{
					if (object.ReferenceEquals(delegate423.Target, instance))
					{
						OnServerUpdateUserPublisherInternalDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.UpdateUserDataResult>)delegate423;
					}
				}
			}
			if (this.OnServerUpdateUserPublisherReadOnlyDataRequestEvent != null)
			{
				Delegate[] invocationList424 = this.OnServerUpdateUserPublisherReadOnlyDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate424 in invocationList424)
				{
					if (object.ReferenceEquals(delegate424.Target, instance))
					{
						OnServerUpdateUserPublisherReadOnlyDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.UpdateUserDataRequest>)delegate424;
					}
				}
			}
			if (this.OnServerUpdateUserPublisherReadOnlyDataResultEvent != null)
			{
				Delegate[] invocationList425 = this.OnServerUpdateUserPublisherReadOnlyDataResultEvent.GetInvocationList();
				foreach (Delegate delegate425 in invocationList425)
				{
					if (object.ReferenceEquals(delegate425.Target, instance))
					{
						OnServerUpdateUserPublisherReadOnlyDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.UpdateUserDataResult>)delegate425;
					}
				}
			}
			if (this.OnServerUpdateUserReadOnlyDataRequestEvent != null)
			{
				Delegate[] invocationList426 = this.OnServerUpdateUserReadOnlyDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate426 in invocationList426)
				{
					if (object.ReferenceEquals(delegate426.Target, instance))
					{
						OnServerUpdateUserReadOnlyDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.UpdateUserDataRequest>)delegate426;
					}
				}
			}
			if (this.OnServerUpdateUserReadOnlyDataResultEvent != null)
			{
				Delegate[] invocationList427 = this.OnServerUpdateUserReadOnlyDataResultEvent.GetInvocationList();
				foreach (Delegate delegate427 in invocationList427)
				{
					if (object.ReferenceEquals(delegate427.Target, instance))
					{
						OnServerUpdateUserReadOnlyDataResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.UpdateUserDataResult>)delegate427;
					}
				}
			}
			if (this.OnServerWriteCharacterEventRequestEvent != null)
			{
				Delegate[] invocationList428 = this.OnServerWriteCharacterEventRequestEvent.GetInvocationList();
				foreach (Delegate delegate428 in invocationList428)
				{
					if (object.ReferenceEquals(delegate428.Target, instance))
					{
						OnServerWriteCharacterEventRequestEvent -= (PlayFabRequestEvent<WriteServerCharacterEventRequest>)delegate428;
					}
				}
			}
			if (this.OnServerWriteCharacterEventResultEvent != null)
			{
				Delegate[] invocationList429 = this.OnServerWriteCharacterEventResultEvent.GetInvocationList();
				foreach (Delegate delegate429 in invocationList429)
				{
					if (object.ReferenceEquals(delegate429.Target, instance))
					{
						OnServerWriteCharacterEventResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.WriteEventResponse>)delegate429;
					}
				}
			}
			if (this.OnServerWritePlayerEventRequestEvent != null)
			{
				Delegate[] invocationList430 = this.OnServerWritePlayerEventRequestEvent.GetInvocationList();
				foreach (Delegate delegate430 in invocationList430)
				{
					if (object.ReferenceEquals(delegate430.Target, instance))
					{
						OnServerWritePlayerEventRequestEvent -= (PlayFabRequestEvent<WriteServerPlayerEventRequest>)delegate430;
					}
				}
			}
			if (this.OnServerWritePlayerEventResultEvent != null)
			{
				Delegate[] invocationList431 = this.OnServerWritePlayerEventResultEvent.GetInvocationList();
				foreach (Delegate delegate431 in invocationList431)
				{
					if (object.ReferenceEquals(delegate431.Target, instance))
					{
						OnServerWritePlayerEventResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.WriteEventResponse>)delegate431;
					}
				}
			}
			if (this.OnServerWriteTitleEventRequestEvent != null)
			{
				Delegate[] invocationList432 = this.OnServerWriteTitleEventRequestEvent.GetInvocationList();
				foreach (Delegate delegate432 in invocationList432)
				{
					if (object.ReferenceEquals(delegate432.Target, instance))
					{
						OnServerWriteTitleEventRequestEvent -= (PlayFabRequestEvent<PlayFab.ServerModels.WriteTitleEventRequest>)delegate432;
					}
				}
			}
			if (this.OnServerWriteTitleEventResultEvent != null)
			{
				Delegate[] invocationList433 = this.OnServerWriteTitleEventResultEvent.GetInvocationList();
				foreach (Delegate delegate433 in invocationList433)
				{
					if (object.ReferenceEquals(delegate433.Target, instance))
					{
						OnServerWriteTitleEventResultEvent -= (PlayFabResultEvent<PlayFab.ServerModels.WriteEventResponse>)delegate433;
					}
				}
			}
			if (this.OnAcceptTradeRequestEvent != null)
			{
				Delegate[] invocationList434 = this.OnAcceptTradeRequestEvent.GetInvocationList();
				foreach (Delegate delegate434 in invocationList434)
				{
					if (object.ReferenceEquals(delegate434.Target, instance))
					{
						OnAcceptTradeRequestEvent -= (PlayFabRequestEvent<AcceptTradeRequest>)delegate434;
					}
				}
			}
			if (this.OnAcceptTradeResultEvent != null)
			{
				Delegate[] invocationList435 = this.OnAcceptTradeResultEvent.GetInvocationList();
				foreach (Delegate delegate435 in invocationList435)
				{
					if (object.ReferenceEquals(delegate435.Target, instance))
					{
						OnAcceptTradeResultEvent -= (PlayFabResultEvent<AcceptTradeResponse>)delegate435;
					}
				}
			}
			if (this.OnAddFriendRequestEvent != null)
			{
				Delegate[] invocationList436 = this.OnAddFriendRequestEvent.GetInvocationList();
				foreach (Delegate delegate436 in invocationList436)
				{
					if (object.ReferenceEquals(delegate436.Target, instance))
					{
						OnAddFriendRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.AddFriendRequest>)delegate436;
					}
				}
			}
			if (this.OnAddFriendResultEvent != null)
			{
				Delegate[] invocationList437 = this.OnAddFriendResultEvent.GetInvocationList();
				foreach (Delegate delegate437 in invocationList437)
				{
					if (object.ReferenceEquals(delegate437.Target, instance))
					{
						OnAddFriendResultEvent -= (PlayFabResultEvent<AddFriendResult>)delegate437;
					}
				}
			}
			if (this.OnAddGenericIDRequestEvent != null)
			{
				Delegate[] invocationList438 = this.OnAddGenericIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate438 in invocationList438)
				{
					if (object.ReferenceEquals(delegate438.Target, instance))
					{
						OnAddGenericIDRequestEvent -= (PlayFabRequestEvent<AddGenericIDRequest>)delegate438;
					}
				}
			}
			if (this.OnAddGenericIDResultEvent != null)
			{
				Delegate[] invocationList439 = this.OnAddGenericIDResultEvent.GetInvocationList();
				foreach (Delegate delegate439 in invocationList439)
				{
					if (object.ReferenceEquals(delegate439.Target, instance))
					{
						OnAddGenericIDResultEvent -= (PlayFabResultEvent<AddGenericIDResult>)delegate439;
					}
				}
			}
			if (this.OnAddOrUpdateContactEmailRequestEvent != null)
			{
				Delegate[] invocationList440 = this.OnAddOrUpdateContactEmailRequestEvent.GetInvocationList();
				foreach (Delegate delegate440 in invocationList440)
				{
					if (object.ReferenceEquals(delegate440.Target, instance))
					{
						OnAddOrUpdateContactEmailRequestEvent -= (PlayFabRequestEvent<AddOrUpdateContactEmailRequest>)delegate440;
					}
				}
			}
			if (this.OnAddOrUpdateContactEmailResultEvent != null)
			{
				Delegate[] invocationList441 = this.OnAddOrUpdateContactEmailResultEvent.GetInvocationList();
				foreach (Delegate delegate441 in invocationList441)
				{
					if (object.ReferenceEquals(delegate441.Target, instance))
					{
						OnAddOrUpdateContactEmailResultEvent -= (PlayFabResultEvent<AddOrUpdateContactEmailResult>)delegate441;
					}
				}
			}
			if (this.OnAddSharedGroupMembersRequestEvent != null)
			{
				Delegate[] invocationList442 = this.OnAddSharedGroupMembersRequestEvent.GetInvocationList();
				foreach (Delegate delegate442 in invocationList442)
				{
					if (object.ReferenceEquals(delegate442.Target, instance))
					{
						OnAddSharedGroupMembersRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.AddSharedGroupMembersRequest>)delegate442;
					}
				}
			}
			if (this.OnAddSharedGroupMembersResultEvent != null)
			{
				Delegate[] invocationList443 = this.OnAddSharedGroupMembersResultEvent.GetInvocationList();
				foreach (Delegate delegate443 in invocationList443)
				{
					if (object.ReferenceEquals(delegate443.Target, instance))
					{
						OnAddSharedGroupMembersResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.AddSharedGroupMembersResult>)delegate443;
					}
				}
			}
			if (this.OnAddUsernamePasswordRequestEvent != null)
			{
				Delegate[] invocationList444 = this.OnAddUsernamePasswordRequestEvent.GetInvocationList();
				foreach (Delegate delegate444 in invocationList444)
				{
					if (object.ReferenceEquals(delegate444.Target, instance))
					{
						OnAddUsernamePasswordRequestEvent -= (PlayFabRequestEvent<AddUsernamePasswordRequest>)delegate444;
					}
				}
			}
			if (this.OnAddUsernamePasswordResultEvent != null)
			{
				Delegate[] invocationList445 = this.OnAddUsernamePasswordResultEvent.GetInvocationList();
				foreach (Delegate delegate445 in invocationList445)
				{
					if (object.ReferenceEquals(delegate445.Target, instance))
					{
						OnAddUsernamePasswordResultEvent -= (PlayFabResultEvent<AddUsernamePasswordResult>)delegate445;
					}
				}
			}
			if (this.OnAddUserVirtualCurrencyRequestEvent != null)
			{
				Delegate[] invocationList446 = this.OnAddUserVirtualCurrencyRequestEvent.GetInvocationList();
				foreach (Delegate delegate446 in invocationList446)
				{
					if (object.ReferenceEquals(delegate446.Target, instance))
					{
						OnAddUserVirtualCurrencyRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.AddUserVirtualCurrencyRequest>)delegate446;
					}
				}
			}
			if (this.OnAddUserVirtualCurrencyResultEvent != null)
			{
				Delegate[] invocationList447 = this.OnAddUserVirtualCurrencyResultEvent.GetInvocationList();
				foreach (Delegate delegate447 in invocationList447)
				{
					if (object.ReferenceEquals(delegate447.Target, instance))
					{
						OnAddUserVirtualCurrencyResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.ModifyUserVirtualCurrencyResult>)delegate447;
					}
				}
			}
			if (this.OnAndroidDevicePushNotificationRegistrationRequestEvent != null)
			{
				Delegate[] invocationList448 = this.OnAndroidDevicePushNotificationRegistrationRequestEvent.GetInvocationList();
				foreach (Delegate delegate448 in invocationList448)
				{
					if (object.ReferenceEquals(delegate448.Target, instance))
					{
						OnAndroidDevicePushNotificationRegistrationRequestEvent -= (PlayFabRequestEvent<AndroidDevicePushNotificationRegistrationRequest>)delegate448;
					}
				}
			}
			if (this.OnAndroidDevicePushNotificationRegistrationResultEvent != null)
			{
				Delegate[] invocationList449 = this.OnAndroidDevicePushNotificationRegistrationResultEvent.GetInvocationList();
				foreach (Delegate delegate449 in invocationList449)
				{
					if (object.ReferenceEquals(delegate449.Target, instance))
					{
						OnAndroidDevicePushNotificationRegistrationResultEvent -= (PlayFabResultEvent<AndroidDevicePushNotificationRegistrationResult>)delegate449;
					}
				}
			}
			if (this.OnAttributeInstallRequestEvent != null)
			{
				Delegate[] invocationList450 = this.OnAttributeInstallRequestEvent.GetInvocationList();
				foreach (Delegate delegate450 in invocationList450)
				{
					if (object.ReferenceEquals(delegate450.Target, instance))
					{
						OnAttributeInstallRequestEvent -= (PlayFabRequestEvent<AttributeInstallRequest>)delegate450;
					}
				}
			}
			if (this.OnAttributeInstallResultEvent != null)
			{
				Delegate[] invocationList451 = this.OnAttributeInstallResultEvent.GetInvocationList();
				foreach (Delegate delegate451 in invocationList451)
				{
					if (object.ReferenceEquals(delegate451.Target, instance))
					{
						OnAttributeInstallResultEvent -= (PlayFabResultEvent<AttributeInstallResult>)delegate451;
					}
				}
			}
			if (this.OnCancelTradeRequestEvent != null)
			{
				Delegate[] invocationList452 = this.OnCancelTradeRequestEvent.GetInvocationList();
				foreach (Delegate delegate452 in invocationList452)
				{
					if (object.ReferenceEquals(delegate452.Target, instance))
					{
						OnCancelTradeRequestEvent -= (PlayFabRequestEvent<CancelTradeRequest>)delegate452;
					}
				}
			}
			if (this.OnCancelTradeResultEvent != null)
			{
				Delegate[] invocationList453 = this.OnCancelTradeResultEvent.GetInvocationList();
				foreach (Delegate delegate453 in invocationList453)
				{
					if (object.ReferenceEquals(delegate453.Target, instance))
					{
						OnCancelTradeResultEvent -= (PlayFabResultEvent<CancelTradeResponse>)delegate453;
					}
				}
			}
			if (this.OnConfirmPurchaseRequestEvent != null)
			{
				Delegate[] invocationList454 = this.OnConfirmPurchaseRequestEvent.GetInvocationList();
				foreach (Delegate delegate454 in invocationList454)
				{
					if (object.ReferenceEquals(delegate454.Target, instance))
					{
						OnConfirmPurchaseRequestEvent -= (PlayFabRequestEvent<ConfirmPurchaseRequest>)delegate454;
					}
				}
			}
			if (this.OnConfirmPurchaseResultEvent != null)
			{
				Delegate[] invocationList455 = this.OnConfirmPurchaseResultEvent.GetInvocationList();
				foreach (Delegate delegate455 in invocationList455)
				{
					if (object.ReferenceEquals(delegate455.Target, instance))
					{
						OnConfirmPurchaseResultEvent -= (PlayFabResultEvent<ConfirmPurchaseResult>)delegate455;
					}
				}
			}
			if (this.OnConsumeItemRequestEvent != null)
			{
				Delegate[] invocationList456 = this.OnConsumeItemRequestEvent.GetInvocationList();
				foreach (Delegate delegate456 in invocationList456)
				{
					if (object.ReferenceEquals(delegate456.Target, instance))
					{
						OnConsumeItemRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.ConsumeItemRequest>)delegate456;
					}
				}
			}
			if (this.OnConsumeItemResultEvent != null)
			{
				Delegate[] invocationList457 = this.OnConsumeItemResultEvent.GetInvocationList();
				foreach (Delegate delegate457 in invocationList457)
				{
					if (object.ReferenceEquals(delegate457.Target, instance))
					{
						OnConsumeItemResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.ConsumeItemResult>)delegate457;
					}
				}
			}
			if (this.OnCreateSharedGroupRequestEvent != null)
			{
				Delegate[] invocationList458 = this.OnCreateSharedGroupRequestEvent.GetInvocationList();
				foreach (Delegate delegate458 in invocationList458)
				{
					if (object.ReferenceEquals(delegate458.Target, instance))
					{
						OnCreateSharedGroupRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.CreateSharedGroupRequest>)delegate458;
					}
				}
			}
			if (this.OnCreateSharedGroupResultEvent != null)
			{
				Delegate[] invocationList459 = this.OnCreateSharedGroupResultEvent.GetInvocationList();
				foreach (Delegate delegate459 in invocationList459)
				{
					if (object.ReferenceEquals(delegate459.Target, instance))
					{
						OnCreateSharedGroupResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.CreateSharedGroupResult>)delegate459;
					}
				}
			}
			if (this.OnExecuteCloudScriptRequestEvent != null)
			{
				Delegate[] invocationList460 = this.OnExecuteCloudScriptRequestEvent.GetInvocationList();
				foreach (Delegate delegate460 in invocationList460)
				{
					if (object.ReferenceEquals(delegate460.Target, instance))
					{
						OnExecuteCloudScriptRequestEvent -= (PlayFabRequestEvent<ExecuteCloudScriptRequest>)delegate460;
					}
				}
			}
			if (this.OnExecuteCloudScriptResultEvent != null)
			{
				Delegate[] invocationList461 = this.OnExecuteCloudScriptResultEvent.GetInvocationList();
				foreach (Delegate delegate461 in invocationList461)
				{
					if (object.ReferenceEquals(delegate461.Target, instance))
					{
						OnExecuteCloudScriptResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.ExecuteCloudScriptResult>)delegate461;
					}
				}
			}
			if (this.OnGetAccountInfoRequestEvent != null)
			{
				Delegate[] invocationList462 = this.OnGetAccountInfoRequestEvent.GetInvocationList();
				foreach (Delegate delegate462 in invocationList462)
				{
					if (object.ReferenceEquals(delegate462.Target, instance))
					{
						OnGetAccountInfoRequestEvent -= (PlayFabRequestEvent<GetAccountInfoRequest>)delegate462;
					}
				}
			}
			if (this.OnGetAccountInfoResultEvent != null)
			{
				Delegate[] invocationList463 = this.OnGetAccountInfoResultEvent.GetInvocationList();
				foreach (Delegate delegate463 in invocationList463)
				{
					if (object.ReferenceEquals(delegate463.Target, instance))
					{
						OnGetAccountInfoResultEvent -= (PlayFabResultEvent<GetAccountInfoResult>)delegate463;
					}
				}
			}
			if (this.OnGetAllUsersCharactersRequestEvent != null)
			{
				Delegate[] invocationList464 = this.OnGetAllUsersCharactersRequestEvent.GetInvocationList();
				foreach (Delegate delegate464 in invocationList464)
				{
					if (object.ReferenceEquals(delegate464.Target, instance))
					{
						OnGetAllUsersCharactersRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.ListUsersCharactersRequest>)delegate464;
					}
				}
			}
			if (this.OnGetAllUsersCharactersResultEvent != null)
			{
				Delegate[] invocationList465 = this.OnGetAllUsersCharactersResultEvent.GetInvocationList();
				foreach (Delegate delegate465 in invocationList465)
				{
					if (object.ReferenceEquals(delegate465.Target, instance))
					{
						OnGetAllUsersCharactersResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.ListUsersCharactersResult>)delegate465;
					}
				}
			}
			if (this.OnGetCatalogItemsRequestEvent != null)
			{
				Delegate[] invocationList466 = this.OnGetCatalogItemsRequestEvent.GetInvocationList();
				foreach (Delegate delegate466 in invocationList466)
				{
					if (object.ReferenceEquals(delegate466.Target, instance))
					{
						OnGetCatalogItemsRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetCatalogItemsRequest>)delegate466;
					}
				}
			}
			if (this.OnGetCatalogItemsResultEvent != null)
			{
				Delegate[] invocationList467 = this.OnGetCatalogItemsResultEvent.GetInvocationList();
				foreach (Delegate delegate467 in invocationList467)
				{
					if (object.ReferenceEquals(delegate467.Target, instance))
					{
						OnGetCatalogItemsResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetCatalogItemsResult>)delegate467;
					}
				}
			}
			if (this.OnGetCharacterDataRequestEvent != null)
			{
				Delegate[] invocationList468 = this.OnGetCharacterDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate468 in invocationList468)
				{
					if (object.ReferenceEquals(delegate468.Target, instance))
					{
						OnGetCharacterDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetCharacterDataRequest>)delegate468;
					}
				}
			}
			if (this.OnGetCharacterDataResultEvent != null)
			{
				Delegate[] invocationList469 = this.OnGetCharacterDataResultEvent.GetInvocationList();
				foreach (Delegate delegate469 in invocationList469)
				{
					if (object.ReferenceEquals(delegate469.Target, instance))
					{
						OnGetCharacterDataResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetCharacterDataResult>)delegate469;
					}
				}
			}
			if (this.OnGetCharacterInventoryRequestEvent != null)
			{
				Delegate[] invocationList470 = this.OnGetCharacterInventoryRequestEvent.GetInvocationList();
				foreach (Delegate delegate470 in invocationList470)
				{
					if (object.ReferenceEquals(delegate470.Target, instance))
					{
						OnGetCharacterInventoryRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetCharacterInventoryRequest>)delegate470;
					}
				}
			}
			if (this.OnGetCharacterInventoryResultEvent != null)
			{
				Delegate[] invocationList471 = this.OnGetCharacterInventoryResultEvent.GetInvocationList();
				foreach (Delegate delegate471 in invocationList471)
				{
					if (object.ReferenceEquals(delegate471.Target, instance))
					{
						OnGetCharacterInventoryResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetCharacterInventoryResult>)delegate471;
					}
				}
			}
			if (this.OnGetCharacterLeaderboardRequestEvent != null)
			{
				Delegate[] invocationList472 = this.OnGetCharacterLeaderboardRequestEvent.GetInvocationList();
				foreach (Delegate delegate472 in invocationList472)
				{
					if (object.ReferenceEquals(delegate472.Target, instance))
					{
						OnGetCharacterLeaderboardRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetCharacterLeaderboardRequest>)delegate472;
					}
				}
			}
			if (this.OnGetCharacterLeaderboardResultEvent != null)
			{
				Delegate[] invocationList473 = this.OnGetCharacterLeaderboardResultEvent.GetInvocationList();
				foreach (Delegate delegate473 in invocationList473)
				{
					if (object.ReferenceEquals(delegate473.Target, instance))
					{
						OnGetCharacterLeaderboardResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetCharacterLeaderboardResult>)delegate473;
					}
				}
			}
			if (this.OnGetCharacterReadOnlyDataRequestEvent != null)
			{
				Delegate[] invocationList474 = this.OnGetCharacterReadOnlyDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate474 in invocationList474)
				{
					if (object.ReferenceEquals(delegate474.Target, instance))
					{
						OnGetCharacterReadOnlyDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetCharacterDataRequest>)delegate474;
					}
				}
			}
			if (this.OnGetCharacterReadOnlyDataResultEvent != null)
			{
				Delegate[] invocationList475 = this.OnGetCharacterReadOnlyDataResultEvent.GetInvocationList();
				foreach (Delegate delegate475 in invocationList475)
				{
					if (object.ReferenceEquals(delegate475.Target, instance))
					{
						OnGetCharacterReadOnlyDataResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetCharacterDataResult>)delegate475;
					}
				}
			}
			if (this.OnGetCharacterStatisticsRequestEvent != null)
			{
				Delegate[] invocationList476 = this.OnGetCharacterStatisticsRequestEvent.GetInvocationList();
				foreach (Delegate delegate476 in invocationList476)
				{
					if (object.ReferenceEquals(delegate476.Target, instance))
					{
						OnGetCharacterStatisticsRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetCharacterStatisticsRequest>)delegate476;
					}
				}
			}
			if (this.OnGetCharacterStatisticsResultEvent != null)
			{
				Delegate[] invocationList477 = this.OnGetCharacterStatisticsResultEvent.GetInvocationList();
				foreach (Delegate delegate477 in invocationList477)
				{
					if (object.ReferenceEquals(delegate477.Target, instance))
					{
						OnGetCharacterStatisticsResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetCharacterStatisticsResult>)delegate477;
					}
				}
			}
			if (this.OnGetContentDownloadUrlRequestEvent != null)
			{
				Delegate[] invocationList478 = this.OnGetContentDownloadUrlRequestEvent.GetInvocationList();
				foreach (Delegate delegate478 in invocationList478)
				{
					if (object.ReferenceEquals(delegate478.Target, instance))
					{
						OnGetContentDownloadUrlRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetContentDownloadUrlRequest>)delegate478;
					}
				}
			}
			if (this.OnGetContentDownloadUrlResultEvent != null)
			{
				Delegate[] invocationList479 = this.OnGetContentDownloadUrlResultEvent.GetInvocationList();
				foreach (Delegate delegate479 in invocationList479)
				{
					if (object.ReferenceEquals(delegate479.Target, instance))
					{
						OnGetContentDownloadUrlResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetContentDownloadUrlResult>)delegate479;
					}
				}
			}
			if (this.OnGetCurrentGamesRequestEvent != null)
			{
				Delegate[] invocationList480 = this.OnGetCurrentGamesRequestEvent.GetInvocationList();
				foreach (Delegate delegate480 in invocationList480)
				{
					if (object.ReferenceEquals(delegate480.Target, instance))
					{
						OnGetCurrentGamesRequestEvent -= (PlayFabRequestEvent<CurrentGamesRequest>)delegate480;
					}
				}
			}
			if (this.OnGetCurrentGamesResultEvent != null)
			{
				Delegate[] invocationList481 = this.OnGetCurrentGamesResultEvent.GetInvocationList();
				foreach (Delegate delegate481 in invocationList481)
				{
					if (object.ReferenceEquals(delegate481.Target, instance))
					{
						OnGetCurrentGamesResultEvent -= (PlayFabResultEvent<CurrentGamesResult>)delegate481;
					}
				}
			}
			if (this.OnGetFriendLeaderboardRequestEvent != null)
			{
				Delegate[] invocationList482 = this.OnGetFriendLeaderboardRequestEvent.GetInvocationList();
				foreach (Delegate delegate482 in invocationList482)
				{
					if (object.ReferenceEquals(delegate482.Target, instance))
					{
						OnGetFriendLeaderboardRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetFriendLeaderboardRequest>)delegate482;
					}
				}
			}
			if (this.OnGetFriendLeaderboardResultEvent != null)
			{
				Delegate[] invocationList483 = this.OnGetFriendLeaderboardResultEvent.GetInvocationList();
				foreach (Delegate delegate483 in invocationList483)
				{
					if (object.ReferenceEquals(delegate483.Target, instance))
					{
						OnGetFriendLeaderboardResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetLeaderboardResult>)delegate483;
					}
				}
			}
			if (this.OnGetFriendLeaderboardAroundPlayerRequestEvent != null)
			{
				Delegate[] invocationList484 = this.OnGetFriendLeaderboardAroundPlayerRequestEvent.GetInvocationList();
				foreach (Delegate delegate484 in invocationList484)
				{
					if (object.ReferenceEquals(delegate484.Target, instance))
					{
						OnGetFriendLeaderboardAroundPlayerRequestEvent -= (PlayFabRequestEvent<GetFriendLeaderboardAroundPlayerRequest>)delegate484;
					}
				}
			}
			if (this.OnGetFriendLeaderboardAroundPlayerResultEvent != null)
			{
				Delegate[] invocationList485 = this.OnGetFriendLeaderboardAroundPlayerResultEvent.GetInvocationList();
				foreach (Delegate delegate485 in invocationList485)
				{
					if (object.ReferenceEquals(delegate485.Target, instance))
					{
						OnGetFriendLeaderboardAroundPlayerResultEvent -= (PlayFabResultEvent<GetFriendLeaderboardAroundPlayerResult>)delegate485;
					}
				}
			}
			if (this.OnGetFriendsListRequestEvent != null)
			{
				Delegate[] invocationList486 = this.OnGetFriendsListRequestEvent.GetInvocationList();
				foreach (Delegate delegate486 in invocationList486)
				{
					if (object.ReferenceEquals(delegate486.Target, instance))
					{
						OnGetFriendsListRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetFriendsListRequest>)delegate486;
					}
				}
			}
			if (this.OnGetFriendsListResultEvent != null)
			{
				Delegate[] invocationList487 = this.OnGetFriendsListResultEvent.GetInvocationList();
				foreach (Delegate delegate487 in invocationList487)
				{
					if (object.ReferenceEquals(delegate487.Target, instance))
					{
						OnGetFriendsListResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetFriendsListResult>)delegate487;
					}
				}
			}
			if (this.OnGetGameServerRegionsRequestEvent != null)
			{
				Delegate[] invocationList488 = this.OnGetGameServerRegionsRequestEvent.GetInvocationList();
				foreach (Delegate delegate488 in invocationList488)
				{
					if (object.ReferenceEquals(delegate488.Target, instance))
					{
						OnGetGameServerRegionsRequestEvent -= (PlayFabRequestEvent<GameServerRegionsRequest>)delegate488;
					}
				}
			}
			if (this.OnGetGameServerRegionsResultEvent != null)
			{
				Delegate[] invocationList489 = this.OnGetGameServerRegionsResultEvent.GetInvocationList();
				foreach (Delegate delegate489 in invocationList489)
				{
					if (object.ReferenceEquals(delegate489.Target, instance))
					{
						OnGetGameServerRegionsResultEvent -= (PlayFabResultEvent<GameServerRegionsResult>)delegate489;
					}
				}
			}
			if (this.OnGetLeaderboardRequestEvent != null)
			{
				Delegate[] invocationList490 = this.OnGetLeaderboardRequestEvent.GetInvocationList();
				foreach (Delegate delegate490 in invocationList490)
				{
					if (object.ReferenceEquals(delegate490.Target, instance))
					{
						OnGetLeaderboardRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetLeaderboardRequest>)delegate490;
					}
				}
			}
			if (this.OnGetLeaderboardResultEvent != null)
			{
				Delegate[] invocationList491 = this.OnGetLeaderboardResultEvent.GetInvocationList();
				foreach (Delegate delegate491 in invocationList491)
				{
					if (object.ReferenceEquals(delegate491.Target, instance))
					{
						OnGetLeaderboardResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetLeaderboardResult>)delegate491;
					}
				}
			}
			if (this.OnGetLeaderboardAroundCharacterRequestEvent != null)
			{
				Delegate[] invocationList492 = this.OnGetLeaderboardAroundCharacterRequestEvent.GetInvocationList();
				foreach (Delegate delegate492 in invocationList492)
				{
					if (object.ReferenceEquals(delegate492.Target, instance))
					{
						OnGetLeaderboardAroundCharacterRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetLeaderboardAroundCharacterRequest>)delegate492;
					}
				}
			}
			if (this.OnGetLeaderboardAroundCharacterResultEvent != null)
			{
				Delegate[] invocationList493 = this.OnGetLeaderboardAroundCharacterResultEvent.GetInvocationList();
				foreach (Delegate delegate493 in invocationList493)
				{
					if (object.ReferenceEquals(delegate493.Target, instance))
					{
						OnGetLeaderboardAroundCharacterResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetLeaderboardAroundCharacterResult>)delegate493;
					}
				}
			}
			if (this.OnGetLeaderboardAroundPlayerRequestEvent != null)
			{
				Delegate[] invocationList494 = this.OnGetLeaderboardAroundPlayerRequestEvent.GetInvocationList();
				foreach (Delegate delegate494 in invocationList494)
				{
					if (object.ReferenceEquals(delegate494.Target, instance))
					{
						OnGetLeaderboardAroundPlayerRequestEvent -= (PlayFabRequestEvent<GetLeaderboardAroundPlayerRequest>)delegate494;
					}
				}
			}
			if (this.OnGetLeaderboardAroundPlayerResultEvent != null)
			{
				Delegate[] invocationList495 = this.OnGetLeaderboardAroundPlayerResultEvent.GetInvocationList();
				foreach (Delegate delegate495 in invocationList495)
				{
					if (object.ReferenceEquals(delegate495.Target, instance))
					{
						OnGetLeaderboardAroundPlayerResultEvent -= (PlayFabResultEvent<GetLeaderboardAroundPlayerResult>)delegate495;
					}
				}
			}
			if (this.OnGetLeaderboardForUserCharactersRequestEvent != null)
			{
				Delegate[] invocationList496 = this.OnGetLeaderboardForUserCharactersRequestEvent.GetInvocationList();
				foreach (Delegate delegate496 in invocationList496)
				{
					if (object.ReferenceEquals(delegate496.Target, instance))
					{
						OnGetLeaderboardForUserCharactersRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetLeaderboardForUsersCharactersRequest>)delegate496;
					}
				}
			}
			if (this.OnGetLeaderboardForUserCharactersResultEvent != null)
			{
				Delegate[] invocationList497 = this.OnGetLeaderboardForUserCharactersResultEvent.GetInvocationList();
				foreach (Delegate delegate497 in invocationList497)
				{
					if (object.ReferenceEquals(delegate497.Target, instance))
					{
						OnGetLeaderboardForUserCharactersResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetLeaderboardForUsersCharactersResult>)delegate497;
					}
				}
			}
			if (this.OnGetPaymentTokenRequestEvent != null)
			{
				Delegate[] invocationList498 = this.OnGetPaymentTokenRequestEvent.GetInvocationList();
				foreach (Delegate delegate498 in invocationList498)
				{
					if (object.ReferenceEquals(delegate498.Target, instance))
					{
						OnGetPaymentTokenRequestEvent -= (PlayFabRequestEvent<GetPaymentTokenRequest>)delegate498;
					}
				}
			}
			if (this.OnGetPaymentTokenResultEvent != null)
			{
				Delegate[] invocationList499 = this.OnGetPaymentTokenResultEvent.GetInvocationList();
				foreach (Delegate delegate499 in invocationList499)
				{
					if (object.ReferenceEquals(delegate499.Target, instance))
					{
						OnGetPaymentTokenResultEvent -= (PlayFabResultEvent<GetPaymentTokenResult>)delegate499;
					}
				}
			}
			if (this.OnGetPhotonAuthenticationTokenRequestEvent != null)
			{
				Delegate[] invocationList500 = this.OnGetPhotonAuthenticationTokenRequestEvent.GetInvocationList();
				foreach (Delegate delegate500 in invocationList500)
				{
					if (object.ReferenceEquals(delegate500.Target, instance))
					{
						OnGetPhotonAuthenticationTokenRequestEvent -= (PlayFabRequestEvent<GetPhotonAuthenticationTokenRequest>)delegate500;
					}
				}
			}
			if (this.OnGetPhotonAuthenticationTokenResultEvent != null)
			{
				Delegate[] invocationList501 = this.OnGetPhotonAuthenticationTokenResultEvent.GetInvocationList();
				foreach (Delegate delegate501 in invocationList501)
				{
					if (object.ReferenceEquals(delegate501.Target, instance))
					{
						OnGetPhotonAuthenticationTokenResultEvent -= (PlayFabResultEvent<GetPhotonAuthenticationTokenResult>)delegate501;
					}
				}
			}
			if (this.OnGetPlayerCombinedInfoRequestEvent != null)
			{
				Delegate[] invocationList502 = this.OnGetPlayerCombinedInfoRequestEvent.GetInvocationList();
				foreach (Delegate delegate502 in invocationList502)
				{
					if (object.ReferenceEquals(delegate502.Target, instance))
					{
						OnGetPlayerCombinedInfoRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetPlayerCombinedInfoRequest>)delegate502;
					}
				}
			}
			if (this.OnGetPlayerCombinedInfoResultEvent != null)
			{
				Delegate[] invocationList503 = this.OnGetPlayerCombinedInfoResultEvent.GetInvocationList();
				foreach (Delegate delegate503 in invocationList503)
				{
					if (object.ReferenceEquals(delegate503.Target, instance))
					{
						OnGetPlayerCombinedInfoResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetPlayerCombinedInfoResult>)delegate503;
					}
				}
			}
			if (this.OnGetPlayerProfileRequestEvent != null)
			{
				Delegate[] invocationList504 = this.OnGetPlayerProfileRequestEvent.GetInvocationList();
				foreach (Delegate delegate504 in invocationList504)
				{
					if (object.ReferenceEquals(delegate504.Target, instance))
					{
						OnGetPlayerProfileRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetPlayerProfileRequest>)delegate504;
					}
				}
			}
			if (this.OnGetPlayerProfileResultEvent != null)
			{
				Delegate[] invocationList505 = this.OnGetPlayerProfileResultEvent.GetInvocationList();
				foreach (Delegate delegate505 in invocationList505)
				{
					if (object.ReferenceEquals(delegate505.Target, instance))
					{
						OnGetPlayerProfileResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetPlayerProfileResult>)delegate505;
					}
				}
			}
			if (this.OnGetPlayerSegmentsRequestEvent != null)
			{
				Delegate[] invocationList506 = this.OnGetPlayerSegmentsRequestEvent.GetInvocationList();
				foreach (Delegate delegate506 in invocationList506)
				{
					if (object.ReferenceEquals(delegate506.Target, instance))
					{
						OnGetPlayerSegmentsRequestEvent -= (PlayFabRequestEvent<GetPlayerSegmentsRequest>)delegate506;
					}
				}
			}
			if (this.OnGetPlayerSegmentsResultEvent != null)
			{
				Delegate[] invocationList507 = this.OnGetPlayerSegmentsResultEvent.GetInvocationList();
				foreach (Delegate delegate507 in invocationList507)
				{
					if (object.ReferenceEquals(delegate507.Target, instance))
					{
						OnGetPlayerSegmentsResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetPlayerSegmentsResult>)delegate507;
					}
				}
			}
			if (this.OnGetPlayerStatisticsRequestEvent != null)
			{
				Delegate[] invocationList508 = this.OnGetPlayerStatisticsRequestEvent.GetInvocationList();
				foreach (Delegate delegate508 in invocationList508)
				{
					if (object.ReferenceEquals(delegate508.Target, instance))
					{
						OnGetPlayerStatisticsRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetPlayerStatisticsRequest>)delegate508;
					}
				}
			}
			if (this.OnGetPlayerStatisticsResultEvent != null)
			{
				Delegate[] invocationList509 = this.OnGetPlayerStatisticsResultEvent.GetInvocationList();
				foreach (Delegate delegate509 in invocationList509)
				{
					if (object.ReferenceEquals(delegate509.Target, instance))
					{
						OnGetPlayerStatisticsResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetPlayerStatisticsResult>)delegate509;
					}
				}
			}
			if (this.OnGetPlayerStatisticVersionsRequestEvent != null)
			{
				Delegate[] invocationList510 = this.OnGetPlayerStatisticVersionsRequestEvent.GetInvocationList();
				foreach (Delegate delegate510 in invocationList510)
				{
					if (object.ReferenceEquals(delegate510.Target, instance))
					{
						OnGetPlayerStatisticVersionsRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetPlayerStatisticVersionsRequest>)delegate510;
					}
				}
			}
			if (this.OnGetPlayerStatisticVersionsResultEvent != null)
			{
				Delegate[] invocationList511 = this.OnGetPlayerStatisticVersionsResultEvent.GetInvocationList();
				foreach (Delegate delegate511 in invocationList511)
				{
					if (object.ReferenceEquals(delegate511.Target, instance))
					{
						OnGetPlayerStatisticVersionsResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetPlayerStatisticVersionsResult>)delegate511;
					}
				}
			}
			if (this.OnGetPlayerTagsRequestEvent != null)
			{
				Delegate[] invocationList512 = this.OnGetPlayerTagsRequestEvent.GetInvocationList();
				foreach (Delegate delegate512 in invocationList512)
				{
					if (object.ReferenceEquals(delegate512.Target, instance))
					{
						OnGetPlayerTagsRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetPlayerTagsRequest>)delegate512;
					}
				}
			}
			if (this.OnGetPlayerTagsResultEvent != null)
			{
				Delegate[] invocationList513 = this.OnGetPlayerTagsResultEvent.GetInvocationList();
				foreach (Delegate delegate513 in invocationList513)
				{
					if (object.ReferenceEquals(delegate513.Target, instance))
					{
						OnGetPlayerTagsResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetPlayerTagsResult>)delegate513;
					}
				}
			}
			if (this.OnGetPlayerTradesRequestEvent != null)
			{
				Delegate[] invocationList514 = this.OnGetPlayerTradesRequestEvent.GetInvocationList();
				foreach (Delegate delegate514 in invocationList514)
				{
					if (object.ReferenceEquals(delegate514.Target, instance))
					{
						OnGetPlayerTradesRequestEvent -= (PlayFabRequestEvent<GetPlayerTradesRequest>)delegate514;
					}
				}
			}
			if (this.OnGetPlayerTradesResultEvent != null)
			{
				Delegate[] invocationList515 = this.OnGetPlayerTradesResultEvent.GetInvocationList();
				foreach (Delegate delegate515 in invocationList515)
				{
					if (object.ReferenceEquals(delegate515.Target, instance))
					{
						OnGetPlayerTradesResultEvent -= (PlayFabResultEvent<GetPlayerTradesResponse>)delegate515;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromFacebookIDsRequestEvent != null)
			{
				Delegate[] invocationList516 = this.OnGetPlayFabIDsFromFacebookIDsRequestEvent.GetInvocationList();
				foreach (Delegate delegate516 in invocationList516)
				{
					if (object.ReferenceEquals(delegate516.Target, instance))
					{
						OnGetPlayFabIDsFromFacebookIDsRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetPlayFabIDsFromFacebookIDsRequest>)delegate516;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromFacebookIDsResultEvent != null)
			{
				Delegate[] invocationList517 = this.OnGetPlayFabIDsFromFacebookIDsResultEvent.GetInvocationList();
				foreach (Delegate delegate517 in invocationList517)
				{
					if (object.ReferenceEquals(delegate517.Target, instance))
					{
						OnGetPlayFabIDsFromFacebookIDsResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetPlayFabIDsFromFacebookIDsResult>)delegate517;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromGameCenterIDsRequestEvent != null)
			{
				Delegate[] invocationList518 = this.OnGetPlayFabIDsFromGameCenterIDsRequestEvent.GetInvocationList();
				foreach (Delegate delegate518 in invocationList518)
				{
					if (object.ReferenceEquals(delegate518.Target, instance))
					{
						OnGetPlayFabIDsFromGameCenterIDsRequestEvent -= (PlayFabRequestEvent<GetPlayFabIDsFromGameCenterIDsRequest>)delegate518;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromGameCenterIDsResultEvent != null)
			{
				Delegate[] invocationList519 = this.OnGetPlayFabIDsFromGameCenterIDsResultEvent.GetInvocationList();
				foreach (Delegate delegate519 in invocationList519)
				{
					if (object.ReferenceEquals(delegate519.Target, instance))
					{
						OnGetPlayFabIDsFromGameCenterIDsResultEvent -= (PlayFabResultEvent<GetPlayFabIDsFromGameCenterIDsResult>)delegate519;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromGenericIDsRequestEvent != null)
			{
				Delegate[] invocationList520 = this.OnGetPlayFabIDsFromGenericIDsRequestEvent.GetInvocationList();
				foreach (Delegate delegate520 in invocationList520)
				{
					if (object.ReferenceEquals(delegate520.Target, instance))
					{
						OnGetPlayFabIDsFromGenericIDsRequestEvent -= (PlayFabRequestEvent<GetPlayFabIDsFromGenericIDsRequest>)delegate520;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromGenericIDsResultEvent != null)
			{
				Delegate[] invocationList521 = this.OnGetPlayFabIDsFromGenericIDsResultEvent.GetInvocationList();
				foreach (Delegate delegate521 in invocationList521)
				{
					if (object.ReferenceEquals(delegate521.Target, instance))
					{
						OnGetPlayFabIDsFromGenericIDsResultEvent -= (PlayFabResultEvent<GetPlayFabIDsFromGenericIDsResult>)delegate521;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromGoogleIDsRequestEvent != null)
			{
				Delegate[] invocationList522 = this.OnGetPlayFabIDsFromGoogleIDsRequestEvent.GetInvocationList();
				foreach (Delegate delegate522 in invocationList522)
				{
					if (object.ReferenceEquals(delegate522.Target, instance))
					{
						OnGetPlayFabIDsFromGoogleIDsRequestEvent -= (PlayFabRequestEvent<GetPlayFabIDsFromGoogleIDsRequest>)delegate522;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromGoogleIDsResultEvent != null)
			{
				Delegate[] invocationList523 = this.OnGetPlayFabIDsFromGoogleIDsResultEvent.GetInvocationList();
				foreach (Delegate delegate523 in invocationList523)
				{
					if (object.ReferenceEquals(delegate523.Target, instance))
					{
						OnGetPlayFabIDsFromGoogleIDsResultEvent -= (PlayFabResultEvent<GetPlayFabIDsFromGoogleIDsResult>)delegate523;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromKongregateIDsRequestEvent != null)
			{
				Delegate[] invocationList524 = this.OnGetPlayFabIDsFromKongregateIDsRequestEvent.GetInvocationList();
				foreach (Delegate delegate524 in invocationList524)
				{
					if (object.ReferenceEquals(delegate524.Target, instance))
					{
						OnGetPlayFabIDsFromKongregateIDsRequestEvent -= (PlayFabRequestEvent<GetPlayFabIDsFromKongregateIDsRequest>)delegate524;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromKongregateIDsResultEvent != null)
			{
				Delegate[] invocationList525 = this.OnGetPlayFabIDsFromKongregateIDsResultEvent.GetInvocationList();
				foreach (Delegate delegate525 in invocationList525)
				{
					if (object.ReferenceEquals(delegate525.Target, instance))
					{
						OnGetPlayFabIDsFromKongregateIDsResultEvent -= (PlayFabResultEvent<GetPlayFabIDsFromKongregateIDsResult>)delegate525;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromSteamIDsRequestEvent != null)
			{
				Delegate[] invocationList526 = this.OnGetPlayFabIDsFromSteamIDsRequestEvent.GetInvocationList();
				foreach (Delegate delegate526 in invocationList526)
				{
					if (object.ReferenceEquals(delegate526.Target, instance))
					{
						OnGetPlayFabIDsFromSteamIDsRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetPlayFabIDsFromSteamIDsRequest>)delegate526;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromSteamIDsResultEvent != null)
			{
				Delegate[] invocationList527 = this.OnGetPlayFabIDsFromSteamIDsResultEvent.GetInvocationList();
				foreach (Delegate delegate527 in invocationList527)
				{
					if (object.ReferenceEquals(delegate527.Target, instance))
					{
						OnGetPlayFabIDsFromSteamIDsResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetPlayFabIDsFromSteamIDsResult>)delegate527;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromTwitchIDsRequestEvent != null)
			{
				Delegate[] invocationList528 = this.OnGetPlayFabIDsFromTwitchIDsRequestEvent.GetInvocationList();
				foreach (Delegate delegate528 in invocationList528)
				{
					if (object.ReferenceEquals(delegate528.Target, instance))
					{
						OnGetPlayFabIDsFromTwitchIDsRequestEvent -= (PlayFabRequestEvent<GetPlayFabIDsFromTwitchIDsRequest>)delegate528;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromTwitchIDsResultEvent != null)
			{
				Delegate[] invocationList529 = this.OnGetPlayFabIDsFromTwitchIDsResultEvent.GetInvocationList();
				foreach (Delegate delegate529 in invocationList529)
				{
					if (object.ReferenceEquals(delegate529.Target, instance))
					{
						OnGetPlayFabIDsFromTwitchIDsResultEvent -= (PlayFabResultEvent<GetPlayFabIDsFromTwitchIDsResult>)delegate529;
					}
				}
			}
			if (this.OnGetPublisherDataRequestEvent != null)
			{
				Delegate[] invocationList530 = this.OnGetPublisherDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate530 in invocationList530)
				{
					if (object.ReferenceEquals(delegate530.Target, instance))
					{
						OnGetPublisherDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetPublisherDataRequest>)delegate530;
					}
				}
			}
			if (this.OnGetPublisherDataResultEvent != null)
			{
				Delegate[] invocationList531 = this.OnGetPublisherDataResultEvent.GetInvocationList();
				foreach (Delegate delegate531 in invocationList531)
				{
					if (object.ReferenceEquals(delegate531.Target, instance))
					{
						OnGetPublisherDataResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetPublisherDataResult>)delegate531;
					}
				}
			}
			if (this.OnGetPurchaseRequestEvent != null)
			{
				Delegate[] invocationList532 = this.OnGetPurchaseRequestEvent.GetInvocationList();
				foreach (Delegate delegate532 in invocationList532)
				{
					if (object.ReferenceEquals(delegate532.Target, instance))
					{
						OnGetPurchaseRequestEvent -= (PlayFabRequestEvent<GetPurchaseRequest>)delegate532;
					}
				}
			}
			if (this.OnGetPurchaseResultEvent != null)
			{
				Delegate[] invocationList533 = this.OnGetPurchaseResultEvent.GetInvocationList();
				foreach (Delegate delegate533 in invocationList533)
				{
					if (object.ReferenceEquals(delegate533.Target, instance))
					{
						OnGetPurchaseResultEvent -= (PlayFabResultEvent<GetPurchaseResult>)delegate533;
					}
				}
			}
			if (this.OnGetSharedGroupDataRequestEvent != null)
			{
				Delegate[] invocationList534 = this.OnGetSharedGroupDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate534 in invocationList534)
				{
					if (object.ReferenceEquals(delegate534.Target, instance))
					{
						OnGetSharedGroupDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetSharedGroupDataRequest>)delegate534;
					}
				}
			}
			if (this.OnGetSharedGroupDataResultEvent != null)
			{
				Delegate[] invocationList535 = this.OnGetSharedGroupDataResultEvent.GetInvocationList();
				foreach (Delegate delegate535 in invocationList535)
				{
					if (object.ReferenceEquals(delegate535.Target, instance))
					{
						OnGetSharedGroupDataResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetSharedGroupDataResult>)delegate535;
					}
				}
			}
			if (this.OnGetStoreItemsRequestEvent != null)
			{
				Delegate[] invocationList536 = this.OnGetStoreItemsRequestEvent.GetInvocationList();
				foreach (Delegate delegate536 in invocationList536)
				{
					if (object.ReferenceEquals(delegate536.Target, instance))
					{
						OnGetStoreItemsRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetStoreItemsRequest>)delegate536;
					}
				}
			}
			if (this.OnGetStoreItemsResultEvent != null)
			{
				Delegate[] invocationList537 = this.OnGetStoreItemsResultEvent.GetInvocationList();
				foreach (Delegate delegate537 in invocationList537)
				{
					if (object.ReferenceEquals(delegate537.Target, instance))
					{
						OnGetStoreItemsResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetStoreItemsResult>)delegate537;
					}
				}
			}
			if (this.OnGetTimeRequestEvent != null)
			{
				Delegate[] invocationList538 = this.OnGetTimeRequestEvent.GetInvocationList();
				foreach (Delegate delegate538 in invocationList538)
				{
					if (object.ReferenceEquals(delegate538.Target, instance))
					{
						OnGetTimeRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetTimeRequest>)delegate538;
					}
				}
			}
			if (this.OnGetTimeResultEvent != null)
			{
				Delegate[] invocationList539 = this.OnGetTimeResultEvent.GetInvocationList();
				foreach (Delegate delegate539 in invocationList539)
				{
					if (object.ReferenceEquals(delegate539.Target, instance))
					{
						OnGetTimeResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetTimeResult>)delegate539;
					}
				}
			}
			if (this.OnGetTitleDataRequestEvent != null)
			{
				Delegate[] invocationList540 = this.OnGetTitleDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate540 in invocationList540)
				{
					if (object.ReferenceEquals(delegate540.Target, instance))
					{
						OnGetTitleDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetTitleDataRequest>)delegate540;
					}
				}
			}
			if (this.OnGetTitleDataResultEvent != null)
			{
				Delegate[] invocationList541 = this.OnGetTitleDataResultEvent.GetInvocationList();
				foreach (Delegate delegate541 in invocationList541)
				{
					if (object.ReferenceEquals(delegate541.Target, instance))
					{
						OnGetTitleDataResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetTitleDataResult>)delegate541;
					}
				}
			}
			if (this.OnGetTitleNewsRequestEvent != null)
			{
				Delegate[] invocationList542 = this.OnGetTitleNewsRequestEvent.GetInvocationList();
				foreach (Delegate delegate542 in invocationList542)
				{
					if (object.ReferenceEquals(delegate542.Target, instance))
					{
						OnGetTitleNewsRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetTitleNewsRequest>)delegate542;
					}
				}
			}
			if (this.OnGetTitleNewsResultEvent != null)
			{
				Delegate[] invocationList543 = this.OnGetTitleNewsResultEvent.GetInvocationList();
				foreach (Delegate delegate543 in invocationList543)
				{
					if (object.ReferenceEquals(delegate543.Target, instance))
					{
						OnGetTitleNewsResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetTitleNewsResult>)delegate543;
					}
				}
			}
			if (this.OnGetTitlePublicKeyRequestEvent != null)
			{
				Delegate[] invocationList544 = this.OnGetTitlePublicKeyRequestEvent.GetInvocationList();
				foreach (Delegate delegate544 in invocationList544)
				{
					if (object.ReferenceEquals(delegate544.Target, instance))
					{
						OnGetTitlePublicKeyRequestEvent -= (PlayFabRequestEvent<GetTitlePublicKeyRequest>)delegate544;
					}
				}
			}
			if (this.OnGetTitlePublicKeyResultEvent != null)
			{
				Delegate[] invocationList545 = this.OnGetTitlePublicKeyResultEvent.GetInvocationList();
				foreach (Delegate delegate545 in invocationList545)
				{
					if (object.ReferenceEquals(delegate545.Target, instance))
					{
						OnGetTitlePublicKeyResultEvent -= (PlayFabResultEvent<GetTitlePublicKeyResult>)delegate545;
					}
				}
			}
			if (this.OnGetTradeStatusRequestEvent != null)
			{
				Delegate[] invocationList546 = this.OnGetTradeStatusRequestEvent.GetInvocationList();
				foreach (Delegate delegate546 in invocationList546)
				{
					if (object.ReferenceEquals(delegate546.Target, instance))
					{
						OnGetTradeStatusRequestEvent -= (PlayFabRequestEvent<GetTradeStatusRequest>)delegate546;
					}
				}
			}
			if (this.OnGetTradeStatusResultEvent != null)
			{
				Delegate[] invocationList547 = this.OnGetTradeStatusResultEvent.GetInvocationList();
				foreach (Delegate delegate547 in invocationList547)
				{
					if (object.ReferenceEquals(delegate547.Target, instance))
					{
						OnGetTradeStatusResultEvent -= (PlayFabResultEvent<GetTradeStatusResponse>)delegate547;
					}
				}
			}
			if (this.OnGetUserDataRequestEvent != null)
			{
				Delegate[] invocationList548 = this.OnGetUserDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate548 in invocationList548)
				{
					if (object.ReferenceEquals(delegate548.Target, instance))
					{
						OnGetUserDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetUserDataRequest>)delegate548;
					}
				}
			}
			if (this.OnGetUserDataResultEvent != null)
			{
				Delegate[] invocationList549 = this.OnGetUserDataResultEvent.GetInvocationList();
				foreach (Delegate delegate549 in invocationList549)
				{
					if (object.ReferenceEquals(delegate549.Target, instance))
					{
						OnGetUserDataResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetUserDataResult>)delegate549;
					}
				}
			}
			if (this.OnGetUserInventoryRequestEvent != null)
			{
				Delegate[] invocationList550 = this.OnGetUserInventoryRequestEvent.GetInvocationList();
				foreach (Delegate delegate550 in invocationList550)
				{
					if (object.ReferenceEquals(delegate550.Target, instance))
					{
						OnGetUserInventoryRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetUserInventoryRequest>)delegate550;
					}
				}
			}
			if (this.OnGetUserInventoryResultEvent != null)
			{
				Delegate[] invocationList551 = this.OnGetUserInventoryResultEvent.GetInvocationList();
				foreach (Delegate delegate551 in invocationList551)
				{
					if (object.ReferenceEquals(delegate551.Target, instance))
					{
						OnGetUserInventoryResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetUserInventoryResult>)delegate551;
					}
				}
			}
			if (this.OnGetUserPublisherDataRequestEvent != null)
			{
				Delegate[] invocationList552 = this.OnGetUserPublisherDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate552 in invocationList552)
				{
					if (object.ReferenceEquals(delegate552.Target, instance))
					{
						OnGetUserPublisherDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetUserDataRequest>)delegate552;
					}
				}
			}
			if (this.OnGetUserPublisherDataResultEvent != null)
			{
				Delegate[] invocationList553 = this.OnGetUserPublisherDataResultEvent.GetInvocationList();
				foreach (Delegate delegate553 in invocationList553)
				{
					if (object.ReferenceEquals(delegate553.Target, instance))
					{
						OnGetUserPublisherDataResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetUserDataResult>)delegate553;
					}
				}
			}
			if (this.OnGetUserPublisherReadOnlyDataRequestEvent != null)
			{
				Delegate[] invocationList554 = this.OnGetUserPublisherReadOnlyDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate554 in invocationList554)
				{
					if (object.ReferenceEquals(delegate554.Target, instance))
					{
						OnGetUserPublisherReadOnlyDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetUserDataRequest>)delegate554;
					}
				}
			}
			if (this.OnGetUserPublisherReadOnlyDataResultEvent != null)
			{
				Delegate[] invocationList555 = this.OnGetUserPublisherReadOnlyDataResultEvent.GetInvocationList();
				foreach (Delegate delegate555 in invocationList555)
				{
					if (object.ReferenceEquals(delegate555.Target, instance))
					{
						OnGetUserPublisherReadOnlyDataResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetUserDataResult>)delegate555;
					}
				}
			}
			if (this.OnGetUserReadOnlyDataRequestEvent != null)
			{
				Delegate[] invocationList556 = this.OnGetUserReadOnlyDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate556 in invocationList556)
				{
					if (object.ReferenceEquals(delegate556.Target, instance))
					{
						OnGetUserReadOnlyDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GetUserDataRequest>)delegate556;
					}
				}
			}
			if (this.OnGetUserReadOnlyDataResultEvent != null)
			{
				Delegate[] invocationList557 = this.OnGetUserReadOnlyDataResultEvent.GetInvocationList();
				foreach (Delegate delegate557 in invocationList557)
				{
					if (object.ReferenceEquals(delegate557.Target, instance))
					{
						OnGetUserReadOnlyDataResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GetUserDataResult>)delegate557;
					}
				}
			}
			if (this.OnGetWindowsHelloChallengeRequestEvent != null)
			{
				Delegate[] invocationList558 = this.OnGetWindowsHelloChallengeRequestEvent.GetInvocationList();
				foreach (Delegate delegate558 in invocationList558)
				{
					if (object.ReferenceEquals(delegate558.Target, instance))
					{
						OnGetWindowsHelloChallengeRequestEvent -= (PlayFabRequestEvent<GetWindowsHelloChallengeRequest>)delegate558;
					}
				}
			}
			if (this.OnGetWindowsHelloChallengeResultEvent != null)
			{
				Delegate[] invocationList559 = this.OnGetWindowsHelloChallengeResultEvent.GetInvocationList();
				foreach (Delegate delegate559 in invocationList559)
				{
					if (object.ReferenceEquals(delegate559.Target, instance))
					{
						OnGetWindowsHelloChallengeResultEvent -= (PlayFabResultEvent<GetWindowsHelloChallengeResponse>)delegate559;
					}
				}
			}
			if (this.OnGrantCharacterToUserRequestEvent != null)
			{
				Delegate[] invocationList560 = this.OnGrantCharacterToUserRequestEvent.GetInvocationList();
				foreach (Delegate delegate560 in invocationList560)
				{
					if (object.ReferenceEquals(delegate560.Target, instance))
					{
						OnGrantCharacterToUserRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.GrantCharacterToUserRequest>)delegate560;
					}
				}
			}
			if (this.OnGrantCharacterToUserResultEvent != null)
			{
				Delegate[] invocationList561 = this.OnGrantCharacterToUserResultEvent.GetInvocationList();
				foreach (Delegate delegate561 in invocationList561)
				{
					if (object.ReferenceEquals(delegate561.Target, instance))
					{
						OnGrantCharacterToUserResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.GrantCharacterToUserResult>)delegate561;
					}
				}
			}
			if (this.OnLinkAndroidDeviceIDRequestEvent != null)
			{
				Delegate[] invocationList562 = this.OnLinkAndroidDeviceIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate562 in invocationList562)
				{
					if (object.ReferenceEquals(delegate562.Target, instance))
					{
						OnLinkAndroidDeviceIDRequestEvent -= (PlayFabRequestEvent<LinkAndroidDeviceIDRequest>)delegate562;
					}
				}
			}
			if (this.OnLinkAndroidDeviceIDResultEvent != null)
			{
				Delegate[] invocationList563 = this.OnLinkAndroidDeviceIDResultEvent.GetInvocationList();
				foreach (Delegate delegate563 in invocationList563)
				{
					if (object.ReferenceEquals(delegate563.Target, instance))
					{
						OnLinkAndroidDeviceIDResultEvent -= (PlayFabResultEvent<LinkAndroidDeviceIDResult>)delegate563;
					}
				}
			}
			if (this.OnLinkCustomIDRequestEvent != null)
			{
				Delegate[] invocationList564 = this.OnLinkCustomIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate564 in invocationList564)
				{
					if (object.ReferenceEquals(delegate564.Target, instance))
					{
						OnLinkCustomIDRequestEvent -= (PlayFabRequestEvent<LinkCustomIDRequest>)delegate564;
					}
				}
			}
			if (this.OnLinkCustomIDResultEvent != null)
			{
				Delegate[] invocationList565 = this.OnLinkCustomIDResultEvent.GetInvocationList();
				foreach (Delegate delegate565 in invocationList565)
				{
					if (object.ReferenceEquals(delegate565.Target, instance))
					{
						OnLinkCustomIDResultEvent -= (PlayFabResultEvent<LinkCustomIDResult>)delegate565;
					}
				}
			}
			if (this.OnLinkFacebookAccountRequestEvent != null)
			{
				Delegate[] invocationList566 = this.OnLinkFacebookAccountRequestEvent.GetInvocationList();
				foreach (Delegate delegate566 in invocationList566)
				{
					if (object.ReferenceEquals(delegate566.Target, instance))
					{
						OnLinkFacebookAccountRequestEvent -= (PlayFabRequestEvent<LinkFacebookAccountRequest>)delegate566;
					}
				}
			}
			if (this.OnLinkFacebookAccountResultEvent != null)
			{
				Delegate[] invocationList567 = this.OnLinkFacebookAccountResultEvent.GetInvocationList();
				foreach (Delegate delegate567 in invocationList567)
				{
					if (object.ReferenceEquals(delegate567.Target, instance))
					{
						OnLinkFacebookAccountResultEvent -= (PlayFabResultEvent<LinkFacebookAccountResult>)delegate567;
					}
				}
			}
			if (this.OnLinkGameCenterAccountRequestEvent != null)
			{
				Delegate[] invocationList568 = this.OnLinkGameCenterAccountRequestEvent.GetInvocationList();
				foreach (Delegate delegate568 in invocationList568)
				{
					if (object.ReferenceEquals(delegate568.Target, instance))
					{
						OnLinkGameCenterAccountRequestEvent -= (PlayFabRequestEvent<LinkGameCenterAccountRequest>)delegate568;
					}
				}
			}
			if (this.OnLinkGameCenterAccountResultEvent != null)
			{
				Delegate[] invocationList569 = this.OnLinkGameCenterAccountResultEvent.GetInvocationList();
				foreach (Delegate delegate569 in invocationList569)
				{
					if (object.ReferenceEquals(delegate569.Target, instance))
					{
						OnLinkGameCenterAccountResultEvent -= (PlayFabResultEvent<LinkGameCenterAccountResult>)delegate569;
					}
				}
			}
			if (this.OnLinkGoogleAccountRequestEvent != null)
			{
				Delegate[] invocationList570 = this.OnLinkGoogleAccountRequestEvent.GetInvocationList();
				foreach (Delegate delegate570 in invocationList570)
				{
					if (object.ReferenceEquals(delegate570.Target, instance))
					{
						OnLinkGoogleAccountRequestEvent -= (PlayFabRequestEvent<LinkGoogleAccountRequest>)delegate570;
					}
				}
			}
			if (this.OnLinkGoogleAccountResultEvent != null)
			{
				Delegate[] invocationList571 = this.OnLinkGoogleAccountResultEvent.GetInvocationList();
				foreach (Delegate delegate571 in invocationList571)
				{
					if (object.ReferenceEquals(delegate571.Target, instance))
					{
						OnLinkGoogleAccountResultEvent -= (PlayFabResultEvent<LinkGoogleAccountResult>)delegate571;
					}
				}
			}
			if (this.OnLinkIOSDeviceIDRequestEvent != null)
			{
				Delegate[] invocationList572 = this.OnLinkIOSDeviceIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate572 in invocationList572)
				{
					if (object.ReferenceEquals(delegate572.Target, instance))
					{
						OnLinkIOSDeviceIDRequestEvent -= (PlayFabRequestEvent<LinkIOSDeviceIDRequest>)delegate572;
					}
				}
			}
			if (this.OnLinkIOSDeviceIDResultEvent != null)
			{
				Delegate[] invocationList573 = this.OnLinkIOSDeviceIDResultEvent.GetInvocationList();
				foreach (Delegate delegate573 in invocationList573)
				{
					if (object.ReferenceEquals(delegate573.Target, instance))
					{
						OnLinkIOSDeviceIDResultEvent -= (PlayFabResultEvent<LinkIOSDeviceIDResult>)delegate573;
					}
				}
			}
			if (this.OnLinkKongregateRequestEvent != null)
			{
				Delegate[] invocationList574 = this.OnLinkKongregateRequestEvent.GetInvocationList();
				foreach (Delegate delegate574 in invocationList574)
				{
					if (object.ReferenceEquals(delegate574.Target, instance))
					{
						OnLinkKongregateRequestEvent -= (PlayFabRequestEvent<LinkKongregateAccountRequest>)delegate574;
					}
				}
			}
			if (this.OnLinkKongregateResultEvent != null)
			{
				Delegate[] invocationList575 = this.OnLinkKongregateResultEvent.GetInvocationList();
				foreach (Delegate delegate575 in invocationList575)
				{
					if (object.ReferenceEquals(delegate575.Target, instance))
					{
						OnLinkKongregateResultEvent -= (PlayFabResultEvent<LinkKongregateAccountResult>)delegate575;
					}
				}
			}
			if (this.OnLinkSteamAccountRequestEvent != null)
			{
				Delegate[] invocationList576 = this.OnLinkSteamAccountRequestEvent.GetInvocationList();
				foreach (Delegate delegate576 in invocationList576)
				{
					if (object.ReferenceEquals(delegate576.Target, instance))
					{
						OnLinkSteamAccountRequestEvent -= (PlayFabRequestEvent<LinkSteamAccountRequest>)delegate576;
					}
				}
			}
			if (this.OnLinkSteamAccountResultEvent != null)
			{
				Delegate[] invocationList577 = this.OnLinkSteamAccountResultEvent.GetInvocationList();
				foreach (Delegate delegate577 in invocationList577)
				{
					if (object.ReferenceEquals(delegate577.Target, instance))
					{
						OnLinkSteamAccountResultEvent -= (PlayFabResultEvent<LinkSteamAccountResult>)delegate577;
					}
				}
			}
			if (this.OnLinkTwitchRequestEvent != null)
			{
				Delegate[] invocationList578 = this.OnLinkTwitchRequestEvent.GetInvocationList();
				foreach (Delegate delegate578 in invocationList578)
				{
					if (object.ReferenceEquals(delegate578.Target, instance))
					{
						OnLinkTwitchRequestEvent -= (PlayFabRequestEvent<LinkTwitchAccountRequest>)delegate578;
					}
				}
			}
			if (this.OnLinkTwitchResultEvent != null)
			{
				Delegate[] invocationList579 = this.OnLinkTwitchResultEvent.GetInvocationList();
				foreach (Delegate delegate579 in invocationList579)
				{
					if (object.ReferenceEquals(delegate579.Target, instance))
					{
						OnLinkTwitchResultEvent -= (PlayFabResultEvent<LinkTwitchAccountResult>)delegate579;
					}
				}
			}
			if (this.OnLinkWindowsHelloRequestEvent != null)
			{
				Delegate[] invocationList580 = this.OnLinkWindowsHelloRequestEvent.GetInvocationList();
				foreach (Delegate delegate580 in invocationList580)
				{
					if (object.ReferenceEquals(delegate580.Target, instance))
					{
						OnLinkWindowsHelloRequestEvent -= (PlayFabRequestEvent<LinkWindowsHelloAccountRequest>)delegate580;
					}
				}
			}
			if (this.OnLinkWindowsHelloResultEvent != null)
			{
				Delegate[] invocationList581 = this.OnLinkWindowsHelloResultEvent.GetInvocationList();
				foreach (Delegate delegate581 in invocationList581)
				{
					if (object.ReferenceEquals(delegate581.Target, instance))
					{
						OnLinkWindowsHelloResultEvent -= (PlayFabResultEvent<LinkWindowsHelloAccountResponse>)delegate581;
					}
				}
			}
			if (this.OnLoginWithAndroidDeviceIDRequestEvent != null)
			{
				Delegate[] invocationList582 = this.OnLoginWithAndroidDeviceIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate582 in invocationList582)
				{
					if (object.ReferenceEquals(delegate582.Target, instance))
					{
						OnLoginWithAndroidDeviceIDRequestEvent -= (PlayFabRequestEvent<LoginWithAndroidDeviceIDRequest>)delegate582;
					}
				}
			}
			if (this.OnLoginWithCustomIDRequestEvent != null)
			{
				Delegate[] invocationList583 = this.OnLoginWithCustomIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate583 in invocationList583)
				{
					if (object.ReferenceEquals(delegate583.Target, instance))
					{
						OnLoginWithCustomIDRequestEvent -= (PlayFabRequestEvent<LoginWithCustomIDRequest>)delegate583;
					}
				}
			}
			if (this.OnLoginWithEmailAddressRequestEvent != null)
			{
				Delegate[] invocationList584 = this.OnLoginWithEmailAddressRequestEvent.GetInvocationList();
				foreach (Delegate delegate584 in invocationList584)
				{
					if (object.ReferenceEquals(delegate584.Target, instance))
					{
						OnLoginWithEmailAddressRequestEvent -= (PlayFabRequestEvent<LoginWithEmailAddressRequest>)delegate584;
					}
				}
			}
			if (this.OnLoginWithFacebookRequestEvent != null)
			{
				Delegate[] invocationList585 = this.OnLoginWithFacebookRequestEvent.GetInvocationList();
				foreach (Delegate delegate585 in invocationList585)
				{
					if (object.ReferenceEquals(delegate585.Target, instance))
					{
						OnLoginWithFacebookRequestEvent -= (PlayFabRequestEvent<LoginWithFacebookRequest>)delegate585;
					}
				}
			}
			if (this.OnLoginWithGameCenterRequestEvent != null)
			{
				Delegate[] invocationList586 = this.OnLoginWithGameCenterRequestEvent.GetInvocationList();
				foreach (Delegate delegate586 in invocationList586)
				{
					if (object.ReferenceEquals(delegate586.Target, instance))
					{
						OnLoginWithGameCenterRequestEvent -= (PlayFabRequestEvent<LoginWithGameCenterRequest>)delegate586;
					}
				}
			}
			if (this.OnLoginWithGoogleAccountRequestEvent != null)
			{
				Delegate[] invocationList587 = this.OnLoginWithGoogleAccountRequestEvent.GetInvocationList();
				foreach (Delegate delegate587 in invocationList587)
				{
					if (object.ReferenceEquals(delegate587.Target, instance))
					{
						OnLoginWithGoogleAccountRequestEvent -= (PlayFabRequestEvent<LoginWithGoogleAccountRequest>)delegate587;
					}
				}
			}
			if (this.OnLoginWithIOSDeviceIDRequestEvent != null)
			{
				Delegate[] invocationList588 = this.OnLoginWithIOSDeviceIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate588 in invocationList588)
				{
					if (object.ReferenceEquals(delegate588.Target, instance))
					{
						OnLoginWithIOSDeviceIDRequestEvent -= (PlayFabRequestEvent<LoginWithIOSDeviceIDRequest>)delegate588;
					}
				}
			}
			if (this.OnLoginWithKongregateRequestEvent != null)
			{
				Delegate[] invocationList589 = this.OnLoginWithKongregateRequestEvent.GetInvocationList();
				foreach (Delegate delegate589 in invocationList589)
				{
					if (object.ReferenceEquals(delegate589.Target, instance))
					{
						OnLoginWithKongregateRequestEvent -= (PlayFabRequestEvent<LoginWithKongregateRequest>)delegate589;
					}
				}
			}
			if (this.OnLoginWithPlayFabRequestEvent != null)
			{
				Delegate[] invocationList590 = this.OnLoginWithPlayFabRequestEvent.GetInvocationList();
				foreach (Delegate delegate590 in invocationList590)
				{
					if (object.ReferenceEquals(delegate590.Target, instance))
					{
						OnLoginWithPlayFabRequestEvent -= (PlayFabRequestEvent<LoginWithPlayFabRequest>)delegate590;
					}
				}
			}
			if (this.OnLoginWithSteamRequestEvent != null)
			{
				Delegate[] invocationList591 = this.OnLoginWithSteamRequestEvent.GetInvocationList();
				foreach (Delegate delegate591 in invocationList591)
				{
					if (object.ReferenceEquals(delegate591.Target, instance))
					{
						OnLoginWithSteamRequestEvent -= (PlayFabRequestEvent<LoginWithSteamRequest>)delegate591;
					}
				}
			}
			if (this.OnLoginWithTwitchRequestEvent != null)
			{
				Delegate[] invocationList592 = this.OnLoginWithTwitchRequestEvent.GetInvocationList();
				foreach (Delegate delegate592 in invocationList592)
				{
					if (object.ReferenceEquals(delegate592.Target, instance))
					{
						OnLoginWithTwitchRequestEvent -= (PlayFabRequestEvent<LoginWithTwitchRequest>)delegate592;
					}
				}
			}
			if (this.OnLoginWithWindowsHelloRequestEvent != null)
			{
				Delegate[] invocationList593 = this.OnLoginWithWindowsHelloRequestEvent.GetInvocationList();
				foreach (Delegate delegate593 in invocationList593)
				{
					if (object.ReferenceEquals(delegate593.Target, instance))
					{
						OnLoginWithWindowsHelloRequestEvent -= (PlayFabRequestEvent<LoginWithWindowsHelloRequest>)delegate593;
					}
				}
			}
			if (this.OnMatchmakeRequestEvent != null)
			{
				Delegate[] invocationList594 = this.OnMatchmakeRequestEvent.GetInvocationList();
				foreach (Delegate delegate594 in invocationList594)
				{
					if (object.ReferenceEquals(delegate594.Target, instance))
					{
						OnMatchmakeRequestEvent -= (PlayFabRequestEvent<MatchmakeRequest>)delegate594;
					}
				}
			}
			if (this.OnMatchmakeResultEvent != null)
			{
				Delegate[] invocationList595 = this.OnMatchmakeResultEvent.GetInvocationList();
				foreach (Delegate delegate595 in invocationList595)
				{
					if (object.ReferenceEquals(delegate595.Target, instance))
					{
						OnMatchmakeResultEvent -= (PlayFabResultEvent<MatchmakeResult>)delegate595;
					}
				}
			}
			if (this.OnOpenTradeRequestEvent != null)
			{
				Delegate[] invocationList596 = this.OnOpenTradeRequestEvent.GetInvocationList();
				foreach (Delegate delegate596 in invocationList596)
				{
					if (object.ReferenceEquals(delegate596.Target, instance))
					{
						OnOpenTradeRequestEvent -= (PlayFabRequestEvent<OpenTradeRequest>)delegate596;
					}
				}
			}
			if (this.OnOpenTradeResultEvent != null)
			{
				Delegate[] invocationList597 = this.OnOpenTradeResultEvent.GetInvocationList();
				foreach (Delegate delegate597 in invocationList597)
				{
					if (object.ReferenceEquals(delegate597.Target, instance))
					{
						OnOpenTradeResultEvent -= (PlayFabResultEvent<OpenTradeResponse>)delegate597;
					}
				}
			}
			if (this.OnPayForPurchaseRequestEvent != null)
			{
				Delegate[] invocationList598 = this.OnPayForPurchaseRequestEvent.GetInvocationList();
				foreach (Delegate delegate598 in invocationList598)
				{
					if (object.ReferenceEquals(delegate598.Target, instance))
					{
						OnPayForPurchaseRequestEvent -= (PlayFabRequestEvent<PayForPurchaseRequest>)delegate598;
					}
				}
			}
			if (this.OnPayForPurchaseResultEvent != null)
			{
				Delegate[] invocationList599 = this.OnPayForPurchaseResultEvent.GetInvocationList();
				foreach (Delegate delegate599 in invocationList599)
				{
					if (object.ReferenceEquals(delegate599.Target, instance))
					{
						OnPayForPurchaseResultEvent -= (PlayFabResultEvent<PayForPurchaseResult>)delegate599;
					}
				}
			}
			if (this.OnPurchaseItemRequestEvent != null)
			{
				Delegate[] invocationList600 = this.OnPurchaseItemRequestEvent.GetInvocationList();
				foreach (Delegate delegate600 in invocationList600)
				{
					if (object.ReferenceEquals(delegate600.Target, instance))
					{
						OnPurchaseItemRequestEvent -= (PlayFabRequestEvent<PurchaseItemRequest>)delegate600;
					}
				}
			}
			if (this.OnPurchaseItemResultEvent != null)
			{
				Delegate[] invocationList601 = this.OnPurchaseItemResultEvent.GetInvocationList();
				foreach (Delegate delegate601 in invocationList601)
				{
					if (object.ReferenceEquals(delegate601.Target, instance))
					{
						OnPurchaseItemResultEvent -= (PlayFabResultEvent<PurchaseItemResult>)delegate601;
					}
				}
			}
			if (this.OnRedeemCouponRequestEvent != null)
			{
				Delegate[] invocationList602 = this.OnRedeemCouponRequestEvent.GetInvocationList();
				foreach (Delegate delegate602 in invocationList602)
				{
					if (object.ReferenceEquals(delegate602.Target, instance))
					{
						OnRedeemCouponRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.RedeemCouponRequest>)delegate602;
					}
				}
			}
			if (this.OnRedeemCouponResultEvent != null)
			{
				Delegate[] invocationList603 = this.OnRedeemCouponResultEvent.GetInvocationList();
				foreach (Delegate delegate603 in invocationList603)
				{
					if (object.ReferenceEquals(delegate603.Target, instance))
					{
						OnRedeemCouponResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.RedeemCouponResult>)delegate603;
					}
				}
			}
			if (this.OnRegisterForIOSPushNotificationRequestEvent != null)
			{
				Delegate[] invocationList604 = this.OnRegisterForIOSPushNotificationRequestEvent.GetInvocationList();
				foreach (Delegate delegate604 in invocationList604)
				{
					if (object.ReferenceEquals(delegate604.Target, instance))
					{
						OnRegisterForIOSPushNotificationRequestEvent -= (PlayFabRequestEvent<RegisterForIOSPushNotificationRequest>)delegate604;
					}
				}
			}
			if (this.OnRegisterForIOSPushNotificationResultEvent != null)
			{
				Delegate[] invocationList605 = this.OnRegisterForIOSPushNotificationResultEvent.GetInvocationList();
				foreach (Delegate delegate605 in invocationList605)
				{
					if (object.ReferenceEquals(delegate605.Target, instance))
					{
						OnRegisterForIOSPushNotificationResultEvent -= (PlayFabResultEvent<RegisterForIOSPushNotificationResult>)delegate605;
					}
				}
			}
			if (this.OnRegisterPlayFabUserRequestEvent != null)
			{
				Delegate[] invocationList606 = this.OnRegisterPlayFabUserRequestEvent.GetInvocationList();
				foreach (Delegate delegate606 in invocationList606)
				{
					if (object.ReferenceEquals(delegate606.Target, instance))
					{
						OnRegisterPlayFabUserRequestEvent -= (PlayFabRequestEvent<RegisterPlayFabUserRequest>)delegate606;
					}
				}
			}
			if (this.OnRegisterPlayFabUserResultEvent != null)
			{
				Delegate[] invocationList607 = this.OnRegisterPlayFabUserResultEvent.GetInvocationList();
				foreach (Delegate delegate607 in invocationList607)
				{
					if (object.ReferenceEquals(delegate607.Target, instance))
					{
						OnRegisterPlayFabUserResultEvent -= (PlayFabResultEvent<RegisterPlayFabUserResult>)delegate607;
					}
				}
			}
			if (this.OnRegisterWithWindowsHelloRequestEvent != null)
			{
				Delegate[] invocationList608 = this.OnRegisterWithWindowsHelloRequestEvent.GetInvocationList();
				foreach (Delegate delegate608 in invocationList608)
				{
					if (object.ReferenceEquals(delegate608.Target, instance))
					{
						OnRegisterWithWindowsHelloRequestEvent -= (PlayFabRequestEvent<RegisterWithWindowsHelloRequest>)delegate608;
					}
				}
			}
			if (this.OnRemoveContactEmailRequestEvent != null)
			{
				Delegate[] invocationList609 = this.OnRemoveContactEmailRequestEvent.GetInvocationList();
				foreach (Delegate delegate609 in invocationList609)
				{
					if (object.ReferenceEquals(delegate609.Target, instance))
					{
						OnRemoveContactEmailRequestEvent -= (PlayFabRequestEvent<RemoveContactEmailRequest>)delegate609;
					}
				}
			}
			if (this.OnRemoveContactEmailResultEvent != null)
			{
				Delegate[] invocationList610 = this.OnRemoveContactEmailResultEvent.GetInvocationList();
				foreach (Delegate delegate610 in invocationList610)
				{
					if (object.ReferenceEquals(delegate610.Target, instance))
					{
						OnRemoveContactEmailResultEvent -= (PlayFabResultEvent<RemoveContactEmailResult>)delegate610;
					}
				}
			}
			if (this.OnRemoveFriendRequestEvent != null)
			{
				Delegate[] invocationList611 = this.OnRemoveFriendRequestEvent.GetInvocationList();
				foreach (Delegate delegate611 in invocationList611)
				{
					if (object.ReferenceEquals(delegate611.Target, instance))
					{
						OnRemoveFriendRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.RemoveFriendRequest>)delegate611;
					}
				}
			}
			if (this.OnRemoveFriendResultEvent != null)
			{
				Delegate[] invocationList612 = this.OnRemoveFriendResultEvent.GetInvocationList();
				foreach (Delegate delegate612 in invocationList612)
				{
					if (object.ReferenceEquals(delegate612.Target, instance))
					{
						OnRemoveFriendResultEvent -= (PlayFabResultEvent<RemoveFriendResult>)delegate612;
					}
				}
			}
			if (this.OnRemoveGenericIDRequestEvent != null)
			{
				Delegate[] invocationList613 = this.OnRemoveGenericIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate613 in invocationList613)
				{
					if (object.ReferenceEquals(delegate613.Target, instance))
					{
						OnRemoveGenericIDRequestEvent -= (PlayFabRequestEvent<RemoveGenericIDRequest>)delegate613;
					}
				}
			}
			if (this.OnRemoveGenericIDResultEvent != null)
			{
				Delegate[] invocationList614 = this.OnRemoveGenericIDResultEvent.GetInvocationList();
				foreach (Delegate delegate614 in invocationList614)
				{
					if (object.ReferenceEquals(delegate614.Target, instance))
					{
						OnRemoveGenericIDResultEvent -= (PlayFabResultEvent<RemoveGenericIDResult>)delegate614;
					}
				}
			}
			if (this.OnRemoveSharedGroupMembersRequestEvent != null)
			{
				Delegate[] invocationList615 = this.OnRemoveSharedGroupMembersRequestEvent.GetInvocationList();
				foreach (Delegate delegate615 in invocationList615)
				{
					if (object.ReferenceEquals(delegate615.Target, instance))
					{
						OnRemoveSharedGroupMembersRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.RemoveSharedGroupMembersRequest>)delegate615;
					}
				}
			}
			if (this.OnRemoveSharedGroupMembersResultEvent != null)
			{
				Delegate[] invocationList616 = this.OnRemoveSharedGroupMembersResultEvent.GetInvocationList();
				foreach (Delegate delegate616 in invocationList616)
				{
					if (object.ReferenceEquals(delegate616.Target, instance))
					{
						OnRemoveSharedGroupMembersResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.RemoveSharedGroupMembersResult>)delegate616;
					}
				}
			}
			if (this.OnReportDeviceInfoRequestEvent != null)
			{
				Delegate[] invocationList617 = this.OnReportDeviceInfoRequestEvent.GetInvocationList();
				foreach (Delegate delegate617 in invocationList617)
				{
					if (object.ReferenceEquals(delegate617.Target, instance))
					{
						OnReportDeviceInfoRequestEvent -= (PlayFabRequestEvent<DeviceInfoRequest>)delegate617;
					}
				}
			}
			if (this.OnReportDeviceInfoResultEvent != null)
			{
				Delegate[] invocationList618 = this.OnReportDeviceInfoResultEvent.GetInvocationList();
				foreach (Delegate delegate618 in invocationList618)
				{
					if (object.ReferenceEquals(delegate618.Target, instance))
					{
						OnReportDeviceInfoResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.EmptyResult>)delegate618;
					}
				}
			}
			if (this.OnReportPlayerRequestEvent != null)
			{
				Delegate[] invocationList619 = this.OnReportPlayerRequestEvent.GetInvocationList();
				foreach (Delegate delegate619 in invocationList619)
				{
					if (object.ReferenceEquals(delegate619.Target, instance))
					{
						OnReportPlayerRequestEvent -= (PlayFabRequestEvent<ReportPlayerClientRequest>)delegate619;
					}
				}
			}
			if (this.OnReportPlayerResultEvent != null)
			{
				Delegate[] invocationList620 = this.OnReportPlayerResultEvent.GetInvocationList();
				foreach (Delegate delegate620 in invocationList620)
				{
					if (object.ReferenceEquals(delegate620.Target, instance))
					{
						OnReportPlayerResultEvent -= (PlayFabResultEvent<ReportPlayerClientResult>)delegate620;
					}
				}
			}
			if (this.OnRestoreIOSPurchasesRequestEvent != null)
			{
				Delegate[] invocationList621 = this.OnRestoreIOSPurchasesRequestEvent.GetInvocationList();
				foreach (Delegate delegate621 in invocationList621)
				{
					if (object.ReferenceEquals(delegate621.Target, instance))
					{
						OnRestoreIOSPurchasesRequestEvent -= (PlayFabRequestEvent<RestoreIOSPurchasesRequest>)delegate621;
					}
				}
			}
			if (this.OnRestoreIOSPurchasesResultEvent != null)
			{
				Delegate[] invocationList622 = this.OnRestoreIOSPurchasesResultEvent.GetInvocationList();
				foreach (Delegate delegate622 in invocationList622)
				{
					if (object.ReferenceEquals(delegate622.Target, instance))
					{
						OnRestoreIOSPurchasesResultEvent -= (PlayFabResultEvent<RestoreIOSPurchasesResult>)delegate622;
					}
				}
			}
			if (this.OnSendAccountRecoveryEmailRequestEvent != null)
			{
				Delegate[] invocationList623 = this.OnSendAccountRecoveryEmailRequestEvent.GetInvocationList();
				foreach (Delegate delegate623 in invocationList623)
				{
					if (object.ReferenceEquals(delegate623.Target, instance))
					{
						OnSendAccountRecoveryEmailRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.SendAccountRecoveryEmailRequest>)delegate623;
					}
				}
			}
			if (this.OnSendAccountRecoveryEmailResultEvent != null)
			{
				Delegate[] invocationList624 = this.OnSendAccountRecoveryEmailResultEvent.GetInvocationList();
				foreach (Delegate delegate624 in invocationList624)
				{
					if (object.ReferenceEquals(delegate624.Target, instance))
					{
						OnSendAccountRecoveryEmailResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.SendAccountRecoveryEmailResult>)delegate624;
					}
				}
			}
			if (this.OnSetFriendTagsRequestEvent != null)
			{
				Delegate[] invocationList625 = this.OnSetFriendTagsRequestEvent.GetInvocationList();
				foreach (Delegate delegate625 in invocationList625)
				{
					if (object.ReferenceEquals(delegate625.Target, instance))
					{
						OnSetFriendTagsRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.SetFriendTagsRequest>)delegate625;
					}
				}
			}
			if (this.OnSetFriendTagsResultEvent != null)
			{
				Delegate[] invocationList626 = this.OnSetFriendTagsResultEvent.GetInvocationList();
				foreach (Delegate delegate626 in invocationList626)
				{
					if (object.ReferenceEquals(delegate626.Target, instance))
					{
						OnSetFriendTagsResultEvent -= (PlayFabResultEvent<SetFriendTagsResult>)delegate626;
					}
				}
			}
			if (this.OnSetPlayerSecretRequestEvent != null)
			{
				Delegate[] invocationList627 = this.OnSetPlayerSecretRequestEvent.GetInvocationList();
				foreach (Delegate delegate627 in invocationList627)
				{
					if (object.ReferenceEquals(delegate627.Target, instance))
					{
						OnSetPlayerSecretRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.SetPlayerSecretRequest>)delegate627;
					}
				}
			}
			if (this.OnSetPlayerSecretResultEvent != null)
			{
				Delegate[] invocationList628 = this.OnSetPlayerSecretResultEvent.GetInvocationList();
				foreach (Delegate delegate628 in invocationList628)
				{
					if (object.ReferenceEquals(delegate628.Target, instance))
					{
						OnSetPlayerSecretResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.SetPlayerSecretResult>)delegate628;
					}
				}
			}
			if (this.OnStartGameRequestEvent != null)
			{
				Delegate[] invocationList629 = this.OnStartGameRequestEvent.GetInvocationList();
				foreach (Delegate delegate629 in invocationList629)
				{
					if (object.ReferenceEquals(delegate629.Target, instance))
					{
						OnStartGameRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.StartGameRequest>)delegate629;
					}
				}
			}
			if (this.OnStartGameResultEvent != null)
			{
				Delegate[] invocationList630 = this.OnStartGameResultEvent.GetInvocationList();
				foreach (Delegate delegate630 in invocationList630)
				{
					if (object.ReferenceEquals(delegate630.Target, instance))
					{
						OnStartGameResultEvent -= (PlayFabResultEvent<StartGameResult>)delegate630;
					}
				}
			}
			if (this.OnStartPurchaseRequestEvent != null)
			{
				Delegate[] invocationList631 = this.OnStartPurchaseRequestEvent.GetInvocationList();
				foreach (Delegate delegate631 in invocationList631)
				{
					if (object.ReferenceEquals(delegate631.Target, instance))
					{
						OnStartPurchaseRequestEvent -= (PlayFabRequestEvent<StartPurchaseRequest>)delegate631;
					}
				}
			}
			if (this.OnStartPurchaseResultEvent != null)
			{
				Delegate[] invocationList632 = this.OnStartPurchaseResultEvent.GetInvocationList();
				foreach (Delegate delegate632 in invocationList632)
				{
					if (object.ReferenceEquals(delegate632.Target, instance))
					{
						OnStartPurchaseResultEvent -= (PlayFabResultEvent<StartPurchaseResult>)delegate632;
					}
				}
			}
			if (this.OnSubtractUserVirtualCurrencyRequestEvent != null)
			{
				Delegate[] invocationList633 = this.OnSubtractUserVirtualCurrencyRequestEvent.GetInvocationList();
				foreach (Delegate delegate633 in invocationList633)
				{
					if (object.ReferenceEquals(delegate633.Target, instance))
					{
						OnSubtractUserVirtualCurrencyRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.SubtractUserVirtualCurrencyRequest>)delegate633;
					}
				}
			}
			if (this.OnSubtractUserVirtualCurrencyResultEvent != null)
			{
				Delegate[] invocationList634 = this.OnSubtractUserVirtualCurrencyResultEvent.GetInvocationList();
				foreach (Delegate delegate634 in invocationList634)
				{
					if (object.ReferenceEquals(delegate634.Target, instance))
					{
						OnSubtractUserVirtualCurrencyResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.ModifyUserVirtualCurrencyResult>)delegate634;
					}
				}
			}
			if (this.OnUnlinkAndroidDeviceIDRequestEvent != null)
			{
				Delegate[] invocationList635 = this.OnUnlinkAndroidDeviceIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate635 in invocationList635)
				{
					if (object.ReferenceEquals(delegate635.Target, instance))
					{
						OnUnlinkAndroidDeviceIDRequestEvent -= (PlayFabRequestEvent<UnlinkAndroidDeviceIDRequest>)delegate635;
					}
				}
			}
			if (this.OnUnlinkAndroidDeviceIDResultEvent != null)
			{
				Delegate[] invocationList636 = this.OnUnlinkAndroidDeviceIDResultEvent.GetInvocationList();
				foreach (Delegate delegate636 in invocationList636)
				{
					if (object.ReferenceEquals(delegate636.Target, instance))
					{
						OnUnlinkAndroidDeviceIDResultEvent -= (PlayFabResultEvent<UnlinkAndroidDeviceIDResult>)delegate636;
					}
				}
			}
			if (this.OnUnlinkCustomIDRequestEvent != null)
			{
				Delegate[] invocationList637 = this.OnUnlinkCustomIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate637 in invocationList637)
				{
					if (object.ReferenceEquals(delegate637.Target, instance))
					{
						OnUnlinkCustomIDRequestEvent -= (PlayFabRequestEvent<UnlinkCustomIDRequest>)delegate637;
					}
				}
			}
			if (this.OnUnlinkCustomIDResultEvent != null)
			{
				Delegate[] invocationList638 = this.OnUnlinkCustomIDResultEvent.GetInvocationList();
				foreach (Delegate delegate638 in invocationList638)
				{
					if (object.ReferenceEquals(delegate638.Target, instance))
					{
						OnUnlinkCustomIDResultEvent -= (PlayFabResultEvent<UnlinkCustomIDResult>)delegate638;
					}
				}
			}
			if (this.OnUnlinkFacebookAccountRequestEvent != null)
			{
				Delegate[] invocationList639 = this.OnUnlinkFacebookAccountRequestEvent.GetInvocationList();
				foreach (Delegate delegate639 in invocationList639)
				{
					if (object.ReferenceEquals(delegate639.Target, instance))
					{
						OnUnlinkFacebookAccountRequestEvent -= (PlayFabRequestEvent<UnlinkFacebookAccountRequest>)delegate639;
					}
				}
			}
			if (this.OnUnlinkFacebookAccountResultEvent != null)
			{
				Delegate[] invocationList640 = this.OnUnlinkFacebookAccountResultEvent.GetInvocationList();
				foreach (Delegate delegate640 in invocationList640)
				{
					if (object.ReferenceEquals(delegate640.Target, instance))
					{
						OnUnlinkFacebookAccountResultEvent -= (PlayFabResultEvent<UnlinkFacebookAccountResult>)delegate640;
					}
				}
			}
			if (this.OnUnlinkGameCenterAccountRequestEvent != null)
			{
				Delegate[] invocationList641 = this.OnUnlinkGameCenterAccountRequestEvent.GetInvocationList();
				foreach (Delegate delegate641 in invocationList641)
				{
					if (object.ReferenceEquals(delegate641.Target, instance))
					{
						OnUnlinkGameCenterAccountRequestEvent -= (PlayFabRequestEvent<UnlinkGameCenterAccountRequest>)delegate641;
					}
				}
			}
			if (this.OnUnlinkGameCenterAccountResultEvent != null)
			{
				Delegate[] invocationList642 = this.OnUnlinkGameCenterAccountResultEvent.GetInvocationList();
				foreach (Delegate delegate642 in invocationList642)
				{
					if (object.ReferenceEquals(delegate642.Target, instance))
					{
						OnUnlinkGameCenterAccountResultEvent -= (PlayFabResultEvent<UnlinkGameCenterAccountResult>)delegate642;
					}
				}
			}
			if (this.OnUnlinkGoogleAccountRequestEvent != null)
			{
				Delegate[] invocationList643 = this.OnUnlinkGoogleAccountRequestEvent.GetInvocationList();
				foreach (Delegate delegate643 in invocationList643)
				{
					if (object.ReferenceEquals(delegate643.Target, instance))
					{
						OnUnlinkGoogleAccountRequestEvent -= (PlayFabRequestEvent<UnlinkGoogleAccountRequest>)delegate643;
					}
				}
			}
			if (this.OnUnlinkGoogleAccountResultEvent != null)
			{
				Delegate[] invocationList644 = this.OnUnlinkGoogleAccountResultEvent.GetInvocationList();
				foreach (Delegate delegate644 in invocationList644)
				{
					if (object.ReferenceEquals(delegate644.Target, instance))
					{
						OnUnlinkGoogleAccountResultEvent -= (PlayFabResultEvent<UnlinkGoogleAccountResult>)delegate644;
					}
				}
			}
			if (this.OnUnlinkIOSDeviceIDRequestEvent != null)
			{
				Delegate[] invocationList645 = this.OnUnlinkIOSDeviceIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate645 in invocationList645)
				{
					if (object.ReferenceEquals(delegate645.Target, instance))
					{
						OnUnlinkIOSDeviceIDRequestEvent -= (PlayFabRequestEvent<UnlinkIOSDeviceIDRequest>)delegate645;
					}
				}
			}
			if (this.OnUnlinkIOSDeviceIDResultEvent != null)
			{
				Delegate[] invocationList646 = this.OnUnlinkIOSDeviceIDResultEvent.GetInvocationList();
				foreach (Delegate delegate646 in invocationList646)
				{
					if (object.ReferenceEquals(delegate646.Target, instance))
					{
						OnUnlinkIOSDeviceIDResultEvent -= (PlayFabResultEvent<UnlinkIOSDeviceIDResult>)delegate646;
					}
				}
			}
			if (this.OnUnlinkKongregateRequestEvent != null)
			{
				Delegate[] invocationList647 = this.OnUnlinkKongregateRequestEvent.GetInvocationList();
				foreach (Delegate delegate647 in invocationList647)
				{
					if (object.ReferenceEquals(delegate647.Target, instance))
					{
						OnUnlinkKongregateRequestEvent -= (PlayFabRequestEvent<UnlinkKongregateAccountRequest>)delegate647;
					}
				}
			}
			if (this.OnUnlinkKongregateResultEvent != null)
			{
				Delegate[] invocationList648 = this.OnUnlinkKongregateResultEvent.GetInvocationList();
				foreach (Delegate delegate648 in invocationList648)
				{
					if (object.ReferenceEquals(delegate648.Target, instance))
					{
						OnUnlinkKongregateResultEvent -= (PlayFabResultEvent<UnlinkKongregateAccountResult>)delegate648;
					}
				}
			}
			if (this.OnUnlinkSteamAccountRequestEvent != null)
			{
				Delegate[] invocationList649 = this.OnUnlinkSteamAccountRequestEvent.GetInvocationList();
				foreach (Delegate delegate649 in invocationList649)
				{
					if (object.ReferenceEquals(delegate649.Target, instance))
					{
						OnUnlinkSteamAccountRequestEvent -= (PlayFabRequestEvent<UnlinkSteamAccountRequest>)delegate649;
					}
				}
			}
			if (this.OnUnlinkSteamAccountResultEvent != null)
			{
				Delegate[] invocationList650 = this.OnUnlinkSteamAccountResultEvent.GetInvocationList();
				foreach (Delegate delegate650 in invocationList650)
				{
					if (object.ReferenceEquals(delegate650.Target, instance))
					{
						OnUnlinkSteamAccountResultEvent -= (PlayFabResultEvent<UnlinkSteamAccountResult>)delegate650;
					}
				}
			}
			if (this.OnUnlinkTwitchRequestEvent != null)
			{
				Delegate[] invocationList651 = this.OnUnlinkTwitchRequestEvent.GetInvocationList();
				foreach (Delegate delegate651 in invocationList651)
				{
					if (object.ReferenceEquals(delegate651.Target, instance))
					{
						OnUnlinkTwitchRequestEvent -= (PlayFabRequestEvent<UnlinkTwitchAccountRequest>)delegate651;
					}
				}
			}
			if (this.OnUnlinkTwitchResultEvent != null)
			{
				Delegate[] invocationList652 = this.OnUnlinkTwitchResultEvent.GetInvocationList();
				foreach (Delegate delegate652 in invocationList652)
				{
					if (object.ReferenceEquals(delegate652.Target, instance))
					{
						OnUnlinkTwitchResultEvent -= (PlayFabResultEvent<UnlinkTwitchAccountResult>)delegate652;
					}
				}
			}
			if (this.OnUnlinkWindowsHelloRequestEvent != null)
			{
				Delegate[] invocationList653 = this.OnUnlinkWindowsHelloRequestEvent.GetInvocationList();
				foreach (Delegate delegate653 in invocationList653)
				{
					if (object.ReferenceEquals(delegate653.Target, instance))
					{
						OnUnlinkWindowsHelloRequestEvent -= (PlayFabRequestEvent<UnlinkWindowsHelloAccountRequest>)delegate653;
					}
				}
			}
			if (this.OnUnlinkWindowsHelloResultEvent != null)
			{
				Delegate[] invocationList654 = this.OnUnlinkWindowsHelloResultEvent.GetInvocationList();
				foreach (Delegate delegate654 in invocationList654)
				{
					if (object.ReferenceEquals(delegate654.Target, instance))
					{
						OnUnlinkWindowsHelloResultEvent -= (PlayFabResultEvent<UnlinkWindowsHelloAccountResponse>)delegate654;
					}
				}
			}
			if (this.OnUnlockContainerInstanceRequestEvent != null)
			{
				Delegate[] invocationList655 = this.OnUnlockContainerInstanceRequestEvent.GetInvocationList();
				foreach (Delegate delegate655 in invocationList655)
				{
					if (object.ReferenceEquals(delegate655.Target, instance))
					{
						OnUnlockContainerInstanceRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.UnlockContainerInstanceRequest>)delegate655;
					}
				}
			}
			if (this.OnUnlockContainerInstanceResultEvent != null)
			{
				Delegate[] invocationList656 = this.OnUnlockContainerInstanceResultEvent.GetInvocationList();
				foreach (Delegate delegate656 in invocationList656)
				{
					if (object.ReferenceEquals(delegate656.Target, instance))
					{
						OnUnlockContainerInstanceResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.UnlockContainerItemResult>)delegate656;
					}
				}
			}
			if (this.OnUnlockContainerItemRequestEvent != null)
			{
				Delegate[] invocationList657 = this.OnUnlockContainerItemRequestEvent.GetInvocationList();
				foreach (Delegate delegate657 in invocationList657)
				{
					if (object.ReferenceEquals(delegate657.Target, instance))
					{
						OnUnlockContainerItemRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.UnlockContainerItemRequest>)delegate657;
					}
				}
			}
			if (this.OnUnlockContainerItemResultEvent != null)
			{
				Delegate[] invocationList658 = this.OnUnlockContainerItemResultEvent.GetInvocationList();
				foreach (Delegate delegate658 in invocationList658)
				{
					if (object.ReferenceEquals(delegate658.Target, instance))
					{
						OnUnlockContainerItemResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.UnlockContainerItemResult>)delegate658;
					}
				}
			}
			if (this.OnUpdateAvatarUrlRequestEvent != null)
			{
				Delegate[] invocationList659 = this.OnUpdateAvatarUrlRequestEvent.GetInvocationList();
				foreach (Delegate delegate659 in invocationList659)
				{
					if (object.ReferenceEquals(delegate659.Target, instance))
					{
						OnUpdateAvatarUrlRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.UpdateAvatarUrlRequest>)delegate659;
					}
				}
			}
			if (this.OnUpdateAvatarUrlResultEvent != null)
			{
				Delegate[] invocationList660 = this.OnUpdateAvatarUrlResultEvent.GetInvocationList();
				foreach (Delegate delegate660 in invocationList660)
				{
					if (object.ReferenceEquals(delegate660.Target, instance))
					{
						OnUpdateAvatarUrlResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.EmptyResult>)delegate660;
					}
				}
			}
			if (this.OnUpdateCharacterDataRequestEvent != null)
			{
				Delegate[] invocationList661 = this.OnUpdateCharacterDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate661 in invocationList661)
				{
					if (object.ReferenceEquals(delegate661.Target, instance))
					{
						OnUpdateCharacterDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.UpdateCharacterDataRequest>)delegate661;
					}
				}
			}
			if (this.OnUpdateCharacterDataResultEvent != null)
			{
				Delegate[] invocationList662 = this.OnUpdateCharacterDataResultEvent.GetInvocationList();
				foreach (Delegate delegate662 in invocationList662)
				{
					if (object.ReferenceEquals(delegate662.Target, instance))
					{
						OnUpdateCharacterDataResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.UpdateCharacterDataResult>)delegate662;
					}
				}
			}
			if (this.OnUpdateCharacterStatisticsRequestEvent != null)
			{
				Delegate[] invocationList663 = this.OnUpdateCharacterStatisticsRequestEvent.GetInvocationList();
				foreach (Delegate delegate663 in invocationList663)
				{
					if (object.ReferenceEquals(delegate663.Target, instance))
					{
						OnUpdateCharacterStatisticsRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.UpdateCharacterStatisticsRequest>)delegate663;
					}
				}
			}
			if (this.OnUpdateCharacterStatisticsResultEvent != null)
			{
				Delegate[] invocationList664 = this.OnUpdateCharacterStatisticsResultEvent.GetInvocationList();
				foreach (Delegate delegate664 in invocationList664)
				{
					if (object.ReferenceEquals(delegate664.Target, instance))
					{
						OnUpdateCharacterStatisticsResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.UpdateCharacterStatisticsResult>)delegate664;
					}
				}
			}
			if (this.OnUpdatePlayerStatisticsRequestEvent != null)
			{
				Delegate[] invocationList665 = this.OnUpdatePlayerStatisticsRequestEvent.GetInvocationList();
				foreach (Delegate delegate665 in invocationList665)
				{
					if (object.ReferenceEquals(delegate665.Target, instance))
					{
						OnUpdatePlayerStatisticsRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.UpdatePlayerStatisticsRequest>)delegate665;
					}
				}
			}
			if (this.OnUpdatePlayerStatisticsResultEvent != null)
			{
				Delegate[] invocationList666 = this.OnUpdatePlayerStatisticsResultEvent.GetInvocationList();
				foreach (Delegate delegate666 in invocationList666)
				{
					if (object.ReferenceEquals(delegate666.Target, instance))
					{
						OnUpdatePlayerStatisticsResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.UpdatePlayerStatisticsResult>)delegate666;
					}
				}
			}
			if (this.OnUpdateSharedGroupDataRequestEvent != null)
			{
				Delegate[] invocationList667 = this.OnUpdateSharedGroupDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate667 in invocationList667)
				{
					if (object.ReferenceEquals(delegate667.Target, instance))
					{
						OnUpdateSharedGroupDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.UpdateSharedGroupDataRequest>)delegate667;
					}
				}
			}
			if (this.OnUpdateSharedGroupDataResultEvent != null)
			{
				Delegate[] invocationList668 = this.OnUpdateSharedGroupDataResultEvent.GetInvocationList();
				foreach (Delegate delegate668 in invocationList668)
				{
					if (object.ReferenceEquals(delegate668.Target, instance))
					{
						OnUpdateSharedGroupDataResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.UpdateSharedGroupDataResult>)delegate668;
					}
				}
			}
			if (this.OnUpdateUserDataRequestEvent != null)
			{
				Delegate[] invocationList669 = this.OnUpdateUserDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate669 in invocationList669)
				{
					if (object.ReferenceEquals(delegate669.Target, instance))
					{
						OnUpdateUserDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.UpdateUserDataRequest>)delegate669;
					}
				}
			}
			if (this.OnUpdateUserDataResultEvent != null)
			{
				Delegate[] invocationList670 = this.OnUpdateUserDataResultEvent.GetInvocationList();
				foreach (Delegate delegate670 in invocationList670)
				{
					if (object.ReferenceEquals(delegate670.Target, instance))
					{
						OnUpdateUserDataResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.UpdateUserDataResult>)delegate670;
					}
				}
			}
			if (this.OnUpdateUserPublisherDataRequestEvent != null)
			{
				Delegate[] invocationList671 = this.OnUpdateUserPublisherDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate671 in invocationList671)
				{
					if (object.ReferenceEquals(delegate671.Target, instance))
					{
						OnUpdateUserPublisherDataRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.UpdateUserDataRequest>)delegate671;
					}
				}
			}
			if (this.OnUpdateUserPublisherDataResultEvent != null)
			{
				Delegate[] invocationList672 = this.OnUpdateUserPublisherDataResultEvent.GetInvocationList();
				foreach (Delegate delegate672 in invocationList672)
				{
					if (object.ReferenceEquals(delegate672.Target, instance))
					{
						OnUpdateUserPublisherDataResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.UpdateUserDataResult>)delegate672;
					}
				}
			}
			if (this.OnUpdateUserTitleDisplayNameRequestEvent != null)
			{
				Delegate[] invocationList673 = this.OnUpdateUserTitleDisplayNameRequestEvent.GetInvocationList();
				foreach (Delegate delegate673 in invocationList673)
				{
					if (object.ReferenceEquals(delegate673.Target, instance))
					{
						OnUpdateUserTitleDisplayNameRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.UpdateUserTitleDisplayNameRequest>)delegate673;
					}
				}
			}
			if (this.OnUpdateUserTitleDisplayNameResultEvent != null)
			{
				Delegate[] invocationList674 = this.OnUpdateUserTitleDisplayNameResultEvent.GetInvocationList();
				foreach (Delegate delegate674 in invocationList674)
				{
					if (object.ReferenceEquals(delegate674.Target, instance))
					{
						OnUpdateUserTitleDisplayNameResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.UpdateUserTitleDisplayNameResult>)delegate674;
					}
				}
			}
			if (this.OnValidateAmazonIAPReceiptRequestEvent != null)
			{
				Delegate[] invocationList675 = this.OnValidateAmazonIAPReceiptRequestEvent.GetInvocationList();
				foreach (Delegate delegate675 in invocationList675)
				{
					if (object.ReferenceEquals(delegate675.Target, instance))
					{
						OnValidateAmazonIAPReceiptRequestEvent -= (PlayFabRequestEvent<ValidateAmazonReceiptRequest>)delegate675;
					}
				}
			}
			if (this.OnValidateAmazonIAPReceiptResultEvent != null)
			{
				Delegate[] invocationList676 = this.OnValidateAmazonIAPReceiptResultEvent.GetInvocationList();
				foreach (Delegate delegate676 in invocationList676)
				{
					if (object.ReferenceEquals(delegate676.Target, instance))
					{
						OnValidateAmazonIAPReceiptResultEvent -= (PlayFabResultEvent<ValidateAmazonReceiptResult>)delegate676;
					}
				}
			}
			if (this.OnValidateGooglePlayPurchaseRequestEvent != null)
			{
				Delegate[] invocationList677 = this.OnValidateGooglePlayPurchaseRequestEvent.GetInvocationList();
				foreach (Delegate delegate677 in invocationList677)
				{
					if (object.ReferenceEquals(delegate677.Target, instance))
					{
						OnValidateGooglePlayPurchaseRequestEvent -= (PlayFabRequestEvent<ValidateGooglePlayPurchaseRequest>)delegate677;
					}
				}
			}
			if (this.OnValidateGooglePlayPurchaseResultEvent != null)
			{
				Delegate[] invocationList678 = this.OnValidateGooglePlayPurchaseResultEvent.GetInvocationList();
				foreach (Delegate delegate678 in invocationList678)
				{
					if (object.ReferenceEquals(delegate678.Target, instance))
					{
						OnValidateGooglePlayPurchaseResultEvent -= (PlayFabResultEvent<ValidateGooglePlayPurchaseResult>)delegate678;
					}
				}
			}
			if (this.OnValidateIOSReceiptRequestEvent != null)
			{
				Delegate[] invocationList679 = this.OnValidateIOSReceiptRequestEvent.GetInvocationList();
				foreach (Delegate delegate679 in invocationList679)
				{
					if (object.ReferenceEquals(delegate679.Target, instance))
					{
						OnValidateIOSReceiptRequestEvent -= (PlayFabRequestEvent<ValidateIOSReceiptRequest>)delegate679;
					}
				}
			}
			if (this.OnValidateIOSReceiptResultEvent != null)
			{
				Delegate[] invocationList680 = this.OnValidateIOSReceiptResultEvent.GetInvocationList();
				foreach (Delegate delegate680 in invocationList680)
				{
					if (object.ReferenceEquals(delegate680.Target, instance))
					{
						OnValidateIOSReceiptResultEvent -= (PlayFabResultEvent<ValidateIOSReceiptResult>)delegate680;
					}
				}
			}
			if (this.OnValidateWindowsStoreReceiptRequestEvent != null)
			{
				Delegate[] invocationList681 = this.OnValidateWindowsStoreReceiptRequestEvent.GetInvocationList();
				foreach (Delegate delegate681 in invocationList681)
				{
					if (object.ReferenceEquals(delegate681.Target, instance))
					{
						OnValidateWindowsStoreReceiptRequestEvent -= (PlayFabRequestEvent<ValidateWindowsReceiptRequest>)delegate681;
					}
				}
			}
			if (this.OnValidateWindowsStoreReceiptResultEvent != null)
			{
				Delegate[] invocationList682 = this.OnValidateWindowsStoreReceiptResultEvent.GetInvocationList();
				foreach (Delegate delegate682 in invocationList682)
				{
					if (object.ReferenceEquals(delegate682.Target, instance))
					{
						OnValidateWindowsStoreReceiptResultEvent -= (PlayFabResultEvent<ValidateWindowsReceiptResult>)delegate682;
					}
				}
			}
			if (this.OnWriteCharacterEventRequestEvent != null)
			{
				Delegate[] invocationList683 = this.OnWriteCharacterEventRequestEvent.GetInvocationList();
				foreach (Delegate delegate683 in invocationList683)
				{
					if (object.ReferenceEquals(delegate683.Target, instance))
					{
						OnWriteCharacterEventRequestEvent -= (PlayFabRequestEvent<WriteClientCharacterEventRequest>)delegate683;
					}
				}
			}
			if (this.OnWriteCharacterEventResultEvent != null)
			{
				Delegate[] invocationList684 = this.OnWriteCharacterEventResultEvent.GetInvocationList();
				foreach (Delegate delegate684 in invocationList684)
				{
					if (object.ReferenceEquals(delegate684.Target, instance))
					{
						OnWriteCharacterEventResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.WriteEventResponse>)delegate684;
					}
				}
			}
			if (this.OnWritePlayerEventRequestEvent != null)
			{
				Delegate[] invocationList685 = this.OnWritePlayerEventRequestEvent.GetInvocationList();
				foreach (Delegate delegate685 in invocationList685)
				{
					if (object.ReferenceEquals(delegate685.Target, instance))
					{
						OnWritePlayerEventRequestEvent -= (PlayFabRequestEvent<WriteClientPlayerEventRequest>)delegate685;
					}
				}
			}
			if (this.OnWritePlayerEventResultEvent != null)
			{
				Delegate[] invocationList686 = this.OnWritePlayerEventResultEvent.GetInvocationList();
				foreach (Delegate delegate686 in invocationList686)
				{
					if (object.ReferenceEquals(delegate686.Target, instance))
					{
						OnWritePlayerEventResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.WriteEventResponse>)delegate686;
					}
				}
			}
			if (this.OnWriteTitleEventRequestEvent != null)
			{
				Delegate[] invocationList687 = this.OnWriteTitleEventRequestEvent.GetInvocationList();
				foreach (Delegate delegate687 in invocationList687)
				{
					if (object.ReferenceEquals(delegate687.Target, instance))
					{
						OnWriteTitleEventRequestEvent -= (PlayFabRequestEvent<PlayFab.ClientModels.WriteTitleEventRequest>)delegate687;
					}
				}
			}
			if (this.OnWriteTitleEventResultEvent == null)
			{
				return;
			}
			Delegate[] invocationList688 = this.OnWriteTitleEventResultEvent.GetInvocationList();
			foreach (Delegate delegate688 in invocationList688)
			{
				if (object.ReferenceEquals(delegate688.Target, instance))
				{
					OnWriteTitleEventResultEvent -= (PlayFabResultEvent<PlayFab.ClientModels.WriteEventResponse>)delegate688;
				}
			}
		}

		private void OnProcessingErrorEvent(PlayFabRequestCommon request, PlayFabError error)
		{
			if (_instance.OnGlobalErrorEvent != null)
			{
				_instance.OnGlobalErrorEvent(request, error);
			}
		}

		private void OnProcessingEvent(ApiProcessingEventArgs e)
		{
			if (e.EventType == ApiProcessingEventType.Pre)
			{
				Type type = e.Request.GetType();
				if (type == typeof(AbortTaskInstanceRequest) && _instance.OnAdminAbortTaskInstanceRequestEvent != null)
				{
					_instance.OnAdminAbortTaskInstanceRequestEvent((AbortTaskInstanceRequest)e.Request);
				}
				else if (type == typeof(AddNewsRequest) && _instance.OnAdminAddNewsRequestEvent != null)
				{
					_instance.OnAdminAddNewsRequestEvent((AddNewsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.AddPlayerTagRequest) && _instance.OnAdminAddPlayerTagRequestEvent != null)
				{
					_instance.OnAdminAddPlayerTagRequestEvent((PlayFab.AdminModels.AddPlayerTagRequest)e.Request);
				}
				else if (type == typeof(AddServerBuildRequest) && _instance.OnAdminAddServerBuildRequestEvent != null)
				{
					_instance.OnAdminAddServerBuildRequestEvent((AddServerBuildRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.AddUserVirtualCurrencyRequest) && _instance.OnAdminAddUserVirtualCurrencyRequestEvent != null)
				{
					_instance.OnAdminAddUserVirtualCurrencyRequestEvent((PlayFab.AdminModels.AddUserVirtualCurrencyRequest)e.Request);
				}
				else if (type == typeof(AddVirtualCurrencyTypesRequest) && _instance.OnAdminAddVirtualCurrencyTypesRequestEvent != null)
				{
					_instance.OnAdminAddVirtualCurrencyTypesRequestEvent((AddVirtualCurrencyTypesRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.BanUsersRequest) && _instance.OnAdminBanUsersRequestEvent != null)
				{
					_instance.OnAdminBanUsersRequestEvent((PlayFab.AdminModels.BanUsersRequest)e.Request);
				}
				else if (type == typeof(CheckLimitedEditionItemAvailabilityRequest) && _instance.OnAdminCheckLimitedEditionItemAvailabilityRequestEvent != null)
				{
					_instance.OnAdminCheckLimitedEditionItemAvailabilityRequestEvent((CheckLimitedEditionItemAvailabilityRequest)e.Request);
				}
				else if (type == typeof(CreateActionsOnPlayerSegmentTaskRequest) && _instance.OnAdminCreateActionsOnPlayersInSegmentTaskRequestEvent != null)
				{
					_instance.OnAdminCreateActionsOnPlayersInSegmentTaskRequestEvent((CreateActionsOnPlayerSegmentTaskRequest)e.Request);
				}
				else if (type == typeof(CreateCloudScriptTaskRequest) && _instance.OnAdminCreateCloudScriptTaskRequestEvent != null)
				{
					_instance.OnAdminCreateCloudScriptTaskRequestEvent((CreateCloudScriptTaskRequest)e.Request);
				}
				else if (type == typeof(CreatePlayerSharedSecretRequest) && _instance.OnAdminCreatePlayerSharedSecretRequestEvent != null)
				{
					_instance.OnAdminCreatePlayerSharedSecretRequestEvent((CreatePlayerSharedSecretRequest)e.Request);
				}
				else if (type == typeof(CreatePlayerStatisticDefinitionRequest) && _instance.OnAdminCreatePlayerStatisticDefinitionRequestEvent != null)
				{
					_instance.OnAdminCreatePlayerStatisticDefinitionRequestEvent((CreatePlayerStatisticDefinitionRequest)e.Request);
				}
				else if (type == typeof(DeleteContentRequest) && _instance.OnAdminDeleteContentRequestEvent != null)
				{
					_instance.OnAdminDeleteContentRequestEvent((DeleteContentRequest)e.Request);
				}
				else if (type == typeof(DeletePlayerRequest) && _instance.OnAdminDeletePlayerRequestEvent != null)
				{
					_instance.OnAdminDeletePlayerRequestEvent((DeletePlayerRequest)e.Request);
				}
				else if (type == typeof(DeletePlayerSharedSecretRequest) && _instance.OnAdminDeletePlayerSharedSecretRequestEvent != null)
				{
					_instance.OnAdminDeletePlayerSharedSecretRequestEvent((DeletePlayerSharedSecretRequest)e.Request);
				}
				else if (type == typeof(DeleteStoreRequest) && _instance.OnAdminDeleteStoreRequestEvent != null)
				{
					_instance.OnAdminDeleteStoreRequestEvent((DeleteStoreRequest)e.Request);
				}
				else if (type == typeof(DeleteTaskRequest) && _instance.OnAdminDeleteTaskRequestEvent != null)
				{
					_instance.OnAdminDeleteTaskRequestEvent((DeleteTaskRequest)e.Request);
				}
				else if (type == typeof(DeleteTitleRequest) && _instance.OnAdminDeleteTitleRequestEvent != null)
				{
					_instance.OnAdminDeleteTitleRequestEvent((DeleteTitleRequest)e.Request);
				}
				else if (type == typeof(GetTaskInstanceRequest) && _instance.OnAdminGetActionsOnPlayersInSegmentTaskInstanceRequestEvent != null)
				{
					_instance.OnAdminGetActionsOnPlayersInSegmentTaskInstanceRequestEvent((GetTaskInstanceRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetAllSegmentsRequest) && _instance.OnAdminGetAllSegmentsRequestEvent != null)
				{
					_instance.OnAdminGetAllSegmentsRequestEvent((PlayFab.AdminModels.GetAllSegmentsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetCatalogItemsRequest) && _instance.OnAdminGetCatalogItemsRequestEvent != null)
				{
					_instance.OnAdminGetCatalogItemsRequestEvent((PlayFab.AdminModels.GetCatalogItemsRequest)e.Request);
				}
				else if (type == typeof(GetCloudScriptRevisionRequest) && _instance.OnAdminGetCloudScriptRevisionRequestEvent != null)
				{
					_instance.OnAdminGetCloudScriptRevisionRequestEvent((GetCloudScriptRevisionRequest)e.Request);
				}
				else if (type == typeof(GetTaskInstanceRequest) && _instance.OnAdminGetCloudScriptTaskInstanceRequestEvent != null)
				{
					_instance.OnAdminGetCloudScriptTaskInstanceRequestEvent((GetTaskInstanceRequest)e.Request);
				}
				else if (type == typeof(GetCloudScriptVersionsRequest) && _instance.OnAdminGetCloudScriptVersionsRequestEvent != null)
				{
					_instance.OnAdminGetCloudScriptVersionsRequestEvent((GetCloudScriptVersionsRequest)e.Request);
				}
				else if (type == typeof(GetContentListRequest) && _instance.OnAdminGetContentListRequestEvent != null)
				{
					_instance.OnAdminGetContentListRequestEvent((GetContentListRequest)e.Request);
				}
				else if (type == typeof(GetContentUploadUrlRequest) && _instance.OnAdminGetContentUploadUrlRequestEvent != null)
				{
					_instance.OnAdminGetContentUploadUrlRequestEvent((GetContentUploadUrlRequest)e.Request);
				}
				else if (type == typeof(GetDataReportRequest) && _instance.OnAdminGetDataReportRequestEvent != null)
				{
					_instance.OnAdminGetDataReportRequestEvent((GetDataReportRequest)e.Request);
				}
				else if (type == typeof(GetMatchmakerGameInfoRequest) && _instance.OnAdminGetMatchmakerGameInfoRequestEvent != null)
				{
					_instance.OnAdminGetMatchmakerGameInfoRequestEvent((GetMatchmakerGameInfoRequest)e.Request);
				}
				else if (type == typeof(GetMatchmakerGameModesRequest) && _instance.OnAdminGetMatchmakerGameModesRequestEvent != null)
				{
					_instance.OnAdminGetMatchmakerGameModesRequestEvent((GetMatchmakerGameModesRequest)e.Request);
				}
				else if (type == typeof(GetPlayerIdFromAuthTokenRequest) && _instance.OnAdminGetPlayerIdFromAuthTokenRequestEvent != null)
				{
					_instance.OnAdminGetPlayerIdFromAuthTokenRequestEvent((GetPlayerIdFromAuthTokenRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetPlayerProfileRequest) && _instance.OnAdminGetPlayerProfileRequestEvent != null)
				{
					_instance.OnAdminGetPlayerProfileRequestEvent((PlayFab.AdminModels.GetPlayerProfileRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetPlayersSegmentsRequest) && _instance.OnAdminGetPlayerSegmentsRequestEvent != null)
				{
					_instance.OnAdminGetPlayerSegmentsRequestEvent((PlayFab.AdminModels.GetPlayersSegmentsRequest)e.Request);
				}
				else if (type == typeof(GetPlayerSharedSecretsRequest) && _instance.OnAdminGetPlayerSharedSecretsRequestEvent != null)
				{
					_instance.OnAdminGetPlayerSharedSecretsRequestEvent((GetPlayerSharedSecretsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetPlayersInSegmentRequest) && _instance.OnAdminGetPlayersInSegmentRequestEvent != null)
				{
					_instance.OnAdminGetPlayersInSegmentRequestEvent((PlayFab.AdminModels.GetPlayersInSegmentRequest)e.Request);
				}
				else if (type == typeof(GetPlayerStatisticDefinitionsRequest) && _instance.OnAdminGetPlayerStatisticDefinitionsRequestEvent != null)
				{
					_instance.OnAdminGetPlayerStatisticDefinitionsRequestEvent((GetPlayerStatisticDefinitionsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetPlayerStatisticVersionsRequest) && _instance.OnAdminGetPlayerStatisticVersionsRequestEvent != null)
				{
					_instance.OnAdminGetPlayerStatisticVersionsRequestEvent((PlayFab.AdminModels.GetPlayerStatisticVersionsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetPlayerTagsRequest) && _instance.OnAdminGetPlayerTagsRequestEvent != null)
				{
					_instance.OnAdminGetPlayerTagsRequestEvent((PlayFab.AdminModels.GetPlayerTagsRequest)e.Request);
				}
				else if (type == typeof(GetPolicyRequest) && _instance.OnAdminGetPolicyRequestEvent != null)
				{
					_instance.OnAdminGetPolicyRequestEvent((GetPolicyRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetPublisherDataRequest) && _instance.OnAdminGetPublisherDataRequestEvent != null)
				{
					_instance.OnAdminGetPublisherDataRequestEvent((PlayFab.AdminModels.GetPublisherDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetRandomResultTablesRequest) && _instance.OnAdminGetRandomResultTablesRequestEvent != null)
				{
					_instance.OnAdminGetRandomResultTablesRequestEvent((PlayFab.AdminModels.GetRandomResultTablesRequest)e.Request);
				}
				else if (type == typeof(GetServerBuildInfoRequest) && _instance.OnAdminGetServerBuildInfoRequestEvent != null)
				{
					_instance.OnAdminGetServerBuildInfoRequestEvent((GetServerBuildInfoRequest)e.Request);
				}
				else if (type == typeof(GetServerBuildUploadURLRequest) && _instance.OnAdminGetServerBuildUploadUrlRequestEvent != null)
				{
					_instance.OnAdminGetServerBuildUploadUrlRequestEvent((GetServerBuildUploadURLRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetStoreItemsRequest) && _instance.OnAdminGetStoreItemsRequestEvent != null)
				{
					_instance.OnAdminGetStoreItemsRequestEvent((PlayFab.AdminModels.GetStoreItemsRequest)e.Request);
				}
				else if (type == typeof(GetTaskInstancesRequest) && _instance.OnAdminGetTaskInstancesRequestEvent != null)
				{
					_instance.OnAdminGetTaskInstancesRequestEvent((GetTaskInstancesRequest)e.Request);
				}
				else if (type == typeof(GetTasksRequest) && _instance.OnAdminGetTasksRequestEvent != null)
				{
					_instance.OnAdminGetTasksRequestEvent((GetTasksRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetTitleDataRequest) && _instance.OnAdminGetTitleDataRequestEvent != null)
				{
					_instance.OnAdminGetTitleDataRequestEvent((PlayFab.AdminModels.GetTitleDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetTitleDataRequest) && _instance.OnAdminGetTitleInternalDataRequestEvent != null)
				{
					_instance.OnAdminGetTitleInternalDataRequestEvent((PlayFab.AdminModels.GetTitleDataRequest)e.Request);
				}
				else if (type == typeof(LookupUserAccountInfoRequest) && _instance.OnAdminGetUserAccountInfoRequestEvent != null)
				{
					_instance.OnAdminGetUserAccountInfoRequestEvent((LookupUserAccountInfoRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetUserBansRequest) && _instance.OnAdminGetUserBansRequestEvent != null)
				{
					_instance.OnAdminGetUserBansRequestEvent((PlayFab.AdminModels.GetUserBansRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetUserDataRequest) && _instance.OnAdminGetUserDataRequestEvent != null)
				{
					_instance.OnAdminGetUserDataRequestEvent((PlayFab.AdminModels.GetUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetUserDataRequest) && _instance.OnAdminGetUserInternalDataRequestEvent != null)
				{
					_instance.OnAdminGetUserInternalDataRequestEvent((PlayFab.AdminModels.GetUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetUserInventoryRequest) && _instance.OnAdminGetUserInventoryRequestEvent != null)
				{
					_instance.OnAdminGetUserInventoryRequestEvent((PlayFab.AdminModels.GetUserInventoryRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetUserDataRequest) && _instance.OnAdminGetUserPublisherDataRequestEvent != null)
				{
					_instance.OnAdminGetUserPublisherDataRequestEvent((PlayFab.AdminModels.GetUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetUserDataRequest) && _instance.OnAdminGetUserPublisherInternalDataRequestEvent != null)
				{
					_instance.OnAdminGetUserPublisherInternalDataRequestEvent((PlayFab.AdminModels.GetUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetUserDataRequest) && _instance.OnAdminGetUserPublisherReadOnlyDataRequestEvent != null)
				{
					_instance.OnAdminGetUserPublisherReadOnlyDataRequestEvent((PlayFab.AdminModels.GetUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GetUserDataRequest) && _instance.OnAdminGetUserReadOnlyDataRequestEvent != null)
				{
					_instance.OnAdminGetUserReadOnlyDataRequestEvent((PlayFab.AdminModels.GetUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.GrantItemsToUsersRequest) && _instance.OnAdminGrantItemsToUsersRequestEvent != null)
				{
					_instance.OnAdminGrantItemsToUsersRequestEvent((PlayFab.AdminModels.GrantItemsToUsersRequest)e.Request);
				}
				else if (type == typeof(IncrementLimitedEditionItemAvailabilityRequest) && _instance.OnAdminIncrementLimitedEditionItemAvailabilityRequestEvent != null)
				{
					_instance.OnAdminIncrementLimitedEditionItemAvailabilityRequestEvent((IncrementLimitedEditionItemAvailabilityRequest)e.Request);
				}
				else if (type == typeof(IncrementPlayerStatisticVersionRequest) && _instance.OnAdminIncrementPlayerStatisticVersionRequestEvent != null)
				{
					_instance.OnAdminIncrementPlayerStatisticVersionRequestEvent((IncrementPlayerStatisticVersionRequest)e.Request);
				}
				else if (type == typeof(ListBuildsRequest) && _instance.OnAdminListServerBuildsRequestEvent != null)
				{
					_instance.OnAdminListServerBuildsRequestEvent((ListBuildsRequest)e.Request);
				}
				else if (type == typeof(ListVirtualCurrencyTypesRequest) && _instance.OnAdminListVirtualCurrencyTypesRequestEvent != null)
				{
					_instance.OnAdminListVirtualCurrencyTypesRequestEvent((ListVirtualCurrencyTypesRequest)e.Request);
				}
				else if (type == typeof(ModifyMatchmakerGameModesRequest) && _instance.OnAdminModifyMatchmakerGameModesRequestEvent != null)
				{
					_instance.OnAdminModifyMatchmakerGameModesRequestEvent((ModifyMatchmakerGameModesRequest)e.Request);
				}
				else if (type == typeof(ModifyServerBuildRequest) && _instance.OnAdminModifyServerBuildRequestEvent != null)
				{
					_instance.OnAdminModifyServerBuildRequestEvent((ModifyServerBuildRequest)e.Request);
				}
				else if (type == typeof(RefundPurchaseRequest) && _instance.OnAdminRefundPurchaseRequestEvent != null)
				{
					_instance.OnAdminRefundPurchaseRequestEvent((RefundPurchaseRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.RemovePlayerTagRequest) && _instance.OnAdminRemovePlayerTagRequestEvent != null)
				{
					_instance.OnAdminRemovePlayerTagRequestEvent((PlayFab.AdminModels.RemovePlayerTagRequest)e.Request);
				}
				else if (type == typeof(RemoveServerBuildRequest) && _instance.OnAdminRemoveServerBuildRequestEvent != null)
				{
					_instance.OnAdminRemoveServerBuildRequestEvent((RemoveServerBuildRequest)e.Request);
				}
				else if (type == typeof(RemoveVirtualCurrencyTypesRequest) && _instance.OnAdminRemoveVirtualCurrencyTypesRequestEvent != null)
				{
					_instance.OnAdminRemoveVirtualCurrencyTypesRequestEvent((RemoveVirtualCurrencyTypesRequest)e.Request);
				}
				else if (type == typeof(ResetCharacterStatisticsRequest) && _instance.OnAdminResetCharacterStatisticsRequestEvent != null)
				{
					_instance.OnAdminResetCharacterStatisticsRequestEvent((ResetCharacterStatisticsRequest)e.Request);
				}
				else if (type == typeof(ResetPasswordRequest) && _instance.OnAdminResetPasswordRequestEvent != null)
				{
					_instance.OnAdminResetPasswordRequestEvent((ResetPasswordRequest)e.Request);
				}
				else if (type == typeof(ResetUserStatisticsRequest) && _instance.OnAdminResetUserStatisticsRequestEvent != null)
				{
					_instance.OnAdminResetUserStatisticsRequestEvent((ResetUserStatisticsRequest)e.Request);
				}
				else if (type == typeof(ResolvePurchaseDisputeRequest) && _instance.OnAdminResolvePurchaseDisputeRequestEvent != null)
				{
					_instance.OnAdminResolvePurchaseDisputeRequestEvent((ResolvePurchaseDisputeRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.RevokeAllBansForUserRequest) && _instance.OnAdminRevokeAllBansForUserRequestEvent != null)
				{
					_instance.OnAdminRevokeAllBansForUserRequestEvent((PlayFab.AdminModels.RevokeAllBansForUserRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.RevokeBansRequest) && _instance.OnAdminRevokeBansRequestEvent != null)
				{
					_instance.OnAdminRevokeBansRequestEvent((PlayFab.AdminModels.RevokeBansRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.RevokeInventoryItemRequest) && _instance.OnAdminRevokeInventoryItemRequestEvent != null)
				{
					_instance.OnAdminRevokeInventoryItemRequestEvent((PlayFab.AdminModels.RevokeInventoryItemRequest)e.Request);
				}
				else if (type == typeof(RunTaskRequest) && _instance.OnAdminRunTaskRequestEvent != null)
				{
					_instance.OnAdminRunTaskRequestEvent((RunTaskRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.SendAccountRecoveryEmailRequest) && _instance.OnAdminSendAccountRecoveryEmailRequestEvent != null)
				{
					_instance.OnAdminSendAccountRecoveryEmailRequestEvent((PlayFab.AdminModels.SendAccountRecoveryEmailRequest)e.Request);
				}
				else if (type == typeof(UpdateCatalogItemsRequest) && _instance.OnAdminSetCatalogItemsRequestEvent != null)
				{
					_instance.OnAdminSetCatalogItemsRequestEvent((UpdateCatalogItemsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.SetPlayerSecretRequest) && _instance.OnAdminSetPlayerSecretRequestEvent != null)
				{
					_instance.OnAdminSetPlayerSecretRequestEvent((PlayFab.AdminModels.SetPlayerSecretRequest)e.Request);
				}
				else if (type == typeof(SetPublishedRevisionRequest) && _instance.OnAdminSetPublishedRevisionRequestEvent != null)
				{
					_instance.OnAdminSetPublishedRevisionRequestEvent((SetPublishedRevisionRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.SetPublisherDataRequest) && _instance.OnAdminSetPublisherDataRequestEvent != null)
				{
					_instance.OnAdminSetPublisherDataRequestEvent((PlayFab.AdminModels.SetPublisherDataRequest)e.Request);
				}
				else if (type == typeof(UpdateStoreItemsRequest) && _instance.OnAdminSetStoreItemsRequestEvent != null)
				{
					_instance.OnAdminSetStoreItemsRequestEvent((UpdateStoreItemsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.SetTitleDataRequest) && _instance.OnAdminSetTitleDataRequestEvent != null)
				{
					_instance.OnAdminSetTitleDataRequestEvent((PlayFab.AdminModels.SetTitleDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.SetTitleDataRequest) && _instance.OnAdminSetTitleInternalDataRequestEvent != null)
				{
					_instance.OnAdminSetTitleInternalDataRequestEvent((PlayFab.AdminModels.SetTitleDataRequest)e.Request);
				}
				else if (type == typeof(SetupPushNotificationRequest) && _instance.OnAdminSetupPushNotificationRequestEvent != null)
				{
					_instance.OnAdminSetupPushNotificationRequestEvent((SetupPushNotificationRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.SubtractUserVirtualCurrencyRequest) && _instance.OnAdminSubtractUserVirtualCurrencyRequestEvent != null)
				{
					_instance.OnAdminSubtractUserVirtualCurrencyRequestEvent((PlayFab.AdminModels.SubtractUserVirtualCurrencyRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.UpdateBansRequest) && _instance.OnAdminUpdateBansRequestEvent != null)
				{
					_instance.OnAdminUpdateBansRequestEvent((PlayFab.AdminModels.UpdateBansRequest)e.Request);
				}
				else if (type == typeof(UpdateCatalogItemsRequest) && _instance.OnAdminUpdateCatalogItemsRequestEvent != null)
				{
					_instance.OnAdminUpdateCatalogItemsRequestEvent((UpdateCatalogItemsRequest)e.Request);
				}
				else if (type == typeof(UpdateCloudScriptRequest) && _instance.OnAdminUpdateCloudScriptRequestEvent != null)
				{
					_instance.OnAdminUpdateCloudScriptRequestEvent((UpdateCloudScriptRequest)e.Request);
				}
				else if (type == typeof(UpdatePlayerSharedSecretRequest) && _instance.OnAdminUpdatePlayerSharedSecretRequestEvent != null)
				{
					_instance.OnAdminUpdatePlayerSharedSecretRequestEvent((UpdatePlayerSharedSecretRequest)e.Request);
				}
				else if (type == typeof(UpdatePlayerStatisticDefinitionRequest) && _instance.OnAdminUpdatePlayerStatisticDefinitionRequestEvent != null)
				{
					_instance.OnAdminUpdatePlayerStatisticDefinitionRequestEvent((UpdatePlayerStatisticDefinitionRequest)e.Request);
				}
				else if (type == typeof(UpdatePolicyRequest) && _instance.OnAdminUpdatePolicyRequestEvent != null)
				{
					_instance.OnAdminUpdatePolicyRequestEvent((UpdatePolicyRequest)e.Request);
				}
				else if (type == typeof(UpdateRandomResultTablesRequest) && _instance.OnAdminUpdateRandomResultTablesRequestEvent != null)
				{
					_instance.OnAdminUpdateRandomResultTablesRequestEvent((UpdateRandomResultTablesRequest)e.Request);
				}
				else if (type == typeof(UpdateStoreItemsRequest) && _instance.OnAdminUpdateStoreItemsRequestEvent != null)
				{
					_instance.OnAdminUpdateStoreItemsRequestEvent((UpdateStoreItemsRequest)e.Request);
				}
				else if (type == typeof(UpdateTaskRequest) && _instance.OnAdminUpdateTaskRequestEvent != null)
				{
					_instance.OnAdminUpdateTaskRequestEvent((UpdateTaskRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.UpdateUserDataRequest) && _instance.OnAdminUpdateUserDataRequestEvent != null)
				{
					_instance.OnAdminUpdateUserDataRequestEvent((PlayFab.AdminModels.UpdateUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.UpdateUserInternalDataRequest) && _instance.OnAdminUpdateUserInternalDataRequestEvent != null)
				{
					_instance.OnAdminUpdateUserInternalDataRequestEvent((PlayFab.AdminModels.UpdateUserInternalDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.UpdateUserDataRequest) && _instance.OnAdminUpdateUserPublisherDataRequestEvent != null)
				{
					_instance.OnAdminUpdateUserPublisherDataRequestEvent((PlayFab.AdminModels.UpdateUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.UpdateUserInternalDataRequest) && _instance.OnAdminUpdateUserPublisherInternalDataRequestEvent != null)
				{
					_instance.OnAdminUpdateUserPublisherInternalDataRequestEvent((PlayFab.AdminModels.UpdateUserInternalDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.UpdateUserDataRequest) && _instance.OnAdminUpdateUserPublisherReadOnlyDataRequestEvent != null)
				{
					_instance.OnAdminUpdateUserPublisherReadOnlyDataRequestEvent((PlayFab.AdminModels.UpdateUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.UpdateUserDataRequest) && _instance.OnAdminUpdateUserReadOnlyDataRequestEvent != null)
				{
					_instance.OnAdminUpdateUserReadOnlyDataRequestEvent((PlayFab.AdminModels.UpdateUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.AdminModels.UpdateUserTitleDisplayNameRequest) && _instance.OnAdminUpdateUserTitleDisplayNameRequestEvent != null)
				{
					_instance.OnAdminUpdateUserTitleDisplayNameRequestEvent((PlayFab.AdminModels.UpdateUserTitleDisplayNameRequest)e.Request);
				}
				else if (type == typeof(AuthUserRequest) && _instance.OnMatchmakerAuthUserRequestEvent != null)
				{
					_instance.OnMatchmakerAuthUserRequestEvent((AuthUserRequest)e.Request);
				}
				else if (type == typeof(PlayerJoinedRequest) && _instance.OnMatchmakerPlayerJoinedRequestEvent != null)
				{
					_instance.OnMatchmakerPlayerJoinedRequestEvent((PlayerJoinedRequest)e.Request);
				}
				else if (type == typeof(PlayerLeftRequest) && _instance.OnMatchmakerPlayerLeftRequestEvent != null)
				{
					_instance.OnMatchmakerPlayerLeftRequestEvent((PlayerLeftRequest)e.Request);
				}
				else if (type == typeof(PlayFab.MatchmakerModels.StartGameRequest) && _instance.OnMatchmakerStartGameRequestEvent != null)
				{
					_instance.OnMatchmakerStartGameRequestEvent((PlayFab.MatchmakerModels.StartGameRequest)e.Request);
				}
				else if (type == typeof(UserInfoRequest) && _instance.OnMatchmakerUserInfoRequestEvent != null)
				{
					_instance.OnMatchmakerUserInfoRequestEvent((UserInfoRequest)e.Request);
				}
				else if (type == typeof(AddCharacterVirtualCurrencyRequest) && _instance.OnServerAddCharacterVirtualCurrencyRequestEvent != null)
				{
					_instance.OnServerAddCharacterVirtualCurrencyRequestEvent((AddCharacterVirtualCurrencyRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.AddFriendRequest) && _instance.OnServerAddFriendRequestEvent != null)
				{
					_instance.OnServerAddFriendRequestEvent((PlayFab.ServerModels.AddFriendRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.AddPlayerTagRequest) && _instance.OnServerAddPlayerTagRequestEvent != null)
				{
					_instance.OnServerAddPlayerTagRequestEvent((PlayFab.ServerModels.AddPlayerTagRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.AddSharedGroupMembersRequest) && _instance.OnServerAddSharedGroupMembersRequestEvent != null)
				{
					_instance.OnServerAddSharedGroupMembersRequestEvent((PlayFab.ServerModels.AddSharedGroupMembersRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.AddUserVirtualCurrencyRequest) && _instance.OnServerAddUserVirtualCurrencyRequestEvent != null)
				{
					_instance.OnServerAddUserVirtualCurrencyRequestEvent((PlayFab.ServerModels.AddUserVirtualCurrencyRequest)e.Request);
				}
				else if (type == typeof(AuthenticateSessionTicketRequest) && _instance.OnServerAuthenticateSessionTicketRequestEvent != null)
				{
					_instance.OnServerAuthenticateSessionTicketRequestEvent((AuthenticateSessionTicketRequest)e.Request);
				}
				else if (type == typeof(AwardSteamAchievementRequest) && _instance.OnServerAwardSteamAchievementRequestEvent != null)
				{
					_instance.OnServerAwardSteamAchievementRequestEvent((AwardSteamAchievementRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.BanUsersRequest) && _instance.OnServerBanUsersRequestEvent != null)
				{
					_instance.OnServerBanUsersRequestEvent((PlayFab.ServerModels.BanUsersRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.ConsumeItemRequest) && _instance.OnServerConsumeItemRequestEvent != null)
				{
					_instance.OnServerConsumeItemRequestEvent((PlayFab.ServerModels.ConsumeItemRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.CreateSharedGroupRequest) && _instance.OnServerCreateSharedGroupRequestEvent != null)
				{
					_instance.OnServerCreateSharedGroupRequestEvent((PlayFab.ServerModels.CreateSharedGroupRequest)e.Request);
				}
				else if (type == typeof(DeleteCharacterFromUserRequest) && _instance.OnServerDeleteCharacterFromUserRequestEvent != null)
				{
					_instance.OnServerDeleteCharacterFromUserRequestEvent((DeleteCharacterFromUserRequest)e.Request);
				}
				else if (type == typeof(DeleteSharedGroupRequest) && _instance.OnServerDeleteSharedGroupRequestEvent != null)
				{
					_instance.OnServerDeleteSharedGroupRequestEvent((DeleteSharedGroupRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.DeleteUsersRequest) && _instance.OnServerDeleteUsersRequestEvent != null)
				{
					_instance.OnServerDeleteUsersRequestEvent((PlayFab.ServerModels.DeleteUsersRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.DeregisterGameRequest) && _instance.OnServerDeregisterGameRequestEvent != null)
				{
					_instance.OnServerDeregisterGameRequestEvent((PlayFab.ServerModels.DeregisterGameRequest)e.Request);
				}
				else if (type == typeof(EvaluateRandomResultTableRequest) && _instance.OnServerEvaluateRandomResultTableRequestEvent != null)
				{
					_instance.OnServerEvaluateRandomResultTableRequestEvent((EvaluateRandomResultTableRequest)e.Request);
				}
				else if (type == typeof(ExecuteCloudScriptServerRequest) && _instance.OnServerExecuteCloudScriptRequestEvent != null)
				{
					_instance.OnServerExecuteCloudScriptRequestEvent((ExecuteCloudScriptServerRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetAllSegmentsRequest) && _instance.OnServerGetAllSegmentsRequestEvent != null)
				{
					_instance.OnServerGetAllSegmentsRequestEvent((PlayFab.ServerModels.GetAllSegmentsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.ListUsersCharactersRequest) && _instance.OnServerGetAllUsersCharactersRequestEvent != null)
				{
					_instance.OnServerGetAllUsersCharactersRequestEvent((PlayFab.ServerModels.ListUsersCharactersRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetCatalogItemsRequest) && _instance.OnServerGetCatalogItemsRequestEvent != null)
				{
					_instance.OnServerGetCatalogItemsRequestEvent((PlayFab.ServerModels.GetCatalogItemsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetCharacterDataRequest) && _instance.OnServerGetCharacterDataRequestEvent != null)
				{
					_instance.OnServerGetCharacterDataRequestEvent((PlayFab.ServerModels.GetCharacterDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetCharacterDataRequest) && _instance.OnServerGetCharacterInternalDataRequestEvent != null)
				{
					_instance.OnServerGetCharacterInternalDataRequestEvent((PlayFab.ServerModels.GetCharacterDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetCharacterInventoryRequest) && _instance.OnServerGetCharacterInventoryRequestEvent != null)
				{
					_instance.OnServerGetCharacterInventoryRequestEvent((PlayFab.ServerModels.GetCharacterInventoryRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetCharacterLeaderboardRequest) && _instance.OnServerGetCharacterLeaderboardRequestEvent != null)
				{
					_instance.OnServerGetCharacterLeaderboardRequestEvent((PlayFab.ServerModels.GetCharacterLeaderboardRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetCharacterDataRequest) && _instance.OnServerGetCharacterReadOnlyDataRequestEvent != null)
				{
					_instance.OnServerGetCharacterReadOnlyDataRequestEvent((PlayFab.ServerModels.GetCharacterDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetCharacterStatisticsRequest) && _instance.OnServerGetCharacterStatisticsRequestEvent != null)
				{
					_instance.OnServerGetCharacterStatisticsRequestEvent((PlayFab.ServerModels.GetCharacterStatisticsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetContentDownloadUrlRequest) && _instance.OnServerGetContentDownloadUrlRequestEvent != null)
				{
					_instance.OnServerGetContentDownloadUrlRequestEvent((PlayFab.ServerModels.GetContentDownloadUrlRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetFriendLeaderboardRequest) && _instance.OnServerGetFriendLeaderboardRequestEvent != null)
				{
					_instance.OnServerGetFriendLeaderboardRequestEvent((PlayFab.ServerModels.GetFriendLeaderboardRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetFriendsListRequest) && _instance.OnServerGetFriendsListRequestEvent != null)
				{
					_instance.OnServerGetFriendsListRequestEvent((PlayFab.ServerModels.GetFriendsListRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetLeaderboardRequest) && _instance.OnServerGetLeaderboardRequestEvent != null)
				{
					_instance.OnServerGetLeaderboardRequestEvent((PlayFab.ServerModels.GetLeaderboardRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetLeaderboardAroundCharacterRequest) && _instance.OnServerGetLeaderboardAroundCharacterRequestEvent != null)
				{
					_instance.OnServerGetLeaderboardAroundCharacterRequestEvent((PlayFab.ServerModels.GetLeaderboardAroundCharacterRequest)e.Request);
				}
				else if (type == typeof(GetLeaderboardAroundUserRequest) && _instance.OnServerGetLeaderboardAroundUserRequestEvent != null)
				{
					_instance.OnServerGetLeaderboardAroundUserRequestEvent((GetLeaderboardAroundUserRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetLeaderboardForUsersCharactersRequest) && _instance.OnServerGetLeaderboardForUserCharactersRequestEvent != null)
				{
					_instance.OnServerGetLeaderboardForUserCharactersRequestEvent((PlayFab.ServerModels.GetLeaderboardForUsersCharactersRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetPlayerCombinedInfoRequest) && _instance.OnServerGetPlayerCombinedInfoRequestEvent != null)
				{
					_instance.OnServerGetPlayerCombinedInfoRequestEvent((PlayFab.ServerModels.GetPlayerCombinedInfoRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetPlayerProfileRequest) && _instance.OnServerGetPlayerProfileRequestEvent != null)
				{
					_instance.OnServerGetPlayerProfileRequestEvent((PlayFab.ServerModels.GetPlayerProfileRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetPlayersSegmentsRequest) && _instance.OnServerGetPlayerSegmentsRequestEvent != null)
				{
					_instance.OnServerGetPlayerSegmentsRequestEvent((PlayFab.ServerModels.GetPlayersSegmentsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetPlayersInSegmentRequest) && _instance.OnServerGetPlayersInSegmentRequestEvent != null)
				{
					_instance.OnServerGetPlayersInSegmentRequestEvent((PlayFab.ServerModels.GetPlayersInSegmentRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetPlayerStatisticsRequest) && _instance.OnServerGetPlayerStatisticsRequestEvent != null)
				{
					_instance.OnServerGetPlayerStatisticsRequestEvent((PlayFab.ServerModels.GetPlayerStatisticsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetPlayerStatisticVersionsRequest) && _instance.OnServerGetPlayerStatisticVersionsRequestEvent != null)
				{
					_instance.OnServerGetPlayerStatisticVersionsRequestEvent((PlayFab.ServerModels.GetPlayerStatisticVersionsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetPlayerTagsRequest) && _instance.OnServerGetPlayerTagsRequestEvent != null)
				{
					_instance.OnServerGetPlayerTagsRequestEvent((PlayFab.ServerModels.GetPlayerTagsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetPlayFabIDsFromFacebookIDsRequest) && _instance.OnServerGetPlayFabIDsFromFacebookIDsRequestEvent != null)
				{
					_instance.OnServerGetPlayFabIDsFromFacebookIDsRequestEvent((PlayFab.ServerModels.GetPlayFabIDsFromFacebookIDsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetPlayFabIDsFromSteamIDsRequest) && _instance.OnServerGetPlayFabIDsFromSteamIDsRequestEvent != null)
				{
					_instance.OnServerGetPlayFabIDsFromSteamIDsRequestEvent((PlayFab.ServerModels.GetPlayFabIDsFromSteamIDsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetPublisherDataRequest) && _instance.OnServerGetPublisherDataRequestEvent != null)
				{
					_instance.OnServerGetPublisherDataRequestEvent((PlayFab.ServerModels.GetPublisherDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetRandomResultTablesRequest) && _instance.OnServerGetRandomResultTablesRequestEvent != null)
				{
					_instance.OnServerGetRandomResultTablesRequestEvent((PlayFab.ServerModels.GetRandomResultTablesRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetSharedGroupDataRequest) && _instance.OnServerGetSharedGroupDataRequestEvent != null)
				{
					_instance.OnServerGetSharedGroupDataRequestEvent((PlayFab.ServerModels.GetSharedGroupDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetTimeRequest) && _instance.OnServerGetTimeRequestEvent != null)
				{
					_instance.OnServerGetTimeRequestEvent((PlayFab.ServerModels.GetTimeRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetTitleDataRequest) && _instance.OnServerGetTitleDataRequestEvent != null)
				{
					_instance.OnServerGetTitleDataRequestEvent((PlayFab.ServerModels.GetTitleDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetTitleDataRequest) && _instance.OnServerGetTitleInternalDataRequestEvent != null)
				{
					_instance.OnServerGetTitleInternalDataRequestEvent((PlayFab.ServerModels.GetTitleDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetTitleNewsRequest) && _instance.OnServerGetTitleNewsRequestEvent != null)
				{
					_instance.OnServerGetTitleNewsRequestEvent((PlayFab.ServerModels.GetTitleNewsRequest)e.Request);
				}
				else if (type == typeof(GetUserAccountInfoRequest) && _instance.OnServerGetUserAccountInfoRequestEvent != null)
				{
					_instance.OnServerGetUserAccountInfoRequestEvent((GetUserAccountInfoRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetUserBansRequest) && _instance.OnServerGetUserBansRequestEvent != null)
				{
					_instance.OnServerGetUserBansRequestEvent((PlayFab.ServerModels.GetUserBansRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetUserDataRequest) && _instance.OnServerGetUserDataRequestEvent != null)
				{
					_instance.OnServerGetUserDataRequestEvent((PlayFab.ServerModels.GetUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetUserDataRequest) && _instance.OnServerGetUserInternalDataRequestEvent != null)
				{
					_instance.OnServerGetUserInternalDataRequestEvent((PlayFab.ServerModels.GetUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetUserInventoryRequest) && _instance.OnServerGetUserInventoryRequestEvent != null)
				{
					_instance.OnServerGetUserInventoryRequestEvent((PlayFab.ServerModels.GetUserInventoryRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetUserDataRequest) && _instance.OnServerGetUserPublisherDataRequestEvent != null)
				{
					_instance.OnServerGetUserPublisherDataRequestEvent((PlayFab.ServerModels.GetUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetUserDataRequest) && _instance.OnServerGetUserPublisherInternalDataRequestEvent != null)
				{
					_instance.OnServerGetUserPublisherInternalDataRequestEvent((PlayFab.ServerModels.GetUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetUserDataRequest) && _instance.OnServerGetUserPublisherReadOnlyDataRequestEvent != null)
				{
					_instance.OnServerGetUserPublisherReadOnlyDataRequestEvent((PlayFab.ServerModels.GetUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GetUserDataRequest) && _instance.OnServerGetUserReadOnlyDataRequestEvent != null)
				{
					_instance.OnServerGetUserReadOnlyDataRequestEvent((PlayFab.ServerModels.GetUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GrantCharacterToUserRequest) && _instance.OnServerGrantCharacterToUserRequestEvent != null)
				{
					_instance.OnServerGrantCharacterToUserRequestEvent((PlayFab.ServerModels.GrantCharacterToUserRequest)e.Request);
				}
				else if (type == typeof(GrantItemsToCharacterRequest) && _instance.OnServerGrantItemsToCharacterRequestEvent != null)
				{
					_instance.OnServerGrantItemsToCharacterRequestEvent((GrantItemsToCharacterRequest)e.Request);
				}
				else if (type == typeof(GrantItemsToUserRequest) && _instance.OnServerGrantItemsToUserRequestEvent != null)
				{
					_instance.OnServerGrantItemsToUserRequestEvent((GrantItemsToUserRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.GrantItemsToUsersRequest) && _instance.OnServerGrantItemsToUsersRequestEvent != null)
				{
					_instance.OnServerGrantItemsToUsersRequestEvent((PlayFab.ServerModels.GrantItemsToUsersRequest)e.Request);
				}
				else if (type == typeof(ModifyItemUsesRequest) && _instance.OnServerModifyItemUsesRequestEvent != null)
				{
					_instance.OnServerModifyItemUsesRequestEvent((ModifyItemUsesRequest)e.Request);
				}
				else if (type == typeof(MoveItemToCharacterFromCharacterRequest) && _instance.OnServerMoveItemToCharacterFromCharacterRequestEvent != null)
				{
					_instance.OnServerMoveItemToCharacterFromCharacterRequestEvent((MoveItemToCharacterFromCharacterRequest)e.Request);
				}
				else if (type == typeof(MoveItemToCharacterFromUserRequest) && _instance.OnServerMoveItemToCharacterFromUserRequestEvent != null)
				{
					_instance.OnServerMoveItemToCharacterFromUserRequestEvent((MoveItemToCharacterFromUserRequest)e.Request);
				}
				else if (type == typeof(MoveItemToUserFromCharacterRequest) && _instance.OnServerMoveItemToUserFromCharacterRequestEvent != null)
				{
					_instance.OnServerMoveItemToUserFromCharacterRequestEvent((MoveItemToUserFromCharacterRequest)e.Request);
				}
				else if (type == typeof(NotifyMatchmakerPlayerLeftRequest) && _instance.OnServerNotifyMatchmakerPlayerLeftRequestEvent != null)
				{
					_instance.OnServerNotifyMatchmakerPlayerLeftRequestEvent((NotifyMatchmakerPlayerLeftRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.RedeemCouponRequest) && _instance.OnServerRedeemCouponRequestEvent != null)
				{
					_instance.OnServerRedeemCouponRequestEvent((PlayFab.ServerModels.RedeemCouponRequest)e.Request);
				}
				else if (type == typeof(RedeemMatchmakerTicketRequest) && _instance.OnServerRedeemMatchmakerTicketRequestEvent != null)
				{
					_instance.OnServerRedeemMatchmakerTicketRequestEvent((RedeemMatchmakerTicketRequest)e.Request);
				}
				else if (type == typeof(RefreshGameServerInstanceHeartbeatRequest) && _instance.OnServerRefreshGameServerInstanceHeartbeatRequestEvent != null)
				{
					_instance.OnServerRefreshGameServerInstanceHeartbeatRequestEvent((RefreshGameServerInstanceHeartbeatRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.RegisterGameRequest) && _instance.OnServerRegisterGameRequestEvent != null)
				{
					_instance.OnServerRegisterGameRequestEvent((PlayFab.ServerModels.RegisterGameRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.RemoveFriendRequest) && _instance.OnServerRemoveFriendRequestEvent != null)
				{
					_instance.OnServerRemoveFriendRequestEvent((PlayFab.ServerModels.RemoveFriendRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.RemovePlayerTagRequest) && _instance.OnServerRemovePlayerTagRequestEvent != null)
				{
					_instance.OnServerRemovePlayerTagRequestEvent((PlayFab.ServerModels.RemovePlayerTagRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.RemoveSharedGroupMembersRequest) && _instance.OnServerRemoveSharedGroupMembersRequestEvent != null)
				{
					_instance.OnServerRemoveSharedGroupMembersRequestEvent((PlayFab.ServerModels.RemoveSharedGroupMembersRequest)e.Request);
				}
				else if (type == typeof(ReportPlayerServerRequest) && _instance.OnServerReportPlayerRequestEvent != null)
				{
					_instance.OnServerReportPlayerRequestEvent((ReportPlayerServerRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.RevokeAllBansForUserRequest) && _instance.OnServerRevokeAllBansForUserRequestEvent != null)
				{
					_instance.OnServerRevokeAllBansForUserRequestEvent((PlayFab.ServerModels.RevokeAllBansForUserRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.RevokeBansRequest) && _instance.OnServerRevokeBansRequestEvent != null)
				{
					_instance.OnServerRevokeBansRequestEvent((PlayFab.ServerModels.RevokeBansRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.RevokeInventoryItemRequest) && _instance.OnServerRevokeInventoryItemRequestEvent != null)
				{
					_instance.OnServerRevokeInventoryItemRequestEvent((PlayFab.ServerModels.RevokeInventoryItemRequest)e.Request);
				}
				else if (type == typeof(SendCustomAccountRecoveryEmailRequest) && _instance.OnServerSendCustomAccountRecoveryEmailRequestEvent != null)
				{
					_instance.OnServerSendCustomAccountRecoveryEmailRequestEvent((SendCustomAccountRecoveryEmailRequest)e.Request);
				}
				else if (type == typeof(SendEmailFromTemplateRequest) && _instance.OnServerSendEmailFromTemplateRequestEvent != null)
				{
					_instance.OnServerSendEmailFromTemplateRequestEvent((SendEmailFromTemplateRequest)e.Request);
				}
				else if (type == typeof(SendPushNotificationRequest) && _instance.OnServerSendPushNotificationRequestEvent != null)
				{
					_instance.OnServerSendPushNotificationRequestEvent((SendPushNotificationRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.SetFriendTagsRequest) && _instance.OnServerSetFriendTagsRequestEvent != null)
				{
					_instance.OnServerSetFriendTagsRequestEvent((PlayFab.ServerModels.SetFriendTagsRequest)e.Request);
				}
				else if (type == typeof(SetGameServerInstanceDataRequest) && _instance.OnServerSetGameServerInstanceDataRequestEvent != null)
				{
					_instance.OnServerSetGameServerInstanceDataRequestEvent((SetGameServerInstanceDataRequest)e.Request);
				}
				else if (type == typeof(SetGameServerInstanceStateRequest) && _instance.OnServerSetGameServerInstanceStateRequestEvent != null)
				{
					_instance.OnServerSetGameServerInstanceStateRequestEvent((SetGameServerInstanceStateRequest)e.Request);
				}
				else if (type == typeof(SetGameServerInstanceTagsRequest) && _instance.OnServerSetGameServerInstanceTagsRequestEvent != null)
				{
					_instance.OnServerSetGameServerInstanceTagsRequestEvent((SetGameServerInstanceTagsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.SetPlayerSecretRequest) && _instance.OnServerSetPlayerSecretRequestEvent != null)
				{
					_instance.OnServerSetPlayerSecretRequestEvent((PlayFab.ServerModels.SetPlayerSecretRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.SetPublisherDataRequest) && _instance.OnServerSetPublisherDataRequestEvent != null)
				{
					_instance.OnServerSetPublisherDataRequestEvent((PlayFab.ServerModels.SetPublisherDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.SetTitleDataRequest) && _instance.OnServerSetTitleDataRequestEvent != null)
				{
					_instance.OnServerSetTitleDataRequestEvent((PlayFab.ServerModels.SetTitleDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.SetTitleDataRequest) && _instance.OnServerSetTitleInternalDataRequestEvent != null)
				{
					_instance.OnServerSetTitleInternalDataRequestEvent((PlayFab.ServerModels.SetTitleDataRequest)e.Request);
				}
				else if (type == typeof(SubtractCharacterVirtualCurrencyRequest) && _instance.OnServerSubtractCharacterVirtualCurrencyRequestEvent != null)
				{
					_instance.OnServerSubtractCharacterVirtualCurrencyRequestEvent((SubtractCharacterVirtualCurrencyRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.SubtractUserVirtualCurrencyRequest) && _instance.OnServerSubtractUserVirtualCurrencyRequestEvent != null)
				{
					_instance.OnServerSubtractUserVirtualCurrencyRequestEvent((PlayFab.ServerModels.SubtractUserVirtualCurrencyRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.UnlockContainerInstanceRequest) && _instance.OnServerUnlockContainerInstanceRequestEvent != null)
				{
					_instance.OnServerUnlockContainerInstanceRequestEvent((PlayFab.ServerModels.UnlockContainerInstanceRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.UnlockContainerItemRequest) && _instance.OnServerUnlockContainerItemRequestEvent != null)
				{
					_instance.OnServerUnlockContainerItemRequestEvent((PlayFab.ServerModels.UnlockContainerItemRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.UpdateAvatarUrlRequest) && _instance.OnServerUpdateAvatarUrlRequestEvent != null)
				{
					_instance.OnServerUpdateAvatarUrlRequestEvent((PlayFab.ServerModels.UpdateAvatarUrlRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.UpdateBansRequest) && _instance.OnServerUpdateBansRequestEvent != null)
				{
					_instance.OnServerUpdateBansRequestEvent((PlayFab.ServerModels.UpdateBansRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.UpdateCharacterDataRequest) && _instance.OnServerUpdateCharacterDataRequestEvent != null)
				{
					_instance.OnServerUpdateCharacterDataRequestEvent((PlayFab.ServerModels.UpdateCharacterDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.UpdateCharacterDataRequest) && _instance.OnServerUpdateCharacterInternalDataRequestEvent != null)
				{
					_instance.OnServerUpdateCharacterInternalDataRequestEvent((PlayFab.ServerModels.UpdateCharacterDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.UpdateCharacterDataRequest) && _instance.OnServerUpdateCharacterReadOnlyDataRequestEvent != null)
				{
					_instance.OnServerUpdateCharacterReadOnlyDataRequestEvent((PlayFab.ServerModels.UpdateCharacterDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.UpdateCharacterStatisticsRequest) && _instance.OnServerUpdateCharacterStatisticsRequestEvent != null)
				{
					_instance.OnServerUpdateCharacterStatisticsRequestEvent((PlayFab.ServerModels.UpdateCharacterStatisticsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.UpdatePlayerStatisticsRequest) && _instance.OnServerUpdatePlayerStatisticsRequestEvent != null)
				{
					_instance.OnServerUpdatePlayerStatisticsRequestEvent((PlayFab.ServerModels.UpdatePlayerStatisticsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.UpdateSharedGroupDataRequest) && _instance.OnServerUpdateSharedGroupDataRequestEvent != null)
				{
					_instance.OnServerUpdateSharedGroupDataRequestEvent((PlayFab.ServerModels.UpdateSharedGroupDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.UpdateUserDataRequest) && _instance.OnServerUpdateUserDataRequestEvent != null)
				{
					_instance.OnServerUpdateUserDataRequestEvent((PlayFab.ServerModels.UpdateUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.UpdateUserInternalDataRequest) && _instance.OnServerUpdateUserInternalDataRequestEvent != null)
				{
					_instance.OnServerUpdateUserInternalDataRequestEvent((PlayFab.ServerModels.UpdateUserInternalDataRequest)e.Request);
				}
				else if (type == typeof(UpdateUserInventoryItemDataRequest) && _instance.OnServerUpdateUserInventoryItemCustomDataRequestEvent != null)
				{
					_instance.OnServerUpdateUserInventoryItemCustomDataRequestEvent((UpdateUserInventoryItemDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.UpdateUserDataRequest) && _instance.OnServerUpdateUserPublisherDataRequestEvent != null)
				{
					_instance.OnServerUpdateUserPublisherDataRequestEvent((PlayFab.ServerModels.UpdateUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.UpdateUserInternalDataRequest) && _instance.OnServerUpdateUserPublisherInternalDataRequestEvent != null)
				{
					_instance.OnServerUpdateUserPublisherInternalDataRequestEvent((PlayFab.ServerModels.UpdateUserInternalDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.UpdateUserDataRequest) && _instance.OnServerUpdateUserPublisherReadOnlyDataRequestEvent != null)
				{
					_instance.OnServerUpdateUserPublisherReadOnlyDataRequestEvent((PlayFab.ServerModels.UpdateUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.UpdateUserDataRequest) && _instance.OnServerUpdateUserReadOnlyDataRequestEvent != null)
				{
					_instance.OnServerUpdateUserReadOnlyDataRequestEvent((PlayFab.ServerModels.UpdateUserDataRequest)e.Request);
				}
				else if (type == typeof(WriteServerCharacterEventRequest) && _instance.OnServerWriteCharacterEventRequestEvent != null)
				{
					_instance.OnServerWriteCharacterEventRequestEvent((WriteServerCharacterEventRequest)e.Request);
				}
				else if (type == typeof(WriteServerPlayerEventRequest) && _instance.OnServerWritePlayerEventRequestEvent != null)
				{
					_instance.OnServerWritePlayerEventRequestEvent((WriteServerPlayerEventRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ServerModels.WriteTitleEventRequest) && _instance.OnServerWriteTitleEventRequestEvent != null)
				{
					_instance.OnServerWriteTitleEventRequestEvent((PlayFab.ServerModels.WriteTitleEventRequest)e.Request);
				}
				else if (type == typeof(AcceptTradeRequest) && _instance.OnAcceptTradeRequestEvent != null)
				{
					_instance.OnAcceptTradeRequestEvent((AcceptTradeRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.AddFriendRequest) && _instance.OnAddFriendRequestEvent != null)
				{
					_instance.OnAddFriendRequestEvent((PlayFab.ClientModels.AddFriendRequest)e.Request);
				}
				else if (type == typeof(AddGenericIDRequest) && _instance.OnAddGenericIDRequestEvent != null)
				{
					_instance.OnAddGenericIDRequestEvent((AddGenericIDRequest)e.Request);
				}
				else if (type == typeof(AddOrUpdateContactEmailRequest) && _instance.OnAddOrUpdateContactEmailRequestEvent != null)
				{
					_instance.OnAddOrUpdateContactEmailRequestEvent((AddOrUpdateContactEmailRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.AddSharedGroupMembersRequest) && _instance.OnAddSharedGroupMembersRequestEvent != null)
				{
					_instance.OnAddSharedGroupMembersRequestEvent((PlayFab.ClientModels.AddSharedGroupMembersRequest)e.Request);
				}
				else if (type == typeof(AddUsernamePasswordRequest) && _instance.OnAddUsernamePasswordRequestEvent != null)
				{
					_instance.OnAddUsernamePasswordRequestEvent((AddUsernamePasswordRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.AddUserVirtualCurrencyRequest) && _instance.OnAddUserVirtualCurrencyRequestEvent != null)
				{
					_instance.OnAddUserVirtualCurrencyRequestEvent((PlayFab.ClientModels.AddUserVirtualCurrencyRequest)e.Request);
				}
				else if (type == typeof(AndroidDevicePushNotificationRegistrationRequest) && _instance.OnAndroidDevicePushNotificationRegistrationRequestEvent != null)
				{
					_instance.OnAndroidDevicePushNotificationRegistrationRequestEvent((AndroidDevicePushNotificationRegistrationRequest)e.Request);
				}
				else if (type == typeof(AttributeInstallRequest) && _instance.OnAttributeInstallRequestEvent != null)
				{
					_instance.OnAttributeInstallRequestEvent((AttributeInstallRequest)e.Request);
				}
				else if (type == typeof(CancelTradeRequest) && _instance.OnCancelTradeRequestEvent != null)
				{
					_instance.OnCancelTradeRequestEvent((CancelTradeRequest)e.Request);
				}
				else if (type == typeof(ConfirmPurchaseRequest) && _instance.OnConfirmPurchaseRequestEvent != null)
				{
					_instance.OnConfirmPurchaseRequestEvent((ConfirmPurchaseRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.ConsumeItemRequest) && _instance.OnConsumeItemRequestEvent != null)
				{
					_instance.OnConsumeItemRequestEvent((PlayFab.ClientModels.ConsumeItemRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.CreateSharedGroupRequest) && _instance.OnCreateSharedGroupRequestEvent != null)
				{
					_instance.OnCreateSharedGroupRequestEvent((PlayFab.ClientModels.CreateSharedGroupRequest)e.Request);
				}
				else if (type == typeof(ExecuteCloudScriptRequest) && _instance.OnExecuteCloudScriptRequestEvent != null)
				{
					_instance.OnExecuteCloudScriptRequestEvent((ExecuteCloudScriptRequest)e.Request);
				}
				else if (type == typeof(GetAccountInfoRequest) && _instance.OnGetAccountInfoRequestEvent != null)
				{
					_instance.OnGetAccountInfoRequestEvent((GetAccountInfoRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.ListUsersCharactersRequest) && _instance.OnGetAllUsersCharactersRequestEvent != null)
				{
					_instance.OnGetAllUsersCharactersRequestEvent((PlayFab.ClientModels.ListUsersCharactersRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetCatalogItemsRequest) && _instance.OnGetCatalogItemsRequestEvent != null)
				{
					_instance.OnGetCatalogItemsRequestEvent((PlayFab.ClientModels.GetCatalogItemsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetCharacterDataRequest) && _instance.OnGetCharacterDataRequestEvent != null)
				{
					_instance.OnGetCharacterDataRequestEvent((PlayFab.ClientModels.GetCharacterDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetCharacterInventoryRequest) && _instance.OnGetCharacterInventoryRequestEvent != null)
				{
					_instance.OnGetCharacterInventoryRequestEvent((PlayFab.ClientModels.GetCharacterInventoryRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetCharacterLeaderboardRequest) && _instance.OnGetCharacterLeaderboardRequestEvent != null)
				{
					_instance.OnGetCharacterLeaderboardRequestEvent((PlayFab.ClientModels.GetCharacterLeaderboardRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetCharacterDataRequest) && _instance.OnGetCharacterReadOnlyDataRequestEvent != null)
				{
					_instance.OnGetCharacterReadOnlyDataRequestEvent((PlayFab.ClientModels.GetCharacterDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetCharacterStatisticsRequest) && _instance.OnGetCharacterStatisticsRequestEvent != null)
				{
					_instance.OnGetCharacterStatisticsRequestEvent((PlayFab.ClientModels.GetCharacterStatisticsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetContentDownloadUrlRequest) && _instance.OnGetContentDownloadUrlRequestEvent != null)
				{
					_instance.OnGetContentDownloadUrlRequestEvent((PlayFab.ClientModels.GetContentDownloadUrlRequest)e.Request);
				}
				else if (type == typeof(CurrentGamesRequest) && _instance.OnGetCurrentGamesRequestEvent != null)
				{
					_instance.OnGetCurrentGamesRequestEvent((CurrentGamesRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetFriendLeaderboardRequest) && _instance.OnGetFriendLeaderboardRequestEvent != null)
				{
					_instance.OnGetFriendLeaderboardRequestEvent((PlayFab.ClientModels.GetFriendLeaderboardRequest)e.Request);
				}
				else if (type == typeof(GetFriendLeaderboardAroundPlayerRequest) && _instance.OnGetFriendLeaderboardAroundPlayerRequestEvent != null)
				{
					_instance.OnGetFriendLeaderboardAroundPlayerRequestEvent((GetFriendLeaderboardAroundPlayerRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetFriendsListRequest) && _instance.OnGetFriendsListRequestEvent != null)
				{
					_instance.OnGetFriendsListRequestEvent((PlayFab.ClientModels.GetFriendsListRequest)e.Request);
				}
				else if (type == typeof(GameServerRegionsRequest) && _instance.OnGetGameServerRegionsRequestEvent != null)
				{
					_instance.OnGetGameServerRegionsRequestEvent((GameServerRegionsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetLeaderboardRequest) && _instance.OnGetLeaderboardRequestEvent != null)
				{
					_instance.OnGetLeaderboardRequestEvent((PlayFab.ClientModels.GetLeaderboardRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetLeaderboardAroundCharacterRequest) && _instance.OnGetLeaderboardAroundCharacterRequestEvent != null)
				{
					_instance.OnGetLeaderboardAroundCharacterRequestEvent((PlayFab.ClientModels.GetLeaderboardAroundCharacterRequest)e.Request);
				}
				else if (type == typeof(GetLeaderboardAroundPlayerRequest) && _instance.OnGetLeaderboardAroundPlayerRequestEvent != null)
				{
					_instance.OnGetLeaderboardAroundPlayerRequestEvent((GetLeaderboardAroundPlayerRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetLeaderboardForUsersCharactersRequest) && _instance.OnGetLeaderboardForUserCharactersRequestEvent != null)
				{
					_instance.OnGetLeaderboardForUserCharactersRequestEvent((PlayFab.ClientModels.GetLeaderboardForUsersCharactersRequest)e.Request);
				}
				else if (type == typeof(GetPaymentTokenRequest) && _instance.OnGetPaymentTokenRequestEvent != null)
				{
					_instance.OnGetPaymentTokenRequestEvent((GetPaymentTokenRequest)e.Request);
				}
				else if (type == typeof(GetPhotonAuthenticationTokenRequest) && _instance.OnGetPhotonAuthenticationTokenRequestEvent != null)
				{
					_instance.OnGetPhotonAuthenticationTokenRequestEvent((GetPhotonAuthenticationTokenRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetPlayerCombinedInfoRequest) && _instance.OnGetPlayerCombinedInfoRequestEvent != null)
				{
					_instance.OnGetPlayerCombinedInfoRequestEvent((PlayFab.ClientModels.GetPlayerCombinedInfoRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetPlayerProfileRequest) && _instance.OnGetPlayerProfileRequestEvent != null)
				{
					_instance.OnGetPlayerProfileRequestEvent((PlayFab.ClientModels.GetPlayerProfileRequest)e.Request);
				}
				else if (type == typeof(GetPlayerSegmentsRequest) && _instance.OnGetPlayerSegmentsRequestEvent != null)
				{
					_instance.OnGetPlayerSegmentsRequestEvent((GetPlayerSegmentsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetPlayerStatisticsRequest) && _instance.OnGetPlayerStatisticsRequestEvent != null)
				{
					_instance.OnGetPlayerStatisticsRequestEvent((PlayFab.ClientModels.GetPlayerStatisticsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetPlayerStatisticVersionsRequest) && _instance.OnGetPlayerStatisticVersionsRequestEvent != null)
				{
					_instance.OnGetPlayerStatisticVersionsRequestEvent((PlayFab.ClientModels.GetPlayerStatisticVersionsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetPlayerTagsRequest) && _instance.OnGetPlayerTagsRequestEvent != null)
				{
					_instance.OnGetPlayerTagsRequestEvent((PlayFab.ClientModels.GetPlayerTagsRequest)e.Request);
				}
				else if (type == typeof(GetPlayerTradesRequest) && _instance.OnGetPlayerTradesRequestEvent != null)
				{
					_instance.OnGetPlayerTradesRequestEvent((GetPlayerTradesRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetPlayFabIDsFromFacebookIDsRequest) && _instance.OnGetPlayFabIDsFromFacebookIDsRequestEvent != null)
				{
					_instance.OnGetPlayFabIDsFromFacebookIDsRequestEvent((PlayFab.ClientModels.GetPlayFabIDsFromFacebookIDsRequest)e.Request);
				}
				else if (type == typeof(GetPlayFabIDsFromGameCenterIDsRequest) && _instance.OnGetPlayFabIDsFromGameCenterIDsRequestEvent != null)
				{
					_instance.OnGetPlayFabIDsFromGameCenterIDsRequestEvent((GetPlayFabIDsFromGameCenterIDsRequest)e.Request);
				}
				else if (type == typeof(GetPlayFabIDsFromGenericIDsRequest) && _instance.OnGetPlayFabIDsFromGenericIDsRequestEvent != null)
				{
					_instance.OnGetPlayFabIDsFromGenericIDsRequestEvent((GetPlayFabIDsFromGenericIDsRequest)e.Request);
				}
				else if (type == typeof(GetPlayFabIDsFromGoogleIDsRequest) && _instance.OnGetPlayFabIDsFromGoogleIDsRequestEvent != null)
				{
					_instance.OnGetPlayFabIDsFromGoogleIDsRequestEvent((GetPlayFabIDsFromGoogleIDsRequest)e.Request);
				}
				else if (type == typeof(GetPlayFabIDsFromKongregateIDsRequest) && _instance.OnGetPlayFabIDsFromKongregateIDsRequestEvent != null)
				{
					_instance.OnGetPlayFabIDsFromKongregateIDsRequestEvent((GetPlayFabIDsFromKongregateIDsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetPlayFabIDsFromSteamIDsRequest) && _instance.OnGetPlayFabIDsFromSteamIDsRequestEvent != null)
				{
					_instance.OnGetPlayFabIDsFromSteamIDsRequestEvent((PlayFab.ClientModels.GetPlayFabIDsFromSteamIDsRequest)e.Request);
				}
				else if (type == typeof(GetPlayFabIDsFromTwitchIDsRequest) && _instance.OnGetPlayFabIDsFromTwitchIDsRequestEvent != null)
				{
					_instance.OnGetPlayFabIDsFromTwitchIDsRequestEvent((GetPlayFabIDsFromTwitchIDsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetPublisherDataRequest) && _instance.OnGetPublisherDataRequestEvent != null)
				{
					_instance.OnGetPublisherDataRequestEvent((PlayFab.ClientModels.GetPublisherDataRequest)e.Request);
				}
				else if (type == typeof(GetPurchaseRequest) && _instance.OnGetPurchaseRequestEvent != null)
				{
					_instance.OnGetPurchaseRequestEvent((GetPurchaseRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetSharedGroupDataRequest) && _instance.OnGetSharedGroupDataRequestEvent != null)
				{
					_instance.OnGetSharedGroupDataRequestEvent((PlayFab.ClientModels.GetSharedGroupDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetStoreItemsRequest) && _instance.OnGetStoreItemsRequestEvent != null)
				{
					_instance.OnGetStoreItemsRequestEvent((PlayFab.ClientModels.GetStoreItemsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetTimeRequest) && _instance.OnGetTimeRequestEvent != null)
				{
					_instance.OnGetTimeRequestEvent((PlayFab.ClientModels.GetTimeRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetTitleDataRequest) && _instance.OnGetTitleDataRequestEvent != null)
				{
					_instance.OnGetTitleDataRequestEvent((PlayFab.ClientModels.GetTitleDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetTitleNewsRequest) && _instance.OnGetTitleNewsRequestEvent != null)
				{
					_instance.OnGetTitleNewsRequestEvent((PlayFab.ClientModels.GetTitleNewsRequest)e.Request);
				}
				else if (type == typeof(GetTitlePublicKeyRequest) && _instance.OnGetTitlePublicKeyRequestEvent != null)
				{
					_instance.OnGetTitlePublicKeyRequestEvent((GetTitlePublicKeyRequest)e.Request);
				}
				else if (type == typeof(GetTradeStatusRequest) && _instance.OnGetTradeStatusRequestEvent != null)
				{
					_instance.OnGetTradeStatusRequestEvent((GetTradeStatusRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetUserDataRequest) && _instance.OnGetUserDataRequestEvent != null)
				{
					_instance.OnGetUserDataRequestEvent((PlayFab.ClientModels.GetUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetUserInventoryRequest) && _instance.OnGetUserInventoryRequestEvent != null)
				{
					_instance.OnGetUserInventoryRequestEvent((PlayFab.ClientModels.GetUserInventoryRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetUserDataRequest) && _instance.OnGetUserPublisherDataRequestEvent != null)
				{
					_instance.OnGetUserPublisherDataRequestEvent((PlayFab.ClientModels.GetUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetUserDataRequest) && _instance.OnGetUserPublisherReadOnlyDataRequestEvent != null)
				{
					_instance.OnGetUserPublisherReadOnlyDataRequestEvent((PlayFab.ClientModels.GetUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GetUserDataRequest) && _instance.OnGetUserReadOnlyDataRequestEvent != null)
				{
					_instance.OnGetUserReadOnlyDataRequestEvent((PlayFab.ClientModels.GetUserDataRequest)e.Request);
				}
				else if (type == typeof(GetWindowsHelloChallengeRequest) && _instance.OnGetWindowsHelloChallengeRequestEvent != null)
				{
					_instance.OnGetWindowsHelloChallengeRequestEvent((GetWindowsHelloChallengeRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.GrantCharacterToUserRequest) && _instance.OnGrantCharacterToUserRequestEvent != null)
				{
					_instance.OnGrantCharacterToUserRequestEvent((PlayFab.ClientModels.GrantCharacterToUserRequest)e.Request);
				}
				else if (type == typeof(LinkAndroidDeviceIDRequest) && _instance.OnLinkAndroidDeviceIDRequestEvent != null)
				{
					_instance.OnLinkAndroidDeviceIDRequestEvent((LinkAndroidDeviceIDRequest)e.Request);
				}
				else if (type == typeof(LinkCustomIDRequest) && _instance.OnLinkCustomIDRequestEvent != null)
				{
					_instance.OnLinkCustomIDRequestEvent((LinkCustomIDRequest)e.Request);
				}
				else if (type == typeof(LinkFacebookAccountRequest) && _instance.OnLinkFacebookAccountRequestEvent != null)
				{
					_instance.OnLinkFacebookAccountRequestEvent((LinkFacebookAccountRequest)e.Request);
				}
				else if (type == typeof(LinkGameCenterAccountRequest) && _instance.OnLinkGameCenterAccountRequestEvent != null)
				{
					_instance.OnLinkGameCenterAccountRequestEvent((LinkGameCenterAccountRequest)e.Request);
				}
				else if (type == typeof(LinkGoogleAccountRequest) && _instance.OnLinkGoogleAccountRequestEvent != null)
				{
					_instance.OnLinkGoogleAccountRequestEvent((LinkGoogleAccountRequest)e.Request);
				}
				else if (type == typeof(LinkIOSDeviceIDRequest) && _instance.OnLinkIOSDeviceIDRequestEvent != null)
				{
					_instance.OnLinkIOSDeviceIDRequestEvent((LinkIOSDeviceIDRequest)e.Request);
				}
				else if (type == typeof(LinkKongregateAccountRequest) && _instance.OnLinkKongregateRequestEvent != null)
				{
					_instance.OnLinkKongregateRequestEvent((LinkKongregateAccountRequest)e.Request);
				}
				else if (type == typeof(LinkSteamAccountRequest) && _instance.OnLinkSteamAccountRequestEvent != null)
				{
					_instance.OnLinkSteamAccountRequestEvent((LinkSteamAccountRequest)e.Request);
				}
				else if (type == typeof(LinkTwitchAccountRequest) && _instance.OnLinkTwitchRequestEvent != null)
				{
					_instance.OnLinkTwitchRequestEvent((LinkTwitchAccountRequest)e.Request);
				}
				else if (type == typeof(LinkWindowsHelloAccountRequest) && _instance.OnLinkWindowsHelloRequestEvent != null)
				{
					_instance.OnLinkWindowsHelloRequestEvent((LinkWindowsHelloAccountRequest)e.Request);
				}
				else if (type == typeof(LoginWithAndroidDeviceIDRequest) && _instance.OnLoginWithAndroidDeviceIDRequestEvent != null)
				{
					_instance.OnLoginWithAndroidDeviceIDRequestEvent((LoginWithAndroidDeviceIDRequest)e.Request);
				}
				else if (type == typeof(LoginWithCustomIDRequest) && _instance.OnLoginWithCustomIDRequestEvent != null)
				{
					_instance.OnLoginWithCustomIDRequestEvent((LoginWithCustomIDRequest)e.Request);
				}
				else if (type == typeof(LoginWithEmailAddressRequest) && _instance.OnLoginWithEmailAddressRequestEvent != null)
				{
					_instance.OnLoginWithEmailAddressRequestEvent((LoginWithEmailAddressRequest)e.Request);
				}
				else if (type == typeof(LoginWithFacebookRequest) && _instance.OnLoginWithFacebookRequestEvent != null)
				{
					_instance.OnLoginWithFacebookRequestEvent((LoginWithFacebookRequest)e.Request);
				}
				else if (type == typeof(LoginWithGameCenterRequest) && _instance.OnLoginWithGameCenterRequestEvent != null)
				{
					_instance.OnLoginWithGameCenterRequestEvent((LoginWithGameCenterRequest)e.Request);
				}
				else if (type == typeof(LoginWithGoogleAccountRequest) && _instance.OnLoginWithGoogleAccountRequestEvent != null)
				{
					_instance.OnLoginWithGoogleAccountRequestEvent((LoginWithGoogleAccountRequest)e.Request);
				}
				else if (type == typeof(LoginWithIOSDeviceIDRequest) && _instance.OnLoginWithIOSDeviceIDRequestEvent != null)
				{
					_instance.OnLoginWithIOSDeviceIDRequestEvent((LoginWithIOSDeviceIDRequest)e.Request);
				}
				else if (type == typeof(LoginWithKongregateRequest) && _instance.OnLoginWithKongregateRequestEvent != null)
				{
					_instance.OnLoginWithKongregateRequestEvent((LoginWithKongregateRequest)e.Request);
				}
				else if (type == typeof(LoginWithPlayFabRequest) && _instance.OnLoginWithPlayFabRequestEvent != null)
				{
					_instance.OnLoginWithPlayFabRequestEvent((LoginWithPlayFabRequest)e.Request);
				}
				else if (type == typeof(LoginWithSteamRequest) && _instance.OnLoginWithSteamRequestEvent != null)
				{
					_instance.OnLoginWithSteamRequestEvent((LoginWithSteamRequest)e.Request);
				}
				else if (type == typeof(LoginWithTwitchRequest) && _instance.OnLoginWithTwitchRequestEvent != null)
				{
					_instance.OnLoginWithTwitchRequestEvent((LoginWithTwitchRequest)e.Request);
				}
				else if (type == typeof(LoginWithWindowsHelloRequest) && _instance.OnLoginWithWindowsHelloRequestEvent != null)
				{
					_instance.OnLoginWithWindowsHelloRequestEvent((LoginWithWindowsHelloRequest)e.Request);
				}
				else if (type == typeof(MatchmakeRequest) && _instance.OnMatchmakeRequestEvent != null)
				{
					_instance.OnMatchmakeRequestEvent((MatchmakeRequest)e.Request);
				}
				else if (type == typeof(OpenTradeRequest) && _instance.OnOpenTradeRequestEvent != null)
				{
					_instance.OnOpenTradeRequestEvent((OpenTradeRequest)e.Request);
				}
				else if (type == typeof(PayForPurchaseRequest) && _instance.OnPayForPurchaseRequestEvent != null)
				{
					_instance.OnPayForPurchaseRequestEvent((PayForPurchaseRequest)e.Request);
				}
				else if (type == typeof(PurchaseItemRequest) && _instance.OnPurchaseItemRequestEvent != null)
				{
					_instance.OnPurchaseItemRequestEvent((PurchaseItemRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.RedeemCouponRequest) && _instance.OnRedeemCouponRequestEvent != null)
				{
					_instance.OnRedeemCouponRequestEvent((PlayFab.ClientModels.RedeemCouponRequest)e.Request);
				}
				else if (type == typeof(RegisterForIOSPushNotificationRequest) && _instance.OnRegisterForIOSPushNotificationRequestEvent != null)
				{
					_instance.OnRegisterForIOSPushNotificationRequestEvent((RegisterForIOSPushNotificationRequest)e.Request);
				}
				else if (type == typeof(RegisterPlayFabUserRequest) && _instance.OnRegisterPlayFabUserRequestEvent != null)
				{
					_instance.OnRegisterPlayFabUserRequestEvent((RegisterPlayFabUserRequest)e.Request);
				}
				else if (type == typeof(RegisterWithWindowsHelloRequest) && _instance.OnRegisterWithWindowsHelloRequestEvent != null)
				{
					_instance.OnRegisterWithWindowsHelloRequestEvent((RegisterWithWindowsHelloRequest)e.Request);
				}
				else if (type == typeof(RemoveContactEmailRequest) && _instance.OnRemoveContactEmailRequestEvent != null)
				{
					_instance.OnRemoveContactEmailRequestEvent((RemoveContactEmailRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.RemoveFriendRequest) && _instance.OnRemoveFriendRequestEvent != null)
				{
					_instance.OnRemoveFriendRequestEvent((PlayFab.ClientModels.RemoveFriendRequest)e.Request);
				}
				else if (type == typeof(RemoveGenericIDRequest) && _instance.OnRemoveGenericIDRequestEvent != null)
				{
					_instance.OnRemoveGenericIDRequestEvent((RemoveGenericIDRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.RemoveSharedGroupMembersRequest) && _instance.OnRemoveSharedGroupMembersRequestEvent != null)
				{
					_instance.OnRemoveSharedGroupMembersRequestEvent((PlayFab.ClientModels.RemoveSharedGroupMembersRequest)e.Request);
				}
				else if (type == typeof(DeviceInfoRequest) && _instance.OnReportDeviceInfoRequestEvent != null)
				{
					_instance.OnReportDeviceInfoRequestEvent((DeviceInfoRequest)e.Request);
				}
				else if (type == typeof(ReportPlayerClientRequest) && _instance.OnReportPlayerRequestEvent != null)
				{
					_instance.OnReportPlayerRequestEvent((ReportPlayerClientRequest)e.Request);
				}
				else if (type == typeof(RestoreIOSPurchasesRequest) && _instance.OnRestoreIOSPurchasesRequestEvent != null)
				{
					_instance.OnRestoreIOSPurchasesRequestEvent((RestoreIOSPurchasesRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.SendAccountRecoveryEmailRequest) && _instance.OnSendAccountRecoveryEmailRequestEvent != null)
				{
					_instance.OnSendAccountRecoveryEmailRequestEvent((PlayFab.ClientModels.SendAccountRecoveryEmailRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.SetFriendTagsRequest) && _instance.OnSetFriendTagsRequestEvent != null)
				{
					_instance.OnSetFriendTagsRequestEvent((PlayFab.ClientModels.SetFriendTagsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.SetPlayerSecretRequest) && _instance.OnSetPlayerSecretRequestEvent != null)
				{
					_instance.OnSetPlayerSecretRequestEvent((PlayFab.ClientModels.SetPlayerSecretRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.StartGameRequest) && _instance.OnStartGameRequestEvent != null)
				{
					_instance.OnStartGameRequestEvent((PlayFab.ClientModels.StartGameRequest)e.Request);
				}
				else if (type == typeof(StartPurchaseRequest) && _instance.OnStartPurchaseRequestEvent != null)
				{
					_instance.OnStartPurchaseRequestEvent((StartPurchaseRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.SubtractUserVirtualCurrencyRequest) && _instance.OnSubtractUserVirtualCurrencyRequestEvent != null)
				{
					_instance.OnSubtractUserVirtualCurrencyRequestEvent((PlayFab.ClientModels.SubtractUserVirtualCurrencyRequest)e.Request);
				}
				else if (type == typeof(UnlinkAndroidDeviceIDRequest) && _instance.OnUnlinkAndroidDeviceIDRequestEvent != null)
				{
					_instance.OnUnlinkAndroidDeviceIDRequestEvent((UnlinkAndroidDeviceIDRequest)e.Request);
				}
				else if (type == typeof(UnlinkCustomIDRequest) && _instance.OnUnlinkCustomIDRequestEvent != null)
				{
					_instance.OnUnlinkCustomIDRequestEvent((UnlinkCustomIDRequest)e.Request);
				}
				else if (type == typeof(UnlinkFacebookAccountRequest) && _instance.OnUnlinkFacebookAccountRequestEvent != null)
				{
					_instance.OnUnlinkFacebookAccountRequestEvent((UnlinkFacebookAccountRequest)e.Request);
				}
				else if (type == typeof(UnlinkGameCenterAccountRequest) && _instance.OnUnlinkGameCenterAccountRequestEvent != null)
				{
					_instance.OnUnlinkGameCenterAccountRequestEvent((UnlinkGameCenterAccountRequest)e.Request);
				}
				else if (type == typeof(UnlinkGoogleAccountRequest) && _instance.OnUnlinkGoogleAccountRequestEvent != null)
				{
					_instance.OnUnlinkGoogleAccountRequestEvent((UnlinkGoogleAccountRequest)e.Request);
				}
				else if (type == typeof(UnlinkIOSDeviceIDRequest) && _instance.OnUnlinkIOSDeviceIDRequestEvent != null)
				{
					_instance.OnUnlinkIOSDeviceIDRequestEvent((UnlinkIOSDeviceIDRequest)e.Request);
				}
				else if (type == typeof(UnlinkKongregateAccountRequest) && _instance.OnUnlinkKongregateRequestEvent != null)
				{
					_instance.OnUnlinkKongregateRequestEvent((UnlinkKongregateAccountRequest)e.Request);
				}
				else if (type == typeof(UnlinkSteamAccountRequest) && _instance.OnUnlinkSteamAccountRequestEvent != null)
				{
					_instance.OnUnlinkSteamAccountRequestEvent((UnlinkSteamAccountRequest)e.Request);
				}
				else if (type == typeof(UnlinkTwitchAccountRequest) && _instance.OnUnlinkTwitchRequestEvent != null)
				{
					_instance.OnUnlinkTwitchRequestEvent((UnlinkTwitchAccountRequest)e.Request);
				}
				else if (type == typeof(UnlinkWindowsHelloAccountRequest) && _instance.OnUnlinkWindowsHelloRequestEvent != null)
				{
					_instance.OnUnlinkWindowsHelloRequestEvent((UnlinkWindowsHelloAccountRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.UnlockContainerInstanceRequest) && _instance.OnUnlockContainerInstanceRequestEvent != null)
				{
					_instance.OnUnlockContainerInstanceRequestEvent((PlayFab.ClientModels.UnlockContainerInstanceRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.UnlockContainerItemRequest) && _instance.OnUnlockContainerItemRequestEvent != null)
				{
					_instance.OnUnlockContainerItemRequestEvent((PlayFab.ClientModels.UnlockContainerItemRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.UpdateAvatarUrlRequest) && _instance.OnUpdateAvatarUrlRequestEvent != null)
				{
					_instance.OnUpdateAvatarUrlRequestEvent((PlayFab.ClientModels.UpdateAvatarUrlRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.UpdateCharacterDataRequest) && _instance.OnUpdateCharacterDataRequestEvent != null)
				{
					_instance.OnUpdateCharacterDataRequestEvent((PlayFab.ClientModels.UpdateCharacterDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.UpdateCharacterStatisticsRequest) && _instance.OnUpdateCharacterStatisticsRequestEvent != null)
				{
					_instance.OnUpdateCharacterStatisticsRequestEvent((PlayFab.ClientModels.UpdateCharacterStatisticsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.UpdatePlayerStatisticsRequest) && _instance.OnUpdatePlayerStatisticsRequestEvent != null)
				{
					_instance.OnUpdatePlayerStatisticsRequestEvent((PlayFab.ClientModels.UpdatePlayerStatisticsRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.UpdateSharedGroupDataRequest) && _instance.OnUpdateSharedGroupDataRequestEvent != null)
				{
					_instance.OnUpdateSharedGroupDataRequestEvent((PlayFab.ClientModels.UpdateSharedGroupDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.UpdateUserDataRequest) && _instance.OnUpdateUserDataRequestEvent != null)
				{
					_instance.OnUpdateUserDataRequestEvent((PlayFab.ClientModels.UpdateUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.UpdateUserDataRequest) && _instance.OnUpdateUserPublisherDataRequestEvent != null)
				{
					_instance.OnUpdateUserPublisherDataRequestEvent((PlayFab.ClientModels.UpdateUserDataRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.UpdateUserTitleDisplayNameRequest) && _instance.OnUpdateUserTitleDisplayNameRequestEvent != null)
				{
					_instance.OnUpdateUserTitleDisplayNameRequestEvent((PlayFab.ClientModels.UpdateUserTitleDisplayNameRequest)e.Request);
				}
				else if (type == typeof(ValidateAmazonReceiptRequest) && _instance.OnValidateAmazonIAPReceiptRequestEvent != null)
				{
					_instance.OnValidateAmazonIAPReceiptRequestEvent((ValidateAmazonReceiptRequest)e.Request);
				}
				else if (type == typeof(ValidateGooglePlayPurchaseRequest) && _instance.OnValidateGooglePlayPurchaseRequestEvent != null)
				{
					_instance.OnValidateGooglePlayPurchaseRequestEvent((ValidateGooglePlayPurchaseRequest)e.Request);
				}
				else if (type == typeof(ValidateIOSReceiptRequest) && _instance.OnValidateIOSReceiptRequestEvent != null)
				{
					_instance.OnValidateIOSReceiptRequestEvent((ValidateIOSReceiptRequest)e.Request);
				}
				else if (type == typeof(ValidateWindowsReceiptRequest) && _instance.OnValidateWindowsStoreReceiptRequestEvent != null)
				{
					_instance.OnValidateWindowsStoreReceiptRequestEvent((ValidateWindowsReceiptRequest)e.Request);
				}
				else if (type == typeof(WriteClientCharacterEventRequest) && _instance.OnWriteCharacterEventRequestEvent != null)
				{
					_instance.OnWriteCharacterEventRequestEvent((WriteClientCharacterEventRequest)e.Request);
				}
				else if (type == typeof(WriteClientPlayerEventRequest) && _instance.OnWritePlayerEventRequestEvent != null)
				{
					_instance.OnWritePlayerEventRequestEvent((WriteClientPlayerEventRequest)e.Request);
				}
				else if (type == typeof(PlayFab.ClientModels.WriteTitleEventRequest) && _instance.OnWriteTitleEventRequestEvent != null)
				{
					_instance.OnWriteTitleEventRequestEvent((PlayFab.ClientModels.WriteTitleEventRequest)e.Request);
				}
			}
			else
			{
				Type type2 = e.Result.GetType();
				if (type2 == typeof(PlayFab.AdminModels.EmptyResult) && _instance.OnAdminAbortTaskInstanceResultEvent != null)
				{
					_instance.OnAdminAbortTaskInstanceResultEvent((PlayFab.AdminModels.EmptyResult)e.Result);
				}
				else if (type2 == typeof(AddNewsResult) && _instance.OnAdminAddNewsResultEvent != null)
				{
					_instance.OnAdminAddNewsResultEvent((AddNewsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.AddPlayerTagResult) && _instance.OnAdminAddPlayerTagResultEvent != null)
				{
					_instance.OnAdminAddPlayerTagResultEvent((PlayFab.AdminModels.AddPlayerTagResult)e.Result);
				}
				else if (type2 == typeof(AddServerBuildResult) && _instance.OnAdminAddServerBuildResultEvent != null)
				{
					_instance.OnAdminAddServerBuildResultEvent((AddServerBuildResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.ModifyUserVirtualCurrencyResult) && _instance.OnAdminAddUserVirtualCurrencyResultEvent != null)
				{
					_instance.OnAdminAddUserVirtualCurrencyResultEvent((PlayFab.AdminModels.ModifyUserVirtualCurrencyResult)e.Result);
				}
				else if (type2 == typeof(BlankResult) && _instance.OnAdminAddVirtualCurrencyTypesResultEvent != null)
				{
					_instance.OnAdminAddVirtualCurrencyTypesResultEvent((BlankResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.BanUsersResult) && _instance.OnAdminBanUsersResultEvent != null)
				{
					_instance.OnAdminBanUsersResultEvent((PlayFab.AdminModels.BanUsersResult)e.Result);
				}
				else if (type2 == typeof(CheckLimitedEditionItemAvailabilityResult) && _instance.OnAdminCheckLimitedEditionItemAvailabilityResultEvent != null)
				{
					_instance.OnAdminCheckLimitedEditionItemAvailabilityResultEvent((CheckLimitedEditionItemAvailabilityResult)e.Result);
				}
				else if (type2 == typeof(CreateTaskResult) && _instance.OnAdminCreateActionsOnPlayersInSegmentTaskResultEvent != null)
				{
					_instance.OnAdminCreateActionsOnPlayersInSegmentTaskResultEvent((CreateTaskResult)e.Result);
				}
				else if (type2 == typeof(CreateTaskResult) && _instance.OnAdminCreateCloudScriptTaskResultEvent != null)
				{
					_instance.OnAdminCreateCloudScriptTaskResultEvent((CreateTaskResult)e.Result);
				}
				else if (type2 == typeof(CreatePlayerSharedSecretResult) && _instance.OnAdminCreatePlayerSharedSecretResultEvent != null)
				{
					_instance.OnAdminCreatePlayerSharedSecretResultEvent((CreatePlayerSharedSecretResult)e.Result);
				}
				else if (type2 == typeof(CreatePlayerStatisticDefinitionResult) && _instance.OnAdminCreatePlayerStatisticDefinitionResultEvent != null)
				{
					_instance.OnAdminCreatePlayerStatisticDefinitionResultEvent((CreatePlayerStatisticDefinitionResult)e.Result);
				}
				else if (type2 == typeof(BlankResult) && _instance.OnAdminDeleteContentResultEvent != null)
				{
					_instance.OnAdminDeleteContentResultEvent((BlankResult)e.Result);
				}
				else if (type2 == typeof(DeletePlayerResult) && _instance.OnAdminDeletePlayerResultEvent != null)
				{
					_instance.OnAdminDeletePlayerResultEvent((DeletePlayerResult)e.Result);
				}
				else if (type2 == typeof(DeletePlayerSharedSecretResult) && _instance.OnAdminDeletePlayerSharedSecretResultEvent != null)
				{
					_instance.OnAdminDeletePlayerSharedSecretResultEvent((DeletePlayerSharedSecretResult)e.Result);
				}
				else if (type2 == typeof(DeleteStoreResult) && _instance.OnAdminDeleteStoreResultEvent != null)
				{
					_instance.OnAdminDeleteStoreResultEvent((DeleteStoreResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.EmptyResult) && _instance.OnAdminDeleteTaskResultEvent != null)
				{
					_instance.OnAdminDeleteTaskResultEvent((PlayFab.AdminModels.EmptyResult)e.Result);
				}
				else if (type2 == typeof(DeleteTitleResult) && _instance.OnAdminDeleteTitleResultEvent != null)
				{
					_instance.OnAdminDeleteTitleResultEvent((DeleteTitleResult)e.Result);
				}
				else if (type2 == typeof(GetActionsOnPlayersInSegmentTaskInstanceResult) && _instance.OnAdminGetActionsOnPlayersInSegmentTaskInstanceResultEvent != null)
				{
					_instance.OnAdminGetActionsOnPlayersInSegmentTaskInstanceResultEvent((GetActionsOnPlayersInSegmentTaskInstanceResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetAllSegmentsResult) && _instance.OnAdminGetAllSegmentsResultEvent != null)
				{
					_instance.OnAdminGetAllSegmentsResultEvent((PlayFab.AdminModels.GetAllSegmentsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetCatalogItemsResult) && _instance.OnAdminGetCatalogItemsResultEvent != null)
				{
					_instance.OnAdminGetCatalogItemsResultEvent((PlayFab.AdminModels.GetCatalogItemsResult)e.Result);
				}
				else if (type2 == typeof(GetCloudScriptRevisionResult) && _instance.OnAdminGetCloudScriptRevisionResultEvent != null)
				{
					_instance.OnAdminGetCloudScriptRevisionResultEvent((GetCloudScriptRevisionResult)e.Result);
				}
				else if (type2 == typeof(GetCloudScriptTaskInstanceResult) && _instance.OnAdminGetCloudScriptTaskInstanceResultEvent != null)
				{
					_instance.OnAdminGetCloudScriptTaskInstanceResultEvent((GetCloudScriptTaskInstanceResult)e.Result);
				}
				else if (type2 == typeof(GetCloudScriptVersionsResult) && _instance.OnAdminGetCloudScriptVersionsResultEvent != null)
				{
					_instance.OnAdminGetCloudScriptVersionsResultEvent((GetCloudScriptVersionsResult)e.Result);
				}
				else if (type2 == typeof(GetContentListResult) && _instance.OnAdminGetContentListResultEvent != null)
				{
					_instance.OnAdminGetContentListResultEvent((GetContentListResult)e.Result);
				}
				else if (type2 == typeof(GetContentUploadUrlResult) && _instance.OnAdminGetContentUploadUrlResultEvent != null)
				{
					_instance.OnAdminGetContentUploadUrlResultEvent((GetContentUploadUrlResult)e.Result);
				}
				else if (type2 == typeof(GetDataReportResult) && _instance.OnAdminGetDataReportResultEvent != null)
				{
					_instance.OnAdminGetDataReportResultEvent((GetDataReportResult)e.Result);
				}
				else if (type2 == typeof(GetMatchmakerGameInfoResult) && _instance.OnAdminGetMatchmakerGameInfoResultEvent != null)
				{
					_instance.OnAdminGetMatchmakerGameInfoResultEvent((GetMatchmakerGameInfoResult)e.Result);
				}
				else if (type2 == typeof(GetMatchmakerGameModesResult) && _instance.OnAdminGetMatchmakerGameModesResultEvent != null)
				{
					_instance.OnAdminGetMatchmakerGameModesResultEvent((GetMatchmakerGameModesResult)e.Result);
				}
				else if (type2 == typeof(GetPlayerIdFromAuthTokenResult) && _instance.OnAdminGetPlayerIdFromAuthTokenResultEvent != null)
				{
					_instance.OnAdminGetPlayerIdFromAuthTokenResultEvent((GetPlayerIdFromAuthTokenResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetPlayerProfileResult) && _instance.OnAdminGetPlayerProfileResultEvent != null)
				{
					_instance.OnAdminGetPlayerProfileResultEvent((PlayFab.AdminModels.GetPlayerProfileResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetPlayerSegmentsResult) && _instance.OnAdminGetPlayerSegmentsResultEvent != null)
				{
					_instance.OnAdminGetPlayerSegmentsResultEvent((PlayFab.AdminModels.GetPlayerSegmentsResult)e.Result);
				}
				else if (type2 == typeof(GetPlayerSharedSecretsResult) && _instance.OnAdminGetPlayerSharedSecretsResultEvent != null)
				{
					_instance.OnAdminGetPlayerSharedSecretsResultEvent((GetPlayerSharedSecretsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetPlayersInSegmentResult) && _instance.OnAdminGetPlayersInSegmentResultEvent != null)
				{
					_instance.OnAdminGetPlayersInSegmentResultEvent((PlayFab.AdminModels.GetPlayersInSegmentResult)e.Result);
				}
				else if (type2 == typeof(GetPlayerStatisticDefinitionsResult) && _instance.OnAdminGetPlayerStatisticDefinitionsResultEvent != null)
				{
					_instance.OnAdminGetPlayerStatisticDefinitionsResultEvent((GetPlayerStatisticDefinitionsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetPlayerStatisticVersionsResult) && _instance.OnAdminGetPlayerStatisticVersionsResultEvent != null)
				{
					_instance.OnAdminGetPlayerStatisticVersionsResultEvent((PlayFab.AdminModels.GetPlayerStatisticVersionsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetPlayerTagsResult) && _instance.OnAdminGetPlayerTagsResultEvent != null)
				{
					_instance.OnAdminGetPlayerTagsResultEvent((PlayFab.AdminModels.GetPlayerTagsResult)e.Result);
				}
				else if (type2 == typeof(GetPolicyResponse) && _instance.OnAdminGetPolicyResultEvent != null)
				{
					_instance.OnAdminGetPolicyResultEvent((GetPolicyResponse)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetPublisherDataResult) && _instance.OnAdminGetPublisherDataResultEvent != null)
				{
					_instance.OnAdminGetPublisherDataResultEvent((PlayFab.AdminModels.GetPublisherDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetRandomResultTablesResult) && _instance.OnAdminGetRandomResultTablesResultEvent != null)
				{
					_instance.OnAdminGetRandomResultTablesResultEvent((PlayFab.AdminModels.GetRandomResultTablesResult)e.Result);
				}
				else if (type2 == typeof(GetServerBuildInfoResult) && _instance.OnAdminGetServerBuildInfoResultEvent != null)
				{
					_instance.OnAdminGetServerBuildInfoResultEvent((GetServerBuildInfoResult)e.Result);
				}
				else if (type2 == typeof(GetServerBuildUploadURLResult) && _instance.OnAdminGetServerBuildUploadUrlResultEvent != null)
				{
					_instance.OnAdminGetServerBuildUploadUrlResultEvent((GetServerBuildUploadURLResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetStoreItemsResult) && _instance.OnAdminGetStoreItemsResultEvent != null)
				{
					_instance.OnAdminGetStoreItemsResultEvent((PlayFab.AdminModels.GetStoreItemsResult)e.Result);
				}
				else if (type2 == typeof(GetTaskInstancesResult) && _instance.OnAdminGetTaskInstancesResultEvent != null)
				{
					_instance.OnAdminGetTaskInstancesResultEvent((GetTaskInstancesResult)e.Result);
				}
				else if (type2 == typeof(GetTasksResult) && _instance.OnAdminGetTasksResultEvent != null)
				{
					_instance.OnAdminGetTasksResultEvent((GetTasksResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetTitleDataResult) && _instance.OnAdminGetTitleDataResultEvent != null)
				{
					_instance.OnAdminGetTitleDataResultEvent((PlayFab.AdminModels.GetTitleDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetTitleDataResult) && _instance.OnAdminGetTitleInternalDataResultEvent != null)
				{
					_instance.OnAdminGetTitleInternalDataResultEvent((PlayFab.AdminModels.GetTitleDataResult)e.Result);
				}
				else if (type2 == typeof(LookupUserAccountInfoResult) && _instance.OnAdminGetUserAccountInfoResultEvent != null)
				{
					_instance.OnAdminGetUserAccountInfoResultEvent((LookupUserAccountInfoResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetUserBansResult) && _instance.OnAdminGetUserBansResultEvent != null)
				{
					_instance.OnAdminGetUserBansResultEvent((PlayFab.AdminModels.GetUserBansResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetUserDataResult) && _instance.OnAdminGetUserDataResultEvent != null)
				{
					_instance.OnAdminGetUserDataResultEvent((PlayFab.AdminModels.GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetUserDataResult) && _instance.OnAdminGetUserInternalDataResultEvent != null)
				{
					_instance.OnAdminGetUserInternalDataResultEvent((PlayFab.AdminModels.GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetUserInventoryResult) && _instance.OnAdminGetUserInventoryResultEvent != null)
				{
					_instance.OnAdminGetUserInventoryResultEvent((PlayFab.AdminModels.GetUserInventoryResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetUserDataResult) && _instance.OnAdminGetUserPublisherDataResultEvent != null)
				{
					_instance.OnAdminGetUserPublisherDataResultEvent((PlayFab.AdminModels.GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetUserDataResult) && _instance.OnAdminGetUserPublisherInternalDataResultEvent != null)
				{
					_instance.OnAdminGetUserPublisherInternalDataResultEvent((PlayFab.AdminModels.GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetUserDataResult) && _instance.OnAdminGetUserPublisherReadOnlyDataResultEvent != null)
				{
					_instance.OnAdminGetUserPublisherReadOnlyDataResultEvent((PlayFab.AdminModels.GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GetUserDataResult) && _instance.OnAdminGetUserReadOnlyDataResultEvent != null)
				{
					_instance.OnAdminGetUserReadOnlyDataResultEvent((PlayFab.AdminModels.GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.GrantItemsToUsersResult) && _instance.OnAdminGrantItemsToUsersResultEvent != null)
				{
					_instance.OnAdminGrantItemsToUsersResultEvent((PlayFab.AdminModels.GrantItemsToUsersResult)e.Result);
				}
				else if (type2 == typeof(IncrementLimitedEditionItemAvailabilityResult) && _instance.OnAdminIncrementLimitedEditionItemAvailabilityResultEvent != null)
				{
					_instance.OnAdminIncrementLimitedEditionItemAvailabilityResultEvent((IncrementLimitedEditionItemAvailabilityResult)e.Result);
				}
				else if (type2 == typeof(IncrementPlayerStatisticVersionResult) && _instance.OnAdminIncrementPlayerStatisticVersionResultEvent != null)
				{
					_instance.OnAdminIncrementPlayerStatisticVersionResultEvent((IncrementPlayerStatisticVersionResult)e.Result);
				}
				else if (type2 == typeof(ListBuildsResult) && _instance.OnAdminListServerBuildsResultEvent != null)
				{
					_instance.OnAdminListServerBuildsResultEvent((ListBuildsResult)e.Result);
				}
				else if (type2 == typeof(ListVirtualCurrencyTypesResult) && _instance.OnAdminListVirtualCurrencyTypesResultEvent != null)
				{
					_instance.OnAdminListVirtualCurrencyTypesResultEvent((ListVirtualCurrencyTypesResult)e.Result);
				}
				else if (type2 == typeof(ModifyMatchmakerGameModesResult) && _instance.OnAdminModifyMatchmakerGameModesResultEvent != null)
				{
					_instance.OnAdminModifyMatchmakerGameModesResultEvent((ModifyMatchmakerGameModesResult)e.Result);
				}
				else if (type2 == typeof(ModifyServerBuildResult) && _instance.OnAdminModifyServerBuildResultEvent != null)
				{
					_instance.OnAdminModifyServerBuildResultEvent((ModifyServerBuildResult)e.Result);
				}
				else if (type2 == typeof(RefundPurchaseResponse) && _instance.OnAdminRefundPurchaseResultEvent != null)
				{
					_instance.OnAdminRefundPurchaseResultEvent((RefundPurchaseResponse)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.RemovePlayerTagResult) && _instance.OnAdminRemovePlayerTagResultEvent != null)
				{
					_instance.OnAdminRemovePlayerTagResultEvent((PlayFab.AdminModels.RemovePlayerTagResult)e.Result);
				}
				else if (type2 == typeof(RemoveServerBuildResult) && _instance.OnAdminRemoveServerBuildResultEvent != null)
				{
					_instance.OnAdminRemoveServerBuildResultEvent((RemoveServerBuildResult)e.Result);
				}
				else if (type2 == typeof(BlankResult) && _instance.OnAdminRemoveVirtualCurrencyTypesResultEvent != null)
				{
					_instance.OnAdminRemoveVirtualCurrencyTypesResultEvent((BlankResult)e.Result);
				}
				else if (type2 == typeof(ResetCharacterStatisticsResult) && _instance.OnAdminResetCharacterStatisticsResultEvent != null)
				{
					_instance.OnAdminResetCharacterStatisticsResultEvent((ResetCharacterStatisticsResult)e.Result);
				}
				else if (type2 == typeof(ResetPasswordResult) && _instance.OnAdminResetPasswordResultEvent != null)
				{
					_instance.OnAdminResetPasswordResultEvent((ResetPasswordResult)e.Result);
				}
				else if (type2 == typeof(ResetUserStatisticsResult) && _instance.OnAdminResetUserStatisticsResultEvent != null)
				{
					_instance.OnAdminResetUserStatisticsResultEvent((ResetUserStatisticsResult)e.Result);
				}
				else if (type2 == typeof(ResolvePurchaseDisputeResponse) && _instance.OnAdminResolvePurchaseDisputeResultEvent != null)
				{
					_instance.OnAdminResolvePurchaseDisputeResultEvent((ResolvePurchaseDisputeResponse)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.RevokeAllBansForUserResult) && _instance.OnAdminRevokeAllBansForUserResultEvent != null)
				{
					_instance.OnAdminRevokeAllBansForUserResultEvent((PlayFab.AdminModels.RevokeAllBansForUserResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.RevokeBansResult) && _instance.OnAdminRevokeBansResultEvent != null)
				{
					_instance.OnAdminRevokeBansResultEvent((PlayFab.AdminModels.RevokeBansResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.RevokeInventoryResult) && _instance.OnAdminRevokeInventoryItemResultEvent != null)
				{
					_instance.OnAdminRevokeInventoryItemResultEvent((PlayFab.AdminModels.RevokeInventoryResult)e.Result);
				}
				else if (type2 == typeof(RunTaskResult) && _instance.OnAdminRunTaskResultEvent != null)
				{
					_instance.OnAdminRunTaskResultEvent((RunTaskResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.SendAccountRecoveryEmailResult) && _instance.OnAdminSendAccountRecoveryEmailResultEvent != null)
				{
					_instance.OnAdminSendAccountRecoveryEmailResultEvent((PlayFab.AdminModels.SendAccountRecoveryEmailResult)e.Result);
				}
				else if (type2 == typeof(UpdateCatalogItemsResult) && _instance.OnAdminSetCatalogItemsResultEvent != null)
				{
					_instance.OnAdminSetCatalogItemsResultEvent((UpdateCatalogItemsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.SetPlayerSecretResult) && _instance.OnAdminSetPlayerSecretResultEvent != null)
				{
					_instance.OnAdminSetPlayerSecretResultEvent((PlayFab.AdminModels.SetPlayerSecretResult)e.Result);
				}
				else if (type2 == typeof(SetPublishedRevisionResult) && _instance.OnAdminSetPublishedRevisionResultEvent != null)
				{
					_instance.OnAdminSetPublishedRevisionResultEvent((SetPublishedRevisionResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.SetPublisherDataResult) && _instance.OnAdminSetPublisherDataResultEvent != null)
				{
					_instance.OnAdminSetPublisherDataResultEvent((PlayFab.AdminModels.SetPublisherDataResult)e.Result);
				}
				else if (type2 == typeof(UpdateStoreItemsResult) && _instance.OnAdminSetStoreItemsResultEvent != null)
				{
					_instance.OnAdminSetStoreItemsResultEvent((UpdateStoreItemsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.SetTitleDataResult) && _instance.OnAdminSetTitleDataResultEvent != null)
				{
					_instance.OnAdminSetTitleDataResultEvent((PlayFab.AdminModels.SetTitleDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.SetTitleDataResult) && _instance.OnAdminSetTitleInternalDataResultEvent != null)
				{
					_instance.OnAdminSetTitleInternalDataResultEvent((PlayFab.AdminModels.SetTitleDataResult)e.Result);
				}
				else if (type2 == typeof(SetupPushNotificationResult) && _instance.OnAdminSetupPushNotificationResultEvent != null)
				{
					_instance.OnAdminSetupPushNotificationResultEvent((SetupPushNotificationResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.ModifyUserVirtualCurrencyResult) && _instance.OnAdminSubtractUserVirtualCurrencyResultEvent != null)
				{
					_instance.OnAdminSubtractUserVirtualCurrencyResultEvent((PlayFab.AdminModels.ModifyUserVirtualCurrencyResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.UpdateBansResult) && _instance.OnAdminUpdateBansResultEvent != null)
				{
					_instance.OnAdminUpdateBansResultEvent((PlayFab.AdminModels.UpdateBansResult)e.Result);
				}
				else if (type2 == typeof(UpdateCatalogItemsResult) && _instance.OnAdminUpdateCatalogItemsResultEvent != null)
				{
					_instance.OnAdminUpdateCatalogItemsResultEvent((UpdateCatalogItemsResult)e.Result);
				}
				else if (type2 == typeof(UpdateCloudScriptResult) && _instance.OnAdminUpdateCloudScriptResultEvent != null)
				{
					_instance.OnAdminUpdateCloudScriptResultEvent((UpdateCloudScriptResult)e.Result);
				}
				else if (type2 == typeof(UpdatePlayerSharedSecretResult) && _instance.OnAdminUpdatePlayerSharedSecretResultEvent != null)
				{
					_instance.OnAdminUpdatePlayerSharedSecretResultEvent((UpdatePlayerSharedSecretResult)e.Result);
				}
				else if (type2 == typeof(UpdatePlayerStatisticDefinitionResult) && _instance.OnAdminUpdatePlayerStatisticDefinitionResultEvent != null)
				{
					_instance.OnAdminUpdatePlayerStatisticDefinitionResultEvent((UpdatePlayerStatisticDefinitionResult)e.Result);
				}
				else if (type2 == typeof(UpdatePolicyResponse) && _instance.OnAdminUpdatePolicyResultEvent != null)
				{
					_instance.OnAdminUpdatePolicyResultEvent((UpdatePolicyResponse)e.Result);
				}
				else if (type2 == typeof(UpdateRandomResultTablesResult) && _instance.OnAdminUpdateRandomResultTablesResultEvent != null)
				{
					_instance.OnAdminUpdateRandomResultTablesResultEvent((UpdateRandomResultTablesResult)e.Result);
				}
				else if (type2 == typeof(UpdateStoreItemsResult) && _instance.OnAdminUpdateStoreItemsResultEvent != null)
				{
					_instance.OnAdminUpdateStoreItemsResultEvent((UpdateStoreItemsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.EmptyResult) && _instance.OnAdminUpdateTaskResultEvent != null)
				{
					_instance.OnAdminUpdateTaskResultEvent((PlayFab.AdminModels.EmptyResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.UpdateUserDataResult) && _instance.OnAdminUpdateUserDataResultEvent != null)
				{
					_instance.OnAdminUpdateUserDataResultEvent((PlayFab.AdminModels.UpdateUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.UpdateUserDataResult) && _instance.OnAdminUpdateUserInternalDataResultEvent != null)
				{
					_instance.OnAdminUpdateUserInternalDataResultEvent((PlayFab.AdminModels.UpdateUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.UpdateUserDataResult) && _instance.OnAdminUpdateUserPublisherDataResultEvent != null)
				{
					_instance.OnAdminUpdateUserPublisherDataResultEvent((PlayFab.AdminModels.UpdateUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.UpdateUserDataResult) && _instance.OnAdminUpdateUserPublisherInternalDataResultEvent != null)
				{
					_instance.OnAdminUpdateUserPublisherInternalDataResultEvent((PlayFab.AdminModels.UpdateUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.UpdateUserDataResult) && _instance.OnAdminUpdateUserPublisherReadOnlyDataResultEvent != null)
				{
					_instance.OnAdminUpdateUserPublisherReadOnlyDataResultEvent((PlayFab.AdminModels.UpdateUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.UpdateUserDataResult) && _instance.OnAdminUpdateUserReadOnlyDataResultEvent != null)
				{
					_instance.OnAdminUpdateUserReadOnlyDataResultEvent((PlayFab.AdminModels.UpdateUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.AdminModels.UpdateUserTitleDisplayNameResult) && _instance.OnAdminUpdateUserTitleDisplayNameResultEvent != null)
				{
					_instance.OnAdminUpdateUserTitleDisplayNameResultEvent((PlayFab.AdminModels.UpdateUserTitleDisplayNameResult)e.Result);
				}
				else if (type2 == typeof(AuthUserResponse) && _instance.OnMatchmakerAuthUserResultEvent != null)
				{
					_instance.OnMatchmakerAuthUserResultEvent((AuthUserResponse)e.Result);
				}
				else if (type2 == typeof(PlayerJoinedResponse) && _instance.OnMatchmakerPlayerJoinedResultEvent != null)
				{
					_instance.OnMatchmakerPlayerJoinedResultEvent((PlayerJoinedResponse)e.Result);
				}
				else if (type2 == typeof(PlayerLeftResponse) && _instance.OnMatchmakerPlayerLeftResultEvent != null)
				{
					_instance.OnMatchmakerPlayerLeftResultEvent((PlayerLeftResponse)e.Result);
				}
				else if (type2 == typeof(StartGameResponse) && _instance.OnMatchmakerStartGameResultEvent != null)
				{
					_instance.OnMatchmakerStartGameResultEvent((StartGameResponse)e.Result);
				}
				else if (type2 == typeof(UserInfoResponse) && _instance.OnMatchmakerUserInfoResultEvent != null)
				{
					_instance.OnMatchmakerUserInfoResultEvent((UserInfoResponse)e.Result);
				}
				else if (type2 == typeof(ModifyCharacterVirtualCurrencyResult) && _instance.OnServerAddCharacterVirtualCurrencyResultEvent != null)
				{
					_instance.OnServerAddCharacterVirtualCurrencyResultEvent((ModifyCharacterVirtualCurrencyResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.EmptyResult) && _instance.OnServerAddFriendResultEvent != null)
				{
					_instance.OnServerAddFriendResultEvent((PlayFab.ServerModels.EmptyResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.AddPlayerTagResult) && _instance.OnServerAddPlayerTagResultEvent != null)
				{
					_instance.OnServerAddPlayerTagResultEvent((PlayFab.ServerModels.AddPlayerTagResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.AddSharedGroupMembersResult) && _instance.OnServerAddSharedGroupMembersResultEvent != null)
				{
					_instance.OnServerAddSharedGroupMembersResultEvent((PlayFab.ServerModels.AddSharedGroupMembersResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.ModifyUserVirtualCurrencyResult) && _instance.OnServerAddUserVirtualCurrencyResultEvent != null)
				{
					_instance.OnServerAddUserVirtualCurrencyResultEvent((PlayFab.ServerModels.ModifyUserVirtualCurrencyResult)e.Result);
				}
				else if (type2 == typeof(AuthenticateSessionTicketResult) && _instance.OnServerAuthenticateSessionTicketResultEvent != null)
				{
					_instance.OnServerAuthenticateSessionTicketResultEvent((AuthenticateSessionTicketResult)e.Result);
				}
				else if (type2 == typeof(AwardSteamAchievementResult) && _instance.OnServerAwardSteamAchievementResultEvent != null)
				{
					_instance.OnServerAwardSteamAchievementResultEvent((AwardSteamAchievementResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.BanUsersResult) && _instance.OnServerBanUsersResultEvent != null)
				{
					_instance.OnServerBanUsersResultEvent((PlayFab.ServerModels.BanUsersResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.ConsumeItemResult) && _instance.OnServerConsumeItemResultEvent != null)
				{
					_instance.OnServerConsumeItemResultEvent((PlayFab.ServerModels.ConsumeItemResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.CreateSharedGroupResult) && _instance.OnServerCreateSharedGroupResultEvent != null)
				{
					_instance.OnServerCreateSharedGroupResultEvent((PlayFab.ServerModels.CreateSharedGroupResult)e.Result);
				}
				else if (type2 == typeof(DeleteCharacterFromUserResult) && _instance.OnServerDeleteCharacterFromUserResultEvent != null)
				{
					_instance.OnServerDeleteCharacterFromUserResultEvent((DeleteCharacterFromUserResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.EmptyResult) && _instance.OnServerDeleteSharedGroupResultEvent != null)
				{
					_instance.OnServerDeleteSharedGroupResultEvent((PlayFab.ServerModels.EmptyResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.DeleteUsersResult) && _instance.OnServerDeleteUsersResultEvent != null)
				{
					_instance.OnServerDeleteUsersResultEvent((PlayFab.ServerModels.DeleteUsersResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.DeregisterGameResponse) && _instance.OnServerDeregisterGameResultEvent != null)
				{
					_instance.OnServerDeregisterGameResultEvent((PlayFab.ServerModels.DeregisterGameResponse)e.Result);
				}
				else if (type2 == typeof(EvaluateRandomResultTableResult) && _instance.OnServerEvaluateRandomResultTableResultEvent != null)
				{
					_instance.OnServerEvaluateRandomResultTableResultEvent((EvaluateRandomResultTableResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.ExecuteCloudScriptResult) && _instance.OnServerExecuteCloudScriptResultEvent != null)
				{
					_instance.OnServerExecuteCloudScriptResultEvent((PlayFab.ServerModels.ExecuteCloudScriptResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetAllSegmentsResult) && _instance.OnServerGetAllSegmentsResultEvent != null)
				{
					_instance.OnServerGetAllSegmentsResultEvent((PlayFab.ServerModels.GetAllSegmentsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.ListUsersCharactersResult) && _instance.OnServerGetAllUsersCharactersResultEvent != null)
				{
					_instance.OnServerGetAllUsersCharactersResultEvent((PlayFab.ServerModels.ListUsersCharactersResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetCatalogItemsResult) && _instance.OnServerGetCatalogItemsResultEvent != null)
				{
					_instance.OnServerGetCatalogItemsResultEvent((PlayFab.ServerModels.GetCatalogItemsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetCharacterDataResult) && _instance.OnServerGetCharacterDataResultEvent != null)
				{
					_instance.OnServerGetCharacterDataResultEvent((PlayFab.ServerModels.GetCharacterDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetCharacterDataResult) && _instance.OnServerGetCharacterInternalDataResultEvent != null)
				{
					_instance.OnServerGetCharacterInternalDataResultEvent((PlayFab.ServerModels.GetCharacterDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetCharacterInventoryResult) && _instance.OnServerGetCharacterInventoryResultEvent != null)
				{
					_instance.OnServerGetCharacterInventoryResultEvent((PlayFab.ServerModels.GetCharacterInventoryResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetCharacterLeaderboardResult) && _instance.OnServerGetCharacterLeaderboardResultEvent != null)
				{
					_instance.OnServerGetCharacterLeaderboardResultEvent((PlayFab.ServerModels.GetCharacterLeaderboardResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetCharacterDataResult) && _instance.OnServerGetCharacterReadOnlyDataResultEvent != null)
				{
					_instance.OnServerGetCharacterReadOnlyDataResultEvent((PlayFab.ServerModels.GetCharacterDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetCharacterStatisticsResult) && _instance.OnServerGetCharacterStatisticsResultEvent != null)
				{
					_instance.OnServerGetCharacterStatisticsResultEvent((PlayFab.ServerModels.GetCharacterStatisticsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetContentDownloadUrlResult) && _instance.OnServerGetContentDownloadUrlResultEvent != null)
				{
					_instance.OnServerGetContentDownloadUrlResultEvent((PlayFab.ServerModels.GetContentDownloadUrlResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetLeaderboardResult) && _instance.OnServerGetFriendLeaderboardResultEvent != null)
				{
					_instance.OnServerGetFriendLeaderboardResultEvent((PlayFab.ServerModels.GetLeaderboardResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetFriendsListResult) && _instance.OnServerGetFriendsListResultEvent != null)
				{
					_instance.OnServerGetFriendsListResultEvent((PlayFab.ServerModels.GetFriendsListResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetLeaderboardResult) && _instance.OnServerGetLeaderboardResultEvent != null)
				{
					_instance.OnServerGetLeaderboardResultEvent((PlayFab.ServerModels.GetLeaderboardResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetLeaderboardAroundCharacterResult) && _instance.OnServerGetLeaderboardAroundCharacterResultEvent != null)
				{
					_instance.OnServerGetLeaderboardAroundCharacterResultEvent((PlayFab.ServerModels.GetLeaderboardAroundCharacterResult)e.Result);
				}
				else if (type2 == typeof(GetLeaderboardAroundUserResult) && _instance.OnServerGetLeaderboardAroundUserResultEvent != null)
				{
					_instance.OnServerGetLeaderboardAroundUserResultEvent((GetLeaderboardAroundUserResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetLeaderboardForUsersCharactersResult) && _instance.OnServerGetLeaderboardForUserCharactersResultEvent != null)
				{
					_instance.OnServerGetLeaderboardForUserCharactersResultEvent((PlayFab.ServerModels.GetLeaderboardForUsersCharactersResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetPlayerCombinedInfoResult) && _instance.OnServerGetPlayerCombinedInfoResultEvent != null)
				{
					_instance.OnServerGetPlayerCombinedInfoResultEvent((PlayFab.ServerModels.GetPlayerCombinedInfoResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetPlayerProfileResult) && _instance.OnServerGetPlayerProfileResultEvent != null)
				{
					_instance.OnServerGetPlayerProfileResultEvent((PlayFab.ServerModels.GetPlayerProfileResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetPlayerSegmentsResult) && _instance.OnServerGetPlayerSegmentsResultEvent != null)
				{
					_instance.OnServerGetPlayerSegmentsResultEvent((PlayFab.ServerModels.GetPlayerSegmentsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetPlayersInSegmentResult) && _instance.OnServerGetPlayersInSegmentResultEvent != null)
				{
					_instance.OnServerGetPlayersInSegmentResultEvent((PlayFab.ServerModels.GetPlayersInSegmentResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetPlayerStatisticsResult) && _instance.OnServerGetPlayerStatisticsResultEvent != null)
				{
					_instance.OnServerGetPlayerStatisticsResultEvent((PlayFab.ServerModels.GetPlayerStatisticsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetPlayerStatisticVersionsResult) && _instance.OnServerGetPlayerStatisticVersionsResultEvent != null)
				{
					_instance.OnServerGetPlayerStatisticVersionsResultEvent((PlayFab.ServerModels.GetPlayerStatisticVersionsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetPlayerTagsResult) && _instance.OnServerGetPlayerTagsResultEvent != null)
				{
					_instance.OnServerGetPlayerTagsResultEvent((PlayFab.ServerModels.GetPlayerTagsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetPlayFabIDsFromFacebookIDsResult) && _instance.OnServerGetPlayFabIDsFromFacebookIDsResultEvent != null)
				{
					_instance.OnServerGetPlayFabIDsFromFacebookIDsResultEvent((PlayFab.ServerModels.GetPlayFabIDsFromFacebookIDsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetPlayFabIDsFromSteamIDsResult) && _instance.OnServerGetPlayFabIDsFromSteamIDsResultEvent != null)
				{
					_instance.OnServerGetPlayFabIDsFromSteamIDsResultEvent((PlayFab.ServerModels.GetPlayFabIDsFromSteamIDsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetPublisherDataResult) && _instance.OnServerGetPublisherDataResultEvent != null)
				{
					_instance.OnServerGetPublisherDataResultEvent((PlayFab.ServerModels.GetPublisherDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetRandomResultTablesResult) && _instance.OnServerGetRandomResultTablesResultEvent != null)
				{
					_instance.OnServerGetRandomResultTablesResultEvent((PlayFab.ServerModels.GetRandomResultTablesResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetSharedGroupDataResult) && _instance.OnServerGetSharedGroupDataResultEvent != null)
				{
					_instance.OnServerGetSharedGroupDataResultEvent((PlayFab.ServerModels.GetSharedGroupDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetTimeResult) && _instance.OnServerGetTimeResultEvent != null)
				{
					_instance.OnServerGetTimeResultEvent((PlayFab.ServerModels.GetTimeResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetTitleDataResult) && _instance.OnServerGetTitleDataResultEvent != null)
				{
					_instance.OnServerGetTitleDataResultEvent((PlayFab.ServerModels.GetTitleDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetTitleDataResult) && _instance.OnServerGetTitleInternalDataResultEvent != null)
				{
					_instance.OnServerGetTitleInternalDataResultEvent((PlayFab.ServerModels.GetTitleDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetTitleNewsResult) && _instance.OnServerGetTitleNewsResultEvent != null)
				{
					_instance.OnServerGetTitleNewsResultEvent((PlayFab.ServerModels.GetTitleNewsResult)e.Result);
				}
				else if (type2 == typeof(GetUserAccountInfoResult) && _instance.OnServerGetUserAccountInfoResultEvent != null)
				{
					_instance.OnServerGetUserAccountInfoResultEvent((GetUserAccountInfoResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetUserBansResult) && _instance.OnServerGetUserBansResultEvent != null)
				{
					_instance.OnServerGetUserBansResultEvent((PlayFab.ServerModels.GetUserBansResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetUserDataResult) && _instance.OnServerGetUserDataResultEvent != null)
				{
					_instance.OnServerGetUserDataResultEvent((PlayFab.ServerModels.GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetUserDataResult) && _instance.OnServerGetUserInternalDataResultEvent != null)
				{
					_instance.OnServerGetUserInternalDataResultEvent((PlayFab.ServerModels.GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetUserInventoryResult) && _instance.OnServerGetUserInventoryResultEvent != null)
				{
					_instance.OnServerGetUserInventoryResultEvent((PlayFab.ServerModels.GetUserInventoryResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetUserDataResult) && _instance.OnServerGetUserPublisherDataResultEvent != null)
				{
					_instance.OnServerGetUserPublisherDataResultEvent((PlayFab.ServerModels.GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetUserDataResult) && _instance.OnServerGetUserPublisherInternalDataResultEvent != null)
				{
					_instance.OnServerGetUserPublisherInternalDataResultEvent((PlayFab.ServerModels.GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetUserDataResult) && _instance.OnServerGetUserPublisherReadOnlyDataResultEvent != null)
				{
					_instance.OnServerGetUserPublisherReadOnlyDataResultEvent((PlayFab.ServerModels.GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GetUserDataResult) && _instance.OnServerGetUserReadOnlyDataResultEvent != null)
				{
					_instance.OnServerGetUserReadOnlyDataResultEvent((PlayFab.ServerModels.GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GrantCharacterToUserResult) && _instance.OnServerGrantCharacterToUserResultEvent != null)
				{
					_instance.OnServerGrantCharacterToUserResultEvent((PlayFab.ServerModels.GrantCharacterToUserResult)e.Result);
				}
				else if (type2 == typeof(GrantItemsToCharacterResult) && _instance.OnServerGrantItemsToCharacterResultEvent != null)
				{
					_instance.OnServerGrantItemsToCharacterResultEvent((GrantItemsToCharacterResult)e.Result);
				}
				else if (type2 == typeof(GrantItemsToUserResult) && _instance.OnServerGrantItemsToUserResultEvent != null)
				{
					_instance.OnServerGrantItemsToUserResultEvent((GrantItemsToUserResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.GrantItemsToUsersResult) && _instance.OnServerGrantItemsToUsersResultEvent != null)
				{
					_instance.OnServerGrantItemsToUsersResultEvent((PlayFab.ServerModels.GrantItemsToUsersResult)e.Result);
				}
				else if (type2 == typeof(ModifyItemUsesResult) && _instance.OnServerModifyItemUsesResultEvent != null)
				{
					_instance.OnServerModifyItemUsesResultEvent((ModifyItemUsesResult)e.Result);
				}
				else if (type2 == typeof(MoveItemToCharacterFromCharacterResult) && _instance.OnServerMoveItemToCharacterFromCharacterResultEvent != null)
				{
					_instance.OnServerMoveItemToCharacterFromCharacterResultEvent((MoveItemToCharacterFromCharacterResult)e.Result);
				}
				else if (type2 == typeof(MoveItemToCharacterFromUserResult) && _instance.OnServerMoveItemToCharacterFromUserResultEvent != null)
				{
					_instance.OnServerMoveItemToCharacterFromUserResultEvent((MoveItemToCharacterFromUserResult)e.Result);
				}
				else if (type2 == typeof(MoveItemToUserFromCharacterResult) && _instance.OnServerMoveItemToUserFromCharacterResultEvent != null)
				{
					_instance.OnServerMoveItemToUserFromCharacterResultEvent((MoveItemToUserFromCharacterResult)e.Result);
				}
				else if (type2 == typeof(NotifyMatchmakerPlayerLeftResult) && _instance.OnServerNotifyMatchmakerPlayerLeftResultEvent != null)
				{
					_instance.OnServerNotifyMatchmakerPlayerLeftResultEvent((NotifyMatchmakerPlayerLeftResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.RedeemCouponResult) && _instance.OnServerRedeemCouponResultEvent != null)
				{
					_instance.OnServerRedeemCouponResultEvent((PlayFab.ServerModels.RedeemCouponResult)e.Result);
				}
				else if (type2 == typeof(RedeemMatchmakerTicketResult) && _instance.OnServerRedeemMatchmakerTicketResultEvent != null)
				{
					_instance.OnServerRedeemMatchmakerTicketResultEvent((RedeemMatchmakerTicketResult)e.Result);
				}
				else if (type2 == typeof(RefreshGameServerInstanceHeartbeatResult) && _instance.OnServerRefreshGameServerInstanceHeartbeatResultEvent != null)
				{
					_instance.OnServerRefreshGameServerInstanceHeartbeatResultEvent((RefreshGameServerInstanceHeartbeatResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.RegisterGameResponse) && _instance.OnServerRegisterGameResultEvent != null)
				{
					_instance.OnServerRegisterGameResultEvent((PlayFab.ServerModels.RegisterGameResponse)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.EmptyResult) && _instance.OnServerRemoveFriendResultEvent != null)
				{
					_instance.OnServerRemoveFriendResultEvent((PlayFab.ServerModels.EmptyResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.RemovePlayerTagResult) && _instance.OnServerRemovePlayerTagResultEvent != null)
				{
					_instance.OnServerRemovePlayerTagResultEvent((PlayFab.ServerModels.RemovePlayerTagResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.RemoveSharedGroupMembersResult) && _instance.OnServerRemoveSharedGroupMembersResultEvent != null)
				{
					_instance.OnServerRemoveSharedGroupMembersResultEvent((PlayFab.ServerModels.RemoveSharedGroupMembersResult)e.Result);
				}
				else if (type2 == typeof(ReportPlayerServerResult) && _instance.OnServerReportPlayerResultEvent != null)
				{
					_instance.OnServerReportPlayerResultEvent((ReportPlayerServerResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.RevokeAllBansForUserResult) && _instance.OnServerRevokeAllBansForUserResultEvent != null)
				{
					_instance.OnServerRevokeAllBansForUserResultEvent((PlayFab.ServerModels.RevokeAllBansForUserResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.RevokeBansResult) && _instance.OnServerRevokeBansResultEvent != null)
				{
					_instance.OnServerRevokeBansResultEvent((PlayFab.ServerModels.RevokeBansResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.RevokeInventoryResult) && _instance.OnServerRevokeInventoryItemResultEvent != null)
				{
					_instance.OnServerRevokeInventoryItemResultEvent((PlayFab.ServerModels.RevokeInventoryResult)e.Result);
				}
				else if (type2 == typeof(SendCustomAccountRecoveryEmailResult) && _instance.OnServerSendCustomAccountRecoveryEmailResultEvent != null)
				{
					_instance.OnServerSendCustomAccountRecoveryEmailResultEvent((SendCustomAccountRecoveryEmailResult)e.Result);
				}
				else if (type2 == typeof(SendEmailFromTemplateResult) && _instance.OnServerSendEmailFromTemplateResultEvent != null)
				{
					_instance.OnServerSendEmailFromTemplateResultEvent((SendEmailFromTemplateResult)e.Result);
				}
				else if (type2 == typeof(SendPushNotificationResult) && _instance.OnServerSendPushNotificationResultEvent != null)
				{
					_instance.OnServerSendPushNotificationResultEvent((SendPushNotificationResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.EmptyResult) && _instance.OnServerSetFriendTagsResultEvent != null)
				{
					_instance.OnServerSetFriendTagsResultEvent((PlayFab.ServerModels.EmptyResult)e.Result);
				}
				else if (type2 == typeof(SetGameServerInstanceDataResult) && _instance.OnServerSetGameServerInstanceDataResultEvent != null)
				{
					_instance.OnServerSetGameServerInstanceDataResultEvent((SetGameServerInstanceDataResult)e.Result);
				}
				else if (type2 == typeof(SetGameServerInstanceStateResult) && _instance.OnServerSetGameServerInstanceStateResultEvent != null)
				{
					_instance.OnServerSetGameServerInstanceStateResultEvent((SetGameServerInstanceStateResult)e.Result);
				}
				else if (type2 == typeof(SetGameServerInstanceTagsResult) && _instance.OnServerSetGameServerInstanceTagsResultEvent != null)
				{
					_instance.OnServerSetGameServerInstanceTagsResultEvent((SetGameServerInstanceTagsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.SetPlayerSecretResult) && _instance.OnServerSetPlayerSecretResultEvent != null)
				{
					_instance.OnServerSetPlayerSecretResultEvent((PlayFab.ServerModels.SetPlayerSecretResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.SetPublisherDataResult) && _instance.OnServerSetPublisherDataResultEvent != null)
				{
					_instance.OnServerSetPublisherDataResultEvent((PlayFab.ServerModels.SetPublisherDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.SetTitleDataResult) && _instance.OnServerSetTitleDataResultEvent != null)
				{
					_instance.OnServerSetTitleDataResultEvent((PlayFab.ServerModels.SetTitleDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.SetTitleDataResult) && _instance.OnServerSetTitleInternalDataResultEvent != null)
				{
					_instance.OnServerSetTitleInternalDataResultEvent((PlayFab.ServerModels.SetTitleDataResult)e.Result);
				}
				else if (type2 == typeof(ModifyCharacterVirtualCurrencyResult) && _instance.OnServerSubtractCharacterVirtualCurrencyResultEvent != null)
				{
					_instance.OnServerSubtractCharacterVirtualCurrencyResultEvent((ModifyCharacterVirtualCurrencyResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.ModifyUserVirtualCurrencyResult) && _instance.OnServerSubtractUserVirtualCurrencyResultEvent != null)
				{
					_instance.OnServerSubtractUserVirtualCurrencyResultEvent((PlayFab.ServerModels.ModifyUserVirtualCurrencyResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.UnlockContainerItemResult) && _instance.OnServerUnlockContainerInstanceResultEvent != null)
				{
					_instance.OnServerUnlockContainerInstanceResultEvent((PlayFab.ServerModels.UnlockContainerItemResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.UnlockContainerItemResult) && _instance.OnServerUnlockContainerItemResultEvent != null)
				{
					_instance.OnServerUnlockContainerItemResultEvent((PlayFab.ServerModels.UnlockContainerItemResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.EmptyResult) && _instance.OnServerUpdateAvatarUrlResultEvent != null)
				{
					_instance.OnServerUpdateAvatarUrlResultEvent((PlayFab.ServerModels.EmptyResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.UpdateBansResult) && _instance.OnServerUpdateBansResultEvent != null)
				{
					_instance.OnServerUpdateBansResultEvent((PlayFab.ServerModels.UpdateBansResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.UpdateCharacterDataResult) && _instance.OnServerUpdateCharacterDataResultEvent != null)
				{
					_instance.OnServerUpdateCharacterDataResultEvent((PlayFab.ServerModels.UpdateCharacterDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.UpdateCharacterDataResult) && _instance.OnServerUpdateCharacterInternalDataResultEvent != null)
				{
					_instance.OnServerUpdateCharacterInternalDataResultEvent((PlayFab.ServerModels.UpdateCharacterDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.UpdateCharacterDataResult) && _instance.OnServerUpdateCharacterReadOnlyDataResultEvent != null)
				{
					_instance.OnServerUpdateCharacterReadOnlyDataResultEvent((PlayFab.ServerModels.UpdateCharacterDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.UpdateCharacterStatisticsResult) && _instance.OnServerUpdateCharacterStatisticsResultEvent != null)
				{
					_instance.OnServerUpdateCharacterStatisticsResultEvent((PlayFab.ServerModels.UpdateCharacterStatisticsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.UpdatePlayerStatisticsResult) && _instance.OnServerUpdatePlayerStatisticsResultEvent != null)
				{
					_instance.OnServerUpdatePlayerStatisticsResultEvent((PlayFab.ServerModels.UpdatePlayerStatisticsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.UpdateSharedGroupDataResult) && _instance.OnServerUpdateSharedGroupDataResultEvent != null)
				{
					_instance.OnServerUpdateSharedGroupDataResultEvent((PlayFab.ServerModels.UpdateSharedGroupDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.UpdateUserDataResult) && _instance.OnServerUpdateUserDataResultEvent != null)
				{
					_instance.OnServerUpdateUserDataResultEvent((PlayFab.ServerModels.UpdateUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.UpdateUserDataResult) && _instance.OnServerUpdateUserInternalDataResultEvent != null)
				{
					_instance.OnServerUpdateUserInternalDataResultEvent((PlayFab.ServerModels.UpdateUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.EmptyResult) && _instance.OnServerUpdateUserInventoryItemCustomDataResultEvent != null)
				{
					_instance.OnServerUpdateUserInventoryItemCustomDataResultEvent((PlayFab.ServerModels.EmptyResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.UpdateUserDataResult) && _instance.OnServerUpdateUserPublisherDataResultEvent != null)
				{
					_instance.OnServerUpdateUserPublisherDataResultEvent((PlayFab.ServerModels.UpdateUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.UpdateUserDataResult) && _instance.OnServerUpdateUserPublisherInternalDataResultEvent != null)
				{
					_instance.OnServerUpdateUserPublisherInternalDataResultEvent((PlayFab.ServerModels.UpdateUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.UpdateUserDataResult) && _instance.OnServerUpdateUserPublisherReadOnlyDataResultEvent != null)
				{
					_instance.OnServerUpdateUserPublisherReadOnlyDataResultEvent((PlayFab.ServerModels.UpdateUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.UpdateUserDataResult) && _instance.OnServerUpdateUserReadOnlyDataResultEvent != null)
				{
					_instance.OnServerUpdateUserReadOnlyDataResultEvent((PlayFab.ServerModels.UpdateUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.WriteEventResponse) && _instance.OnServerWriteCharacterEventResultEvent != null)
				{
					_instance.OnServerWriteCharacterEventResultEvent((PlayFab.ServerModels.WriteEventResponse)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.WriteEventResponse) && _instance.OnServerWritePlayerEventResultEvent != null)
				{
					_instance.OnServerWritePlayerEventResultEvent((PlayFab.ServerModels.WriteEventResponse)e.Result);
				}
				else if (type2 == typeof(PlayFab.ServerModels.WriteEventResponse) && _instance.OnServerWriteTitleEventResultEvent != null)
				{
					_instance.OnServerWriteTitleEventResultEvent((PlayFab.ServerModels.WriteEventResponse)e.Result);
				}
				else if (type2 == typeof(LoginResult) && _instance.OnLoginResultEvent != null)
				{
					_instance.OnLoginResultEvent((LoginResult)e.Result);
				}
				else if (type2 == typeof(AcceptTradeResponse) && _instance.OnAcceptTradeResultEvent != null)
				{
					_instance.OnAcceptTradeResultEvent((AcceptTradeResponse)e.Result);
				}
				else if (type2 == typeof(AddFriendResult) && _instance.OnAddFriendResultEvent != null)
				{
					_instance.OnAddFriendResultEvent((AddFriendResult)e.Result);
				}
				else if (type2 == typeof(AddGenericIDResult) && _instance.OnAddGenericIDResultEvent != null)
				{
					_instance.OnAddGenericIDResultEvent((AddGenericIDResult)e.Result);
				}
				else if (type2 == typeof(AddOrUpdateContactEmailResult) && _instance.OnAddOrUpdateContactEmailResultEvent != null)
				{
					_instance.OnAddOrUpdateContactEmailResultEvent((AddOrUpdateContactEmailResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.AddSharedGroupMembersResult) && _instance.OnAddSharedGroupMembersResultEvent != null)
				{
					_instance.OnAddSharedGroupMembersResultEvent((PlayFab.ClientModels.AddSharedGroupMembersResult)e.Result);
				}
				else if (type2 == typeof(AddUsernamePasswordResult) && _instance.OnAddUsernamePasswordResultEvent != null)
				{
					_instance.OnAddUsernamePasswordResultEvent((AddUsernamePasswordResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.ModifyUserVirtualCurrencyResult) && _instance.OnAddUserVirtualCurrencyResultEvent != null)
				{
					_instance.OnAddUserVirtualCurrencyResultEvent((PlayFab.ClientModels.ModifyUserVirtualCurrencyResult)e.Result);
				}
				else if (type2 == typeof(AndroidDevicePushNotificationRegistrationResult) && _instance.OnAndroidDevicePushNotificationRegistrationResultEvent != null)
				{
					_instance.OnAndroidDevicePushNotificationRegistrationResultEvent((AndroidDevicePushNotificationRegistrationResult)e.Result);
				}
				else if (type2 == typeof(AttributeInstallResult) && _instance.OnAttributeInstallResultEvent != null)
				{
					_instance.OnAttributeInstallResultEvent((AttributeInstallResult)e.Result);
				}
				else if (type2 == typeof(CancelTradeResponse) && _instance.OnCancelTradeResultEvent != null)
				{
					_instance.OnCancelTradeResultEvent((CancelTradeResponse)e.Result);
				}
				else if (type2 == typeof(ConfirmPurchaseResult) && _instance.OnConfirmPurchaseResultEvent != null)
				{
					_instance.OnConfirmPurchaseResultEvent((ConfirmPurchaseResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.ConsumeItemResult) && _instance.OnConsumeItemResultEvent != null)
				{
					_instance.OnConsumeItemResultEvent((PlayFab.ClientModels.ConsumeItemResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.CreateSharedGroupResult) && _instance.OnCreateSharedGroupResultEvent != null)
				{
					_instance.OnCreateSharedGroupResultEvent((PlayFab.ClientModels.CreateSharedGroupResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.ExecuteCloudScriptResult) && _instance.OnExecuteCloudScriptResultEvent != null)
				{
					_instance.OnExecuteCloudScriptResultEvent((PlayFab.ClientModels.ExecuteCloudScriptResult)e.Result);
				}
				else if (type2 == typeof(GetAccountInfoResult) && _instance.OnGetAccountInfoResultEvent != null)
				{
					_instance.OnGetAccountInfoResultEvent((GetAccountInfoResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.ListUsersCharactersResult) && _instance.OnGetAllUsersCharactersResultEvent != null)
				{
					_instance.OnGetAllUsersCharactersResultEvent((PlayFab.ClientModels.ListUsersCharactersResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetCatalogItemsResult) && _instance.OnGetCatalogItemsResultEvent != null)
				{
					_instance.OnGetCatalogItemsResultEvent((PlayFab.ClientModels.GetCatalogItemsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetCharacterDataResult) && _instance.OnGetCharacterDataResultEvent != null)
				{
					_instance.OnGetCharacterDataResultEvent((PlayFab.ClientModels.GetCharacterDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetCharacterInventoryResult) && _instance.OnGetCharacterInventoryResultEvent != null)
				{
					_instance.OnGetCharacterInventoryResultEvent((PlayFab.ClientModels.GetCharacterInventoryResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetCharacterLeaderboardResult) && _instance.OnGetCharacterLeaderboardResultEvent != null)
				{
					_instance.OnGetCharacterLeaderboardResultEvent((PlayFab.ClientModels.GetCharacterLeaderboardResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetCharacterDataResult) && _instance.OnGetCharacterReadOnlyDataResultEvent != null)
				{
					_instance.OnGetCharacterReadOnlyDataResultEvent((PlayFab.ClientModels.GetCharacterDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetCharacterStatisticsResult) && _instance.OnGetCharacterStatisticsResultEvent != null)
				{
					_instance.OnGetCharacterStatisticsResultEvent((PlayFab.ClientModels.GetCharacterStatisticsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetContentDownloadUrlResult) && _instance.OnGetContentDownloadUrlResultEvent != null)
				{
					_instance.OnGetContentDownloadUrlResultEvent((PlayFab.ClientModels.GetContentDownloadUrlResult)e.Result);
				}
				else if (type2 == typeof(CurrentGamesResult) && _instance.OnGetCurrentGamesResultEvent != null)
				{
					_instance.OnGetCurrentGamesResultEvent((CurrentGamesResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetLeaderboardResult) && _instance.OnGetFriendLeaderboardResultEvent != null)
				{
					_instance.OnGetFriendLeaderboardResultEvent((PlayFab.ClientModels.GetLeaderboardResult)e.Result);
				}
				else if (type2 == typeof(GetFriendLeaderboardAroundPlayerResult) && _instance.OnGetFriendLeaderboardAroundPlayerResultEvent != null)
				{
					_instance.OnGetFriendLeaderboardAroundPlayerResultEvent((GetFriendLeaderboardAroundPlayerResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetFriendsListResult) && _instance.OnGetFriendsListResultEvent != null)
				{
					_instance.OnGetFriendsListResultEvent((PlayFab.ClientModels.GetFriendsListResult)e.Result);
				}
				else if (type2 == typeof(GameServerRegionsResult) && _instance.OnGetGameServerRegionsResultEvent != null)
				{
					_instance.OnGetGameServerRegionsResultEvent((GameServerRegionsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetLeaderboardResult) && _instance.OnGetLeaderboardResultEvent != null)
				{
					_instance.OnGetLeaderboardResultEvent((PlayFab.ClientModels.GetLeaderboardResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetLeaderboardAroundCharacterResult) && _instance.OnGetLeaderboardAroundCharacterResultEvent != null)
				{
					_instance.OnGetLeaderboardAroundCharacterResultEvent((PlayFab.ClientModels.GetLeaderboardAroundCharacterResult)e.Result);
				}
				else if (type2 == typeof(GetLeaderboardAroundPlayerResult) && _instance.OnGetLeaderboardAroundPlayerResultEvent != null)
				{
					_instance.OnGetLeaderboardAroundPlayerResultEvent((GetLeaderboardAroundPlayerResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetLeaderboardForUsersCharactersResult) && _instance.OnGetLeaderboardForUserCharactersResultEvent != null)
				{
					_instance.OnGetLeaderboardForUserCharactersResultEvent((PlayFab.ClientModels.GetLeaderboardForUsersCharactersResult)e.Result);
				}
				else if (type2 == typeof(GetPaymentTokenResult) && _instance.OnGetPaymentTokenResultEvent != null)
				{
					_instance.OnGetPaymentTokenResultEvent((GetPaymentTokenResult)e.Result);
				}
				else if (type2 == typeof(GetPhotonAuthenticationTokenResult) && _instance.OnGetPhotonAuthenticationTokenResultEvent != null)
				{
					_instance.OnGetPhotonAuthenticationTokenResultEvent((GetPhotonAuthenticationTokenResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetPlayerCombinedInfoResult) && _instance.OnGetPlayerCombinedInfoResultEvent != null)
				{
					_instance.OnGetPlayerCombinedInfoResultEvent((PlayFab.ClientModels.GetPlayerCombinedInfoResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetPlayerProfileResult) && _instance.OnGetPlayerProfileResultEvent != null)
				{
					_instance.OnGetPlayerProfileResultEvent((PlayFab.ClientModels.GetPlayerProfileResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetPlayerSegmentsResult) && _instance.OnGetPlayerSegmentsResultEvent != null)
				{
					_instance.OnGetPlayerSegmentsResultEvent((PlayFab.ClientModels.GetPlayerSegmentsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetPlayerStatisticsResult) && _instance.OnGetPlayerStatisticsResultEvent != null)
				{
					_instance.OnGetPlayerStatisticsResultEvent((PlayFab.ClientModels.GetPlayerStatisticsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetPlayerStatisticVersionsResult) && _instance.OnGetPlayerStatisticVersionsResultEvent != null)
				{
					_instance.OnGetPlayerStatisticVersionsResultEvent((PlayFab.ClientModels.GetPlayerStatisticVersionsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetPlayerTagsResult) && _instance.OnGetPlayerTagsResultEvent != null)
				{
					_instance.OnGetPlayerTagsResultEvent((PlayFab.ClientModels.GetPlayerTagsResult)e.Result);
				}
				else if (type2 == typeof(GetPlayerTradesResponse) && _instance.OnGetPlayerTradesResultEvent != null)
				{
					_instance.OnGetPlayerTradesResultEvent((GetPlayerTradesResponse)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetPlayFabIDsFromFacebookIDsResult) && _instance.OnGetPlayFabIDsFromFacebookIDsResultEvent != null)
				{
					_instance.OnGetPlayFabIDsFromFacebookIDsResultEvent((PlayFab.ClientModels.GetPlayFabIDsFromFacebookIDsResult)e.Result);
				}
				else if (type2 == typeof(GetPlayFabIDsFromGameCenterIDsResult) && _instance.OnGetPlayFabIDsFromGameCenterIDsResultEvent != null)
				{
					_instance.OnGetPlayFabIDsFromGameCenterIDsResultEvent((GetPlayFabIDsFromGameCenterIDsResult)e.Result);
				}
				else if (type2 == typeof(GetPlayFabIDsFromGenericIDsResult) && _instance.OnGetPlayFabIDsFromGenericIDsResultEvent != null)
				{
					_instance.OnGetPlayFabIDsFromGenericIDsResultEvent((GetPlayFabIDsFromGenericIDsResult)e.Result);
				}
				else if (type2 == typeof(GetPlayFabIDsFromGoogleIDsResult) && _instance.OnGetPlayFabIDsFromGoogleIDsResultEvent != null)
				{
					_instance.OnGetPlayFabIDsFromGoogleIDsResultEvent((GetPlayFabIDsFromGoogleIDsResult)e.Result);
				}
				else if (type2 == typeof(GetPlayFabIDsFromKongregateIDsResult) && _instance.OnGetPlayFabIDsFromKongregateIDsResultEvent != null)
				{
					_instance.OnGetPlayFabIDsFromKongregateIDsResultEvent((GetPlayFabIDsFromKongregateIDsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetPlayFabIDsFromSteamIDsResult) && _instance.OnGetPlayFabIDsFromSteamIDsResultEvent != null)
				{
					_instance.OnGetPlayFabIDsFromSteamIDsResultEvent((PlayFab.ClientModels.GetPlayFabIDsFromSteamIDsResult)e.Result);
				}
				else if (type2 == typeof(GetPlayFabIDsFromTwitchIDsResult) && _instance.OnGetPlayFabIDsFromTwitchIDsResultEvent != null)
				{
					_instance.OnGetPlayFabIDsFromTwitchIDsResultEvent((GetPlayFabIDsFromTwitchIDsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetPublisherDataResult) && _instance.OnGetPublisherDataResultEvent != null)
				{
					_instance.OnGetPublisherDataResultEvent((PlayFab.ClientModels.GetPublisherDataResult)e.Result);
				}
				else if (type2 == typeof(GetPurchaseResult) && _instance.OnGetPurchaseResultEvent != null)
				{
					_instance.OnGetPurchaseResultEvent((GetPurchaseResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetSharedGroupDataResult) && _instance.OnGetSharedGroupDataResultEvent != null)
				{
					_instance.OnGetSharedGroupDataResultEvent((PlayFab.ClientModels.GetSharedGroupDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetStoreItemsResult) && _instance.OnGetStoreItemsResultEvent != null)
				{
					_instance.OnGetStoreItemsResultEvent((PlayFab.ClientModels.GetStoreItemsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetTimeResult) && _instance.OnGetTimeResultEvent != null)
				{
					_instance.OnGetTimeResultEvent((PlayFab.ClientModels.GetTimeResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetTitleDataResult) && _instance.OnGetTitleDataResultEvent != null)
				{
					_instance.OnGetTitleDataResultEvent((PlayFab.ClientModels.GetTitleDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetTitleNewsResult) && _instance.OnGetTitleNewsResultEvent != null)
				{
					_instance.OnGetTitleNewsResultEvent((PlayFab.ClientModels.GetTitleNewsResult)e.Result);
				}
				else if (type2 == typeof(GetTitlePublicKeyResult) && _instance.OnGetTitlePublicKeyResultEvent != null)
				{
					_instance.OnGetTitlePublicKeyResultEvent((GetTitlePublicKeyResult)e.Result);
				}
				else if (type2 == typeof(GetTradeStatusResponse) && _instance.OnGetTradeStatusResultEvent != null)
				{
					_instance.OnGetTradeStatusResultEvent((GetTradeStatusResponse)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetUserDataResult) && _instance.OnGetUserDataResultEvent != null)
				{
					_instance.OnGetUserDataResultEvent((PlayFab.ClientModels.GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetUserInventoryResult) && _instance.OnGetUserInventoryResultEvent != null)
				{
					_instance.OnGetUserInventoryResultEvent((PlayFab.ClientModels.GetUserInventoryResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetUserDataResult) && _instance.OnGetUserPublisherDataResultEvent != null)
				{
					_instance.OnGetUserPublisherDataResultEvent((PlayFab.ClientModels.GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetUserDataResult) && _instance.OnGetUserPublisherReadOnlyDataResultEvent != null)
				{
					_instance.OnGetUserPublisherReadOnlyDataResultEvent((PlayFab.ClientModels.GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GetUserDataResult) && _instance.OnGetUserReadOnlyDataResultEvent != null)
				{
					_instance.OnGetUserReadOnlyDataResultEvent((PlayFab.ClientModels.GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(GetWindowsHelloChallengeResponse) && _instance.OnGetWindowsHelloChallengeResultEvent != null)
				{
					_instance.OnGetWindowsHelloChallengeResultEvent((GetWindowsHelloChallengeResponse)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.GrantCharacterToUserResult) && _instance.OnGrantCharacterToUserResultEvent != null)
				{
					_instance.OnGrantCharacterToUserResultEvent((PlayFab.ClientModels.GrantCharacterToUserResult)e.Result);
				}
				else if (type2 == typeof(LinkAndroidDeviceIDResult) && _instance.OnLinkAndroidDeviceIDResultEvent != null)
				{
					_instance.OnLinkAndroidDeviceIDResultEvent((LinkAndroidDeviceIDResult)e.Result);
				}
				else if (type2 == typeof(LinkCustomIDResult) && _instance.OnLinkCustomIDResultEvent != null)
				{
					_instance.OnLinkCustomIDResultEvent((LinkCustomIDResult)e.Result);
				}
				else if (type2 == typeof(LinkFacebookAccountResult) && _instance.OnLinkFacebookAccountResultEvent != null)
				{
					_instance.OnLinkFacebookAccountResultEvent((LinkFacebookAccountResult)e.Result);
				}
				else if (type2 == typeof(LinkGameCenterAccountResult) && _instance.OnLinkGameCenterAccountResultEvent != null)
				{
					_instance.OnLinkGameCenterAccountResultEvent((LinkGameCenterAccountResult)e.Result);
				}
				else if (type2 == typeof(LinkGoogleAccountResult) && _instance.OnLinkGoogleAccountResultEvent != null)
				{
					_instance.OnLinkGoogleAccountResultEvent((LinkGoogleAccountResult)e.Result);
				}
				else if (type2 == typeof(LinkIOSDeviceIDResult) && _instance.OnLinkIOSDeviceIDResultEvent != null)
				{
					_instance.OnLinkIOSDeviceIDResultEvent((LinkIOSDeviceIDResult)e.Result);
				}
				else if (type2 == typeof(LinkKongregateAccountResult) && _instance.OnLinkKongregateResultEvent != null)
				{
					_instance.OnLinkKongregateResultEvent((LinkKongregateAccountResult)e.Result);
				}
				else if (type2 == typeof(LinkSteamAccountResult) && _instance.OnLinkSteamAccountResultEvent != null)
				{
					_instance.OnLinkSteamAccountResultEvent((LinkSteamAccountResult)e.Result);
				}
				else if (type2 == typeof(LinkTwitchAccountResult) && _instance.OnLinkTwitchResultEvent != null)
				{
					_instance.OnLinkTwitchResultEvent((LinkTwitchAccountResult)e.Result);
				}
				else if (type2 == typeof(LinkWindowsHelloAccountResponse) && _instance.OnLinkWindowsHelloResultEvent != null)
				{
					_instance.OnLinkWindowsHelloResultEvent((LinkWindowsHelloAccountResponse)e.Result);
				}
				else if (type2 == typeof(MatchmakeResult) && _instance.OnMatchmakeResultEvent != null)
				{
					_instance.OnMatchmakeResultEvent((MatchmakeResult)e.Result);
				}
				else if (type2 == typeof(OpenTradeResponse) && _instance.OnOpenTradeResultEvent != null)
				{
					_instance.OnOpenTradeResultEvent((OpenTradeResponse)e.Result);
				}
				else if (type2 == typeof(PayForPurchaseResult) && _instance.OnPayForPurchaseResultEvent != null)
				{
					_instance.OnPayForPurchaseResultEvent((PayForPurchaseResult)e.Result);
				}
				else if (type2 == typeof(PurchaseItemResult) && _instance.OnPurchaseItemResultEvent != null)
				{
					_instance.OnPurchaseItemResultEvent((PurchaseItemResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.RedeemCouponResult) && _instance.OnRedeemCouponResultEvent != null)
				{
					_instance.OnRedeemCouponResultEvent((PlayFab.ClientModels.RedeemCouponResult)e.Result);
				}
				else if (type2 == typeof(RegisterForIOSPushNotificationResult) && _instance.OnRegisterForIOSPushNotificationResultEvent != null)
				{
					_instance.OnRegisterForIOSPushNotificationResultEvent((RegisterForIOSPushNotificationResult)e.Result);
				}
				else if (type2 == typeof(RegisterPlayFabUserResult) && _instance.OnRegisterPlayFabUserResultEvent != null)
				{
					_instance.OnRegisterPlayFabUserResultEvent((RegisterPlayFabUserResult)e.Result);
				}
				else if (type2 == typeof(RemoveContactEmailResult) && _instance.OnRemoveContactEmailResultEvent != null)
				{
					_instance.OnRemoveContactEmailResultEvent((RemoveContactEmailResult)e.Result);
				}
				else if (type2 == typeof(RemoveFriendResult) && _instance.OnRemoveFriendResultEvent != null)
				{
					_instance.OnRemoveFriendResultEvent((RemoveFriendResult)e.Result);
				}
				else if (type2 == typeof(RemoveGenericIDResult) && _instance.OnRemoveGenericIDResultEvent != null)
				{
					_instance.OnRemoveGenericIDResultEvent((RemoveGenericIDResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.RemoveSharedGroupMembersResult) && _instance.OnRemoveSharedGroupMembersResultEvent != null)
				{
					_instance.OnRemoveSharedGroupMembersResultEvent((PlayFab.ClientModels.RemoveSharedGroupMembersResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.EmptyResult) && _instance.OnReportDeviceInfoResultEvent != null)
				{
					_instance.OnReportDeviceInfoResultEvent((PlayFab.ClientModels.EmptyResult)e.Result);
				}
				else if (type2 == typeof(ReportPlayerClientResult) && _instance.OnReportPlayerResultEvent != null)
				{
					_instance.OnReportPlayerResultEvent((ReportPlayerClientResult)e.Result);
				}
				else if (type2 == typeof(RestoreIOSPurchasesResult) && _instance.OnRestoreIOSPurchasesResultEvent != null)
				{
					_instance.OnRestoreIOSPurchasesResultEvent((RestoreIOSPurchasesResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.SendAccountRecoveryEmailResult) && _instance.OnSendAccountRecoveryEmailResultEvent != null)
				{
					_instance.OnSendAccountRecoveryEmailResultEvent((PlayFab.ClientModels.SendAccountRecoveryEmailResult)e.Result);
				}
				else if (type2 == typeof(SetFriendTagsResult) && _instance.OnSetFriendTagsResultEvent != null)
				{
					_instance.OnSetFriendTagsResultEvent((SetFriendTagsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.SetPlayerSecretResult) && _instance.OnSetPlayerSecretResultEvent != null)
				{
					_instance.OnSetPlayerSecretResultEvent((PlayFab.ClientModels.SetPlayerSecretResult)e.Result);
				}
				else if (type2 == typeof(StartGameResult) && _instance.OnStartGameResultEvent != null)
				{
					_instance.OnStartGameResultEvent((StartGameResult)e.Result);
				}
				else if (type2 == typeof(StartPurchaseResult) && _instance.OnStartPurchaseResultEvent != null)
				{
					_instance.OnStartPurchaseResultEvent((StartPurchaseResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.ModifyUserVirtualCurrencyResult) && _instance.OnSubtractUserVirtualCurrencyResultEvent != null)
				{
					_instance.OnSubtractUserVirtualCurrencyResultEvent((PlayFab.ClientModels.ModifyUserVirtualCurrencyResult)e.Result);
				}
				else if (type2 == typeof(UnlinkAndroidDeviceIDResult) && _instance.OnUnlinkAndroidDeviceIDResultEvent != null)
				{
					_instance.OnUnlinkAndroidDeviceIDResultEvent((UnlinkAndroidDeviceIDResult)e.Result);
				}
				else if (type2 == typeof(UnlinkCustomIDResult) && _instance.OnUnlinkCustomIDResultEvent != null)
				{
					_instance.OnUnlinkCustomIDResultEvent((UnlinkCustomIDResult)e.Result);
				}
				else if (type2 == typeof(UnlinkFacebookAccountResult) && _instance.OnUnlinkFacebookAccountResultEvent != null)
				{
					_instance.OnUnlinkFacebookAccountResultEvent((UnlinkFacebookAccountResult)e.Result);
				}
				else if (type2 == typeof(UnlinkGameCenterAccountResult) && _instance.OnUnlinkGameCenterAccountResultEvent != null)
				{
					_instance.OnUnlinkGameCenterAccountResultEvent((UnlinkGameCenterAccountResult)e.Result);
				}
				else if (type2 == typeof(UnlinkGoogleAccountResult) && _instance.OnUnlinkGoogleAccountResultEvent != null)
				{
					_instance.OnUnlinkGoogleAccountResultEvent((UnlinkGoogleAccountResult)e.Result);
				}
				else if (type2 == typeof(UnlinkIOSDeviceIDResult) && _instance.OnUnlinkIOSDeviceIDResultEvent != null)
				{
					_instance.OnUnlinkIOSDeviceIDResultEvent((UnlinkIOSDeviceIDResult)e.Result);
				}
				else if (type2 == typeof(UnlinkKongregateAccountResult) && _instance.OnUnlinkKongregateResultEvent != null)
				{
					_instance.OnUnlinkKongregateResultEvent((UnlinkKongregateAccountResult)e.Result);
				}
				else if (type2 == typeof(UnlinkSteamAccountResult) && _instance.OnUnlinkSteamAccountResultEvent != null)
				{
					_instance.OnUnlinkSteamAccountResultEvent((UnlinkSteamAccountResult)e.Result);
				}
				else if (type2 == typeof(UnlinkTwitchAccountResult) && _instance.OnUnlinkTwitchResultEvent != null)
				{
					_instance.OnUnlinkTwitchResultEvent((UnlinkTwitchAccountResult)e.Result);
				}
				else if (type2 == typeof(UnlinkWindowsHelloAccountResponse) && _instance.OnUnlinkWindowsHelloResultEvent != null)
				{
					_instance.OnUnlinkWindowsHelloResultEvent((UnlinkWindowsHelloAccountResponse)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.UnlockContainerItemResult) && _instance.OnUnlockContainerInstanceResultEvent != null)
				{
					_instance.OnUnlockContainerInstanceResultEvent((PlayFab.ClientModels.UnlockContainerItemResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.UnlockContainerItemResult) && _instance.OnUnlockContainerItemResultEvent != null)
				{
					_instance.OnUnlockContainerItemResultEvent((PlayFab.ClientModels.UnlockContainerItemResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.EmptyResult) && _instance.OnUpdateAvatarUrlResultEvent != null)
				{
					_instance.OnUpdateAvatarUrlResultEvent((PlayFab.ClientModels.EmptyResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.UpdateCharacterDataResult) && _instance.OnUpdateCharacterDataResultEvent != null)
				{
					_instance.OnUpdateCharacterDataResultEvent((PlayFab.ClientModels.UpdateCharacterDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.UpdateCharacterStatisticsResult) && _instance.OnUpdateCharacterStatisticsResultEvent != null)
				{
					_instance.OnUpdateCharacterStatisticsResultEvent((PlayFab.ClientModels.UpdateCharacterStatisticsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.UpdatePlayerStatisticsResult) && _instance.OnUpdatePlayerStatisticsResultEvent != null)
				{
					_instance.OnUpdatePlayerStatisticsResultEvent((PlayFab.ClientModels.UpdatePlayerStatisticsResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.UpdateSharedGroupDataResult) && _instance.OnUpdateSharedGroupDataResultEvent != null)
				{
					_instance.OnUpdateSharedGroupDataResultEvent((PlayFab.ClientModels.UpdateSharedGroupDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.UpdateUserDataResult) && _instance.OnUpdateUserDataResultEvent != null)
				{
					_instance.OnUpdateUserDataResultEvent((PlayFab.ClientModels.UpdateUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.UpdateUserDataResult) && _instance.OnUpdateUserPublisherDataResultEvent != null)
				{
					_instance.OnUpdateUserPublisherDataResultEvent((PlayFab.ClientModels.UpdateUserDataResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.UpdateUserTitleDisplayNameResult) && _instance.OnUpdateUserTitleDisplayNameResultEvent != null)
				{
					_instance.OnUpdateUserTitleDisplayNameResultEvent((PlayFab.ClientModels.UpdateUserTitleDisplayNameResult)e.Result);
				}
				else if (type2 == typeof(ValidateAmazonReceiptResult) && _instance.OnValidateAmazonIAPReceiptResultEvent != null)
				{
					_instance.OnValidateAmazonIAPReceiptResultEvent((ValidateAmazonReceiptResult)e.Result);
				}
				else if (type2 == typeof(ValidateGooglePlayPurchaseResult) && _instance.OnValidateGooglePlayPurchaseResultEvent != null)
				{
					_instance.OnValidateGooglePlayPurchaseResultEvent((ValidateGooglePlayPurchaseResult)e.Result);
				}
				else if (type2 == typeof(ValidateIOSReceiptResult) && _instance.OnValidateIOSReceiptResultEvent != null)
				{
					_instance.OnValidateIOSReceiptResultEvent((ValidateIOSReceiptResult)e.Result);
				}
				else if (type2 == typeof(ValidateWindowsReceiptResult) && _instance.OnValidateWindowsStoreReceiptResultEvent != null)
				{
					_instance.OnValidateWindowsStoreReceiptResultEvent((ValidateWindowsReceiptResult)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.WriteEventResponse) && _instance.OnWriteCharacterEventResultEvent != null)
				{
					_instance.OnWriteCharacterEventResultEvent((PlayFab.ClientModels.WriteEventResponse)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.WriteEventResponse) && _instance.OnWritePlayerEventResultEvent != null)
				{
					_instance.OnWritePlayerEventResultEvent((PlayFab.ClientModels.WriteEventResponse)e.Result);
				}
				else if (type2 == typeof(PlayFab.ClientModels.WriteEventResponse) && _instance.OnWriteTitleEventResultEvent != null)
				{
					_instance.OnWriteTitleEventResultEvent((PlayFab.ClientModels.WriteEventResponse)e.Result);
				}
			}
		}
	}
}
