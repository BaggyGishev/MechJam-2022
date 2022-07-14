using System.Collections;
using Gisha.MechJam.Core;
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
            while (true)
            {
                yield return new WaitForSeconds(produceDelay);

                if (GameManager.Instance.IsSustainableAmountOfAllyUnits)
                    Instantiate(mechPrefab, spawnpoint.position, Quaternion.identity);
            }
        }
    }
}