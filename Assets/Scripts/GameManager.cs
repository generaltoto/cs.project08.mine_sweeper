using System;
using Grid;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance => _instance ??= FindObjectOfType<GameManager>();

        public void OnLeftClick(Tile tile)
        {
            if (!gameStarted) HandleFirstClick(tile);

            if ((tile.IsRevealed && tile.Type != Tile.TileType.CLUE) || tile.IsFlagged) return;

            switch (tile.Type)
            {
                case Tile.TileType.BOMB: GridManager.Instance.HandleBombTileReveal(tile); break;
                case Tile.TileType.CLUE: GridManager.Instance.HandleClueTileReveal(tile); break;
                case Tile.TileType.EMPTY: GridManager.Instance.HandleEmptyTileReveal(tile); break;
            }

            bool gameOver = CheckIfGameOver();
            if (gameOver) HandleWin();
        }

        public void OnRightClick(Tile tile)
        {
            if (!tile.IsRevealed)
            {
                tile.ToggleFlag();
                GridManager.Instance.FlagPositions.Add(tile.Position);
            }

            bool gameOver = CheckIfGameOver();
            if (gameOver) HandleWin();
        }

        private static GameManager _instance;

        private const int WIDTH = 15;
        private const int HEIGHT = 10;
        private const int BOMBS_COUNT = 3;

        [SerializeField] private Camera cam;

        [SerializeField] private bool gameStarted;

        private void Start()
        {
            gameStarted = false;
            InitCam();
            GridManager.Instance.Init(WIDTH, HEIGHT, BOMBS_COUNT);
            GridManager.Instance.GenerateBoard();
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
        }

        private void InitCam()
        {
            cam = Camera.main;
            if (cam == null) throw new Exception("No camera found in scene!");
            cam.transform.position = new Vector3(WIDTH * 0.5f - 0.5f, HEIGHT * 0.5f - 0.5f, -10);
            cam.orthographicSize = HEIGHT * 0.5f;
        }

        private bool CheckIfGameOver()
        {
            var flagPositions = GridManager.Instance.FlagPositions;
            var bombsPositions = GridManager.Instance.BombsPositions;

            if (flagPositions.Count != bombsPositions.Count) return false;

            foreach (Vector2Int flagPos in flagPositions)
            {
                if (bombsPositions.Contains(flagPos)) continue;
                return false;
            }

            return true;
        }
    }
}