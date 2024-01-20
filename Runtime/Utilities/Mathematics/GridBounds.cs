using System;
using Unity.Mathematics;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    [Serializable]
    public struct GridBounds
    {
        public GridBounds(int2 center, int2 size)
        {
            this.center = center;
            
            if (size.x % 2 != 0)
            {
                size.x++;
                UnityEngine.Debug.LogError( "Size must be a multiple of 2 in both dimensions.");
            }

            if (size.y % 2 != 0)
            {
                size.y++;
                UnityEngine.Debug.LogError( "Size must be a multiple of 2 in both dimensions.");
            }
            
            this.size = size;

            var half = size / 2;
            
            min = center - half;
            max = center + half;

            IsInitialized = true;
        }

        [SerializeField] private int2 center;
        [SerializeField] private int2 size;
        [SerializeField, HideInInspector] private int2 min;
        [SerializeField, HideInInspector] private int2 max;
        
        public bool IsInitialized { get; private set; }
        
        public int2 Center
        {
            get => center;
            set
            {
                center = value;
                min = center - HalfSize;
                max = center + HalfSize;
            }
        }
        public int2 Size
        {
            get => size;
            set
            {
                if (size.x % 2 != 0 || size.y % 2 != 0)
                {
                    UnityEngine.Debug.LogError( "Size must be a multiple of 2 in both dimensions.");
                    return;
                }
                
                size = value;
                
                min = center - HalfSize;
                max = center + HalfSize;
            }
        }

        public int2 HalfSize => size / 2;
        
        public int2 Min => min;
        public int2 Max => max;
        
        public int MinX => min.x;
        public int MinY => min.y;
        public int MaxX => max.x;
        public int MaxY => max.y;
        
        public bool Contains(float2 position) => position.x >= MinX && position.x <= MaxX && position.y >= MinY && position.y <= MaxY; //todo : <= max or < max ?
        public bool Contains(Vector2 position) => position.x >= MinX && position.x <= MaxX && position.y >= MinY && position.y <= MaxY; //todo : <= max or < max ?
        public bool Contains(Vector3 position) => position.x >= MinX && position.x <= MaxX && position.y >= MinY && position.y <= MaxY; //todo : <= max or < max ?
        public bool Contains(int2 position) => position.x >= MinX && position.x < MaxX && position.y >= MinY && position.y < MaxY;
        public bool Contains(Vector2Int position) => position.x >= MinX && position.x < MaxX && position.y >= MinY && position.y < MaxY;
        
        public float2 NearPosition(float2 pos) => new(Mathf.Clamp(pos.x, MinX, MaxX), Mathf.Clamp(pos.y, MinY, MaxY));
        public float2 NearPosition(Vector2Int pos) => new(Mathf.Clamp(pos.x, MinX, MaxX), Mathf.Clamp(pos.y, MinY, MaxY));
        public Vector2 NearPosition(Vector2 pos) => new(Mathf.Clamp(pos.x, MinX, MaxX), Mathf.Clamp(pos.y, MinY, MaxY));
        public Vector3 NearPosition(Vector3 pos) => new(Mathf.Clamp(pos.x, MinX, MaxX), Mathf.Clamp(pos.y, MinY, MaxY));
        
        public int2 NearPositionInt(int2 pos) => new(math.clamp(pos.x, MinX, MaxX), math.clamp(pos.y, MinY, MaxY));
        public int2 NearPositionInt(float2 pos) => new(Mathf.RoundToInt(math.clamp(pos.x, MinX, MaxX)), Mathf.RoundToInt(math.clamp(pos.y, MinY, MaxY)));
        public int2 NearPositionInt(Vector2Int pos) => new(math.clamp(pos.x, MinX, MaxX), math.clamp(pos.y, MinY, MaxY));
        public int2 NearPositionInt(Vector2 pos) => new(Mathf.RoundToInt(math.clamp(pos.x, MinX, MaxX)), Mathf.RoundToInt(math.clamp(pos.y, MinY, MaxY)));
        public int2 NearPositionInt(Vector3 pos) => new(Mathf.RoundToInt(math.clamp(pos.x, MinX, MaxX)), Mathf.RoundToInt(math.clamp(pos.y, MinY, MaxY)));


        public readonly void Debug(float duration)
        {
            UnityEngine.Debug.DrawLine(new Vector3(MinX,MinY), new Vector3(MaxX, MinY), Color.green, duration);
            UnityEngine.Debug.DrawLine(new Vector3(MinX,MinY), new Vector3(MinX, MaxY), Color.green, duration);
            UnityEngine.Debug.DrawLine(new Vector3(MaxX,MaxY), new Vector3(MaxX, MinY), Color.green, duration);
            UnityEngine.Debug.DrawLine(new Vector3(MaxX,MaxY), new Vector3(MinX, MaxY), Color.green, duration);
        }
    }
}
