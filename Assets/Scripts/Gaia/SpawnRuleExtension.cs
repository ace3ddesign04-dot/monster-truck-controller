using UnityEngine;

namespace Gaia
{
	public class SpawnRuleExtension : MonoBehaviour
	{
		public virtual void Initialise()
		{
		}

		public virtual bool AffectsTextures()
		{
			return false;
		}

		public virtual bool AffectsDetails()
		{
			return false;
		}

		public virtual float GetFitness(float fitness, ref SpawnInfo spawnInfo)
		{
			return fitness;
		}

		public virtual bool OverridesSpawn(SpawnRule spawnRule, ref SpawnInfo spawnInfo)
		{
			return false;
		}

		public virtual void Spawn(SpawnRule spawnRule, ref SpawnInfo spawnInfo)
		{
		}

		public virtual void PostSpawn(SpawnRule spawnRule, ref SpawnInfo spawnInfo)
		{
		}
	}
}
