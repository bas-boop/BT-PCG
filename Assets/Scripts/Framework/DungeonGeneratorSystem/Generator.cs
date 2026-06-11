using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using Framework.Extensions;
using CollectionExtensions = Framework.Extensions.CollectionExtensions;

namespace Framework.DungeonGeneratorSystem
{
    public sealed class Generator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer cell;
        [SerializeField] private Vector2Int size = Vector2Int.one * 20;
        [SerializeField] private int stepAmount = 5;
        [SerializeField] private int positiveRooms;
        [SerializeField] private int negativeRooms;
        [SerializeField] private int secretRooms = 1;
        [SerializeField] private Color[] colors;
        [SerializeField] private Doors test;

        private readonly Dictionary<Vector2Int, Cell> _cells = new();
        private Vector2Int _currentPos;
        private Vector2Int _startPos;
        private Vector2Int _endPos;

        private IEnumerable<KeyValuePair<Vector2Int, Cell>> ActiveCells => _cells.Where(kvp => kvp.Value.Type != CellType.EMPTY);

        public void Generate()
        {
            Setup();
            RandomWalk();
            
            // fix the end position when also ends at start, example seed "0"
            if (_endPos == _startPos)
                FixEndPosition();

            ColorGrid();

            for (int i = 0; i < negativeRooms; i++)
                PlaceSpecialRoomRandom(CellType.NEGATIVE);

            for (int i = 0; i < positiveRooms; i++)
                PlaceSpecialRoomRandom(CellType.POSITIVE);

            for (int i = 0; i < secretRooms; i++)
                PlaceSpecialRoomByDistance(CellType.SECRET);
        }

        private void Setup()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
                Destroy(transform.GetChild(i).gameObject);

            _cells.Clear();

            _startPos.x = RandomSeedSystem.GetRandomInt(0, size.x);
            _startPos.y = RandomSeedSystem.GetRandomInt(0, size.y);
            _currentPos = _startPos;

