using Assets.SimpleZip;
using Facebook.Unity;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DataStore
{
	public static List<Key> usedKeys;

	private static int saveInterval;

	private static long lastCloudSave;

	private static bool cloudSaveInProgress;

	public static bool disableCloudSave;

	private static List<string> cloudExceptions;

	private static bool savingCloudKeys;

	private static string uncompressedData;

	static DataStore()
	{
		usedKeys = new List<Key>();
		saveInterval = 10;
		lastCloudSave = 0L;
		cloudSaveInProgress = false;
		disableCloudSave = false;
		cloudExceptions = new List<string>();
		savingCloudKeys = false;
		uncompressedData = string.Empty;
		if (usedKeys.Count == 0)
		{
			string @string = PlayerPrefs.GetString("UsedKeys", string.Empty);
			string[] array = @string.Split('|');
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (text != string.Empty && text != null)
				{
					string[] array3 = text.Split(new char[1]
					{
						'}'
					}, 4);
					if (array3.Length == 4)
					{
						usedKeys.Add(Key.MakeKey(array3[0], long.Parse(array3[1]), (array3[2] == "1") ? true : false, (KeyType)int.Parse(array3[3])));
					}
				}
			}
		}
		cloudExceptions.Add("LastCloudSave");
		lastCloudSave = GetLong("LastCloudSave");
	}

	public static int CurrentFieldFind()
	{
		int num = LastFoundFieldFind();
		if (Utility.FoundAllParts(num.ToString()))
		{
			num++;
		}
		return num;
	}

	public static int LastFoundFieldFind()
	{
		int result = 0;
		for (int i = 1; i < 10; i++)
		{
			if (GetBool("FoundFieldFind" + i.ToString()))
			{
				result = i;
			}
		}
		return result;
	}

	public static bool NeedToCloudSave()
	{
		bool result = false;
		foreach (Key usedKey in usedKeys)
		{
			if (!usedKey.Uploaded && !cloudExceptions.Contains(usedKey.Name))
			{
				return true;
			}
		}
		return result;
	}

	public static void CloudSave(bool ignoreTime = false)
	{
		if (lastCloudSave <= 0 || DateTime.Now.Ticks - lastCloudSave >= TimeSpan.FromSeconds(saveInterval).Ticks || ignoreTime)
		{
			if (lastCloudSave == 0)
			{
				UnityEngine.Debug.Log("No cloud save recorded yet");
				SetLastCloudSave();
			}
			else if (disableCloudSave)
			{
				UnityEngine.Debug.Log("Not saving, disabled right now");
				SetLastCloudSave();
			}
			else if (!NeedToCloudSave())
			{
				UnityEngine.Debug.Log("No keys need to be saved right now");
				SetLastCloudSave();
			}
			else if (!FB.IsInitialized || !FB.IsLoggedIn || !PlayFabClientAPI.IsClientLoggedIn())
			{
				UnityEngine.Debug.Log("Could not save to cloud - not logged into FB or PF");
				SetLastCloudSave();
			}
			else if (!cloudSaveInProgress)
			{
				string value = DumpData();
				cloudSaveInProgress = true;
				UpdateUserDataRequest updateUserDataRequest = new UpdateUserDataRequest();
				updateUserDataRequest.Data = new Dictionary<string, string>
				{
					{
						"PlayerData",
						value
					}
				};
				PlayFabClientAPI.UpdateUserData(updateUserDataRequest, delegate
				{
					UnityEngine.Debug.Log("User data saved to PlayFab");
					SetLastCloudSave();
					cloudSaveInProgress = false;
					foreach (Key usedKey in usedKeys)
					{
						usedKey.Uploaded = true;
					}
					SaveKeysToDisk();
					UnityEngine.Debug.Log("Just saved data.. any to update? " + NeedToCloudSave());
				}, delegate(PlayFabError error)
				{
					UnityEngine.Debug.Log("Error saving user data to PlayFab: " + error.GenerateErrorReport());
					SetLastCloudSave();
					cloudSaveInProgress = false;
				});
			}
		}
	}

	public static void SetLastCloudSave()
	{
		SetLong("LastCloudSave", DateTime.Now.Ticks);
		lastCloudSave = DateTime.Now.Ticks;
	}

	public static string DumpData()
	{
		string text = string.Empty;
		foreach (Key usedKey in usedKeys)
		{
			usedKey.Populate();
			string text2 = text;
			text = text2 + usedKey.Name + "<;>" + usedKey.Type.ToString() + "<;>" + usedKey.ValueAsString() + "<{-_-}>";
		}
		UnityEngine.Debug.Log("Raw Length: " + text.Length);
		string text3 = Zip.CompressToString(text);
		UnityEngine.Debug.Log("Compressed Length: " + text3.Length);
		string text4 = Zip.Decompress(text3);
		UnityEngine.Debug.Log("Uncompressed Length: " + text4.Length);
		return text3;
	}

	public static void DownloadCloudData()
	{
		if (!FB.IsInitialized || !FB.IsLoggedIn || !PlayFabClientAPI.IsClientLoggedIn())
		{
			UnityEngine.Debug.Log("User not logged in to FB or PF, can't download");
			return;
		}
		disableCloudSave = true;
		GetUserDataRequest getUserDataRequest = new GetUserDataRequest();
		getUserDataRequest.Keys = new List<string>
		{
			"PlayerData"
		};
		PlayFabClientAPI.GetUserData(getUserDataRequest, delegate(GetUserDataResult result)
		{
			if (result.Data == null || result.Data.Count == 0)
			{
				UnityEngine.Debug.Log("Looks like there was no cloud data");
				disableCloudSave = false;
				MenuManager.Instance.CloudRestoreComplete(reloadScene: false);
			}
			else
			{
				UserDataRecord userDataRecord = result.Data["PlayerData"];
				string value = userDataRecord.Value;
				if (value == null || value.Length < 10)
				{
					disableCloudSave = false;
					MenuManager.Instance.CloudRestoreComplete(reloadScene: false);
				}
				else
				{
					uncompressedData = Zip.Decompress(value);
					UnityEngine.Debug.Log("Got user data: " + uncompressedData);
					string[] savedVehiclesIDs = MenuManager.Instance.GetSavedVehiclesIDs();
					int num = (savedVehiclesIDs != null) ? savedVehiclesIDs.Length : 0;
					int num2 = 0;
					StatsData statsData = GameState.LoadStatsData();
					StatsData statsData2 = new StatsData();
					if (uncompressedData != null)
					{
						string[] array = uncompressedData.Split(new string[1]
						{
							"<{-_-}>"
						}, StringSplitOptions.None);
						string[] array2 = array;
						foreach (string text in array2)
						{
							string[] array3 = text.Split(new string[1]
							{
								"<;>"
							}, 3, StringSplitOptions.None);
							if (array3.Length == 3)
							{
								if (array3[0].ToUpper().Contains("VEHICLE_"))
								{
									UnityEngine.Debug.Log("VEHICLE: " + text);
									num2++;
								}
								if (array3[0].ToUpper() == "STATS")
								{
									statsData2 = (StatsData)XmlSerialization.DeserializeData<StatsData>(array3[2]);
									UnityEngine.Debug.Log("Stats:\r\n" + array3[2]);
								}
							}
						}
					}
					if (num2 <= num && statsData2.Gold <= statsData.Gold && statsData2.Money <= statsData.Money)
					{
						UnityEngine.Debug.Log("Cloud Vehicles: " + num2);
						UnityEngine.Debug.Log("Local Vehicles: " + num);
						UnityEngine.Debug.Log("Cloud Gold: " + statsData2.Gold);
						UnityEngine.Debug.Log("Local Gold: " + statsData.Gold);
						UnityEngine.Debug.Log("Cloud Money: " + statsData2.Money);
						UnityEngine.Debug.Log("Local Money: " + statsData.Money);
						disableCloudSave = false;
						MenuManager.Instance.CloudRestoreComplete(reloadScene: false);
						uncompressedData = null;
					}
					else if (statsData2.Gold > statsData.Gold && statsData2.Money > statsData.Money && num2 > num)
					{
						ImportCloudData();
					}
					else
					{
						UnityEngine.Debug.Log("Cloud Vehicles: " + num2);
						UnityEngine.Debug.Log("Local Vehicles: " + num);
						UnityEngine.Debug.Log("Cloud Gold: " + statsData2.Gold);
						UnityEngine.Debug.Log("Local Gold: " + statsData.Gold);
						UnityEngine.Debug.Log("Cloud Money: " + statsData2.Money);
						UnityEngine.Debug.Log("Local Money: " + statsData.Money);
						MenuManager.Instance.ShowImportCloudDataBox(statsData.Gold, statsData2.Gold, statsData.Money, statsData2.Money, num, num2);
					}
				}
			}
		}, delegate(PlayFabError error)
		{
			UnityEngine.Debug.Log("Couldn't get user data: " + error.GenerateErrorReport());
			disableCloudSave = false;
		});
	}

	public static void ImportCloudData()
	{
		Clear();
		savingCloudKeys = true;
		string[] array = uncompressedData.Split(new string[1]
		{
			"<{-_-}>"
		}, StringSplitOptions.None);
		string[] array2 = array;
		foreach (string text in array2)
		{
			string[] array3 = text.Split(new string[1]
			{
				"<;>"
			}, 3, StringSplitOptions.None);
			if (array3.Length == 3)
			{
				switch (array3[1])
				{
				case "String":
					SetString(array3[0], array3[2]);
					break;
				case "Int":
					SetInt(array3[0], int.Parse(array3[2]));
					break;
				case "Long":
					SetLong(array3[0], long.Parse(array3[2]));
					break;
				case "Bool":
					SetBool(array3[0], bool.Parse(array3[2]));
					break;
				case "Float":
					SetFloat(array3[0], float.Parse(array3[2]));
					break;
				}
			}
			else
			{
				UnityEngine.Debug.Log("Not 3: " + array3.Length);
			}
		}
		SetInt("GraphicsLevel", 2);
		UnityEngine.Debug.Log("Data downloaded and imported.");
		MenuManager.Instance.CloudRestoreComplete();
		disableCloudSave = false;
		savingCloudKeys = false;
		uncompressedData = null;
	}

	private static void UpdateKey(string key, KeyType type)
	{
		try
		{
			Key key2 = usedKeys.Find((Key k) => k.Name == key && k.Type == type);
			if (key2 == null && key != string.Empty)
			{
				key2 = Key.MakeKey(key, type);
				usedKeys.Add(key2);
			}
			if (!savingCloudKeys)
			{
				key2.Uploaded = false;
			}
			else
			{
				key2.Uploaded = true;
			}
			Key key3 = usedKeys.Find((Key k) => k.Name == key && k.Type == type);
			SaveKeysToDisk();
		}
		catch
		{
			UnityEngine.Debug.Log("Exception adding key for datastore");
		}
	}

	public static void DeleteKey(string key)
	{
		try
		{
			PlayerPrefs.DeleteKey(key);
			if (usedKeys.Find((Key k) => k.Name == key) != null && key != string.Empty)
			{
				usedKeys.Remove(usedKeys.Find((Key k) => k.Name == key));
				SaveKeysToDisk();
			}
		}
		catch
		{
			UnityEngine.Debug.Log("Exception removing key from datastore");
		}
	}

	public static void SaveKeysToDisk()
	{
		string text = string.Empty;
		foreach (Key usedKey in usedKeys)
		{
			string text2 = (!usedKey.Uploaded) ? "0" : "1";
			object[] obj = new object[9]
			{
				text,
				usedKey.Name,
				"}",
				DateTime.Now.Ticks,
				"}",
				text2,
				"}",
				null,
				null
			};
			int type = (int)usedKey.Type;
			obj[7] = type.ToString();
			obj[8] = "|";
			text = string.Concat(obj);
		}
		PlayerPrefs.SetString("UsedKeys", text);
	}

	private static void ClearKeys()
	{
		usedKeys.Clear();
	}

	public static void SetString(string key, string value)
	{
		UpdateKey(key, KeyType.String);
		PlayerPrefs.SetString(key, value);
	}

	public static void SetLong(string key, long value)
	{
		UpdateKey(key, KeyType.Long);
		PlayerPrefs.SetString(key, value.ToString());
	}

	public static void SetBool(string key, bool value)
	{
		UpdateKey(key, KeyType.Bool);
		PlayerPrefs.SetInt(key, value ? 1 : 0);
	}

	public static void SetInt(string key, int value)
	{
		UpdateKey(key, KeyType.Int);
		PlayerPrefs.SetInt(key, value);
	}

	public static void Clear()
	{
		ClearKeys();
		PlayerPrefs.DeleteAll();
	}

	public static bool HasKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	public static void SetFloat(string key, float value)
	{
		UpdateKey(key, KeyType.Float);
		PlayerPrefs.SetFloat(key, value);
	}

	public static string GetString(string key)
	{
		return GetString(key, null);
	}

	public static long GetLong(string key)
	{
		return long.Parse(GetString(key, "0"));
	}

	public static bool GetBool(string key)
	{
		return (GetInt(key, 0) == 1) ? true : false;
	}

	public static int GetInt(string key)
	{
		return GetInt(key, 0);
	}

	public static float GetFloat(string key)
	{
		return GetFloat(key, 0f);
	}

	public static string GetString(string key, string defaultValue)
	{
		return PlayerPrefs.GetString(key, defaultValue);
	}

	public static int GetInt(string key, int defaultValue)
	{
		return PlayerPrefs.GetInt(key, defaultValue);
	}

	public static float GetFloat(string key, float defaultValue)
	{
		return PlayerPrefs.GetFloat(key, defaultValue);
	}
}
