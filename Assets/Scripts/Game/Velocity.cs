using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ShackJam
{
    public struct Velocity : IComponentData
    {
        public float3 Value;
        public float3 PreviousTranslation;
    }

    public class VelocitySystem : JobComponentSystem
    {
        private EntityQuery m_VelocityQuery;
        protected override void OnCreate()
        {
            base.OnCreate();
            m_VelocityQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadWrite<Velocity>(), 
                    ComponentType.ReadOnly<Translation>(),
                },
                None = new ComponentType[]
                {
                    //ComponentType.ReadOnly<DirectControlTag>(),
                }
            });
        }

        [BurstCompile]
        private struct CalculateVelocity : IJobForEach<Velocity, Translation>
        {
            public float DeltaTime;
            
            public void Execute(ref Velocity v, ref Translation t)
            {
                v.Value = t.Value - v.PreviousTranslation;
                
                v.PreviousTranslation = t.Value;
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var velocityJob = new CalculateVelocity{DeltaTime = Time.deltaTime};
            var velocityHandle = velocityJob.Schedule(this, inputDeps);

            return velocityHandle;
        }
    }
}