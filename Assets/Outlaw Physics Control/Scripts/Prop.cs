using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
	public int propID;

	public PropType propType;

	public string propName;

	public Sprite propImage;

	[Space(10f)]
	public float circleDrawerSizeMultiplier = 1f;

	[Space(10f)]
	public Collider placementCollider;

	public GameObject highlightedObject;

	[Header("Snapping")]
	public SnapPoint[] snapPoints;

	public float snapRadius;

	public bool snapAngle;

	public bool snapPosition;

	private float Xangle;

	private float Yangle;

	public float maxHeightIncrement;

	public float maxSideIncrement;

	public float maxCorrectingSizeIncrement;

	[Header("Alignment")]
	public Transform frontSupport;

	public Transform rearSupport;

	[Space(10f)]
	public float minScale = 1f;

	public float maxScale = 5f;

	[HideInInspector]
	public float defaultScale;

	[Space(10f)]
	public float minLift;

	public float maxLift = 5f;

	[HideInInspector]
	public float currentLift;

	[Header("Extras")]
	public GameObject extra0;

	public string extra0Name;

	public GameObject extra1;

	public string extra1Name;

	[HideInInspector]
	public bool extra0Enabled;

	[HideInInspector]
	public bool extra1Enabled;

	[Header("Debris")]
	public GameObject[] debrisPrefabs;

	public DebrisRect[] debrisRects;

	public Transform debrisParent;

	public float minDebrisScale = 0.2f;

	public float maxDebrisScale = 1f;

	public int maxDebrisCount;

	private int debrisSeed;

	private int debrisCount;

	[Header("Surface material")]
	public PhysicMaterial physicMaterial;

	[HideInInspector]
	public List<SnapPoint> attachedSnapPoints = new List<SnapPoint>();

	private float[] offsets;

	private bool initialized;

	private MeshRenderer[] preBakedMeshes;

	private int snappedPointsCount
	{
		get
		{
			int num = 0;
			SnapPoint[] array = snapPoints;
			foreach (SnapPoint snapPoint in array)
			{
				if (snapPoint.busy)
				{
					num++;
				}
			}
			return num;
		}
	}

	public virtual void Start()
	{
		if (!initialized)
		{
			Initialize();
		}
	}

	[ContextMenu("Disable")]
	private void disableHL()
	{
		highlightedObject.SetActive(value: false);
	}

	[ContextMenu("enable")]
	private void ebableHL()
	{
		highlightedObject.SetActive(value: true);
	}

	public void Initialize()
	{
		initialized = true;
		debrisSeed = 0;
		Vector3 localScale = base.transform.localScale;
		defaultScale = localScale.x;
		offsets = new float[snapPoints.Length];
		for (int i = 0; i < snapPoints.Length; i++)
		{
			offsets[i] = Vector3.SignedAngle(base.transform.forward, snapPoints[i].transform.forward, base.transform.up);
		}
		SnapPoint[] array = snapPoints;
		foreach (SnapPoint snapPoint in array)
		{
			if (snapPoint.leftAffector != null)
			{
				snapPoint.leftAffectorDefPos = snapPoint.leftAffector.localPosition;
			}
			if (snapPoint.rightAffector != null)
			{
				snapPoint.rightAffectorDefPos = snapPoint.rightAffector.localPosition;
			}
		}
		PlaceDebris();
	}

	private void PlaceDebris(int seed = -1, int _debrisCount = -1)
	{
		if (debrisPrefabs.Length == 0 || debrisParent == null || debrisRects.Length == 0)
		{
			return;
		}
		for (int i = 0; i < debrisParent.childCount; i++)
		{
			UnityEngine.Object.Destroy(debrisParent.GetChild(i).gameObject);
		}
		if (seed == -1)
		{
			debrisSeed = UnityEngine.Random.Range(0, 1000);
		}
		else
		{
			debrisSeed = seed;
		}
		if (_debrisCount == -1)
		{
			debrisCount = (int)((float)maxDebrisCount * UnityEngine.Random.Range(0f, 1f));
			if (debrisCount == 0)
			{
				debrisCount = 3;
			}
		}
		else
		{
			debrisCount = _debrisCount;
		}
		Random.InitState(debrisSeed);
		for (int j = 0; j < debrisCount; j++)
		{
			int num = UnityEngine.Random.Range(0, debrisPrefabs.Length);
			int num2 = UnityEngine.Random.Range(0, debrisRects.Length);
			Vector3 randomPos = debrisRects[num2].GetRandomPos();
			Quaternion rotation = Quaternion.Euler(UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f));
			GameObject gameObject = UnityEngine.Object.Instantiate(debrisPrefabs[num], randomPos, rotation, debrisParent);
			gameObject.transform.localScale = Vector3.one * Mathf.Lerp(minDebrisScale, maxDebrisScale, UnityEngine.Random.Range(0f, 1f));
		}
	}

	public virtual void Update()
	{
	}

	public SnapPoint ClosestFreeSnapPoint(Vector3 toPosition)
	{
		if (snapPoints == null)
		{
			return null;
		}
		if (snapPoints.Length == 0)
		{
			return null;
		}
		SnapPoint snapPoint = null;
		for (int i = 0; i < snapPoints.Length; i++)
		{
			if (snapPoint == null)
			{
				if (!snapPoints[i].busy)
				{
					snapPoint = snapPoints[i];
				}
			}
			else if (Vector3.Distance(toPosition, snapPoints[i].transform.position) < Vector3.Distance(toPosition, snapPoint.transform.position))
			{
				snapPoint = snapPoints[i];
			}
		}
		return snapPoint;
	}

	public int GetSuitableSnapPointID(SnapPoint otherSnapPoint)
	{
		if (snapPoints == null)
		{
			return -1;
		}
		if (snapPoints.Length == 0)
		{
			return -1;
		}
		for (int i = 0; i < snapPoints.Length; i++)
		{
			if (Vector3.Angle(snapPoints[i].transform.forward, otherSnapPoint.transform.forward) > 90f)
			{
				return i;
			}
		}
		return -1;
	}

	public void ToggleExtra0(bool on)
	{
		extra0Enabled = on;
		if (extra0 != null)
		{
			extra0.SetActive(on);
		}
	}

	public void ToggleExtra1(bool on)
	{
		extra1Enabled = on;
		if (extra1 != null)
		{
			extra1.SetActive(on);
		}
	}

	public void ResetSnapping()
	{
		if (snapPoints.Length != 0 && offsets != null)
		{
			SnapPoint[] array = snapPoints;
			foreach (SnapPoint snapPoint in array)
			{
				snapPoint.ResetAffectors();
				snapPoint.busy = false;
			}
			foreach (SnapPoint attachedSnapPoint in attachedSnapPoints)
			{
				attachedSnapPoint.ResetAffectors();
				attachedSnapPoint.busy = false;
				attachedSnapPoint.transform.GetComponentInParent<Prop>().OnPropDetached(this);
			}
			attachedSnapPoints.Clear();
		}
	}

	public bool DoSnapping()
	{
		if (snapPoints.Length == 0)
		{
			return false;
		}
		if (offsets == null)
		{
			return false;
		}
		List<SnapPoint> list = new List<SnapPoint>();
		bool result = false;
		for (int i = 0; i < snapPoints.Length; i++)
		{
			SnapPoint snapPoint = snapPoints[i];
			snapPoint.ResetAffectors();
			snapPoint.busy = false;
			Collider[] array = Physics.OverlapSphere(snapPoint.transform.position, snapRadius);
			if (array.Length <= 0)
			{
				continue;
			}
			Collider[] array2 = array;
			foreach (Collider collider in array2)
			{
				Prop componentInParent = collider.GetComponentInParent<Prop>();
				if (componentInParent == this || componentInParent == null || componentInParent.propType != propType)
				{
					continue;
				}
				SnapPoint[] array3 = componentInParent.snapPoints;
				if (array3.Length <= 0)
				{
					continue;
				}
				SnapPoint snapPoint2 = array3[0];
				SnapPoint[] array4 = array3;
				foreach (SnapPoint snapPoint3 in array4)
				{
					if (Vector3.Distance(snapPoint3.transform.position, snapPoint.transform.position) < Vector3.Distance(snapPoint2.transform.position, snapPoint.transform.position))
					{
						snapPoint2 = snapPoint3;
					}
				}
				if (Vector3.Angle(snapPoint2.transform.right, -snapPoint.transform.right) > 90f)
				{
					continue;
				}
				if (snapPoint2.busy)
				{
					bool flag = false;
					foreach (SnapPoint attachedSnapPoint in attachedSnapPoints)
					{
						if (attachedSnapPoint.transform.Equals(snapPoint2.transform))
						{
							flag = true;
						}
					}
					if (!flag)
					{
						continue;
					}
				}
				snapPoint2.ResetAffectors();
				list.Add(snapPoint2);
				if (!attachedSnapPoints.Contains(snapPoint2))
				{
					attachedSnapPoints.Add(snapPoint2);
				}
				snapPoint2.busy = true;
				if (!componentInParent.attachedSnapPoints.Contains(snapPoint))
				{
					componentInParent.attachedSnapPoints.Add(snapPoint);
				}
				snapPoint.busy = true;
				if (snapAngle)
				{
					Vector3 position = snapPoint.transform.position;
					Vector3 vector = -snapPoint2.transform.forward;
					vector = Vector3.ProjectOnPlane(vector, base.transform.up);
					Vector3 b = Quaternion.AngleAxis(0f - offsets[i], base.transform.up) * vector;
					base.transform.rotation *= Quaternion.FromToRotation(base.transform.forward, Vector3.Lerp(base.transform.forward, b, 0.3f));
					Vector3 position2 = snapPoint.transform.position;
					base.transform.position -= position2 - position;
				}
				bool flag2 = propType == PropType.Road && array3.Length == 2 && snappedPointsCount == 1;
				if (snapPosition || flag2)
				{
					Vector3 vector2 = snapPoint2.transform.InverseTransformPoint(snapPoint.transform.position);
					vector2 = snapPoint.transform.position - snapPoint2.transform.position;
					vector2 /= 3f;
					if (flag2)
					{
						vector2.y = 0f;
					}
					base.transform.position -= vector2;
					result = true;
				}
				if (snapPoint.leftAffector != null && snapPoint.rightAffector != null && snapPoint2.leftAffector != null && snapPoint2.rightAffector != null)
				{
					Vector3 vector3 = (snapPoint.leftAffector.position + snapPoint2.rightAffector.position) / 2f;
					Vector3 vector4 = (snapPoint.rightAffector.position + snapPoint2.leftAffector.position) / 2f;
					Transform leftAffector = snapPoint.leftAffector;
					Vector3 position3 = vector3;
					snapPoint2.rightAffector.position = position3;
					leftAffector.position = position3;
					Transform rightAffector = snapPoint.rightAffector;
					position3 = vector4;
					snapPoint2.leftAffector.position = position3;
					rightAffector.position = position3;
					Xangle = Vector3.SignedAngle(snapPoint.transform.up, snapPoint2.transform.up, snapPoint.transform.right);
					Vector3 axis = (snapPoint.transform.up + snapPoint2.transform.up) / 2f;
					UnityEngine.Debug.DrawRay(snapPoint.transform.position, axis.normalized * 5f, Color.red);
					Yangle = Vector3.SignedAngle(snapPoint.transform.right, -snapPoint2.transform.right, axis);
					Vector3 vector5 = (snapPoint.transform.right - snapPoint2.transform.right) / 2f;
					UnityEngine.Debug.DrawRay(snapPoint.transform.position, vector5.normalized * 5f, Color.cyan);
					float d = Mathf.Abs(Xangle) / 90f;
					float d2 = Mathf.Abs(Yangle) / 90f;
					snapPoint.leftAffector.position += axis.normalized * maxHeightIncrement * (0f - Mathf.Sign(Xangle)) * d;
					snapPoint.rightAffector.position += axis.normalized * maxHeightIncrement * (0f - Mathf.Sign(Xangle)) * d;
					snapPoint2.leftAffector.position += axis.normalized * maxHeightIncrement * (0f - Mathf.Sign(Xangle)) * d;
					snapPoint2.rightAffector.position += axis.normalized * maxHeightIncrement * (0f - Mathf.Sign(Xangle)) * d;
					snapPoint.leftAffector.position += vector5.normalized * maxSideIncrement * Mathf.Sign(Yangle) * d2 - vector5.normalized * d2 * maxCorrectingSizeIncrement;
					snapPoint.rightAffector.position += vector5.normalized * maxSideIncrement * Mathf.Sign(Yangle) * d2 + vector5.normalized * d2 * maxCorrectingSizeIncrement;
					snapPoint2.leftAffector.position += vector5.normalized * maxSideIncrement * Mathf.Sign(Yangle) * d2 + vector5.normalized * d2 * maxCorrectingSizeIncrement;
					snapPoint2.rightAffector.position += vector5.normalized * maxSideIncrement * Mathf.Sign(Yangle) * d2 - vector5.normalized * d2 * maxCorrectingSizeIncrement;
				}
			}
		}
		for (int l = 0; l < attachedSnapPoints.Count; l++)
		{
			bool flag3 = false;
			for (int m = 0; m < list.Count; m++)
			{
				if (list[m].transform.Equals(attachedSnapPoints[l].transform))
				{
					flag3 = true;
				}
			}
			if (!flag3)
			{
				attachedSnapPoints[l].ResetAffectors();
				attachedSnapPoints[l].busy = false;
				attachedSnapPoints[l].transform.GetComponentInParent<Prop>().OnPropDetached(this);
				attachedSnapPoints.RemoveAt(l);
			}
		}
		return result;
	}

	public void SnapToSnapPoint(Prop otherProp, SnapPoint otherSnapPoint)
	{
	}

	public void OnPropDetached(Prop otherProp)
	{
		for (int i = 0; i < attachedSnapPoints.Count; i++)
		{
			SnapPoint[] array = otherProp.snapPoints;
			foreach (SnapPoint snapPoint in array)
			{
				if (i < attachedSnapPoints.Count && attachedSnapPoints[i].transform.Equals(snapPoint.transform))
				{
					attachedSnapPoints.RemoveAt(i);
				}
			}
		}
	}

	[ContextMenu("Pre bake")]
	public void PreBakeProp()
	{
		SkinnedMeshRenderer[] componentsInChildren = GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: false);
		preBakedMeshes = new MeshRenderer[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!(componentsInChildren[i].gameObject == highlightedObject))
			{
				Mesh mesh = new Mesh();
				componentsInChildren[i].BakeMesh(mesh);
				preBakedMeshes[i] = new GameObject("PreBakedMesh:" + componentsInChildren[i].name).AddComponent<MeshRenderer>();
				preBakedMeshes[i].gameObject.AddComponent<MeshFilter>().mesh = mesh;
				preBakedMeshes[i].gameObject.AddComponent<MeshCollider>().material = physicMaterial;
				preBakedMeshes[i].transform.position = componentsInChildren[i].transform.position;
				preBakedMeshes[i].transform.rotation = componentsInChildren[i].transform.rotation;
				preBakedMeshes[i].materials = componentsInChildren[i].materials;
				preBakedMeshes[i].transform.parent = base.transform;
				Transform transform = preBakedMeshes[i].transform;
				Vector3 localScale = base.transform.localScale;
				float x = 1f / localScale.x;
				Vector3 localScale2 = base.transform.localScale;
				float y = 1f / localScale2.y;
				Vector3 localScale3 = base.transform.localScale;
				transform.localScale = new Vector3(x, y, 1f / localScale3.z);
				componentsInChildren[i].enabled = false;
			}
		}
		if (placementCollider != null)
		{
			placementCollider.enabled = false;
		}
		if (!(debrisParent != null))
		{
			return;
		}
		Transform[] componentsInChildren2 = debrisParent.gameObject.GetComponentsInChildren<Transform>();
		Transform[] array = componentsInChildren2;
		foreach (Transform transform2 in array)
		{
			if (!(transform2 == debrisParent))
			{
				transform2.position += Vector3.up;
				Ray ray = new Ray(transform2.position, Vector3.down);
				if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f))
				{
					transform2.position = hitInfo.point;
				}
			}
		}
	}

	[ContextMenu("Cancel pre bake")]
	public void CancelPreBake()
	{
		if (preBakedMeshes != null)
		{
			MeshRenderer[] array = preBakedMeshes;
			foreach (MeshRenderer meshRenderer in array)
			{
				UnityEngine.Object.Destroy(meshRenderer.gameObject);
			}
		}
		SkinnedMeshRenderer[] componentsInChildren = GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: false);
		SkinnedMeshRenderer[] array2 = componentsInChildren;
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array2)
		{
			skinnedMeshRenderer.enabled = true;
		}
		if (placementCollider != null)
		{
			placementCollider.enabled = true;
		}
	}

	[ContextMenu("Bake prop")]
	public void BakeProp()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		SkinnedMeshRenderer[] componentsInChildren = GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren)
		{
			Transform[] bones = skinnedMeshRenderer.bones;
			foreach (Transform transform in bones)
			{
				UnityEngine.Object.Destroy(transform.gameObject);
			}
			if (!skinnedMeshRenderer.gameObject.activeSelf)
			{
				UnityEngine.Object.Destroy(skinnedMeshRenderer.gameObject);
				continue;
			}
			Mesh mesh = new Mesh();
			skinnedMeshRenderer.BakeMesh(mesh);
			MeshRenderer meshRenderer = skinnedMeshRenderer.gameObject.AddComponent<MeshRenderer>();
			MeshFilter meshFilter = meshRenderer.gameObject.AddComponent<MeshFilter>();
			meshFilter.mesh = mesh;
			meshRenderer.materials = skinnedMeshRenderer.materials;
			UnityEngine.Object.Destroy(skinnedMeshRenderer);
			MeshCollider meshCollider = meshRenderer.gameObject.AddComponent<MeshCollider>();
			Transform transform2 = meshRenderer.transform;
			Vector3 localScale = base.transform.localScale;
			float x = 1f / localScale.x;
			Vector3 localScale2 = base.transform.localScale;
			float y = 1f / localScale2.y;
			Vector3 localScale3 = base.transform.localScale;
			transform2.localScale = new Vector3(x, y, 1f / localScale3.z);
		}
		SnapPoint[] array = snapPoints;
		foreach (SnapPoint snapPoint in array)
		{
			if (snapPoint.leftAffector != null)
			{
				UnityEngine.Object.Destroy(snapPoint.leftAffector.gameObject);
			}
			if (snapPoint.rightAffector != null)
			{
				UnityEngine.Object.Destroy(snapPoint.rightAffector.gameObject);
			}
			if (snapPoint.transform != null)
			{
				UnityEngine.Object.Destroy(snapPoint.transform.gameObject);
			}
		}
		if (frontSupport != null)
		{
			UnityEngine.Object.Destroy(frontSupport.gameObject);
		}
		if (rearSupport != null)
		{
			UnityEngine.Object.Destroy(rearSupport.gameObject);
		}
		if (highlightedObject != null)
		{
			UnityEngine.Object.Destroy(highlightedObject);
		}
		if (debrisParent != null)
		{
			Transform[] componentsInChildren2 = debrisParent.gameObject.GetComponentsInChildren<Transform>();
			Transform[] array2 = componentsInChildren2;
			foreach (Transform transform3 in array2)
			{
				if (!(transform3 == debrisParent))
				{
					transform3.position += Vector3.up;
					Ray ray = new Ray(transform3.position, Vector3.down);
					if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f))
					{
						transform3.position = hitInfo.point;
					}
					else
					{
						UnityEngine.Object.Destroy(transform3.gameObject);
					}
				}
			}
		}
		float realtimeSinceStartup2 = Time.realtimeSinceStartup;
		float num = realtimeSinceStartup2 - realtimeSinceStartup;
		UnityEngine.Debug.Log(num);
		UnityEngine.Object.Destroy(this);
	}

	public void Highlight(bool on)
	{
		if (highlightedObject != null)
		{
			highlightedObject.SetActive(on);
		}
	}

	private void OnDestroy()
	{
		if (placementCollider != null)
		{
			UnityEngine.Object.Destroy(placementCollider);
		}
		if (highlightedObject != null)
		{
			UnityEngine.Object.Destroy(highlightedObject);
		}
	}

	public string Serialize()
	{
		string empty = string.Empty;
		empty = empty + propID + "|";
		string arg = empty;
		Vector3 position = base.transform.position;
		empty = arg + position.x + "|";
		string arg2 = empty;
		Vector3 position2 = base.transform.position;
		empty = arg2 + position2.y + "|";
		string arg3 = empty;
		Vector3 position3 = base.transform.position;
		empty = arg3 + position3.z + "|";
		string arg4 = empty;
		Vector3 eulerAngles = base.transform.eulerAngles;
		empty = arg4 + (int)eulerAngles.x + "|";
		string arg5 = empty;
		Vector3 eulerAngles2 = base.transform.eulerAngles;
		empty = arg5 + (int)eulerAngles2.y + "|";
		string arg6 = empty;
		Vector3 eulerAngles3 = base.transform.eulerAngles;
		empty = arg6 + (int)eulerAngles3.z + "|";
		string arg7 = empty;
		Vector3 localScale = base.transform.localScale;
		empty = arg7 + localScale.x + "|";
		string arg8 = empty;
		Vector3 localScale2 = base.transform.localScale;
		empty = arg8 + localScale2.y + "|";
		string arg9 = empty;
		Vector3 localScale3 = base.transform.localScale;
		empty = arg9 + localScale3.z + "|";
		empty = empty + extra0Enabled.ToString() + "|";
		empty = empty + extra1Enabled.ToString() + "|";
		empty = empty + debrisSeed.ToString() + "|";
		empty = empty + debrisCount.ToString() + "|";
		empty = empty + currentLift + "|";
		for (int i = 0; i < snapPoints.Length; i++)
		{
			if (!(snapPoints[i].leftAffector == null) && !(snapPoints[i].rightAffector == null))
			{
				Vector3 position4 = snapPoints[i].leftAffector.transform.position;
				string text = empty;
				empty = text + position4.x + "/" + position4.y + "/" + position4.z + "!";
				Vector3 position5 = snapPoints[i].rightAffector.transform.position;
				text = empty;
				empty = text + position5.x + "/" + position5.y + "/" + position5.z + "|";
			}
		}
		return empty;
	}

	public void Deserialize(string data)
	{
		string[] array = data.Split('|');
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		Vector3 zero3 = Vector3.zero;
		propID = int.Parse(array[0]);
		zero.x = float.Parse(array[1]);
		zero.y = float.Parse(array[2]);
		zero.z = float.Parse(array[3]);
		zero2.x = float.Parse(array[4]);
		zero2.y = float.Parse(array[5]);
		zero2.z = float.Parse(array[6]);
		zero3.x = float.Parse(array[7]);
		zero3.y = float.Parse(array[8]);
		zero3.z = float.Parse(array[9]);
		extra0Enabled = bool.Parse(array[10]);
		extra1Enabled = bool.Parse(array[11]);
		debrisSeed = int.Parse(array[12]);
		debrisCount = int.Parse(array[13]);
		currentLift = float.Parse(array[14]);
		base.transform.position = zero;
		base.transform.eulerAngles = zero2;
		base.transform.localScale = zero3;
		if (array.Length > 16)
		{
			for (int i = 0; i < snapPoints.Length; i++)
			{
				if (!(snapPoints[i].leftAffector == null) && !(snapPoints[i].rightAffector == null))
				{
					string text = array[15 + i];
					string[] array2 = text.Split('!');
					string text2 = array2[0];
					string[] array3 = text2.Split('/');
					float x = float.Parse(array3[0]);
					float y = float.Parse(array3[1]);
					float z = float.Parse(array3[2]);
					snapPoints[i].leftAffector.transform.position = new Vector3(x, y, z);
					string text3 = array2[1];
					string[] array4 = text3.Split('/');
					float x2 = float.Parse(array4[0]);
					float y2 = float.Parse(array4[1]);
					float z2 = float.Parse(array4[2]);
					snapPoints[i].rightAffector.transform.position = new Vector3(x2, y2, z2);
				}
			}
		}
		ToggleExtra0(extra0Enabled);
		ToggleExtra1(extra1Enabled);
		PlaceDebris(debrisSeed, debrisCount);
	}
}
