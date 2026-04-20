using System.Collections.Generic;

namespace Gaia
{
	public class TemplateFrame
	{
		public Dictionary<string, TemplateFrameVariable> Variables = new Dictionary<string, TemplateFrameVariable>();

		public string FilePath;

		public string Text;

		private const char BeginChar = '{';

		private const char EndChar = '}';

		private int VariableCount;

		public TemplateFrame(string filePath, bool debug)
		{
			FilePath = filePath;
			Text = Build(filePath);
			if (!debug)
			{
				TemplateFrames.List.Add(this);
			}
		}

		public string Build(string filePath)
		{
			char[] array = Utils.ReadAllText(filePath).ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != '{' || array[i + 1] != '{')
				{
					continue;
				}
				int num = i;
				i += 2;
				string varName = getVarName(array, ref i);
				if (varName != null)
				{
					if (!Variables.ContainsKey(varName))
					{
						Variables.Add(varName, new TemplateFrameVariable(new List<int>(), new List<int>()));
					}
					Variables[varName].Indicies.Add(num);
					Variables[varName].Positions.Add(VariableCount++);
				}
				array = shiftCharArryLeft(array, num, i);
				i = num - 1;
			}
			return new string(array);
		}

		private char[] shiftCharArryLeft(char[] arry, int startIndex, int endIndex)
		{
			char[] array = new char[arry.Length - (endIndex - startIndex + 1)];
			for (int i = 0; i < startIndex; i++)
			{
				array[i] = arry[i];
			}
			int num = 0;
			for (int j = endIndex + 1; j < arry.Length; j++)
			{
				array[startIndex + num] = arry[j];
				num++;
			}
			return array;
		}

		private string getVarName(char[] text, ref int pos)
		{
			pos = skipSpaces(text, pos);
			int num = pos;
			while (text[pos] != ' ')
			{
				pos++;
			}
			int num2 = pos;
			char[] array = new char[num2 - num];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = text[num + i];
			}
			pos = skipSpaces(text, pos);
			if (text[pos] != '}' && text[++pos] != '}')
			{
				return null;
			}
			pos++;
			return new string(array);
		}

		private static int skipSpaces(char[] text, int pos)
		{
			while (text[pos] == ' ')
			{
				pos++;
			}
			return pos;
		}
	}
}
