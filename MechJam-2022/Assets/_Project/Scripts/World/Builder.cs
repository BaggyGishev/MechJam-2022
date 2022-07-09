using UnityEngine;

namespace Gisha.MechJam.World
{
    [RequireComponent(typeof(WorldManager))]
    public class Builder : MonoBehaviour
    {
        [SerializeField] private StructureData structureToBuild;

        private WorldManager _worldManager;

        private void Awake()
        {
            _worldManager = GetComponent<WorldManager>();
        }

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
                Vector2Int modifiedDimensions =
                    new Vector2Int(
                        Mathf.RoundToInt(structureToBuild.Dimensions.x / _worldManager.Grid.CellSize),
                        Mathf.RoundToInt(structureToBuild.Dimensions.y / _worldManager.Grid.CellSize));
                Cell[] selectedCells = GridTransform.GetCells(_worldManager.Grid, hitInfo.point,
                    modifiedDimensions, 0f);

                if (!CheckForBusyCell(selectedCells))
                {
                    Cell firstCell = selectedCells[0];
                    Cell lastCell = selectedCells[selectedCells.Length - 1];
                    Vector3 pos = GridTransform.CenterVector3FromCoords(_worldManager.Grid, firstCell.Coords,
                        lastCell.Coords);

                    BuildStructure(selectedCells, pos);
                }
            }
        }


        private void BuildStructure(Cell[] selectedCells, Vector3 pos)
        {
            Instantiate(structureToBuild.Prefab, pos, Quaternion.identity);

            for (int i = 0; i < selectedCells.Length; i++)
                selectedCells[i].isBusy = true;
        }

        private bool CheckForBusyCell(Cell[] cells)
        {
            for (int i = 0; i < cells.Length; i++)
                if (cells[i].isBusy)
                    return true;

            return false;
        }
    }
}