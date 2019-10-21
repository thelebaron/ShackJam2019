using Unity.Entities;
using UnityEngine;

namespace ShackJam
{
    public class MouseInputAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponent<UserInput>(entity);
            dstManager.AddComponent<PlayerInputData>(entity);
            dstManager.AddComponent<MenuState>(entity);
        }
    }
}