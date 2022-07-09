using UnityEngine;

namespace Gisha.MechJam.World
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField] private int width, height;
        [SerializeField] private float cellSize;

        public Grid Grid { private set; get; }

        private void Awake()
        {
            Grid = new Grid(width, height, cellSize);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                Grid = new Grid(width, height, cellSize);

            for (int x = 0; x < Grid.Cells.GetLength(0); x++)
            for (int y = 0; y < Grid.Cells.GetLength(1); y++)
            {
                if (Grid.Cells[x, y].isBusy)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.blue;

                Gizmos.DrawWireCube(Grid.GetWorldPosFromCoords(new Vector2Int(x, y)),
                    new Vector3(Grid.CellSize, 0.25f, Grid.CellSize));
            }
        }
    }
}