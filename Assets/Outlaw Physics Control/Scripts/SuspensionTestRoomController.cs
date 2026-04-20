using CustomVP;
using UnityEngine;

public class SuspensionTestRoomController : MonoBehaviour
{
	public static SuspensionTestRoomController Instance;

	public GameObject bumpTerrain0;

	public GameObject bumpTerrain1;

	public GameObject bumpTerrain2;

	public Transform CarPositionPoint;

	public float MoveSpeed;

	private int TerrainPattern;

	private GameObject[] terrains0;

	private GameObject[] terrains1;

	private GameObject[] terrains2;

	private CarController car;

	private Rigidbody carRigidbody;

	private bool TestStarted;

	private bool CarInitialized;

	public SuspensionTestRoomController()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	private void Start()
	{
		Instance = this;
		GameObject gameObject = UnityEngine.Object.Instantiate(bumpTerrain0.gameObject);
		gameObject.transform.position = bumpTerrain0.transform.position + new Vector3(0f, 0f, 50f);
		gameObject.transform.parent = bumpTerrain0.transform.parent;
		terrains0 = new GameObject[2];
		terrains0[0] = bumpTerrain0;
		terrains0[1] = gameObject;
		GameObject gameObject2 = UnityEngine.Object.Instantiate(bumpTerrain1.gameObject);
		gameObject2.transform.position = bumpTerrain1.transform.position + new Vector3(0f, 0f, 50f);
		gameObject2.transform.parent = bumpTerrain1.transform.parent;
		terrains1 = new GameObject[2];
		terrains1[0] = bumpTerrain1;
		terrains1[1] = gameObject2;
		GameObject gameObject3 = UnityEngine.Object.Instantiate(bumpTerrain2.gameObject);
		gameObject3.transform.position = bumpTerrain2.transform.position + new Vector3(0f, 0f, 50f);
		gameObject3.transform.parent = bumpTerrain2.transform.parent;
		terrains2 = new GameObject[2];
		terrains2[0] = bumpTerrain2;
		terrains2[1] = gameObject3;
	}

	public void InitializeSuspensionTest(GameObject Vehicle)
	{
		car = Vehicle.GetComponent<CarController>();
		carRigidbody = Vehicle.GetComponent<Rigidbody>();
		car.transform.position = CarPositionPoint.position;
		car.transform.rotation = CarPositionPoint.rotation;
		car.enabled = true;
		car.FWD = (car.RWD = true);
		car.vehicleIsActive = true;
		car.PreventFromSideSliding = false;
		car.OnValidate();
		CarInitialized = true;
	}

	public void DeinitializeSuspensionTest()
	{
		StopTest();
		CarInitialized = false;
		car.vehicleIsActive = false;
	}

	public void SetPattern(int ID)
	{
		TerrainPattern = ID;
		if (TestStarted)
		{
			StopTest();
			StartTest();
		}
	}

	public void SetMoveSpeed(float value)
	{
		MoveSpeed = value;
		car.FakeRPM = MoveSpeed * 2f;
		car.OnValidate();
	}

	[ContextMenu("Start test")]
	public void StartTest()
	{
		car.FakeRPM = MoveSpeed * 2f;
		car.SetZeroFriction();
		car.OnValidate();
		GameObject[] array = terrains0;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(TerrainPattern == 0);
		}
		GameObject[] array2 = terrains1;
		foreach (GameObject gameObject2 in array2)
		{
			gameObject2.SetActive(TerrainPattern == 1);
		}
		GameObject[] array3 = terrains2;
		foreach (GameObject gameObject3 in array3)
		{
			gameObject3.SetActive(TerrainPattern == 2);
		}
		TestStarted = true;
		if (MoveSpeed == 0f)
		{
			MoveSpeed = 2f;
		}
	}

	[ContextMenu("Stop test")]
	public void StopTest()
	{
		car.FakeRPM = 0f;
		car.SetDefaultFriction();
		car.OnValidate();
		Transform transform = terrains0[0].transform;
		Vector3 zero = Vector3.zero;
		terrains2[0].transform.localPosition = zero;
		zero = zero;
		terrains1[0].transform.localPosition = zero;
		transform.localPosition = zero;
		Transform transform2 = terrains0[1].transform;
		zero = new Vector3(0f, 0f, 50f);
		terrains2[1].transform.localPosition = zero;
		zero = zero;
		terrains1[1].transform.localPosition = zero;
		transform2.localPosition = zero;
		TestStarted = false;
	}

	private void MoveTerrains()
	{
		GameObject[] array = terrains0;
		foreach (GameObject gameObject in array)
		{
			gameObject.transform.position -= new Vector3(0f, 0f, MoveSpeed * Time.deltaTime);
			Vector3 localPosition = gameObject.transform.localPosition;
			if (localPosition.z < -99f)
			{
				gameObject.transform.localPosition = Vector3.zero;
			}
		}
		GameObject[] array2 = terrains1;
		foreach (GameObject gameObject2 in array2)
		{
			gameObject2.transform.position -= new Vector3(0f, 0f, MoveSpeed * Time.deltaTime);
			Vector3 localPosition2 = gameObject2.transform.localPosition;
			if (localPosition2.z < -99f)
			{
				gameObject2.transform.localPosition = Vector3.zero;
			}
		}
		GameObject[] array3 = terrains2;
		foreach (GameObject gameObject3 in array3)
		{
			gameObject3.transform.position -= new Vector3(0f, 0f, MoveSpeed * Time.deltaTime);
			Vector3 localPosition3 = gameObject3.transform.localPosition;
			if (localPosition3.z < -99f)
			{
				gameObject3.transform.localPosition = Vector3.zero;
			}
		}
	}

	private void HoldCarAtPosition()
	{
		Vector3 position = CarPositionPoint.position;
		Vector3 position2 = car.transform.position;
		position.y = position2.y;
		carRigidbody.velocity /= 1.05f;
		carRigidbody.AddForce((position - car.transform.position) * 5000f);
		carRigidbody.angularVelocity /= 1.05f;
		carRigidbody.AddTorque(Vector3.Cross(car.transform.forward, Vector3.forward) * 5000f);
	}

	private void Update()
	{
		if (CarInitialized && TestStarted)
		{
			HoldCarAtPosition();
			MoveTerrains();
		}
	}
}
