﻿using Gisha.MechJam.AI;
using UnityEngine;

namespace Gisha.MechJam.Core
{
    public enum InteractionMode
    {
        Build,
        Command,
        CommandsCount
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private int _maxAllyUnits;
        private int _steelCount;
        public bool IsSustainableAmountOfAllyUnits => CurrentAllyUnits + 1 <= MaxAllyUnits;
        public int MaxAllyUnits => _maxAllyUnits;
        public int CurrentAllyUnits => FindObjectsOfType<AllyUnitAI>().Length;
        public int SteelCount => _steelCount;

        public InteractionMode InteractionMode { get; private set; }

        private void Awake()
        {
            Instance = this;
            InteractionMode = InteractionMode.Build;
        }

        public void ChangeInteractionMode(InteractionMode modeToChange)
        {
            InteractionMode = modeToChange;
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