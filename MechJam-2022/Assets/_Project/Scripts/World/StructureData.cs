using UnityEngine;

namespace Gisha.MechJam.World
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

        public Vector2Int Dimensions
        {
            get
            {
                int x = Mathf.RoundToInt((MeshFilter.sharedMesh.bounds.max.x - MeshFilter.sharedMesh.bounds.min.x) *
                                         Prefab.transform.localScale.x);
                int z = Mathf.RoundToInt((MeshFilter.sharedMesh.bounds.max.z - MeshFilter.sharedMesh.bounds.min.z) *
                                         Prefab.transform.localScale.z);

                return new Vector2Int(x, z);
            }
        }

        
    }
}