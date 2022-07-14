using System.Collections;
using Gisha.MechJam.AI;
using UnityEngine;

namespace Gisha.MechJam.World.Targets
{
    public abstract class Target : MonoBehaviour
    {
        [SerializeField] private float captureRadius = 15f;
        [SerializeField] private float captureSpeed = 1f;

        private LayerMask _allyLayerMask;

        private UnitAI[] _defenders;
        private float _captureProgress;
        private bool _isCaptured;

        public bool IsCaptured => _isCaptured;

        public virtual void Start()
        {
            _defenders = GetComponentsInChildren<EnemyUnitAI>();
            _allyLayerMask = 1 << LayerMask.NameToLayer("Ally");

            StartCoroutine(CaptureCheckRoutine());
        }

        private IEnumerator CaptureCheckRoutine()
        {
            while (!IsCaptured)
            {
                yield return new WaitForSeconds(1f);

                if (IsCaptureReady(out var allyCount))
                    yield return CapturingRoutine();

                yield return null;
            }
        }

        private IEnumerator CapturingRoutine()
        {
            while (IsCaptureReady(out var allyCount) || !IsCaptured)
            {
                _captureProgress += allyCount * captureSpeed * Time.deltaTime;

                if (_captureProgress >= 1f)
                {
                    FinishCapture();
                    _isCaptured = true;
                }

                yield return null;
            }
        }

        private bool IsCaptureReady(out int allyCount)
        {
            // Checking for any ally near target.
            var allyColls = Physics.OverlapSphere(transform.position, captureRadius, _allyLayerMask);
            allyCount = allyColls.Length;
            if (allyColls.Length == 0)
                return false;

            // If there is zero enemies near target, start capturing.
            for (int i = 0; i < _defenders.Length; i++)
                if (_defenders[i] != null)
                    return false;

            return true;
        }
        
        public abstract void FinishCapture();

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, captureRadius);
        }
    }
}