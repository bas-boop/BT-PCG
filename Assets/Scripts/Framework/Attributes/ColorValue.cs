using System;
using UnityEngine;

namespace Framework.Attributes
{
    public class ColorValue : Attribute
    {
        public Color Value { get; }

        public ColorValue(float r, float g, float b) => Value = new (r, g, b);
        
        public ColorValue(float r, float g, float b, float a) => Value = new (r, g, b, a);
    }
}