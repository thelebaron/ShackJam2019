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
    public class CopyHybridAiToDotsAiSystem : ComponentSystem
    {
        public EntityQuery ControlledPawnQuery;

        protected override void OnCreate()
        {
            base.OnCreate();
            ControlledPawnQuery = GetEntityQuery(typeof(NavMeshAgent), typeof(Agent));
        }

        protected override void OnUpdate()
        {
            Entities.With(ControlledPawnQuery).ForEach((Entity entity, NavMeshAgent hybridAgent, ref Agent dotsAgent) =>
            {
                if(!hybridAgent.enabled)
                    return;
                // Copy from
                {
                    hybridAgent.destination = dotsAgent.Destination;
                }
                
                // Copy to
                {
                    dotsAgent.Velocity = hybridAgent.velocity;
                }
                
                
                // Some destination data
                if (hybridAgent.remainingDistance <= dotsAgent.StoppingDistance)
                {
                    if (!hybridAgent.hasPath || hybridAgent.velocity.sqrMagnitude == 0f)
                    {
                        // Done
                        dotsAgent.ReachedDestination = true;
                    }
                }
                
                var lastDistanceToTarget = hybridAgent.remainingDistance;
                
                if(hybridAgent.remainingDistance > hybridAgent.stoppingDistance && dotsAgent.Think <= 0)
                {
                    float distanceToTarget = hybridAgent.remainingDistance;
                    if(lastDistanceToTarget - distanceToTarget < 1f)
                    {
                        Vector3 destination = hybridAgent.destination;
                        hybridAgent.ResetPath();
                        hybridAgent.SetDestination(destination);
                        lastDistanceToTarget = distanceToTarget;
                        dotsAgent.Think = 1;
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