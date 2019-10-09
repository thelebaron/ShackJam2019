using System;
using Unity.Entities;
using Unity.Mathematics;

namespace ShackJam
{
    [Serializable]
    public struct CartoonBody : IComponentData
    {
        public float BreathingSquashYAmplitude;// = 0.085f;
        public float BreathingSquashYRate;// = 1;
        public float BreathingSquashXZAmplitude;// = 0.1085f;
        public float BreathingSquashXZRate;// = 1;
        public float3 PositionOffset;
        public Entity RotationSpring;
    }

}