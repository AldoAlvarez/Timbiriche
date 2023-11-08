using Generic.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Timbiriche {
    [System.Serializable]
    public class ObjectPool_Grid<T> where T:Behaviour, IPoolable
    {
        public ObjectPool_Grid() {
            pool = new List<T>();
            inactive = new Queue<T>();
        }

        #region VARIABLES
        public GameObject prefab;
        public RectTransform container;

        private List<T> pool;
        private Queue<T> inactive;

        public System.Action<T, int, int> OnObjectCreation;
        #endregion

        #region PUBLIC METHODS
        public void CreateGrid(int columns, int rows, float separation) {
            var postion = Vector2.zero;
            for (int col = 0; col < columns; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    var obj = GetInactiveObject();
                    obj.GetComponent<RectTransform>().anchoredPosition = postion;
                    postion.y -= separation;
                    OnObjectCreation?.Invoke(obj, col, row);
                }
                postion.x += separation;
                postion.y = 0;
            }
        }
        public void Restart() {
            foreach (var obj in pool) {
                obj.Disable();
            }
        }
        public void SetContainerPosition(Vector2 position) {
            container.anchoredPosition = position;
        }
        public T GetObjectAt(Vector2Int position) {
            foreach (var obj in pool) {
                if (obj.Active) { 
                var gridable = obj.GetComponent<IGridable>();
                    if (position == gridable.GetPosition()) 
                        return obj;
                }
            }
            return null;
        }

        public T GetAvailableObject() { return GetInactiveObject(); }

        public void IterateAll(System.Action<T> OnIteration) {
            foreach (var obj in pool)
            {
                OnIteration?.Invoke(obj);
            }
        }
        public bool areAnyInactive() {
            foreach (var obj in pool) {
                if (!obj.Active) return true;
            }
            return false;
        }
        #endregion

        #region PRIVATE METHODS
        private T GetInactiveObject (){
            if (inactive.Count == 0) return CreateNewObject();
            return inactive.Dequeue();
        }
        private T CreateNewObject() {
            var obj = GameObject.Instantiate(prefab, container);
            obj.GetComponent<IPoolable>().OnDisable = () => {
                inactive.Enqueue(obj.GetComponent<T>()); 
            };
            pool.Add(obj.GetComponent<T>());
            return obj.GetComponent<T>();
        }
        #endregion
    }
}