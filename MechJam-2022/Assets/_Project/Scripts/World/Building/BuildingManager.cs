using Gisha.MechJam.UI;
using UnityEngine;

namespace Gisha.MechJam.World.Building
{
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] private Transform structuresParent;
        
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

                if (!WorldManager.Grid.CheckForBusyCell(selectedCells))
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
            structure.transform.SetParent(structuresParent);
        }
    }
}