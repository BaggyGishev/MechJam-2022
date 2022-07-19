using System;
using System.Collections;
using System.Linq;
using Gisha.MechJam.Core;
using Gisha.MechJam.World.Targets;
using UnityEngine;

namespace Gisha.MechJam.AI
{
    public class AllyUnitAI : UnitAI
    {
        private Transform _priorityTarget { get; set; } // Base or outpost.

        public static Action AllyUnitDestroyed;

        public override void Start()
        {
            base.Start();
            LayerToAttack = 1 << LayerMask.NameToLayer("Enemy");
        }

        private void OnEnable()
        {
            CommandManager.CommandSent += OnCommandSent;
        }

        private void OnDisable()
        {
            CommandManager.CommandSent -= OnCommandSent;
        }

        private void OnCommandSent(Command cmd)
        {
            var attackCommand = (AttackCommand) cmd;
            _priorityTarget = attackCommand.Target;
        }

        protected override IEnumerator CustomAIRoutine()
        {
            MoveTowardsPriorityTarget();
            yield return null;
        }

        protected override void Die()
        {
            base.Die();
            AllyUnitDestroyed?.Invoke();
        }

        private void MoveTowardsPriorityTarget()
        {
            if (CommandManager.CurrentCommand != null)
            {
                var attackCmd = (AttackCommand) CommandManager.CurrentCommand;
                _priorityTarget = attackCmd.Target;
            }
            else
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