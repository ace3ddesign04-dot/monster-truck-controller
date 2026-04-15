using System;
using UnityEngine;

namespace CustomVP
{
	public class WheelCollider : MonoBehaviour
	{
		private GameObject wheel;

		private Rigidbody rigidBody;

		private float wheelMass = 1f;

		private float wheelRadius = 0.5f;

		private float suspensionLength = 1f;

		private float suspensionSpring = 10f;

		private float suspensionDamper = 2f;

		private float currentFwdFrictionCoef = 1f;

		private float currentSideFrictionCoef = 1f;

		private float currentSurfaceFrictionCoef = 1f;

		private float currentSteeringAngle;

		private float currentMotorTorque;

		private float currentBrakeTorque;

		private float currentMomentOfInertia = 0.125f;

		private int currentRaycastMask = -67108865;

		private WheelSweepType currentSweepType;

		private CustomWheelFrictionCurve fwdFrictionCurve = new CustomWheelFrictionCurve(0.06f, 1.2f, 0.065f, 1.25f, 0.7f);

		private CustomWheelFrictionCurve sideFrictionCurve = new CustomWheelFrictionCurve(0.03f, 1f, 0.04f, 1.05f, 0.7f);

		private bool automaticUpdates;

		private Vector3 gNorm = new Vector3(0f, -1f, 0f);

		private Action<Vector3> onImpactCallback;

		private Action<WheelCollider> preUpdateCallback;

		private Action<WheelCollider> postUpdateCallback;

		private float extSpringForce;

		private float rollingResistanceCoefficient = 0.005f;

		private float rotationalResistanceCoefficient;

		private bool grounded;

		private float inertiaInverse;

		private float radiusInverse;

		private float prevFLong;

		private float prevFLat;

		private float currentSuspensionCompression;

		private float prevSuspensionCompression;

		private float currentAngularVelocity;

		private float vSpring;

		private float fDamp;

		private Vector3 wF;

		private Vector3 wR;

		private Vector3 wheelForward;

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

		private Vector3 hitNormal;

		private Collider hitCollider;

		private AnimationCurve springcurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

		[HideInInspector]
		public WheelCollider OppositeWheel;

		[HideInInspector]
		public WheelCollider AnotherAxleWheelL;

		[HideInInspector]
		public WheelCollider AnotherAxleWheelR;

		[HideInInspector]
		public bool DiffLock;

		[HideInInspector]
		public float DiffLockRatio;

		[HideInInspector]
		public float InteraxleDiffLockRatio;

		[HideInInspector]
		public bool InteraxleDifLock;

		[HideInInspector]
		public float FakeRPM;

		private float MinClampedOffset = 0.3f;

		private float MaxClampedOffset = 0.5f;

		public float hitOffset;

		public float _hitOffsetSmooth;

		[HideInInspector]
		public float correctedSuspensionCompression;

		private float alignSpeed;

		[HideInInspector]
		public float rpmLimit;

		private float fakeForce;

		public Vector3 LongForce => wF * localForce.z;

		public AnimationCurve springCurve
		{
			get
			{
				return springcurve;
			}
			set
			{
				springcurve = value;
			}
		}

		public Rigidbody rb
		{
			get
			{
				return rigidBody;
			}
			set
			{
				rigidBody = value;
			}
		}

		public float spring
		{
			get
			{
				return suspensionSpring;
			}
			set
			{
				suspensionSpring = value;
			}
		}

		public float damper
		{
			get
			{
				return suspensionDamper;
			}
			set
			{
				suspensionDamper = value;
			}
		}

		public float length
		{
			get
			{
				return suspensionLength;
			}
			set
			{
				suspensionLength = value;
			}
		}

		public float mass
		{
			get
			{
				return wheelMass;
			}
			set
			{
				wheelMass = value;
				currentMomentOfInertia = wheelMass * wheelRadius * wheelRadius * 0.5f;
				inertiaInverse = 1f / currentMomentOfInertia;
			}
		}

		public float Velocity => vWheel;

		public float radius
		{
			get
			{
				return wheelRadius;
			}
			set
			{
				wheelRadius = value;
				currentMomentOfInertia = wheelMass * wheelRadius * wheelRadius * 0.5f;
				inertiaInverse = 1f / currentMomentOfInertia;
				radiusInverse = 1f / wheelRadius;
			}
		}

