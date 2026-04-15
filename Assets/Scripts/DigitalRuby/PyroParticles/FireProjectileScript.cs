using System.Collections;
using UnityEngine;

namespace DigitalRuby.PyroParticles
{
	public class FireProjectileScript : FireBaseScript, ICollisionHandler
	{
		[Tooltip("The collider object to use for collision and physics.")]
		public GameObject ProjectileColliderObject;

		[Tooltip("The sound to play upon collision.")]
		public AudioSource ProjectileCollisionSound;

		[Tooltip("The particle system to play upon collision.")]
		public ParticleSystem ProjectileExplosionParticleSystem;

		[Tooltip("The radius of the explosion upon collision.")]
		public float ProjectileExplosionRadius = 50f;

		[Tooltip("The force of the explosion upon collision.")]
		public float ProjectileExplosionForce = 50f;

		[Tooltip("An optional delay before the collider is sent off, in case the effect has a pre fire animation.")]
		public float ProjectileColliderDelay;

		[Tooltip("The speed of the collider.")]
		public float ProjectileColliderSpeed = 450f;

		[Tooltip("The direction that the collider will go. For example, flame strike goes down, and fireball goes forward.")]
		public Vector3 ProjectileDirection = Vector3.forward;

		[Tooltip("What layers the collider can collide with.")]
		public LayerMask ProjectileCollisionLayers = -1;

		[Tooltip("Particle systems to destroy upon collision.")]
		public ParticleSystem[] ProjectileDestroyParticleSystemsOnCollision;

		[HideInInspector]
		public FireProjectileCollisionDelegate CollisionDelegate;

		private bool collided;

		private IEnumerator SendCollisionAfterDelay()
		{
			yield return new WaitForSeconds(ProjectileColliderDelay);
			Vector3 dir2 = ProjectileDirection * ProjectileColliderSpeed;
			dir2 = ProjectileColliderObject.transform.rotation * dir2;
			ProjectileColliderObject.GetComponent<Rigidbody>().velocity = dir2;
		}

		protected override void Start()
		{
			base.Start();
			StartCoroutine(SendCollisionAfterDelay());
		}

		public void HandleCollision(GameObject obj, Collision c)
		{
			if (collided)
			{
				return;
			}
			collided = true;
			Stop();
			if (ProjectileDestroyParticleSystemsOnCollision != null)
			{
				ParticleSystem[] projectileDestroyParticleSystemsOnCollision = ProjectileDestroyParticleSystemsOnCollision;
				foreach (ParticleSystem obj2 in projectileDestroyParticleSystemsOnCollision)
				{
					UnityEngine.Object.Destroy(obj2, 0.1f);
				}
			}
			if (ProjectileCollisionSound != null)
			{
				ProjectileCollisionSound.Play();
			}
			if (c.contacts.Length != 0)
			{
				ProjectileExplosionParticleSystem.transform.position = c.contacts[0].point;
				ProjectileExplosionParticleSystem.Play();
				FireBaseScript.CreateExplosion(c.contacts[0].point, ProjectileExplosionRadius, ProjectileExplosionForce);
				if (CollisionDelegate != null)
				{
					CollisionDelegate(this, c.contacts[0].point);
				}
			}
		}
	}
}
