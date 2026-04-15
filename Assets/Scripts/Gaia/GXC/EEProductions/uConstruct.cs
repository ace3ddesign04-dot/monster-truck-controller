using UnityEngine;

namespace Gaia.GXC.EEProductions
{
	public class uConstruct : MonoBehaviour
	{
		public static string GetPublisherName()
		{
			return "EE Productions";
		}

		public static string GetPackageName()
		{
			return "uConstruct - Runtime Building System";
		}

		public static string GetPackageImage()
		{
			return "uConstruct";
		}

		public static string GetPackageDescription()
		{
			return "Do you want your game players to be able to construct their own buildings and structures? \r\n\r\nuConstruct is an easy-to-implement, run-time, socket-based building system.\r\nuConstruct can save any of the created structures for restoration.\r\nThis capability also allows developers to use uConstruct as part of their level design/creation toolkit. \r\n\r\nFEATURES:\r\n\r\nSocket based construction: Building components snap into position in snap sockets, or free placed on the socket bounds on free placed sockets, while allowing visual scaling and rotation.\r\n\r\nConditional placement: Ability to set the conditions for building placement within your worlds. Four very useful examples are included that you can extend for your requirements.\r\n\r\nDraw call batching: uConstruct can reduce draw calls from thousands down to a few. The performance from batching eliminates the need for creating LOD views.\r\n\r\nArea of interest influence: this multi-threaded subsystem of uConstruct allows you define the players influence on Unity physics, sockets and conditionals based on the player proximity to the area. Eliminating contention with the Unity main thread and optimizing your collider count.\r\n\r\nTemplates: Through the use of templates you can predefine sockets and conditions and apply them to any of the structures. With a template you can uniformly impact as many structures as you want instead of adjusting the same settings across multiple structures.\r\n\r\nPrefab structure database: Each structure is assigned its own unique ID and enables the ability to access the structure's prefab at run-time. This is very useful for networking, pooling systems, etc.\r\n\r\nCustom physics: uConstruct has a socket optimized physics sub-system that greatly improves performance. Utilizing this feature does not preclude you from using Unity’s default physics system.\r\n\r\nStructure saving: Built-in structure saving system that is fully extendable for your own data needs.\r\n\r\nBuilding types code generator: You can create your own building types within the editor without the requirement of having to adjust the source code.\r\n\r\nSmart optimizations: uConstruct will detect and implement many optimizations at run-time. For example, it will disable used or overlapping sockets.\r\n\r\nFully Functional API: The API extends your flexibility to only be limited by your or your player’s creativity.\r\n\r\nComplete documentation.\r\n\r\nVideo tutorials.\r\n\r\nFull source code included.";
		}

		public static string GetPackageURL()
		{
			return "https://www.assetstore.unity3d.com/en/#!/content/51881";
		}
	}
}
