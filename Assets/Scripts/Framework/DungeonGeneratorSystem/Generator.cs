using System.Text;
using UnityEngine;

using Framework.Extensions;

namespace Framework.DungeonGeneratorSystem
{
    public sealed class Generator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer cell;
        [SerializeField] private Vector2Int size = Vector2Int.one * 20;
        [SerializeField] private int stepAmount = 5;
        [SerializeField] private Color[] colors;
        
        private int[,] _grid;
        private Vector2Int _currentPos = Vector2Int.one * 5;

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

            _currentPos.x = RandomSeedSystem.GetRandomInt(0, size.x);
            _currentPos.y = RandomSeedSystem.GetRandomInt(0, size.y);

            Debug.Log($"{_grid.GetLength(0)} - {_grid.GetLength(1)}");
            
            for (int i = 0; i < stepAmount; i++)
            {
                Walk();
                _grid[_currentPos.x, _currentPos.y] = 1;
            }

            LogGrid();
            ShowGrid();
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

        private void ShowGrid()
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    SpriteRenderer sr = Instantiate(cell, new (x, y, 0), Quaternion.identity, transform);
                    sr.color = _grid[x, y] == 1 ? colors[1] : colors[0];
                }
            }
        }
    }
}