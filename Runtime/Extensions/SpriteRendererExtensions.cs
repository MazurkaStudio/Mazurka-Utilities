using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    public static class SpriteRendererExtensions
    {
        public static void Fade(this SpriteRenderer spriteRenderer, float value)
        {
            var color = spriteRenderer.color;
            color.a = value;
            spriteRenderer.color = color;
        }
    }
}
