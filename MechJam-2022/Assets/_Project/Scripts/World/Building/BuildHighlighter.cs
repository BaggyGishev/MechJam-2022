using System.Collections;
using Gisha.MechJam.Core;
using Gisha.MechJam.UI;
using UnityEngine;

namespace Gisha.MechJam.World.Building
{
    public class BuildHighlighter : MonoBehaviour
    {
        [SerializeField] private Material redMaterial;

        private LayerMask _layerMask;
        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Ground");
        }

        private void OnEnable()
        {
            StructureUIElement.OnStructureSelected += EnableHighlight;
            StructureUIElement.OnStructureDeselected += DisableHighligt;
            BuildingManager.StructureDeselected += DisableHighligt;
            GameManager.InteractionModeChanged += mode => DisableHighligt();
        }

        private void OnDisable()
        {
            StructureUIElement.OnStructureSelected -= EnableHighlight;
            StructureUIElement.OnStructureDeselected -= DisableHighligt;
            BuildingManager.StructureDeselected -= DisableHighligt;
            GameManager.InteractionModeChanged -= mode => DisableHighligt();
        }

        private void EnableHighlight(StructureData structureData)
        {
            if (BuildingManager.BuildMode != BuildMode.Build)
                return;

            _meshRenderer.enabled = true;
            StartCoroutine(PlaceHighlightRoutine(structureData));
        }

        private void DisableHighligt()
        {
            _meshRenderer.enabled = false;
            StopAllCoroutines();
        }

        private IEnumerator PlaceHighlightRoutine(StructureData structureData)
        {
            var strMeshRenderer = structureData.Prefab.GetComponent<MeshRenderer>();
            var strMeshFilter = structureData.Prefab.GetComponent<MeshFilter>();
            transform.rotation = structureData.Prefab.transform.rotation;

            while (true)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hitInfo, 1000f, _layerMask))
                {
                    if (hitInfo.collider != null)
                    {
                        _meshFilter.mesh = strMeshFilter.sharedMesh;

                        var selectedCells = GridManager.Grid.GetCellsArea(hitInfo.point,
                            structureData.GetDimensions(GridManager.Grid.CellSize),
                            structureData.Prefab.transform.rotation.eulerAngles.y);

                        Cell firstCell = selectedCells[0];
                        Cell lastCell = selectedCells[selectedCells.Length - 1];

                        if (firstCell == null || lastCell == null)
                        {
                            yield return null;
                            continue;
                        }

                        // Updating mesh renderer.
                        if (GridManager.Grid.CheckForBusyCell(selectedCells))
                        {
                            var materials = new Material[strMeshRenderer.sharedMaterials.Length];
                            for (int i = 0; i < strMeshRenderer.sharedMaterials.Length; i++)
                                materials[i] = redMaterial;

                            _meshRenderer.materials = materials;
                        }
                        else
                            _meshRenderer.materials = strMeshRenderer.sharedMaterials;

                        // Placing to right position.
                        Vector3 pos = GridManager.Grid.CenterWorldPosFromCoords(firstCell.Coords,
                            lastCell.Coords);
                        transform.position = pos;
                    }
                }

                yield return null;
            }
        }
    }
}