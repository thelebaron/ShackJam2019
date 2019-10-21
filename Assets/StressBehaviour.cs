using System;
using System.Collections;
using System.Collections.Generic;
using Game.Modules.Hybrid;
using ShackJam;
using Unity.Entities;
using UnityEngine;

public class StressBehaviour :  EntityBehaviour
{
    public bool isStressed;

    private LerpMaterial LerpMaterial;
    private List<LerpMaterial> LerpMaterials = new List<LerpMaterial>();
    private Pawn pawn;
    
    private MouthController mouth;
    
    [SerializeField] private StressType m_StressType;
    [SerializeField] private float wanderDistance = 2f;

    private void Start()
    {
        mouth = GetComponent<MouthController>();
        
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

        if (m_StressType == StressType.Wander)
        {
            World.Active.EntityManager.AddComponentData(Entity, new WanderTag{ Distance = wanderDistance});
        }

        if (mouth != null)
            mouth.OpenSesame();
    }
    
}

public enum StressType
{
    Wander,

}
