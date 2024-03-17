using System;

namespace TheMazurkaStudio.Utilities.Sensors
{
    [Serializable]
    public enum SeekMethode
    {
        None = 0,
        Raycast = 1,
        LineCast = 2,
        CircleCast = 5,
        BoxCast = 7,
        OverlapSphere = 10,
        Burst = 14,
        Ladder = 18,
    }
}
