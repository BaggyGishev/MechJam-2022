using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Gisha.MechJam.AI
{
    [RequireComponent(typeof(NavMeshAgent), typeof(NavMeshObstacle))]
    public class NavObstacleAgent : MonoBehaviour
    {
        [SerializeField] private float carvingTime = 0.5f;
        [SerializeField] private float carvingMoveThreshold = 0.1f;

        public Vector3 Velocity => _navMeshAgent.velocity;

        private NavMeshAgent _navMeshAgent;
        private NavMeshObstacle _navMeshObstacle;

        private float _lastMoveTime;
        private Vector3 _lastPosition;

        private void Awake()
        {
            _navMeshObstacle = GetComponent<NavMeshObstacle>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.updateRotation = false;

            _navMeshObstacle.enabled = false;
            _navMeshObstacle.carveOnlyStationary = false;
            _navMeshObstacle.carving = true;
        }

        // Enable agent when moving, enable obstacle when stationary. 
        private void Update()
        {
            if (Vector3.Distance(_lastPosition, transform.position) > carvingMoveThreshold)
            {
                _lastMoveTime = Time.time;
                _lastPosition = transform.position;
            }

            if (_lastMoveTime + carvingTime < Time.time)
            {
                _navMeshAgent.enabled = false;
                _navMeshObstacle.enabled = true;
            }
        }

        private void OnDisable()
        {
            _navMeshAgent.enabled = false;
            _navMeshObstacle.enabled = false;
        }
        
        public void SetDestination(Vector3 position)
        {
            _navMeshObstacle.enabled = false;

            _lastMoveTime = Time.time;
            _lastPosition = transform.position;

            StartCoroutine(MoveAgentRoutine(position));
        }
        
        private IEnumerator MoveAgentRoutine(Vector3 pos)
        {
            yield return null;
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(pos);
        }
    }
}