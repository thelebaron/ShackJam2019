using Unity.Entities;
using Unity.Mathematics;

namespace Game.Components.Springs
{
    // This describes the number of buffer elements that should be reserved
    // in chunk data for each instance of a buffer. In this case, 8 integers
    // will be reserved (32 bytes) along with the size of the buffer header
    // (currently 16 bytes on 64-bit targets)
    [InternalBufferCapacity(120)]
    public struct SoftForceElement : IBufferElementData
    {
        // These implicit conversions are optional, but can help reduce typing.
        public static implicit operator float3 (SoftForceElement s) { return s.Force; }
        public static implicit operator SoftForceElement (float3 s) { return new SoftForceElement { Force = s }; }
    
        // Actual value each buffer element will store.
        public float3 Force;
    }
}