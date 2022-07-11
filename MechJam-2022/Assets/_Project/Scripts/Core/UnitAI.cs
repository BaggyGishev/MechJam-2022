using System;
using UnityEngine;
using UnityEngine.AI;

namespace Gisha.MechJam.Core
{
    public class UnitAI : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [Space] [SerializeField] private float rotationSmoothness;


        private NavMeshAgent _navMeshAgent;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.updateRotation = false;
        }

        private void Start()
        {
            _navMeshAgent.SetDestination(target.position);
        }

        private void LateUpdate()
        {
            if (_navMeshAgent.velocity == Vector3.zero)
                return;

            var rotation = Quaternion.LookRotation(_navMeshAgent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSmoothness * Time.deltaTime);
        }
    }
}