using UnityEngine;
using UnityEngine.UI;

public class MapElement : MonoBehaviour
{
	[HideInInspector]
	public string mapFileName;

	[HideInInspector]
	public int rating;

	public Text mapNameText;

	public Text mapDescriptionText;

	public Text mapRatingText;

	public Text mapAuthorText;

	public GameObject inactiveStar;

	public GameObject activeStar;

	public Image mainTabImage;

	public Color selectedMapColor;

	public Color deselectedMapColor;

	public Color hiddenMapColor;

	private bool hiddenMap;

	public void ToggleStar(bool fav)
	{
		inactiveStar.SetActive(!fav);
		activeStar.SetActive(fav);
	}

	public void ToggleFavorite(bool fav)
	{
		CommunityMapsMenu componentInParent = GetComponentInParent<CommunityMapsMenu>();
		if (!(componentInParent == null))
		{
			if (fav)
			{
				componentInParent.AddMapToFavs(mapFileName);
			}
			else
			{
				componentInParent.RemoveFromFavs(mapFileName);
			}
			ToggleStar(fav);
		}
	}

	public void ToggleSelection(bool selected, bool hidden)
	{
		hiddenMap = hidden;
		mainTabImage.color = (selected ? selectedMapColor : ((!hiddenMap) ? deselectedMapColor : hiddenMapColor));
	}

	public void ToggleSelection(bool selected)
	{
		mainTabImage.color = (selected ? selectedMapColor : ((!hiddenMap) ? deselectedMapColor : hiddenMapColor));
	}

	public void SelectMyMap()
	{
		CommunityMapsMenu componentInParent = GetComponentInParent<CommunityMapsMenu>();
		if (!(componentInParent == null))
		{
			componentInParent.SelectMap(mapFileName);
		}
	}
}
