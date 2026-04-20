using System;
using UnityEngine;

namespace Gaia.FullSerializer.Internal
{
	public class fsSerializationCallbackReceiverProcessor : fsObjectProcessor
	{
		public override bool CanProcess(Type type)
		{
			return typeof(ISerializationCallbackReceiver).IsAssignableFrom(type);
		}

		public override void OnBeforeSerialize(Type storageType, object instance)
		{
			((ISerializationCallbackReceiver)instance).OnBeforeSerialize();
		}

		public override void OnAfterDeserialize(Type storageType, object instance)
		{
			((ISerializationCallbackReceiver)instance).OnAfterDeserialize();
		}
	}
}
