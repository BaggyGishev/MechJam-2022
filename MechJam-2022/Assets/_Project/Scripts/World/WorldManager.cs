using UnityEngine;

namespace Gisha.MechJam.World
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField] private int width, height;
        [SerializeField] private float cellSize;

        private Grid _grid;

        private void Awake()
        {
            _grid = new Grid(width, height, cellSize);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hitInfo))
                {
                    var coords = _grid.ConvertWorldPosToCoords(hitInfo.point);
                    Debug.Log("Coords are: " + coords.x + " " + coords.y);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                _grid = new Grid(width, height, cellSize);

            for (int x = 0; x < _grid.Cells.GetLength(0); x++)
            for (int y = 0; y < _grid.Cells.GetLength(1); y++)
                Gizmos.DrawWireCube(_grid.GetWorldPosFromCoords(x, y),
                    new Vector3(_grid.CellSize, 0.25f, _grid.CellSize));
        }
    }
}