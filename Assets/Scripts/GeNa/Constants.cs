namespace GeNa
{
	public static class Constants
	{
		public enum SpawnRangeShape
		{
			Circle,
			Square
		}

		public enum LocationAlgorithm
		{
			Centered,
			Every,
			LastSpawn,
			Organic
		}

		public enum RotationAlgorithm
		{
			Ranged,
			LastSpawnCenter,
			LastSpawnClosest
		}

		public enum VirginCheckType
		{
			None,
			Point,
			Bounds
		}

		public enum ResourceType
		{
			Prefab,
			TerrainTree,
			TerrainGrass
		}

		public enum MaskType
		{
			Perlin,
			Billow,
			Ridged,
			Image
		}

		public static string MajorVersion = "1";

		public static string MinorVersion = "5.0.2";

		public const float MinimimProbeGroupDistance = 100f;

		public const float MinimimProbeDistance = 15f;

		public const float MaximumOptimisationSize = 10f;
	}
}
