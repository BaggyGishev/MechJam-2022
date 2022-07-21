using System.Collections;
using System.Collections.Generic;
using Gisha.MechJam.AI;
using UnityEngine;

namespace Gisha.MechJam.World.Targets
{
    public class Base : Outpost
    {
        [Header("Base variables")] [SerializeField]
        private float defenderSpawnDelay;

        [SerializeField] private GameObject defenderPrefab;
        [SerializeField] private Transform spawnPoint;

        private int _initialDefendersCount;

        public override void Start()
        {
            base.Start();
            _initialDefendersCount = GetComponentsInChildren<EnemyUnitAI>().Length;

            StartCoroutine(CheckDefendersCount());
        }

        private IEnumerator CheckDefendersCount()
        {
            while (true)
            {
                yield return new WaitForSeconds(defenderSpawnDelay);
                if (GetComponentsInChildren<EnemyUnitAI>().Length < _initialDefendersCount)
                {
                    Vector3 offset = new Vector3(Random.Range(5f, 7f), 0f, Random.Range(5f, 7f));
                    var defender = Instantiate(defenderPrefab, spawnPoint.position, spawnPoint.rotation)
                        .GetComponent<UnitAI>();
                    defender.transform.SetParent(transform);

                    Vector3 navPosition = transform.position + offset;
                    defender.SetDestination(navPosition);
                }
            }
        }
    }
}