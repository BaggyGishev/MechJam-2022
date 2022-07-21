using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gisha.MechJam.UI
{
    // Disables connected components.
    public class UIDeactivator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static Action PointerEntered;
        public static Action PointerExited;

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEntered?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExited?.Invoke();
        }
    }
}