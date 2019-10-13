using System;
using Unity.Entities;
using UnityEngine;

namespace Game.Modules.Hybrid
{
    public abstract class EntityBehaviour : MonoBehaviour, IConvertGameObjectToEntity
    {
        public EntityManager EntityManager;
        public Entity Entity;

        protected virtual void Start()
        {
            EntityManager = World.Active.EntityManager;
        }

        public virtual void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            Entity = entity;
        }
    }
}