            BuildGrid();
        }

        private void BuildGrid()
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int pos = new(x, y);

                    if (_cells.ContainsKey(pos))
                        continue;

                    SpriteRenderer sr = Instantiate(cell, (Vector3Int)pos, Quaternion.identity, transform);
                    sr.gameObject.name = $"Cell {pos}";
                    _cells[pos] = new (CellType.EMPTY, 0, sr);
                }
            }
        }

        private void RandomWalk()
        {
            if (_cells[_currentPos].Type == CellType.EMPTY)
                SetCellType(_currentPos, CellType.NORMAL);

            for (int i = 0; i < stepAmount; i++)
            {
                Walk();

                if (i == stepAmount - 1)
                    _endPos = _currentPos;
            }
        }

        private void Walk()
        {
            Vector2Int nextPos;
            CardinalDirections r;

            do {
                r = EnumExtensions.GetRandomEnumValue<CardinalDirections>(RandomSeedSystem.GetRandom());
                nextPos = _currentPos + r.GetVector2Int();
            } while (nextPos.x < 0 || nextPos.x >= size.x || nextPos.y < 0 || nextPos.y >= size.y);

            Vector2Int prevPos = _currentPos;
            _currentPos = nextPos;

            if (_cells[_currentPos].Type == CellType.EMPTY)
                SetCellType(_currentPos, CellType.NORMAL);

            AddDoor(prevPos, r.ToDoor());
            AddDoor(_currentPos, r.ToOppositeDoor());
        }

        private void AddDoor(Vector2Int pos, Doors door)
        {
            Cell cellToAddDoor = _cells[pos];
            cellToAddDoor.Doors |= door;
            _cells[pos] = cellToAddDoor;
        }

        private void FixEndPosition()
        {
            List<KeyValuePair<Vector2Int, Cell>> active = ActiveCells.ToList();
            
            if (active.Count <= 0)
                return;

            KeyValuePair<Vector2Int, Cell> picked = CollectionExtensions.GetRandomItem(active, RandomSeedSystem.GetRandom());
            _endPos = picked.Key;

            if (_endPos == _startPos)
                FixEndPosition();
        }

        private void ColorGrid()
        {
            CleanupDoors();
            
            foreach (Vector2Int pos in _cells.Keys.ToList())
            {
                CellType type = pos == _startPos ? CellType.START
                                : pos == _endPos ? CellType.END
                                : _cells[pos].Type;

                SetCellType(pos, type, _cells[pos].Doors);
            }
        }

        private void PlaceSpecialRoomRandom(CellType roomType)
        {
            List<KeyValuePair<Vector2Int, Cell>> candidates = ActiveCells.Where(kvp => kvp.Value.Type == CellType.NORMAL).ToList();

            while (true)
            {
                KeyValuePair<Vector2Int, Cell> picked = CollectionExtensions.GetRandomItem(candidates, RandomSeedSystem.GetRandom());

                if (picked.Value.Type != CellType.NORMAL)
                    continue;
                
                SetCellType(picked.Key, roomType);
                break;
            }
        }

        private void PlaceSpecialRoomByDistance(CellType roomType)
        {
            float bestDistance = 0f;
            Vector2Int bestPos = default;
            bool found = false;

            foreach (KeyValuePair<Vector2Int, Cell> kvp in _cells)
            {
                if (kvp.Value.Type != CellType.NORMAL)
                    continue;

                float dist = Vector2Int.Distance(_startPos, kvp.Key);

                if (dist <= bestDistance)
                    continue;
                
                bestDistance = dist;
                bestPos = kvp.Key;
                found = true;
            }

            if (found)
                SetCellType(bestPos, roomType);
        }

        private void SetCellType(Vector2Int pos, CellType type)
        {
            if (!_cells.TryGetValue(pos, out Cell cellOut))
                return;

            cellOut.Type = type;
            cellOut.Renderer.color = colors[(int)type];
            _cells[pos] = cellOut;
            
            ShowCellDoors(cellOut);
        }
        
        private void SetCellType(Vector2Int pos, CellType type, Doors doors)
        {
            if (!_cells.TryGetValue(pos, out Cell cellOut))
                return;

            cellOut.Type = type;
            cellOut.Doors = doors;
            cellOut.Renderer.color = colors[(int)type];
            _cells[pos] = cellOut;
            
            Debug.Log($"{pos} type={type} doors={doors}");
            ShowCellDoors(cellOut);
        }

        private void ShowCellDoors(Cell cell)
        {
            foreach (Doors dir in System.Enum.GetValues(typeof(Doors)))
            {
                cell.DoorsObject[dir].SetActive(cell.Doors.HasFlag(dir));
            }
        }
        
        private void CleanupDoors()
        {
            int removed = 0;
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    Cell cell = _cells[pos];

                    if (cell.Type == CellType.EMPTY) continue;

                    if (HasEmptyNeighbor(pos, CardinalDirections.NORTH)) { RemoveDoor(pos, Doors.NORTH); removed++; }
                    if (HasEmptyNeighbor(pos, CardinalDirections.EAST))  { RemoveDoor(pos, Doors.EAST);  removed++; }
                    if (HasEmptyNeighbor(pos, CardinalDirections.SOUTH)) { RemoveDoor(pos, Doors.SOUTH); removed++; }
                    if (HasEmptyNeighbor(pos, CardinalDirections.WEST))  { RemoveDoor(pos, Doors.WEST);  removed++; }
                }
            }
            Debug.Log($"CleanupDoors removed {removed} doors");
        }

        private bool HasEmptyNeighbor(Vector2Int pos, CardinalDirections dir)
        {
            Vector2Int neighbor = pos + dir.GetVector2Int();

            if (neighbor.x < 0 || neighbor.x >= size.x || neighbor.y < 0 || neighbor.y >= size.y)
                return true;

            return _cells[neighbor].Type == CellType.EMPTY;
        }

        private void RemoveDoor(Vector2Int pos, Doors door)
        {
            Cell cellToRemoveDoor = _cells[pos];
            cellToRemoveDoor.Doors &= ~door;
            _cells[pos] = cellToRemoveDoor;
        }
        
        private void LogGrid()
        {
            StringBuilder sb = new();

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int pos = new(x, y);
                    int val = _cells.TryGetValue(pos, out Cell c) ? (int)c.Type : 0;
                    sb.Append(val).Append(' ');
                }
                sb.AppendLine();
            }

            Debug.Log(sb.ToString());
        }
    }
}