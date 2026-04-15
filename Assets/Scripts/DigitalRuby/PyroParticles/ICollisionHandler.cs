using UnityEngine;

namespace DigitalRuby.PyroParticles
{
	public interface ICollisionHandler
	{
		void HandleCollision(GameObject obj, Collision c);
	}
}
