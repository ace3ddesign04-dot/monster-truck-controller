using System.Collections.Generic;
using UnityEngine;

namespace GeNa
{
	public class GenaDefaults : ScriptableObject
	{
		[Header("Deletion CTRL")]
		public KeyCode m_keyDeleteSpawnedResources = KeyCode.Backspace;

		[Header("Position CTRL, Rotation+Height SHIFT+CTRL")]
		public KeyCode m_keyLeft = KeyCode.LeftArrow;

		public KeyCode m_keyRight = KeyCode.RightArrow;

		public KeyCode m_keyForward = KeyCode.UpArrow;

		public KeyCode m_keyBackward = KeyCode.DownArrow;

		[Header("Light Probe Defaults")]
		public bool m_autoLightProbe = true;

		public float m_minProbeGroupDistance = 100f;

		public float m_minProbeDistance = 15f;

		[Header("Optimization Defaults")]
		public bool m_autoOptimize = true;

		public float m_maxOptimizeSize = 10f;

		[Header("Help Defaults")]
		public bool m_showTooltips = true;

		public bool m_showDetailedHelp = true;

		private static Dictionary<KeyCode, string> m_keyCodeMap = new Dictionary<KeyCode, string>
		{
			{
				KeyCode.Keypad0,
				"[0]"
			},
			{
				KeyCode.Keypad1,
				"[1]"
			},
			{
				KeyCode.Keypad2,
				"[2]"
			},
			{
				KeyCode.Keypad3,
				"[3]"
			},
			{
				KeyCode.Keypad4,
				"[4]"
			},
			{
				KeyCode.Keypad5,
				"[5]"
			},
			{
				KeyCode.Keypad6,
				"[6]"
			},
			{
				KeyCode.Keypad7,
				"[7]"
			},
			{
				KeyCode.Keypad8,
				"[8]"
			},
			{
				KeyCode.Keypad9,
				"[9]"
			},
			{
				KeyCode.KeypadPeriod,
				"[.]"
			},
			{
				KeyCode.KeypadDivide,
				"[/]"
			},
			{
				KeyCode.KeypadMinus,
				"[-]"
			},
			{
				KeyCode.KeypadPlus,
				"[+]"
			},
			{
				KeyCode.KeypadEquals,
				"[=]"
			},
			{
				KeyCode.KeypadEnter,
				"[enter]"
			},
			{
				KeyCode.UpArrow,
				"up"
			},
			{
				KeyCode.DownArrow,
				"down"
			},
			{
				KeyCode.LeftArrow,
				"left"
			},
			{
				KeyCode.RightArrow,
				"right"
			},
			{
				KeyCode.Insert,
				"insert"
			},
			{
				KeyCode.Home,
				"home"
			},
			{
				KeyCode.End,
				"end"
			},
			{
				KeyCode.PageDown,
				"pgdown"
			},
			{
				KeyCode.PageUp,
				"pgup"
			},
			{
				KeyCode.Backspace,
				"backspace"
			},
			{
				KeyCode.Delete,
				"delete"
			},
			{
				KeyCode.Tab,
				"tab"
			},
			{
				KeyCode.F1,
				"f1"
			},
			{
				KeyCode.F2,
				"f2"
			},
			{
				KeyCode.F3,
				"f3"
			},
			{
				KeyCode.F4,
				"f4"
			},
			{
				KeyCode.F5,
				"f5"
			},
			{
				KeyCode.F6,
				"f6"
			},
			{
				KeyCode.F7,
				"f7"
			},
			{
				KeyCode.F8,
				"f8"
			},
			{
				KeyCode.F9,
				"f9"
			},
			{
				KeyCode.F10,
				"f10"
			},
			{
				KeyCode.F11,
				"f11"
			},
			{
				KeyCode.F12,
				"f12"
			},
			{
				KeyCode.F13,
				"f13"
			},
			{
				KeyCode.F14,
				"f14"
			},
			{
				KeyCode.F15,
				"f15"
			},
			{
				KeyCode.Escape,
				"[esc]"
			},
			{
				KeyCode.Return,
				"return"
			},
			{
				KeyCode.Space,
				"space"
			}
		};

		public Event KeyDeleteEvent(bool shift = false, bool control = false)
		{
			Event @event = EventMapper(m_keyDeleteSpawnedResources, shift, control);
			if (@event != null)
			{
				return @event;
			}
			UnityEngine.Debug.LogWarning("KeyDeleteSpawnedResources keycode not supported");
			return null;
		}

		public Event KeyLeftEvent(bool shift = false, bool control = false)
		{
			Event @event = EventMapper(m_keyLeft, shift, control);
			if (@event != null)
			{
				return @event;
			}
			UnityEngine.Debug.LogWarning("KeyLeft keycode not supported");
			return null;
		}

		public Event KeyRightEvent(bool shift = false, bool control = false)
		{
			Event @event = EventMapper(m_keyRight, shift, control);
			if (@event != null)
			{
				return @event;
			}
			UnityEngine.Debug.LogWarning("KeyRight keycode not supported");
			return null;
		}

		public Event KeyForwardEvent(bool shift = false, bool control = false)
		{
			Event @event = EventMapper(m_keyForward, shift, control);
			if (@event != null)
			{
				return @event;
			}
			UnityEngine.Debug.LogWarning("KeyForward keycode not supported");
			return null;
		}

		public Event KeyBackwardEvent(bool shift = false, bool control = false)
		{
			Event @event = EventMapper(m_keyBackward, shift, control);
			if (@event != null)
			{
				return @event;
			}
			UnityEngine.Debug.LogWarning("KeyBackward keycode not supported");
			return null;
		}

		private Event EventMapper(KeyCode keyCode, bool shift = false, bool control = false)
		{
			string str = string.Empty;
			string value = string.Empty;
			if (m_keyCodeMap.TryGetValue(keyCode, out value))
			{
				if (shift)
				{
					str += "#";
				}
				if (control)
				{
					str += "^";
				}
				return Event.KeyboardEvent(str + value);
			}
			return null;
		}
	}
}
