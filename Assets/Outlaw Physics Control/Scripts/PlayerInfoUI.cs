using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
	public GameObject MemberLogo;

	public GameObject MemberBadge;

	public GameObject NormalBadge;

	public Text PlayerName;

	public Text XPLabel;

	public bool Populated;

	public void Populate(string name, int xp, bool isMember)
	{
		MemberLogo.SetActive(isMember);
		MemberBadge.SetActive(isMember);
		NormalBadge.SetActive(!isMember);
		PlayerName.text = name;
		XPLabel.text = xp.ToString() + "XP";
		Populated = true;
	}
}
