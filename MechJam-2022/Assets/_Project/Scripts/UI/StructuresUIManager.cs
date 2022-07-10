using Gisha.MechJam.Core;
using UnityEngine;

namespace Gisha.MechJam.UI
{
    public class StructuresUIManager : MonoBehaviour
    {
        [SerializeField] private Transform structureElementsParent;
        [SerializeField] private GameObject structureElementPrefab;
        
        private void Awake()
        {
            var gameData = ResourcesGetter.GameData;
            
            for (int i = 0; i < gameData.StructuresData.Length; i++)
            {
                var element = Instantiate(structureElementPrefab, structureElementsParent)
                    .GetComponent<StructureUIElement>();
                element.Setup(gameData.StructuresData[i]);
            }
        }
    }
}