using System;
using Gisha.MechJam.Core;
using Gisha.MechJam.UI;
using Gisha.MechJam.World;
using Gisha.MechJam.World.Targets;
using UnityEngine;

namespace Gisha.MechJam.AI
{
    public class CommandManager : MonoBehaviour
    {
        [SerializeField] private CommandVisualizer commandVisualizer;

        public static Command CurrentCommand { private set; get; }

        public static Action<Command> CommandSent;

        private LayerMask _targetLayerMask;
        private bool _isDisabled;

        private void Awake()
        {
            _targetLayerMask = 1 << LayerMask.NameToLayer("Area");
        }

        private void Update()
        {
            if (GameManager.InteractionMode != InteractionMode.Command || _isDisabled)
                return;

            if (Input.GetMouseButtonDown(0))
                CommandRaycast();
        }

        private void OnEnable()
        {
            UIDeactivator.PointerEntered += OnPointerEntered;
            UIDeactivator.PointerExited += OnPointerExited;
            Outpost.OutpostCaptured += OnOutpostCaptured;
        }

        private void OnDisable()
        {
            UIDeactivator.PointerEntered -= OnPointerEntered;
            UIDeactivator.PointerExited -= OnPointerExited;
            Outpost.OutpostCaptured -= OnOutpostCaptured;
        }

        private void OnOutpostCaptured()
        {
            commandVisualizer.Hide();
        }

        private void OnPointerExited()
        {
            _isDisabled = false;
        }

        private void OnPointerEntered()
        {
            _isDisabled = true;
        }

        private void CommandRaycast()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, 1000f, _targetLayerMask))
            {
                var area = hitInfo.collider.GetComponent<Area>();

                if (!area.IsAlly)
                    CurrentCommand = new Command(area.transform, CommandType.Attack);
                else
                    CurrentCommand = new Command(area.transform, CommandType.Defense);

                CommandSent?.Invoke(CurrentCommand);
                CurrentCommand.Implement();
                commandVisualizer.Visualize(CurrentCommand);
            }
        }
    }

    public enum CommandType
    {
        Attack,
        Defense
    }

    public class Command
    {
        public Transform Target => _target;
        public CommandType CommandType => _commandType;

        private Transform _target;
        private CommandType _commandType;

        public Command(Transform target, CommandType commandType)
        {
            _target = target;
            _commandType = commandType;
        }

        internal void Implement()
        {
            Debug.Log($"Command type: {_commandType}, on {_target.position}");
        }
    }


    [Serializable]
    public class CommandVisualizer
    {
        [SerializeField] private GameObject visualizer;
        [SerializeField] private Sprite attackSprite, defenseSprite;
        [SerializeField] private Color attackColor, defenseColor;

        private SpriteRenderer _spriteRenderer;

        public void Visualize(Command command)
        {
            if (_spriteRenderer == null)
                _spriteRenderer = visualizer.GetComponent<SpriteRenderer>();

            _spriteRenderer.enabled = true;
            switch (command.CommandType)
            {
                case CommandType.Attack:
                    _spriteRenderer.sprite = attackSprite;
                    _spriteRenderer.color = attackColor;
                    break;
                case CommandType.Defense:
                    _spriteRenderer.sprite = defenseSprite;
                    _spriteRenderer.color = defenseColor;
                    break;
            }

            visualizer.transform.position = command.Target.position;
        }

        public void Hide()
        {
            _spriteRenderer.enabled = false;
        }
    }
}