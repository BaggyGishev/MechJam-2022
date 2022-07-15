using System.Collections;
using Gisha.MechJam.Core;
using UnityEngine;

namespace Gisha.MechJam.World.Building.Structures
{
    public class MineStructure : Structure
    {
        [SerializeField] private float produceDelayInSeconds = 2f;
        [SerializeField] private int countPerIteration = 5;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(SteelProductionRoutine());
        }

        private IEnumerator SteelProductionRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(produceDelayInSeconds);
                GameManager.Instance.AddSteelCount(countPerIteration);
                Debug.Log($"Current metal count is: {GameManager.Instance.SteelCount}");
            }
        }
    }
}