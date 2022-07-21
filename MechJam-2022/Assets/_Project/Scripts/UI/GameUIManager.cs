using Gisha.MechJam.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gisha.MechJam.UI
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject buildPanel;
        [SerializeField] private GameObject commandPanel;
        [Space] [SerializeField] private Image interactionModeImage;
        [SerializeField] private TMP_Text interactionModeText;
        [SerializeField] private Color buildModeColor, commandModeColor;


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
                    interactionModeImage.color = buildModeColor;
                    interactionModeText.text = "Build";
                    break;
                case InteractionMode.Command:
                    buildPanel.SetActive(false);
                    commandPanel.SetActive(true);
                    interactionModeImage.color = commandModeColor;
                    interactionModeText.text = "Command";
                    break;
            }
        }
    }
}