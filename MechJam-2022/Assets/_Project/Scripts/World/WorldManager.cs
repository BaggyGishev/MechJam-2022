using UnityEngine;

namespace Gisha.MechJam.World
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField] private int xSize, ySize;

        private void Awake()
        {
            Grid.CreateGrid(xSize, ySize);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                Grid.CreateGrid(xSize, ySize);

            for (int x = 0; x < Grid.Nodes.GetLength(0); x++)
            for (int y = 0; y < Grid.Nodes.GetLength(1); y++)
                Gizmos.DrawWireCube(new Vector3(x, 0, y), Vector3.one);
        }
    }
}