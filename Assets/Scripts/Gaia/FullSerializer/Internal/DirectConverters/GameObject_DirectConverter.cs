using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gaia.FullSerializer.Internal.DirectConverters
{
	public class GameObject_DirectConverter : fsDirectConverter<GameObject>
	{
		protected override fsResult DoSerialize(GameObject model, Dictionary<string, fsData> serialized)
		{
			fsResult success = fsResult.Success;
			if (model == null)
			{
				return success + SerializeMember(serialized, "present", value: false);
			}
			success += SerializeMember(serialized, "present", value: true);
			success += SerializeMember(serialized, "name", model.name);
			return success + SerializeMember(serialized, "path", string.Empty);
		}

		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref GameObject model)
		{
			fsResult success = fsResult.Success;
			bool value = false;
			success += DeserializeMember(data, "present", out value);
			if (value)
			{
				string value2 = model.name;
				success += DeserializeMember(data, "name", out value2);
				model.name = value2;
				string value3 = string.Empty;
				success += DeserializeMember(data, "path", out value3);
				if (string.IsNullOrEmpty(value3))
				{
				}
			}
			return success;
		}

		public override object CreateInstance(fsData data, Type storageType)
		{
			return new Texture2D(1024, 1024);
		}
	}
}
