using System.Collections;
using System.Collections.Generic;
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

        #region Position/Coords converting

        public Vector3 GetWorldPosFromCoords(Vector2Int coords)
        {
            if (coords.x > Width || coords.y > Height || coords.x < 0 || coords.y < 0)
            {
                Debug.LogError("Out of coords.");
                return Vector3.zero;
            }

            float worldX = (coords.x - Width / 2f + .5f) * CellSize;
            float worldY = (coords.y - Height / 2f + .5f) * CellSize;
            return new Vector3(worldX, 0f, worldY);
        }

        private Vector2Int GetCoordsFromWorldPos(Vector3 worldPosition)
        {
            int xCoords = Mathf.FloorToInt(worldPosition.x / CellSize + Width / 2f);
            int yCoords = Mathf.FloorToInt(worldPosition.z / CellSize + Height / 2f);

            return new Vector2Int(xCoords, yCoords);
        }

        public Vector3 CenterWorldPosFromCoords(Vector2Int a, Vector2Int b)
        {
            Vector3 firstPos = GetWorldPosFromCoords(a);
            Vector3 lastPos = GetWorldPosFromCoords(b);

            // Debug.DrawLine(firstPos, lastPos, Color.green, 100f);
            // Debug.Log(firstPos + " " + lastPos);

            return (firstPos + lastPos) / 2f;
        }

        #endregion

        #region Area Getter

        public Cell[] GetCellsArea(Vector3 position, Vector2Int areaDimensions, float yEulerAngles)
        {
            int yRotation = Mathf.RoundToInt(yEulerAngles);

            int newXSize = yRotation % 180f == 0 ? areaDimensions.x : areaDimensions.y;
            int newZSize = yRotation % 180f == 0 ? areaDimensions.y : areaDimensions.x;

            Vector2Int inputCoords = GetCoordsFromWorldPos(position);
            Vector2 offset = GetOffsetCoords(newXSize, newZSize);

            // Applying offsets differently for x/y % 2 == 0 or x/y % 2 != 0.
            int xCoord = newXSize % 2 == 0
                ? Mathf.RoundToInt(inputCoords.x - offset.x - 0.5f)
                : Mathf.CeilToInt(inputCoords.x - offset.x);
            int zCoord = newZSize % 2 == 0
                ? Mathf.RoundToInt(inputCoords.y - offset.y - 0.5f)
                : Mathf.CeilToInt(inputCoords.y - offset.y);
            Vector2Int startCoords = new Vector2Int(xCoord, zCoord);

            var result = new Cell[newXSize * newZSize];
            for (int aZ = 0; aZ < newZSize; aZ++)
            for (int aX = 0; aX < newXSize; aX++)
            {
                int x = startCoords.x + aX;
                int z = startCoords.y + aZ;

                if (x >= 0 && x < Width && z >= 0 && z < Height)
                    result[aX + (aZ * newXSize)] = Cells[x, z];
            }

            return result;
        }
        
        private Vector2 GetOffsetCoords(int xSize, int zSize)
        {
            float xOffset = xSize % 2 == 0 ? xSize / 2f - 0.5f : Mathf.Ceil(xSize / 2f);
            float zOffset = zSize % 2 == 0 ? zSize / 2f - 0.5f : Mathf.Ceil(zSize / 2f);

            return new Vector2(xOffset, zOffset);
        }

        #endregion

        public bool CheckForBusyCell(Cell[] cells)
        {
            for (int i = 0; i < cells.Length; i++)
                if (cells[i] == null || cells[i].IsBusy)
                    return true;

            return false;
        }
    }

    public class Cell
    {
        public bool isBlockedByStructure = false, isOutOfBuildArea = true;

        public bool IsBusy => isBlockedByStructure || isOutOfBuildArea;

        public Vector2Int Coords { get; }

        public Cell(Vector2Int coords)
        {
            Coords = coords;
        }
    }
}