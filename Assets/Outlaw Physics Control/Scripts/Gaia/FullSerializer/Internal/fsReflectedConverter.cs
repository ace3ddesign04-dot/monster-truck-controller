using System;
using System.Collections;

namespace Gaia.FullSerializer.Internal
{
	public class fsReflectedConverter : fsConverter
	{
		public override bool CanProcess(Type type)
		{
			if (type.Resolve().IsArray || typeof(ICollection).IsAssignableFrom(type))
			{
				return false;
			}
			return true;
		}

		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			serialized = fsData.CreateDictionary();
			fsResult success = fsResult.Success;
			fsMetaType fsMetaType = fsMetaType.Get(instance.GetType());
			fsMetaType.EmitAotData();
			for (int i = 0; i < fsMetaType.Properties.Length; i++)
			{
				fsMetaProperty fsMetaProperty = fsMetaType.Properties[i];
				if (fsMetaProperty.CanRead)
				{
					fsData data;
					fsResult result = Serializer.TrySerialize(fsMetaProperty.StorageType, fsMetaProperty.Read(instance), out data);
					success.AddMessages(result);
					if (!result.Failed)
					{
						serialized.AsDictionary[fsMetaProperty.JsonName] = data;
					}
				}
			}
			return success;
		}

		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			fsResult fsResult;



            fsResult success = fsResult.Success;
			fsResult = success += CheckType(data, fsDataType.Object);
			if (fsResult.Failed)
			{
				return success;
			}
			fsMetaType fsMetaType = fsMetaType.Get(storageType);
			fsMetaType.EmitAotData();
			for (int i = 0; i < fsMetaType.Properties.Length; i++)
			{
				fsMetaProperty fsMetaProperty = fsMetaType.Properties[i];
				if (fsMetaProperty.CanWrite && data.AsDictionary.TryGetValue(fsMetaProperty.JsonName, out fsData value))
				{
					object result = null;
					if (fsMetaProperty.CanRead)
					{
						result = fsMetaProperty.Read(instance);
					}
					fsResult result2 = Serializer.TryDeserialize(value, fsMetaProperty.StorageType, ref result);
					success.AddMessages(result2);
					if (!result2.Failed)
					{
						fsMetaProperty.Write(instance, result);
					}
				}
			}
			return success;
		}

		public override object CreateInstance(fsData data, Type storageType)
		{
			fsMetaType fsMetaType = fsMetaType.Get(storageType);
			return fsMetaType.CreateInstance();
		}
	}
}