		public CustomWheelFrictionCurve forwardFrictionCurve
		{
			get
			{
				return fwdFrictionCurve;
			}
			set
			{
				if (value != null)
				{
					fwdFrictionCurve = value;
				}
			}
		}

		public CustomWheelFrictionCurve sidewaysFrictionCurve
		{
			get
			{
				return sideFrictionCurve;
			}
			set
			{
				if (value != null)
				{
					sideFrictionCurve = value;
				}
			}
		}

		public float forwardFrictionCoefficient
		{
			get
			{
				return currentFwdFrictionCoef;
			}
			set
			{
				currentFwdFrictionCoef = value;
			}
		}

		public float sideFrictionCoefficient
		{
			get
			{
				return currentSideFrictionCoef;
			}
			set
			{
				currentSideFrictionCoef = value;
			}
		}

		public float surfaceFrictionCoefficient
		{
			get
			{
				return currentSurfaceFrictionCoef;
			}
			set
			{
				currentSurfaceFrictionCoef = value;
			}
		}

		public float rollingResistance
		{
			get
			{
				return rollingResistanceCoefficient;
			}
			set
			{
				rollingResistanceCoefficient = value;
			}
		}

		public float rotationalResistance
		{
			get
			{
				return rotationalResistanceCoefficient;
			}
			set
			{
				rotationalResistanceCoefficient = value;
			}
		}

		public float brakeTorque
		{
			get
			{
				return currentBrakeTorque;
			}
			set
			{
				currentBrakeTorque = Mathf.Abs(value);
			}
		}

		public float motorTorque
		{
			get
			{
				return currentMotorTorque;
			}
			set
			{
				currentMotorTorque = value;
			}
		}

		public float steeringAngle
		{
			get
			{
				return currentSteeringAngle;
			}
			set
			{
				currentSteeringAngle = value;
			}
		}

		public WheelSweepType sweepType
		{
			get
			{
				return currentSweepType;
			}
			set
			{
				currentSweepType = value;
			}
		}

		public bool autoUpdateEnabled
		{
			get
			{
				return automaticUpdates;
			}
			set
			{
				automaticUpdates = value;
			}
		}

		public bool isGrounded => grounded;

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

		public float angularVelocity
		{
			get
			{
				return currentAngularVelocity;
			}
			set
			{
				currentAngularVelocity = value;
			}
		}

		public float linearVelocity => currentAngularVelocity * wheelRadius;

		public float compressionDistance => currentSuspensionCompression;

		public float compressionPercent => correctedSuspensionCompression / suspensionLength;

		public int raycastMask
		{
			get
			{
				return currentRaycastMask;
			}
			set
			{
				currentRaycastMask = value;
			}
		}

		public float perFrameRotation
		{
			get
			{
				if (FakeRPM > 0f)
				{
					return FakeRPM;
				}
				return rpm * 6f * Time.deltaTime;
			}
		}

		public float externalSpringForce
		{
			get
			{
				return extSpringForce;
			}
			set
			{
				extSpringForce = value;
			}
		}

		public float momentOfInertia => currentMomentOfInertia;

		public float springForce => localForce.y + extSpringForce;

		public float dampForce => fDamp;

		public float longitudinalForce => localForce.z;

		public float lateralForce => localForce.x;

		public float longitudinalSlip => sLong;

		public float lateralSlip => sLat;

		public Vector3 wheelLocalVelocity => localVelocity;

		public Collider contactColliderHit => hitCollider;

		public Vector3 contactNormal => hitNormal;

		public Vector3 worldHitPos => base.transform.position - base.transform.up * (suspensionLength - currentSuspensionCompression + wheelRadius - 0.5f);

		public void setImpactCallback(Action<Vector3> callback)
		{
			onImpactCallback = callback;
		}

		public void setPreUpdateCallback(Action<WheelCollider> callback)
		{
			preUpdateCallback = callback;
		}

		public void setPostUpdateCallback(Action<WheelCollider> callback)
		{
			postUpdateCallback = callback;
		}

		public void Update()
		{
			_hitOffsetSmooth = Mathf.MoveTowards(_hitOffsetSmooth, hitOffset, Time.deltaTime);
			if (automaticUpdates)
			{
				if (preUpdateCallback != null)
				{
					preUpdateCallback(this);
				}
				updateWheel();
				if (postUpdateCallback != null)
				{
					postUpdateCallback(this);
				}
			}
		}

