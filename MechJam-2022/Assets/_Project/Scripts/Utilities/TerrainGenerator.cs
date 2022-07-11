using UnityEngine;

namespace Gisha.MechJam.Utilities
{
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject[] terrainPrefabs;
        [Space] [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private Vector3 offset;

        [ContextMenu("Generate Terrain")]
        private void GenerateTerrain()
        {
            var parent = new GameObject("Terrain");
            parent.transform.SetParent(transform);

            // Generating terrain grid
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                var prefab = terrainPrefabs[Random.Range(0, terrainPrefabs.Length)];
                var position = new Vector3(x * offset.x, 0f, y * offset.z);
                position += new Vector3(-offset.x * width / 2f, 0f, -offset.z * height / 2f) + offset / 2f;
                var rotation = Quaternion.Euler(0f,Random.Range(0,4) * 90f,0f);
                
                var obj = Instantiate(prefab, position, rotation);
                obj.transform.SetParent(parent.transform);
            }
        }
    }
}