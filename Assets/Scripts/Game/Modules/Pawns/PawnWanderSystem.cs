using ShackJam;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PawnWanderController : ComponentSystem
{
    private EntityQuery wanderQuery;
    private float3 mousepos;
    protected override void OnCreate()
    {
        base.OnCreate();
        wanderQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[]
            {
                ComponentType.ReadOnly<PawnData>(),
                ComponentType.ReadWrite<Agent>(),
                ComponentType.ReadWrite<Pawn>(),
                ComponentType.ReadOnly<WanderTag>(),

            },
            None = new ComponentType[]
            {
                    ComponentType.ReadOnly<PlayerControlTag>(),
            }
        });
    }

    protected override void OnUpdate()
    {
        Entities.With(wanderQuery).ForEach((Entity entity, Pawn pawn, ref Agent agent, ref WanderTag wanderTag) =>
        {
            //agent.SetDestination(mousepos);
            var randomPos = new float3(
                                Random.Range(-wanderTag.Distance, wanderTag.Distance),
                                Random.Range(wanderTag.Distance, wanderTag.Distance),
                                Random.Range(-wanderTag.Distance, wanderTag.Distance)) 
                            + (float3) pawn.transform.position;
            
            if (RandomPoint(randomPos, _range, out var point)) 
            {
                //Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                if(agent.ReachedDestination)
                    agent.SetDestination(randomPos);
            }
        });
    }

    private readonly float _range = 10.0f;
    
    bool RandomPoint(Vector3 center, float range, out Vector3 result) 
    {
        for (int i = 0; i < 30; i++) 
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}