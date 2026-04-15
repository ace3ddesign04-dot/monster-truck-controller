namespace PlayFab.Internal
{
	public interface IPlayFabHttp
	{
		bool SessionStarted
		{
			get;
			set;
		}

		string AuthKey
		{
			get;
			set;
		}

		string EntityToken
		{
			get;
			set;
		}

		void InitializeHttp();

		void Update();

		void OnDestroy();

		void MakeApiCall(CallRequestContainer reqContainer);

		int GetPendingMessages();
	}
}
