using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SimpleZip
{
	public class Example : MonoBehaviour
	{
		public Text Text;

		public void Start()
		{
			try
			{
				string text = "El perro de San Roque no tiene rabo porque Ramón Rodríguez se lo ha robado.";
				text = text + text + text + text + text;
				string text2 = Zip.CompressToString(text);
				string arg = Zip.Decompress(text2);
				Text.text = $"Plain text: {text}\n\nCompressed: {text2}\n\nDecompressed: {arg}";
			}
			catch (Exception ex)
			{
				Text.text = ex.ToString();
			}
		}
	}
}
