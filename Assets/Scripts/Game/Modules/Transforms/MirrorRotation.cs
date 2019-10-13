using Unity.Entities;

namespace Game.Modules.Transforms
{
    public struct MirrorRotation: IComponentData
    {
        public Entity Target;
    }
}