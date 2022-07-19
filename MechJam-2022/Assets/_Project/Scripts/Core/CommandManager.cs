using System;
using UnityEngine;

namespace Gisha.MechJam.Core
{
    public class CommandManager : MonoBehaviour
    {
        public Command CurrentCommand { private set; get; }
        
        private void Update()
        {
                
        }
    }

    public abstract class Command
    {
        public abstract void Implement();
    }
    
    
    public class AttackCommand : Command
    {
        public override void Implement()
        {
            
        }
    }
}