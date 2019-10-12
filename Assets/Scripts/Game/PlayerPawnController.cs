using ShackJam;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PlayerPawnController : ComponentSystem
{
    public EntityQuery ControlledPawnQuery;
    private float3 mousepos;
    protected override void OnCreate()
    {
        base.OnCreate();
        ControlledPawnQuery = GetEntityQuery(typeof(DirectControlTag), typeof(PawnTag), typeof(Agent), typeof(PawnControllerAuthoring));
        
    }

    protected override void OnUpdate()
    {
        
        bool hasSingleton = HasSingleton<PlayerInputData>();
        if (hasSingleton)
        {
            var singletonMousePos = GetSingleton<PlayerInputData>();
            mousepos = singletonMousePos.CurrentMousePosition;
        }

        
        
        Entities.With(ControlledPawnQuery).ForEach((Entity entity, PawnControllerAuthoring pawn, ref Agent agent) =>
        {
            agent.SetDestination(mousepos);
            
        });
    }

}