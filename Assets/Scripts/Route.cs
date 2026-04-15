using CustomVP;
using PlayFab;
using PlayFab.AdminModels;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Route : MonoBehaviour
{
	[SerializeField]
	public List<Transform> Waypoints;

	[SerializeField]
	public Color RouteColor;

	[SerializeField]
	public bool ThickLine = true;

	[SerializeField]
	public Transform StartWP;

	[SerializeField]
	public Transform EndWP;

	[SerializeField]
	public bool DrawTextureOnTerrain;

	[SerializeField]
	public Texture2D RouteTexture;

	[SerializeField]
	public Texture2D StampTexture;

	[SerializeField]
	public bool RandomStep;

	[SerializeField]
	public float MinStep;

	[SerializeField]
	public float MaxStep;

	[SerializeField]
	public float StampStep;

	[SerializeField]
	public int StampDiameter;

	[SerializeField]
	public bool RandomOpacity;

	[SerializeField]
	public float TargetOpacity;

	[SerializeField]
	public float[,,] undoAlphaData;

	[SerializeField]
	public GameObject StartPrefab;

	[SerializeField]
	public GameObject FinishPrefab;

	[SerializeField]
	public GameObject EventMarkPrefab;

	[SerializeField]
	public GameObject[] CheckpointsPrefabs;

	[SerializeField]
	public long RouteRecord;

	[SerializeField]
	public bool RecordLoaded;

	[SerializeField]
	public List<RouteGoal> RouteGoals;

	[SerializeField]
	public int MinimumXP;

	[SerializeField]
	public VehicleType RecommendedVehicleType;

	[SerializeField]
	public Transform CheckpointsHolder;

	[SerializeField]
	public GameObject SpawnedStart;

	[SerializeField]
	public GameObject SpawnedFinish;

	[SerializeField]
	public GameObject SpawnedEventMark;

	[SerializeField]
	public List<GameObject> SpawnedCheckpoints;

	[SerializeField]
	public List<GameObject> SpawnedFences;

	[SerializeField]
	public List<LineRenderer> SpawnedRopes;

	[SerializeField]
	public List<GameObject> SpawnedBlocks;

	[SerializeField]
	public List<RopeFencedSegment> RopeFencedSegments;

	[SerializeField]
	public List<BlockFencedSegment> BlockFencedSegments;

	[SerializeField]
	public int CheckpointsNumber;

	[SerializeField]
	public bool Circuit;

	[SerializeField]
	public bool Completed;

	[SerializeField]
	public int LapsNumber;

	[SerializeField]
	public string RouteName;

	[SerializeField]
	public string EventDescription;

	[SerializeField]
	public string FinishText;

	[SerializeField]
	public int DelayTime;

	[SerializeField]
	public bool AlwaysShowCheckpoints;

	[SerializeField]
	public float DistanceBetweenBlocks;

	[SerializeField]
	public RouteManager Manager;

	public bool TrailblazerEligible;

	[Header("Multiplayer trail")]
	public bool MultiplayerTrail;

	public string MultiplayerName;

	public Transform Player1StartPos;

	public Transform Player2StartPos;

	[SerializeField]
	public LightTree lightTree;

	private VehicleDataManager vehicleDataManager
	{
		get
		{
			if (VehicleLoader.Instance != null)
			{
				return VehicleLoader.Instance.playerDataManager;
			}
			return null;
		}
	}

	private void Start()
	{
		if (!Circuit)
		{
			LapsNumber = 1;
		}
		for (int i = 0; i < SpawnedCheckpoints.Count; i++)
		{
			Checkpoint checkpoint = SpawnedCheckpoints[i].AddComponent<Checkpoint>();
			checkpoint.ID = i;
			BoxCollider boxCollider = checkpoint.gameObject.AddComponent<BoxCollider>();
			boxCollider.size = new Vector3(5f, 10f, 1f);
			boxCollider.isTrigger = true;
		}
		if (GameState.GameType == GameType.TrailRace)
		{
			GameObject gameObject = GameObject.Find("RouteIndicator(Clone)");
			if (gameObject != null)
			{
				gameObject.SetActive(value: false);
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = RouteColor;
		for (int i = 0; i < Waypoints.Count; i++)
		{
			if (Waypoints.Count > i + 1)
			{
				DrawThickLine(Waypoints[i].position, Waypoints[i + 1].position);
			}
			else if (Circuit)
			{
				DrawThickLine(Waypoints[i].position, Waypoints[0].position);
			}
		}
		for (int j = 0; j < Waypoints.Count; j++)
		{
			DrawNormalizedSphere(Waypoints[j].position, 0.5f);
		}
		if (DrawTextureOnTerrain)
		{
			Gizmos.DrawWireCube(base.transform.position + Vector3.up, new Vector3(StampDiameter * 2, 0f, StampDiameter * 2));
		}
	}

	private void DrawNormalizedSphere(Vector3 center, float radius)
	{
		if (!ThickLine)
		{
			Gizmos.DrawSphere(center, 0.25f);
			return;
		}
		Camera current = Camera.current;
		float num = Vector3.Distance(current.transform.position, center);
		Gizmos.DrawSphere(center, 0.02f * num);
	}

	private void DrawThickLine(Vector3 Point1, Vector3 Point2)
	{
		if (!ThickLine)
		{
			Gizmos.DrawLine(Point1, Point2);
			return;
		}
		Camera current = Camera.current;
		Vector3 normalized = (Point1 - Point2).normalized;
		Vector3 a = Quaternion.Euler(0f, 90f, 0f) * normalized;
		float d = Vector3.Distance(current.transform.position, (Point1 + Point2) / 2f);
		for (int i = 0; i < 5; i++)
		{
			Vector3 b = a / 100f * d * ((float)i / 9f);
			Gizmos.DrawLine(Point1 + b, Point2 + b);
			Gizmos.DrawLine(Point1 - b, Point2 - b);
		}
	}

	public void HideShowAllCheckpoints(bool Show)
	{
		if (SpawnedCheckpoints != null)
		{
			foreach (GameObject spawnedCheckpoint in SpawnedCheckpoints)
			{
				spawnedCheckpoint.SetActive(Show);
			}
		}
		if (SpawnedFences != null)
		{
			foreach (GameObject spawnedFence in SpawnedFences)
			{
				spawnedFence.SetActive(Show);
			}
		}
		if (SpawnedRopes != null)
		{
			foreach (LineRenderer spawnedRope in SpawnedRopes)
			{
				spawnedRope.gameObject.SetActive(Show);
			}
		}
	}

	public void HideShowStartAndFinish(bool Show)
	{
		if (SpawnedFinish != null)
		{
			SpawnedFinish.SetActive(Show);
		}
		if (SpawnedStart != null)
		{
			SpawnedStart.SetActive(Show);
		}
	}

	public void HideShowEventMark(bool Show)
	{
		if (SpawnedEventMark != null)
		{
			SpawnedEventMark.SetActive(Show);
		}
	}

	public void ShowCheckpoint(int ID)
	{
		if (!AlwaysShowCheckpoints)
		{
			for (int i = 0; i < SpawnedCheckpoints.Count; i++)
			{
				SpawnedCheckpoints[i].SetActive(ID == i);
			}
		}
		if (AlwaysShowCheckpoints)
		{
			for (int j = 0; j < SpawnedCheckpoints.Count; j++)
			{
				SpawnedCheckpoints[j].SetActive(j >= ID);
			}
		}
		if (SpawnedFences != null)
		{
			foreach (GameObject spawnedFence in SpawnedFences)
			{
				spawnedFence.SetActive(value: true);
			}
		}
		if (SpawnedRopes != null)
		{
			foreach (LineRenderer spawnedRope in SpawnedRopes)
			{
				spawnedRope.gameObject.SetActive(value: true);
			}
		}
	}

	public void RedrawRoute()
	{
		foreach (Transform waypoint in Waypoints)
		{
			AlignObjectByGround(waypoint.gameObject);
			waypoint.position += Vector3.up;
		}
		for (int i = 0; i < Waypoints.Count; i++)
		{
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			if (i + 1 < Waypoints.Count && i - 1 >= 0)
			{
				zero = Waypoints[i - 1].position - Waypoints[i].position;
				zero2 = Waypoints[i + 1].position - Waypoints[i].position;
				if (Vector3.Angle(zero, zero2) < 90f)
				{
					UnityEngine.Debug.LogError("Waypoint #" + i + " has acute angle. To avoid bugs please make it obtuse.");
				}
				Waypoints[i].rotation = Quaternion.LookRotation(zero.normalized + zero2.normalized);
				if (Vector3.Dot(Waypoints[i].forward, Waypoints[i - 1].forward) < 0f)
				{
					Waypoints[i].localEulerAngles += new Vector3(0f, 180f, 0f);
				}
			}
			if (i == 0)
			{
				if (!Circuit)
				{
					Waypoints[i].LookAt(Waypoints[i + 1]);
					Waypoints[i].eulerAngles += new Vector3(0f, 90f, 0f);
				}
				else
				{
					zero = Waypoints[1].position - Waypoints[0].position;
					zero2 = Waypoints[Waypoints.Count - 1].position - Waypoints[0].position;
					Waypoints[i].rotation = Quaternion.LookRotation(zero.normalized + zero2.normalized);
				}
			}
			if (i == Waypoints.Count - 1)
			{
				if (!Circuit)
				{
					Waypoints[i].LookAt(Waypoints[i - 1]);
					Waypoints[i].eulerAngles += new Vector3(0f, -90f, 0f);
				}
				else
				{
					zero = Waypoints[0].position - Waypoints[i].position;
					zero2 = Waypoints[i - 1].position - Waypoints[i].position;
					Waypoints[i].rotation = Quaternion.LookRotation(zero.normalized + zero2.normalized);
				}
			}
		}
	}

	private void FindCheckpointsHolder()
	{
		if (CheckpointsHolder == null)
		{
			CheckpointsHolder = base.transform.Find("Checkpoints");
		}
		if (CheckpointsHolder == null)
		{
			GameObject gameObject = new GameObject("Checkpoints");
			gameObject.transform.parent = base.transform;
			CheckpointsHolder = gameObject.transform;
		}
	}

	public void CreateFenceForAllWaypoints()
	{
		float segmentWidth = RopeFencedSegments[0].SegmentWidth;
		GameObject fencePrefab = RopeFencedSegments[0].FencePrefab;
		RopeFencedSegments = new List<RopeFencedSegment>();
		int num = (!Circuit) ? 1 : 0;
		for (int i = 0; i < Waypoints.Count - num; i++)
		{
			RopeFencedSegment ropeFencedSegment = new RopeFencedSegment();
			ropeFencedSegment.SegmentWidth = segmentWidth;
			ropeFencedSegment.FencePrefab = fencePrefab;
			ropeFencedSegment.StartWaypoint = i;
			RopeFencedSegments.Add(ropeFencedSegment);
		}
		PlaceRopeFence();
	}

	public void PlaceStartPrefab()
	{
		if (!(StartPrefab == null))
		{
			if (SpawnedStart != null)
			{
				UnityEngine.Object.DestroyImmediate(SpawnedStart);
			}
			FindCheckpointsHolder();
			SpawnedStart = UnityEngine.Object.Instantiate(StartPrefab);
			SpawnedStart.transform.position = Waypoints[0].position;
			Vector3 position = Waypoints[1].position;
			Vector3 position2 = SpawnedStart.transform.position;
			position.y = position2.y;
			SpawnedStart.transform.rotation = Quaternion.LookRotation(position - SpawnedStart.transform.position, Vector3.up);
			SpawnedStart.transform.parent = CheckpointsHolder;
		}
	}

	public void PlaceFinishPrefab()
	{
		if (!(FinishPrefab == null) && !Circuit)
		{
			if (SpawnedFinish != null)
			{
				UnityEngine.Object.DestroyImmediate(SpawnedFinish);
			}
			FindCheckpointsHolder();
			SpawnedFinish = UnityEngine.Object.Instantiate(FinishPrefab);
			SpawnedFinish.transform.position = Waypoints[Waypoints.Count - 1].position;
			Vector3 position = Waypoints[Waypoints.Count - 2].position;
			Vector3 position2 = SpawnedFinish.transform.position;
			position.y = position2.y;
			SpawnedFinish.transform.rotation = Quaternion.LookRotation(position - SpawnedFinish.transform.position, Vector3.up);
			SpawnedFinish.transform.eulerAngles += new Vector3(0f, 180f, 0f);
			SpawnedFinish.transform.parent = CheckpointsHolder;
		}
	}

	public void PlaceEventMarkPrefab()
	{
		if (!(EventMarkPrefab == null))
		{
			if (SpawnedEventMark != null)
			{
				UnityEngine.Object.DestroyImmediate(SpawnedEventMark);
			}
			SpawnedEventMark = UnityEngine.Object.Instantiate(EventMarkPrefab);
			SpawnedEventMark.transform.position = Waypoints[0].position;
			Vector3 position = Waypoints[1].position;
			Vector3 position2 = SpawnedEventMark.transform.position;
			position.y = position2.y;
			SpawnedEventMark.transform.rotation = Quaternion.LookRotation(position - SpawnedEventMark.transform.position);
			SpawnedEventMark.transform.parent = base.transform;
			AlignObjectByGround(SpawnedEventMark);
		}
	}

	public void UndoDrawingTexture()
	{
		if (Physics.Raycast(base.transform.position, Vector3.down, out RaycastHit hitInfo))
		{
			if (hitInfo.collider.GetType() != typeof(TerrainCollider))
			{
				UnityEngine.Debug.LogError("Terrain is not found under route");
				return;
			}
			Terrain component = hitInfo.collider.GetComponent<Terrain>();
			TerrainData terrainData = component.terrainData;
			terrainData.SetAlphamaps(0, 0, undoAlphaData);
		}
	}

	public void ClearUndo()
	{
		undoAlphaData = null;
	}

	public void DrawTexture()
	{
		if (!Physics.Raycast(base.transform.position, Vector3.down, out RaycastHit hitInfo))
		{
			return;
		}
		if (hitInfo.collider.GetType() != typeof(TerrainCollider))
		{
			UnityEngine.Debug.LogError("Terrain is not found under route");
			return;
		}
		Terrain component = hitInfo.collider.GetComponent<Terrain>();
		TerrainData terrainData = component.terrainData;
		if (StampStep == 0f)
		{
			StampStep = 2f;
		}
		float num = 0f;
		float[] array = new float[Waypoints.Count];
		for (int i = 0; i < Waypoints.Count; i++)
		{
			array[i] = num;
			if (Waypoints.Count > i + 1)
			{
				num += Vector3.Distance(Waypoints[i].position, Waypoints[i + 1].position);
			}
		}
		if (Circuit)
		{
			num += Vector3.Distance(Waypoints[Waypoints.Count - 1].position, Waypoints[0].position);
		}
		SplatPrototype[] splatPrototypes = terrainData.splatPrototypes;
		int num2 = -1;
		for (int j = 0; j < splatPrototypes.Length; j++)
		{
			if (splatPrototypes[j].texture.Equals(RouteTexture))
			{
				num2 = j;
			}
		}
		if (undoAlphaData == null)
		{
			undoAlphaData = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
		}
		if (num2 < 0)
		{
			List<SplatPrototype> list = new List<SplatPrototype>();
			for (int k = 0; k < splatPrototypes.Length; k++)
			{
				list.Add(splatPrototypes[k]);
			}
			SplatPrototype splatPrototype = new SplatPrototype();
			splatPrototype.texture = RouteTexture;
			list.Add(splatPrototype);
			terrainData.splatPrototypes = list.ToArray();
			num2 = list.Count - 1;
		}
		float num3 = (!RandomStep) ? StampStep : UnityEngine.Random.Range(MinStep, MaxStep);
		for (float num4 = 0f; num4 <= num; num4 += num3)
		{
			if (RandomStep)
			{
				num3 = UnityEngine.Random.Range(MinStep, MaxStep);
			}
			int num5 = 0;
			int num6 = 0;
			bool flag = false;
			for (int l = 0; l < Waypoints.Count; l++)
			{
				if (array[l] > num4)
				{
					num5 = l - 1;
					num6 = l;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				num5 = Waypoints.Count - 1;
				num6 = 0;
			}
			float num7 = 0f;
			Vector3 a = Vector3.Lerp(t: (!flag) ? ((num4 - array[num5]) / (num - array[num5])) : ((num4 - array[num5]) / (array[num6] - array[num5])), a: Waypoints[num5].position, b: Waypoints[num6].position);
			Vector2 vector = default(Vector3);
			Vector3 vector2 = a - component.GetPosition();
			float x = vector2.x;
			Vector3 size = terrainData.size;
			vector.x = x / size.x * (float)terrainData.heightmapResolution;
			Vector3 vector3 = a - component.GetPosition();
			float z = vector3.z;
			Vector3 size2 = terrainData.size;
			vector.y = z / size2.z * (float)terrainData.heightmapResolution;
			int x2 = Mathf.Clamp((int)vector.x - StampDiameter / 2, 0, terrainData.alphamapResolution - StampDiameter / 2 - 2);
			int y = Mathf.Clamp((int)vector.y - StampDiameter / 2, 0, terrainData.alphamapResolution - StampDiameter / 2 - 2);
			float[,,] alphamaps = terrainData.GetAlphamaps(x2, y, StampDiameter, StampDiameter);
			float num8 = (!RandomOpacity) ? TargetOpacity : UnityEngine.Random.Range(0f, 1f);
			for (int m = 0; m < StampDiameter; m++)
			{
				for (int n = 0; n < StampDiameter; n++)
				{
					int x3 = (int)((float)m / (float)StampDiameter * (float)StampTexture.width);
					int y2 = (int)((float)n / (float)StampDiameter * (float)StampTexture.height);
					Color pixel = StampTexture.GetPixel(x3, y2);
					float num9 = pixel.r * num8;
					for (int num10 = 0; num10 < terrainData.splatPrototypes.Length; num10++)
					{
						if (terrainData.splatPrototypes[num10].texture == RouteTexture)
						{
							alphamaps[n, m, num10] += num9;
						}
						else
						{
							alphamaps[n, m, num10] -= num9;
						}
						alphamaps[n, m, num10] = Mathf.Clamp01(alphamaps[n, m, num10]);
					}
				}
			}
			terrainData.SetAlphamaps(x2, y, alphamaps);
		}
	}

	public void PlaceCheckpointsPrefabs()
	{
		if (SpawnedCheckpoints != null)
		{
			for (int i = 0; i < SpawnedCheckpoints.Count; i++)
			{
				if (SpawnedCheckpoints[i] != null)
				{
					UnityEngine.Object.DestroyImmediate(SpawnedCheckpoints[i]);
				}
			}
		}
		if (CheckpointsPrefabs == null || CheckpointsPrefabs.Length == 0)
		{
			return;
		}
		GameObject[] checkpointsPrefabs = CheckpointsPrefabs;
		foreach (GameObject x in checkpointsPrefabs)
		{
			if (x == null)
			{
				UnityEngine.Debug.LogError("One of waypoints prefabs is null");
				return;
			}
		}
		if (CheckpointsNumber == 0)
		{
			return;
		}
		float num = 0f;
		float[] array = new float[Waypoints.Count];
		for (int k = 0; k < Waypoints.Count; k++)
		{
			array[k] = num;
			if (Waypoints.Count > k + 1)
			{
				num += Vector3.Distance(Waypoints[k].position, Waypoints[k + 1].position);
			}
		}
		if (Circuit)
		{
			num += Vector3.Distance(Waypoints[Waypoints.Count - 1].position, Waypoints[0].position);
		}
		float num2 = num / (float)(CheckpointsNumber + 1);
		SpawnedCheckpoints = new List<GameObject>();
		FindCheckpointsHolder();
		for (float num3 = num2; num3 <= num - num2; num3 += num2)
		{
			int num4 = 0;
			int num5 = 0;
			bool flag = false;
			for (int l = 0; l < Waypoints.Count; l++)
			{
				if (array[l] > num3)
				{
					num4 = l - 1;
					num5 = l;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				num4 = Waypoints.Count - 1;
				num5 = 0;
			}
			float num6 = 0f;
			Vector3 position = Vector3.Lerp(t: (!flag) ? ((num3 - array[num4]) / (num - array[num4])) : ((num3 - array[num4]) / (array[num5] - array[num4])), a: Waypoints[num4].position, b: Waypoints[num5].position);
			GameObject gameObject = UnityEngine.Object.Instantiate(CheckpointsPrefabs[UnityEngine.Random.Range(0, CheckpointsPrefabs.Length)]);
			gameObject.transform.position = position;
			AlignObjectByGround(gameObject);
			gameObject.transform.parent = CheckpointsHolder.transform;
			SpawnedCheckpoints.Add(gameObject);
		}
		for (int m = 0; m < SpawnedCheckpoints.Count; m++)
		{
			Vector3 position2 = SpawnedCheckpoints[m].transform.position;
			if (m + 1 < SpawnedCheckpoints.Count)
			{
				Vector3 position3 = SpawnedCheckpoints[m + 1].transform.position;
				position3.y = position2.y;
				SpawnedCheckpoints[m].transform.rotation = Quaternion.LookRotation(position3 - position2, Vector3.up);
			}
			else if (SpawnedFinish != null)
			{
				Vector3 position4 = SpawnedFinish.transform.position;
				position4.y = position2.y;
				SpawnedCheckpoints[m].transform.rotation = Quaternion.LookRotation(position4 - position2, Vector3.up);
			}
			else
			{
				Vector3 position5 = SpawnedStart.transform.position;
				position5.y = position2.y;
				SpawnedCheckpoints[m].transform.rotation = Quaternion.LookRotation(position5 - position2, Vector3.up);
			}
		}
		foreach (GameObject spawnedCheckpoint in SpawnedCheckpoints)
		{
			for (int n = 0; n < spawnedCheckpoint.transform.childCount; n++)
			{
				AlignObjectByGround(spawnedCheckpoint.transform.GetChild(n).gameObject);
			}
		}
	}

	private void AlignObjectByGround(GameObject go)
	{
		Vector3 position = go.transform.position;
		HideShowAllCheckpoints(Show: false);
		HideShowStartAndFinish(Show: false);
		HideShowEventMark(Show: false);
		go.SetActive(value: false);
		if (Physics.Raycast(position + Vector3.up * 10f, Vector3.down, out RaycastHit hitInfo))
		{
			Vector3 point = hitInfo.point;
			position.y = point.y;
		}
		go.SetActive(value: true);
		HideShowAllCheckpoints(Show: true);
		HideShowStartAndFinish(Show: true);
		HideShowEventMark(Show: true);
		MeshFilter meshFilter = go.GetComponent<MeshFilter>();
		if (meshFilter == null)
		{
			meshFilter = go.GetComponentInChildren<MeshFilter>();
		}
		if (meshFilter != null)
		{
			Mesh sharedMesh = meshFilter.sharedMesh;
			float y = position.y;
			Vector3 min = sharedMesh.bounds.min;
			float y2 = min.y;
			Vector3 lossyScale = go.transform.lossyScale;
			position.y = y - y2 * lossyScale.y;
		}
		go.transform.position = position;
	}

	public RoutePayment GetRoutePayment(VehicleType vehicleType, int completionTime, int winchCount, int flipCount, float damageCount)
	{
		RoutePayment result = null;
		RouteGoal routeGoal = RouteGoals.Find((RouteGoal goal) => goal.VehicleType == vehicleType);
		if (routeGoal == null)
		{
			routeGoal = RouteGoal.Default(RouteRecord, this, vehicleType);
		}
		if (routeGoal != null)
		{
			result = routeGoal.GetPayment(completionTime, winchCount, flipCount, damageCount);
		}
		return result;
	}

	private DateTime DateTimeFromMinutes(int minutes)
	{
		TimeSpan t = TimeSpan.FromMinutes(minutes);
		DateTime d = new DateTime(2018, 1, 1, 0, 0, 0);
		return d + t;
	}

	private int GetCurrentMinutes()
	{
		DateTime d = new DateTime(2018, 1, 1, 0, 0, 0);
		return (int)(DateTime.Now - d).TotalMinutes;
	}

	public void SubmitTime(int time, bool fromError = false)
	{
		int @int = DataStore.GetInt(Manager.mapName + RouteName + vehicleDataManager.vehicleType.ToString());
		UnityEngine.Debug.Log("This time: " + time);
		UnityEngine.Debug.Log("Stored time: " + @int);
		if (@int == 0 || @int >= time)
		{
			DataStore.SetInt(Manager.mapName + RouteName + vehicleDataManager.vehicleType.ToString(), time);
			UnityEngine.Debug.Log("Submitting time");
			if (PlayFabClientAPI.IsClientLoggedIn())
			{
				UnityEngine.Debug.Log("Client was logged in");
				UpdatePlayerStatisticsRequest updatePlayerStatisticsRequest = new UpdatePlayerStatisticsRequest();
				updatePlayerStatisticsRequest.Statistics = new List<StatisticUpdate>
				{
					new StatisticUpdate
					{
						StatisticName = Manager.mapName + RouteName + vehicleDataManager.vehicleType.ToString(),
						Value = -time
					}
				};
				PlayFabClientAPI.UpdatePlayerStatistics(updatePlayerStatisticsRequest, delegate
				{
					UnityEngine.Debug.Log("Time submitted");
				}, delegate(PlayFabError error)
				{
					UnityEngine.Debug.Log("Statistic not found!");
					if (error.Error == PlayFabErrorCode.StatisticNotFound && !fromError)
					{
						UnityEngine.Debug.Log("Creating statistic.");
						PlayFabAdminAPI.CreatePlayerStatisticDefinition(new CreatePlayerStatisticDefinitionRequest
						{
							AggregationMethod = StatisticAggregationMethod.Max,
							StatisticName = Manager.mapName + RouteName + vehicleDataManager.vehicleType.ToString(),
							VersionChangeInterval = StatisticResetIntervalOption.Never
						}, delegate
						{
							UnityEngine.Debug.Log("Created Statistic, submitting again.");
							SubmitTime(time, fromError: true);
						}, delegate(PlayFabError statsError)
						{
							UnityEngine.Debug.Log("Could not create statistic: " + statsError.GenerateErrorReport());
						});
					}
					else
					{
						UnityEngine.Debug.Log("Error: " + error.GenerateErrorReport());
					}
				});
			}
		}
	}

	public void RefreshRecord()
	{
		long @long = DataStore.GetLong(Manager.mapName + RouteName + "Refreshed");
		if (!PlayFabClientAPI.IsClientLoggedIn())
		{
			GetLeaderboardError(new PlayFabError());
		}
		else if (@long < DateTime.Now.Ticks - TimeSpan.FromMinutes(Manager.RefreshInterval).Ticks)
		{
			UnityEngine.Debug.Log("Need to refresh route record!");
			RecordLoaded = false;
			if (PlayFabClientAPI.IsClientLoggedIn())
			{
				UnityEngine.Debug.Log("Getting Leaderboard: " + Manager.mapName + RouteName + vehicleDataManager.vehicleType.ToString());
				GetLeaderboardRequest getLeaderboardRequest = new GetLeaderboardRequest();
				getLeaderboardRequest.StatisticName = Manager.mapName + RouteName + vehicleDataManager.vehicleType.ToString();
				getLeaderboardRequest.StartPosition = 0;
				getLeaderboardRequest.MaxResultsCount = 10;
				GetLeaderboardRequest request = getLeaderboardRequest;
				PlayFabClientAPI.GetLeaderboard(request, GotLeaderboard, GetLeaderboardError);
			}
		}
		else
		{
			TrailblazerEligible = false;
		}
	}

	public string GetRecordKeeper()
	{
		return DataStore.GetString(Manager.mapName + RouteName + vehicleDataManager.vehicleType.ToString() + "RecordKeeper");
	}

	public void GotLeaderboard(GetLeaderboardResult result)
	{
		if (result != null && result.Leaderboard != null && result.Leaderboard.Count > 0 && result.Leaderboard[0].StatValue < 0)
		{
			RouteRecord = -result.Leaderboard[0].StatValue;
			UnityEngine.Debug.Log("DisplayName:" + result.Leaderboard[0].DisplayName + "; PlayFabID: " + result.Leaderboard[0].PlayFabId);
			DataStore.SetLong(Manager.mapName + RouteName + vehicleDataManager.vehicleType.ToString(), RouteRecord);
			DataStore.SetLong(Manager.mapName + RouteName + vehicleDataManager.vehicleType.ToString() + "Refreshed", DateTime.Now.Ticks);
			DataStore.SetString(Manager.mapName + RouteName + vehicleDataManager.vehicleType.ToString() + "RecordKeeper", result.Leaderboard[0].DisplayName);
		}
		if (result.Leaderboard.Count == 0)
		{
			TrailblazerEligible = true;
		}
		RecordLoaded = true;
		CarUIControl.Instance.DisplayEventInfo();
	}

	public void GetLeaderboardError(PlayFabError error)
	{
		if (error.Error == PlayFabErrorCode.StatisticNotFound)
		{
			TrailblazerEligible = true;
		}
		RecordLoaded = true;
		CarUIControl.Instance.DisplayEventInfo();
	}

	public void PlaceRopeFence()
	{
		if (RopeFencedSegments == null)
		{
			return;
		}
		if (SpawnedFences != null)
		{
			for (int i = 0; i < SpawnedFences.Count; i++)
			{
				if (SpawnedFences[i] != null)
				{
					UnityEngine.Object.DestroyImmediate(SpawnedFences[i]);
				}
			}
		}
		if (SpawnedRopes != null)
		{
			for (int j = 0; j < SpawnedRopes.Count; j++)
			{
				if (SpawnedRopes[j] != null)
				{
					UnityEngine.Object.DestroyImmediate(SpawnedRopes[j].gameObject);
				}
			}
		}
		SpawnedFences = new List<GameObject>();
		SpawnedRopes = new List<LineRenderer>();
		FindCheckpointsHolder();
		List<Transform> list = new List<Transform>();
		for (int k = 0; k < Waypoints.Count; k++)
		{
			list.Add(Waypoints[k]);
		}
		foreach (RopeFencedSegment ropeFencedSegment in RopeFencedSegments)
		{
			if (ropeFencedSegment.StartWaypoint <= list.Count - 1 && !(ropeFencedSegment.FencePrefab == null))
			{
				List<Vector3> list2 = new List<Vector3>();
				for (int l = 0; l < 4; l++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(ropeFencedSegment.FencePrefab);
					int num = (l > 1) ? 1 : 0;
					int num2 = ((float)l % 2f != 0f) ? 1 : (-1);
					Transform transform = list[0];
					if (ropeFencedSegment.StartWaypoint + num < list.Count)
					{
						transform = list[ropeFencedSegment.StartWaypoint + num];
					}
					gameObject.transform.position = transform.position + transform.forward * ropeFencedSegment.SegmentWidth * Mathf.Sign(num2);
					gameObject.transform.parent = CheckpointsHolder;
					SpawnedFences.Add(gameObject);
					AlignObjectByGround(gameObject);
					list2.Add(gameObject.transform.position);
				}
				for (int m = 0; m < 2; m++)
				{
					GameObject gameObject2 = new GameObject("Rope");
					gameObject2.transform.parent = CheckpointsHolder;
					LineRenderer lineRenderer = gameObject2.AddComponent<LineRenderer>();
					lineRenderer.material = (Resources.Load("Materials/Rope", typeof(Material)) as Material);
					lineRenderer.useWorldSpace = true;
					lineRenderer.positionCount = 2;
					lineRenderer.textureMode = LineTextureMode.Tile;
					lineRenderer.widthMultiplier = 0.3f;
					lineRenderer.SetPosition(0, list2[m]);
					lineRenderer.SetPosition(1, list2[m + 2]);
					SpawnedRopes.Add(lineRenderer);
				}
			}
		}
	}

	public void UpdateRoute()
	{
		RedrawRoute();
		PlaceStartPrefab();
		PlaceFinishPrefab();
		PlaceCheckpointsPrefabs();
		PlaceEventMarkPrefab();
		PlaceRopeFence();
	}
}
