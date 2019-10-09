using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ShackJam
{
    public struct TransformationsToTransform : IComponentData
    {
        
    }
    
    public class CopyEntityTransformationsToTransform: ComponentSystem
    {
        public EntityQuery MouseInputQuery;

        protected override void OnCreate()
        {
            base.OnCreate();
            MouseInputQuery = GetEntityQuery(typeof(Translation), typeof(Rotation), typeof(NonUniformScale), typeof(TransformationsToTransform), typeof(Transform));
        }

        protected override void OnUpdate()
        {
            Entities.With(MouseInputQuery).ForEach((Entity entity, Transform transform, ref Translation translation, ref Rotation rotation, ref NonUniformScale nonUniformScale) =>
                {
                    transform.localPosition = translation.Value;
                    transform.localRotation = rotation.Value;
                    transform.localScale = nonUniformScale.Value;

                });
        }
    }
}