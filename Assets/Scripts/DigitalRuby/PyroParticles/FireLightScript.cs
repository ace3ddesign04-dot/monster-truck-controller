using UnityEngine;

namespace DigitalRuby.PyroParticles
{
	public class FireLightScript : MonoBehaviour
	{
		[Tooltip("Random seed for movement, 0 for no movement.")]
		public float Seed = 100f;

		[Tooltip("Multiplier for light intensity.")]
		public float IntensityModifier = 2f;

		[SingleLine("Min and max intensity range.")]
		public RangeOfFloats IntensityMaxRange = new RangeOfFloats
		{
			Minimum = 0f,
			Maximum = 8f
		};

		private Light firePointLight;

		private float lightIntensity;

		private float seed;

		private FireBaseScript fireBaseScript;

		private float baseY;

		private void Awake()
		{
			firePointLight = base.gameObject.GetComponentInChildren<Light>();
			if (firePointLight != null)
			{
				lightIntensity = firePointLight.intensity;
				firePointLight.intensity = 0f;
				Vector3 position = firePointLight.gameObject.transform.position;
				baseY = position.y;
			}
			seed = UnityEngine.Random.value * Seed;
			fireBaseScript = base.gameObject.GetComponent<FireBaseScript>();
		}

		private void Update()
		{
			if (firePointLight == null)
			{
				return;
			}
			if (seed != 0f)
			{
				bool flag = true;
				float num = 1f;
				if (fireBaseScript != null)
				{
					if (fireBaseScript.Stopping)
					{
						flag = false;
						firePointLight.intensity = Mathf.Lerp(firePointLight.intensity, 0f, fireBaseScript.StopPercent);
					}
					else if (fireBaseScript.Starting)
					{
						num = fireBaseScript.StartPercent;
					}
				}
				if (flag)
				{
					float intensity = Mathf.Clamp(IntensityModifier * num * Mathf.PerlinNoise(seed + Time.time, seed + 1f + Time.time), IntensityMaxRange.Minimum, IntensityMaxRange.Maximum);
					firePointLight.intensity = intensity;
				}
				float x = Mathf.PerlinNoise(seed + Time.time * 2f, seed + 1f + Time.time * 2f) - 0.5f;
				float y = baseY + Mathf.PerlinNoise(seed + 2f + Time.time * 2f, seed + 3f + Time.time * 2f) - 0.5f;
				float z = Mathf.PerlinNoise(seed + 4f + Time.time * 2f, seed + 5f + Time.time * 2f) - 0.5f;
				firePointLight.gameObject.transform.localPosition = Vector3.up + new Vector3(x, y, z);
			}
			else if (fireBaseScript.Stopping)
			{
				firePointLight.intensity = Mathf.Lerp(firePointLight.intensity, 0f, fireBaseScript.StopPercent);
			}
			else if (fireBaseScript.Starting)
			{
				firePointLight.intensity = Mathf.Lerp(0f, lightIntensity, fireBaseScript.StartPercent);
			}
		}
	}
}
