using Gisha.MechJam.World.Building;
using UnityEngine;

namespace Gisha.MechJam.Core
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/Game Data", order = 0)]
    public class GameData : ScriptableObject
    {
        [SerializeField] private StructureData[] structuresData;
    }
}