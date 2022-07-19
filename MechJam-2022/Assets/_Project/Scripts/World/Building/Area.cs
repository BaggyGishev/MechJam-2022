using UnityEngine;

namespace Gisha.MechJam.World.Building
{
    public class Area : MonoBehaviour
    {
        [SerializeField] private Transform topPoint, bottomPoint;

        private LineRenderer _outline;

        private void Awake()
        {
            _outline = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            var dimensions = GetDimensions(WorldManager.Grid.CellSize);
            Cell[] selectedCells = WorldManager.Grid.GetCellsArea(transform.position, dimensions, 0f);

            for (int i = 0; i < selectedCells.Length; i++)
                selectedCells[i].isOutOfBuildArea = false;

            RenderOutline(selectedCells, dimensions);
        }

        private Vector2Int GetDimensions(float cellSize)
        {
            int x = Mathf.RoundToInt((topPoint.position.x - bottomPoint.position.x) / cellSize);
            int y = Mathf.RoundToInt((topPoint.position.z - bottomPoint.position.z) / cellSize);

            return new Vector2Int(x, y);
        }

        private void RenderOutline(Cell[] cellsArea, Vector2Int dimensions)
        {
            Vector3[] points = new Vector3[5];
            _outline.positionCount = points.Length;
            
            Cell topRight = cellsArea[dimensions.x - 1];
            Cell bottomRight = cellsArea[cellsArea.Length - 1];
            Cell bottomLeft = cellsArea[dimensions.x * (dimensions.y - 1) + 1];
            Cell topLeft = cellsArea[0];

            points[0] = WorldManager.Grid.GetWorldPosFromCoords(topRight.Coords);
            points[1] = WorldManager.Grid.GetWorldPosFromCoords(bottomRight.Coords);
            points[3] = WorldManager.Grid.GetWorldPosFromCoords(topLeft.Coords);
            points[2] = WorldManager.Grid.GetWorldPosFromCoords(bottomLeft.Coords);
            points[4] = points[0];
            
            for (int i = 0; i < points.Length; i++)
                _outline.SetPosition(i, points[i]);
        }
    }
}