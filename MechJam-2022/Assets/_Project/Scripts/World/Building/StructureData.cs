using UnityEngine;

namespace Gisha.MechJam.World.Building
{
    [CreateAssetMenu(fileName = "StructureData", menuName = "Scriptable Objects/Structure Data", order = 0)]
    public class StructureData : ScriptableObject
    {
        [SerializeField] private string structureName;
        [SerializeField] private GameObject prefab;
        [SerializeField] private Vector2Int dimensionsAddition = Vector2Int.one;

        public GameObject Prefab => prefab;

        public MeshFilter MeshFilter
        {
            get => Prefab.GetComponent<MeshFilter>();
        }

        public Vector2Int GetDimensions(float cellSize)
        {
            int x = Mathf.CeilToInt((MeshFilter.sharedMesh.bounds.max.x - MeshFilter.sharedMesh.bounds.min.x) *
                Prefab.transform.localScale.x / cellSize) + dimensionsAddition.x;
            int y = Mathf.CeilToInt((MeshFilter.sharedMesh.bounds.max.z - MeshFilter.sharedMesh.bounds.min.z) *
                Prefab.transform.localScale.z / cellSize) + dimensionsAddition.y;

            return new Vector2Int(x, y);
        }
    }
}