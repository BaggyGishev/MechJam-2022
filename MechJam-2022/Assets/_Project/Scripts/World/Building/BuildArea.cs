using Gisha.MechJam.World;
using UnityEngine;

namespace Gisha.MechJam.World.Building
{
    [RequireComponent(typeof(LineRenderer))]
    public class BuildArea : Area
    {
        public override void Start()
        {
            base.Start();
            var dimensions = GetDimensions(GridManager.Grid.CellSize);
            var selectedCells = GridManager.Grid.GetCellsArea(transform.position, dimensions, 0f);

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

            points[0] = GridManager.Grid.GetWorldPosFromCoords(topRight.Coords);
            points[1] = GridManager.Grid.GetWorldPosFromCoords(bottomRight.Coords);
            points[3] = GridManager.Grid.GetWorldPosFromCoords(topLeft.Coords);
            points[2] = GridManager.Grid.GetWorldPosFromCoords(bottomLeft.Coords);
            points[4] = points[0];

            for (int i = 0; i < points.Length; i++)
                _outline.SetPosition(i, points[i]);
        }
    }
}