using Unity.Entities;

namespace Game.Modules.Transforms
{
    public struct MirrorTranslation : IComponentData
    {
        public Entity Target;
    }
}