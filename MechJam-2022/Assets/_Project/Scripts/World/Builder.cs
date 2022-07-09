using UnityEngine;

namespace Gisha.MechJam.World
{
    [RequireComponent(typeof(WorldManager))]
    public class Builder : MonoBehaviour
    {
        [SerializeField] private GameObject prefabToBuild;

        private WorldManager _worldManager;

        private void Awake()
        {
            _worldManager = GetComponent<WorldManager>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hitInfo))
                {
                    var coords = _worldManager.Grid.GetCoordsFromWorldPos(hitInfo.point);

                    if (!CheckIfCellBusy(coords))
                    {
                        Build(coords);
                        Debug.Log("Build at: " + coords.x + " " + coords.y);
                    }
                }
            }
        }

        private void Build(Vector2Int coords)
        {
            Instantiate(prefabToBuild, _worldManager.Grid.GetWorldPosFromCoords(coords), Quaternion.identity);
            _worldManager.Grid.Cells[coords.x, coords.y].isBusy = true;
        }

        private bool CheckIfCellBusy(Vector2Int coords)
        {
            return _worldManager.Grid.Cells[coords.x, coords.y].isBusy;
        }
    }
}