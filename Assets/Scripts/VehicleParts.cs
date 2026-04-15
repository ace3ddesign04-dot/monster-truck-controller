using CustomVP;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class VehicleParts
{
	public static List<BodyPart> Parts;

	public static Color DryMudColor;

	public static Color WetMudColor;

	static VehicleParts()
	{
		Parts = new List<BodyPart>();
		DryMudColor = hexToColor("FDC572FF");
		WetMudColor = hexToColor("4F2E0CFF");
		Parts.Add(new BodyPart(VehicleType.Any, PartType.FrontBumper, "DEFAULT", 500));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.FrontBumper, "FrontBumperStock", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.FrontBumper, "NoBumper", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.FrontBumper, "FrontBumper1", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.FrontBumper, "FrontBumper2", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.FrontBumper, "FrontBumper3", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.FrontBumper, "FrontBumper4", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.FrontBumper, "FrontBumper5", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.RearBumper, "DEFAULT", 500));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.RearBumper, "RearBumperStock", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.RearBumper, "RearBumper1", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.RearBumper, "RearBumper2", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.RearBumper, "RearBumper3", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Cage, "DEFAULT", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Cage, "RoofCageShort", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Cage, "RoofCageLong", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Cage, "BedCage", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Cage, "ExoCage_Front", 3000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Cage, "ExoCage_Rear", 3000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Cage, "ExoCage_Front+Rear", 5000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Cage, "TentCage", 1500));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Cage, "TentCage_Covered", 1750));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Horn, "DEFAULT", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Horn, "DevilHorns", 500));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Horn, "BullHorns", 500));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.RoofLight, "DEFAULT", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.RoofLight, "RoofLights1", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.RoofLight, "RoofLights2", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.RoofLight, "LedLightsLong", 3000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.RoofLight, "PoliceFlasher", 5000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.RunningBoard, "DEFAULT", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.RunningBoard, "RunningBoardStock", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.RunningBoard, "RunningBoard1_Single", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.RunningBoard, "RunningBoard1_Double", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.RunningBoard, "RunningBoard2", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.RunningBoard, "RunningBoard3", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Snorkel, "DEFAULT", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Snorkel, "SnorkelSport", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Snorkel, "SnorkelMilitary", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Fender, "DEFAULT", 5000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Fender, "FendersStock", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Fender, "FendersThickLip", 5000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Fender, "FendersMediumLip", 5000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Fender, "FendersFatLip", 5000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Fender, "FendersCutOff", 5000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Fender, "FendersFlare", 5000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.SpareWheelHolder, "DEFAULT", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "DEFAULT", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Winch", 750));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "WindshieldProtection", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "ProtectionPlates", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "ExhaustPipe", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "ExhaustPipeHood", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BedExhaustPipe", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "ExhaustPipeHoodDouble", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Jack", 500));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "FrontLights", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "FrontLight", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "DEFAULT", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "DEFAULT", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "Rim0", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "Rim1", 4000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "Rim2", 4000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "Rim3", 4000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "Rim4", 4000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "Rim5", 4000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "Rim6", 4000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "Rim7", 4000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "Rim8", 4000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "DEFAULT", 2500));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "Tire0", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "Tire1", 2500));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "Tire2", 2500));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "Tire3", 2500));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "Tire4", 2500));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "Tire5", 2500));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "Tire6", 2500));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "Tire7", 2500));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Wheel, "Tire8", 2500));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "ProtectionPlates_Level1", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "ProtectionPlates_Level2", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "ProtectionPlates_Level3", 3000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Headlights, "HeadlightsYellow", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Headlights, "HeadlightsWhite", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Headlights, "HeadlightsBlue", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Headlights, "HeadlightsGreen", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Headlights, "HeadlightsMagenta", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Frame0", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Frame1", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Frame2", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Frame3", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Frame4", 1000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Grill0", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Grill1", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Hood0", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Hood1", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Hood2", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Hood3", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BedType0", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BedType1", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BedType2", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BedType3", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BedType4", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BedType5", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BedType6", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BedType7", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BedType8", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BedType9", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BedType10", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Spoiler0", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Spoiler1", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Spoiler2", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.GunRack, "GunRack", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BodyPartPaint", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "RimPaint", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BeadlockPaint", 500));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "RepairPack", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "MudGuard0", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "MudGuard1", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "MudGuard2", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Light", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Shield", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Skull", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "ExhaustPipe0", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "ExhaustPipe1", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "ExhaustPipe2", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "ExhaustPipe3", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "ExhaustPipe4", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BodyType0", 0));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BodyType1", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BodyType2", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BodyType3", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BodyType4", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BodyType5", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BodyType6", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "BodyType7", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "RoofWing", 2000));
		Parts.Add(new BodyPart(VehicleType.Any, PartType.Other, "Deflector", 2000));
	}

	public static BodyPart GetPart(VehicleType _vehicleType, PartType _partType, string _partName)
	{
		BodyPart bodyPart = Parts.Find((BodyPart p) => p.partType == _partType && p.vehicleType == _vehicleType && p.partName == _partName);
		if (bodyPart == null)
		{
			bodyPart = Parts.Find((BodyPart p) => p.partType == _partType && p.vehicleType == _vehicleType && p.partName == "DEFAULT");
		}
		if (bodyPart == null)
		{
			bodyPart = Parts.Find((BodyPart p) => p.partType == _partType && p.vehicleType == VehicleType.Any && p.partName == _partName);
		}
		if (bodyPart == null)
		{
			bodyPart = Parts.Find((BodyPart p) => p.partType == _partType && p.vehicleType == VehicleType.Any && p.partName == "DEFAULT");
		}
		return bodyPart;
	}

	public static Color hexToColor(string hex)
	{
		hex = hex.Replace("0x", string.Empty);
		hex = hex.Replace("#", string.Empty);
		byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
		return new Color32(r, g, b, byte.MaxValue);
	}
}
