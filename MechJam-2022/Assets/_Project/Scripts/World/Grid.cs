using UnityEngine;

namespace Gisha.MechJam.World
{
    public class Grid
    {
        public Cell[,] Cells { get; }
        public int Width { get; }
        public int Height { get; }
        public float CellSize { get; }
        public Cell FirstCell => Cells[0, 0];
        public Cell LastCell => Cells[Cells.GetLength(0) - 1, Cells.GetLength(1) - 1];
        
        public Grid(int xSize, int ySize, float cellSize)
        {
            Width = xSize;
            Height = ySize;
            CellSize = cellSize;

            Cells = new Cell[Width, Height];
            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                Cells[x, y] = new Cell(new Vector2Int(x, y));
        }

        public Vector2Int GetCoordsFromWorldPos(Vector3 worldPosition)
        {
            return GridTransform.GetCoordsFromWorldPos(worldPosition, this);
        }

        public Vector3 GetWorldPosFromCoords(Vector2Int coords)
        {
            if (coords.x > Width || coords.y > Height || coords.x < 0 || coords.y < 0)
            {
                Debug.LogError("Out of coords.");
                return Vector3.zero;
            }

            return GridTransform.GetWorldPosFromCoords(coords, this);
        }
    }

    public static class GridTransform
    {
        public static Vector3 GetWorldPosFromCoords(Vector2Int coords, Grid grid)
        {
            float worldX = (coords.x - grid.Width / 2f + .5f) * grid.CellSize;
            float worldY = (coords.y - grid.Height / 2f + .5f) * grid.CellSize;
            return new Vector3(worldX, 0f, worldY);
        }

        public static Vector2Int GetCoordsFromWorldPos(Vector3 worldPosition, Grid grid)
        {
            int xCoords = Mathf.FloorToInt(worldPosition.x / grid.CellSize + grid.Width / 2f);
            int yCoords = Mathf.FloorToInt(worldPosition.z / grid.CellSize + grid.Height / 2f);

            return new Vector2Int(xCoords, yCoords);
        }

        public static Cell[] GetCells(Grid grid, Vector3 position, Vector2Int areaDimensions, float yEulerAngles)
        {
            int yRotation = Mathf.RoundToInt(yEulerAngles);

            int newXSize = yRotation % 180f == 0 ? areaDimensions.x : areaDimensions.y;
            int newZSize = yRotation % 180f == 0 ? areaDimensions.y : areaDimensions.x;

            Vector2Int inputCoords = GetCoordsFromWorldPos(position, grid);
            Vector2 offset = GetOffsetCoords(newXSize, newZSize);

            // Applying offsets differently for x/y % 2 == 0 or x/y % 2 != 0.
            int xCoord = newXSize % 2 == 0
                ? Mathf.RoundToInt(inputCoords.x - offset.x - 0.5f)
                : Mathf.CeilToInt(inputCoords.x - offset.x);
            int zCoord = newZSize % 2 == 0
                ? Mathf.RoundToInt(inputCoords.y - offset.y - 0.5f)
                : Mathf.CeilToInt(inputCoords.y - offset.y);
            Vector2Int startCoords = new Vector2Int(xCoord, zCoord);

            Cell[] result = new Cell[newXSize * newZSize];

            for (int aZ = 0; aZ < newZSize; aZ++)
            for (int aX = 0; aX < newXSize; aX++)
            {
                int x = startCoords.x + aX;
                int z = startCoords.y + aZ;

                if (x >= 0 && x < grid.Width && z >= 0 && z < grid.Height)
                    result[aX + (aZ * newXSize)] = grid.Cells[x, z];
            }

            return result;
        }

        public static Vector2 GetOffsetCoords(int xSize, int zSize)
        {
            float xOffset = xSize % 2 == 0 ? xSize / 2f - 0.5f : Mathf.Ceil(xSize / 2f);
            float zOffset = zSize % 2 == 0 ? zSize / 2f - 0.5f : Mathf.Ceil(zSize / 2f);

            return new Vector2(xOffset, zOffset);
        }

        public static Vector3 CenterVector3FromCoords(Grid grid, Vector2Int a, Vector2Int b)
        {
            Vector3 firstPos = GetWorldPosFromCoords(a, grid);
            Vector3 lastPos = GetWorldPosFromCoords(b, grid);

            return (firstPos + lastPos) / 2f;
        }
    }


    public class Cell
    {
        public bool isBusy;
        public Vector2Int Coords { get; }

        public Cell(Vector2Int coords)
        {
            Coords = coords;
        }
    }
}