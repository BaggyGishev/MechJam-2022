using UnityEngine;

namespace Gisha.MechJam.World.Building
{
    [CreateAssetMenu(fileName = "StructureData", menuName = "Scriptable Objects/Structure Data", order = 0)]
    public class StructureData : ScriptableObject
    {
        [SerializeField] private string structureName;
        [SerializeField] private GameObject prefab;
        public GameObject Prefab => prefab;

        public MeshFilter MeshFilter
        {
            get => Prefab.GetComponent<MeshFilter>();
        }

        public Vector2Int GetDimensions(float cellSize)
        {
            int x = Mathf.CeilToInt((MeshFilter.sharedMesh.bounds.max.x - MeshFilter.sharedMesh.bounds.min.x) *
                                     Prefab.transform.localScale.x / cellSize);
            int y = Mathf.CeilToInt((MeshFilter.sharedMesh.bounds.max.z - MeshFilter.sharedMesh.bounds.min.z) *
                                     Prefab.transform.localScale.z / cellSize);

            return new Vector2Int(x, y);
        }
    }
}