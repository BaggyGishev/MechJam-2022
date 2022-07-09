using UnityEngine;

namespace Gisha.MechJam.World
{
    public class Grid
    {
        public Cell[,] Cells { get; }
        public int Width { get; }
        public int Height { get; }
        public float CellSize { get; }

        public Grid(int xSize, int ySize, float cellSize)
        {
            Width = xSize;
            Height = ySize;
            CellSize = cellSize;

            Cells = new Cell[Width, Height];
            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                Cells[x, y] = new Cell();
        }

        public Vector2Int GetCoordsFromWorldPos(Vector3 worldPosition)
        {
            return GridTransform.GetCoordsFromWorldPos(worldPosition, Height, Width, CellSize);
        }

        public Vector3 GetWorldPosFromCoords(Vector2Int coords)
        {
            if (coords.x > Width || coords.y > Height || coords.x < 0 || coords.y < 0)
            {
                Debug.LogError("Out of coords.");
                return Vector3.zero;
            }

            return GridTransform.GetWorldPosFromCoords(coords, Height, Width, CellSize);
        }
    }

    public static class GridTransform
    {
        public static Vector3 GetWorldPosFromCoords(Vector2Int coords, int height, int width, float cellSize)
        {
            float worldX = (coords.x - width / 2f + .5f) * cellSize;
            float worldY = (coords.y - height / 2f + .5f) * cellSize;
            return new Vector3(worldX, 0f, worldY);
        }

        public static Vector2Int GetCoordsFromWorldPos(Vector3 worldPosition, int height, int width, float cellSize)
        {
            int xCoords = Mathf.FloorToInt(worldPosition.x / cellSize + width / 2f);
            int yCoords = Mathf.FloorToInt(worldPosition.z / cellSize + height / 2f);

            return new Vector2Int(xCoords, yCoords);
        }
    }


    public class Cell
    {
        public bool isBusy;
    }
}