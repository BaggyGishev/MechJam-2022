using UnityEngine;

namespace Gisha.MechJam.AI
{
    public class EnemyUnitAI : UnitAI
    {
        private void Start()
        {
            LayerToAttack = 1 << LayerMask.NameToLayer("Ally");
        }
    }
}