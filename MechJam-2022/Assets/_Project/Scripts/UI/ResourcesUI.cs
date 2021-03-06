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
        [SerializeField] private TMP_Text energyCountText;

        private void Start()
        {
            UpdateCapacityUI();
            UpdateSteelCountUI();
            UpdateEnergyUI();
        }

        private void OnEnable()
        {
            MineStructure.SteelProduced += UpdateSteelCountUI;

            AllyUnitAI.AllyUnitDestroyed += UpdateCapacityUI;
            BarracksStructure.BarracksModified += UpdateCapacityUI;
            FactoryStructure.MechProduced += UpdateCapacityUI;
            GameManager.EnergyCountChanged += UpdateEnergyUI;
        }

        private void OnDisable()
        {
            MineStructure.SteelProduced -= UpdateSteelCountUI;

            AllyUnitAI.AllyUnitDestroyed -= UpdateCapacityUI;
            BarracksStructure.BarracksModified -= UpdateCapacityUI;
            FactoryStructure.MechProduced -= UpdateCapacityUI;
            GameManager.EnergyCountChanged -= UpdateEnergyUI;
        }

        private void UpdateSteelCountUI()
        {
            steelCountText.text = GameManager.Instance.SteelCount.ToString();
        }

        private void UpdateCapacityUI()
        {
            capacityText.text = $"{GameManager.Instance.CurrentAllyUnits}/{GameManager.Instance.MaxAllyUnits}";
        }

        private void UpdateEnergyUI()
        {
            energyCountText.text = GameManager.Instance.EnergyCount.ToString();
        }
    }
}