		public void updateWheel()
		{
			if (wheel == null)
			{
				wheel = base.gameObject;
			}
			wheelForward = Quaternion.AngleAxis(currentSteeringAngle, wheel.transform.up) * wheel.transform.forward;
			prevSuspensionCompression = correctedSuspensionCompression;
			bool flag = grounded;
			float xContactOffset = 0f;
			if (checkSuspensionContact(ref xContactOffset))
			{
				float spring = this.spring;
				float num = this.spring * 2f;
				float num2 = 0f;
				if (xContactOffset >= MaxClampedOffset)
				{
					num2 = spring;
				}
				else if (xContactOffset > MinClampedOffset)
				{
					float num3 = (MaxClampedOffset - xContactOffset) / (MaxClampedOffset - MinClampedOffset);
					num2 = spring + (num - spring) * num3;
				}
				wR = Vector3.Cross(hitNormal, wheelForward);
				wF = -Vector3.Cross(hitNormal, wR);
				wF = wheelForward - hitNormal * Vector3.Dot(wheelForward, hitNormal);
				wR = Vector3.Cross(hitNormal, wF);
				Vector3 a = Vector3.zero;
				if (rigidBody != null)
				{
					a = rigidBody.GetPointVelocity(hitPoint);
					if (hitCollider != null && hitCollider.attachedRigidbody != null)
					{
						a -= hitCollider.attachedRigidbody.GetPointVelocity(hitPoint);
					}
				}
				float magnitude = a.magnitude;
				localVelocity.z = Vector3.Dot(a.normalized, wF) * magnitude;
				localVelocity.x = Vector3.Dot(a.normalized, wR) * magnitude;
				localVelocity.y = Vector3.Dot(a.normalized, hitNormal) * magnitude;
				calcSpring();
				integrateForces(spring);
				if (!flag && onImpactCallback != null)
				{
					onImpactCallback(localVelocity);
				}
			}
			else
			{
				integrateUngroundedTorques();
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
			if (OppositeWheel != null && DiffLock)
			{
				float num4 = currentAngularVelocity - OppositeWheel.currentAngularVelocity;
				currentAngularVelocity -= num4 * 0.5f * DiffLockRatio;
			}
			if (AnotherAxleWheelL != null && AnotherAxleWheelR != null && InteraxleDifLock)
			{
				float num5 = (AnotherAxleWheelL.currentAngularVelocity + AnotherAxleWheelR.currentAngularVelocity) / 2f;
				float num6 = (currentAngularVelocity + OppositeWheel.currentAngularVelocity) / 2f;
				float num7 = 1f;
				if (AnotherAxleWheelL.radius > radius)
				{
					num7 = radius / AnotherAxleWheelL.radius;
				}
				else
				{
					num7 = AnotherAxleWheelL.radius / radius;
				}
				float num8 = num6 - num5;
				currentAngularVelocity -= num8 * 0.5f * InteraxleDiffLockRatio * AnotherAxleWheelL.radius * AnotherAxleWheelL.radius;
			}
			ClampMaxRPM();
		}

		public void clearGroundedState()
		{
			grounded = false;
			vSpring = (fDamp = (prevSuspensionCompression = (currentSuspensionCompression = (correctedSuspensionCompression = 0f))));
			localForce = Vector3.zero;
			hitNormal = Vector3.up;
			hitPoint = Vector3.zero;
			realHitPoint = Vector3.zero;
			localVelocity = Vector3.zero;
			hitCollider = null;
		}

		private void integrateForces(float clampedForce)
		{
			calcFriction();
			float num = 0.1f;
			if ((prevFLong < 0f && localForce.z > 0f) || (prevFLong > 0f && localForce.z < 0f))
			{
				localForce.z *= num;
			}
			if ((prevFLat < 0f && localForce.x > 0f) || (prevFLat > 0f && localForce.x < 0f))
			{
				localForce.x *= num;
			}
			Vector3 vector = hitNormal * (localForce.y - fakeForce);
			if (clampedForce > 0f)
			{
				Vector3.ClampMagnitude(vector, clampedForce);
			}
			vector += calcAG(hitNormal, localForce.y - fakeForce);
			vector += localForce.z * wF;
			vector += localForce.x * wR;
			if (rigidBody != null && !float.IsNaN(vector.x))
			{
				rigidBody.AddForceAtPosition(vector, hitPoint, ForceMode.Force);
			}
			if (hitCollider != null && hitCollider.attachedRigidbody != null)
			{
				hitCollider.attachedRigidbody.AddForceAtPosition(-vector, hitPoint, ForceMode.Force);
			}
			prevFLong = localForce.z;
			prevFLat = localForce.x;
		}

		private Vector3 calcAG(Vector3 hitNormal, float springForce)
		{
			Vector3 vector = new Vector3(0f, 0f, 0f);
			float num = Vector3.Dot(hitNormal, gNorm);
			float num2 = num * springForce;
			Vector3 lhs = Vector3.Cross(hitNormal, gNorm);
			Vector3 lhs2 = Vector3.Cross(lhs, hitNormal);
			float num3 = Vector3.Dot(lhs2, wR);
			vector = num2 * num3 * wR * Mathf.Clamp(currentSideFrictionCoef, 0f, 1f);
			if (brakeTorque > 0f && Mathf.Abs(motorTorque) < brakeTorque)
			{
				float num4 = Vector3.Dot(lhs2, wF);
				vector += num2 * num4 * wF * Mathf.Clamp(currentFwdFrictionCoef, 0f, 1f);
			}
			return vector;
		}

		private void integrateUngroundedTorques()
		{
			currentAngularVelocity += currentMotorTorque * inertiaInverse * Time.fixedDeltaTime;
			if (currentAngularVelocity != 0f)
			{
				float f = rotationalResistanceCoefficient * currentAngularVelocity * inertiaInverse * Time.fixedDeltaTime;
				f = Mathf.Min(Mathf.Abs(f), Mathf.Abs(currentAngularVelocity)) * Mathf.Sign(currentAngularVelocity);
				currentAngularVelocity -= f;
			}
			if (currentAngularVelocity != 0f)
			{
				float b = currentBrakeTorque * inertiaInverse * Time.fixedDeltaTime;
				b = Mathf.Min(Mathf.Abs(currentAngularVelocity), b) * Mathf.Sign(currentAngularVelocity);
				currentAngularVelocity -= b;
			}
		}

		private bool checkSuspensionContact(ref float xContactOffset)
		{
			switch (currentSweepType)
			{
			case WheelSweepType.RAY:
				return suspensionSweepRaycast();
			case WheelSweepType.SPHERE:
				return suspensionSweepSpherecast(ref xContactOffset);
			default:
				return suspensionSweepRaycast();
			}
		}

		private bool suspensionSweepRaycast()
		{
			if (Physics.Raycast(wheel.transform.position, -wheel.transform.up, out RaycastHit hitInfo, suspensionLength + wheelRadius, currentRaycastMask))
			{
				currentSuspensionCompression = suspensionLength + wheelRadius - hitInfo.distance;
				correctedSuspensionCompression = currentSuspensionCompression;
				hitNormal = hitInfo.normal;
				hitCollider = hitInfo.collider;
				hitPoint = hitInfo.point;
				grounded = true;
				return true;
			}
			grounded = false;
			return false;
		}

		private bool suspensionSweepSpherecast(ref float xContactOffset)
		{
			if (Physics.SphereCast(wheel.transform.position + wheel.transform.up * wheelRadius, radius, -wheel.transform.up, out RaycastHit hitInfo, length + wheelRadius, currentRaycastMask))
			{
				realHitPoint = hitInfo.point;
				hitInfo.point -= wheel.transform.up * _hitOffsetSmooth;
				hitInfo.distance += _hitOffsetSmooth;
				Vector3 b = base.transform.position - base.transform.up * suspensionLength + base.transform.up * compressionDistance;
				Vector3 direction = Vector3.ProjectOnPlane((hitInfo.point - b).normalized, base.transform.forward);
				Vector3 vector = base.transform.InverseTransformDirection(direction);
				xContactOffset = vector.x;
				currentSuspensionCompression = suspensionLength + wheelRadius - hitInfo.distance;
				float b2 = Mathf.InverseLerp(60f, 0f, Vector3.Angle(base.transform.up, hitInfo.normal)) * 6f + 1f;
				alignSpeed = Mathf.Lerp(alignSpeed, b2, Time.fixedDeltaTime * 5f);
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

		private void calcSpring()
		{
			vSpring = (correctedSuspensionCompression - prevSuspensionCompression) / Time.fixedDeltaTime;
			fDamp = suspensionDamper * vSpring;
			float num = Mathf.InverseLerp(140f, 0f, Vector3.Angle(base.transform.up, hitNormal));
			float num2 = suspensionSpring * (springCurve.Evaluate(compressionPercent) * (correctedSuspensionCompression / compressionPercent));
			num2 += fDamp;
			num2 *= num;
			if (correctedSuspensionCompression > suspensionLength)
			{
				fakeForce = 10000f;
			}
			else
			{
				fakeForce = 0f;
			}
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			localForce.y = num2 + fakeForce;
		}

		private void calcFriction()
		{
			calcFrictionStandard();
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

		public void calcFrictionStandard()
		{
			currentAngularVelocity += currentMotorTorque * inertiaInverse * Time.fixedDeltaTime;
			if (currentAngularVelocity != 0f)
			{
				float num = localForce.y * rollingResistanceCoefficient;
				float num2 = num * wheelRadius;
				float a = num2 * inertiaInverse * Time.fixedDeltaTime;
				a = Mathf.Min(a, Mathf.Abs(currentAngularVelocity)) * Mathf.Sign(currentAngularVelocity);
				currentAngularVelocity -= a;
			}
			if (currentAngularVelocity != 0f)
			{
				currentAngularVelocity -= currentAngularVelocity * rotationalResistanceCoefficient * radiusInverse * inertiaInverse * Time.fixedDeltaTime;
			}
			float num3 = currentBrakeTorque * inertiaInverse * Time.fixedDeltaTime;
			float num4 = Mathf.Min(Mathf.Abs(currentAngularVelocity), num3);
			currentAngularVelocity += num4 * (0f - Mathf.Sign(currentAngularVelocity));
			float num5 = num3 - num4;
			vWheel = currentAngularVelocity * wheelRadius;
			sLong = calcLongSlip(localVelocity.z, vWheel);
			sLat = calcLatSlip(localVelocity.z, localVelocity.x);
			vWheelDelta = vWheel - localVelocity.z;
			float num6 = localForce.y + extSpringForce;
			float num7 = fwdFrictionCurve.evaluate(sLong) * num6 * currentFwdFrictionCoef * currentSurfaceFrictionCoef;
			float num8 = sideFrictionCurve.evaluate(sLat) * num6 * currentSideFrictionCoef * currentSurfaceFrictionCoef;
			localForce.x = num8;
			if (localForce.x > Mathf.Abs(localVelocity.x) * num6)
			{
				localForce.x = Mathf.Abs(localVelocity.x) * num6;
			}
			localForce.x *= 0f - Mathf.Sign(localVelocity.x);
			float num9 = vWheelDelta * radiusInverse;
			float f = num9 * currentMomentOfInertia;
			float num10 = Mathf.Abs(f) / Time.fixedDeltaTime;
			float a2 = num10 * radiusInverse;
			a2 = Mathf.Min(a2, num7);
			float num11 = a2 * wheelRadius * (0f - Mathf.Sign(vWheelDelta));
			localForce.z = a2 * Mathf.Sign(vWheelDelta);
			float num12 = num11 * inertiaInverse * Time.fixedDeltaTime;
			currentAngularVelocity += num12;
			if (Mathf.Abs(currentAngularVelocity) < num5)
			{
				currentAngularVelocity = 0f;
				num5 -= Mathf.Abs(currentAngularVelocity);
				float a3 = Mathf.Max(0f, Mathf.Abs(num7) - Mathf.Abs(localForce.z));
				float b = Mathf.Max(0f, num6 * Mathf.Abs(localVelocity.z) - Mathf.Abs(localForce.z));
				float num13 = Mathf.Min(a3, b);
				localForce.z += num13 * (0f - Mathf.Sign(localVelocity.z));
			}
			else
			{
				currentAngularVelocity += (0f - Mathf.Sign(currentAngularVelocity)) * num5;
			}
			combinatorialFriction(num8, num7, localForce.x, localForce.z, out localForce.x, out localForce.z);
		}

		private void ClampMaxRPM()
		{
			if (rpmLimit != 0f)
			{
				float num = Mathf.Abs(currentAngularVelocity);
				if (num > rpmLimit)
				{
					currentAngularVelocity = rpmLimit * Mathf.Sign(currentAngularVelocity);
				}
			}
		}

		private void combinatorialFriction(float latMax, float longMax, float fLat, float fLong, out float combLat, out float combLong)
		{
			float num = (fwdFrictionCurve.max + sideFrictionCurve.max) * 0.5f * (localForce.y + extSpringForce);
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
