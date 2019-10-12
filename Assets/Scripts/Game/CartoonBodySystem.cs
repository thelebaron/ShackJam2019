using Game.Modules.Springs;
using Game.Utils;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace ShackJam
{
    
[UpdateInGroup(typeof(PresentationSystemGroup))]
public class CartoonBodySystem : JobComponentSystem
{
    /*
    struct EmitDust : IJobForEachWithEntity<DustTrail, CartoonBody, LocalToWorld>
    {
        public EntityCommandBuffer.Concurrent CommandBuffer;
        public PlayerInput PlayerInput;
        public float deltaTime;
        public uint random;
        public void Execute(Entity entity, int index, ref DustTrail trail, ref CartoonBody c1, ref LocalToWorld l0)
        {
            trail.m_CurrentTime += deltaTime;
            
            var coinflip = new Unity.Mathematics.Random(12345);
            var outcome = coinflip.NextInt(0, 1);
            
            if (trail.m_CurrentTime > trail.Rate || outcome.Equals(1))
            {
                if (!PlayerInput.Move.Equals(float2.zero))
                {
                    var rand = new Unity.Mathematics.Random(12345);
                    float f1 = (float)rand.NextInt(-2, 2);
                    var intRand = rand.NextInt(trail.MinAmount, trail.MaxAmount + 5);
                    
                    for (int i = 0; i < intRand; i++)
                    {
                        var dust = CommandBuffer.Instantiate(index, trail.DustPrefabEntity);
                        var PositionOffset = new float3(0,-0.5f,0);
                        
                        CommandBuffer.SetComponent(index, dust, new Translation{ Value = l0.Position + PositionOffset});
                        var localToWorld = new LocalToWorld
                        {
                            Value = float4x4.TRS(new float3(l0.Position + PositionOffset),
                                quaternion.LookRotationSafe(l0.Forward, math.up()),
                                new float3(0.30f, 0.30f, 0.30f))
                        };
                        
                
                        CommandBuffer.SetComponent(index, dust, new LocalToWorld
                        {
                            Value = localToWorld.Value
                        });
                        
                        var seed = rand.NextUInt(1, 100) + (uint)i;
                        CommandBuffer.AddComponent(index, dust, new Seed{ Value = seed });
                    }

                    trail.m_CurrentTime = 0;

                }
                
            }
        }
    }
    struct AlignEyes : IJobForEachWithEntity<CartoonEye, Translation, Rotation>
    {
        [ReadOnly]public ComponentDataFromEntity<LocalToWorld> LocalToWorldDataFromEntity;
        
        public void Execute(Entity entity, int index, ref CartoonEye eye, ref Translation c1, ref Rotation c2)
        {
            if (LocalToWorldDataFromEntity.Exists(eye.SocketEntity))
            {
                c1.Value = LocalToWorldDataFromEntity[eye.SocketEntity].Position;
            }
        }
    }
    */



    struct SpringJob : IJobForEach<Parent, CartoonBody, PostRotationEulerXYZ, RotationEulerXYZ, LocalToWorld>
    {
        public float deltaTime;
        public float time;
        [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Spring> Spring;
        [ReadOnly] public ComponentDataFromEntity<Agent> Agent;

        public void Execute(ref Parent parent,ref CartoonBody cartoonBody, ref PostRotationEulerXYZ postRotationEulerXyz, ref RotationEulerXYZ rotationEulerXyz, ref LocalToWorld localToWorld)
        {
            if(!Agent.Exists(parent.Value) || !Spring.Exists(cartoonBody.RotationSpring))
                return;
            
            //Debug.Log("blarg");
            
            var agent = Agent[parent.Value];
            var rotationSpring = Spring[cartoonBody.RotationSpring];

            rotationSpring.Damping = 0.1f;
            
            var rotation = quaternion.LookRotation(localToWorld.Forward, maths.up);//math.inverse(math.quaternion(localToWorld.Value));
            var rotfwd = math.mul(rotation, localToWorld.Forward);
            
            
            var localVelocity = math.mul(rotation, agent.Velocity /2 );
            localVelocity.y = 0;
            localVelocity.z = 0;
            // --- pitch & yaw rotational sway ---
            // sway the weapon transform using input and weapon 'weight'
                
            rotationSpring.AddForce( new float3(
                localVelocity.x  * 0.025f,
                localVelocity.y  * -0.025f,
                localVelocity .z * -0.025f));
            
            //postRotationEulerXyz.Value = rotationSpring.State;// + rotationSpring2.State;
            rotationEulerXyz.Value = rotationSpring.State;// + rotationSpring2.State;
            
            //Agent[parent.Value] = agent;
            Spring[cartoonBody.RotationSpring] = rotationSpring;
        }
    }

    struct ScaleBodyJob : IJobForEachWithEntity<CartoonBody, NonUniformScale, Translation>
    {
        public float deltaTime;
        public float time;
        
        public void Execute(Entity entity, int index, ref CartoonBody cartoonBody, ref NonUniformScale nonUniformScale, ref Translation translation)
        {
            ScaleY(cartoonBody, ref nonUniformScale, ref translation);
            ScaleXZ(cartoonBody, ref nonUniformScale);
        }

        private void ScaleY(CartoonBody cartoonBody, ref NonUniformScale nonUniformScale, ref Translation translation)
        {
            // scale y
            var amplitude = cartoonBody.BreathingSquashYAmplitude;
            var frequency = cartoonBody.BreathingSquashYRate;
            var scale = nonUniformScale.Value;
            scale += amplitude * (math.sin(2 * math.PI * frequency * time) -
                                  math.sin(2 * Mathf.PI * frequency * (time - deltaTime))) * maths.up;
            nonUniformScale.Value.y = scale.y;
            translation.Value.y = nonUniformScale.Value.y + cartoonBody.PositionOffset.y;
        }
        
        private void ScaleXZ(CartoonBody cartoonBody, ref NonUniformScale nonUniformScale)
        {
            // scale y
            var amplitude = cartoonBody.BreathingSquashXZAmplitude;
            var frequency = cartoonBody.BreathingSquashXZRate;
            var scale = nonUniformScale.Value;
            scale -= amplitude * (math.sin(2 * math.PI * frequency * time) - math.sin(-2 * Mathf.PI * frequency * (time - deltaTime))) * maths.up;
            nonUniformScale.Value.x = scale.y;
            nonUniformScale.Value.z = scale.y;
        }
    }
    
    
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        /*
        if (!HasSingleton<PlayerInput>())
            return inputDeps;
        
        var emitDustJob = new EmitDust
        {
            deltaTime = Time.deltaTime,
            CommandBuffer = endSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(), 
            PlayerInput = GetSingleton<PlayerInput>(),
            random = (uint)Random.Range(0,12345)
        };
        var emitDustHandle = emitDustJob.Schedule(this, inputDeps);
        endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(emitDustHandle);
        
        var eyeJob = new AlignEyes
        {
            LocalToWorldDataFromEntity = GetComponentDataFromEntity<LocalToWorld>(),
        };
        var eyeHandle = eyeJob.Schedule(this, emitDustHandle);
        */
        var springRotationJob = new SpringJob
        {
            deltaTime = Time.deltaTime,
            time = Time.time,
            Spring = GetComponentDataFromEntity<Spring>(),
            Agent = GetComponentDataFromEntity<Agent>(true)
        }; 
        var springRotationJobHandle = springRotationJob.Schedule(this, inputDeps);
        
        var scaleBodyJob = new ScaleBodyJob
        {
            deltaTime = Time.deltaTime,
            time = Time.time
        };
        return scaleBodyJob.Schedule(this, springRotationJobHandle);
    }

    protected override void OnCreate()
    {
        endSimulationEntityCommandBufferSystem = World.Active.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;
}

}