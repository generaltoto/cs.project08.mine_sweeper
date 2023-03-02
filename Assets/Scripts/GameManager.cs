using System;
using System.Linq;
using Grid;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance => _instance ??= FindObjectOfType<GameManager>();
        public GameObject popup;

        public void OnLeftClick(Tile tile)
        {
            if (!gameStarted) HandleFirstClick(tile);

            if ((tile.IsRevealed && tile.Type != Tile.TileType.CLUE) || tile.IsFlagged) return;

            switch (tile.Type)
            {
                case Tile.TileType.BOMB:
                    GridManager.Instance.HandleBombTileReveal(tile);
                    break;
                case Tile.TileType.CLUE:
                    GridManager.Instance.HandleClueTileReveal(tile);
                    break;
                case Tile.TileType.EMPTY:
                    GridManager.Instance.HandleEmptyTileReveal(tile);
                    break;
            }

            bool gameOver = CheckIfGameOver();
            if (gameOver) HandleWin();
        }

        public void OnRightClick(Tile tile)
        {
            if (!tile.IsRevealed)
            {
                switch (tile.IsFlagged)
                {
                    case true:
                        GridManager.Instance.FlagPositions.Remove(tile.Position);
                        break;
                    case false:
                        GridManager.Instance.FlagPositions.Add(tile.Position);
                        break;
                }

                tile.ToggleFlag();
                popup.SetActive(!popup.activeSelf);

            }

            bool gameOver = CheckIfGameOver();
            if (gameOver) HandleWin();
        }

        public void StartGame(int width, int height)
        {
            SceneManager.LoadSceneAsync(GAME_SCENE_INDEX, LoadSceneMode.Additive).completed += _ =>
            {
                SceneManager.UnloadSceneAsync(MAIN_MENU_SCENE_INDEX).completed += _ =>
                {
                    InitCam(width, height);
                };
                gameStarted = false;

                int bombCount = (int)(width * height * 0.2);
                GridManager.Instance.Init(width, height, bombCount);

                GridManager.Instance.GenerateBoard();
                
                SceneManager.LoadScene(UI_SCENE_INDEX, LoadSceneMode.Additive);
                
            };
        }

        public void SetDifficulty()
        {
            int value = GetComponent<Dropdown>().value;

            switch (value)
            {
                case 1:
                    StartGame(15, 10);
                    break;
                case 2:
                    StartGame(20, 20);
                    break;
                case 3:
                    StartGame(30, 30);
                    break;
            }
        }

        private static GameManager _instance;

        [SerializeField] private bool gameStarted;

        private const int MAIN_MENU_SCENE_INDEX = 0;
        private const int GAME_SCENE_INDEX = 1;
        private const int UI_SCENE_INDEX = 2;



        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SceneManager.LoadScene(MAIN_MENU_SCENE_INDEX);
        }

        private void HandleFirstClick(Tile tile)
        {
            GridManager.Instance.GenerateBombs(tile.Position);
            GridManager.Instance.GenerateCluesAndEmpty();
            gameStarted = true;
        }

        private void HandleWin()
        {
            Debug.Log("You won!");
            popup.SetActive(!popup.activeSelf);      
        }

        private void InitCam(int w, int h)
        {
            Camera mainCam = Camera.main!;
            mainCam.transform.position = new Vector3(w * 0.5f - 0.5f, h * 0.5f - 0.5f, -10);
            mainCam.orthographicSize = h * 0.5f + 5f;
        }

        private bool CheckIfGameOver()
        {
            return
                GridManager.Instance.FlagPositions.Count == GridManager.Instance.BombsPositions.Count &&
                GridManager.Instance.FlagPositions.All(flagPos =>
                    GridManager.Instance.BombsPositions.Contains(flagPos)
                );
        }

    }
}