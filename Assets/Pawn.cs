using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace ShackJam
{
    public struct DirectControlTag : IComponentData
    {
        
    }
    
    public struct PawnTag : IComponentData
    {
        
    }

    public struct Agent : IComponentData
    {
        public float3 Destination;
        //public float3 NavmeshRealDestination;
        public float StoppingDistance;
        public bool ReachedDestination;
        public bool IsStopped;

        public void SetDestination(float3 destinaton)
        {
            Destination = destinaton;
            ReachedDestination = false;
        }
    }
    
    [RequiresEntityConversion]
    [SelectionBase]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Pawn : MonoBehaviour, IConvertGameObjectToEntity
    {
        public NavMeshAgent NavmeshAgent
        {
            get { return m_NavMeshAgent; }
            set => m_NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        private NavMeshAgent m_NavMeshAgent;

        public void Awake()
        {
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            NavmeshAgent = GetComponent<NavMeshAgent>();
            
            dstManager.AddComponent<PawnTag>(entity);
            dstManager.AddComponent<DirectControlTag>(entity);
            dstManager.AddComponentData(entity, new Agent
            {
                Destination = transform.position,
                StoppingDistance = NavmeshAgent.stoppingDistance,
                ReachedDestination = false,
                IsStopped = false
            });
            dstManager.AddComponentObject(entity, this);
            dstManager.AddComponentObject(entity, NavmeshAgent);
        }
    }
}