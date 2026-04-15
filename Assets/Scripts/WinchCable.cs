using UnityEngine;

public class WinchCable
{
	public CableType cableType;

	public LineRenderer lineRenderer;

	public string CableID;

	public Transform t1;

	public Transform t2;

	public Transform Car;

	public Vector3 CarTargetPos;

	public bool IsCarMissing()
	{
		switch (cableType)
		{
		case CableType.CarToCar:
			return t1 == null || t2 == null;
		case CableType.CarToStatic:
			return Car == null;
		default:
			return false;
		}
	}

	public void UpdateCable()
	{
		if (cableType == CableType.CarToCar)
		{
			lineRenderer.SetPosition(0, t1.position);
			lineRenderer.SetPosition(1, t2.position);
		}
		if (cableType == CableType.CarToStatic)
		{
			lineRenderer.SetPosition(0, Car.position);
			lineRenderer.SetPosition(1, CarTargetPos);
		}
	}
}
