using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class Game : MonoBehaviour
    {
        public static Game Instance => _instance ??= FindObjectOfType<Game>();

        public void OnLeftClick(Tile tile)
        {
            if (!gameStarted) HandleFirstClick(tile);

            if ((tile.IsRevealed && tile.Type != Tile.TileType.CLUE) || tile.IsFlagged) return;

            switch (tile.Type)
            {
                case Tile.TileType.BOMB: HandleBombTileReveal(tile); break;
                case Tile.TileType.CLUE: HandleClueTileReveal(tile); break;
                case Tile.TileType.EMPTY:HandleEmptyTileReveal(tile); break;
            }

            bool gameOver = CheckIfGameOver();
            if (gameOver) HandleWin();
        }
        
        public void OnRightClick(Tile tile)
        {
            if (!tile.IsRevealed)
            {
                tile.ToggleFlag();
                _flagPositions.Add(tile.Position);
            }
            
            bool gameOver = CheckIfGameOver();
            if (gameOver) HandleWin();
        }

        private static Game _instance;

        private const int WIDTH = 15;
        private const int HEIGHT = 10;
        private const int BOMBS_COUNT = 3;

        private const string PREFAB_PATH = "Prefabs/";
        private const string DEFAULT_PREFAB_NAME = "default_tile";

        private GameObject[,] _board;
        private List<Vector2Int> _bombsPositions;
        private List<Vector2Int> _flagPositions;

        [SerializeField] private Camera cam;

        [SerializeField] private bool gameStarted;

        private void Start()
        {
            _board = new GameObject[WIDTH, HEIGHT];
            _bombsPositions = new List<Vector2Int>();
            _flagPositions = new List<Vector2Int>();
            
            gameStarted = false;
            InitCam();
            GenerateBoard();
        }
        
        private void HandleFirstClick(Tile tile)
        {
            GenerateBombs(tile.Position);
            GenerateCluesAndEmpty();
            gameStarted = true;
        }
        
        private void HandleWin()
        {
            Debug.Log("You won!");
        }

        private void InitCam()
        {
            cam = Camera.main;
            if (cam == null) throw new Exception("No camera found in scene!");
            cam.transform.position = new Vector3(WIDTH * 0.5f - 0.5f, HEIGHT * 0.5f - 0.5f, -10);
            cam.orthographicSize = HEIGHT * 0.5f;
        }

        private void GenerateBoard()
        {
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    _board[x, y] = InitTileGameObject(DEFAULT_PREFAB_NAME, x, y);
                    _board[x, y].GetComponent<Tile>().Position = new Vector2Int(x, y);
                    _board[x, y].GetComponent<Tile>().InitWithType(Tile.TileType.EMPTY);
                }
            }
        }

        private void GenerateBombs(Vector2Int forbiddenPos)
        {
            (int x, int y) bombPos = (0, 0);

            for (int i = 0; i < BOMBS_COUNT; i++)
            {
                // We generate bombs coordinates until we find an empty spot. 
                // The bomb coordinates are not allowed to be around / be first clicked tile coordinates.
                do
                {
                    bombPos.x = Random.Range(0, WIDTH);
                    bombPos.y = Random.Range(0, HEIGHT);
                } while (
                    _board[bombPos.x, bombPos.y].GetComponent<Tile>().Type == Tile.TileType.BOMB ||
                    IsAroundClickedTile(bombPos.x, bombPos.y, forbiddenPos.x, forbiddenPos.y)
                );

                _board[bombPos.x, bombPos.y].GetComponent<Tile>().InitWithType(Tile.TileType.BOMB);
                _bombsPositions.Add(new Vector2Int(bombPos.x, bombPos.y));
            }
        }

        private void GenerateCluesAndEmpty()
        {
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    Tile tile = _board[x, y].GetComponent<Tile>();

                    if (tile.Type == Tile.TileType.BOMB) continue;

                    tile.ClueCount = GetBombsCountAround(x, y);
                    Tile.TileType type = tile.ClueCount == 0 ? Tile.TileType.EMPTY : Tile.TileType.CLUE;
                    tile.InitWithType(type);
                }
            }
        }

        private List<Tile> GetNeighbours(int x, int y)
        {
            List<Tile> neighbours = new List<Tile>();
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i < 0 || i >= WIDTH || j < 0 || j >= HEIGHT || (i == x && j == y)) continue;

                    neighbours.Add(_board[i, j].GetComponent<Tile>()); 
                }
            }
            return neighbours;
        }
        
        private int GetBombsCountAround(int x, int y)
        {
            int count = 0;

            List<Tile> neighbours = GetNeighbours(x, y);
            foreach (Tile neighbour in neighbours)
            {
                if (neighbour.Type == Tile.TileType.BOMB) count++;
            }

            return count;
        }

        private bool IsAroundClickedTile(int x, int y, int tileX, int tileY)
        {
            for (int i = tileX - 1; i <= tileX + 1; i++)
            {
                for (int j = tileY - 1; j <= tileY + 1; j++)
                {
                    if (i < 0 || i >= WIDTH || j < 0 || j >= HEIGHT) continue;

                    if (i == x && j == y) return true;
                }
            }

            return false;
        }

        private GameObject InitTileGameObject(string prefabName, int x, int y, int z = 0)
        {
            Object original = Resources.Load(PREFAB_PATH + prefabName);
            Vector3 position = new Vector3(x, y, z);
            Quaternion rotation = Quaternion.identity;

            GameObject tileGameObject = Instantiate(original, position, rotation) as GameObject;
            if (tileGameObject == null) throw new Exception("tileGameObject is null with position : " + position);

            tileGameObject.AddComponent<Tile>();
            tileGameObject.AddComponent<BoxCollider2D>();
            tileGameObject.transform.parent = transform;
            tileGameObject.name = new StringBuilder().Append(x).Append("_").Append(y).ToString();

            return tileGameObject;
        }
        
        private void HandleEmptyTileReveal(Tile tile)
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
                        queue.Add(new Tuple<int, int, bool>(neighbour.Position.x, neighbour.Position.y, neighbour.Type == Tile.TileType.CLUE));
                    }
                }
            }
        }

        private void HandleClueTileReveal(Tile tile)
        {
            if (tile.IsRevealed)
            {
                foreach (Tile neighbour in GetNeighbours(tile.Position.x, tile.Position.y))
                {
                    Debug.Log(tile.Type);
                    switch (neighbour.Type)
                    {
                        case Tile.TileType.BOMB: HandleBombTileReveal(neighbour); break;
                        case Tile.TileType.CLUE: neighbour.Reveal(); break;
                        case Tile.TileType.EMPTY: HandleEmptyTileReveal(neighbour); break;
                    }
                }
            }
            else tile.Reveal();
        }

        private void HandleBombTileReveal(Tile tile)
        {
            if (!tile.IsFlagged) tile.Reveal(true);
        }

        private bool CheckIfGameOver()
        {
            if (_flagPositions.Count != _bombsPositions.Count) return false;
            
            foreach (Vector2Int flagPos in _flagPositions)
            {
                if (_bombsPositions.Contains(flagPos)) continue;
                return false;
            }

            return true;
        }
    }
}