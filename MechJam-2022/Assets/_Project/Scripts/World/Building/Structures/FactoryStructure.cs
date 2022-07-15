﻿using System.Collections;
using Gisha.MechJam.Core;
using UnityEngine;

namespace Gisha.MechJam.World.Building.Structures
{
    public class FactoryStructure : Structure
    {
        [SerializeField] private GameObject mechPrefab;
        [SerializeField] private Transform spawnpoint;
        [Space] [SerializeField] private float produceDelay;
        [SerializeField] private int steelPerMech = 10;


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

                if (GameManager.Instance.IsSustainableAmountOfAllyUnits &&
                    GameManager.Instance.SteelCount >= steelPerMech)
                {
                    Instantiate(mechPrefab, spawnpoint.position, Quaternion.identity);
                    GameManager.Instance.AddSteelCount(-steelPerMech);
                }
            }
        }
    }
}