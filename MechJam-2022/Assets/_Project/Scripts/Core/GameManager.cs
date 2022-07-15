using Gisha.MechJam.AI;
using UnityEngine;

namespace Gisha.MechJam.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; set; }

        private int _maxAllyUnits;
        private int _steelCount;
        public bool IsSustainableAmountOfAllyUnits => CurrentAllyUnits + 1 <= MaxAllyUnits;
        public int MaxAllyUnits => _maxAllyUnits;
        public int CurrentAllyUnits => FindObjectsOfType<AllyUnitAI>().Length;
        
        public int SteelCount => _steelCount;
        

        private void Awake()
        {
            Instance = this;
        }

        public void UpdateAllyUnits(int count)
        {
            _maxAllyUnits = count;
        }

        public void AddSteelCount(int count)
        {
            _steelCount += count;
        }
    }
}