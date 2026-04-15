using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CommunityMapsMenu : MonoBehaviour
{
	public GameObject mapsLoadingObject;

	public GameObject nothingFoundObject;

	public GameObject pullToLoadMessage;

	public GameObject loadingMoreMapsMessage;

	public MapElement exampleMapElement;

	public GameObject playButton;

	public GameObject editMapButton;

	public InputField searchField;

	public GameObject nameSortUpTriangle;

	public GameObject nameSortDownTriangle;

	public GameObject ratingSortUpTriangle;

	public GameObject ratingSortDownTriangle;

	public GameObject authorSortUpTriangle;

	public GameObject authorSortDownTriangle;

	public Image allMapsButton;

	public Image newestMapsButton;

	public Image favMapsButton;

	public Image myMapsButton;

	public Color selectedButtonColor;

	public Color deselectedButtonColor;

	public Color hiddenButtonColor;

	public int mapsPerPage = 20;

	private List<MapElement> mapElements = new List<MapElement>();

	private SortType currentSortType;

	private bool raising;

	private int lastSortType;

	private int selectedMapCategory;

	private string currentFavsListString;

	private string selectedMapID;

	private int currentMapCategory;

	private int currentPage;

	public int totalMapsCount;

	private bool loadingMaps;

	public string[] mapMetas;

	private LevelWebHandler webHandler
	{
		get
		{
			LevelWebHandler levelWebHandler = GetComponent<LevelWebHandler>();
			if (levelWebHandler == null)
			{
				levelWebHandler = base.gameObject.AddComponent<LevelWebHandler>();
			}
			return levelWebHandler;
		}
	}

	private void OnEnable()
	{
		raising = false;
		lastSortType = -1;
		currentSortType = SortType.None;
		searchField.text = string.Empty;
		loadingMaps = false;
		currentFavsListString = string.Empty;
		allMapsButton.color = selectedButtonColor;
		favMapsButton.color = deselectedButtonColor;
		myMapsButton.color = deselectedButtonColor;
		LoadMapsPage(0, SortType.None, string.Empty, currentFavsListString);
		UpdateTriangles();
	}

	public void LoadMapsPage(int page, SortType sortType, string searchString, string favMapsList)
	{
		if (!loadingMaps)
		{
			currentPage = page;
			if (page == 0)
			{
				mapsLoadingObject.SetActive(value: true);
				loadingMoreMapsMessage.SetActive(value: false);
				exampleMapElement.gameObject.SetActive(value: false);
				playButton.SetActive(value: false);
				editMapButton.SetActive(value: false);
				foreach (MapElement mapElement in mapElements)
				{
					UnityEngine.Object.Destroy(mapElement.gameObject);
				}
				mapElements.Clear();
			}
			else
			{
				loadingMoreMapsMessage.SetActive(value: true);
			}
			pullToLoadMessage.SetActive(value: false);
			nothingFoundObject.SetActive(value: false);
			int from = page * mapsPerPage;
			loadingMaps = true;
			int count = mapsPerPage;
			bool includeHidden = false;
			if (selectedMapCategory == 3)
			{
				count = 1000;
				includeHidden = true;
			}
			webHandler.DownloadLevelsMetadatasPage(from, count, sortType, searchString, favMapsList, includeHidden, OnLevelsMetasDownloaded, OnLevelListLoadError);
		}
	}

	[ContextMenu("Get all maps")]
	private void GetAllMaps()
	{
		StartCoroutine(GetAllMapsCor());
	}

	private IEnumerator GetAllMapsCor()
	{
		WWW w = new WWW("https://keereedev.000webhostapp.com/GetMaps.php?meta&from=0&count=200000&sortType=None&search=&favsList=&includeHidden=0");
		yield return w;
		mapMetas = w.text.Split('\n');
		for (int i = 0; i < mapMetas.Length; i++)
		{
			if (Utility.HasBadWord(mapMetas[i]))
			{
				UnityEngine.Debug.Log(mapMetas[i]);
			}
		}
	}

	private void OnLevelsMetasDownloaded(string rawData)
	{
		string[] array = rawData.Split('\n');
		mapsLoadingObject.SetActive(value: false);
		UnityEngine.Debug.Log(rawData);
		totalMapsCount = int.Parse(array[array.Length - 1]);
		for (int i = 0; i < array.Length - 1; i++)
		{
			MapElement component = UnityEngine.Object.Instantiate(exampleMapElement.gameObject, exampleMapElement.transform.parent).GetComponent<MapElement>();
			string[] array2 = array[i].Split('|');
			string text = array2[0];
			string text2 = array2[1];
			string text3 = array2[2];
			int rating = int.Parse(array2[3]);
			bool flag = bool.Parse(array2[4]);
			string text4 = array2[5];
			if (!flag && !LevelEditorTools.IsMapMadeMyMe(text))
			{
				totalMapsCount--;
				continue;
			}
			component.mapFileName = text;
			component.mapNameText.text = text2;
			component.mapDescriptionText.text = text3;
			component.mapRatingText.text = rating.ToString();
			component.rating = rating;
			if (component.rating > 0)
			{
				component.mapRatingText.color = Color.green;
			}
			else if (component.rating < 0)
			{
				component.mapRatingText.color = Color.red;
			}
			else
			{
				component.mapRatingText.color = Color.white;
			}
			component.mapAuthorText.text = text4;
			component.gameObject.SetActive(value: true);
			component.ToggleStar(LevelEditorTools.IsMapInFavs(text));
			component.ToggleSelection(selected: false, !flag);
			mapElements.Add(component);
		}
		loadingMoreMapsMessage.transform.SetAsLastSibling();
		loadingMoreMapsMessage.SetActive(value: false);
		pullToLoadMessage.transform.SetAsLastSibling();
		pullToLoadMessage.SetActive(mapElements.Count < totalMapsCount);
		nothingFoundObject.SetActive(mapElements.Count == 0);
		loadingMaps = false;
	}

	private void OnLevelListLoadError(string error)
	{
	}

	public void SelectMap(string mapID)
	{
		foreach (MapElement mapElement in mapElements)
		{
			mapElement.ToggleSelection(mapID == mapElement.mapFileName);
		}
		selectedMapID = mapID;
		playButton.SetActive(value: true);
		editMapButton.SetActive(LevelEditorTools.IsMapMadeMyMe(mapID));
	}

	public void SearchMap(string text)
	{
		LoadMapsPage(0, currentSortType, text, currentFavsListString);
	}

	public void SortMaps(int parameter)
	{
		if (!loadingMaps)
		{
			if (lastSortType == parameter && raising)
			{
				raising = false;
			}
			else
			{
				raising = true;
			}
			SortType sortType = SortType.None;
			if (parameter == 1)
			{
				sortType = (raising ? SortType.nameAsc : SortType.nameDesc);
			}
			if (parameter == 2)
			{
				sortType = ((!raising) ? SortType.ratingDesc : SortType.ratingAsc);
			}
			if (parameter == 3)
			{
				sortType = ((!raising) ? SortType.authorDesc : SortType.authorAsc);
			}
			currentSortType = sortType;
			LoadMapsPage(0, sortType, searchField.text, currentFavsListString);
			lastSortType = parameter;
			UpdateTriangles();
		}
	}

	private void UpdateTriangles()
	{
		nameSortDownTriangle.SetActive(lastSortType != 1 || (lastSortType == 1 && raising));
		nameSortUpTriangle.SetActive(lastSortType == 1 && !raising);
		ratingSortDownTriangle.SetActive(lastSortType != 2 || (lastSortType == 2 && raising));
		ratingSortUpTriangle.SetActive(lastSortType == 2 && !raising);
		authorSortDownTriangle.SetActive(lastSortType != 3 || (lastSortType == 3 && raising));
		authorSortUpTriangle.SetActive(lastSortType == 3 && !raising);
	}

	public void ShowMapCategory(int cat)
	{
		if (loadingMaps)
		{
			return;
		}
		selectedMapCategory = cat;
		allMapsButton.color = ((cat != 0) ? deselectedButtonColor : selectedButtonColor);
		newestMapsButton.color = ((cat != 1) ? deselectedButtonColor : selectedButtonColor);
		favMapsButton.color = ((cat != 2) ? deselectedButtonColor : selectedButtonColor);
		myMapsButton.color = ((cat != 3) ? deselectedButtonColor : selectedButtonColor);
		currentFavsListString = string.Empty;
		if (cat == 0 && currentSortType == SortType.newest)
		{
			currentSortType = SortType.None;
		}
		if (cat == 1)
		{
			currentSortType = SortType.newest;
			lastSortType = -1;
		}
		if (cat == 2)
		{
			currentFavsListString = "blank999";
			if (LevelEditorTools.FavMapsList != null)
			{
				foreach (string favMaps in LevelEditorTools.FavMapsList)
				{
					currentFavsListString = currentFavsListString + favMaps + "999";
				}
			}
		}
		if (cat == 3)
		{
			currentFavsListString = "blank999";
			if (LevelEditorTools.MyMapsList != null)
			{
				foreach (string myMaps in LevelEditorTools.MyMapsList)
				{
					currentFavsListString = currentFavsListString + myMaps + "999";
				}
			}
		}
		LoadMapsPage(0, currentSortType, searchField.text, currentFavsListString);
		UpdateTriangles();
	}

	public void LoadLevelEditor()
	{
		if (MenuManager.Instance.CurrentVehicle == null)
		{
			MenuManager.Instance.ShowMessage("Buy a vehicle first!");
			return;
		}
		StatsData statsData = GameState.LoadStatsData();
		if (!statsData.IsMember)
		{
			MenuManager.Instance.ShowMessage("Level editor is currently available only for Members!");
			return;
		}
		GameState.GameMode = GameMode.SinglePlayer;
		GameState.mapToDownload = string.Empty;
		MenuManager.Instance.SceneLoadingText.text = "Loading level editor...";
		MenuManager.Instance.SceneLoading.SetActive(value: true);
		SceneManager.LoadScene("LevelEditor");
	}

	public void LoadCustomMap()
	{
		if (MenuManager.Instance.CurrentVehicle == null)
		{
			MenuManager.Instance.ShowMessage("Buy a vehicle first!");
			return;
		}
		GameState.mapToDownload = selectedMapID;
		GameState.SceneName = "CustomMap";
		if (GameState.GameMode == GameMode.Multiplayer)
		{
			StartCoroutine(LoadCustomMapInMultiplayer());
			return;
		}
		MenuManager.Instance.SceneLoadingText.text = "Loading community map...";
		MenuManager.Instance.SceneLoading.SetActive(value: true);
		SceneManager.LoadScene("CustomMap");
	}

	public void EditMap()
	{
		if (MenuManager.Instance.CurrentVehicle == null)
		{
			MenuManager.Instance.ShowMessage("Buy a vehicle first!");
			return;
		}
		GameState.mapToDownload = selectedMapID;
		GameState.GameMode = GameMode.SinglePlayer;
		MenuManager.Instance.SceneLoadingText.text = "Loading level editor...";
		MenuManager.Instance.SceneLoading.SetActive(value: true);
		SceneManager.LoadScene("LevelEditor");
	}

	private IEnumerator LoadCustomMapInMultiplayer()
	{
		MenuManager.Instance.SceneLoadingText.text = "Loading trailer...";
		MenuManager.Instance.SceneLoading.SetActive(value: true);
		if (!PhotonNetwork.connectedAndReady || !PhotonNetwork.insideLobby || PhotonNetwork.inRoom)
		{
			int num = 0;
			if (!PhotonNetwork.connectedAndReady)
			{
				num = 1;
			}
			if (!PhotonNetwork.insideLobby)
			{
				num = 2;
			}
			if (PhotonNetwork.inRoom)
			{
				num = 3;
			}
			MenuManager.Instance.ShowMessage("Multiplayer isn't ready yet. Please try again in a moment. Make sure you have Internet access! (" + num.ToString() + ")");
			MenuManager.Instance.SceneLoading.SetActive(value: false);
		}
		else
		{
			MultiplayerManager.JoinRoom();
			for (float time = 0f; time < 10f; time += 1f)
			{
				yield return new WaitForSeconds(1f);
			}
			if (!PhotonNetwork.inRoom)
			{
				MenuManager.Instance.ShowMessage("Can't connect to the room. Try again");
			}
		}
	}

	public void AddMapToFavs(string mapID)
	{
		LevelEditorTools.AddMapToFavs(mapID);
	}

	public void RemoveFromFavs(string mapID)
	{
		LevelEditorTools.RemoveFromFavs(mapID);
	}

	private void Update()
	{
		if (!loadingMaps && pullToLoadMessage.gameObject.activeSelf)
		{
			Vector3 position = pullToLoadMessage.transform.position;
			if (position.y > 100f)
			{
				LoadMapsPage(currentPage + 1, currentSortType, searchField.text, currentFavsListString);
			}
		}
	}
}
