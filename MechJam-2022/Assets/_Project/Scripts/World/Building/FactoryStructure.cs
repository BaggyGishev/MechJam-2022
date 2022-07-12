using System.Collections;
using UnityEngine;

namespace Gisha.MechJam.World.Building
{
    public class FactoryStructure : Structure
    {
        [SerializeField] private GameObject mechPrefab;
        [SerializeField] private Transform spawnpoint;
        [SerializeField] private float produceDelay;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(MechProductionRoutine());
        }

        private IEnumerator MechProductionRoutine()
        {
            yield return new WaitForSeconds(produceDelay);
            Instantiate(mechPrefab, spawnpoint.position, Quaternion.identity);
        }
    }
}