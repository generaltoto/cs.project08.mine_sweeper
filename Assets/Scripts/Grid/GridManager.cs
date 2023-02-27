using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

namespace Grid
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance => _instance ??= FindObjectOfType<GridManager>();

        public List<Vector2Int> FlagPositions => _flagPositions;

        public List<Vector2Int> BombsPositions => _bombsPositions;

        public void Init(int width, int height, int bombsCount)
        {
            _width = width;
            _height = height;
            _bombsCount = bombsCount;
            _board = new Tile[width, height];
            _bombsPositions = new List<Vector2Int>();
            _flagPositions = new List<Vector2Int>();
        }

        public void GenerateBoard()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    _board[x, y] = CreateTileFromPrefab(DEFAULT_PREFAB_NAME, x, y);
                    _board[x, y].Position = new Vector2Int(x, y);
                    _board[x, y].InitWithType(Tile.TileType.EMPTY);
                }
            }
        }

        public void GenerateBombs(Vector2Int forbiddenPos)
        {
            (int x, int y) bombPos = (0, 0);

            for (int i = 0; i < _bombsCount; i++)
            {
                // We generate bombs coordinates until we find an empty spot. 
                // The bomb coordinates are not allowed to be around / be first clicked tile coordinates.
                do
                {
                    bombPos.x = Random.Range(0, _width);
                    bombPos.y = Random.Range(0, _height);
                } while (
                    _board[bombPos.x, bombPos.y].Type == Tile.TileType.BOMB ||
                    IsAroundClickedTile(bombPos.x, bombPos.y, forbiddenPos.x, forbiddenPos.y)
                );

                _board[bombPos.x, bombPos.y].InitWithType(Tile.TileType.BOMB);
                _bombsPositions.Add(new Vector2Int(bombPos.x, bombPos.y));
            }
        }

        public void GenerateCluesAndEmpty()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Tile tile = _board[x, y];

                    if (tile.Type == Tile.TileType.BOMB) continue;

                    tile.ClueCount = GetBombsCountAround(x, y);
                    Tile.TileType type = tile.ClueCount == 0 ? Tile.TileType.EMPTY : Tile.TileType.CLUE;
                    tile.InitWithType(type);
                }
            }
        }

        public void HandleEmptyTileReveal(Tile tile)
        {
            int x = tile.Position.x;
            int y = tile.Position.y;

            // List of all tiles to check, int x, int y and bool isClue.
            // Is the tile is a clue, we don't need to check its adjacent tiles (Item3)
            List<Tuple<int, int, bool>> queue = new List<Tuple<int, int, bool>>() { new(x, y, false) };

            // Reveal the tile we clicked on
            tile.Reveal();

            while (queue.Count > 0)
            {
                // Select the last tile in the queue and remove it from the queue
                Tuple<int, int, bool> curTile = queue.Last();
                queue.RemoveAt(queue.Count - 1);

                // If the tile is a clue, we don't need to check its adjacent tiles
                // The algorithm will reveal all linked empty tiles
                // and also the clue tiles that are directly linked a revealed empty tile
                if (curTile.Item3) continue;

                int posX = curTile.Item1;
                int posY = curTile.Item2;

                foreach (Tile neighbour in GetNeighbours(posX, posY))
                {
                    // If the tile is not a bomb and is not already revealed, we reveal it and add it to the queue
                    if (neighbour.Type != Tile.TileType.BOMB && !neighbour.IsRevealed)
                    {
                        neighbour.Reveal();
                        queue.Add(new Tuple<int, int, bool>(neighbour.Position.x, neighbour.Position.y,
                            neighbour.Type == Tile.TileType.CLUE));
                    }
                }
            }
        }

        public void HandleClueTileReveal(Tile tile)
        {
            if (tile.IsRevealed)
            {
                foreach (Tile neighbour in GetNeighbours(tile.Position.x, tile.Position.y))
                {
                    Debug.Log(tile.Type);
                    switch (neighbour.Type)
                    {
                        case Tile.TileType.BOMB:
                            HandleBombTileReveal(neighbour);
                            break;
                        case Tile.TileType.CLUE:
                            neighbour.Reveal();
                            break;
                        case Tile.TileType.EMPTY:
                            HandleEmptyTileReveal(neighbour);
                            break;
                    }
                }
            }
            else tile.Reveal();
        }

        public void HandleBombTileReveal(Tile tile)
        {
            if (!tile.IsFlagged) tile.Reveal(true);

            foreach (Vector2Int bombPos in _bombsPositions)
            {
                Tile bombTile = _board[bombPos.x, bombPos.y];
                if (!bombTile.IsFlagged) bombTile.Reveal();
            }
        }


        private static GridManager _instance;

        private const string PREFAB_PATH = "Prefabs/";
        private const string DEFAULT_PREFAB_NAME = "default_tile";

        private int _width;
        private int _height;
        private int _bombsCount;
        private Tile[,] _board;
        private List<Vector2Int> _bombsPositions;
        private List<Vector2Int> _flagPositions;

        private List<Tile> GetNeighbours(int x, int y)
        {
            List<Tile> neighbours = new List<Tile>();
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (TileIsInvalid(i, j) || (i == x && j == y)) continue;

                    neighbours.Add(_board[i, j]);
                }
            }

            return neighbours;
        }

        private int GetBombsCountAround(int x, int y)
        {
            int count = 0;
            GetNeighbours(x, y).ForEach(neighbour =>
            {
                if (neighbour.Type == Tile.TileType.BOMB) count++;
            });

            return count;
        }

        private bool IsAroundClickedTile(int x, int y, int tileX, int tileY)
            => GetNeighbours(tileX, tileY).Exists(neighbour => neighbour.Position.x == x && neighbour.Position.y == y);

        private bool TileIsInvalid(int x, int y) => (x < 0 || x >= _width || y < 0 || y >= _height);

        private Tile CreateTileFromPrefab(string prefabName, int x, int y, int z = 0)
        {
            Tile original = Resources.Load<Tile>(PREFAB_PATH + prefabName);
            Vector3 position = new Vector3(x, y, z);
            Quaternion rotation = Quaternion.identity;

            Tile tileGameObject = Instantiate(original, position, rotation);
            if (tileGameObject == null) throw new Exception("tileGameObject is null with position : " + position);

            tileGameObject.transform.parent = transform;
            tileGameObject.name = new StringBuilder().Append(x).Append("_").Append(y).ToString();

            return tileGameObject;
        }
    }
}