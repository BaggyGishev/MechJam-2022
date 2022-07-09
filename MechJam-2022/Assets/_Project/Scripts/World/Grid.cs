using UnityEngine;

namespace Gisha.MechJam.World
{
    public class Grid
    {
        public Grid[,] Cells { private set; get; }
        public int Width { private set; get; }
        public int Height { private set; get; }
        public float CellSize { private set; get; }

        public Grid(int xSize, int ySize, float cellSize)
        {
            Width = xSize;
            Height = ySize;
            CellSize = cellSize;
            Cells = new Grid[Width, Height];
        }

        public Vector2Int ConvertWorldPosToCoords(Vector3 worldPosition)
        {
            int xCoords = Mathf.FloorToInt(worldPosition.x / CellSize + Width / 2f);
            int yCoords = Mathf.FloorToInt(worldPosition.z / CellSize + Height / 2f);

            return new Vector2Int(xCoords, yCoords);
        }

        public Vector3 GetWorldPosFromCoords(int xCoords, int yCoords)
        {
            if (xCoords > Width || yCoords > Height || xCoords < 0 || yCoords < 0)
            {
                Debug.LogError("Out of coords.");
                return Vector3.zero;
            }

            float worldX = (xCoords - Width / 2f + .5f) * CellSize;
            float worldY = (yCoords - Height / 2f + .5f) * CellSize;
            return new Vector3(worldX, 0f, worldY);
        }
    }

    public class Cell
    {
    }
}