using System;
using UnityEngine;

namespace Gisha.MechJam.AI
{
    public class AllyUnitAI : UnitAI
    {
        private Transform _priorityTarget { get; set; } // Base or outpost.

        private void OnEnable()
        {
            TargetDestroyed += MoveTowardsPriorityTarget;
        }

        private void OnDisable()
        {
            TargetDestroyed -= MoveTowardsPriorityTarget;
        }

        private void Start()
        {
            LayerToAttack = 1 << LayerMask.NameToLayer("Enemy");

            // Searching for nearest enemy base or outpost.
            _priorityTarget = FindNearestPriorityTarget();
            MoveTowardsPriorityTarget();
        }

        private void MoveTowardsPriorityTarget()
        {
            SetDestination(_priorityTarget.position);
        }

        private Transform FindNearestPriorityTarget()
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