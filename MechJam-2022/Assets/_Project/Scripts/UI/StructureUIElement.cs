using System;
using Gisha.MechJam.World.Building;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gisha.MechJam.UI
{
    public class StructureUIElement : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TMP_Text structureNameText;
        [SerializeField] private Image structureImage;

        public static Action<StructureData> OnStructureSelected;
        public static Action OnStructureDeselected;

        private bool _isSelected;
        private StructureData _structureData;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isSelected)
                OnStructureSelected?.Invoke(_structureData);
            else
                OnStructureDeselected?.Invoke();

            _isSelected = !_isSelected;
        }

        public void Setup(StructureData newStructureData)
        {
            _structureData = newStructureData;

            structureImage.sprite = _structureData.StructureSprite;
            structureNameText.text = _structureData.name;
        }
    }
}