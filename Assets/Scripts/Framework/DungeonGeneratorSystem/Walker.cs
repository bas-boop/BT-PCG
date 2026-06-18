using System.Collections.Generic;
using UnityEngine;

using Framework.DungeonGeneratorSystem.Seed;
using Framework.Extensions;
using CollectionExtensions = Framework.Extensions.CollectionExtensions;

namespace Framework.DungeonGeneratorSystem
{
    public class Walker
    {
        private readonly Grid _grid;
        private readonly int _stepAmount;
 
        public Vector2Int StartPos { get; private set; }
        public Vector2Int EndPos { get; private set; }
 
        public Walker(Grid grid, int stepAmount)
        {
            _grid = grid;
            _stepAmount = stepAmount;
        }
 
        public void Walk(Vector2Int startPos)
        {
            StartPos = startPos;
            Vector2Int currentPos = startPos;
 
            if (_grid.Get(currentPos).Type == CellType.EMPTY)
                _grid.SetCellType(currentPos, CellType.NORMAL);
 
            for (int i = 0; i < _stepAmount; i++)
            {
                currentPos = Step(currentPos);
 
                if (i == _stepAmount - 1)
                    EndPos = currentPos;
            }
 
            if (EndPos == StartPos)
                FixEndPosition();
        }
 
        private Vector2Int Step(Vector2Int currentPos)
        {
            Vector2Int nextPos;
            CardinalDirections r;
 
            do
            {
                r = EnumExtensions.GetRandomEnumValue<CardinalDirections>(RandomSeedSystem.GetRandom());
                nextPos = currentPos + r.GetVector2Int();
            } while (!_grid.InBounds(nextPos));
 
            Vector2Int prevPos = currentPos;
            currentPos = nextPos;
 
            if (_grid.Get(currentPos).Type == CellType.EMPTY)
                _grid.SetCellType(currentPos, CellType.NORMAL);
 
            _grid.AddDoor(prevPos, r.ToDoor());
            _grid.AddDoor(currentPos, r.ToOppositeDoor());
 
            return currentPos;
        }
 
        private void FixEndPosition()
        {
            List<KeyValuePair<Vector2Int, Cell>> active = System.Linq.Enumerable.ToList(_grid.ActiveCells);
 
            if (active.Count <= 0)
                return;
 
            KeyValuePair<Vector2Int, Cell> picked = CollectionExtensions.GetRandomItem(active, RandomSeedSystem.GetRandom());
            EndPos = picked.Key;
 
            if (EndPos == StartPos)
                FixEndPosition();
        }
    }
}