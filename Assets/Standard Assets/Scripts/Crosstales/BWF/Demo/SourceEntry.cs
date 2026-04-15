using Crosstales.BWF.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.BWF.Demo
{
	[HelpURL("https://www.crosstales.com/media/data/assets/badwordfilter/api/class_crosstales_1_1_b_w_f_1_1_demo_1_1_source_entry.html")]
	public class SourceEntry : MonoBehaviour
	{
		public Text Text;

		public Image Icon;

		public Image Main;

		public Source Source;

		public GUIMain GuiMain;

		public Color32 EnabledColor = new Color32(0, byte.MaxValue, 0, 192);

		private Color32 disabledColor;

		public void Start()
		{
			disabledColor = Main.color;
		}

		public void Update()
		{
			Text.text = Source.Name;
			Icon.sprite = Source.Icon;
			if (GuiMain.Sources.Contains(Source.Name))
			{
				Main.color = EnabledColor;
			}
			else
			{
				Main.color = disabledColor;
			}
		}

		public void Click()
		{
			if (GuiMain.Sources.Contains(Source.Name))
			{
				GuiMain.Sources.Remove(Source.Name);
			}
			else
			{
				GuiMain.Sources.Add(Source.Name);
			}
		}
	}
}
