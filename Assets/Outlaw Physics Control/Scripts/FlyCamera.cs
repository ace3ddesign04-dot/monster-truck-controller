using UnityEngine;

public class FlyCamera : MonoBehaviour
{
	public float mainSpeed = 100f;

	public float shiftAdd = 250f;

	public float maxShift = 1000f;

	public float camSens = 0.25f;

	private Vector3 lastMouse = new Vector3(255f, 255f, 255f);

	private float totalRun = 1f;

	private bool isRotating;

	private float speedMultiplier;

	public float mouseSensitivity = 5f;

	private float rotationY;

	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			isRotating = true;
		}
		if (Input.GetMouseButtonUp(1))
		{
			isRotating = false;
		}
		if (isRotating)
		{
			Vector3 localEulerAngles = base.transform.localEulerAngles;
			float y = localEulerAngles.y + UnityEngine.Input.GetAxis("Mouse X") * mouseSensitivity;
			rotationY += UnityEngine.Input.GetAxis("Mouse Y") * mouseSensitivity;
			rotationY = Mathf.Clamp(rotationY, -90f, 90f);
			base.transform.localEulerAngles = new Vector3(0f - rotationY, y, 0f);
		}
		float num = 0f;
		Vector3 vector = GetBaseInput();
		if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
		{
			totalRun += Time.deltaTime;
			vector = vector * totalRun * shiftAdd;
			vector.x = Mathf.Clamp(vector.x, 0f - maxShift, maxShift);
			vector.y = Mathf.Clamp(vector.y, 0f - maxShift, maxShift);
			vector.z = Mathf.Clamp(vector.z, 0f - maxShift, maxShift);
			speedMultiplier = totalRun * shiftAdd * Time.deltaTime;
			speedMultiplier = Mathf.Clamp(speedMultiplier, 0f - maxShift, maxShift);
		}
		else
		{
			totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
			vector *= mainSpeed;
			speedMultiplier = mainSpeed * Time.deltaTime;
		}
		vector *= Time.deltaTime;
		Vector3 position = base.transform.position;
		base.transform.Translate(vector);
		Vector3 position2 = base.transform.position;
		position.x = position2.x;
		Vector3 position3 = base.transform.position;
		position.z = position3.z;
		if (UnityEngine.Input.GetKey(KeyCode.Q))
		{
			position.y += 0f - speedMultiplier;
		}
		if (UnityEngine.Input.GetKey(KeyCode.E))
		{
			position.y += speedMultiplier;
		}
		base.transform.position = position;
	}

	public bool amIRotating()
	{
		return isRotating;
	}

	private Vector3 GetBaseInput()
	{
		Vector3 vector = default(Vector3);
		if (UnityEngine.Input.GetKey(KeyCode.W))
		{
			vector += new Vector3(0f, 0f, 1f);
		}
		if (UnityEngine.Input.GetKey(KeyCode.S))
		{
			vector += new Vector3(0f, 0f, -1f);
		}
		if (UnityEngine.Input.GetKey(KeyCode.A))
		{
			vector += new Vector3(-1f, 0f, 0f);
		}
		if (UnityEngine.Input.GetKey(KeyCode.D))
		{
			vector += new Vector3(1f, 0f, 0f);
		}
		return vector;
	}
}
