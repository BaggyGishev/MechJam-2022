using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gisha.MechJam.AI;
using Gisha.MechJam.Core;
using UnityEngine;

namespace Gisha.MechJam.World.Targets
{
    public class Outpost : MonoBehaviour
    {
        [Header("Outpost variables")] [SerializeField]
        private Transform buildingTrans;

        [SerializeField] private Material placeholderMaterial;
        [SerializeField] private Material capturedMaterial;

        [SerializeField] private float captureRadius = 15f;
        [SerializeField] private float captureSpeed = 1f;

        public static Action OutpostCaptured; 
        public List<EnemyUnitAI> Defenders => _defenders;
        public bool IsCaptured => _isCaptured;

        private LayerMask _allyLayerMask;
        private List<EnemyUnitAI> _defenders = new List<EnemyUnitAI>();
        private float _captureProgress;
        private bool _isCaptured;

        public virtual void Start()
        {
            _defenders = GetComponentsInChildren<EnemyUnitAI>().ToList();
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
                    GetComponentInChildren<Area>().Capture();
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
            for (int i = 0; i < Defenders.Count; i++)
                if (Defenders[i] != null)
                    return false;

            return true;
        }

        private void FinishCapture()
        {
            SwitchMaterials();
            GameManager.Instance.AddEnergyCount(1);
            OutpostCaptured?.Invoke();
            StopAllCoroutines();
        }

        private void SwitchMaterials()
        {
            var meshRenderers = buildingTrans.GetComponentsInChildren<MeshRenderer>();

            foreach (var mr in meshRenderers)
            {
                Material[] newMaterials = mr.sharedMaterials;
                for (var i = 0; i < mr.sharedMaterials.Length; i++)
                {
                    if (mr.sharedMaterials[i] == placeholderMaterial)
                        newMaterials[i] = capturedMaterial;
                }

                mr.materials = newMaterials;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, captureRadius);
        }
    }
}