using System;
using System.Reflection;

namespace Gaia.FullSerializer.Internal
{
	public class fsMetaProperty
	{
		private MemberInfo _memberInfo;

		public Type StorageType
		{
			get;
			private set;
		}

		public bool CanRead
		{
			get;
			private set;
		}

		public bool CanWrite
		{
			get;
			private set;
		}

		public string JsonName
		{
			get;
			private set;
		}

		public string MemberName
		{
			get;
			private set;
		}

		public bool IsPublic
		{
			get;
			private set;
		}

		internal fsMetaProperty(FieldInfo field)
		{
			_memberInfo = field;
			StorageType = field.FieldType;
			JsonName = GetJsonName(field);
			MemberName = field.Name;
			IsPublic = field.IsPublic;
			CanRead = true;
			CanWrite = true;
		}

		internal fsMetaProperty(PropertyInfo property)
		{
			_memberInfo = property;
			StorageType = property.PropertyType;
			JsonName = GetJsonName(property);
			MemberName = property.Name;
			IsPublic = (property.GetGetMethod() != null && property.GetGetMethod().IsPublic && property.GetSetMethod() != null && property.GetSetMethod().IsPublic);
			CanRead = property.CanRead;
			CanWrite = property.CanWrite;
		}

		public void Write(object context, object value)
		{
			FieldInfo fieldInfo = _memberInfo as FieldInfo;
			PropertyInfo propertyInfo = _memberInfo as PropertyInfo;
			if (fieldInfo != null)
			{
				fieldInfo.SetValue(context, value);
			}
			else
			{
				propertyInfo?.GetSetMethod(nonPublic: true)?.Invoke(context, new object[1]
				{
					value
				});
			}
		}

		public object Read(object context)
		{
			if (_memberInfo is PropertyInfo)
			{
				return ((PropertyInfo)_memberInfo).GetValue(context, new object[0]);
			}
			return ((FieldInfo)_memberInfo).GetValue(context);
		}

		private static string GetJsonName(MemberInfo member)
		{
			fsPropertyAttribute attribute = fsPortableReflection.GetAttribute<fsPropertyAttribute>(member);
			if (attribute != null && !string.IsNullOrEmpty(attribute.Name))
			{
				return attribute.Name;
			}
			return member.Name;
		}
	}
}
