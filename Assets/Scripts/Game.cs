using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class Game : MonoBehaviour
    {
        public static Game Instance => _instance ??= FindObjectOfType<Game>();

        public void OnTileClicked(Tile tile)
        {
            if (!gameStarted) HandleFirstClick(tile);
            
            if (tile.IsRevealed || tile.IsFlagged) return;
            
            switch (tile.Type)
            {
                case Tile.TileType.BOMB:
                    Thread.Sleep(2000);
                    tile.Reveal();
                    HandleLoseCase();
                    break;
                case Tile.TileType.CLUE:
                    tile.Reveal();
                    break;
                case Tile.TileType.EMPTY:
                    var tilePos = tile.Position;
                    FloodFill(tilePos.x, tilePos.y);
                    break;
            }
        }

        private static Game _instance;

        private const int WIDTH = 15;
        private const int HEIGHT = 10;
        private const int BOMBS_COUNT = 25;

        private const string PREFAB_PATH = "Prefabs/";
        private const string BOMB_PREFAB_NAME = "bomb";
        private const string EMPTY_PREFAB_NAME = "empty";

        private GameObject[,] _board;

        [SerializeField] private Camera cam;
        
        [SerializeField] private bool gameStarted;

        private void Start()
        {
            _board = new GameObject[WIDTH, HEIGHT];
            gameStarted = false;
            InitCam();
            GenerateBoard();
        }

        private void HandleFirstClick(Tile tile)
        {
            Debug.Log("Cliked on tile: " + tile.Position);
            GenerateBombs(tile.Position);
            GenerateCluesAndEmpty();
            gameStarted = true;
        }

        private void HandleLoseCase()
        {
            Application.Quit();
        }

        private void InitCam()
        {
            cam = Camera.main;
            cam.transform.position = new Vector3(WIDTH * 0.5f - 0.5f, HEIGHT * 0.5f - 0.5f, -10);
            cam.orthographicSize = HEIGHT * 0.5f;
        }

        private void GenerateBoard()
        {
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    _board[x, y] = InitTileGameObject(EMPTY_PREFAB_NAME, x, y);
                    _board[x, y].GetComponent<Tile>().Position = new Vector2Int(x, y);
                    _board[x, y].GetComponent<Tile>().InitWithType(Tile.TileType.EMPTY, EMPTY_PREFAB_NAME);
                }
            }
        }

        private void GenerateBombs(Vector2Int forbiddenPos)
        {
            (int x, int y) bombPos = (0, 0);

            for (int i = 0; i < BOMBS_COUNT; i++)
            {
                do
                {
                    bombPos.x = Random.Range(0, WIDTH);
                    bombPos.y = Random.Range(0, HEIGHT);
                } while (
                    _board[bombPos.x, bombPos.y].GetComponent<Tile>().Type == Tile.TileType.BOMB ||
                    IsAroundClickedTile(bombPos.x, bombPos.y, forbiddenPos.x, forbiddenPos.y)
                );

                _board[bombPos.x, bombPos.y].GetComponent<Tile>().InitWithType(Tile.TileType.BOMB, BOMB_PREFAB_NAME);
            }
        }

        private void GenerateCluesAndEmpty()
        {
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    if (_board[x, y].GetComponent<Tile>().Type == Tile.TileType.BOMB) continue;

                    int bombsCount = GetBombsCountAround(x, y);

                    string prefabName = bombsCount == 0 ? EMPTY_PREFAB_NAME : bombsCount.ToString();
                    Tile.TileType type = bombsCount == 0 ? Tile.TileType.EMPTY : Tile.TileType.CLUE;

                    _board[x, y].GetComponent<Tile>().InitWithType(type, prefabName);
                }
            }
        }

        private int GetBombsCountAround(int x, int y)
        {
            int count = 0;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i < 0 || i >= WIDTH || j < 0 || j >= HEIGHT) continue;

                    if (_board[i, j].GetComponent<Tile>().Type == Tile.TileType.BOMB) count++;
                }
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

        private bool IsTileValid(int x, int y) => (x >= 0 && x < WIDTH && y >= 0 && y < HEIGHT);

        private void FloodFill(int x, int y)
        {
            // List of all tiles to check, int x, int y and bool isClue.
            // Is the tile is a clue, we don't need to check its adjacent tiles
            List<Tuple<int, int, bool>> queue = new List<Tuple<int, int, bool>>();

            queue.Add(new Tuple<int, int, bool>(x, y, false));

            Tile firstTile = _board[x, y].GetComponent<Tile>();
            firstTile.Reveal();

            while (queue.Count > 0)
            {
                // Dequeue the front node
                Tuple<int, int, bool> curTile = queue.Last();
                queue.RemoveAt(queue.Count - 1);
                if (curTile.Item3) continue;

                int posX = curTile.Item1;
                int posY = curTile.Item2;

                CheckAdjacentTile(posX + 1, posY, ref queue);
                CheckAdjacentTile(posX - 1, posY, ref queue);
                CheckAdjacentTile(posX, posY + 1, ref queue);
                CheckAdjacentTile(posX, posY - 1, ref queue);
            }
        }

        private void CheckAdjacentTile(int posX, int posY, ref List<Tuple<int, int, bool>> queue)
        {
            if (!IsTileValid(posX, posY)) return;

            Tile tile = _board[posX, posY].GetComponent<Tile>();
            if (tile.Type != Tile.TileType.BOMB && !tile.IsRevealed)
            {
                tile.Reveal();
                queue.Add(new Tuple<int, int, bool>(posX, posY, tile.Type == Tile.TileType.CLUE));
            }
        }
    }
}