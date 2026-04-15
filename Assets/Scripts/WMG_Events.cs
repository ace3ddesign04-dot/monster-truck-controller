using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class WMG_Events : WMG_GUI_Functions
{
	public delegate void WMG_Click_H(WMG_Series aSeries, WMG_Node aNode);

	public delegate void WMG_Link_Click_H(WMG_Series aSeries, WMG_Link aLink);

	public delegate void WMG_Click_Leg_H(WMG_Series aSeries, WMG_Node aNode);

	public delegate void WMG_Link_Click_Leg_H(WMG_Series aSeries, WMG_Link aLink);

	public delegate void WMG_MouseEnter_H(WMG_Series aSeries, WMG_Node aNode, bool state);

	public delegate void WMG_Link_MouseEnter_H(WMG_Series aSeries, WMG_Link aLink, bool state);

	public delegate void WMG_MouseEnter_Leg_H(WMG_Series aSeries, WMG_Node aNode, bool state);

	public delegate void WMG_Link_MouseEnter_Leg_H(WMG_Series aSeries, WMG_Link aLink, bool state);

	public event WMG_Click_H WMG_Click;

	public event WMG_Link_Click_H WMG_Link_Click;

	public event WMG_Click_Leg_H WMG_Click_Leg;

	public event WMG_Link_Click_Leg_H WMG_Link_Click_Leg;

	public event WMG_MouseEnter_H WMG_MouseEnter;

	public event WMG_Link_MouseEnter_H WMG_Link_MouseEnter;

	public event WMG_MouseEnter_Leg_H WMG_MouseEnter_Leg;

	public event WMG_Link_MouseEnter_Leg_H WMG_Link_MouseEnter_Leg;

	private void AddEventTrigger(UnityAction<GameObject> action, EventTriggerType triggerType, GameObject go)
	{
		EventTrigger eventTrigger = go.GetComponent<EventTrigger>();
		if (eventTrigger == null)
		{
			eventTrigger = go.AddComponent<EventTrigger>();
			eventTrigger.triggers = new List<EventTrigger.Entry>();
		}
		EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
		triggerEvent.AddListener(delegate
		{
			action(go);
		});
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.callback = triggerEvent;
		entry.eventID = triggerType;
		EventTrigger.Entry item = entry;
		eventTrigger.triggers.Add(item);
	}

	private void AddEventTrigger(UnityAction<GameObject, bool> action, EventTriggerType triggerType, GameObject go, bool state)
	{
		EventTrigger eventTrigger = go.GetComponent<EventTrigger>();
		if (eventTrigger == null)
		{
			eventTrigger = go.AddComponent<EventTrigger>();
			eventTrigger.triggers = new List<EventTrigger.Entry>();
		}
		EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
		triggerEvent.AddListener(delegate
		{
			action(go, state);
		});
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.callback = triggerEvent;
		entry.eventID = triggerType;
		EventTrigger.Entry item = entry;
		eventTrigger.triggers.Add(item);
	}

	public void addNodeClickEvent(GameObject go)
	{
		AddEventTrigger(WMG_Click_2, EventTriggerType.PointerClick, go);
	}

	private void WMG_Click_2(GameObject go)
	{
		if (this.WMG_Click != null)
		{
			WMG_Series component = go.transform.parent.parent.GetComponent<WMG_Series>();
			this.WMG_Click(component, go.GetComponent<WMG_Node>());
		}
	}

	public void addLinkClickEvent(GameObject go)
	{
		AddEventTrigger(WMG_Link_Click_2, EventTriggerType.PointerClick, go);
	}

	private void WMG_Link_Click_2(GameObject go)
	{
		if (this.WMG_Link_Click != null)
		{
			WMG_Series component = go.transform.parent.parent.GetComponent<WMG_Series>();
			this.WMG_Link_Click(component, go.GetComponent<WMG_Link>());
		}
	}

	public void addNodeClickEvent_Leg(GameObject go)
	{
		AddEventTrigger(WMG_Click_Leg_2, EventTriggerType.PointerClick, go);
	}

	private void WMG_Click_Leg_2(GameObject go)
	{
		if (this.WMG_Click_Leg != null)
		{
			WMG_Series seriesRef = go.transform.parent.GetComponent<WMG_Legend_Entry>().seriesRef;
			this.WMG_Click_Leg(seriesRef, go.GetComponent<WMG_Node>());
		}
	}

	public void addLinkClickEvent_Leg(GameObject go)
	{
		AddEventTrigger(WMG_Link_Click_Leg_2, EventTriggerType.PointerClick, go);
	}

	private void WMG_Link_Click_Leg_2(GameObject go)
	{
		if (this.WMG_Link_Click_Leg != null)
		{
			WMG_Series seriesRef = go.transform.parent.GetComponent<WMG_Legend_Entry>().seriesRef;
			this.WMG_Link_Click_Leg(seriesRef, go.GetComponent<WMG_Link>());
		}
	}

	public void addNodeMouseEnterEvent(GameObject go)
	{
		AddEventTrigger(WMG_MouseEnter_2, EventTriggerType.PointerEnter, go, state: true);
		AddEventTrigger(WMG_MouseEnter_2, EventTriggerType.PointerExit, go, state: false);
	}

	private void WMG_MouseEnter_2(GameObject go, bool state)
	{
		if (this.WMG_MouseEnter != null)
		{
			WMG_Series component = go.transform.parent.parent.GetComponent<WMG_Series>();
			this.WMG_MouseEnter(component, go.GetComponent<WMG_Node>(), state);
		}
	}

	public void addLinkMouseEnterEvent(GameObject go)
	{
		AddEventTrigger(WMG_Link_MouseEnter_2, EventTriggerType.PointerEnter, go, state: true);
		AddEventTrigger(WMG_Link_MouseEnter_2, EventTriggerType.PointerExit, go, state: false);
	}

	private void WMG_Link_MouseEnter_2(GameObject go, bool state)
	{
		if (this.WMG_Link_MouseEnter != null)
		{
			WMG_Series component = go.transform.parent.parent.GetComponent<WMG_Series>();
			this.WMG_Link_MouseEnter(component, go.GetComponent<WMG_Link>(), state);
		}
	}

	public void addNodeMouseEnterEvent_Leg(GameObject go)
	{
		AddEventTrigger(WMG_MouseEnter_Leg_2, EventTriggerType.PointerEnter, go, state: true);
		AddEventTrigger(WMG_MouseEnter_Leg_2, EventTriggerType.PointerExit, go, state: false);
	}

	private void WMG_MouseEnter_Leg_2(GameObject go, bool state)
	{
		if (this.WMG_MouseEnter_Leg != null)
		{
			WMG_Series seriesRef = go.transform.parent.GetComponent<WMG_Legend_Entry>().seriesRef;
			this.WMG_MouseEnter_Leg(seriesRef, go.GetComponent<WMG_Node>(), state);
		}
	}

	public void addLinkMouseEnterEvent_Leg(GameObject go)
	{
		AddEventTrigger(WMG_Link_MouseEnter_Leg_2, EventTriggerType.PointerEnter, go, state: true);
		AddEventTrigger(WMG_Link_MouseEnter_Leg_2, EventTriggerType.PointerExit, go, state: false);
	}

	private void WMG_Link_MouseEnter_Leg_2(GameObject go, bool state)
	{
		if (this.WMG_Link_MouseEnter_Leg != null)
		{
			WMG_Series seriesRef = go.transform.parent.GetComponent<WMG_Legend_Entry>().seriesRef;
			this.WMG_Link_MouseEnter_Leg(seriesRef, go.GetComponent<WMG_Link>(), state);
		}
	}

	public void addNodeMouseLeaveEvent(GameObject go)
	{
	}

	public void addLinkMouseLeaveEvent(GameObject go)
	{
	}

	public void addNodeMouseLeaveEvent_Leg(GameObject go)
	{
	}

	public void addLinkMouseLeaveEvent_Leg(GameObject go)
	{
	}
}
