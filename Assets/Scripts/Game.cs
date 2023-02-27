using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class Game : MonoBehaviour
    {
        private const int WIDTH = 10;
        private const int HEIGHT = 10;
        private const int BOMBS_COUNT = 10;

        private const string PREFAB_PATH = "Prefabs/";
        private const string BOMB_PREFAB_NAME = "bomb";
        private const string EMPTY_PREFAB_NAME = "empty";
        private const string MASK_PREFAB_NAME = "mask";
        private const string FLAG_PREFAB_NAME = "flag";

        private GameObject[,] _board;

        [SerializeField] private Transform cam;

        private void Start()
        {
            _board = new GameObject[WIDTH, HEIGHT];
            InitCam();
            GenerateBombs();
            GenerateCluesAndEmpty();
        }

        public void SetDifficulty()
        {
            int value = GetComponent<Dropdown>().value;

            switch (value)
            {
                case 1:
                    SceneManager.LoadScene(1);
                    break;

                case 2:
                    _board = new GameObject[20, 20];
                    break;

                case 3:
                    _board = new GameObject[30, 30];
                    break;
            }
        }
        private void InitCam()
        {
            cam = Camera.main.transform;
            cam.transform.position = new Vector3(WIDTH * 0.5f - 0.5f, HEIGHT * 0.5f - 0.5f, -10);
            cam.GetComponent<Camera>().orthographicSize = HEIGHT * 0.5f;
        }

        private void GenerateBombs()
        {
            (int x, int y) bombPos = (0, 0);

            for (int i = 0; i < BOMBS_COUNT; i++)
            {
                do
                {
                    bombPos.x = Random.Range(0, WIDTH);
                    bombPos.y = Random.Range(0, HEIGHT);
                } while (
                    _board[bombPos.x, bombPos.y] != null
                );

                _board[bombPos.x, bombPos.y] = InitTileGameObject(BOMB_PREFAB_NAME, bombPos.x, bombPos.y);
                _board[bombPos.x, bombPos.y].GetComponent<Tile>().InitWithType(Tile.TileType.BOMB, BOMB_PREFAB_NAME);
            }
        }

        private void GenerateCluesAndEmpty()
        {
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    if (_board[x, y] != null) continue;

                    int bombsCount = GetBombsCountAround(x, y);

                    string prefabName = bombsCount == 0 ? EMPTY_PREFAB_NAME : bombsCount.ToString();
                    Tile.TileType type = bombsCount == 0 ? Tile.TileType.EMPTY : Tile.TileType.CLUE;

                    _board[x, y] = InitTileGameObject(prefabName, x, y);
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

                    if (_board[i, j] != null)
                    {
                        if (_board[i, j].name.Contains(BOMB_PREFAB_NAME)) count++;
                    }
                }
            }

            return count;
        }

        private GameObject InitTileGameObject(string prefabName, int x, int y, int z = 0)
        {
            Object original = Resources.Load(PREFAB_PATH + prefabName);
            Vector3 position = new Vector3(x, y, z);
            Quaternion rotation = Quaternion.identity;

            GameObject tileGameObject = Instantiate(original, position, rotation) as GameObject;
            tileGameObject!.AddComponent<Tile>();
            tileGameObject!.AddComponent<BoxCollider2D>();

            StringBuilder sb = new StringBuilder();
            sb.Append(prefabName).Append("_").Append(x).Append("_").Append(y);
            tileGameObject!.name = sb.ToString();

            return tileGameObject;
        }
    }
}