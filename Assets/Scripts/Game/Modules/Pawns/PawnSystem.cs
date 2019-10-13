using ShackJam;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PawnSystem : ComponentSystem
{
    private EntityQuery m_PawnQuery;
    private float3 mousepos;
    protected override void OnCreate()
    {
        base.OnCreate();
        m_PawnQuery = GetEntityQuery( typeof(Agent), typeof(Pawn), typeof(Translation));
        
    }

    protected override void OnUpdate()
    {
        Entities.With(m_PawnQuery).ForEach((Entity entity, Pawn pawn, ref Translation translation) =>
            {
                //translation.Value = pawn.transform.position;
            });
    }

}