using UnityEngine;

namespace CustomVP
{
	public class CarAIControl : MonoBehaviour
	{
		public enum BrakeCondition
		{
			NeverBrake,
			TargetDirectionDifference,
			TargetDistance
		}

		[SerializeField]
		[Range(0f, 1f)]
		private float m_CautiousSpeedFactor = 0.05f;

		[SerializeField]
		[Range(0f, 180f)]
		private float m_CautiousMaxAngle = 50f;

		[SerializeField]
		private float m_CautiousMaxDistance = 100f;

		[SerializeField]
		private float m_CautiousAngularVelocityFactor = 30f;

		[SerializeField]
		private float m_SteerSensitivity = 0.05f;

		[SerializeField]
		private float m_AccelSensitivity = 0.04f;

		[SerializeField]
		private float m_BrakeSensitivity = 1f;

		[SerializeField]
		private float m_LateralWanderDistance = 3f;

		[SerializeField]
		private float m_LateralWanderSpeed = 0.1f;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_AccelWanderAmount = 0.1f;

		[SerializeField]
		private float m_AccelWanderSpeed = 0.1f;

		[SerializeField]
		private BrakeCondition m_BrakeCondition = BrakeCondition.TargetDistance;

		[SerializeField]
		private bool m_Driving;

		[SerializeField]
		private Transform m_Target;

		[SerializeField]
		private bool m_StopWhenTargetReached;

		[SerializeField]
		private float m_ReachTargetThreshold = 2f;

		private float m_RandomPerlin;

		private CarController m_CarController;

		private float m_AvoidOtherCarTime;

		private float m_AvoidOtherCarSlowdown;

		private float m_AvoidPathOffset;

		private Rigidbody m_Rigidbody;

		private void Awake()
		{
			m_CarController = GetComponent<CarController>();
			m_RandomPerlin = UnityEngine.Random.value * 100f;
			m_Rigidbody = GetComponent<Rigidbody>();
		}

		private void Update()
		{
		}

		private void OnCollisionStay(Collision col)
		{
			if (!(col.rigidbody != null))
			{
				return;
			}
			CarAIControl component = col.rigidbody.GetComponent<CarAIControl>();
			if (component != null)
			{
				m_AvoidOtherCarTime = Time.time + 1f;
				if (Vector3.Angle(base.transform.forward, component.transform.position - base.transform.position) < 90f)
				{
					m_AvoidOtherCarSlowdown = 0.5f;
				}
				else
				{
					m_AvoidOtherCarSlowdown = 1f;
				}
				Vector3 vector = base.transform.InverseTransformPoint(component.transform.position);
				float f = Mathf.Atan2(vector.x, vector.z);
				m_AvoidPathOffset = m_LateralWanderDistance * (0f - Mathf.Sign(f));
			}
		}

		public void SetTarget(Transform target)
		{
			m_Target = target;
			m_Driving = true;
		}
	}
}
