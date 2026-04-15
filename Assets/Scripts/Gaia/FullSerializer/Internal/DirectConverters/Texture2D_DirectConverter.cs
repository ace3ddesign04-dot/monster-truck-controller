using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gaia.FullSerializer.Internal.DirectConverters
{
	public class Texture2D_DirectConverter : fsDirectConverter<Texture2D>
	{
		protected override fsResult DoSerialize(Texture2D model, Dictionary<string, fsData> serialized)
		{
			fsResult success = fsResult.Success;
			if (model == null)
			{
				return success + SerializeMember(serialized, "present", value: false);
			}
			success += SerializeMember(serialized, "present", value: true);
			success += SerializeMember(serialized, "name", model.name);
			success += SerializeMember(serialized, "path", string.Empty);
			success += SerializeMember(serialized, "width", model.width);
			return success + SerializeMember(serialized, "height", model.height);
		}

		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Texture2D model)
		{
			fsResult success = fsResult.Success;
			bool value = false;
			success += DeserializeMember(data, "present", out value);
			if (value)
			{
				string value2 = string.Empty;
				success += DeserializeMember(data, "path", out value2);
				if (!string.IsNullOrEmpty(value2))
				{
					model = (Utils.GetAsset(value2, typeof(Texture2D)) as Texture2D);
					if (model == null)
					{
						UnityEngine.Debug.LogWarning("Unable to locate asset : " + value2);
					}
					else
					{
						string value3 = model.name;
						success += DeserializeMember(data, "name", out value3);
						model.name = value3;
					}
				}
			}
			return success;
		}

		public override object CreateInstance(fsData data, Type storageType)
		{
			return new Texture2D(0, 0);
		}
	}
}
