using Cinemachine;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    [System.Serializable]
    public struct CameraShakeDefinition
    {
        public CinemachineImpulseDefinition.ImpulseTypes impulseType;
        public CinemachineImpulseDefinition.ImpulseShapes impulseShape;
        
        public float impulseDuration;
        public float dissipationDistance;
        public float dissipationRate;
        public float propagationSpeed;
        public AnimationCurve customImpulseShape;
        public float force;
        
        public void GenerateAt(Vector3 impulseSourcePosition, Vector3 velocity)
        {
            var impulse = new CinemachineImpulseDefinition()
            {
                m_ImpulseChannel = 1,
                m_CustomImpulseShape = customImpulseShape,
                m_DissipationRate = dissipationRate,
                m_PropagationSpeed = propagationSpeed,
                m_DissipationDistance = dissipationDistance,
                m_ImpulseDuration = impulseDuration,
                m_ImpulseType = impulseType,
                m_ImpulseShape = impulseShape
            };
            
            impulse.CreateEvent(impulseSourcePosition, velocity * force);
        }
        
        public static CameraShakeDefinition DefaultShake => new()
        {
            customImpulseShape = new AnimationCurve(),
            dissipationDistance = 100,
            dissipationRate = 0.25f,
            impulseDuration = 1.5f,
            impulseShape = CinemachineImpulseDefinition.ImpulseShapes.Rumble,
            impulseType = CinemachineImpulseDefinition.ImpulseTypes.Uniform,
            propagationSpeed = 343f,
        };
    }
}
