using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Framework.Extensions;

namespace Framework.DungeonGeneratorSystem
{
    public sealed class Grid
    {
        private readonly Dictionary<Vector2Int, Cell> _cells = new();
        private readonly Vector2Int _size;
 
        public Vector2Int Size => _size;
 
        public Grid(Vector2Int size) => _size = size;

        public IEnumerable<KeyValuePair<Vector2Int, Cell>> AllCells => _cells;
 
        public IEnumerable<KeyValuePair<Vector2Int, Cell>> ActiveCells => _cells.Where(kvp => kvp.Value.Type != CellType.EMPTY);
 
        public bool Contains(Vector2Int pos) => _cells.ContainsKey(pos);
 
        public bool InBounds(Vector2Int pos) => pos.x >= 0 && pos.x < _size.x && pos.y >= 0 && pos.y < _size.y;
 
        public Cell Get(Vector2Int pos) => _cells[pos];
 
        public bool TryGet(Vector2Int pos, out Cell cell) => _cells.TryGetValue(pos, out cell);
 
        public void Set(Vector2Int pos, Cell cell) => _cells[pos] = cell;

        public void Clear() => _cells.Clear();
        
        public void AddDoor(Vector2Int pos, Doors door)
        {
            Cell cellToAddDoor = _cells[pos];
            cellToAddDoor.Doors |= door;
            _cells[pos] = cellToAddDoor;
        }
 
        public void SetCellType(Vector2Int pos, CellType type)
        {
            if (!_cells.TryGetValue(pos, out Cell cellOut))
                return;
 
            cellOut.Type = type;
            cellOut.Renderer.color = type.GetColor();
            _cells[pos] = cellOut;
 
            ShowCellDoors(cellOut);
        }
 
        public void SetCellType(Vector2Int pos, CellType type, Doors doors)
        {
            if (!_cells.TryGetValue(pos, out Cell cellOut))
                return;
 
            cellOut.Type = type;
            cellOut.Doors = doors;
            cellOut.Renderer.color = type.GetColor();
            _cells[pos] = cellOut;
 
            ShowCellDoors(cellOut);
        }
 
        public static int GetDoorCount(Doors doors)
        {
            return Enum.GetValues(typeof(Doors)).Cast<Doors>().Count(dir => doors.HasFlag(dir));
        }
 
        private static void ShowCellDoors(Cell targetCell)
        {
            foreach (Doors dir in Enum.GetValues(typeof(Doors)))
            {
                targetCell.DoorsObject[dir].SetActive(targetCell.Doors.HasFlag(dir));
            }
        }
    }
}