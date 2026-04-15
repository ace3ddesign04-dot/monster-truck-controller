using UnityEngine;

public class Key
{
	public string Name = string.Empty;

	public KeyType Type;

	public long LastRefresh;

	public long Version;

	public bool Uploaded;

	public long Long;

	public int Int;

	public string String = string.Empty;

	public float Float;

	public bool Bool;

	public bool Existed;

	public static Key MakeKey(string name, KeyType type)
	{
		Key key = new Key();
		key.Name = name;
		key.Type = type;
		return key;
	}

	public static Key MakeKey(string name, long lastRefresh, bool uploaded, KeyType type)
	{
		Key key = new Key();
		key.Name = name;
		key.Type = type;
		key.LastRefresh = lastRefresh;
		key.Uploaded = uploaded;
		return key;
	}

	public void Populate()
	{
		Key key = ValueFromPlayerPrefs(this);
		Int = key.Int;
		Float = key.Float;
		Bool = key.Bool;
		Long = key.Long;
		String = key.String;
	}

	public string ValueAsString()
	{
		string result = string.Empty;
		switch (Type)
		{
		case KeyType.String:
			result = String;
			break;
		case KeyType.Int:
			result = Int.ToString();
			break;
		case KeyType.Bool:
			result = Bool.ToString();
			break;
		case KeyType.Float:
			result = Float.ToString();
			break;
		case KeyType.Long:
			result = Long.ToString();
			break;
		}
		return result;
	}

	public static Key ValueFromPlayerPrefs(Key key)
	{
		switch (key.Type)
		{
		case KeyType.Float:
			key.Float = PlayerPrefs.GetFloat(key.Name, -100000f);
			if (key.Float == -100000f)
			{
				key.Float = 0f;
				key.Existed = false;
			}
			break;
		case KeyType.Int:
			key.Float = PlayerPrefs.GetInt(key.Name, -99999);
			if ((float)key.Int == -100000f)
			{
				key.Int = 0;
				key.Existed = false;
			}
			break;
		case KeyType.Long:
		{
			string @string = PlayerPrefs.GetString(key.Name, "DIDNOTEXIST");
			if (@string == "DIDNOTEXIST")
			{
				key.Existed = false;
			}
			else
			{
				key.Float = float.Parse(@string);
			}
			break;
		}
		case KeyType.String:
			key.String = PlayerPrefs.GetString(key.Name, "DIDNOTEXIST");
			if (key.String == "DIDNOTEXIST")
			{
				key.Existed = false;
				key.String = null;
			}
			break;
		case KeyType.Bool:
		{
			int @int = PlayerPrefs.GetInt(key.Name, -99999);
			if (@int == -99999)
			{
				key.Existed = false;
			}
			else
			{
				key.Bool = ((@int == 1) ? true : false);
			}
			break;
		}
		}
		return key;
	}
}
