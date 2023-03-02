using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Grid
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance => _instance ??= FindObjectOfType<GridManager>();

        public List<Vector2Int> FlagPositions { get; private set; }

        public List<Vector2Int> BombsPositions { get; private set; }

        public int BombsCount { get; private set; }

        public int FlagsCount => FlagPositions.Count;

        public void Init(int width, int height, int bombsCount)
        {
            _width = width;
            _height = height;
            BombsCount = bombsCount;
            _board = new Tile[width, height];
            BombsPositions = new List<Vector2Int>();
            FlagPositions = new List<Vector2Int>();
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
            // Get all tiles positions
            List<Vector2Int> tilesPositions = GetAllTilesPositions();

            for (int i = 0; i < BombsCount; i++)
            {
                // Generate a random index and get the position at this index
                int randomIndex;
                Vector2Int position;
                do
                {
                    // Regenerate a random index if the position is around the clicked tile or is the clicked tile
                    randomIndex = Random.Range(0, tilesPositions.Count);
                    position = tilesPositions[randomIndex];
                } while (IsAroundClickedTile(position.x, position.y, forbiddenPos.x, forbiddenPos.y));

                // Init the tile at the final position as a bomb
                _board[position.x, position.y].InitWithType(Tile.TileType.BOMB);

                // Remove the position from the list of available positions and add it to the list of bombs positions
                tilesPositions.RemoveAt(randomIndex);
                BombsPositions.Add(position);
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
                    if (neighbour.Type == Tile.TileType.BOMB || neighbour.IsRevealed) continue;
                    neighbour.Reveal();
                    queue.Add(new Tuple<int, int, bool>(neighbour.Position.x, neighbour.Position.y,
                        neighbour.Type == Tile.TileType.CLUE));
                }
            }
        }

        public void HandleClueTileReveal(Tile tile)
        {
            if (tile.IsRevealed)
            {
                foreach (Tile neighbour in GetNeighbours(tile.Position.x, tile.Position.y))
                {
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
            // ReSharper disable HeapView.ObjectAllocation
            
            if (!tile.IsFlagged) tile.Reveal(true, false);

            StartCoroutine(HandleBombOnFire(tile));

            FlagPositions
                .ForEach(tilePos => _board[tilePos.x, tilePos.y].Reveal(false, true));

            BombsPositions
                .Where(tilePos => _board[tilePos.x, tilePos.y].IsRevealed == false)
                .ToList()
                .ForEach(tilePos => _board[tilePos.x, tilePos.y].Reveal(false, false));
            
            StartCoroutine(HandleTilesFadeOut());
        }


        private static GridManager _instance;

        private const string PREFAB_PATH = "Prefabs/";
        private const string DEFAULT_PREFAB_NAME = "default_tile";

        private int _width;
        private int _height;
        private Tile[,] _board;

        private void Awake()
        {
            if (_instance == null) _instance = this;
        }

        private List<Vector2Int> GetAllTilesPositions() =>
            Enumerable.Range(0, _width)
                .SelectMany(x => Enumerable.Range(0, _height).Select(y => new Vector2Int(x, y)))
                .ToList();

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

        private int GetBombsCountAround(int x, int y) =>
            GetNeighbours(x, y).Count(neighbour => neighbour.Type == Tile.TileType.BOMB);

        private bool IsAroundClickedTile(int x, int y, int tileX, int tileY)
        {
            var neighbours = GetNeighbours(tileX, tileY);
            neighbours.Add(_board[tileX, tileY]);
            return neighbours.Exists(neighbour => neighbour.Position.x == x && neighbour.Position.y == y);
        }

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

        private IEnumerator HandleTilesFadeOut()
        {
            // We wait for the explosion animation to end
            yield return new WaitForSeconds(2f);
            
            // Get all tiles positions
            List<Vector2Int> tilesPositions = GetAllTilesPositions();

            while (tilesPositions.Count > 0)
            {
                // Get a random tile position
                int randomIndex = Random.Range(0, tilesPositions.Count);
                Vector2Int position = tilesPositions[randomIndex];

                // Get the tile at the random position
                Tile tile = _board[position.x, position.y];

                // Remove the position from the list of available positions
                tilesPositions.RemoveAt(randomIndex);

                // Fade out the tile
                tile.FadeOut();
                
                // Wait for the tile to fade out
                yield return new WaitForSeconds(0.0015f);
            }
        }

        private IEnumerator HandleBombOnFire(Tile tile)
        {
            tile.PlayFireAnimation();
            
            yield return new WaitForSeconds(2f);
        }
    }
}