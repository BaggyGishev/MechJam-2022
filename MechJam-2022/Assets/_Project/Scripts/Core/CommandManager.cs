using System;
using Gisha.MechJam.World.Building;
using UnityEngine;

namespace Gisha.MechJam.Core
{
    public class CommandManager : MonoBehaviour
    {
        public static Command CurrentCommand { private set; get; }

        public static Action<Command> CommandSent;

        private LayerMask _targetLayerMask;

        private void Awake()
        {
            _targetLayerMask = 1 << LayerMask.NameToLayer("Area");
        }

        private void Update()
        {
            if (GameManager.InteractionMode != InteractionMode.Command)
                return;

            if (Input.GetMouseButtonDown(0))
                CommandRaycast();
        }

        private void CommandRaycast()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, 1000f, _targetLayerMask))
            {
                var area = hitInfo.collider.GetComponent<Area>();

                if (!area.IsAlly)
                {
                    CurrentCommand = new AttackCommand(area.transform);
                    CurrentCommand.Implement();
                    CommandSent?.Invoke(CurrentCommand);
                }
            }
        }
    }

    public abstract class Command
    {
        internal abstract void Implement();
    }


    public class AttackCommand : Command
    {
        private Transform _target;

        public Transform Target => _target;

        public AttackCommand(Transform target)
        {
            _target = target;
        }

        internal override void Implement()
        {
            Debug.Log($"Attack on: {Target.name}");
        }
    }
}