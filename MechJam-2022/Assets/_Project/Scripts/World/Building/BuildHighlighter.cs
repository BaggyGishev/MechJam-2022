using System.Collections;
using Gisha.MechJam.UI;
using UnityEngine;

namespace Gisha.MechJam.World.Building
{
    public class BuildHighlighter : MonoBehaviour
    {
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
        }

        private void OnDisable()
        {
            StructureUIElement.OnStructureSelected -= EnableHighlight;
            StructureUIElement.OnStructureDeselected -= DisableHighligt;
        }

        private void EnableHighlight(StructureData structureData)
        {
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
                        _meshRenderer.materials = strMeshRenderer.sharedMaterials;

                        Cell[] selectedCells = WorldManager.Grid.GetCellsArea(hitInfo.point,
                            structureData.GetDimensions(WorldManager.Grid.CellSize),
                            structureData.Prefab.transform.rotation.eulerAngles.y);

                        Cell firstCell = selectedCells[0];
                        Cell lastCell = selectedCells[selectedCells.Length - 1];

                        if (firstCell == null || lastCell == null)
                        {
                            yield return null;
                            continue;
                        }

                        Vector3 pos = WorldManager.Grid.CenterWorldPosFromCoords(firstCell.Coords,
                            lastCell.Coords);

                        transform.position = pos;
                    }
                }

                yield return null;
            }
        }
    }
}