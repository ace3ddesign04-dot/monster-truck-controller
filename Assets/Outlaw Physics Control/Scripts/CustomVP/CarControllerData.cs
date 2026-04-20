using System;

namespace CustomVP
{
	[Serializable]
	public class CarControllerData
	{
		public int EngineBlockStage;

		public int HeadStage;

		public int ValvetrainStage;

		public int GripStage;

		public int WeightStage;

		public int DurabilityStage;

		public int TurboStage;

		public int BlowerStage;

		public int DieselStage;

		public int TransmissionType;

		public bool ManualTransmissionPurchased;

		public bool DieselPurchased;

		public int PurchasedTurboStage;

		public int PurchasedBlowerStage;

		public bool TankTracksPurchased;

		public bool TuningEnginePurchased;

		public bool PerfectSetupPurchased;

		public int GearingStage;

		public int Ebrake;

		public float[] GearRatios;

		public float LowGearRatio;

		public float FuelRatio;

		public float TimingRatio;

		public float PerfectFuelRatio;

		public float PerfectTimingRatio;

		public float CarHealth;
	}
}
