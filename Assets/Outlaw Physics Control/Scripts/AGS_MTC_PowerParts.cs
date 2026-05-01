using AGS_MonsterTruckControl;
using System.Collections.Generic;

public static class AGS_MTC_PowerParts
{
	public static List<AGS_MTC_PowerPart> Parts;

	static AGS_MTC_PowerParts()
	{
		Parts = new List<AGS_MTC_PowerPart>();
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.EngineBlock, 0, 0, 0f, "Stock engine"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.EngineBlock, 1, 1000, 5f, "Bored engine block (more cubic inches!).\r\n +5% power!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.EngineBlock, 2, 2000, 10f, "Bored and stroked block.\r\n +10% power!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.EngineBlock, 3, 3000, 15f, "Big block swap.\r\n +15% power!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.EngineBlock, 4, 5000, 20f, "Blueprinted big block with all forged components.\r\n +20% power!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Head, 0, 0, 0f, "Stock heads"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Head, 1, 1000, 5f, "Hand ported steal heads.\r\n +5% power!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Head, 2, 1200, 10f, "CNC ported steal heads.\r\n +10% power!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Head, 3, 1500, 15f, "Large port aluminum heads.\r\n +15% power!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Head, 4, 2000, 20f, "Flow bench dyno and tweaked aluminum heads.\r\n +20% power!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Grip, 0, 0, 0f, "Stock tires"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Grip, 1, 1500, 5f, "Sticky tires.\r\n+5% Grip!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Grip, 2, 1500, 10f, "Tuned suspension geometry.\r\n+10% Grip!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Grip, 3, 2000, 15f, "Tuned vehicle COG.\r\n+15% Grip!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Grip, 4, 2000, 20f, "Tuned chassis stiffness.\r\n+20% Grip!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Valvetrain, 0, 0, 0f, "Stock valvetrain"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Valvetrain, 1, 1000, 5f, "Clean-up of intake and exhaust ports.\r\n+5% power!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Valvetrain, 2, 1000, 10f, "Polished rotating assembly.\r\n+10% power!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Valvetrain, 3, 1600, 15f, "Lightweight pistons.\r\n+15% power!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Valvetrain, 4, 1800, 20f, "Lightweight crankshaft.\r\n+20% power!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Weight, 0, 0, 0f, "Stock chassis"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Weight, 1, 1000, 5f, "Lightweight seats.\r\n-5% weight!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Weight, 2, 3000, 10f, "Lightweight interior components.\r\n-10% weight!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Weight, 3, 5000, 15f, "Lightweight drivetrain components.\r\n-15% weight!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Weight, 4, 8000, 20f, "Titanium chassis components.\r\n-20% weight!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Durability, 0, 0, 0f, "Stock fuel tank"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Durability, 1, 1000, 5f, "TITAN fuel tank and frame supports.\r\n+5% Durability!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Durability, 2, 1200, 10f, "TITAN fuel tank and upgraded differentials.\r\n+10% Durability!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Durability, 3, 1400, 15f, "TITAN fuel tank and upgraded cooling system.\r\n+15% Durability!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Durability, 4, 1600, 20f, "TITAN fuel tank and upgraded driveshafts.\r\n+20% Durability!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Truck, AGS_MTC_PowerPartType.Diesel, 3, 10000, 20f, "Swap to a diesel and ROLL COAL!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Truck, AGS_MTC_PowerPartType.Diesel, 4, 10000, 20f, "Swap to a diesel and ROLL COAL!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Gearbox, 0, 0, 0f, "Get manual transmission and control torque like a PRO!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Gearbox, 1, 10000, 0f, "Get manual transmission and control torque like a PRO!"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Ebrake, 0, 0, 0f, "E-brake not installed"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Ebrake, 1, 10000, 0f, "E-brake installed"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.TankTracks, 0, 0, 0f, "Tank tracks"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.TankTracks, 1, 10000, 0f, "Tank tracks"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Gearing, 0, 0, 0f, string.Empty));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Gearing, 1, 5000, 0f, string.Empty));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Gearing, 2, 5000, 0f, string.Empty));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Gearing, 3, 5000, 0f, string.Empty));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Gearing, 4, 5000, 0f, string.Empty));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Turbo, 0, 0, 0f, "No turbo"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Turbo, 1, 1000, 15f, "Turbo stage 1"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Turbo, 2, 1000, 17f, "Turbo stage 2"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Turbo, 3, 1600, 19f, "Turbo stage 3"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Turbo, 4, 1800, 21f, "Turbo stage 4"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Blower, 0, 0, 0f, "No blower"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Blower, 1, 1000, 15f, "Blower stage 1"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Blower, 2, 1000, 17f, "Blower stage 2"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Blower, 3, 1600, 19f, "Blower stage 3"));
		Parts.Add(new AGS_MTC_PowerPart(AGS_MTC_VehicleType.Any, AGS_MTC_PowerPartType.Blower, 4, 1800, 21f, "Blower stage 4"));
	}

	public static AGS_MTC_PowerPart GetPart(AGS_MTC_VehicleType _vehicleType, AGS_MTC_PowerPartType _partType, int _stage)
	{
        AGS_MTC_PowerPart powerPart = Parts.Find((AGS_MTC_PowerPart p) => p.partType == _partType && p.vehicleType == _vehicleType && p.Stage == _stage);
		if (powerPart == null)
		{
			powerPart = Parts.Find((AGS_MTC_PowerPart p) => p.partType == _partType && p.vehicleType == AGS_MTC_VehicleType.Any && p.Stage == _stage);
		}
		return powerPart;
	}
}
