using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class OnScreenSteeringWheelController : MonoBehaviour
{
	private RectTransform rect;

	private Vector2 centerPoint;

	public float maximumSteeringAngle = 200f;

	public float wheelReleasedSpeed = 200f;

	private float wheelAngle;

	private float wheelPrevAngle;

	private bool wheelBeingHeld;

	private CrossPlatformInputManager.VirtualAxis m_SteerAxis;

	public float GetClampedValue()
	{
		return wheelAngle / maximumSteeringAngle;
	}

	private void Start()
	{
		rect = GetComponent<RectTransform>();
		InitEventsSystem();
		UpdateRect();
	}

	private void OnEnable()
	{
		if (!CrossPlatformInputManager.AxisExists("Horizontal"))
		{
			m_SteerAxis = new CrossPlatformInputManager.VirtualAxis("Horizontal");
			CrossPlatformInputManager.RegisterVirtualAxis(m_SteerAxis);
		}
		else
		{
			m_SteerAxis = CrossPlatformInputManager.VirtualAxisReference("Horizontal");
		}
	}

	private void OnDisable()
	{
		m_SteerAxis.Remove();
	}

	private void Update()
	{
		if (!wheelBeingHeld && !Mathf.Approximately(0f, wheelAngle))
		{
			float num = wheelReleasedSpeed * Time.deltaTime;
			if (Mathf.Abs(num) > Mathf.Abs(wheelAngle))
			{
				wheelAngle = 0f;
			}
			else if (wheelAngle > 0f)
			{
				wheelAngle -= num;
			}
			else
			{
				wheelAngle += num;
			}
		}
		rect.localEulerAngles = Vector3.back * wheelAngle;
		m_SteerAxis.Update(GetClampedValue());
	}

	private void InitEventsSystem()
	{
		EventTrigger component = GetComponent<EventTrigger>();
		if (component.triggers == null)
		{
			component.triggers = new List<EventTrigger.Entry>();
		}
		EventTrigger.Entry entry = new EventTrigger.Entry();
		EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
		UnityAction<BaseEventData> call = PressEvent;
		triggerEvent.AddListener(call);
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback = triggerEvent;
		component.triggers.Add(entry);
		entry = new EventTrigger.Entry();
		triggerEvent = new EventTrigger.TriggerEvent();
		call = DragEvent;
		triggerEvent.AddListener(call);
		entry.eventID = EventTriggerType.Drag;
		entry.callback = triggerEvent;
		component.triggers.Add(entry);
		entry = new EventTrigger.Entry();
		triggerEvent = new EventTrigger.TriggerEvent();
		call = ReleaseEvent;
		triggerEvent.AddListener(call);
		entry.eventID = EventTriggerType.PointerUp;
		entry.callback = triggerEvent;
		component.triggers.Add(entry);
	}

	private void UpdateRect()
	{
		Vector3[] array = new Vector3[4];
		this.rect.GetWorldCorners(array);
		for (int i = 0; i < 4; i++)
		{
			array[i] = RectTransformUtility.WorldToScreenPoint(null, array[i]);
		}
		Vector3 vector = array[0];
		Vector3 vector2 = array[2];
		float width = vector2.x - vector.x;
		float height = vector2.y - vector.y;
		Rect rect = new Rect(vector.x, vector2.y, width, height);
		centerPoint = new Vector2(rect.x + rect.width * 0.5f, rect.y - rect.height * 0.5f);
	}

	public void PressEvent(BaseEventData eventData)
	{
		Vector2 position = ((PointerEventData)eventData).position;
		wheelBeingHeld = true;
		wheelPrevAngle = Vector2.Angle(Vector2.up, position - centerPoint);
	}

	public void DragEvent(BaseEventData eventData)
	{
		Vector2 position = ((PointerEventData)eventData).position;
		float num = Vector2.Angle(Vector2.up, position - centerPoint);
		if (Vector2.Distance(position, centerPoint) > 20f)
		{
			if (position.x > centerPoint.x)
			{
				wheelAngle += num - wheelPrevAngle;
			}
			else
			{
				wheelAngle -= num - wheelPrevAngle;
			}
		}
		wheelAngle = Mathf.Clamp(wheelAngle, 0f - maximumSteeringAngle, maximumSteeringAngle);
		wheelPrevAngle = num;
	}

	public void ReleaseEvent(BaseEventData eventData)
	{
		DragEvent(eventData);
		wheelBeingHeld = false;
	}
}
