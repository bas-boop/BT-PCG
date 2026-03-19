using System;
using UnityEngine;

namespace Framework.Visuals
{
    public static class SpriteMaker
    {
        private const int SIZE = 100;
        private const int BORDER_THICKNESS = 12;
        private const float BORDER_COLOR_DARKNESS = 0.4f;
        
        private static readonly Color clear = Color.clear;
        private static readonly Color[] pixels = new Color[SIZE * SIZE];
        
        public static void MakeSprite(SpriteRenderer spriteRenderer, ShapeType shapeType, Color color)
        {
            Texture2D tex = new (SIZE, SIZE);
            Color outlineColor = new (
                color.r * BORDER_COLOR_DARKNESS,
                color.g * BORDER_COLOR_DARKNESS,
                color.b * BORDER_COLOR_DARKNESS,
                color.a
            );
            
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = clear;
            
            tex.SetPixels(pixels);

            switch (shapeType)
            {
                case ShapeType.SQUARE:
                    DrawSquare(SIZE, BORDER_THICKNESS, tex, color, outlineColor);
                    break;

                case ShapeType.TRIANGLE:
                    DrawTriangle(SIZE, BORDER_THICKNESS, tex, color, outlineColor);
                    break;

                case ShapeType.CIRCLE:
                    DrawCircle(SIZE, BORDER_THICKNESS, tex, color, outlineColor);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(shapeType), shapeType, null);
            }

            tex.Apply();
            spriteRenderer.sprite = Sprite.Create(tex, new (0, 0, SIZE, SIZE), Vector2.one * 0.5f);
        }

        #region Draw
        
        private static void DrawSquare(
            int size,
            int borderThickness,
            Texture2D tex,
            Color fillColor,
            Color outlineColor)
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (x < borderThickness 
                        || x >= size - borderThickness 
                        || y < borderThickness 
                        || y >= size - borderThickness)
                        tex.SetPixel(x, y, outlineColor);
                    else
                        tex.SetPixel(x, y, fillColor);
                }
            }
        }
        
        private static void DrawTriangle(
            int size,
            int borderThickness,
            Texture2D tex,
            Color fillColor,
            Color outlineColor)
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x <= size - y - 1; x++)
                {
                    bool isBorder = x < borderThickness 
                                    || y < borderThickness
                                    || x >= size - y - borderThickness;
                    
                    tex.SetPixel(x, y, isBorder ? outlineColor : fillColor);
                }
            }
        }
        
        private static void DrawCircle(
            int size,
            int borderThickness,
            Texture2D tex,
            Color fillColor,
            Color outlineColor)
        {
            int center = size / 2;
            float outerRadius = size / 2f;
            float innerRadius = outerRadius - borderThickness;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dx = x - center;
                    float dy = y - center;
                    float dist = Mathf.Sqrt(dx * dx + dy * dy);

                    if (dist <= outerRadius)
                        tex.SetPixel(x, y, dist >= innerRadius ? outlineColor : fillColor);
                }
            }
        }
        
        #endregion
    }
}