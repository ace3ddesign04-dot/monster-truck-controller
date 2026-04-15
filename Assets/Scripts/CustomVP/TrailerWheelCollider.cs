using System;
using UnityEngine;

namespace CustomVP
{
	public class TrailerWheelCollider : MonoBehaviour
	{
		private Rigidbody rigidBody;

		public float wheelMass = 1f;

		public float wheelRadius = 0.5f;

		public float suspensionLength = 1f;

		public float suspensionSpring = 10f;

		public float suspensionDamper = 2f;

		public float currentFwdFrictionCoef = 1f;

		public float currentSideFrictionCoef = 1f;

		public float currentSurfaceFrictionCoef = 1f;

		[HideInInspector]
		public float currentBrakeTorque;

		public AnimationCurve springcurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

		private float currentMomentOfInertia = 0.125f;

		private CustomWheelFrictionCurve fwdFrictionCurve = new CustomWheelFrictionCurve(0.06f, 1.2f, 0.065f, 1.25f, 0.7f);

		private CustomWheelFrictionCurve sideFrictionCurve = new CustomWheelFrictionCurve(0.03f, 1f, 0.04f, 1.05f, 0.7f);

		private float inertiaInverse;

		private float radiusInverse;

		private bool grounded;

		private float prevFLong;

		private float prevFLat;

		private float currentSuspensionCompression;

		private float prevSuspensionCompression;

		private float vSpring;

		private float fDamp;

		private float alignSpeed;

		[HideInInspector]
		public float correctedSuspensionCompression;

		private float currentAngularVelocity;

		private Vector3 wF;

		private Vector3 wR;

		private Vector3 localVelocity;

		private Vector3 localForce;

		private float vWheel;

		private float vWheelDelta;

		private float sLong;

		private float sLat;

		[HideInInspector]
		public Vector3 hitPoint;

		[HideInInspector]
		public Vector3 realHitPoint;

		[HideInInspector]
		public float hitOffset;

		private float hitOffsetSmooth;

		private Vector3 hitNormal;

		private Collider hitCollider;

		private SphereCollider BumpStopCollider;

		private SphereCollider SpherecastProtector;

		public float rpm
		{
			get
			{
				return currentAngularVelocity * (30f / (float)Math.PI);
			}
			set
			{
				currentAngularVelocity = value * ((float)Math.PI / 30f);
			}
		}

		public float compressionPercent => correctedSuspensionCompression / suspensionLength;

		public float perFrameRotation => rpm * 6f * Time.deltaTime;

		public Vector3 GetVisualWheelPosition()
		{
			return base.transform.position - base.transform.up * (suspensionLength - correctedSuspensionCompression);
		}

		private void Start()
		{
			rigidBody = GetComponentInParent<Rigidbody>();
			InitializeColliders();
		}

		private void InitializeColliders()
		{
			BumpStopCollider = new GameObject("BumpStop").AddComponent<SphereCollider>();
			BumpStopCollider.transform.parent = base.gameObject.transform;
			BumpStopCollider.transform.localPosition = new Vector3(0f, 0f, 0f);
			BumpStopCollider.gameObject.layer = 26;
			BumpStopCollider.radius = 0.1f;
			SpherecastProtector = new GameObject("SpherecastProtector").AddComponent<SphereCollider>();
			SpherecastProtector.transform.parent = base.gameObject.transform;
			SpherecastProtector.transform.localPosition = new Vector3(0f, 0f, 0f);
			SpherecastProtector.gameObject.layer = 26;
			SpherecastProtector.radius = wheelRadius;
			PhysicMaterial physicMaterial = new PhysicMaterial("ZeroFriction");
			physicMaterial.bounciness = 0f;
			physicMaterial.dynamicFriction = 0f;
			physicMaterial.staticFriction = 0f;
			SpherecastProtector.material = physicMaterial;
			BumpStopCollider.material = physicMaterial;
		}

		public void Update()
		{
			hitOffsetSmooth = Mathf.MoveTowards(hitOffsetSmooth, hitOffset, Time.deltaTime);
			BumpStopCollider.transform.position = base.transform.position - base.transform.up * (wheelRadius - hitOffset - 0.1f);
		}

		private void FixedUpdate()
		{
			DoWheelCollider();
		}

