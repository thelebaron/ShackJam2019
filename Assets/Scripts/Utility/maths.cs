using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace Game.Utils
{
    public static class maths
    {
        private static readonly float3 zeroVector    = new float3(0.0f, 0.0f, 0.0f);
        private static readonly float3 oneVector     = new float3(1f, 1f, 1f);
        private static readonly float3 upVector      = new float3(0.0f, 1f, 0.0f);
        private static readonly float3 downVector    = new float3(0.0f, -1f, 0.0f);
        private static readonly float3 leftVector    = new float3(-1f, 0.0f, 0.0f);
        private static readonly float3 rightVector   = new float3(1f, 0.0f, 0.0f);
        private static readonly float3 forwardVector = new float3(0.0f, 0.0f, 1f);
        private static readonly float3 backVector    = new float3(0.0f, 0.0f, -1f);

        public static float3 zero    => zeroVector;
        public static float3 one     => oneVector;
        public static float3 up      => upVector;
        public static float3 down    => downVector;
        public static float3 left    => leftVector;
        public static float3 right   => rightVector;
        public static float3 forward => forwardVector;
        public static float3 back    => backVector;
        public static float  epsilon => 0.0001f;

        //public static readonly float Epsilon = !MathfInternal.IsFlushToZeroEnabled ? MathfInternal.FloatMinDenormal : MathfInternal.FloatMinNormal;
        public const float pi               = 3.141593f;
        public const float infinity         = float.PositiveInfinity;
        public const float negativeInfinity = float.NegativeInfinity;
        public const float deg2Rad          = 0.01745329f;
        public const float rad2Deg          = 57.29578f;

        /*
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct MathfInternal
        {
            public static volatile float FloatMinNormal = 1.175494E-38f;
            public static volatile float FloatMinDenormal = float.Epsilon;
            public static bool IsFlushToZeroEnabled = (double) MathfInternal.FloatMinDenormal == 0.0;
        }*/

        public static bool sameVector(Vector3 lhs, Vector3 rhs)
        {
            var x = System.Math.Round(lhs.x, 2);
            var y = System.Math.Round(lhs.y, 2);
            var z = System.Math.Round(lhs.z, 2);
            var xyz = new double3(x,y,z);
            
            var a = System.Math.Round(rhs.x, 2);
            var b = System.Math.Round(rhs.y, 2);
            var c = System.Math.Round(rhs.z, 2);
            var abc = new double3(a,b,c);

            return xyz.Equals(abc);
        }
        
        //Returns angle in degree, modulo 360.
        public static float anglemod(float a)
        {
            if (a >= 0)
                a -= 360 * (int) (a / 360);
            else
                a += 360 * (1 + (int) (-a / 360));
            a = (float) 360.0 / 65536 * ((int) (a * (65536 / 360.0)) & 65535);

            return a;
        }

        public static float abs(float value)
        {
            value = (Math.Abs(value) < epsilon) ? 0.0f : value;
            return value;
        }

        public static float3 clampMagnitude(float3 vector, float maxLength)
        {
            float sqrMagnitude = math.lengthsq(vector); //vector.sqrMagnitude;
            if ((double) sqrMagnitude <= (double) maxLength * (double) maxLength)
                return vector;
            float num1 = (float) Math.Sqrt((double) sqrMagnitude);
            float num2 = vector.x / num1;
            float num3 = vector.y / num1;
            float num4 = vector.z / num1;
            return new float3(num2 * maxLength, num3 * maxLength, num4 * maxLength);
        }

        [BurstCompile]
        public static float3 scale(float3 lhs, float3 rhs)
        {
            return new float3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
        }

        /// <summary>
        /// this can be used to snap individual super-small property
        /// values to zero, for avoiding some floating point issues.
        /// </summary>
        public static float3 notnan(float3 value, float newepsilon = 0.0001f) // was SnapToZero
        {
            value.x = (Mathf.Abs(value.x) < newepsilon) ? 0.0f : value.x;
            value.y = (Mathf.Abs(value.y) < newepsilon) ? 0.0f : value.y;
            value.z = (Mathf.Abs(value.z) < newepsilon) ? 0.0f : value.z;
            return value;
        }

        public static float notnan(float value, float newepsilon = 0.0001f)
        {
            value = (Mathf.Abs(value) < newepsilon) ? 0.0f : value;
            return value;
        }

        /// <summary>
        /// this can be used to snap individual super-small property
        /// values to zero, for avoiding some floating point issues.
        /// </summary>
        public static float3 snapToZero(float3 value)
        {
            value.x = maths.abs(value.x);
            value.y = maths.abs(value.y);
            value.z = maths.abs(value.z);
            return value;
        }

        /*
        public static float SnapToZeroA(float value, float epsilon = 0.0001f)
        {
            value = (Mathf.Abs(value) < epsilon) ? 0.0f : value;
            return value;

        }   */
        /// <summary>
        /// this can be used to snap individual super-small property
        /// values to zero, for avoiding some floating point issues.
        /// </summary>
        public static float SnapToZeroB(float value, float epsilon = 0.0001f)
        {
            value = (math.abs(value) < epsilon) ? 0.0f : value;
            return value;
        }

        /// <summary>
        ///   <para>Projects a vector onto a plane defined by a normal orthogonal to the plane.</para>
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="planeNormal"></param>
        public static float3 projectOnPlane(float3 vector, float3 planeNormal)
        {
            float num1 = math.dot(planeNormal, planeNormal);
            if ((double) num1 < (double) maths.epsilon)
                return vector;
            float num2 = math.dot(vector, planeNormal);
            return new float3(vector.x - planeNormal.x * num2 / num1, vector.y - planeNormal.y * num2 / num1, vector.z - planeNormal.z * num2 / num1);
        }

        [BurstCompile]
        public static quaternion nanSafeQuaternion(quaternion quaternion, quaternion prevQuaternion = default(quaternion))
        {
            quaternion.value.x = double.IsNaN(quaternion.value.x) ? prevQuaternion.value.x : quaternion.value.x;
            quaternion.value.y = double.IsNaN(quaternion.value.y) ? prevQuaternion.value.y : quaternion.value.y;
            quaternion.value.z = double.IsNaN(quaternion.value.z) ? prevQuaternion.value.z : quaternion.value.z;
            quaternion.value.w = double.IsNaN(quaternion.value.w) ? prevQuaternion.value.w : quaternion.value.w;

            return quaternion;
        }


        public static quaternion ToQ(float3 v)
        {
            return ToQ(v.y, v.x, v.z);
        }

        public static quaternion ToQ(float yaw, float pitch, float roll)
        {
            yaw   *= Mathf.Deg2Rad;
            pitch *= Mathf.Deg2Rad;
            roll  *= Mathf.Deg2Rad;
            float      rollOver2     = roll * 0.5f;
            float      sinRollOver2  = (float) Math.Sin((double) rollOver2);
            float      cosRollOver2  = (float) Math.Cos((double) rollOver2);
            float      pitchOver2    = pitch * 0.5f;
            float      sinPitchOver2 = (float) Math.Sin((double) pitchOver2);
            float      cosPitchOver2 = (float) Math.Cos((double) pitchOver2);
            float      yawOver2      = yaw * 0.5f;
            float      sinYawOver2   = (float) Math.Sin((double) yawOver2);
            float      cosYawOver2   = (float) Math.Cos((double) yawOver2);
            Quaternion result;
            result.w = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
            result.x = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2;
            result.y = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2;
            result.z = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;

            return result;
        }

        public static float3 getAngles(quaternion q1)
        {
            float   sqw  = q1.value.w * q1.value.w;
            float   sqx  = q1.value.x * q1.value.x;
            float   sqy  = q1.value.y * q1.value.y;
            float   sqz  = q1.value.z * q1.value.z;
            float   unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            float   test = q1.value.x * q1.value.w - q1.value.y * q1.value.z;
            Vector3 v;

            if (test > 0.4995f * unit)
            {
                // singularity at north pole
                v.y = 2f * Mathf.Atan2(q1.value.y, q1.value.x);
                v.x = Mathf.PI / 2;
                v.z = 0;
                return NormalizeAngles(v * Mathf.Rad2Deg);
            }

            if (test < -0.4995f * unit)
            {
                // singularity at south pole
                v.y = -2f * Mathf.Atan2(q1.value.y, q1.value.x);
                v.x = -Mathf.PI / 2;
                v.z = 0;
                return NormalizeAngles(v * Mathf.Rad2Deg);
            }

            quaternion q = new quaternion(q1.value.w, q1.value.z, q1.value.x, q1.value.y);
            v.y = (float) Math.Atan2(2f * q.value.x * q.value.w + 2f * q.value.y * q.value.z, 1 - 2f * (q.value.z * q.value.z + q.value.w * q.value.w)); // Yaw
            v.x = (float) Math.Asin(2f * (q.value.x * q.value.z - q.value.w * q.value.y));                                                               // Pitch
            v.z = (float) Math.Atan2(2f * q.value.x * q.value.y + 2f * q.value.z * q.value.w, 1 - 2f * (q.value.y * q.value.y + q.value.z * q.value.z)); // Roll
            return NormalizeAngles(v * Mathf.Rad2Deg);
        }

        static float3 NormalizeAngles(float3 angles)
        {
            angles.x = normalizeAngle(angles.x);
            angles.y = normalizeAngle(angles.y);
            angles.z = normalizeAngle(angles.z);
            return angles;
        }

        static float normalizeAngle(float angle)
        {
            while (angle > 360)
                angle -= 360;
            while (angle < 0)
                angle += 360;
            return angle;
        }
        
        /// from unity forums https://forum.unity.com/threads/does-somebody-have-an-ecs-version-of-this.724358/#post-4836008
        /// tertle
        /// <summary>
        /// Converts a quaternion to euler.
        /// </summary>
        /// <param name="quaternion">The quaternion.</param>
        /// <returns>Euler angles.</returns>
        public static float3 ToEuler(this quaternion quaternion)
        {
            var q = quaternion.value;
 
            var sinRCosP = 2 * ((q.w * q.x) + (q.y * q.z));
            var cosRCosP = 1 - (2 * ((q.x * q.x) + (q.y * q.y)));
            var roll     = math.atan2(sinRCosP, cosRCosP);
 
            // pitch (y-axis rotation)
            var sinP  = 2 * ((q.w * q.y) - (q.z * q.x));
            var pitch = math.abs(sinP) >= 1 ? math.sign(sinP) * math.PI / 2 : math.asin(sinP);
 
            // yaw (z-axis rotation)
            var sinYCosP = 2 * ((q.w * q.z) + (q.x * q.y));
            var cosYCosP = 1 - (2 * ((q.y * q.y) + (q.z * q.z)));
            var yaw      = math.atan2(sinYCosP, cosYCosP);
 
            return new float3(roll, pitch, yaw);
        }

        /// <summary>
        /// returns a quaternion rotated randomly around a specific axis, typically used for raycast surface normal calculations
        /// </summary>
        /// <param name="surfaceNormal"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        public static quaternion randomAroundAxis(float3 surfaceNormal, Unity.Mathematics.Random random)
        {
            // from https://answers.unity.com/questions/1232279/more-specific-quaternionlookrotation.html
            //var x = Quaternion.AngleAxis(random.NextFloat(0, 360f),surfaceNormal) * Quaternion.LookRotation(hit.normal);
            var normal = surfaceNormal;
                
            // Should there be checks for other Unit measurements?
            if (normal.Equals(up) ) 
            {
                normal          += new float3(random.NextFloat(-0.002f, 0.002f), 0, random.NextFloat(-0.002f, 0.002f));
            }
            
            return math.mul(quaternion.AxisAngle(surfaceNormal,random.NextFloat(0, 360f)) , quaternion.LookRotationSafe(normal, up));
        }
        
        /* Doesnt appear to work as a static method, need to put into each system
        public static NativeArray<Unity.Mathematics.Random> GetRandoms(Unity.Mathematics.Random random, int count)
        {
            var array = new NativeArray<Unity.Mathematics.Random>(count, Allocator.TempJob);
            for (int i = 0; i < array.Length; i++)
                array[i] = new Unity.Mathematics.Random((uint) random.NextInt());

            return array;
        }
        */
    }
}