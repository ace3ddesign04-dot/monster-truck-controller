using CustomVP;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utility
{
	public static int CashToGoldRatio = 200;

	public static int MemberBonusMultiplier = 3;

	public static string RandomDigits(int length)
	{
		string text = string.Empty;
		for (int i = 0; i < length; i++)
		{
			text += UnityEngine.Random.Range(0, 10);
		}
		return text;
	}

	public static List<T> FindObjectsOfTypeAll<T>()
	{
		List<T> list = new List<T>();
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			Scene sceneAt = SceneManager.GetSceneAt(i);
			if (sceneAt.isLoaded)
			{
				GameObject[] rootGameObjects = sceneAt.GetRootGameObjects();
				foreach (GameObject gameObject in rootGameObjects)
				{
					list.AddRange(gameObject.GetComponentsInChildren<T>(includeInactive: true));
				}
			}
		}
		return list;
	}

	public static string GenerateName()
	{
		string[] array = new string[15]
		{
			"Trail",
			"Winch",
			"Diesel",
			"Mud",
			"Tread",
			"Rut",
			"Dirt",
			"Bog",
			"Gear",
			"Fast",
			"Quick",
			"Rock",
			"Stone",
			"Water",
			"Sand"
		};
		string[] array2 = new string[9]
		{
			"Dog",
			"Master",
			"Hawk",
			"Killer",
			"Man",
			"Boss",
			"Rig",
			"Smoke",
			"Dust"
		};
		string str = array[UnityEngine.Random.Range(0, array.Length)];
		string str2 = array2[UnityEngine.Random.Range(0, array2.Length)];
		string str3 = UnityEngine.Random.Range(0, 500).ToString();
		return str + str2 + str3;
	}

	public static bool HashMatch(string check, string hash)
	{
		return MD5(check) == hash;
	}

	public static string MD5(string data)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		byte[] bytes = uTF8Encoding.GetBytes(data);
		MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
		byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes);
		string text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			text += Convert.ToString(array[i], 16).PadLeft(2, '0');
		}
		return text.PadLeft(32, '0');
	}

	public static int CashToGold(int cashAmount)
	{
		int a = (int)Mathf.Ceil((float)cashAmount / (float)CashToGoldRatio);
		return Mathf.Max(a, 1);
	}

	public static void AlignVehicleByGround(Transform vehicle)
	{
		WheelComponent[] componentsInChildren = vehicle.GetComponentsInChildren<WheelComponent>();
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return;
		}
		Vector3 position = vehicle.position;
		RaycastHit[] array = Physics.RaycastAll(position + Vector3.up * 2f, Vector3.down, 50f);
		List<Vector3> list = new List<Vector3>();
		RaycastHit[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			RaycastHit raycastHit = array2[i];
		}
		if (list.Count == 0)
		{
			return;
		}
		Vector3 vector = list[0];
		if (list.Count > 1)
		{
			for (int j = 1; j < list.Count; j++)
			{
				Vector3 vector2 = list[j];
				if (vector2.y > vector.y)
				{
					vector = list[j];
				}
			}
		}
		Vector3 a = vector;
		float num = 0f;
		int num2 = 0;
		WheelComponent[] componentsInChildren2 = vehicle.GetComponentsInChildren<WheelComponent>();
		foreach (WheelComponent wheelComponent in componentsInChildren2)
		{
			num += wheelComponent.wheelRadius;
			num2++;
		}
		num /= (float)num2;
		float num3 = 0f;
		WheelComponent[] componentsInChildren3 = vehicle.GetComponentsInChildren<WheelComponent>();
		foreach (WheelComponent wheelComponent2 in componentsInChildren3)
		{
			num3 += wheelComponent2.suspensionLength;
		}
		num3 /= (float)num2;
		Vector3 a2 = Vector3.zero;
		WheelComponent[] componentsInChildren4 = vehicle.GetComponentsInChildren<WheelComponent>();
		foreach (WheelComponent wheelComponent3 in componentsInChildren4)
		{
			a2 += wheelComponent3.transform.position;
		}
		a2 /= num2;
		Vector3 vector3 = vehicle.InverseTransformPoint(a2);
		float num4 = 0f - vector3.y;
		Vector3 localScale = vehicle.localScale;
		float num5 = num4 * localScale.y;
		a = (vehicle.position = a + Vector3.up * (num + num3 + num5));
	}

}
