using UnityEngine;

namespace Gisha.MechJam.World.Building
{
    [RequireComponent(typeof(BoxCollider))]
    public class Area : MonoBehaviour
    {
        [SerializeField] protected Transform topPoint, bottomPoint;
        [SerializeField] private bool isAlly;

        public bool IsAlly => isAlly;

        protected LineRenderer _outline;
        private BoxCollider _collider;


        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _outline = GetComponent<LineRenderer>();
        }

        public virtual void Start()
        {
            _collider.isTrigger = true;

            Vector3 center = Vector3.zero;

            float xSize = Mathf.Abs(topPoint.localPosition.x) + Mathf.Abs(bottomPoint.localPosition.x);
            float ySize = 15f;
            float zSize = Mathf.Abs(topPoint.localPosition.z) + Mathf.Abs(bottomPoint.localPosition.z);

            _collider.size = new Vector3(xSize, ySize, zSize);
            _collider.center = center;
        }
    }
}