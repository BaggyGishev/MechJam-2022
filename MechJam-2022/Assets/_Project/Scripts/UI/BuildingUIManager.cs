using Gisha.MechJam.Core;
using Gisha.MechJam.World.Building;
using UnityEngine;
using UnityEngine.UI;

namespace Gisha.MechJam.UI
{
    public class BuildingUIManager : MonoBehaviour
    {
        public static BuildingUIManager Instance { get; private set; }

        [Header("Structures")] [SerializeField]
        private Transform structureElementsParent;

        [SerializeField] private GameObject structureElementPrefab;

        [Header("Build Modes")] [SerializeField]
        private Sprite buildModeSprite;

        [SerializeField] private Sprite destroyModeSprite;
        [SerializeField] private Image buildModeImage;


        private void Awake()
        {
            Instance = this;
            var gameData = ResourcesGetter.GameData;

            for (int i = 0; i < gameData.StructuresData.Length; i++)
            {
                var element = Instantiate(structureElementPrefab, structureElementsParent)
                    .GetComponent<StructureUIElement>();
                element.Setup(gameData.StructuresData[i]);
            }
        }

        public void UpdateBuildMode(BuildMode buildMode)
        {
            switch (buildMode)
            {
                case BuildMode.Build:
                    buildModeImage.sprite = buildModeSprite;
                    break;
                case BuildMode.Destroy:
                    buildModeImage.sprite = destroyModeSprite;
                    break;
            }
        }
    }
}