using ShackJam;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace ShackJam
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public class FeetSystem : ComponentSystem
    {
        public EntityQuery ControlledPawnQuery;
        private float3 mousepos;

        protected override void OnCreate()
        {
            base.OnCreate();
            ControlledPawnQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadWrite<FeetController>()
                }
            });
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.deltaTime;
            Entities.With(ControlledPawnQuery).ForEach((Entity entity, FeetController feet, NavMeshAgent agent) =>
            {
                feet.timer += deltaTime;
                
                var dista = math.distance((float3)feet.TargetLeft.position/* + Position()*/, feet.LeftFoot.transform.position);
                feet.currentDistanceL = dista;
                if (dista > feet.maxDistance || agent.velocity.magnitude <= feet.minVelocity)
                {
                    if (feet.timer >feet.maxTime && feet.switchFoot)
                        feet.LeftFoot.transform.position = (float3) feet.TargetLeft.position;// + Position();
                }
                var distb = math.distance((float3)feet.TargetRight.position/* + Position()*/, feet.RightFoot.transform.position);
                if (distb > feet.maxDistance || agent.velocity.magnitude <= feet.minVelocity)
                {
                    if (feet.timer > feet.maxTime && !feet.switchFoot)
                        feet.RightFoot.transform.position = (float3) feet.TargetRight.position;// + Position();
                }
                
                feet.LeftLegLineRenderer.SetPosition(0, feet.LeftHipSocket.transform.position);
                feet.LeftLegLineRenderer.SetPosition(1, feet.LeftFoot.transform.position);
                feet.RightLegLineRenderer.SetPosition(0, feet.RightHipSocket.transform.position);
                feet.RightLegLineRenderer.SetPosition(1, feet.RightFoot.transform.position);
                
                if (feet.timer > feet.maxTime)
                {
                    feet.timer = 0;
                    feet.switchFoot = !feet.switchFoot;
                }
            });
        }

    }
}