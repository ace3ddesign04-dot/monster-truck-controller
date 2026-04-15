using UnityEngine;

namespace Gaia
{
	public static class GaiaConstants
	{
		public enum ManagerEditorMode
		{
			Standard,
			Advanced,
			Utilities,
			Extensions
		}

		public enum OperationMode
		{
			DesignTime,
			RuntimeInterval,
			RuntimeTriggeredInterval
		}

		public enum NoiseType
		{
			None,
			Perlin,
			Billow,
			Ridged
		}

		public enum ImageFitnessFilterMode
		{
			None,
			ImageGreyScale,
			ImageRedChannel,
			ImageGreenChannel,
			ImageBlueChannel,
			ImageAlphaChannel,
			TerrainTexture0,
			TerrainTexture1,
			TerrainTexture2,
			TerrainTexture3,
			TerrainTexture4,
			TerrainTexture5,
			TerrainTexture6,
			TerrainTexture7,
			PerlinNoise,
			BillowNoise,
			RidgedNoise
		}

		public enum FeatureType
		{
			Adhoc,
			Bases,
			Hills,
			Islands,
			Lakes,
			Mesas,
			Mountains,
			Plains,
			Rivers,
			Rocks,
			Valleys,
			Villages,
			Waterfalls
		}

		public enum GeneratorBorderStyle
		{
			None,
			Mountains,
			Water
		}

		public enum FeatureOperation
		{
			RaiseHeight,
			LowerHeight,
			BlendHeight,
			StencilHeight,
			DifferenceHeight
		}

		public enum SpawnerShape
		{
			Box,
			Sphere
		}

		public enum SpawnerLocation
		{
			RandomLocation,
			RandomLocationClustered,
			EveryLocation,
			EveryLocationJittered
		}

		public enum SpawnerLocationCheckType
		{
			PointCheck,
			BoundedAreaCheck
		}

		public enum SpawnerRuleSelector
		{
			All,
			Fittest,
			WeightedFittest,
			Random
		}

		public enum SpawnerResourceType
		{
			TerrainTexture,
			TerrainDetail,
			TerrainTree,
			GameObject
		}

		public enum StorageFormat
		{
			PNG,
			JPG
		}

		public enum ImageChannel
		{
			R,
			G,
			B,
			A
		}

		public static string GaiaMajorVersion = "1";

		public static string GaiaMinorVersion = "6.1";

		public static readonly string AssetDir = "Gaia/Stamps";

		public static readonly string AssetDirFromAssetDB = "Assets/Gaia/Stamps";

		public static float VirginTerrainCheckThreshold = 0.01f;

		public const TextureFormat defaultTextureFormat = TextureFormat.RGBA32;

		public const TextureFormat fmtHmTextureFormat = TextureFormat.RGBA32;

		public const TextureFormat fmtRGBA32 = TextureFormat.RGBA32;

		public const StorageFormat defaultImageStorageFormat = StorageFormat.PNG;

		public const ImageChannel defaultImageStorageChannel = ImageChannel.R;
	}
}
