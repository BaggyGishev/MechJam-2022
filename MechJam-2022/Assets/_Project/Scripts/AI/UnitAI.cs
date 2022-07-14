using System;
using Gisha.MechJam.Core;
using UnityEngine;

namespace Gisha.MechJam.AI
{
    [RequireComponent(typeof(NavObstacleAgent))]
    public abstract class UnitAI : MonoBehaviour, IDamageable
    {
        [SerializeField] private float rotationSmoothness;
        [SerializeField] private float attackRadius;
        [SerializeField] private float followRadius;

        private Transform AttackTarget { get; set; }
        protected LayerMask LayerToAttack { get; set; }
        protected Action TargetDestroyed;

        private NavObstacleAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavObstacleAgent>();
        }

        private void Update()
        {
            if (AttackTarget == null)
            {
                AttackTarget = CheckAreaForTarget();
                return;
            }

            if (Vector3.Distance(AttackTarget.transform.position, transform.position) < followRadius)
            {
                SetDestination(AttackTarget.position);

                if (Vector3.Distance(AttackTarget.transform.position, transform.position) < attackRadius)
                {
                    // Stop and Attack Target.
                    AttackTarget.GetComponent<IDamageable>().GetDamage(1);

                    if (AttackTarget == null)
                        TargetDestroyed?.Invoke();
                }
            }
        }

        private void LateUpdate()
        {
            UpdateRotation();
        }

        protected void SetDestination(Vector3 pos)
        {
            _agent.SetDestination(pos);
        }

        private Transform CheckAreaForTarget()
        {
            var colls = Physics.OverlapSphere(transform.position, followRadius, LayerToAttack);
            Transform result = null;

            if (colls.Length > 0)
                result = colls[0].transform;

            if (result != null)
                Debug.Log("Target acquired!");

            return result;
        }

        private void UpdateRotation()
        {
            if (_agent.Velocity == Vector3.zero || AttackTarget == null)
                return;

            var rotation = Quaternion.LookRotation(_agent.Velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSmoothness * Time.deltaTime);
        }


        public void GetDamage(float damage)
        {
            Debug.Log("Damaged! " + gameObject.name);
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, followRadius);
        }
    }
}