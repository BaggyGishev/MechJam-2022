using System;
using Gisha.MechJam.Core;
using Gisha.MechJam.UI;
using UnityEngine;

namespace Gisha.MechJam.World.Building
{
    public enum BuildMode
    {
        Build,
        Destroy
    }

    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] private Transform structuresParent;

        public static Action StructureDeselected;
        public static BuildMode BuildMode { private set; get; }

        private StructureData _structureToBuild;
        private LayerMask _groundLayerMask;
        private LayerMask _structureLayerMask;
        private bool _isDisabled;

        private void Awake()
        {
            _groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
            _structureLayerMask = 1 << LayerMask.NameToLayer("Structure");
            BuildMode = BuildMode.Build;
        }

        private void Start()
        {
            BuildingUIManager.Instance.UpdateBuildMode(BuildMode);
        }

        private void OnEnable()
        {
            StructureUIElement.OnStructureSelected += SelectStructure;
            UIDeactivator.PointerEntered += OnPointerEntered;
            UIDeactivator.PointerExited += OnPointerExited;
        }

        private void OnDisable()
        {
            StructureUIElement.OnStructureSelected -= SelectStructure;
            UIDeactivator.PointerEntered -= OnPointerEntered;
            UIDeactivator.PointerExited -= OnPointerExited;
        }

        private void Update()
        {
            if (GameManager.InteractionMode != InteractionMode.Build || _isDisabled)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                if (BuildMode == BuildMode.Build)
                {
                    if (_structureToBuild == null)
                        return;

                    BuildRaycast();
                }
                else
                    DestroyRaycast();
            }
        }

        private void OnPointerExited()
        {
            _isDisabled = false;
        }

        private void OnPointerEntered()
        {
            _isDisabled = true;
        }

        public void OnClick_ChangeMode()
        {
            int index = (int) BuildMode;
            index++;
            if (index > 1)
                index = 0;
            BuildMode = (BuildMode) index;

            BuildingUIManager.Instance.UpdateBuildMode(BuildMode);
            DeselectStructure();
        }

        private void SelectStructure(StructureData structureData)
        {
            if (_structureToBuild == structureData)
                DeselectStructure();
            
            _structureToBuild = structureData;
        }

        private void DeselectStructure()
        {
            _structureToBuild = null;
            StructureDeselected?.Invoke();
        }

        private void BuildRaycast()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, 100f, _groundLayerMask))
            {
                // Getting modified dimensions for structure building area. 
                Cell[] selectedCells = GridManager.Grid.GetCellsArea(hitInfo.point,
                    _structureToBuild.GetDimensions(GridManager.Grid.CellSize),
                    _structureToBuild.Prefab.transform.rotation.eulerAngles.y);

                if (!GridManager.Grid.CheckForBusyCell(selectedCells) && GameManager.Instance.EnergyCount > 0)
                {
                    Cell firstCell = selectedCells[0];
                    Cell lastCell = selectedCells[selectedCells.Length - 1];
                    Vector3 pos = GridManager.Grid.CenterWorldPosFromCoords(firstCell.Coords,
                        lastCell.Coords);

                    BuildStructure(pos, selectedCells);
                }
            }
        }

        // Destroy structure and return 1 wasted energy.
        private void DestroyRaycast()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, 100f, _structureLayerMask))
            {
                var structure = hitInfo.collider.GetComponent<Structure>();
                if (hitInfo.collider != null && structure != null && structure.IsDestroyable)
                {
                    structure.FreeTheArea();
                    Destroy(hitInfo.collider.gameObject);
                    GameManager.Instance.AddEnergyCount(1);
                }
            }
        }

        private void BuildStructure(Vector3 pos, Cell[] selectedCells)
        {
            var structure = Instantiate(_structureToBuild.Prefab, pos, _structureToBuild.Prefab.transform.rotation)
                .GetComponent<Structure>();
            structure.takenArea = selectedCells;
            structure.transform.SetParent(structuresParent);

            GameManager.Instance.AddEnergyCount(-1);
        }
    }
}