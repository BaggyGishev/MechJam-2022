using System;
using Gisha.MechJam.World.Building;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gisha.MechJam.UI
{
    public class StructureUIButton : Button
    {
        [SerializeField] private StructureData structureData;

        public static Action<StructureData> OnStructureSelected;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            OnStructureSelected?.Invoke(structureData);
        }
    }
}