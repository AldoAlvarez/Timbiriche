using Generic.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Timbiriche { 
public class GridCell : MonoBehaviour,IRestartable, IPoolable, IGridable

{
        #region VARIABLES
        public Vector2Int position;
        [SerializeField] private Image box;
        [SerializeField] private Color offColor;
        private List<CellEdge> edges = new List<CellEdge>();

        public bool Active { get; private set; } = false;
        public Action OnEnable { get; set; }
        public Action OnDisable { get; set; }
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// The vertices must be assigned clockwise or counterclockwise
        /// </summary>
        /// <param name="vertices"></param>
        public void Initialize(Vector2Int[]vertices) {
            box.color = offColor;
            for (int i = 0; i < vertices.Length; i++)
            {
                var nextVertex = (i + 1) % vertices.Length;
                edges.Add(new CellEdge(vertices[i], vertices[nextVertex]));
            }
        }
        public void ConnectVertices(Vector2Int v1, Vector2Int v2) {
            if (isCellCompleted()) return;

            foreach (var edge in edges) {
                if (edge.isFormedBy(v1, v2)) {
                    edge.Close();
                    CheckCompletition();
                    return;
                }
            }
        }
        public void Restart()
        {
            box.color = offColor;
            foreach (var edge in edges)
                edge.Restart();
        }
        public void SetPosition(int x, int y)
        {
            position.x = x;
            position.y = y;
        }
        public Vector2Int GetPosition() { return position; }

        public void Disable()
        {
            if (!Active) return;
            Active = false;
            gameObject.SetActive(false);
            Restart();
            OnDisable?.Invoke();
        }

        public void Enable()
        {
            if (Active) return;
            Active = true;
            gameObject.SetActive(true);
            OnEnable?.Invoke();
        }
        #endregion

        #region PRIVATE METHODS
        private void CheckCompletition() {
            if (!isCellCompleted()) return;
            Enable();
            PlayersController.Instance.AddScoreToCurrentPlayer();
            box.color = PlayersController.Instance.GetCurrentPlayerColor();
            GameManager.Instance.AddTurnToCurrentPlayer();
        }
        private bool isCellCompleted() {
            foreach (var edge in edges)
                if (edge.Open) return false;
            return true;
        }
        #endregion
    }
}