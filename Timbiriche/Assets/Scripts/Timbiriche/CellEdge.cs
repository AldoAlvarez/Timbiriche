using Generic.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Timbiriche
{
    [System.Serializable]
    public class CellEdge: IRestartable
    {
        public CellEdge(Vector2Int v1, Vector2Int v2) {
            vertices = new List<Vector2Int>() { v1, v2 };
        }


        public bool Open { get; private set; }
        private List<Vector2Int> vertices;


        public bool isFormedBy(params Vector2Int[] vertices) {
            foreach (var vertex in vertices)
                if (!this.vertices.Contains(vertex)) return false;
            return true;
        }

        public void Restart() { Open = true; }
        public void Close() { Open = false; }
        public void Clear() { vertices.Clear(); }
    }
}