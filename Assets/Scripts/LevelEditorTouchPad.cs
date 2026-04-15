using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class LevelEditorTouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	private LevelEditor levelEditor;

	private const string horizontalAxisName = "Drag X";

	private const string verticalAxisName = "Drag Y";

	private const string zoomAxisName = "Zoom";

	private CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis;

	private CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis;

	private CrossPlatformInputManager.VirtualAxis m_ZoomVirtualAxis;

	private bool dragging;

	public int lastTouchID = -1;

	private Vector2[] prevTouchPos;

	private Vector2[] pointerDeltas;

	private Vector2[] startTouchPos;

	private float startTouchDistance;

	private float lastTouchDistance;

	private Vector3 prevMousePos;

	private Vector2 mouseDelta;

	private Vector2 lastTapPos;

	private float lastTapTime;

	private void Start()
	{
		levelEditor = LevelEditor.Instance;
		CreateVirtualAxes();
		prevTouchPos = new Vector2[2];
		pointerDeltas = new Vector2[2];
		startTouchPos = new Vector2[2];
		SceneManager.sceneUnloaded += DestroyVirtualAxes;
	}

	private void CreateVirtualAxes()
	{
		m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis("Drag X");
		CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
		m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis("Drag Y");
		CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
		m_ZoomVirtualAxis = new CrossPlatformInputManager.VirtualAxis("Zoom");
		CrossPlatformInputManager.RegisterVirtualAxis(m_ZoomVirtualAxis);
	}

	private void DestroyVirtualAxes<Scene>(Scene scene)
	{
		CrossPlatformInputManager.UnRegisterVirtualAxis("Drag Y");
		CrossPlatformInputManager.UnRegisterVirtualAxis("Drag X");
		CrossPlatformInputManager.UnRegisterVirtualAxis("Zoom");
		SceneManager.sceneUnloaded -= DestroyVirtualAxes;
	}

	private void UpdateDragAxes(Vector3 value)
	{
		m_HorizontalVirtualAxis.Update(value.x);
		m_VerticalVirtualAxis.Update(value.y);
	}

	private void UpdateZoomAxis(float value)
	{
		m_ZoomVirtualAxis.Update(value);
	}

	public void OnPointerDown(PointerEventData data)
	{
		dragging = true;
		levelEditor.draggingScreen = true;
		prevMousePos = UnityEngine.Input.mousePosition;
		lastTouchID = data.pointerId;
		prevTouchPos[lastTouchID] = Input.touches[lastTouchID].position;
		startTouchPos[lastTouchID] = Input.touches[lastTouchID].position;
		if (UnityEngine.Input.touchCount == 2)
		{
			startTouchDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
			lastTouchDistance = startTouchDistance;
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.touchCount == 1 && lastTouchID > -1 && lastTouchID < 2)
		{
			ref Vector2 reference = ref pointerDeltas[lastTouchID];
			Vector2 position = Input.touches[lastTouchID].position;
			float x = position.x - prevTouchPos[lastTouchID].x;
			Vector2 position2 = Input.touches[lastTouchID].position;
			reference = new Vector2(x, position2.y - prevTouchPos[lastTouchID].y);
			prevTouchPos[lastTouchID] = Input.touches[lastTouchID].position;
			UpdateDragAxes(new Vector3(pointerDeltas[lastTouchID].x, pointerDeltas[lastTouchID].y, 0f));
		}
		if (UnityEngine.Input.touchCount == 2 && lastTouchID > -1 && lastTouchID < 2)
		{
			for (int i = 0; i < 2; i++)
			{
				ref Vector2 reference2 = ref pointerDeltas[i];
				Vector2 position3 = Input.touches[i].position;
				float x2 = position3.x - prevTouchPos[i].x;
				Vector2 position4 = Input.touches[i].position;
				reference2 = new Vector2(x2, position4.y - prevTouchPos[i].y);
				prevTouchPos[i] = Input.touches[i].position;
			}
			Vector3 vector = (pointerDeltas[0] + pointerDeltas[1]) / 2f;
			float num = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
			float num2 = num - lastTouchDistance;
			lastTouchDistance = num;
			UpdateDragAxes(new Vector3(vector.x, vector.y, 0f));
			UpdateZoomAxis(num2 * 3f);
		}
	}

	public void OnPointerUp(PointerEventData data)
	{
		dragging = false;
		levelEditor.draggingScreen = false;
		lastTouchID = -1;
		UpdateDragAxes(Vector3.zero);
		UpdateZoomAxis(0f);
		bool flag = false;
		if (((Vector3)(data.position - lastTapPos)).magnitude < 20f && Time.realtimeSinceStartup - lastTapTime < 0.5f)
		{
			flag = true;
			lastTapTime = 0f;
		}
		if (!flag)
		{
			lastTapTime = Time.realtimeSinceStartup;
			lastTapPos = data.position;
		}
		bool fingerMoved = Vector2.Distance(data.position, startTouchPos[data.pointerId]) > 20f;
		levelEditor.OnTouchTap(data.position, fingerMoved, flag);
	}

	private void OnDestroy()
	{
		if (CrossPlatformInputManager.AxisExists("Drag X"))
		{
			CrossPlatformInputManager.UnRegisterVirtualAxis("Drag X");
		}
		if (CrossPlatformInputManager.AxisExists("Drag Y"))
		{
			CrossPlatformInputManager.UnRegisterVirtualAxis("Drag Y");
		}
	}
}
