using Cinemachine;
using UnityEngine;

namespace  TheMazurkaStudio.Utilities
{
    /// <summary>
    /// This script allow you to ovveride cinemachine blend in runtime, u just need use SetNextBlend(), or Clear if you want .. clear.
    /// </summary>
    public static class CinemachineBlendManager
    {
        private static CinemachineBlendDefinition? nextBlend;

        static CinemachineBlendManager()
        {
            CinemachineCore.GetBlendOverride = GetBlendOverrideDelegate;
        }

        public static void ClearNextBlend()
        {
            nextBlend = null;
        }

        public static void SetNextBlend(CinemachineBlendDefinition blend)
        {
            nextBlend = blend;
        }

        public static CinemachineBlendDefinition GetBlendOverrideDelegate(ICinemachineCamera fromVcam, ICinemachineCamera toVcam, CinemachineBlendDefinition defaultBlend, MonoBehaviour owner)
        {
            if (nextBlend.HasValue)
            {
                CinemachineBlendDefinition blend = nextBlend.Value;
                nextBlend = null;
                return blend;
            }
            return defaultBlend;
        }
    }

}

