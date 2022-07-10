using UnityEngine;

namespace Gisha.MechJam.World.Building
{
    [RequireComponent(typeof(WorldManager))]
    public class Builder : MonoBehaviour
    {
        [SerializeField] private StructureData structureToBuild;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                BuildRaycast();
        }

        private void BuildRaycast()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                // Getting modified dimensions for structure building area. 
                Cell[] selectedCells = WorldManager.Grid.GetCellsArea(hitInfo.point,
                    structureToBuild.GetDimensions(WorldManager.Grid.CellSize), 0f);

                if (!CheckForBusyCell(selectedCells))
                {
                    Cell firstCell = selectedCells[0];
                    Cell lastCell = selectedCells[selectedCells.Length - 1];
                    Vector3 pos = WorldManager.Grid.CenterWorldPosFromCoords(firstCell.Coords,
                        lastCell.Coords);

                    BuildStructure(pos);
                }
            }
        }

        private void BuildStructure(Vector3 pos)
        {
            Instantiate(structureToBuild.Prefab, pos, Quaternion.identity);
        }

        private bool CheckForBusyCell(Cell[] cells)
        {
            for (int i = 0; i < cells.Length; i++)
                if (cells[i] == null || cells[i].IsBusy)
                    return true;

            return false;
        }
    }
}