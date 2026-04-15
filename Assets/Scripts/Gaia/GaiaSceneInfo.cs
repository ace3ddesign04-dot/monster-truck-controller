using UnityEngine;

namespace Gaia
{
	public class GaiaSceneInfo
	{
		public Bounds m_sceneBounds = default(Bounds);

		public Vector3 m_centrePointOnTerrain = Vector3.zero;

		public float m_seaLevel;

		public static GaiaSceneInfo GetSceneInfo()
		{
			GaiaSceneInfo gaiaSceneInfo = new GaiaSceneInfo();
			Terrain activeTerrain = TerrainHelper.GetActiveTerrain();
			if (activeTerrain == null)
			{
				UnityEngine.Debug.LogWarning("You must have a valid terrain for sceneinfo to work correctly.");
			}
			else
			{
				GaiaSessionManager sessionManager = GaiaSessionManager.GetSessionManager();
				TerrainHelper.GetTerrainBounds(activeTerrain, ref gaiaSceneInfo.m_sceneBounds);
				gaiaSceneInfo.m_seaLevel = sessionManager.GetSeaLevel();
				GaiaSceneInfo gaiaSceneInfo2 = gaiaSceneInfo;
				Vector3 center = gaiaSceneInfo.m_sceneBounds.center;
				float x = center.x;
				float y = activeTerrain.SampleHeight(gaiaSceneInfo.m_sceneBounds.center);
				Vector3 center2 = gaiaSceneInfo.m_sceneBounds.center;
				gaiaSceneInfo2.m_centrePointOnTerrain = new Vector3(x, y, center2.z);
			}
			return gaiaSceneInfo;
		}
	}
}
