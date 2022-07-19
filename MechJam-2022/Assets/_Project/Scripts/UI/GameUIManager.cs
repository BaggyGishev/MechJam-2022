using Gisha.MechJam.Core;
using UnityEngine;

namespace Gisha.MechJam.UI
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject buildPanel;
        [SerializeField] private GameObject commandPanel;

        private int _interactionModeIndex;

        public void OnClick_ModeChange()
        {
            _interactionModeIndex++;
            if (_interactionModeIndex > (int) InteractionMode.CommandsCount - 1)
                _interactionModeIndex = 0;

            GameManager.Instance.ChangeInteractionMode((InteractionMode) _interactionModeIndex);
            UpdateUI((InteractionMode) _interactionModeIndex);
        }

        private void UpdateUI(InteractionMode interactionMode)
        {
            switch (interactionMode)
            {
                case InteractionMode.Build:
                    buildPanel.SetActive(true);
                    commandPanel.SetActive(false);
                    break;
                case InteractionMode.Command:
                    buildPanel.SetActive(false);
                    commandPanel.SetActive(true);
                    break;
            }
        }
    }
}