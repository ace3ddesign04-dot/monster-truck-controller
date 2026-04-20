using System;
using System.Collections;
using System.Text;
using UnityEngine;

public class LevelWebHandler : MonoBehaviour
{
	private const string uploadWebAdress = "https://keereedev.000webhostapp.com/UploadFile.php";

	private const string getMapWebAdress = "https://keereedev.000webhostapp.com/GetMaps.php?ID=";

	public void UploadLevel(string data, string fileName, Action successCallback = null, Action errorCallback = null)
	{
		StartCoroutine(UploadLevelCor(data, fileName, successCallback, errorCallback));
	}

	private IEnumerator UploadLevelCor(string data, string fileName, Action successCallback, Action errorCallback)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(data);
		if (fileName == string.Empty)
		{
			UnityEngine.Debug.Log("file name is null");
		}
		WWWForm form = new WWWForm();
		form.AddField("file", "file");
		form.AddBinaryData("file", bytes, fileName, "text/xml");
		bool success = false;
		for (int i = 0; i < 10; i++)
		{
			WWW query = new WWW("https://keereedev.000webhostapp.com/UploadFile.php", form);
			yield return query;
			if (query.error == null)
			{
				success = true;
				break;
			}
		}
		if (success)
		{
			successCallback?.Invoke();
		}
		if (!success)
		{
			errorCallback?.Invoke();
		}
	}

	public void DownloadLevel(string fileName, Action<string> successCallback, Action failCallback = null)
	{
		StartCoroutine(DownloadLeveLCor(fileName, successCallback, failCallback));
	}

	private IEnumerator DownloadLeveLCor(string fileName, Action<string> successCallback, Action failCallback)
	{
		bool success = false;
		string levelData = string.Empty;
		for (int i = 0; i < 10; i++)
		{
			WWW w = new WWW("https://keereedev.000webhostapp.com/GetMaps.php?ID=" + fileName);
			yield return w;
			if (w.error == null)
			{
				success = true;
				levelData = w.text;
				break;
			}
		}
		if (success)
		{
			successCallback?.Invoke(levelData);
		}
		if (!success)
		{
			failCallback?.Invoke();
		}
	}

	public void DownloadLevelsMetadatasPage(int from, int count, SortType sortType, string searchString, string favMapsList, bool includeHidden, Action<string> successCallback, Action<string> failCallback = null)
	{
		StartCoroutine(DownloadLevelsMetadatasCor(from, count, sortType, searchString, favMapsList, includeHidden, successCallback, failCallback));
	}

	private IEnumerator DownloadLevelsMetadatasCor(int from, int count, SortType sortType, string searchString, string favMapsList, bool includeHidden, Action<string> successCallback, Action<string> failCallback)
	{
		string err = string.Empty;
		for (int i = 0; i < 10; i++)
		{
			WWW w = new WWW("https://keereedev.000webhostapp.com/GetMaps.php?meta&from=" + from + "&count=" + count + "&sortType=" + sortType.ToString() + "&search=" + searchString + "&favsList=" + favMapsList + "&includeHidden=" + ((!includeHidden) ? "0" : "1"));
			UnityEngine.Debug.Log(w.url);
			yield return w;
			if (w.error == null)
			{
				successCallback?.Invoke(w.text);
				yield break;
			}
			err = w.error;
		}
		if (err == string.Empty)
		{
			err = "Can't connect to server";
		}
		failCallback?.Invoke(err);
	}
}
