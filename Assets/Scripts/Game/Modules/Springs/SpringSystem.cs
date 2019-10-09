using Game.Components.Springs;
using Game.Utils;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Modules.Springs
{
    public class SpringSystem : JobComponentSystem
    {
        private EntityQuery m_Springs;
        private EntityQuery m_PositionSprings;

        protected override void OnCreate()
        {
            m_Springs = GetEntityQuery(typeof(Spring));
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var updateJob = new SpringFixedUpdateJob
            {
                SoftForceBuffer = GetBufferFromEntity<SoftForceElement>(false),
                Time = Time.time,
                TimeScale = Time.timeScale
            };
            var updateHandle = updateJob.Schedule(this, inputDeps);

            return updateHandle;
        }


        [BurstCompile]
        [RequireComponentTag(typeof(SoftForceElement))]
        struct SpringFixedUpdateJob : IJobForEachWithEntity<Spring>
        {
            public float Time;
            public float TimeScale;
            [NativeDisableParallelForRestriction] public BufferFromEntity<SoftForceElement> SoftForceBuffer;

            public void Execute(Entity entity, int index, ref Spring spring)
            {
                if (!SoftForceBuffer.Exists(entity))
                    return;

                DynamicBuffer<SoftForceElement> dynamicBuffer = SoftForceBuffer[entity];

                if (dynamicBuffer.Length == 0)
                {
                    for (int i = 0; i < 120; i++)
                    {
                        dynamicBuffer.Add(new SoftForceElement());
                    }

                    SoftForceElement buffer = dynamicBuffer[0];
                }


                //new as of 5.21.2019
                Stop(ref dynamicBuffer, ref spring);

                if (spring.HasPendingForces())
                {
                    //Debug.Log(entity + " " + spring.m_InternalForce);
                    AddForceInternal(ref spring, spring.m_InternalForce, TimeScale);
                    ClearForces(ref spring);
                }

                if (spring.HasPendingSoftForces())
                {
                    AddSoftForce(ref dynamicBuffer, ref spring, spring.m_InternalForce, spring.m_InternalFrames,
                        TimeScale);
                    ClearForces(ref spring);
                }


                // handle forced velocity fade in
                if (spring.VelocityFadeInEndTime > Time)
                    spring.VelocityFadeInCap =
                        Mathf.Clamp01(1 - ((spring.VelocityFadeInEndTime - Time) / spring.VelocityFadeInLength));
                else
                    spring.VelocityFadeInCap = 1.0f;

                // handle smooth force
                if (!dynamicBuffer[0].Force.Equals(float3.zero))
                {
                    AddForceInternal(ref spring, dynamicBuffer[0].Force, TimeScale);
                    for (int v = 0; v < 120; v++)
                    {
                        dynamicBuffer[v] = (v < 119) ? dynamicBuffer[v + 1].Force : float3.zero;
                        if (dynamicBuffer[v].Force.Equals(float3.zero))
                            break;
                    }
                }

                Calculate(ref spring, TimeScale);
            }
        }


        public void AddForce(Entity entity, float3 force)
        {
            //entitymanager conversion flow
            var spring = EntityManager.GetComponentData<Spring>(entity);
            var buffer = EntityManager.GetBuffer<SoftForceElement>(entity);
            AddForce(ref buffer, ref spring, force, Time.timeScale);

            EntityManager.SetComponentData(entity, spring);
        }


        // do the spring calculations
        private static void Calculate(ref Spring spring, float timeScale)
        {
            if (spring.State.Equals(spring.RestState))
                return;

            spring.Velocity +=
                maths.scale((spring.RestState - spring.State),
                    spring.Stiffness); // add rest state distance * stiffness to velocity
            spring.Velocity = maths.scale(spring.Velocity, spring.Damping); // dampen velocity

            // clamp velocity to maximum
            spring.Velocity =
                maths.clampMagnitude(spring.Velocity,
                    spring.MaxVelocity);

            // apply velocity, or stop if velocity is below minimum
            if (math.lengthsq(spring.Velocity) > (spring.MinVelocity * spring.MinVelocity))
                Move(ref spring, timeScale);
            else
                Reset(ref spring);
        }


        /// <summary>
        /// adds velocity to the state and clamps state between min
        /// and max values
        /// </summary>
        private static void Move(ref Spring spring, float timeScale)
        {
            spring.State += spring.Velocity * timeScale;
            spring.State.x =
                math.clamp(spring.State.x, spring.MinState.x,
                    spring.MaxState.x); //Mathf.Clamp(spring.State.x, spring.MinState.x, spring.MaxState.x);
            spring.State.y = math.clamp(spring.State.y, spring.MinState.y, spring.MaxState.y);
            spring.State.z = math.clamp(spring.State.z, spring.MinState.z, spring.MaxState.z);
        }


        /// <summary>
        /// stops spring velocity and resets state to the static
        /// equilibrium
        /// </summary>
        private static void Reset(ref Spring spring)
        {
            spring.Velocity = float3.zero;
            spring.State = spring.RestState;
        }

        /// <summary>
        /// adds external velocity to the spring in one frame
        /// </summary>
        private static void AddForceInternal(ref Spring spring, float3 force, float timeScale)
        {
            force *= spring.VelocityFadeInCap;
            spring.Velocity += force;
            spring.Velocity = maths.clampMagnitude(spring.Velocity, spring.MaxVelocity);

            Move(ref spring, timeScale);
        }

        /// <summary>
        /// adds external velocity to the spring in one frame
        /// </summary>
        public static void AddForce(ref DynamicBuffer<SoftForceElement> softForce, ref Spring spring, float3 force,
            float timeScale)
        {
            if (timeScale < 1.0f)
                AddSoftForce(ref softForce, ref spring, force, 1, timeScale);
            else
                AddForceInternal(ref spring, force, timeScale);
        }


        /// <summary>
        /// adds a force distributed over up to 120 fixed frames
        /// </summary>
        public static void AddSoftForce(ref DynamicBuffer<SoftForceElement> softForce, ref Spring spring, float3 force,
            float frames, float timeScale)
        {
            force /= timeScale;

            frames = Mathf.Clamp(frames, 1, 120);

            AddForceInternal(ref spring, force / frames, timeScale);

            for (int v = 0; v < (Mathf.RoundToInt(frames) - 1); v++)
            {
                softForce[v] += (force / frames);
            }
        }


        /// <summary>
        /// stops spring velocity
        /// </summary>
        public static void Stop(ref DynamicBuffer<SoftForceElement> softForce, ref Spring spring)
        {
            if (spring.m_Stop)
            {
                spring.Velocity = float3.zero;
                spring.m_Stop = false;

                if (spring.m_StopAndIncludeSoftForce)
                {
                    StopSoftForce(ref softForce);
                    spring.m_StopAndIncludeSoftForce = false;
                }
            }
        }


        /// <summary>
        /// stops soft force
        /// </summary>
        public static void StopSoftForce(ref DynamicBuffer<SoftForceElement> softForce)
        {
            for (int v = 0; v < 120; v++)
            {
                softForce[v] = float3.zero;
            }
        }


        /// <summary>
        /// instantly kills any forces added to the spring, gradually
        /// easing them back in over 'seconds'.
        /// this is useful when you need a spring to freeze up for a
        /// brief amount of time, then slowly relaxing back to normal.
        /// </summary>
        public static void ForceVelocityFadeIn(ref Spring spring, float seconds)
        {
            spring.VelocityFadeInLength = seconds;
            spring.VelocityFadeInEndTime = Time.time + seconds;
            spring.VelocityFadeInCap = 0.0f;
        }

        /// <summary>
        /// clears all internal forces for the spring
        /// </summary>
        public static void ClearForces(ref Spring spring)
        {
            spring.m_InternalFrames = 0;
            spring.m_InternalSoftForce = float3.zero;
            spring.m_InternalForce = float3.zero;
        }
    }
}