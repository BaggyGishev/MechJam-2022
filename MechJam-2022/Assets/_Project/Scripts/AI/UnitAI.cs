using System;
using System.Collections;
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
        [Space] [SerializeField] private float maxHealth;
        [SerializeField] private float attackDelay;

        private Transform AttackTarget { get; set; }
        protected LayerMask LayerToAttack { get; set; }

        private NavObstacleAgent _agent;
        private float _health;

        public virtual void Awake()
        {
            _agent = GetComponent<NavObstacleAgent>();
        }

        public virtual void Start()
        {
            _health = maxHealth;
            StartCoroutine(AIRoutine());
        }

        public abstract IEnumerator CustomAIRoutine();

        private IEnumerator AIRoutine()
        {
            while (true)
            {
                if (AttackTarget == null)
                {
                    AttackTarget = CheckAreaForTarget();
                    yield return CustomAIRoutine();
                }
                else
                {
                    if (Vector3.Distance(AttackTarget.transform.position, transform.position) < followRadius)
                    {
                        SetDestination(AttackTarget.position);

                        if (Vector3.Distance(AttackTarget.transform.position, transform.position) < attackRadius)
                        {
                            // Stop and Attack Target.
                            yield return new WaitForSeconds(attackDelay);

                            if (AttackTarget != null)
                                AttackTarget.GetComponent<IDamageable>().GetDamage(1);
                        }
                    }
                }

                yield return null;
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
            _health -= damage;

            if (_health <= 0)
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