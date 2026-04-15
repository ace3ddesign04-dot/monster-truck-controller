using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gaia.FullSerializer.Internal.DirectConverters
{
	public class AnimationCurve_DirectConverter : fsDirectConverter<AnimationCurve>
	{
		protected override fsResult DoSerialize(AnimationCurve model, Dictionary<string, fsData> serialized)
		{
			fsResult success = fsResult.Success;
			success += SerializeMember(serialized, "keys", model.keys);
			success += SerializeMember(serialized, "preWrapMode", model.preWrapMode);
			return success + SerializeMember(serialized, "postWrapMode", model.postWrapMode);
		}

		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref AnimationCurve model)
		{
			fsResult success = fsResult.Success;
			Keyframe[] value = model.keys;
			success += DeserializeMember(data, "keys", out value);
			model.keys = value;
			WrapMode value2 = model.preWrapMode;
			success += DeserializeMember(data, "preWrapMode", out value2);
			model.preWrapMode = value2;
			WrapMode value3 = model.postWrapMode;
			success += DeserializeMember(data, "postWrapMode", out value3);
			model.postWrapMode = value3;
			return success;
		}

		public override object CreateInstance(fsData data, Type storageType)
		{
			return new AnimationCurve();
		}
	}
}
