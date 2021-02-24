﻿using UnityEngine;

namespace Egsp.Extensions.Primitives
{
    public static class ColorExtensions
    {
        public static Color Random()
        {
            return new Color(
                UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f));
        }

        public static Color Random(this Color[] colors)
        {
            if(colors.Length == 0)
                return Color.gray;
            
            return colors[UnityEngine.Random.Range(0, colors.Length)];
        }
    }
}