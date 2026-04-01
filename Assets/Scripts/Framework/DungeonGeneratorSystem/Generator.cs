using System.Collections.Generic;
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
        [SerializeField] private Color[] colors;
        
        private int[,] _grid;
        private List<SpriteRenderer> _cellSprites = new ();
        private Vector2Int _currentPos = Vector2Int.one * 5;
        private Vector2Int _startPos;
        private Vector2Int _endPos;

        private Dictionary<Vector2Int, SpriteRenderer> _cells = new ();

        public void Generate()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            
            _grid = new int[size.x, size.y];
            
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    _grid[x, y] = 0;
                }
            }

            _startPos.x = RandomSeedSystem.GetRandomInt(0, size.x);
            _startPos.y = RandomSeedSystem.GetRandomInt(0, size.y);
            _currentPos = _startPos;
            
            BuildGrid();
            
            for (int i = 0; i < stepAmount; i++)
            {
                Walk();
                _grid[_currentPos.x, _currentPos.y] = 1;
                
                if (!_cellSprites.Contains(_cells[_currentPos]))
                    _cellSprites.Add(_cells[_currentPos]);
                
                if (i != stepAmount - 1)
                    continue;
                
                _endPos = _currentPos;
            }
            
            // seed "0" has the same place
            if (_endPos == _startPos)
                FixEndPosition();

            ColorGrid();
        }
        
        private void Walk()
        {
            CardinalDirections r = EnumExtensions.GetRandomEnumValue<CardinalDirections>(RandomSeedSystem.GetRandom());
            _currentPos += r.GetVector2Int();
            
            if (_currentPos.x < 0)
                _currentPos.x = 0;
            if (_currentPos.y < 0)
                _currentPos.y = 0;
            if (_currentPos.x >= size.x)
                _currentPos.x = size.x - 1;
            if (_currentPos.y >= size.y)
                _currentPos.y = size.y - 1;
        }

        private void LogGrid()
        {
            StringBuilder sb = new ();

            for (int i = 0; i < _grid.GetLength(0); i++)
            {
                for (int j = 0; j < _grid.GetLength(1); j++)
                {
                    sb.Append(_grid[i, j]).Append(" ");
                }
                
                sb.AppendLine();
            }

            Debug.Log(sb.ToString());
        }

        private void BuildGrid()
        {
            _cells.Clear();
            _cellSprites.Clear();

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int pos = new(x, y);
                    
                    if (_cells.ContainsKey(pos))
                        continue;
                    
                    SpriteRenderer sr = Instantiate(cell, (Vector3Int) pos, Quaternion.identity, transform);
                    sr.gameObject.name = $"Cell {pos}";
                    _cells.Add(pos, sr);
                }
            }
        }
        
        private void ColorGrid()
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    if (!_cells.TryGetValue(new (x, y), out SpriteRenderer sr))
                        continue;

                    sr.color = _grid[x, y] == 1 ? colors[1] : colors[0];

                    if (_startPos.x == x && _startPos.y == y)
                        sr.color = colors[2];

                    if (_endPos.x == x && _endPos.y == y)
                        sr.color = colors[3];
                }
            }
            
            Debug.Log(_startPos);
            Debug.Log(_endPos);
        }
        
        private void FixEndPosition()
        {
            if (_cellSprites.Count <= 0)
                return;
                    
            SpriteRenderer a = CollectionExtensions.GetRandomItem(_cellSprites, RandomSeedSystem.GetRandom());
            _endPos = new (Mathf.RoundToInt(a.transform.position.x), Mathf.RoundToInt(a.transform.position.y));
            
            if (_endPos == _startPos)
                FixEndPosition();
        }
    }
}