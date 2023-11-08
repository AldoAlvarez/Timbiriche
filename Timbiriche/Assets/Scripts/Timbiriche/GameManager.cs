using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Timbiriche
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            if (instance == null) instance = this;
        }
        private void Start()
        {
            difficulty = 0;
            if (players == null) players = PlayersController.Instance;
            verticesSelected = new Vector2Int[2] { 
                new Vector2Int(-1, -1), 
                new Vector2Int(-1, -1)
                };
        }

        #region VARIABLES
        public static GameManager Instance=>instance;
        private static GameManager instance;

        [SerializeField] private TMPro.TMP_Dropdown DifficultySelector;
        [SerializeField] private Grid grid;
        [SerializeField] private TimbiricheAI ai;
        [SerializeField] private PlayersController players;
        public int difficulty { get; private set; }

        public int currentPlayer { get; private set; }
        private Vector2Int[] verticesSelected;

        [SerializeField] private UnityEvent OnGameOver;
        [SerializeField] private Image winnerDisplay;
        [SerializeField] private Image currentplayerDisplay;
        #endregion

        #region PUBLIC METHODS
        public void Restart() {
            difficulty = DifficultySelector.value;
            currentPlayer = 0;
            players.Restart();
            ResetVerticesSelected();
            grid.Restart();
            UpdateCurrentPlayerDislay();
        }
        public void SetVertex(Vector2Int vertex)
        {
            for (int i = 0; i < verticesSelected.Length; i++)
            {
                if (verticesSelected[i].x < 0)
                {
                    verticesSelected[i] = vertex;
                    break;
                }
            }

            if (!CanCreateACellEdge()) return;
            if (!AreSelectedVerticesAdjacent()) { ResetVerticesSelected(); return; }
            grid.ConnectVertices(verticesSelected[0], verticesSelected[1]);
            //cerró una celda y se le dió un turno extra
            if (!CanCreateACellEdge()) return;

            ChangePlayer();
        }
        public void AddTurnToCurrentPlayer() {
            if (grid.Solved()) {
                var winner = players.GetHighestScorePlayer().color;
                winner.a = winnerDisplay.color.a;
                winnerDisplay.color = winner;
                OnGameOver?.Invoke(); 
            }
            ResetVerticesSelected();
        }
        #endregion

        #region PRIVATE METHODS
        private bool CanCreateACellEdge()
        {
            for (int i = 0; i < verticesSelected.Length; i++)
            {
                if (verticesSelected[i].x < 0)
                    return false;
            }
            return true;
        }
        private bool AreSelectedVerticesAdjacent()
        {
            for (int i = 0; i < verticesSelected.Length - 1; i++)
            {
                var distance = Vector2Int.Distance(verticesSelected[i], verticesSelected[i + 1]);
                if (distance > 1.1f) return false;
                if (verticesSelected[i] == verticesSelected[i + 1]) return false;
            }

            return true;
        }
        private void ChangePlayer()
        {
            ResetVerticesSelected();
            currentPlayer = (currentPlayer + 1) % players.TotalPlayers;
            UpdateCurrentPlayerDislay();

            if (players.isPlayerAI(currentPlayer)) {
                StartCoroutine(AITurn());
            }
        }
        private void ResetVerticesSelected()
        {
            for (int v = 0; v < verticesSelected.Length; v++)
            {
                verticesSelected[v] = Vector2Int.one * -1;
            }
        }
        private void UpdateCurrentPlayerDislay() {
            currentplayerDisplay.color = players.GetCurrentPlayerColor();
        }

        private IEnumerator AITurn() {
            yield return new WaitForSeconds(0.6f);
            var vertices = ai.GetVertices(difficulty);
            foreach (var vertex in vertices) {
                SetVertex(vertex);
            }
        }
        #endregion

    }
}