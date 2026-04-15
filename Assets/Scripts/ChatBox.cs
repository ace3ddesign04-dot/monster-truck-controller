using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour
{
	public static ChatBox Instance;

	public Text ChatText;

	public ScrollRect scrollRect;

	public RectTransform ChatBoxHolder;

	public Image ChatTabArrow;

	private string myName;

	private string[] colors = new string[7]
	{
		"#00ffffff",
		"#ff00ffff",
		"#a52a2aff",
		"#00ff00ff",
		"#add8e6ff",
		"#ffa500ff",
		"#ffff00ff"
	};

	private string myColor;

	private bool DraggedScroll;

	private bool CustomScrollPos;

	private bool ChatExpanded;

	private TouchScreenKeyboard keyboard;

	private Coroutine BlinkingCoroutine;

	private bool joiningMessageSent;

	private PhotonTransformView photonTransformView
	{
		get
		{
			if (VehicleLoader.Instance != null)
			{
				return VehicleLoader.Instance.playerTView;
			}
			return null;
		}
	}

	public ChatBox()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	private void Awake()
	{
		Instance = this;
		ChatExpanded = false;
		myColor = colors[Random.Range(0, colors.Length)];
		myName = PhotonNetwork.playerName;
		if (GameState.GameMode != GameMode.Multiplayer)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public void TouchUp()
	{
		if (!DraggedScroll)
		{
			OpenKeyboard();
		}
	}

	public void DragBegan()
	{
		DraggedScroll = true;
		CustomScrollPos = true;
	}

	public void DragEnded()
	{
		DraggedScroll = false;
	}

	private void StartBlinking()
	{
		if (BlinkingCoroutine == null)
		{
			BlinkingCoroutine = StartCoroutine(Blinking());
		}
	}

	private IEnumerator Blinking()
	{
		Color tempColor = ChatTabArrow.color;
		for (float f = 0f; f < 4f; f += 0.1f)
		{
			tempColor.a = 1f - Mathf.PingPong(f, 1f);
			ChatTabArrow.color = tempColor;
			yield return null;
		}
		BlinkingCoroutine = null;
	}

	public void OpenKeyboard()
	{
		keyboard = TouchScreenKeyboard.Open(string.Empty, TouchScreenKeyboardType.Default);
	}

	private void Update()
	{
		if (keyboard != null && keyboard.done)
		{
			SendChatMessage(keyboard.text);
			keyboard = null;
		}
		if (photonTransformView != null && !joiningMessageSent)
		{
			SendJoiningMessage();
		}
		if (!CustomScrollPos)
		{
			scrollRect.verticalNormalizedPosition = 0f;
		}
		Vector3 b = new Vector3((!ChatExpanded) ? (-568) : (-400), 0f, 0f);
		ChatBoxHolder.localPosition = Vector3.Lerp(ChatBoxHolder.localPosition, b, Time.deltaTime * 10f);
	}

	public void ToggleChat()
	{
		ChatExpanded = !ChatExpanded;
	}

	private void SendChatMessage(string msg)
	{
		if (msg.Trim().Length != 0)
		{
			msg = Utility.CleanBadWords(msg);
			msg = AddMyName(msg);
			msg = AddLineBreak(msg);
			photonTransformView.SendChatMessage(msg);
		}
	}

	public void SendJoiningMessage()
	{
		string msg = "\n<i>" + myName + " has joined </i>";
		photonTransformView.SendChatMessage(msg);
		joiningMessageSent = true;
	}

	public void ReceiveChatMessage(string msg)
	{
		ChatText.text += msg;
		scrollRect.verticalNormalizedPosition = 0f;
		CustomScrollPos = false;
		if (msg.Contains("</color>"))
		{
			StartBlinking();
		}
	}

	private string AddMyName(string source)
	{
		return "<color=" + myColor + ">[" + myName + "] </color>" + source;
	}

	private string AddLineBreak(string source)
	{
		return "\n" + source;
	}
}
