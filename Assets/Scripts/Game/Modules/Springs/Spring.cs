using Unity.Entities;
using Unity.Mathematics;

namespace Game.Modules.Springs
{
    public struct Spring : IComponentData
    {
        //the target entity to act upon
        public float3 State;
        public float3 Velocity;

        // the static equilibrium of this spring
        public float3 RestState;
        // mechanical strength of the spring
        public float3 Stiffness; // = new float3(0.5f, 0.5f, 0.5f);
        // 'Damping' makes spring velocity wear off as it
        // approaches its rest state
        public float3 Damping; // = new float3(0.75f, 0.75f, 0.75f);
        
        public Spring(float3 restState = new float3(), float minVelocity = 0.0000001f)
        {
            State = float3.zero;
            Velocity = float3.zero;
            RestState = restState;
            Stiffness = new float3(0.5f, 0.5f, 0.5f);
            Damping = new float3(0.75f, 0.75f, 0.75f);
            VelocityFadeInCap = 1.0f;
            VelocityFadeInEndTime = 0.0f;
            VelocityFadeInLength = 0.0f;
            MaxVelocity = 10000.0f;
            MinVelocity = minVelocity;
            MaxState = new float3(10000,  10000,  10000);
            MinState = new float3(-10000, -10000, -10000);
            m_InternalForce = float3.zero;
            m_InternalSoftForce= float3.zero;
            m_InternalFrames = 0;
            m_Stop = false;
            m_StopAndIncludeSoftForce = false;
        }
        
        // force velocity fadein variables
        public float VelocityFadeInCap;// = 1.0f;
        public float VelocityFadeInEndTime;// = 0.0f;
        public float VelocityFadeInLength;// = 0.0f;
        // transform limitations
        public float MaxVelocity;// = 10000.0f;
        public float MinVelocity;// = 0.0000001f;
        public float3 MaxState;// = new float3(10000,  10000,  10000);
        public float3 MinState;// = new float3(-10000, -10000, -10000);
        
        // internal forces
        public void AddForce(float3 force)
        {
            m_InternalForce += force;
        }
        public void AddSoftForce(float3 force, int frames)
        {
            m_InternalFrames    = frames;
            m_InternalSoftForce += force;
        }  
        public bool HasPendingForces()
        {
            return !m_InternalForce.Equals(float3.zero);
        }
        public bool HasPendingSoftForces()
        {
            return !m_InternalSoftForce.Equals(float3.zero);
        }
        public float3 m_InternalForce;
        public float3 m_InternalSoftForce;
        public int m_InternalFrames;
        public bool m_Stop;
        public bool m_StopAndIncludeSoftForce;

        public void Stop(bool includeSoftForce = false)
        {
            m_Stop = true;
            m_StopAndIncludeSoftForce = includeSoftForce;
        }
    }

    public enum UpdateMode
    {
        Position,
        PositionAdditiveLocal,  // adds position in relation to parent transform
        PositionAdditiveGlobal, // adds position in relation to world
        PositionAdditiveSelf,   // adds position in relation to own transform
        Rotation,
        RotationAdditiveLocal,  // rotates in relation to parent transform
        RotationAdditiveGlobal, // rotates in relation to world
        Scale,
        ScaleAdditiveLocal
    }
    
    /*

    public struct PositionSpring : IComponentData
    {
        public Entity Spring;
        public float3 State;
        
        public void AddForce(float3 force)
        {
            Force = force;
        }
        
        public void AddSoftForce(float3 force, int frames)
        {
            Frames    = frames;
            SoftForce = force;
        }
        public float3 Force;
        public float3 SoftForce;
        public int Frames;

        public void ClearForces()
        {
            Frames    = 0;
            SoftForce = float3.zero;
            Force     = float3.zero;
        }
    }
    
    public struct SpringPositionAdditiveLink : IComponentData
    {
        public Entity Spring;
        public void AddForce(float3 force)
        {
            Force = force;
        }
        
        public void AddSoftForce(float3 force, int frames)
        {
            Frames    = frames;
            SoftForce = force;
        }        
        public float3 Force;
        public float3 SoftForce;
        public int Frames;
    }*/
}