using UnityEngine;

namespace AGS_MonsterTruckControl
{
	public class AGS_MTC_WheelComponent : MonoBehaviour
	{
		private Rigidbody rigidBody;

		public Transform VisualWheel;

		public float wheelRadius = 0.5f;

		public float wheelMass = 1f;

		public AGS_MTC_WheelSweepType sweepType;

		[Header("Springs")]
		public float travel = 0.5f;

		public float spring = 1000f;

		public float damper = 1500f;

		public AnimationCurve SpringCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

		[Header("Friction")]
		public float forwardFrictionCoefficient = 1f;

		public float sideFrictionCoefficient = 1f;

		public float surfaceFrictionCoefficient = 1f;

		[Header("Forward friction params")]
		public float f_extSlip;

		public float f_extVal;

		public float f_asSlip;

		public float f_asVal;

		public float f_tailVal;

		[Header("Sideways friction params")]
		public float s_extSlip;

		public float s_extVal;

		public float s_asSlip;

		public float s_asVal;

		public float s_tailVal;

		[HideInInspector]
		public float rpm;

		[HideInInspector]
		public float FakeRPM;

		[HideInInspector]
		public float sLong;

		[HideInInspector]
		public float sLat;

		[HideInInspector]
		public float Compression;

		[HideInInspector]
		public float lastCompression;

		[HideInInspector]
		public float deltaCompression;

		[HideInInspector]
		public float CommonSlip;

		//[HideInInspector]
		public bool IsGrounded;

		[HideInInspector]
		public AGS_MTC_WheelCollider wheelCollider;

		[HideInInspector]
		public float MotorTorque;

		[HideInInspector]
		public float Steer;

		[HideInInspector]
		public float BrakeTorque;

		private SphereCollider SpherecastProtector;

		private float m_speed;

		private float lastHitOffset;

		public float suspensionLength
		{
			get
			{
				return travel;
			}
			set
			{
				travel = value + 0.2f;
			}
		}

		public void Start()
		{
			rigidBody = GetComponentInParent<Rigidbody>();
			wheelCollider = base.gameObject.AddComponent<AGS_MTC_WheelCollider>();
			wheelCollider.rb = rigidBody;
			SpherecastProtector = new GameObject("SpherecastProtector").AddComponent<SphereCollider>();
			SpherecastProtector.transform.parent = base.gameObject.transform;
			SpherecastProtector.transform.localPosition = new Vector3(0f, 0f, 0f);
			SpherecastProtector.gameObject.layer = 26;
			PhysicMaterial physicMaterial = new PhysicMaterial("ZeroFriction");
			physicMaterial.bounciness = 0f;
			physicMaterial.dynamicFriction = 0f;
			physicMaterial.staticFriction = 0f;
			SpherecastProtector.material = physicMaterial;
			OnValidate();
		}

		public Vector3 GetVisualWheelPosition()
		{
			return base.transform.position - base.transform.up * (travel - wheelCollider.correctedSuspensionCompression);
		}

        public void FixedUpdate() {
            wheelCollider.motorTorque = MotorTorque;
            wheelCollider.steeringAngle = Steer;
            wheelCollider.brakeTorque = BrakeTorque;

            wheelCollider.updateWheel();

            Compression = wheelCollider.compressionDistance;

            if (wheelCollider.rb != null) {
                m_speed = wheelCollider.rb.velocity.magnitude;
            }

            rpm = (!(FakeRPM > 0f)) ? wheelCollider.rpm : FakeRPM;

            sLong = wheelCollider.longitudinalSlip;
            if (m_speed < 1f && Mathf.Abs(rpm) < 5f) {
                sLong = 0f;
            }

            sLat = wheelCollider.lateralSlip;
            if (m_speed < 1f && Mathf.Abs(rpm) < 5f) {
                sLat = 0f;
            }

            deltaCompression = Mathf.Abs(Compression - lastCompression);
            lastCompression = Compression;
            CommonSlip = sLat + sLong;
            IsGrounded = wheelCollider.isGrounded;
        }

        private void LateUpdate() {
            if (lastHitOffset != wheelCollider._hitOffsetSmooth) {
                SpherecastProtector.transform.localPosition = new Vector3(0f, wheelCollider._hitOffsetSmooth, 0f);
            }

            lastHitOffset = wheelCollider._hitOffsetSmooth;

            if (VisualWheel != null) {
                VisualWheel.Rotate(VisualWheel.right, wheelCollider.perFrameRotation, Space.World);
                VisualWheel.position = GetVisualWheelPosition();
            }
        }

        public void OnValidate()
		{
			if (wheelCollider != null)
			{
				wheelCollider.radius = wheelRadius;
				wheelCollider.mass = wheelMass;	
				wheelCollider.length = travel;
				wheelCollider.spring = spring;
				wheelCollider.damper = damper;
				wheelCollider.sweepType = sweepType;
				wheelCollider.springCurve = SpringCurve;
				UpdateFriction();
				SpherecastProtector.radius = wheelRadius * 0.9f;
			}
		}

		public void UpdateFriction()
		{
			wheelCollider.forwardFrictionCoefficient = forwardFrictionCoefficient;
			wheelCollider.sideFrictionCoefficient = sideFrictionCoefficient;
			wheelCollider.surfaceFrictionCoefficient = surfaceFrictionCoefficient;
			wheelCollider.forwardFrictionCurve = new AGS_MTC_WheelFrictionCurve(f_extSlip, f_extVal, f_asSlip, f_asVal, f_tailVal);
			wheelCollider.sidewaysFrictionCurve = new AGS_MTC_WheelFrictionCurve(s_extSlip, s_extVal, s_asSlip, s_asVal, s_tailVal);
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(base.gameObject.transform.position, 0.04f);
			Vector3 vector = base.gameObject.transform.position - base.gameObject.transform.up * travel;
			if (wheelCollider != null)
			{
				vector += base.gameObject.transform.up * wheelCollider.compressionDistance;
			}
			Gizmos.DrawWireSphere(vector, wheelRadius);
			Gizmos.DrawSphere(vector, 0.04f);
			Gizmos.DrawRay(base.gameObject.transform.position - base.gameObject.transform.up * wheelRadius, base.gameObject.transform.up * travel);
			if (wheelCollider != null)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(wheelCollider.hitPoint, 0.05f);
			}
		}
	}
}
