using System.Collections;
using Gisha.MechJam.Core;
using UnityEngine;
using UnityEngine.AI;

namespace Gisha.MechJam.AI
{
    [RequireComponent(typeof(NavObstacleAgent))]
    public abstract class UnitAI : MonoBehaviour, IDamageable
    {
        [Header("General")] [SerializeField] private Transform topMount;
        [Header("Rotation")] [SerializeField] private float bodyRotationSmoothness = 3f;
        [SerializeField] private float turretRotationSmoothness = 3f;
        [Header("Other")] [SerializeField] private float attackRadius = 5f;
        [SerializeField] private float followRadius = 10f;
        [Space] [SerializeField] private float maxHealth = 5f;
        [SerializeField] private float attackDelay = 1f;

        private Transform AttackTarget { get; set; }
        protected LayerMask LayerToAttack { get; set; }

        private NavObstacleAgent _agent;
        private UnitAnimationController _animationController;

        private float _health;
        private bool _isDestroyed;

        public virtual void Awake()
        {
            _agent = GetComponent<NavObstacleAgent>();
            _animationController = new UnitAnimationController(GetComponent<Animator>());
        }

        public virtual void Start()
        {
            _health = maxHealth;
            StartCoroutine(AIRoutine());
        }

        private void LateUpdate()
        {
            if (_isDestroyed)
                return;

            if (_agent.Velocity != Vector3.zero)
            {
                UpdateBodyRotation();
                _animationController.StartMovementAnimation(false);
            }
            else
                _animationController.StartMovementAnimation(true);

            UpdateTurretRotation();
        }

        #region AI

        protected abstract IEnumerator CustomAIRoutine();

        private IEnumerator AIRoutine()
        {
            while (!_isDestroyed)
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
                            {
                                if (AttackTarget.TryGetComponent(out IDamageable damageable))
                                    damageable.GetDamage(1);
                            }
                        }
                    }
                }

                yield return null;
            }
        }

        #endregion

        public void SetDestination(Vector3 pos)
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

        private void UpdateBodyRotation()
        {
            var rotation = Quaternion.LookRotation(_agent.Velocity.normalized);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, rotation, bodyRotationSmoothness * Time.deltaTime);
        }

        private void UpdateTurretRotation()
        {
            if (AttackTarget == null)
                return;

            var direction = (AttackTarget.position - transform.position).normalized;
            var rotation = Quaternion.LookRotation(direction);
            topMount.rotation =
                Quaternion.Slerp(topMount.rotation, rotation, turretRotationSmoothness * Time.deltaTime);
        }


        public void GetDamage(float damage)
        {
            _health -= damage;

            if (_health <= 0)
                Die();
        }

        protected virtual void Die()
        {
            _animationController.StartDeathAnimation();
            Destroy(gameObject, 1.25f);
            _agent.enabled = false;
            enabled = false;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, followRadius);
        }
    }

    public class UnitAnimationController
    {
        private Animator _animator;

        public UnitAnimationController(Animator animator)
        {
            _animator = animator;
        }

        public void StartMovementAnimation(bool isIdle)
        {
            _animator.SetBool("IsIdle", isIdle);
        }

        public void StartDeathAnimation()
        {
            _animator.SetTrigger("Die");
        }
    }
}