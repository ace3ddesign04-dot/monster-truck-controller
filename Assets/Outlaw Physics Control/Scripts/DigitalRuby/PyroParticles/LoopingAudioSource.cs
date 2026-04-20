using UnityEngine;

namespace DigitalRuby.PyroParticles
{
	public class LoopingAudioSource
	{
		private float startMultiplier;

		private float stopMultiplier;

		private float currentMultiplier;

		public AudioSource AudioSource
		{
			get;
			private set;
		}

		public float TargetVolume
		{
			get;
			private set;
		}

		public LoopingAudioSource(MonoBehaviour script, AudioSource audioSource, float startMultiplier, float stopMultiplier)
		{
			AudioSource = audioSource;
			if (audioSource != null)
			{
				AudioSource.loop = true;
				AudioSource.volume = 0f;
				AudioSource.Stop();
			}
			TargetVolume = 1f;
			this.startMultiplier = (currentMultiplier = startMultiplier);
			this.stopMultiplier = stopMultiplier;
		}

		public void Play()
		{
			Play(TargetVolume);
		}

		public void Play(float targetVolume)
		{
			if (AudioSource != null && !AudioSource.isPlaying)
			{
				AudioSource.volume = 0f;
				AudioSource.Play();
				currentMultiplier = startMultiplier;
			}
			TargetVolume = targetVolume;
		}

		public void Stop()
		{
			if (AudioSource != null && AudioSource.isPlaying)
			{
				TargetVolume = 0f;
				currentMultiplier = stopMultiplier;
			}
		}

		public void Update()
		{
			if (AudioSource != null && AudioSource.isPlaying)
			{
				float num = Mathf.Lerp(AudioSource.volume, TargetVolume, Time.deltaTime / currentMultiplier);
				AudioSource.volume = num;
				if (num == 0f)
				{
					AudioSource.Stop();
				}
			}
		}
	}
}
