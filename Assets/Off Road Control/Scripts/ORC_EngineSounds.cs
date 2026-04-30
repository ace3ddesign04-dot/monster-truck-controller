using CustomVP;
using UnityEngine;

public class ORC_EngineSounds : MonoBehaviour
{
	[HideInInspector]
	public float RPM;

	public float RPMLimit = 6000f;

	public AudioClip idleClip;

	public AnimationCurve idleVolCurve;

	public AnimationCurve idlePitchCurve;

	public AudioClip lowOffClip;

	public AudioClip lowOnClip;

	public AnimationCurve lowVolCurve;

	public AnimationCurve lowPitchCurve;

	public AudioClip medOffClip;

	public AudioClip medOnClip;

	public AnimationCurve medVolCurve;

	public AnimationCurve medPitchCurve;

	public AudioClip highOffClip;

	public AudioClip highOnClip;

	public AnimationCurve highVolCurve;

	public AnimationCurve highPitchCurve;

	public AudioClip maxRPMClip;

	public AnimationCurve maxRPMVolCurve;

	public AnimationCurve maxRPMPitchCurve;

	public AudioClip turboClip;

	public AnimationCurve turboVolCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

	public AnimationCurve turboPitchCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 2f));

	private AudioSource gearShiftSound;

	private AudioSource idleSound;

	private AudioSource lowOffSound;

	private AudioSource lowOnSound;

	private AudioSource medOffSound;

	private AudioSource medOnSound;

	private AudioSource highOffSound;

	private AudioSource highOnSound;

	private AudioSource limiterSound;

	// private AudioSource turboSound;

	public float volumeLevelInSettings;

	// [HideInInspector]
	public float load;

	[HideInInspector]
	public bool RevLimiterAllowed;

	[HideInInspector]
	public bool Turbo;

	private bool SoundsLoaded;

	private bool SoundsDestroyed;

	private CarController _orcTruckController;

	private void Start()
	{
		_orcTruckController = GetComponentInParent<CarController>();
		LoadSounds();
	}

	private void LoadSounds()
	{
		idleSound = CreateAudioSource(idleClip);
		lowOffSound = CreateAudioSource(lowOffClip);
		lowOnSound = CreateAudioSource(lowOnClip);
		medOffSound = CreateAudioSource(medOffClip);
		medOnSound = CreateAudioSource(medOnClip);
		highOffSound = CreateAudioSource(highOffClip);
		highOnSound = CreateAudioSource(highOnClip);
		limiterSound = CreateAudioSource(maxRPMClip);
		// turboSound = CreateAudioSource(turboClip);
		// gearShiftSound = (UnityEngine.Object.Instantiate(Resources.Load("Sounds/GearShift", typeof(GameObject)), base.transform.position, base.transform.rotation, base.transform) as GameObject).GetComponent<AudioSource>();
		SoundsLoaded = true;
	}

	private void DestroySounds()
	{
		if (!SoundsDestroyed)
		{
			UnityEngine.Object.Destroy(lowOnSound);
			UnityEngine.Object.Destroy(medOnSound);
			UnityEngine.Object.Destroy(highOnSound);
			UnityEngine.Object.Destroy(limiterSound);
			// UnityEngine.Object.Destroy(turboSound);
			UnityEngine.Object.Destroy(gearShiftSound);
		}
	}

	private void OnEnable()
	{
		if (SoundsLoaded)
		{
			idleSound.Play();
			lowOffSound.Play();
			lowOnSound.Play();
			medOffSound.Play();
			medOnSound.Play();
			highOffSound.Play();
			highOnSound.Play();
			limiterSound.Play();
			// turboSound.Play();
		}
	}

	private void OnDisable()
	{
		if (SoundsLoaded)
		{
			if (idleSound != null)
			{
				idleSound.Stop();
			}
			if (lowOffSound != null)
			{
				lowOffSound.Stop();
			}
			if (lowOnSound != null)
			{
				lowOnSound.Stop();
			}
			if (medOffSound != null)
			{
				medOffSound.Stop();
			}
			if (medOnSound != null)
			{
				medOnSound.Stop();
			}
			if (highOffSound != null)
			{
				highOffSound.Stop();
			}
			if (highOnSound != null)
			{
				highOnSound.Stop();
			}
			if (limiterSound != null)
			{
				limiterSound.Stop();
			}
			// if (turboSound != null)
			// {
			// 	turboSound.Stop();
			// }
		}
	}

	private void Update()
	{
		float time = Mathf.Abs(RPM / RPMLimit);
		float num = idleVolCurve.Evaluate(time);
		float num2 = lowVolCurve.Evaluate(time);
		float num3 = medVolCurve.Evaluate(time);
		float num4 = highVolCurve.Evaluate(time);
		float num5 = maxRPMVolCurve.Evaluate(time);
		// float num6 = turboVolCurve.Evaluate(time);

		// Fix
		// volumeLevelInSettings = SoundManager.Instance.soundSource.volume * 0.3f;
		volumeLevelInSettings = 1f;
		if (!RevLimiterAllowed)
		{
			num5 = 0f * volumeLevelInSettings;
		}
		if (_orcTruckController == null)
		{
			DestroySounds();
			load = 0f;
		}
		if (num > 0.01f)
		{
			idleSound.volume = num * volumeLevelInSettings;
			idleSound.pitch = idlePitchCurve.Evaluate(time);
		}
		if (num2 > 0.01f)
		{
			if (lowOnSound != null)
			{
				lowOnSound.volume = num2 * load * volumeLevelInSettings;
				lowOnSound.pitch = lowPitchCurve.Evaluate(time);
			}
			lowOffSound.volume = num2 * (1f - load) * volumeLevelInSettings;
			lowOffSound.pitch = lowPitchCurve.Evaluate(time);
		}
		if (num3 > 0.01f)
		{
			if (medOnSound != null)
			{
				medOnSound.volume = num3 * load * volumeLevelInSettings;
				medOnSound.pitch = medPitchCurve.Evaluate(time);
			}
			medOffSound.volume = num3 * (1f - load) * volumeLevelInSettings;
			medOffSound.pitch = medPitchCurve.Evaluate(time);
		}
		if (num4 > 0.01f)
		{
			if (highOnSound != null)
			{
				highOnSound.volume = num4 * load * (1f - limiterSound.volume) * volumeLevelInSettings;
				highOnSound.pitch = highPitchCurve.Evaluate(time);
			}
			highOffSound.volume = num4 * (1f - load) * volumeLevelInSettings;
			highOffSound.pitch = highPitchCurve.Evaluate(time);
		}
		if (limiterSound != null)
		{
			limiterSound.volume = num5 * load * volumeLevelInSettings;
			limiterSound.pitch = maxRPMPitchCurve.Evaluate(time);
		}
		// if (num6 > 0.05f && turboSound != null)
		// {
		// 	turboSound.volume = num6 * load * (float)(Turbo ? 1 : 0) * volumeLevelInSettings;
		// 	turboSound.pitch = turboPitchCurve.Evaluate(time);
		// }
	}

	public void GearShift()
	{
		if (gearShiftSound != null && !gearShiftSound.isPlaying)
		{
			gearShiftSound.Play();
		}
	}

	private AudioSource CreateAudioSource(AudioClip clip)
	{
		AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
		audioSource.maxDistance = 20f;
		audioSource.minDistance = 5f;
		audioSource.clip = clip;
		audioSource.loop = true;
		audioSource.spatialBlend = 1f;
		audioSource.volume = 0f;
		audioSource.pitch = 0f;
		audioSource.Play();
		return audioSource;
	}
}
