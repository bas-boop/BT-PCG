using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Framework.DungeonGeneratorSystem.Rules;
using Framework.DungeonGeneratorSystem.Seed;
using Quaternion = UnityEngine.Quaternion;

namespace Framework.DungeonGeneratorSystem
{
    public sealed class Generator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer cell;
        [SerializeField] private Vector2Int size = Vector2Int.one * 20;
        [SerializeField] private int stepAmount = 5;
        [SerializeField] private GenerationRule[] generationRules;
 
        private Grid _grid;
        private Vector2Int _startPos;
        private Vector2Int _endPos;
 
        public void Generate()
        {
            _grid = BuildGrid();
            Walker carver = new(_grid, stepAmount);
            Vector2Int randomStart = new(RandomSeedSystem.GetRandomInt(0, size.x), RandomSeedSystem.GetRandomInt(0, size.y));
 
            carver.Walk(randomStart);
 
            _startPos = carver.StartPos;
            _endPos = carver.EndPos;
 
            ColorGrid();
            ApplyGenerationRules();
        }
 
        private Grid BuildGrid()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
                Destroy(transform.GetChild(i).gameObject);
 
            Grid grid = new(size);
 
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int pos = new(x, y);
 
                    if (grid.Contains(pos))
                        continue;
 
                    SpriteRenderer sr = Instantiate(cell, (Vector3Int)pos, Quaternion.identity, transform);
                    sr.gameObject.name = $"Cell {pos}";
                    grid.Set(pos, new (CellType.EMPTY, 0, sr));
                }
            }
 
            return grid;
        }
 
        private void ColorGrid()
        {
            List<Vector2Int> positions = _grid.AllCells.Select(kvp => kvp.Key).ToList();
 
            foreach (Vector2Int pos in positions)
            {
                CellType type = pos == _startPos
                    ? CellType.START
                    : pos == _endPos
                        ? CellType.END
                        : _grid.Get(pos).Type;
 
                _grid.SetCellType(pos, type, _grid.Get(pos).Doors);
            }
        }
 
        private void ApplyGenerationRules()
        {
            foreach (GenerationRule genRule in generationRules)
            {
                for (int i = 0; i < genRule.amount; i++)
                {
                    genRule.TryPlace(_grid);
                }
            }
        }
    }
}