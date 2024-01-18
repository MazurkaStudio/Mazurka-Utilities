using System;
using Unity.Mathematics;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    [Serializable]
    public struct BoundsIntTwoDimension
    {
        public BoundsIntTwoDimension(int2 center, int2 size)
        {
            this.center = center;
            this.size = size;

            this.halfSize = (size / 2);
            
            this.min = center - halfSize;
            this.max = center + halfSize;
            
            this.minX = min.x;
            this.minY = min.y;
            this.maxX = max.x;
            this.maxY = max.y;
            
            if (size.x % 2 != 0 || size.y % 2 != 0)
            {
                this.hasBeenCreated = false;
                UnityEngine.Debug.LogError( "Size must be a multiple of 2 in both dimensions.");
            }
            else this.hasBeenCreated = true;
        }

        public BoundsIntTwoDimension(int minX, int maxX, int minY, int maxY)
        {
            
            this.minX = minX;
            this.minY = minY;
            this.maxX = maxX;
            this.maxY = maxY;
            
            this.min = new int2(minX, minY);
            this.max = new int2(maxX, maxY);
            
            this.size = new int2(maxX - minX, maxY - minY);
            this.halfSize = size / 2;
            this.center = new int2(minX + halfSize.x, minY + halfSize.y);
            
            if ((maxX - minX) % 2 != 0 || (maxY - minY) % 2 != 0)
            {
                this.hasBeenCreated = false;
                UnityEngine.Debug.LogError( "Size must be a multiple of 2 in both dimensions.");
            }
            else this.hasBeenCreated = true;
        }

        public readonly bool hasBeenCreated;
        public readonly int2 center;
        public readonly int2 size;
        public readonly int2 halfSize;
        public readonly int2 min;
        public readonly int2 max;
        public readonly int minX;
        public readonly int minY;
        public readonly int maxX;
        public readonly int maxY;
        
        
        
        
        public bool Contains(float2 position) => position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY; //todo : <= max or < max ?
        public bool Contains(Vector2 position) => position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY; //todo : <= max or < max ?
        public bool Contains(Vector3 position) => position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY; //todo : <= max or < max ?
        public bool Contains(int2 position) => position.x >= minX && position.x < maxX && position.y >= minY && position.y < maxY;
        public bool Contains(Vector2Int position) => position.x >= minX && position.x < maxX && position.y >= minY && position.y < maxY;
        
        public float2 NearPosition(float2 pos) => new(Mathf.Clamp(pos.x, minX, maxX), Mathf.Clamp(pos.y, minY, maxY));
        public float2 NearPosition(Vector2Int pos) => new(Mathf.Clamp(pos.x, minX, maxX), Mathf.Clamp(pos.y, minY, maxY));
        public Vector2 NearPosition(Vector2 pos) => new(Mathf.Clamp(pos.x, minX, maxX), Mathf.Clamp(pos.y, minY, maxY));
        public Vector3 NearPosition(Vector3 pos) => new(Mathf.Clamp(pos.x, minX, maxX), Mathf.Clamp(pos.y, minY, maxY));
        
        public int2 NearPositionInt(int2 pos) => new(math.clamp(pos.x, minX, maxX), math.clamp(pos.y, minY, maxY));
        public int2 NearPositionInt(float2 pos) => new(Mathf.RoundToInt(math.clamp(pos.x, minX, maxX)), Mathf.RoundToInt(math.clamp(pos.y, minY, maxY)));
        public int2 NearPositionInt(Vector2Int pos) => new(math.clamp(pos.x, minX, maxX), math.clamp(pos.y, minY, maxY));
        public int2 NearPositionInt(Vector2 pos) => new(Mathf.RoundToInt(math.clamp(pos.x, minX, maxX)), Mathf.RoundToInt(math.clamp(pos.y, minY, maxY)));
        public int2 NearPositionInt(Vector3 pos) => new(Mathf.RoundToInt(math.clamp(pos.x, minX, maxX)), Mathf.RoundToInt(math.clamp(pos.y, minY, maxY)));


        public readonly void Debug(float duration)
        {
            UnityEngine.Debug.DrawLine(new Vector3(minX,minY), new Vector3(maxX, minY), Color.green, duration);
            UnityEngine.Debug.DrawLine(new Vector3(minX,minY), new Vector3(minX, maxY), Color.green, duration);
            UnityEngine.Debug.DrawLine(new Vector3(maxX,maxY), new Vector3(maxX, minY), Color.green, duration);
            UnityEngine.Debug.DrawLine(new Vector3(maxX,maxY), new Vector3(minX, maxY), Color.green, duration);
        }
    }
}
