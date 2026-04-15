using UnityEngine;

public class StashCrate : MonoBehaviour
{
	public CrateSize Size;

	public int LockTimer = -1;

	public float TimeLeft = -1f;

	private StashManager stashManager;

	public GameObject RegularLabels;

	public GameObject LockboxLabels;

	public GameObject MissingPartsLabels;

	public ParticleSystem Explosion;

	public StashContent Content;

	public bool IsMissingParts;

	public int FieldFindID = 1;

	private bool enableTimer;

	private void Start()
	{
		stashManager = UnityEngine.Object.FindObjectOfType<StashManager>();
		if (!IsMissingParts)
		{
			if (MissingPartsLabels != null)
			{
				MissingPartsLabels.SetActive(value: false);
			}
			if (Size != CrateSize.Vehicle)
			{
				RegularLabels = base.transform.Find("Regular").gameObject;
				LockboxLabels = base.transform.Find("Explosive").gameObject;
				Explosion = base.transform.Find("Explosion").GetComponent<ParticleSystem>();
			}
			if (Size != CrateSize.Vehicle && !enableTimer)
			{
				LockboxLabels.SetActive(value: false);
				RegularLabels.SetActive(value: true);
			}
		}
	}

	private void Update()
	{
		if (enableTimer)
		{
			TimeLeft -= Time.deltaTime;
		}
		if (TimeLeft <= 0f && TimeLeft != -1f)
		{
			LockTimer = -1;
			TimeLeft = -1f;
			StashManager.Instance.LockboxDisabled(expired: true);
			UnityEngine.Debug.Log("Lockbox expired!");
			Explosion.Play();
			Invoke("DisableMe", 0.4f);
			enableTimer = false;
		}
	}

	public void SetAsMissingParts()
	{
		RegularLabels = base.transform.Find("Regular").gameObject;
		LockboxLabels = base.transform.Find("Explosive").gameObject;
		MissingPartsLabels = base.transform.Find("MissingParts").gameObject;
		LockboxLabels.SetActive(value: false);
		RegularLabels.SetActive(value: false);
		if (MissingPartsLabels != null)
		{
			MissingPartsLabels.SetActive(value: true);
		}
		IsMissingParts = true;
	}

	public void DisableMe()
	{
		base.gameObject.SetActive(value: false);
	}

	public void SetLockTimer(int time)
	{
		LockTimer = time;
		TimeLeft = time;
		enableTimer = true;
		Explosion = base.transform.Find("Explosion").GetComponent<ParticleSystem>();
		RegularLabels = base.transform.Find("Regular").gameObject;
		LockboxLabels = base.transform.Find("Explosive").gameObject;
		RegularLabels.SetActive(value: false);
		LockboxLabels.SetActive(value: true);
	}

	[ContextMenu("Pick up")]
	private void OnMouseDown()
	{
		if (stashManager != null)
		{
			stashManager.FoundStashCrate(this);
		}
	}
}
