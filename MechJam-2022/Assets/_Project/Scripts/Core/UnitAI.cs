using System;
using UnityEngine;
using UnityEngine.AI;

namespace Gisha.MechJam.Core
{
    public class UnitAI : MonoBehaviour
    {
        [Space] [SerializeField] private float rotationSmoothness;

        private Transform _target;
        private NavObstacleAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavObstacleAgent>();
        }

        private void Update()
        {
            if (_target != null)
                return;

            _target = FindNearestTarget();
            _agent.SetDestination(_target.position);
        }

        private void LateUpdate()
        {
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            if (_agent.Velocity == Vector3.zero || _target == null)
                return;

            var rotation = Quaternion.LookRotation(_agent.Velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSmoothness * Time.deltaTime);
        }


        private Transform FindNearestTarget()
        {
            float minDist = Mathf.Infinity;
            var targets = GameObject.FindGameObjectsWithTag("Target");
            Transform result = targets[0].transform;

            for (int i = 0; i < targets.Length; i++)
            {
                float dist = Vector3.SqrMagnitude(targets[i].transform.position - transform.position);
                if (dist < minDist)
                {
                    result = targets[i].transform;
                    minDist = dist;
                }
            }

            return result;
        }
    }
}