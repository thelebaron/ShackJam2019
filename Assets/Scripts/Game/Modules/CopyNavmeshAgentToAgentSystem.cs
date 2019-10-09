using ShackJam;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Modules
{
    public class CopyNavmeshAgentToAgentSystem : ComponentSystem
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
                // Copy from
                navMeshAgent.destination = agent.Destination;
                
                // Copy to
                agent.Velocity = navMeshAgent.velocity;
                
                
                // Some destination data
                if (navMeshAgent.remainingDistance <= agent.StoppingDistance)
                {
                    if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        // Done
                        agent.ReachedDestination = true;
                    }
                }
                
                float lastDistanceToTarget = navMeshAgent.remainingDistance;
                
                if(navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && agent.Think <= 0)
                {
                    float distanceToTarget = navMeshAgent.remainingDistance;
                    if(lastDistanceToTarget - distanceToTarget < 1f)
                    {
                        Vector3 destination = navMeshAgent.destination;
                        navMeshAgent.ResetPath();
                        navMeshAgent.SetDestination(destination);
                        lastDistanceToTarget = distanceToTarget;
                        agent.Think = 1;
                    }
                }
                //agent.NavmeshRealDestination = navMeshAgent.pathEndPosition;
            });
        }
    }

    public class AgentSystem : JobComponentSystem
    {
        [BurstCompile]
        private struct AgentReachedDestinationJob : IJobForEach<Translation, Agent>
        {
            public float DeltaTime;
            public void Execute(ref Translation translation, ref Agent agent)
            {
                agent.Think -= DeltaTime;
                //var dist = math.distance(translation.Value, agent.Destination);
                //if (dist <= agent.StoppingDistance)
                //agent.ReachedDestination = true;
            }
        }
    
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var handle =  new AgentReachedDestinationJob
            {
                DeltaTime = Time.deltaTime
            }.Schedule(this, inputDeps);

            return handle;
        }
    }
}