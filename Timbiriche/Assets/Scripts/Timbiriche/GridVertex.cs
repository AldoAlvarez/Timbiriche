using Generic.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Timbiriche
{
    public class GridVertex : MonoBehaviour, IPoolable, IGridable
    {
        public Vector2Int position = new Vector2Int();

        public Action OnClick;

        public bool Active { get; private set; } = false;
        public Action OnEnable { get; set; }
        public Action OnDisable { get; set; }


        public void SetPosition(int x, int y) {
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

        public void Interact() {
            OnClick?.Invoke();
        }
    }
}
