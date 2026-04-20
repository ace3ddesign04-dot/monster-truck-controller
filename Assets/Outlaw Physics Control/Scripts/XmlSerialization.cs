using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class XmlSerialization : MonoBehaviour
{
	public static object DeserializeData<T>(string xmlString)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		MemoryStream stream = new MemoryStream(StringToUTF8(xmlString));
		object result = null;
		try
		{
			result = xmlSerializer.Deserialize(stream);
			return result;
		}
		catch
		{
			UnityEngine.Debug.LogError("Error while deserialization");
			return result;
		}
	}

	public static string SerializeData<T>(object Object)
	{
		string text = null;
		MemoryStream stream = new MemoryStream();
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
		xmlSerializer.Serialize(xmlTextWriter, Object);
		stream = (MemoryStream)xmlTextWriter.BaseStream;
		return UTF8ToString(stream.ToArray());
	}

	public static string UTF8ToString(byte[] characters)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		return uTF8Encoding.GetString(characters);
	}

	public static byte[] StringToUTF8(string pXmlString)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		return uTF8Encoding.GetBytes(pXmlString);
	}
}
