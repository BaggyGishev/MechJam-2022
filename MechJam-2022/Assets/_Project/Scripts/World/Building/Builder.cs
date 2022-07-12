using Gisha.MechJam.UI;
using UnityEngine;

namespace Gisha.MechJam.World.Building
{
    [RequireComponent(typeof(WorldManager))]
    public class Builder : MonoBehaviour
    {
        private StructureData _structureToBuild;

        private void OnEnable()
        {
            StructureUIElement.OnStructureSelected += SelectStructure;
            StructureUIElement.OnStructureDeselected += DeselectStructure;
        }

        private void OnDisable()
        {
            StructureUIElement.OnStructureSelected -= SelectStructure;
            StructureUIElement.OnStructureDeselected -= DeselectStructure;
        }

        private void Update()
        {
            if (_structureToBuild == null)
                return;

            if (Input.GetMouseButtonDown(0))
                BuildRaycast();
        }

        public void SelectStructure(StructureData structureData)
        {
            _structureToBuild = structureData;
        }

        public void DeselectStructure()
        {
            _structureToBuild = null;
        }
        
        
        private void BuildRaycast()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                // Getting modified dimensions for structure building area. 
                Cell[] selectedCells = WorldManager.Grid.GetCellsArea(hitInfo.point,
                    _structureToBuild.GetDimensions(WorldManager.Grid.CellSize),
                    _structureToBuild.Prefab.transform.rotation.eulerAngles.y);

                if (!CheckForBusyCell(selectedCells))
                {
                    Cell firstCell = selectedCells[0];
                    Cell lastCell = selectedCells[selectedCells.Length - 1];
                    Vector3 pos = WorldManager.Grid.CenterWorldPosFromCoords(firstCell.Coords,
                        lastCell.Coords);

                    BuildStructure(pos, selectedCells);
                }
            }
        }

        private void BuildStructure(Vector3 pos, Cell[] selectedCells)
        {
            var structure = Instantiate(_structureToBuild.Prefab, pos, _structureToBuild.Prefab.transform.rotation)
                .GetComponent<Structure>();
            structure.takenArea = selectedCells;
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