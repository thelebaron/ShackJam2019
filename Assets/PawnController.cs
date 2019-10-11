using System;
using System.Collections.Generic;
using Game.Components.Springs;
using Game.Modules.Springs;
using Game.Utils;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
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
        public float3 Velocity;
        
        public float Think;

        public void SetDestination(float3 destinaton)
        {
            Destination = destinaton;
            ReachedDestination = false;
        }
    }
    
    [RequiresEntityConversion]
    [SelectionBase]
    [RequireComponent(typeof(NavMeshAgent),typeof(ConvertToEntity))]
    public class PawnController : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        public bool directControl;
        public BodyRenderer BodyRenderer { get; set; }

        public NavMeshAgent NavmeshAgent
        {
            get { return m_NavMeshAgent; }
            set => m_NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        private NavMeshAgent m_NavMeshAgent;

        

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            NavmeshAgent = GetComponent<NavMeshAgent>();
            BodyRenderer = GetComponentInChildren<BodyRenderer>();
            
            dstManager.AddComponent<PawnTag>(entity);
            if(directControl)
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
            dstManager.AddComponentData(entity, new CopyTransformFromGameObject());
            
            // Do conversion for the toon renderer
            ConvertBodyRenderer(entity, dstManager, conversionSystem);
        }

        private void ConvertBodyRenderer(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var rotationPivotSpringEntity = dstManager.CreateEntity();
            #if UNITY_EDITOR
            dstManager.SetName(rotationPivotSpringEntity, "RotationSpring");
            #endif
            dstManager.AddComponentData(rotationPivotSpringEntity, new Spring(math.radians(BodyRenderer.RotationOffset), 0.00001f));
            dstManager.AddBuffer<SoftForceElement>(rotationPivotSpringEntity);
            
            
            var toonrenderEntity = dstManager.CreateEntity();//;conversionSystem.GetPrimaryEntity(BodyRenderer.gameObject));
#if UNITY_EDITOR
            dstManager.SetName(toonrenderEntity, "ToonBody");
#endif
            dstManager.AddComponentData(toonrenderEntity, new Parent{ Value = entity});
            dstManager.AddComponentData(toonrenderEntity, new LocalToParent());
            dstManager.AddComponentData(toonrenderEntity, new TransformationsToTransform());
            dstManager.AddComponentObject(toonrenderEntity, BodyRenderer.transform);
            
            
            dstManager.AddComponentData(toonrenderEntity, new Rotation{ Value = quaternion.identity });
            dstManager.AddComponentData(toonrenderEntity, new CompositeRotation());
            dstManager.AddComponentData(toonrenderEntity, new RotationEulerXYZ());
            //dstManager.AddComponentData(toonrenderEntity, new PostRotation());
            dstManager.AddComponentData(toonrenderEntity, new PostRotationEulerXYZ());
            
            dstManager.AddComponentData(toonrenderEntity, new Translation {Value = (float3) BodyRenderer.transform.localPosition + maths.up * BodyRenderer.PositionOffset});
            dstManager.AddComponentData(toonrenderEntity, new NonUniformScale {Value = new float3(1, 1, 1)});
            dstManager.AddComponentData(toonrenderEntity, new LocalToWorld());
            dstManager.AddComponentData(toonrenderEntity, new CartoonBody
            {
                BreathingSquashYAmplitude = BodyRenderer.BreathingSquashYAmplitude,
                BreathingSquashYRate = BodyRenderer.BreathingSquashYRate,
                BreathingSquashXZAmplitude = BodyRenderer.BreathingSquashXZAmplitude,
                BreathingSquashXZRate = BodyRenderer.BreathingSquashXZRate,
                PositionOffset = BodyRenderer.PositionOffset,
                RotationSpring = rotationPivotSpringEntity
            });
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            //referencedPrefabs.Add();
        }
    }
}