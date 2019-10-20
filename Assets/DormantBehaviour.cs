using Unity.Entities;
using UnityEngine;

public class DormantBehaviour : MonoBehaviour, IConvertGameObjectToEntity
{
    private Rigidbody m_Rigidbody;
    public float delaySimulationTime = 5;
    

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    
    private void Update()
    {
        delaySimulationTime -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(delaySimulationTime>0)
            return;
        
        if (m_Rigidbody.velocity.magnitude > 2)
        {
            Destroy(gameObject);
        }
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent<PhysicsSleep>(entity);
    }
}

public struct PhysicsSleep : IComponentData
{
    
}

/*
public class AwakenPhysicsSystem : JobComponentSystem
{
    //[BurstCompile]
    [RequireComponentTag(typeof(PhysicsSleep))]
    private struct AwakeJob : IJobForEachWithEntity<PhysicsCollider, Translation>
    {
        public Entity Player;
        [ReadOnly] public CollisionWorld CollisionWorld;
        [ReadOnly]public ComponentDataFromEntity<LocalToWorld> LocalToWorldData;
        public EntityCommandBuffer.Concurrent EntityCommandBuffer;


        public void Execute(Entity entity, int index, ref PhysicsCollider c0, ref Translation translation)
        {
            
            
            
            if (LocalToWorldData.Exists(Player))
            {
                
              
                
                
                
                
                if (math.distance(LocalToWorldData[Player].Position, translation.Value) < 3.5f)
                {
                    EntityCommandBuffer.AddComponent(index, entity, new PhysicsVelocity());
                    EntityCommandBuffer.RemoveComponent(index, entity, typeof(PhysicsSleep));
                }
                
            }
        }
    }
    
    [BurstCompile]
    public struct RaycastJob : IJobParallelFor
    {
        [ReadOnly] public CollisionWorld world;
        [ReadOnly] public NativeArray<RaycastInput> inputs;
        public            NativeArray<Unity.Physics.RaycastHit>   results;

        public void Execute(int index)
        {
            Unity.Physics.RaycastHit hit;
            world.CastRay(inputs[index], out hit);
            results[index] = hit;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var awakejob = new AwakeJob
        {
            Player              = GetSingletonEntity<PlayerControlTag>(),
            CollisionWorld = physicsWorldSystem.PhysicsWorld.CollisionWorld,
            LocalToWorldData = GetComponentDataFromEntity<LocalToWorld>(true),
            EntityCommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent()
        };
        
        var awakehandle= awakejob.Schedule(this, inputDeps);
        m_EntityCommandBufferSystem.AddJobHandleForProducer(awakehandle);
        
        return awakehandle;

    }

    private EndSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;
    private BuildPhysicsWorld physicsWorldSystem;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        m_EntityCommandBufferSystem = World.Active.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        physicsWorldSystem = Unity.Entities.World.Active.GetOrCreateSystem<Unity.Physics.Systems.BuildPhysicsWorld>();

    }
}*/