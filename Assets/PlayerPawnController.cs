using ShackJam;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PlayerPawnController : ComponentSystem
{
    public EntityQuery ControlledPawnQuery;

    private float timer;

    protected override void OnCreate()
    {
        base.OnCreate();
        ControlledPawnQuery = GetEntityQuery(typeof(DirectControlTag), typeof(PawnTag), typeof(Agent), typeof(Pawn));
    }

    protected override void OnUpdate()
    {
        timer += Time.deltaTime;

        Entities.With(ControlledPawnQuery).ForEach((Entity entity, Pawn pawn, ref Agent agent) =>
        {
            var randomPos = new float3(Random.Range(-5, 5), Random.Range(1, 1), Random.Range(-5, 5));
            
            
            if (RandomPoint(randomPos, range, out var point)) {
                //Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                if(agent.ReachedDestination)
                    agent.SetDestination(randomPos);
            }
            
            

            
        });
    }
    
    public float range = 10.0f;
    
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