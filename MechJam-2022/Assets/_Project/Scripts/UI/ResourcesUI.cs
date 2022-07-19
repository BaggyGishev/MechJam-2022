using System;
using Gisha.MechJam.World.Building.Structures;
using Gisha.MechJam.AI;
using Gisha.MechJam.Core;
using TMPro;
using UnityEngine;

namespace Gisha.MechJam.UI
{
    public class ResourcesUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text capacityText;
        [SerializeField] private TMP_Text steelCountText;

        private void Start()
        {
            UpdateCapacityUI();
            UpdateSteelCountUI();
        }

        private void OnEnable()
        {
            MineStructure.SteelProduced += UpdateSteelCountUI;

            AllyUnitAI.AllyUnitDestroyed += UpdateCapacityUI;
            BarracksStructure.BarracksBuilt += UpdateCapacityUI;
            FactoryStructure.MechProduced += UpdateCapacityUI;
        }

        private void OnDisable()
        {
            MineStructure.SteelProduced -= UpdateSteelCountUI;

            AllyUnitAI.AllyUnitDestroyed -= UpdateCapacityUI;
            BarracksStructure.BarracksBuilt -= UpdateCapacityUI;
            FactoryStructure.MechProduced -= UpdateCapacityUI;
        }

        private void UpdateSteelCountUI()
        {
            steelCountText.text = GameManager.Instance.SteelCount.ToString();
        }

        private void UpdateCapacityUI()
        {
            capacityText.text = $"{GameManager.Instance.CurrentAllyUnits}/{GameManager.Instance.MaxAllyUnits}";
        }
    }
}