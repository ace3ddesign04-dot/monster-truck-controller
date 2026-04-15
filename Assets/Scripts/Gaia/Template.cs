using System.Collections.Generic;
using System.Text;

namespace Gaia
{
	public class Template
	{
		public string FilePath;

		public TemplateFrame Frame;

		private Dictionary<string, TemplateValue> Variables;

		public Template(string filePath, bool debug)
		{
			FilePath = filePath;
			if (Frame == null && !findPreviouslyBuiltFrame())
			{
				Frame = new TemplateFrame(filePath, debug);
			}
			Variables = new Dictionary<string, TemplateValue>();
		}

		private bool findPreviouslyBuiltFrame()
		{
			foreach (TemplateFrame item in TemplateFrames.List)
			{
				if (item.FilePath == FilePath)
				{
					Frame = item;
					return true;
				}
			}
			return false;
		}

		public void Set(string name, string value)
		{
			if (value == null)
			{
				value = string.Empty;
			}
			if (Variables.ContainsKey(name))
			{
				Variables[name].Value = value;
			}
			else
			{
				Variables.Add(name, new TemplateValue(value, Frame.Variables[name]));
			}
		}

		private int[] CopyIndicies(TemplateFrameVariable tfv)
		{
			int[] array = new int[tfv.Indicies.Count];
			tfv.Indicies.CopyTo(array);
			return array;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(Frame.Text);
			foreach (TemplateValue value in Variables.Values)
			{
				value.Indicies = CopyIndicies(value.FrameVar);
			}
			foreach (TemplateValue value2 in Variables.Values)
			{
				for (int i = 0; i < value2.Indicies.Length; i++)
				{
					stringBuilder.Insert(value2.Indicies[i], value2.Value);
					UpdateIndicies(value2.FrameVar.Positions[i], value2.Value.Length);
				}
			}
			return stringBuilder.ToString();
		}

		private void UpdateIndicies(int position, int length)
		{
			foreach (TemplateValue value in Variables.Values)
			{
				for (int i = 0; i < value.FrameVar.Positions.Count; i++)
				{
					if (value.FrameVar.Positions[i] > position)
					{
						value.Indicies[i] = value.Indicies[i] + length;
					}
				}
			}
		}
	}
}
