using System.Collections.Generic;
using UnityEngine;

namespace MeshCombineStudio
{
	public class SwapCombineKey : MonoBehaviour
	{
		public static List<MeshCombiner> meshCombinerList = new List<MeshCombiner>();

		private MeshCombiner meshCombiner;

		private void Awake()
		{
			meshCombiner = GetComponent<MeshCombiner>();
			meshCombinerList.Add(meshCombiner);
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(meshCombiner.combineSwapKey))
			{
				meshCombiner.SwapCombine();
			}
		}

		private void OnGUI()
		{
			GUI.color = Color.red;
			GUI.Label(new Rect(10f, 10f, 200f, 20f), "Toggle with '" + this.meshCombiner.combineSwapKey.ToString() + "' key.");
			for (int i = 0; i < meshCombinerList.Count; i++)
			{
				MeshCombiner meshCombiner = meshCombinerList[i];
				if (meshCombiner.combinedActive)
				{
					GUI.Label(new Rect(10f, 30 + i * 20, 300f, 20f), meshCombiner.gameObject.name + " is Enabled.");
				}
				else
				{
					GUI.Label(new Rect(10f, 30 + i * 20, 300f, 20f), meshCombiner.gameObject.name + " is Disabled.");
				}
			}
			GUI.color = Color.white;
		}
	}
}
