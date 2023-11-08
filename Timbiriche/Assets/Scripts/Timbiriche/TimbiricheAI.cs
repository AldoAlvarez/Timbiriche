using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Timbiriche {
    public class TimbiricheAI : MonoBehaviour
    {

        [SerializeField] private Grid grid;
        public Vector2Int[] GetVertices(int difficulty) {
            switch (difficulty) {
                default:
                    var v1 = GetRandomVertex();
                    //vamos izquierda
                    Vector2Int v2 = new Vector2Int(v1.x-1, v1.y);
                    //vamos derecha
                    if (v1.x <= grid.columns) { v2.x = v1.x + 1; v2.y = v1.y; }
                    //vamos abajo
                    else if (v1.y <= grid.rows) { v2.y = v1.y + 1; v2.x = v1.x; }
                    //vamos arriba
                    else if (v1.y > 0) { v2.y = v1.y - 1; v2.x = v1.x; }
                    return new Vector2Int[] { v1,v2};
            }
        }

        private Vector2Int GetRandomVertex()
        {
            var x = Random.Range(0, grid.columns + 1);
            var y = Random.Range(0, grid.rows + 1);
            return new Vector2Int(x, y);
        }
    }
}