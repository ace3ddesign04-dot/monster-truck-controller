using UnityEngine;

namespace DigitalRuby.PyroParticles
{
	public class FireCollisionForwardScript : MonoBehaviour
	{
		public ICollisionHandler CollisionHandler;

		public void OnCollisionEnter(Collision col)
		{
			CollisionHandler.HandleCollision(base.gameObject, col);
		}
	}
}
