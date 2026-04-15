using UnityEngine;

namespace DigitalRuby.PyroParticles
{
	public class FireConstantBaseScript : FireBaseScript
	{
		[HideInInspector]
		public LoopingAudioSource LoopingAudioSource;

		protected override void Awake()
		{
			base.Awake();
			LoopingAudioSource = new LoopingAudioSource(this, AudioSource, StartTime, StopTime);
			Duration = 1E+09f;
		}

		protected override void Update()
		{
			base.Update();
			LoopingAudioSource.Update();
		}

		protected override void Start()
		{
			base.Start();
			LoopingAudioSource.Play();
		}

		public override void Stop()
		{
			LoopingAudioSource.Stop();
			base.Stop();
		}
	}
}
