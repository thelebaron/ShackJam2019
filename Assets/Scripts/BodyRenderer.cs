using System;
using System.Collections.Generic;
using Game.Utils;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;

namespace ShackJam
{
    public class BodyRenderer : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        public GameObject EyePrefab;
        public Entity EyePrefabEntity;
        public GameObject LeftEyeSocket;
        public GameObject RightEyeSocket;
        public Entity LeftEyeEntity;
        public Entity RightEyeEntity;

        public GameObject DustTrailPrefab;
        public Entity DustTrailEntity;

        public float BreathingSquashYAmplitude = 0.035f;
        public float BreathingSquashYRate = 1;
        public float BreathingSquashXZAmplitude = 0.065f;
        public float BreathingSquashXZRate = 1;

        public float3 PositionOffset;
        public float3 RotationOffset;

        private NavMeshAgent m_NavMeshAgent;
        [SerializeField] private CartoonBody m_CartoonBody;
        [SerializeField] private NonUniformScale m_NonUniformScale;
        [SerializeField] private Translation m_Translation;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            //Debug.Log("converted");
            dstManager.AddComponentData(entity, new CartoonBody
            {
                BreathingSquashYAmplitude = BreathingSquashYAmplitude,
                BreathingSquashYRate = BreathingSquashYRate,
                BreathingSquashXZAmplitude = BreathingSquashXZAmplitude,
                BreathingSquashXZRate = BreathingSquashXZRate,
                PositionOffset = PositionOffset
            });
            dstManager.AddComponentData(entity, new NonUniformScale {Value = new float3(1, 1, 1)});
/*
//eye stuff
        LeftEyeEntity = conversionSystem.GetPrimaryEntity(LeftEyeSocket);
        RightEyeEntity = conversionSystem.GetPrimaryEntity(RightEyeSocket);
        EyePrefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(EyePrefab, World.Active);
        
        var leftEye = dstManager.Instantiate(EyePrefabEntity);
        dstManager.AddComponentData(leftEye, new CartoonEye(LeftEyeEntity));
        var rightEye = dstManager.Instantiate(EyePrefabEntity);
        dstManager.AddComponentData(rightEye, new CartoonEye(RightEyeEntity));
#if UNITY_EDITOR
        dstManager.SetName(LeftEyeEntity, "eyeSocket_L" + LeftEyeEntity);
        dstManager.SetName(RightEyeEntity, "eyeSocket_R" + RightEyeEntity);
        dstManager.SetName(leftEye, "eye_L" + leftEye);
        dstManager.SetName(rightEye, "eye_R" + rightEye);
#endif
        */
            // dust stuff
            /*
            DustTrailEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(DustTrailPrefab, World.Active);
            dstManager.AddComponentData(entity, new DustTrail
            {
                DustPrefabEntity = DustTrailEntity,
                Rate             = 0.25f,
                MinAmount        = 1,
                MaxAmount        = 5,
                m_CurrentTime    = 0
            });*/


            //dstManager.AddComponentData(entity, new Scale());
            //dstManager.AddComponentData(entity, new ScalePivot {Value = new float3()});
            //dstManager.AddComponentData(entity, new ScalePivotTranslation());
            //dstManager.AddComponentData(entity, new CompositeScale());
        }

        private void Awake()
        {
            m_NavMeshAgent = transform.root.GetComponent<NavMeshAgent>();

            m_CartoonBody = new CartoonBody
            {
                BreathingSquashYAmplitude = BreathingSquashYAmplitude,
                BreathingSquashYRate = BreathingSquashYRate,
                BreathingSquashXZAmplitude = BreathingSquashXZAmplitude,
                BreathingSquashXZRate = BreathingSquashXZRate,
                PositionOffset = PositionOffset
            };

            m_Translation.Value = transform.localPosition + Vector3.up * 2;
            m_NonUniformScale.Value = transform.localScale;
        }

        /*
        public void Update()
        {
            ScaleY(m_CartoonBody, ref m_NonUniformScale, ref m_Translation);
            ScaleXZ(m_CartoonBody, ref m_NonUniformScale);

            transform.localPosition = m_Translation.Value + maths.up;
            transform.localScale = m_NonUniformScale.Value;
        }

        private void ScaleY(CartoonBody cartoonBody, ref NonUniformScale nonUniformScale, ref Translation translation)
        {
            var time = Time.time;
            var deltaTime = Time.deltaTime;
            // scale y
            var amplitude = cartoonBody.BreathingSquashYAmplitude;
            var frequency =
                cartoonBody.BreathingSquashYRate /
                2; // * (float3)m_NavMeshAgent.desiredVelocity/1.5f + maths.one/1.5f;;
            var scale = nonUniformScale.Value;
            scale += amplitude * (math.sin(2 * math.PI * frequency * time) -
                                  math.sin(2 * Mathf.PI * frequency * (time - deltaTime))) * maths.up;
            nonUniformScale.Value.y = scale.y;
            translation.Value.y = nonUniformScale.Value.y + cartoonBody.PositionOffset.y;
        }

        private void ScaleXZ(CartoonBody cartoonBody, ref NonUniformScale nonUniformScale)
        {
            var time = Time.time;
            var deltaTime = Time.deltaTime;
            // scale y
            var amplitude = cartoonBody.BreathingSquashXZAmplitude;
            var frequency =
                cartoonBody.BreathingSquashXZRate / 2; //* (float3)m_NavMeshAgent.desiredVelocity/1.5f + maths.one/1.5f;
            var scale = nonUniformScale.Value;
            scale -= amplitude * (math.sin(2 * math.PI * frequency * time) -
                                  math.sin(-2 * Mathf.PI * frequency * (time - deltaTime))) * maths.up;
            nonUniformScale.Value.x = scale.y;
            nonUniformScale.Value.z = scale.y;
        }*/

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(EyePrefab);
            referencedPrefabs.Add(DustTrailPrefab);
        }
    }
}