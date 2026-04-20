using UnityEngine;

namespace Crosstales.BWF.Manager
{
	[ExecuteInEditMode]
	public abstract class BaseManager : MonoBehaviour
	{
		[Header("Marker Settings")]
		[Tooltip("Mark prefix for bad words (default: bold and color).")]
		public string MarkPrefix = "<b><color=red>";

		[Tooltip("Mark postfix for bad words (default: bold and color).")]
		public string MarkPostfix = "</color></b>";

		[Header("Behaviour Settings")]
		[Tooltip("Don't destroy gameobject during scene switches (default: true).")]
		public bool DontDestroy = true;
	}
}
