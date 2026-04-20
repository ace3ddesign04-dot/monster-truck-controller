using System;
using System.Collections.Generic;

[Serializable]
public class VehicleData
{
	public string VehicleName;

	public string CarControllerXMLData;

	public string SuspensionControllerXMLData;

	public string BodyPartsSwitcherXMLData;

	public List<string> PurchasedPartsList;

	public string SavedID;

	public bool equippedTrailer;
}
