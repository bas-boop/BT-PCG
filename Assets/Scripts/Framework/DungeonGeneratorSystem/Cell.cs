using UnityEngine;

namespace Framework.DungeonGeneratorSystem
{
    public struct Cell
    {
        public CellType Type;
        public SpriteRenderer Renderer;

        public Cell(CellType type, SpriteRenderer renderer)
        {
            Type = type;
            Renderer = renderer;
        }
    }
}