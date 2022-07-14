using Gisha.MechJam.AI;
using UnityEngine;

namespace Gisha.MechJam.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; set; }

        private int _maxAllyUnits;
        public bool IsSustainableAmountOfAllyUnits => FindObjectsOfType<AllyUnitAI>().Length <= MaxAllyUnits;
        public int MaxAllyUnits => _maxAllyUnits;

        private void Awake()
        {
            Instance = this;
        }

        public void UpdateAllyUnits(int count)
        {
            _maxAllyUnits = count;
        }
    }
}