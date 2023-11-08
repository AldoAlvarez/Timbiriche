using Generic.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Timbiriche {
    public class Grid : MonoBehaviour, IRestartable
    {
        #region UNITY METHODS
        private void Start()
        {



        }
        private void Update()
        {

        }
        #endregion

        #region VARIABLES
        [SerializeField] private Slider GridSizeSelector;
        [SerializeField] private RectTransform GridHolder;

        [Tooltip("Upper left corner of the grid")]
        [SerializeField] private ObjectPool_Grid<GridVertex> vertices;
        [SerializeField] private ObjectPool_Grid<GridCell> cells;
        [SerializeField] private ObjectPool_Grid<ConnectionLine> edges;

        private float separation = 0;
        public int columns { get; private set; } = 8;
        public int rows{get; private set; } = 8;

        private int requiredCells = 0;
        #endregion

        #region PUBLIC METHODS
        public bool Solved() {
            return requiredCells<=0;
        }
        public void ConnectVertices(Vector2Int v1, Vector2Int v2) {
            var rotation = GetOrientation(v1, v2);
            var vertex = vertices.GetObjectAt(v1);
            var edge = edges.GetAvailableObject();
            //bug: se pueden repetir lineas, aunque no se de el punto
            edge.GetComponent<RectTransform>().rotation = rotation;
            edge.GetComponent<RectTransform>().anchoredPosition = vertex.GetComponent<RectTransform>().anchoredPosition;
            edge.Enable();
            edge.SetColor(PlayersController.Instance.GetCurrentPlayerColor());

            cells.IterateAll((cell) => {
                cell.ConnectVertices(v1, v2);
            });
        }

        public void Restart() {
            columns = rows = (int)GridSizeSelector.value;
            var width = GridHolder.rect.width;
            separation = width / columns;

            vertices.Restart();

            vertices.SetContainerPosition(GridHolder.anchoredPosition);

            var vertexSize = vertices.prefab.GetComponent<RectTransform>().sizeDelta;
            vertexSize.y *= -1;
            cells.SetContainerPosition(GridHolder.anchoredPosition + (vertexSize / 2));
            edges.SetContainerPosition(GridHolder.anchoredPosition + (vertexSize / 2));


            CreateVerticesGrid();
            CreateCellsGrid();
            CreateGridEdges();
  
            cells.Restart();
            edges.Restart();
            requiredCells = rows * columns;
        }

        #endregion

        #region PRIVATE METHODS

        private Vector2Int[] GetCellVertices(int x, int y)
        {
            List<Vector2Int> vertices = new List<Vector2Int>();
            if (isVertexInsideGrid(x,y)) vertices.Add(new Vector2Int(x, y));
            if (isVertexInsideGrid(x+1,y)) vertices.Add(new Vector2Int(x + 1, y));
            if (isVertexInsideGrid(x+1,y+1)) vertices.Add(new Vector2Int(x+1, y + 1));
            if (isVertexInsideGrid(x,y+1)) vertices.Add(new Vector2Int(x, y + 1));
            return vertices.ToArray();
        }
        private bool isVertexInsideGrid(int x, int y) {
            return (x >= 0) && (x <= columns) && (y >= 0) && (y <= rows);
        }

        private Quaternion GetOrientation(Vector2Int v1, Vector2Int v2) {
            var angle = 0;
            if (v1.x == v2.x)
            {
                //es linea vertical y asumo que va hacia abajo
                angle = 270;
                if (v1.y > v2.y) {
                    //va hacia arriba
                    angle = 90;
                }
            }
            else {
                //si es horizontal y asumo que es de izquierda a derecha (0)
                if (v1.x > v2.x) {
                    //derecha a izquierda
                    angle = 180;
                }
            }
            return Quaternion.Euler(0, 0, angle);
        }

        private void CreateVerticesGrid() {

            vertices.OnObjectCreation = (vertex, x, y) => {
                vertex.SetPosition(x, y);
                vertex.Enable();
                vertex.OnClick = () => { GameManager.Instance.SetVertex(vertex.position); };
            };
            vertices.CreateGrid(columns + 1, rows + 1, separation);
        }
        private void CreateCellsGrid()
        {
            cells.OnObjectCreation = (cell, x, y) => { 
                var size = new Vector2(separation, separation);
                cell.GetComponent<RectTransform>().sizeDelta = size;

                cell.Enable();
                cell.SetPosition(x, y);
                var adjacent = GetCellVertices(x, y);
                cell.Initialize(adjacent);
                cell.OnEnable = () => { requiredCells--; };
            };
            cells.CreateGrid(columns, rows, separation);
        }
        private void CreateGridEdges() {
            edges.OnObjectCreation = (edge, x, y) => {
                var size = new Vector2(separation, 20);
                edge.GetComponent<RectTransform>().sizeDelta = size;

                edge.Enable();
                edge.SetPosition(x, y);
            };
            edges.CreateGrid(columns, rows, separation);
        }
        #endregion
    }
}