public class AGS_MTC_SuspensionControlLimit
{
	public string SuspensionName;

	public string ValueName;

	public float fMin;

	public float fMax;

	public float fDef;

	public int iMin;

	public int iMax;

	public int iDef;

	public bool ModifiableByPlayer;

	public AGS_MTC_SuspensionControlLimit(string suspensionName, string name, float fmin, float fmax, float fdef, int imin, int imax, int idef, bool modifiableByPlayer)
	{
		SuspensionName = suspensionName;
		ValueName = name;
		fMin = fmin;
		fMax = fmax;
		fDef = fdef;
		iMin = imin;
		iMax = imax;
		iDef = idef;
		ModifiableByPlayer = modifiableByPlayer;
	}
}
