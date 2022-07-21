using System;
using Gisha.MechJam.Core;
using UnityEngine;

namespace Gisha.MechJam.World.Building.Structures
{
    public class BarracksStructure : Structure
    {
        [SerializeField] private int allyExtension = 3;

        public static Action BarracksModified;

        protected override void Start()
        {
            base.Start();
            OnModify();
        }

        private void OnModify()
        {
            var barracks = FindObjectsOfType<BarracksStructure>();
            GameManager.Instance.UpdateAllyUnits(barracks.Length * allyExtension);

            BarracksModified?.Invoke();
        }

        private void OnDestroy()
        {
            OnModify();
        }
    }
}