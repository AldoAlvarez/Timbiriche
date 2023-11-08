using Generic.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Timbiriche
{
    public class ConnectionLine : MonoBehaviour, IPoolable, IRestartable, IGridable
    {
        #region VARIABLES
        [SerializeField] private Image img;
        public bool Active { get; private set; } = false;
        public Action OnEnable { get; set; }
        public Action OnDisable { get; set; }

        public Vector2Int position;
        #endregion

        #region PUBLIC METHODS
        public void SetColor(Color color) { img.color = color; }
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
            OnDisable?.Invoke();
        }

        public void Enable()
        {
            if (Active) return;
            Active = true;
            gameObject.SetActive(true);
            OnEnable?.Invoke();
        }
        public void Restart()
        {
            Disable();
        }
        #endregion
    }
}