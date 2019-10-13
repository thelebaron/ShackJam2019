using Unity.Entities;

namespace Game.Modules.Transforms
{
    public struct MirrorLocalToWorld : IComponentData
    {
        public Entity Target;
    }
}