using System;
using Gisha.MechJam.Core;
using UnityEngine;

namespace Gisha.MechJam.World.Building.Structures
{
    public class BarracksStructure : Structure
    {
        [SerializeField] private int allyExtension = 3;

        public static Action BarracksBuilt;
        
        protected override void Start()
        {
            base.Start();

            var barracks = FindObjectsOfType<BarracksStructure>();
            GameManager.Instance.UpdateAllyUnits(barracks.Length * allyExtension);

            Debug.Log($"Current barracks capacity: {GameManager.Instance.MaxAllyUnits}");

            BarracksBuilt?.Invoke();
        }
    }
}