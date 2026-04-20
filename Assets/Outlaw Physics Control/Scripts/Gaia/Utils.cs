using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Gaia
{
	public class Utils : MonoBehaviour
	{
		public static string GetGaiaAssetDirectory()
		{
			string text = Path.Combine(Application.dataPath, GaiaConstants.AssetDir);
			return text.Replace('\\', '/');
		}

		public static string GetGaiaAssetDirectory(GaiaConstants.FeatureType featureType)
		{
			string path = Path.Combine(Application.dataPath, GaiaConstants.AssetDir);
			path = Path.Combine(path, featureType.ToString());
			return path.Replace('\\', '/');
		}

		public static List<string> GetGaiaStampsList(GaiaConstants.FeatureType featureType)
		{
			return new List<string>(Directory.GetFiles(GetGaiaAssetDirectory(featureType), "*.jpg"));
		}

		public static string GetGaiaAssetPath(GaiaConstants.FeatureType featureType, string assetName)
		{
			string gaiaAssetDirectory = GetGaiaAssetDirectory(featureType);
			gaiaAssetDirectory = Path.Combine(GetGaiaAssetDirectory(featureType), assetName);
			return gaiaAssetDirectory.Replace('\\', '/');
		}

		public static string GetGaiaStampAssetPath(GaiaConstants.FeatureType featureType, string assetName)
		{
			string gaiaAssetDirectory = GetGaiaAssetDirectory(featureType);
			gaiaAssetDirectory = Path.Combine(GetGaiaAssetDirectory(featureType), "Data");
			gaiaAssetDirectory = Path.Combine(gaiaAssetDirectory, assetName);
			return gaiaAssetDirectory.Replace('\\', '/');
		}

		public static string GetGaiaStampPath(Texture2D source)
		{
			string empty = string.Empty;
			string fileName = Path.GetFileName(empty);
			empty = Path.Combine(Path.GetDirectoryName(empty), "Data");
			empty = Path.Combine(empty, fileName);
			empty = Path.ChangeExtension(empty, ".bytes");
			return empty.Replace('\\', '/');
		}

		public static bool CheckValidGaiaStampPath(Texture2D source)
		{
			string empty = string.Empty;
			if (Path.GetExtension(empty).ToLower() != ".jpg")
			{
				return false;
			}
			string fileName = Path.GetFileName(empty);
			empty = Path.Combine(Path.GetDirectoryName(empty), "Data");
			empty = Path.Combine(empty, fileName);
			empty = Path.ChangeExtension(empty, ".bytes");
			empty = empty.Replace('\\', '/');
			if (File.Exists(empty))
			{
				return true;
			}
			return false;
		}

		public static void CreateGaiaAssetDirectories()
		{
		}

		public static T[] GetAtPath<T>(string path)
		{
			ArrayList arrayList = new ArrayList();
			T[] array = new T[arrayList.Count];
			for (int i = 0; i < arrayList.Count; i++)
			{
				array[i] = (T)arrayList[i];
			}
			return array;
		}

		public static void MakeTextureNormal(Texture2D texture)
		{
			if (!(texture == null))
			{
			}
		}

		public static void MakeTextureReadable(Texture2D texture)
		{
			if (!(texture == null))
			{
			}
		}

		public static void CompressToSingleChannelFileImage(float[,] input, string imageName, TextureFormat imageStorageFormat = TextureFormat.RGBA32, bool exportPNG = true, bool exportJPG = true)
		{
			int length = input.GetLength(0);
			int length2 = input.GetLength(1);
			Texture2D texture2D = new Texture2D(length, length2, imageStorageFormat, mipChain: false);
			Color color = default(Color);
			color.a = 1f;
			color.r = (color.g = (color.b = 0f));
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					color.r = (color.b = (color.g = input[i, j]));
					texture2D.SetPixel(i, j, color);
				}
			}
			texture2D.Apply();
			if (exportJPG)
			{
				ExportJPG(imageName, texture2D);
			}
			if (exportPNG)
			{
				ExportPNG(imageName, texture2D);
			}
			UnityEngine.Object.DestroyImmediate(texture2D);
		}

		public static void CompressToMultiChannelFileImage(float[,,] input, string imageName, TextureFormat imageStorageFormat = TextureFormat.RGBA32, bool exportPNG = true, bool exportJPG = true)
		{
			int length = input.GetLength(0);
			int length2 = input.GetLength(1);
			int length3 = input.GetLength(2);
			int num = (length3 + 3) / 4;
			for (int i = 0; i < num; i++)
			{
				Texture2D texture2D = new Texture2D(length, length, imageStorageFormat, mipChain: false);
				Color color = default(Color);
				int num2 = i * 4;
				for (int j = 0; j < length; j++)
				{
					for (int k = 0; k < length2; k++)
					{
						color.r = ((num2 >= length3) ? 0f : input[j, k, num2]);
						color.g = ((num2 + 1 >= length3) ? 0f : input[j, k, num2 + 1]);
						color.b = ((num2 + 2 >= length3) ? 0f : input[j, k, num2 + 2]);
						color.a = ((num2 + 3 >= length3) ? 0f : input[j, k, num2 + 3]);
						texture2D.SetPixel(j, k, color);
					}
				}
				texture2D.Apply();
				if (exportJPG)
				{
					byte[] bytes = texture2D.EncodeToJPG();
					WriteAllBytes(imageName + i + ".jpg", bytes);
				}
				if (exportPNG)
				{
					byte[] bytes2 = texture2D.EncodeToPNG();
					WriteAllBytes(imageName + i + ".png", bytes2);
				}
				UnityEngine.Object.DestroyImmediate(texture2D);
			}
		}

		public static float[,] ConvertTextureToArray(Texture2D texture)
		{
			float[,] array = new float[texture.width, texture.height];
			for (int i = 0; i < texture.width; i++)
			{
				for (int j = 0; j < texture.height; j++)
				{
					array[i, j] = texture.GetPixel(i, j).grayscale;
				}
			}
			return array;
		}

		public static float[,] DecompressFromSingleChannelFileImage(string fileName, int width, int height, TextureFormat imageStorageFormat = TextureFormat.RGBA32, bool channelR = true, bool channelG = false, bool channelB = false, bool channelA = false)
		{
			float[,] array = null;
			if (File.Exists(fileName))
			{
				byte[] data = ReadAllBytes(fileName);
				Texture2D texture2D = new Texture2D(width, height, imageStorageFormat, mipChain: false);
				texture2D.LoadImage(data);
				array = new float[width, height];
				for (int i = 0; i < width; i++)
				{
					for (int j = 0; j < height; j++)
					{
						float[,] array2 = array;
						int num = i;
						int num2 = j;
						Color pixel = texture2D.GetPixel(i, j);
						array2[num, num2] = pixel.r;
					}
				}
				UnityEngine.Object.DestroyImmediate(texture2D);
			}
			else
			{
				UnityEngine.Debug.LogError("Unable to find " + fileName);
			}
			return array;
		}

		public static float[,] DecompressFromSingleChannelTexture(Texture2D importTexture, bool channelR = true, bool channelG = false, bool channelB = false, bool channelA = false)
		{
			if (importTexture == null || importTexture.width <= 0 || importTexture.height <= 0)
			{
				UnityEngine.Debug.LogError("Unable to import from texture");
				return null;
			}
			float[,] array = new float[importTexture.width, importTexture.height];
			if (channelR)
			{
				for (int i = 0; i < importTexture.width; i++)
				{
					for (int j = 0; j < importTexture.height; j++)
					{
						float[,] array2 = array;
						int num = i;
						int num2 = j;
						Color pixel = importTexture.GetPixel(i, j);
						array2[num, num2] = pixel.r;
					}
				}
			}
			else if (channelG)
			{
				for (int k = 0; k < importTexture.width; k++)
				{
					for (int l = 0; l < importTexture.height; l++)
					{
						float[,] array3 = array;
						int num3 = k;
						int num4 = l;
						Color pixel2 = importTexture.GetPixel(k, l);
						array3[num3, num4] = pixel2.g;
					}
				}
			}
			else if (channelB)
			{
				for (int m = 0; m < importTexture.width; m++)
				{
					for (int n = 0; n < importTexture.height; n++)
					{
						float[,] array4 = array;
						int num5 = m;
						int num6 = n;
						Color pixel3 = importTexture.GetPixel(m, n);
						array4[num5, num6] = pixel3.b;
					}
				}
			}
			if (channelA)
			{
				for (int num7 = 0; num7 < importTexture.width; num7++)
				{
					for (int num8 = 0; num8 < importTexture.height; num8++)
					{
						float[,] array5 = array;
						int num9 = num7;
						int num10 = num8;
						Color pixel4 = importTexture.GetPixel(num7, num8);
						array5[num9, num10] = pixel4.a;
					}
				}
			}
			return array;
		}

		public static void ExportJPG(string fileName, Texture2D texture)
		{
			byte[] bytes = texture.EncodeToJPG();
			WriteAllBytes(fileName + ".jpg", bytes);
		}

		public static void ExportPNG(string fileName, Texture2D texture)
		{
			byte[] bytes = texture.EncodeToPNG();
			WriteAllBytes(fileName + ".png", bytes);
		}

		public static float[,] LoadRawFile(string fileName)
		{
			if (!File.Exists(fileName))
			{
				UnityEngine.Debug.LogError("Could not locate heightmap file : " + fileName);
				return null;
			}
			float[,] array = null;
			using (FileStream fileStream = File.OpenRead(fileName))
			{
				using (BinaryReader binaryReader = new BinaryReader(fileStream))
				{
					int num = Mathf.CeilToInt(Mathf.Sqrt(fileStream.Length / 2));
					array = new float[num, num];
					for (int i = 0; i < num; i++)
					{
						for (int j = 0; j < num; j++)
						{
							array[i, j] = (float)(int)binaryReader.ReadUInt16() / 65535f;
						}
					}
				}
				fileStream.Close();
				return array;
			}
		}

		public static Mesh CreateMesh(float[,] heightmap, Vector3 targetSize)
		{
			int length = heightmap.GetLength(0);
			int length2 = heightmap.GetLength(1);
			int num = 1;
			Vector3 b = Vector3.zero - targetSize / 2f;
			Vector2 b2 = new Vector2(1f / (float)(length - 1), 1f / (float)(length2 - 1));
			for (num = 1; num < 100 && length / num * (length2 / num) >= 65000; num++)
			{
			}
			targetSize = new Vector3(targetSize.x / (float)(length - 1) * (float)num, targetSize.y, targetSize.z / (float)(length2 - 1) * (float)num);
			length = (length - 1) / num + 1;
			length2 = (length2 - 1) / num + 1;
			Vector3[] array = new Vector3[length * length2];
			Vector2[] array2 = new Vector2[length * length2];
			Vector3[] array3 = new Vector3[length * length2];
			Color[] array4 = new Color[length * length2];
			int[] array5 = new int[(length - 1) * (length2 - 1) * 6];
			for (int i = 0; i < length2; i++)
			{
				for (int j = 0; j < length; j++)
				{
					array4[i * length + j] = Color.black;
					array3[i * length + j] = Vector3.up;
					array[i * length + j] = Vector3.Scale(targetSize, new Vector3(j, heightmap[j * num, i * num], i)) + b;
					array2[i * length + j] = Vector2.Scale(new Vector2(j * num, i * num), b2);
				}
			}
			int num2 = 0;
			for (int k = 0; k < length2 - 1; k++)
			{
				for (int l = 0; l < length - 1; l++)
				{
					array5[num2++] = k * length + l;
					array5[num2++] = (k + 1) * length + l;
					array5[num2++] = k * length + l + 1;
					array5[num2++] = (k + 1) * length + l;
					array5[num2++] = (k + 1) * length + l + 1;
					array5[num2++] = k * length + l + 1;
				}
			}
			Mesh mesh = new Mesh();
			mesh.vertices = array;
			mesh.colors = array4;
			mesh.normals = array3;
			mesh.uv = array2;
			mesh.triangles = array5;
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			return mesh;
		}

		public static Bounds GetBounds(GameObject go)
		{
			Bounds result = new Bounds(go.transform.position, Vector3.zero);
			Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				result.Encapsulate(renderer.bounds);
			}
			Collider[] componentsInChildren2 = go.GetComponentsInChildren<Collider>();
			foreach (Collider collider in componentsInChildren2)
			{
				result.Encapsulate(collider.bounds);
			}
			return result;
		}

		private Vector3 Rotate90LeftXAxis(Vector3 input)
		{
			return new Vector3(input.x, 0f - input.z, input.y);
		}

		private Vector3 Rotate90RightXAxis(Vector3 input)
		{
			return new Vector3(input.x, input.z, 0f - input.y);
		}

		private Vector3 Rotate90LeftYAxis(Vector3 input)
		{
			return new Vector3(0f - input.z, input.y, input.x);
		}

		private Vector3 Rotate90RightYAxis(Vector3 input)
		{
			return new Vector3(input.z, input.y, 0f - input.x);
		}

		private Vector3 Rotate90LeftZAxis(Vector3 input)
		{
			return new Vector3(input.y, 0f - input.x, input.z);
		}

		private Vector3 Rotate90RightZAxis(Vector3 input)
		{
			return new Vector3(0f - input.y, input.x, input.z);
		}

		public static bool Math_ApproximatelyEqual(float a, float b, float threshold)
		{
			if (a == b || Mathf.Abs(a - b) < threshold)
			{
				return true;
			}
			return false;
		}

		public static bool Math_ApproximatelyEqual(float a, float b)
		{
			return Math_ApproximatelyEqual(a, b, float.Epsilon);
		}

		public static bool Math_IsPowerOf2(int value)
		{
			return (value & (value - 1)) == 0;
		}

		public static float Math_Clamp(float min, float max, float value)
		{
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		public static float Math_Modulo(float value, float mod)
		{
			return value - mod * (float)Math.Floor(value / mod);
		}

		public static int Math_Modulo(int value, int mod)
		{
			return (int)((float)value - (float)mod * (float)Math.Floor((float)value / (float)mod));
		}

		public static float Math_InterpolateLinear(float value1, float value2, float fraction)
		{
			return value1 * (1f - fraction) + value2 * fraction;
		}

		public static float Math_InterpolateSmooth(float value1, float value2, float fraction)
		{
			fraction = ((!(fraction < 0.5f)) ? (1f - 2f * (fraction - 1f) * (fraction - 1f)) : (2f * fraction * fraction));
			return value1 * (1f - fraction) + value2 * fraction;
		}

		public static float Math_Distance(float x1, float y1, float x2, float y2)
		{
			return Mathf.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
		}

		public static float Math_InterpolateSmooth2(float v1, float v2, float fraction)
		{
			float num = fraction * fraction;
			fraction = 3f * num - 2f * fraction * num;
			return v1 * (1f - fraction) + v2 * fraction;
		}

		public static float Math_InterpolateCubic(float v0, float v1, float v2, float v3, float fraction)
		{
			float num = v3 - v2 - (v0 - v1);
			float num2 = v0 - v1 - num;
			float num3 = v2 - v0;
			float num4 = fraction * fraction;
			return num * fraction * num4 + num2 * num4 + num3 * fraction + v1;
		}

		public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angle)
		{
			Vector3 point2 = point - pivot;
			point2 = Quaternion.Euler(angle) * point2;
			point = point2 + pivot;
			return point;
		}

		public static string FixFileName(string sourceFileName)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in sourceFileName)
			{
				if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_')
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		public static FileStream OpenRead(string path)
		{
			return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		public static string ReadAllText(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Argument_EmptyPath");
			}
			using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, 1024))
			{
				return streamReader.ReadToEnd();
			}
		}

		public static void WriteAllText(string path, string contents)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Argument_EmptyPath");
			}
			if (path == null)
			{
				throw new ArgumentNullException("contents");
			}
			if (contents.Length == 0)
			{
				throw new ArgumentException("Argument_EmptyContents");
			}
			using (StreamWriter streamWriter = new StreamWriter(path, append: false, Encoding.UTF8, 1024))
			{
				streamWriter.Write(contents);
			}
		}

		public static byte[] ReadAllBytes(string path)
		{
			using (FileStream fileStream = OpenRead(path))
			{
				long length = fileStream.Length;
				if (length > int.MaxValue)
				{
					throw new IOException("Reading more than 2GB with this call is not supported");
				}
				int num = 0;
				int num2 = (int)length;
				byte[] array = new byte[length];
				while (num2 > 0)
				{
					int num3 = fileStream.Read(array, num, num2);
					if (num3 == 0)
					{
						throw new IOException("Unexpected end of stream");
					}
					num += num3;
					num2 -= num3;
				}
				return array;
			}
		}

		public static void WriteAllBytes(string path, byte[] bytes)
		{
			using (Stream stream = File.Create(path))
			{
				stream.Write(bytes, 0, bytes.Length);
			}
		}

		public static void CreateAsset<T>() where T : ScriptableObject
		{
		}

		public static string GetAssetPath(UnityEngine.Object uo)
		{
			return string.Empty;
		}

		public static string WrapScriptableObject(ScriptableObject so)
		{
			return string.Empty;
		}

		public static void UnwrapScriptableObject(string path, string newpath)
		{
		}

		public static string WrapGameObjectAsPrefab(GameObject go)
		{
			return string.Empty;
		}

		public static bool IsInLayerMask(GameObject obj, LayerMask mask)
		{
			return (mask.value & (1 << obj.layer)) > 0;
		}

		public static bool IsSameTexture(Texture2D tex1, Texture2D tex2, bool checkID = false)
		{
			if (tex1 == null || tex2 == null)
			{
				return false;
			}
			if (checkID)
			{
				if (tex1.GetInstanceID() != tex2.GetInstanceID())
				{
					return false;
				}
				return true;
			}
			if (tex1.name != tex2.name)
			{
				return false;
			}
			if (tex1.width != tex2.width)
			{
				return false;
			}
			if (tex1.height != tex2.height)
			{
				return false;
			}
			return true;
		}

		public static bool IsSameGameObject(GameObject go1, GameObject go2, bool checkID = false)
		{
			if (go1 == null || go2 == null)
			{
				return false;
			}
			if (checkID)
			{
				if (go1.GetInstanceID() != go2.GetInstanceID())
				{
					return false;
				}
				return true;
			}
			if (go1.name != go2.name)
			{
				return false;
			}
			return true;
		}

		public static string GetAssetPath(string fileName)
		{
			return string.Empty;
		}

		public static string GetAssetPath(string name, string type)
		{
			return string.Empty;
		}

		public static GaiaSettings GetGaiaSettings()
		{
			return GetAsset("GaiaSettings.asset", typeof(GaiaSettings)) as GaiaSettings;
		}

		public static UnityEngine.Object GetAsset(string fileNameOrPath, Type assetType)
		{
			return null;
		}

		public static GameObject GetAssetPrefab(string name)
		{
			return null;
		}

		public static ScriptableObject GetAssetScriptableObject(string name)
		{
			return null;
		}

		public static Texture2D GetAssetTexture2D(string name)
		{
			return null;
		}

		public static Type GetType(string TypeName)
		{
			Type type = Type.GetType(TypeName);
			if (type != null)
			{
				return type;
			}
			if (TypeName.Contains("."))
			{
				string assemblyString = TypeName.Substring(0, TypeName.IndexOf('.'));
				try
				{
					Assembly assembly = Assembly.Load(assemblyString);
					if (assembly == null)
					{
						return null;
					}
					type = assembly.GetType(TypeName);
					if (type != null)
					{
						return type;
					}
				}
				catch (Exception)
				{
				}
			}
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			if (callingAssembly != null)
			{
				type = callingAssembly.GetType(TypeName);
				if (type != null)
				{
					return type;
				}
			}
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.GetLength(0); i++)
			{
				type = assemblies[i].GetType(TypeName);
				if (type != null)
				{
					return type;
				}
			}
			AssemblyName[] referencedAssemblies = callingAssembly.GetReferencedAssemblies();
			AssemblyName[] array = referencedAssemblies;
			foreach (AssemblyName assemblyRef in array)
			{
				Assembly assembly2 = Assembly.Load(assemblyRef);
				if (assembly2 != null)
				{
					type = assembly2.GetType(TypeName);
					if (type != null)
					{
						return type;
					}
				}
			}
			return null;
		}
	}
}
