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

        public void OnLeftClick(Tile tile)
        {
            // If the game did not started yet, so we need to generate the bombs and the clues
            if (!gameStarted) HandleFirstClick(tile);

            // Cannot click on revealed tiles (except clues) or flagged tiles
            if ((tile.IsRevealed && tile.Type != Tile.TileType.CLUE) || tile.IsFlagged) return;

            switch (tile.Type)
            {
                case Tile.TileType.BOMB:
                    GridManager.Instance.HandleBombTileReveal(tile);
                    audioSource.PlayOneShot(sound);
                    HandleLose();
                    break;
                case Tile.TileType.CLUE:
                    GridManager.Instance.HandleClueTileReveal(tile);
                    break;
                case Tile.TileType.EMPTY:
                    GridManager.Instance.HandleEmptyTileReveal(tile);
                    break;
                default:
                    throw new Exception($"Tile {tile.Position} type was not recognized");
            }

            // Check if the game is over
            bool gameOver = CheckIfGameOver();
            if (gameOver) HandleWin();
        }

        public void OnRightClick(Tile tile)
        {
            if (tile.IsRevealed) return;
            switch (tile.IsFlagged)
            {
                case true:
                    int index = GridManager.Instance.FlagPositions.IndexOf(tile.Position);
                    if (index != -1) GridManager.Instance.FlagPositions.RemoveAt(index);
                    break;
                case false:
                    GridManager.Instance.FlagPositions.Add(tile.Position);
                    break;
            }

            tile.ToggleFlag();

            bool gameOver = CheckIfGameOver();
            if (gameOver) HandleWin();
        }

        public void StartGame(int width, int height)
        {
            SceneManager.LoadSceneAsync(GAME_SCENE_INDEX, LoadSceneMode.Additive).completed += _ =>
            {
                SceneManager.UnloadSceneAsync(MAIN_MENU_SCENE_INDEX).completed += _ => { InitCam(width, height); };
                gameStarted = false;

                int bombCount = (int)(width * height * 0.2);
                GridManager.Instance.Init(width, height, bombCount);
                GridManager.Instance.GenerateBoard();

                SceneManager.LoadScene(UI_SCENE_INDEX, LoadSceneMode.Additive);
            };
        }

        public void EndGame()
        {
            SceneManager.LoadSceneAsync(MAIN_MENU_SCENE_INDEX);
            SceneManager.UnloadSceneAsync(GAME_SCENE_INDEX);
            SceneManager.UnloadSceneAsync(UI_SCENE_INDEX);
        }

        private void HandleWin()
        {
            popup = Instantiate(popup, new Vector3(0, 0, 3), Quaternion.identity);
        }

        private void HandleLose()
        {
            popup = Instantiate(Losepopup, new Vector3(0, 0, 3), Quaternion.identity);
        }

        private static GameManager _instance;

        [SerializeField] private bool gameStarted;

        private const int MAIN_MENU_SCENE_INDEX = 0;
        private const int GAME_SCENE_INDEX = 1;
        private const int UI_SCENE_INDEX = 2;
        
        [SerializeField] private GameObject popup;
        [SerializeField] private GameObject Losepopup;
        [SerializeField] private GameObject NewGame;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip sound;

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

        private void InitCam(int w, int h)
        {
            Camera mainCam = Camera.main!;
            mainCam.transform.position = new Vector3(w * 0.5f - 0.5f, h * 0.5f - 0.5f, -10);
            mainCam.orthographicSize = h * 0.5f + 5f;
        }

        private bool CheckIfGameOver()
        {
            bool enoughFlags =
                (GridManager.Instance.FlagPositions.Count == GridManager.Instance.BombsPositions.Count) &&
                (gameStarted);
            bool allFlagsAreBombs = GridManager.Instance.FlagPositions.All(flagPos =>
                GridManager.Instance.BombsPositions.Contains(flagPos)
            );

            return enoughFlags && allFlagsAreBombs && gameStarted;
        }
    }
}