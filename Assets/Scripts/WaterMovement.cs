using System;
using UnityEngine;

[Serializable]
public class WaterMovement : MonoBehaviour
{
	public float speed;

	public float alpha;

	public float waveScale;

	public Material m;

	public WaterMovement()
	{
		speed = 0.7f;
		alpha = 0.5f;
		waveScale = 3f;
	}

	public void Start()
	{
		m = gameObject.GetComponent<Renderer>().material;
	}

	public void Update()
	{
		float time = Time.time;
		float num = Mathf.PingPong(time * speed, 100f) * 0.15f;
		m.mainTextureOffset = new Vector2(num, num);
		float a = alpha;
		Color color = m.color;
		color.a = a;
		Color color3 = m.color = color;
		m.mainTextureScale = new Vector2(waveScale, waveScale);
	}

	public void Main()
	{
	}
}
