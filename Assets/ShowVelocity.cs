using System.Collections;
using System.Collections.Generic;
using Game.Modules.Hybrid;
using ShackJam;
using Unity.Entities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class ShowVelocity : EntityBehaviour
{
    public float Velocity;
    
    // Update is called once per frame
    void Update()
    {
        if (EntityManager.HasComponent<Velocity>(Entity))
        {
            var velocity = EntityManager.GetComponentData<Velocity>(Entity);
            Velocity = math.length(velocity.Value);
        }
    }

}
