using UnityEngine;
using UnityEngine.UI;

public class InGameMapMenu : MonoBehaviour
{
	public GameObject menu;

	public GameObject activeStar;

	public GameObject inactiveStar;

	public Button upvoteButton;

	public Button downvoteButton;

	public Color selectedVoteButtonColor;

	public Color deselectedVoteButtonColor;

	private void Start()
	{
		menu.SetActive(value: false);
	}

	public void OpenMenu()
	{
		menu.SetActive(value: true);
		if (LevelEditorTools.DidIVoteForMap(GameState.mapToDownload, out int amount))
		{
			upvoteButton.interactable = false;
			downvoteButton.interactable = false;
			upvoteButton.GetComponent<Image>().color = ((amount != 1) ? deselectedVoteButtonColor : selectedVoteButtonColor);
			downvoteButton.GetComponent<Image>().color = ((amount != -1) ? deselectedVoteButtonColor : selectedVoteButtonColor);
		}
		activeStar.SetActive(LevelEditorTools.IsMapInFavs(GameState.mapToDownload));
		inactiveStar.SetActive(!LevelEditorTools.IsMapInFavs(GameState.mapToDownload));
	}

	public void AddMapToFavs()
	{
		if (!LevelEditorTools.IsMapInFavs(GameState.mapToDownload))
		{
			LevelEditorTools.AddMapToFavs(GameState.mapToDownload);
		}
		else
		{
			LevelEditorTools.RemoveFromFavs(GameState.mapToDownload);
		}
		activeStar.SetActive(LevelEditorTools.IsMapInFavs(GameState.mapToDownload));
		inactiveStar.SetActive(!LevelEditorTools.IsMapInFavs(GameState.mapToDownload));
	}

	public void VoteForMap(bool up)
	{
		if (!LevelEditorTools.DidIVoteForMap(GameState.mapToDownload, out int _))
		{
			upvoteButton.interactable = false;
			downvoteButton.interactable = false;
			upvoteButton.GetComponent<Image>().color = ((!up) ? deselectedVoteButtonColor : selectedVoteButtonColor);
			downvoteButton.GetComponent<Image>().color = (up ? deselectedVoteButtonColor : selectedVoteButtonColor);
			LevelEditorTools.AddMapToVoted(GameState.mapToDownload, up);
			WWW wWW = new WWW("keereedev.000webhostapp.com/ChangeMapRating.php?ID=" + GameState.mapToDownload + "&amount=" + ((!up) ? "-1" : "1"));
		}
	}
}