		private void DoWheelCollider()
		{
			prevSuspensionCompression = correctedSuspensionCompression;
			if (DoSpherecast())
			{
				wF = base.transform.forward - hitNormal * Vector3.Dot(base.transform.forward, hitNormal);
				wR = Vector3.Cross(hitNormal, wF);
				currentMomentOfInertia = wheelMass * wheelRadius * wheelRadius * 0.5f;
				inertiaInverse = 1f / currentMomentOfInertia;
				radiusInverse = 1f / wheelRadius;
				Vector3 zero = Vector3.zero;
				zero = rigidBody.GetPointVelocity(hitPoint);
				if (hitCollider != null && hitCollider.attachedRigidbody != null)
				{
					zero -= hitCollider.attachedRigidbody.GetPointVelocity(hitPoint);
				}
				float magnitude = zero.magnitude;
				localVelocity.z = Vector3.Dot(zero.normalized, wF) * magnitude;
				localVelocity.x = Vector3.Dot(zero.normalized, wR) * magnitude;
				localVelocity.y = Vector3.Dot(zero.normalized, hitNormal) * magnitude;
				DoSpring();
				ApplyForces();
			}
			else
			{
				grounded = false;
				vSpring = (fDamp = (prevSuspensionCompression = 0f));
				currentSuspensionCompression = Mathf.MoveTowards(currentSuspensionCompression, 0f, Time.deltaTime);
				correctedSuspensionCompression = currentSuspensionCompression;
				localForce = Vector3.zero;
				hitNormal = Vector3.zero;
				hitPoint = Vector3.zero;
				realHitPoint = Vector3.zero;
				hitCollider = null;
				localVelocity = Vector3.zero;
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(base.gameObject.transform.position, 0.04f);
			Vector3 a = base.gameObject.transform.position - base.gameObject.transform.up * suspensionLength;
			a += base.transform.up * currentSuspensionCompression;
			Gizmos.DrawWireSphere(a, wheelRadius);
			Gizmos.DrawSphere(a, 0.04f);
			Gizmos.DrawRay(base.gameObject.transform.position - base.gameObject.transform.up * wheelRadius, base.gameObject.transform.up * suspensionLength);
		}

		private void ApplyForces()
		{
			CalcFrictionStandard();
			float num = 0.1f;
			if ((prevFLong < 0f && localForce.z > 0f) || (prevFLong > 0f && localForce.z < 0f))
			{
				localForce.z *= num;
			}
			if ((prevFLat < 0f && localForce.x > 0f) || (prevFLat > 0f && localForce.x < 0f))
			{
				localForce.x *= num;
			}
			Vector3 a = hitNormal * localForce.y;
			a += calcAG(hitNormal, localForce.y);
			a += localForce.z * wF;
			a += localForce.x * wR;
			if (rigidBody != null && !float.IsNaN(a.x))
			{
				rigidBody.AddForceAtPosition(a, hitPoint, ForceMode.Force);
			}
			prevFLong = localForce.z;
			prevFLat = localForce.x;
		}

		private Vector3 calcAG(Vector3 hitNormal, float springForce)
		{
			Vector3 vector = new Vector3(0f, 0f, 0f);
			float num = Vector3.Dot(hitNormal, Vector3.down);
			float num2 = num * springForce;
			Vector3 lhs = Vector3.Cross(hitNormal, Vector3.down);
			Vector3 lhs2 = Vector3.Cross(lhs, hitNormal);
			float num3 = Vector3.Dot(lhs2, wR);
			return num2 * num3 * wR * Mathf.Clamp(currentSideFrictionCoef, 0f, 1f);
		}

		private bool DoSpherecast()
		{
			if (Physics.SphereCast(base.transform.position + base.transform.up * wheelRadius, wheelRadius, -base.transform.up, out RaycastHit hitInfo, suspensionLength + wheelRadius, -67108865))
			{
				realHitPoint = hitInfo.point;
				hitInfo.point -= base.transform.up * hitOffsetSmooth;
				hitInfo.distance += hitOffsetSmooth;
				currentSuspensionCompression = suspensionLength + wheelRadius - hitInfo.distance;
				float b = Mathf.InverseLerp(60f, 0f, Vector3.Angle(base.transform.up, hitInfo.normal)) * 6f + 1f;
				alignSpeed = Mathf.Lerp(alignSpeed, b, Time.fixedDeltaTime * 5f);
				correctedSuspensionCompression = Mathf.MoveTowards(correctedSuspensionCompression, currentSuspensionCompression, Time.fixedDeltaTime * alignSpeed);
				hitNormal = hitInfo.normal;
				hitCollider = hitInfo.collider;
				hitPoint = hitInfo.point;
				grounded = true;
				return true;
			}
			grounded = false;
			return false;
		}

		private void DoSpring()
		{
			vSpring = (correctedSuspensionCompression - prevSuspensionCompression) / Time.fixedDeltaTime;
			fDamp = suspensionDamper * vSpring;
			float num = Mathf.InverseLerp(140f, 0f, Vector3.Angle(base.transform.up, hitNormal));
			float num2 = suspensionSpring * (springcurve.Evaluate(compressionPercent) * (correctedSuspensionCompression / compressionPercent));
			num2 += fDamp;
			num2 *= num;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			localForce.y = num2;
		}

		private float calcLongSlip(float vLong, float vWheel)
		{
			float num = 0f;
			if (vLong == 0f && vWheel == 0f)
			{
				return 0f;
			}
			float num2 = Mathf.Max(vLong, vWheel);
			float num3 = Mathf.Min(vLong, vWheel);
			num = (num2 - num3) / Mathf.Abs(num2);
			return Mathf.Clamp(num, 0f, 1f);
		}

		private float calcLatSlip(float vLong, float vLat)
		{
			float num = 0f;
			if (vLat == 0f)
			{
				return 0f;
			}
			if (vLong == 0f)
			{
				return 1f;
			}
			num = Mathf.Abs(Mathf.Atan(vLat / vLong));
			num *= 57.29578f;
			return num / 90f;
		}

		public void CalcFrictionStandard()
		{
			float num = currentBrakeTorque * inertiaInverse * Time.fixedDeltaTime;
			float num2 = Mathf.Min(Mathf.Abs(currentAngularVelocity), num);
			currentAngularVelocity += num2 * (0f - Mathf.Sign(currentAngularVelocity));
			float num3 = num - num2;
			vWheel = currentAngularVelocity * wheelRadius;
			sLong = calcLongSlip(localVelocity.z, vWheel);
			sLat = calcLatSlip(localVelocity.z, localVelocity.x);
			vWheelDelta = vWheel - localVelocity.z;
			float y = localForce.y;
			float num4 = fwdFrictionCurve.evaluate(sLong) * y * currentFwdFrictionCoef * currentSurfaceFrictionCoef;
			float num5 = sideFrictionCurve.evaluate(sLat) * y * currentSideFrictionCoef * currentSurfaceFrictionCoef;
			localForce.x = num5;
			if (localForce.x > Mathf.Abs(localVelocity.x) * y)
			{
				localForce.x = Mathf.Abs(localVelocity.x) * y;
			}
			localForce.x *= 0f - Mathf.Sign(localVelocity.x);
			float num6 = vWheelDelta * radiusInverse;
			float f = num6 * currentMomentOfInertia;
			float num7 = Mathf.Abs(f) / Time.fixedDeltaTime;
			float a = num7 * radiusInverse;
			a = Mathf.Min(a, num4);
			float num8 = a * wheelRadius * (0f - Mathf.Sign(vWheelDelta));
			localForce.z = a * Mathf.Sign(vWheelDelta);
			float num9 = num8 * inertiaInverse * Time.fixedDeltaTime;
			currentAngularVelocity += num9;
			if (Mathf.Abs(currentAngularVelocity) < num3)
			{
				currentAngularVelocity = 0f;
				num3 -= Mathf.Abs(currentAngularVelocity);
				float a2 = Mathf.Max(0f, Mathf.Abs(num4) - Mathf.Abs(localForce.z));
				float b = Mathf.Max(0f, y * Mathf.Abs(localVelocity.z) - Mathf.Abs(localForce.z));
				float num10 = Mathf.Min(a2, b);
				localForce.z += num10 * (0f - Mathf.Sign(localVelocity.z));
			}
			else
			{
				currentAngularVelocity += (0f - Mathf.Sign(currentAngularVelocity)) * num3;
			}
			combinatorialFriction(num5, num4, localForce.x, localForce.z, out localForce.x, out localForce.z);
		}

		private void combinatorialFriction(float latMax, float longMax, float fLat, float fLong, out float combLat, out float combLong)
		{
			float num = (fwdFrictionCurve.max + sideFrictionCurve.max) * 0.5f * localForce.y;
			float num2 = Mathf.Sqrt(fLat * fLat + fLong * fLong);
			if (num2 > num)
			{
				fLong /= num2;
				fLat /= num2;
				fLong *= num;
				fLat *= num;
			}
			combLat = fLat;
			combLong = fLong;
		}
	}
}
