using UnityEngine;

namespace AGS_MonsterTruckControl
{
	public class AGS_MTC_CarEffects : MonoBehaviour
	{

		private AGS_MTC_CarController carController;

		private AGS_MTC_EngineController engine;

		private AudioSource HitSound;

		private AudioSource SkidSound;

		private AudioSource WheelBumpSound;

		private AudioSource OffroadSound;

		private AudioSource WaterSplashSound;

		private ParticleSystem ExhaustParticle;

		public Transform[] ExhaustPoints;

		[Header("Impacts")]
		public float MinCollisionSoundForce = 2000f;

		public float SkidMaxVolume = 1f;

		public float MaxSplashVolume = 0.5f;

		private float MinWheelBumpValue = 1f;

		private float OffroadMaxVolume = 0.2f;

		private float LastHitTime;

		private bool ResourcesLoaded;

		private void Awake()
		{
			carController = GetComponent<AGS_MTC_CarController>();
			engine = GetComponent<AGS_MTC_EngineController>();
		}

		private void LoadResources()
		{
			Transform transform = base.transform.Find("Sounds");
			if (transform == null)
			{
				transform = base.transform;
			}
			HitSound = (UnityEngine.Object.Instantiate(Resources.Load("Sounds/Hit", typeof(GameObject)), base.transform.position, base.transform.rotation, transform) as GameObject).GetComponent<AudioSource>();
			SkidSound = (UnityEngine.Object.Instantiate(Resources.Load("Sounds/Skid", typeof(GameObject)), base.transform.position, base.transform.rotation, transform) as GameObject).GetComponent<AudioSource>();
			WheelBumpSound = (UnityEngine.Object.Instantiate(Resources.Load("Sounds/WheelBump", typeof(GameObject)), base.transform.position, base.transform.rotation, transform) as GameObject).GetComponent<AudioSource>();
			OffroadSound = (UnityEngine.Object.Instantiate(Resources.Load("Sounds/Offroad", typeof(GameObject)), base.transform.position, base.transform.rotation, transform) as GameObject).GetComponent<AudioSource>();
			WaterSplashSound = (UnityEngine.Object.Instantiate(Resources.Load("Sounds/WaterSplash", typeof(GameObject)), base.transform.position, base.transform.rotation, transform) as GameObject).GetComponent<AudioSource>();
			ExhaustParticle = (UnityEngine.Object.Instantiate(Resources.Load("ParticleEffects/ExhaustParticle", typeof(ParticleSystem))) as ParticleSystem);
			ExhaustParticle.transform.parent = base.transform;
			ExhaustParticle.transform.localPosition = Vector3.zero;
			ResourcesLoaded = true;
		}

		private void Update()
		{
			if (!(carController == null))
			{
				if (!ResourcesLoaded)
				{
					LoadResources();
				}
				DoParticles();
				DoWheelBumpSounds();
			}
		}

		private void DoParticles()
		{
			if (ExhaustParticle == null || ExhaustPoints == null || engine == null || !engine.Diesel)
			{
				return;
			}
			Transform[] exhaustPoints = ExhaustPoints;
			foreach (Transform transform in exhaustPoints)
			{
				if (transform.gameObject.activeInHierarchy && Mathf.Abs(carController.Throttle) == 1f && Mathf.Abs(carController.Speed) < 10f)
				{
					ExhaustParticle.transform.position = transform.position;
					ExhaustParticle.transform.rotation = transform.rotation;
					ExhaustParticle.Emit(1);
				}
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			DoHitSounds(collision);
		}

		private void DoWheelBumpSounds()
		{
			if (!(WheelBumpSound == null))
			{
				foreach (AGS_MTC_Wheel wheel in carController.wheels)
				{
					if (wheel.wc.deltaCompression > 0.1f * MinWheelBumpValue && wheel.wc.wheelCollider.isGrounded && !WheelBumpSound.isPlaying)
					{
						WheelBumpSound.Play();
					}
				}
			}
		}

		private void DoSurfaceSounds()
		{
			if (!(SkidSound == null) && !(OffroadSound == null))
			{
				if (!SkidSound.isPlaying)
				{
					SkidSound.Play();
				}
				if (!OffroadSound.isPlaying)
				{
					OffroadSound.Play();
				}
				if (!WaterSplashSound.isPlaying)
				{
					WaterSplashSound.Play();
				}
				float num = Mathf.Lerp(0f, 1f, (Mathf.Abs(carController.Speed) - 1f) / 6f);
				float t = (!carController.Grounded()) ? 0f : Mathf.Lerp(0f, 1f, Mathf.Abs(carController.Speed) / 20f);
				float num2 = 0f;
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				
				foreach (AGS_MTC_Wheel wheel in carController.wheels)
				{
					if (wheel.wc.sLong > num2 && wheel.wc.wheelCollider.isGrounded)
					{
						num2 = wheel.wc.CommonSlip;
					}
				}
				SkidSound.volume = ((!flag) ? 0f : Mathf.Lerp(0f, SkidMaxVolume, (num2 - 0.3f) / 0.7f * num));
				OffroadSound.volume = ((!flag2) ? 0f : Mathf.Lerp(0f, OffroadMaxVolume, t));
				WaterSplashSound.volume = ((!flag3) ? 0f : Mathf.Lerp(0f, MaxSplashVolume, t));
			}
		}

		private void DoHitSounds(Collision collision)
		{
			if (HitSound == null || (double)(Time.time - LastHitTime) < 0.5 || collision.impulse.magnitude < MinCollisionSoundForce)
			{
				return;
			}
			bool flag = false;
			ContactPoint[] contacts = collision.contacts;
			for (int i = 0; i < contacts.Length; i++)
			{
				ContactPoint contactPoint = contacts[i];
				Collider[] bodyColliders = carController.BodyColliders;
				foreach (Collider y in bodyColliders)
				{
					if (contactPoint.thisCollider == y)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				HitSound.Play();
				LastHitTime = Time.time;
			}
		}
	}
}
