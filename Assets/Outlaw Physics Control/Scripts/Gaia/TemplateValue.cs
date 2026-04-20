namespace Gaia
{
	public class TemplateValue
	{
		public string Value;

		public int[] Indicies;

		public readonly TemplateFrameVariable FrameVar;

		public TemplateValue(string value, TemplateFrameVariable fv)
		{
			Value = value;
			FrameVar = fv;
		}
	}
}
