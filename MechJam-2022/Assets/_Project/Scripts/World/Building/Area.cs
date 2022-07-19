using UnityEngine;

namespace Gisha.MechJam.World.Building
{
    public class Area : MonoBehaviour
    {
        [SerializeField] protected Transform topPoint, bottomPoint;

        protected LineRenderer _outline;

        private void Awake()
        {
            _outline = GetComponent<LineRenderer>();
        }
    }
}