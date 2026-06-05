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
                    _cells[pos] = new (CellType.EMPTY, sr);
                }
            }
        }

        private void RandomWalk()
        {
            for (int i = 0; i < stepAmount; i++)
            {
                Walk();
                Cell current = _cells[_currentPos];

                if (current.Type == CellType.EMPTY)
                    SetCellType(_currentPos, CellType.NORMAL);

                if (i == stepAmount - 1)
                    _endPos = _currentPos;
            }
        }

        private void Walk()
        {
            CardinalDirections r = EnumExtensions.GetRandomEnumValue<CardinalDirections>(RandomSeedSystem.GetRandom());
            _currentPos += r.GetVector2Int();

            _currentPos.x = Mathf.Clamp(_currentPos.x, 0, size.x - 1);
            _currentPos.y = Mathf.Clamp(_currentPos.y, 0, size.y - 1);
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
            foreach (Vector2Int pos in _cells.Keys.ToList())
            {
                CellType type = pos == _startPos ? CellType.START
                                : pos == _endPos ? CellType.END
                                : _cells[pos].Type;

                SetCellType(pos, type);
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