using UnityEngine;

namespace Battlehub.Integration
{
	public static class MeshCombinerIntegration
	{
		public static event IntegrationHandler Combined;

		public static event IntegrationHandler BeginEditPivot;

		public static void RaiseCombined(GameObject go, Mesh mesh)
		{
			if (MeshCombinerIntegration.Combined != null)
			{
				MeshCombinerIntegration.Combined(new IntegrationArgs(go, mesh));
			}
		}

		public static bool RaiseBeginEditPivot(GameObject go, Mesh mesh)
		{
			if (MeshCombinerIntegration.BeginEditPivot != null)
			{
				IntegrationArgs integrationArgs = new IntegrationArgs(go, mesh);
				MeshCombinerIntegration.BeginEditPivot(integrationArgs);
				return !integrationArgs.Cancel;
			}
			return true;
		}
	}
}
