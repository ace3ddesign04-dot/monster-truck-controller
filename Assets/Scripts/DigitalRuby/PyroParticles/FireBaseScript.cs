using System;
using System.Collections;
using UnityEngine;

namespace DigitalRuby.PyroParticles
{
	public class FireBaseScript : MonoBehaviour
	{
		[Tooltip("Optional audio source to play once when the script starts.")]
		public AudioSource AudioSource;

		[Tooltip("How long the script takes to fully start. This is used to fade in animations and sounds, etc.")]
		public float StartTime = 1f;

		[Tooltip("How long the script takes to fully stop. This is used to fade out animations and sounds, etc.")]
		public float StopTime = 3f;

		[Tooltip("How long the effect lasts. Once the duration ends, the script lives for StopTime and then the object is destroyed.")]
		public float Duration = 2f;

		[Tooltip("How much force to create at the center (explosion), 0 for none.")]
		public float ForceAmount;

		[Tooltip("The radius of the force, 0 for none.")]
		public float ForceRadius;

		[Tooltip("A hint to users of the script that your object is a projectile and is meant to be shot out from a person or trap, etc.")]
		public bool IsProjectile;

		[Tooltip("Particle systems that must be manually started and will not be played on start.")]
		public ParticleSystem[] ManualParticleSystems;

		private float startTimeMultiplier;

		private float startTimeIncrement;

		private float stopTimeMultiplier;

		private float stopTimeIncrement;

		public bool Starting
		{
			get;
			private set;
		}

		public float StartPercent
		{
			get;
			private set;
		}

		public bool Stopping
		{
			get;
			private set;
		}

		public float StopPercent
		{
			get;
			private set;
		}

		private IEnumerator CleanupEverythingCoRoutine()
		{
			yield return new WaitForSeconds(StopTime + 2f);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		private void StartParticleSystems()
		{
			ParticleSystem[] componentsInChildren = base.gameObject.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem particleSystem in componentsInChildren)
			{
				if (ManualParticleSystems == null || ManualParticleSystems.Length == 0 || Array.IndexOf(ManualParticleSystems, particleSystem) < 0)
				{
					if (particleSystem.startDelay == 0f)
					{
						particleSystem.startDelay = 0.01f;
					}
					particleSystem.Play();
				}
			}
		}

		protected virtual void Awake()
		{
			Starting = true;
			int num = LayerMask.NameToLayer("FireLayer");
			Physics.IgnoreLayerCollision(num, num);
		}

		protected virtual void Start()
		{
			if (AudioSource != null)
			{
				AudioSource.Play();
			}
			stopTimeMultiplier = 1f / StopTime;
			startTimeMultiplier = 1f / StartTime;
			CreateExplosion(base.gameObject.transform.position, ForceRadius, ForceAmount);
			StartParticleSystems();
			ICollisionHandler collisionHandler = this as ICollisionHandler;
			if (collisionHandler != null)
			{
				FireCollisionForwardScript componentInChildren = GetComponentInChildren<FireCollisionForwardScript>();
				if (componentInChildren != null)
				{
					componentInChildren.CollisionHandler = collisionHandler;
				}
			}
		}

		protected virtual void Update()
		{
			Duration -= Time.deltaTime;
			if (Stopping)
			{
				stopTimeIncrement += Time.deltaTime;
				if (stopTimeIncrement < StopTime)
				{
					StopPercent = stopTimeIncrement * stopTimeMultiplier;
				}
			}
			else if (Starting)
			{
				startTimeIncrement += Time.deltaTime;
				if (startTimeIncrement < StartTime)
				{
					StartPercent = startTimeIncrement * startTimeMultiplier;
				}
				else
				{
					Starting = false;
				}
			}
			else if (Duration <= 0f)
			{
				Stop();
			}
		}

		public static void CreateExplosion(Vector3 pos, float radius, float force)
		{
			if (force <= 0f || radius <= 0f)
			{
				return;
			}
			Collider[] array = Physics.OverlapSphere(pos, radius);
			Collider[] array2 = array;
			foreach (Collider collider in array2)
			{
				Rigidbody component = collider.GetComponent<Rigidbody>();
				if (component != null)
				{
					component.AddExplosionForce(force, pos, radius);
				}
			}
		}

		public virtual void Stop()
		{
			if (!Stopping)
			{
				Stopping = true;
				ParticleSystem[] componentsInChildren = base.gameObject.GetComponentsInChildren<ParticleSystem>();
				foreach (ParticleSystem particleSystem in componentsInChildren)
				{
					particleSystem.Stop();
				}
				StartCoroutine(CleanupEverythingCoRoutine());
			}
		}
	}
}
