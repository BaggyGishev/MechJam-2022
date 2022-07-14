using System.Collections;
using UnityEngine;

namespace Gisha.MechJam.AI
{
    public class EnemyUnitAI : UnitAI
    {
        private Vector3 holdPosition;

        public override void Start()
        {
            base.Start();
            LayerToAttack = 1 << LayerMask.NameToLayer("Ally");
            holdPosition = transform.position;
        }

        protected override IEnumerator CustomAIRoutine()
        {
            if (Mathf.Abs(Vector3.SqrMagnitude(holdPosition - transform.position)) < 1)
                yield break;

            SetDestination(holdPosition);
            yield return null;
        }
    }
}