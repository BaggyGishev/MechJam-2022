using System;
using Gisha.MechJam.World.Building;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gisha.MechJam.UI
{
    public class StructureUIElement : MonoBehaviour, IPointerClickHandler
    {
        public static Action<StructureData> OnStructureSelected;

        private StructureData _structureData;
        private TMP_Text _text;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnStructureSelected?.Invoke(_structureData);
        }

        public void Setup(StructureData newStructureData)
        {
            _text = GetComponentInChildren<TMP_Text>();
            _structureData = newStructureData;
            
            _text.text = _structureData.name;
        }
    }
}