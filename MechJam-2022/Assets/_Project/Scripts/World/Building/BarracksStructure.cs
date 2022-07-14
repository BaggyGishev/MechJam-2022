using Gisha.MechJam.Core;
using UnityEngine;

namespace Gisha.MechJam.World.Building
{
    public class BarracksStructure : Structure
    {
        [SerializeField] private int allyExtension = 3;

        protected override void Start()
        {
            base.Start();

            var barracks = FindObjectsOfType<BarracksStructure>();
            GameManager.Instance.UpdateAllyUnits(barracks.Length * allyExtension);

            Debug.Log($"Current barracks capacity: {GameManager.Instance.MaxAllyUnits}");
        }
    }
}