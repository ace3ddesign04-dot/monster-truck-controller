using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampsLoadTest : MonoBehaviour
{
	public List<Texture2D> loadedStamps = new List<Texture2D>();

	[ContextMenu("Load stamps")]
	private void LoadStamps()
	{
		StartCoroutine(LoadStampsCor());
	}

	private IEnumerator LoadStampsCor()
	{
		List<string> stampFileNames = new List<string>();
		WWW listQuery = new WWW("https://keereedev.000webhostapp.com/LoadStamps.php");
		yield return listQuery;
		string[] rawNames = listQuery.text.Split('\n');
		string[] array = rawNames;
		foreach (string text in array)
		{
			if (text != string.Empty)
			{
				stampFileNames.Add(text);
			}
		}
		foreach (string fn in stampFileNames)
		{
			WWW stampQuery = new WWW("https://keereedev.000webhostapp.com/Stamps/" + fn);
			UnityEngine.Debug.Log(stampQuery.url);
			yield return stampQuery;
			loadedStamps.Add(stampQuery.texture);
		}
	}
}
