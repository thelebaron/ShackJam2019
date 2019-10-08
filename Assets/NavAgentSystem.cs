using ShackJam;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NavMeshAgentToAgentSystem : ComponentSystem
{
    public EntityQuery ControlledPawnQuery;

    private float timer;

    protected override void OnCreate()
    {
        base.OnCreate();
        ControlledPawnQuery = GetEntityQuery(typeof(NavMeshAgent), typeof(Agent));
    }

    protected override void OnUpdate()
    {
        timer += Time.deltaTime;

        Entities.With(ControlledPawnQuery).ForEach((Entity entity, NavMeshAgent navMeshAgent, ref Agent agent) =>
            {
                navMeshAgent.destination = agent.Destination;
                
                if (navMeshAgent.remainingDistance <= agent.StoppingDistance)
                {
                    if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        // Done
                        agent.ReachedDestination = true;
                    }
                }
                //agent.NavmeshRealDestination = navMeshAgent.pathEndPosition;
            });
    }
}

public class AgentSystem : JobComponentSystem
{
    private struct AgentReachedDestinationJob : IJobForEach<Translation, Agent>
    {
        public void Execute(ref Translation translation, ref Agent agent)
        {
            var dist = math.distance(translation.Value, agent.Destination);
            //if (dist <= agent.StoppingDistance)
                //agent.ReachedDestination = true;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        return new AgentReachedDestinationJob().Schedule(this, inputDeps);
    }
}