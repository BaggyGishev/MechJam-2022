using UnityEngine;

namespace Gisha.MechJam.Core
{
    public static class ResourcesGetter
    {
        public static GameData GameData => (GameData)Resources.Load("Data/GameData", typeof(GameData));
    }
}