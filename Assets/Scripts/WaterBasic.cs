using UnityEngine;

public class WaterBasic : MonoBehaviour
{
	private Renderer r;

	private Material m;

	private float waveScale;

	private float t;

	private Vector4 waveSpeed;

	private Vector4 offset4;

	private Vector4 offsetClamped;

	private void Start()
	{
		r = GetComponent<Renderer>();
		m = r.sharedMaterial;
	}

	private void Update()
	{
		if ((bool)r && (bool)m)
		{
			waveSpeed = m.GetVector("WaveSpeed");
			waveScale = m.GetFloat("_WaveScale");
			t = Time.time / 20f;
			offset4 = waveSpeed * (t * waveScale);
			offsetClamped = new Vector4(Mathf.Repeat(offset4.x, 1f), Mathf.Repeat(offset4.y, 1f), Mathf.Repeat(offset4.z, 1f), Mathf.Repeat(offset4.w, 1f));
			m.SetVector("_WaveOffset", offsetClamped);
		}
	}
}
