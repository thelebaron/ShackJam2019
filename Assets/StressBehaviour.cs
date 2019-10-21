using System;
using System.Collections;
using System.Collections.Generic;
using Game.Modules.Hybrid;
using ShackJam;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;

public class StressBehaviour :  EntityBehaviour
{
    public bool isStressed;

    private NavMeshAgent NavMeshAgent;
    private LerpMaterial LerpMaterial;
    private List<LerpMaterial> LerpMaterials = new List<LerpMaterial>();
    private Pawn pawn;
    
    private MouthController mouth;
    
    [SerializeField] private StressType m_StressType;
    [SerializeField] private float wanderDistance = 2f;

    
    private void Start()
    {
        mouth = GetComponent<MouthController>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        var list = GetComponentsInChildren<LerpMaterial>();
        foreach (LerpMaterial lerp in list)
        {
            LerpMaterials.Add(lerp);
        }
        
        pawn = GetComponent<Pawn>();
    }

    public void Stress()
    {
        if(isStressed)
            return;

        pawn.freeze = false;
        isStressed = true;
        
        foreach (var lerp in LerpMaterials)
        {
            lerp.enabled = true;
        }

        if (m_StressType == StressType.Wander || m_StressType == StressType.WanderAndSick)
        {
            NavMeshAgent.enabled = true;
            World.Active.EntityManager.AddComponentData(Entity, new WanderTag{ Distance = wanderDistance});
        }

        if (mouth != null)
            mouth.OpenSesame();
        
        if (m_StressType == StressType.Sick || m_StressType == StressType.WanderAndSick)
        {
            if (mouth != null)
                mouth.Vomiting = true;
        }
    }
    
}

public enum StressType
{
    Wander,Nothing,WanderAndSick,Sick

}
