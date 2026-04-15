using UnityEngine;

public class EnableInTime : MonoBehaviour
{
	public GameObject[] ItemsToEnable = new GameObject[0];

	public float EnableInterval = 3f;

	private float lastEnabledTime;

	private int itemID;

	private void OnEnable()
	{
		GameObject[] itemsToEnable = ItemsToEnable;
		foreach (GameObject gameObject in itemsToEnable)
		{
			gameObject.SetActive(value: false);
		}
		lastEnabledTime = Time.time;
	}

	private void Update()
	{
		if (Time.time - lastEnabledTime > EnableInterval && itemID < ItemsToEnable.Length)
		{
			lastEnabledTime = Time.time;
			ItemsToEnable[itemID].SetActive(value: true);
			itemID++;
		}
	}
}
