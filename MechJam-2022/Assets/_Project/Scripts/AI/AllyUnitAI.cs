using System.Collections;
using System.Linq;
using Gisha.MechJam.World.Targets;
using UnityEngine;

namespace Gisha.MechJam.AI
{
    public class AllyUnitAI : UnitAI
    {
        private Transform _priorityTarget { get; set; } // Base or outpost.

        public override void Start()
        {
            base.Start();
            LayerToAttack = 1 << LayerMask.NameToLayer("Enemy");
        }

        protected override IEnumerator CustomAIRoutine()
        {
            MoveTowardsPriorityTarget();
            yield return null;
        }

        private void MoveTowardsPriorityTarget()
        {
            _priorityTarget = FindNearestPriorityTarget();
            SetDestination(_priorityTarget.position);
        }

        // Find nearest not captured target. (outpost, base)
        private Transform FindNearestPriorityTarget()
        {
            float minDist = Mathf.Infinity;
            var targets = FindObjectsOfType<Target>()
                .Where(x => !x.IsCaptured)
                .ToArray();
            
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