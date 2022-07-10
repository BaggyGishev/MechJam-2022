using System;
using UnityEngine;

namespace Gisha.MechJam.World.Building
{
    public class BuildArea : MonoBehaviour
    {
        [SerializeField] private Transform topPoint, bottomPoint;
        
        private void Start()
        {
            Cell[] selectedCells = WorldManager.Grid.GetCellsArea(transform.position,
                GetDimensions(WorldManager.Grid.CellSize), 0f);
            
            for (int i = 0; i < selectedCells.Length; i++)
                selectedCells[i].isOutOfBuildArea = false;
        }

        private Vector2Int GetDimensions(float cellSize)
        {
            int x = Mathf.RoundToInt((topPoint.position.x - bottomPoint.position.x) / cellSize);
            int y = Mathf.RoundToInt((topPoint.position.z - bottomPoint.position.z) / cellSize);

            return new Vector2Int(x, y);
        }
    }
}