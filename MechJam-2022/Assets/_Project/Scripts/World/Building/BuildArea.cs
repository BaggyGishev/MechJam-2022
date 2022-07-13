using UnityEngine;

namespace Gisha.MechJam.World.Building
{
    public class BuildArea : MonoBehaviour
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

            Vector3 xOffset = new Vector3(WorldManager.Grid.CellSize / 2f, 0f, 0f);
            Vector3 zOffset = new Vector3(0f, 0f, WorldManager.Grid.CellSize / 2f);

            Cell topRight = cellsArea[dimensions.x - 1];
            Cell bottomRight = cellsArea[cellsArea.Length - 1];
            Cell bottomLeft = cellsArea[dimensions.x * (dimensions.y - 1) + 1];
            Cell topLeft = cellsArea[0];

            points[0] = WorldManager.Grid.GetWorldPosFromCoords(topRight.Coords) + xOffset + zOffset;
            points[1] = WorldManager.Grid.GetWorldPosFromCoords(bottomRight.Coords) + xOffset - zOffset;
            points[2] = WorldManager.Grid.GetWorldPosFromCoords(bottomLeft.Coords) - xOffset - zOffset;
            points[3] = WorldManager.Grid.GetWorldPosFromCoords(topLeft.Coords) - xOffset + zOffset;
            points[4] = points[0];
            
            for (int i = 0; i < points.Length; i++)
                _outline.SetPosition(i, points[i] + Vector3.up * 0.75f);
        }
    }
}