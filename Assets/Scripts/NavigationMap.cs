using CustomVP;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NavigationMap : MonoBehaviour
{
	public Transform[] OtherCars;

	public Texture2D EventStamp;

	public RectTransform PlayerMarker;

	public Image MapBackgroundImage;

	public Image MapElementsImage;

	public Color StartStampColor;

	public Color FinishStampColor;

	public Color PlayerMarkerColor;

	public Color OtherCarsMarkerColor;

	private Terrain terrain;

	private int imageResolution = 400;

	private Texture2D texture;

	private Transform PlayerCar;

	private RectTransform[] OtherCarsMarkers;

	private float terrainSide;

	private void Start()
	{
		Terrain[] activeTerrains = Terrain.activeTerrains;
		if (activeTerrains != null && activeTerrains.Length > 0)
		{
			for (int i = 0; i < activeTerrains.Length; i++)
			{
				if (i == 0)
				{
					terrain = activeTerrains[i];
					continue;
				}
				Vector3 size = activeTerrains[i].terrainData.size;
				float x = size.x;
				Vector3 size2 = terrain.terrainData.size;
				if (x > size2.x)
				{
					terrain = activeTerrains[i];
				}
			}
		}
		if (terrain == null)
		{
			UnityEngine.Debug.LogError("Terrain is not found");
			return;
		}
		Vector3 size3 = terrain.terrainData.size;
		terrainSide = size3.x;
		if (PlayerMarker != null)
		{
			PlayerMarker.GetComponent<Image>().color = PlayerMarkerColor;
		}
		Texture2D texture2D = (Texture2D)Resources.Load("Map/" + SceneManager.GetActiveScene().name + "_MapBackground");
		Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
		MapBackgroundImage.sprite = sprite;
		Texture2D texture2D2 = (Texture2D)Resources.Load("Map/" + SceneManager.GetActiveScene().name + "_MapElements");
		Sprite sprite2 = Sprite.Create(texture2D2, new Rect(0f, 0f, texture2D2.width, texture2D2.height), new Vector2(0.5f, 0.5f));
		MapElementsImage.sprite = sprite2;
	}

	private void Update()
	{
		if (PlayerCar == null)
		{
			FindPlayer();
		}
		else
		{
			AlignPlayerMarker();
		}
		if (OtherCarsMarkers != null)
		{
			if (OtherCarsMarkers.Length == OtherCars.Length)
			{
				AlignOtherCarsMarkers();
			}
			else
			{
				InitializeOtherCarsMarkers();
			}
		}
		else
		{
			InitializeOtherCarsMarkers();
		}
	}

	public void BakeMap()
	{
		Terrain[] activeTerrains = Terrain.activeTerrains;
		if (activeTerrains != null && activeTerrains.Length > 0)
		{
			for (int i = 0; i < activeTerrains.Length; i++)
			{
				if (i == 0)
				{
					terrain = activeTerrains[i];
					continue;
				}
				Vector3 size = activeTerrains[i].terrainData.size;
				float x = size.x;
				Vector3 size2 = terrain.terrainData.size;
				if (x > size2.x)
				{
					terrain = activeTerrains[i];
				}
			}
		}
		if (terrain == null)
		{
			UnityEngine.Debug.LogError("Terrain is not found");
			return;
		}
		Vector3 size3 = terrain.terrainData.size;
		terrainSide = size3.x;
		texture = new Texture2D(imageResolution, imageResolution);
		for (int j = 0; j < imageResolution; j++)
		{
			for (int k = 0; k < imageResolution; k++)
			{
				texture.SetPixel(j, k, Color.clear);
			}
		}
		Route[] array = UnityEngine.Object.FindObjectsOfType<Route>();
		for (int l = 0; l < array.Length; l++)
		{
			Vector3 position = array[l].Waypoints[0].position;
			DrawIcon(StartStampColor, position);
			Vector3 worldPos = array[l].Waypoints[array[l].Waypoints.Count - 1].position;
			if (array[l].Circuit)
			{
				worldPos = position;
			}
			DrawIcon(FinishStampColor, worldPos);
			for (int m = 0; m < array[l].Waypoints.Count; m++)
			{
				if (array[l].Waypoints.Count > m + 1)
				{
					DrawLine(array[l].RouteColor, array[l].Waypoints[m].position, array[l].Waypoints[m + 1].position);
				}
				else if (array[l].Circuit)
				{
					DrawLine(array[l].RouteColor, array[l].Waypoints[m].position, array[l].Waypoints[0].position);
				}
			}
		}
		texture.Apply();
		Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
		MapElementsImage.sprite = sprite;
		byte[] buffer = texture.EncodeToPNG();
		FileStream fileStream = File.Open(Application.dataPath + "/Resources/Map/" + SceneManager.GetActiveScene().name + "_MapElements.png", FileMode.Create);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		binaryWriter.Write(buffer);
		fileStream.Close();
	}

	private void DrawIcon(Color color, Vector3 WorldPos)
	{
		int x = GetTexCoords(WorldPos).x;
		int y = GetTexCoords(WorldPos).y;
		int num = x - EventStamp.width / 2;
		int num2 = y - EventStamp.height / 2;
		int num3 = x + EventStamp.width / 2;
		int num4 = y + EventStamp.height / 2;
		for (int i = num; i < num3; i++)
		{
			for (int j = num2; j < num4; j++)
			{
				int x2 = i - x + EventStamp.width / 2;
				int y2 = j - y + EventStamp.height / 2;
				texture.SetPixel(i, j, EventStamp.GetPixel(x2, y2) * color);
			}
		}
	}

	private void DrawLine(Color color, Vector3 A, Vector3 B)
	{
		Vector2Int texCoords = GetTexCoords(A);
		Vector2Int texCoords2 = GetTexCoords(B);
		DrawBresenhamsAlghorytmLine(texCoords.x, texCoords.y, texCoords2.x, texCoords2.y, color);
	}

	public void DrawBresenhamsAlghorytmLine(int x, int y, int x2, int y2, Color color)
	{
		int num = x2 - x;
		int num2 = y2 - y;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		if (num < 0)
		{
			num3 = -1;
		}
		else if (num > 0)
		{
			num3 = 1;
		}
		if (num2 < 0)
		{
			num4 = -1;
		}
		else if (num2 > 0)
		{
			num4 = 1;
		}
		if (num < 0)
		{
			num5 = -1;
		}
		else if (num > 0)
		{
			num5 = 1;
		}
		int num7 = Mathf.Abs(num);
		int num8 = Mathf.Abs(num2);
		if (num7 <= num8)
		{
			num7 = Mathf.Abs(num2);
			num8 = Mathf.Abs(num);
			if (num2 < 0)
			{
				num6 = -1;
			}
			else if (num2 > 0)
			{
				num6 = 1;
			}
			num5 = 0;
		}
		int num9 = num7 >> 1;
		for (int i = 0; i <= num7; i++)
		{
			texture.SetPixel(x, y, color);
			num9 += num8;
			if (num9 >= num7)
			{
				num9 -= num7;
				x += num3;
				y += num4;
			}
			else
			{
				x += num5;
				y += num6;
			}
		}
	}

	private Vector2Int GetTexCoords(Vector3 WorldPos)
	{
		Vector3 vector = WorldPos - terrain.transform.position;
		int x = (int)(vector.x / terrainSide * (float)imageResolution);
		Vector3 vector2 = WorldPos - terrain.transform.position;
		int y = (int)(vector2.z / terrainSide * (float)imageResolution);
		return new Vector2Int(x, y);
	}

	private void AlignPlayerMarker()
	{
		if (!(PlayerMarker == null))
		{
			PlayerMarker.gameObject.SetActive(value: true);
			PlayerMarker.anchoredPosition = GetTexCoords(PlayerCar.position);
			RectTransform playerMarker = PlayerMarker;
			Vector3 eulerAngles = PlayerCar.eulerAngles;
			playerMarker.eulerAngles = new Vector3(0f, 0f, 0f - eulerAngles.y + 90f);
		}
	}

	private void AlignOtherCarsMarkers()
	{
		if (OtherCars == null || OtherCars.Length == 0)
		{
			return;
		}
		for (int i = 0; i < OtherCarsMarkers.Length; i++)
		{
			if (OtherCars[i] != null && OtherCarsMarkers[i] != null)
			{
				OtherCarsMarkers[i].anchoredPosition = GetTexCoords(OtherCars[i].position);
				RectTransform obj = OtherCarsMarkers[i];
				Vector3 eulerAngles = OtherCars[i].eulerAngles;
				obj.eulerAngles = new Vector3(0f, 0f, 0f - eulerAngles.y + 90f);
			}
		}
	}

	private void InitializeOtherCarsMarkers()
	{
		if (OtherCarsMarkers != null)
		{
			for (int i = 0; i < OtherCarsMarkers.Length; i++)
			{
				UnityEngine.Object.DestroyImmediate(OtherCarsMarkers[i].gameObject);
			}
		}
		OtherCarsMarkers = new RectTransform[0];
		if (OtherCars != null && OtherCars.Length != 0 && !(PlayerMarker == null))
		{
			OtherCarsMarkers = new RectTransform[OtherCars.Length];
			for (int j = 0; j < OtherCarsMarkers.Length; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(PlayerMarker.gameObject);
				gameObject.transform.parent = PlayerMarker.transform.parent;
				OtherCarsMarkers[j] = gameObject.GetComponent<RectTransform>();
				OtherCarsMarkers[j].GetComponent<Image>().color = OtherCarsMarkerColor;
				OtherCarsMarkers[j].gameObject.SetActive(value: true);
			}
		}
	}

	private void FindPlayer()
	{
		if (PlayerMarker != null)
		{
			PlayerMarker.gameObject.SetActive(value: false);
		}
		CarController carController = UnityEngine.Object.FindObjectOfType<CarController>();
		if (carController != null)
		{
			PlayerCar = carController.transform;
		}
	}
}
