using Generic.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Timbiriche
{
    public class PlayersController : MonoBehaviour, IRestartable
    {
        private void Awake() {if(instance==null) instance = this; }

        #region VARIABLES
        public static PlayersController Instance=>instance;
        private static PlayersController instance;

        [SerializeField] private Player []players;

        public int TotalPlayers => players.Length;
        #endregion

        #region PUBLIC METHODS
        public void AddScoreToCurrentPlayer() {
            players[GameManager.Instance.currentPlayer].ModifyScore(1);
        }
        public Color GetCurrentPlayerColor() {
            return players[GameManager.Instance.currentPlayer].color;
        }
        public void Restart()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].Restart();
            }
        }
        public Player GetHighestScorePlayer() {
            var max = 0;
            Player plyr = null;
            foreach (var player in players) {
                if (player.score > max) {
                    max = player.score;
                    plyr = player;
                }
            }
            return plyr;
        }
        public bool isPlayerAI(int player) {
            return players[player].isAI;
        }
        #endregion

        #region PRIVATE METHODS

        #endregion

        [System.Serializable]
        public class Player : IRestartable
        {
            public int score;
            public Color color;
            public bool isAI = false;
            public TMPro.TMP_Text scoreDisplay;

            public void Restart() {
                score = 0;
                UpdateScoreDisplay(); 
            }
            public void ModifyScore(int value) {
                score += value;
                UpdateScoreDisplay();
            }

            private void UpdateScoreDisplay() {
                scoreDisplay.text = score.ToString();
            }
        }
    }
}