using Gisha.MechJam.World;
using UnityEngine;

namespace Gisha.MechJam.Core
{
    public class CameraController : MonoBehaviour
    {
        [Header("Movement")] [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float movementSmoothness = 0.1f;
        [SerializeField] private float movementMouseSens = 1f;
        [Header("Rotation")] [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float rotationSmoothness = 0.1f;
        [SerializeField] private float rotationMouseSens = 1f;
        [Header("Height")] [SerializeField] private float heightSmoothness = 0.1f;
        [SerializeField] private float heightStepsCount = 5;
        [SerializeField] private float minHeight, maxHeight = 3f;

        private Quaternion _newRotation;
        private float _yDeltaRotation, _height, _heightStep;
        private Vector3 _newPos, _dragStartPos, _dragCurrentPos, _rotateStartPos, _rotateCurrentPos;
        private Vector3 _top, _bottom;
        private Vector3 _movementInput;
        bool _isLargeView;
        
        private void Start()
        {
            _yDeltaRotation = transform.rotation.eulerAngles.y;
            _newPos = transform.position;
            _newRotation = transform.rotation;
            
            _height = transform.position.y;
            _heightStep = (maxHeight - minHeight) / heightStepsCount;

            _top = GridManager.Grid.GetWorldPosFromCoords(GridManager.Grid.LastCell.Coords);
            _bottom = GridManager.Grid.GetWorldPosFromCoords(GridManager.Grid.FirstCell.Coords);
        }

        private void Update()
        {
            HandleMouseRotation();
            HandleKeyboardRotation();

            if (!_isLargeView)
            {
                HandleMouseMovement();
                HandleKeyboardMovement();

                HandleHeight();
                transform.position = Vector3.Lerp(transform.position, _newPos, Time.deltaTime / movementSmoothness);
            }

            transform.rotation =
                Quaternion.Slerp(transform.rotation, _newRotation, Time.deltaTime / rotationSmoothness);
        }

        #region Keyboard

        void HandleKeyboardRotation()
        {
            float rotationInput;
            if (Input.GetKey(KeyCode.Q))
                rotationInput = 1f;
            else if (Input.GetKey(KeyCode.E))
                rotationInput = -1f;
            else
                return;

            _yDeltaRotation = rotationInput * rotationSpeed * Time.deltaTime;
            _newRotation *= Quaternion.Euler(Vector3.up * _yDeltaRotation);
        }

        void HandleKeyboardMovement()
        {
            Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            Vector3 f = movementInput.z * transform.forward * movementSpeed * Time.deltaTime;
            Vector3 h = movementInput.x * transform.right * movementSpeed * Time.deltaTime;

            _newPos += h + f;
            _newPos.x = Mathf.Clamp(_newPos.x, _bottom.x, _top.x);
            _newPos.y = _height;
            _newPos.z = Mathf.Clamp(_newPos.z, _bottom.z, _top.z);
        }

        #endregion

        #region Mouse

        void HandleMouseMovement()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float entry;

                if (plane.Raycast(ray, out entry))
                    _dragStartPos = ray.GetPoint(entry);
            }

            if (Input.GetMouseButton(1))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float entry;

                if (plane.Raycast(ray, out entry))
                {
                    _dragCurrentPos = ray.GetPoint(entry);

                    _newPos += (_dragStartPos - _dragCurrentPos) * movementMouseSens;
                    _newPos.x = Mathf.Clamp(_newPos.x, _bottom.x, _top.x);
                    _newPos.y = _height;
                    _newPos.z = Mathf.Clamp(_newPos.z, _bottom.z, _top.z);

                    transform.position = Vector3.Lerp(transform.position, _newPos, Time.deltaTime / movementSmoothness);
                }
            }
        }

        void HandleMouseRotation()
        {
            if (Input.GetMouseButtonDown(2))
                _rotateStartPos = Input.mousePosition;
            if (Input.GetMouseButton(2))
            {
                _rotateCurrentPos = Input.mousePosition;

                float diff = _rotateStartPos.x - _rotateCurrentPos.x;
                _rotateStartPos = _rotateCurrentPos;

                _yDeltaRotation = diff * rotationMouseSens;
                _newRotation *= Quaternion.Euler(-Vector3.up * _yDeltaRotation);
            }
        }

        void HandleHeight()
        {
            float height;
            if (Input.mouseScrollDelta.y > 0f)
            {
                height = -_heightStep;
                _isLargeView = false;
            }
            else if (Input.mouseScrollDelta.y < 0f)
            {
                height = _heightStep;
                _isLargeView = false;
            }
            else
                return;

            _height = Mathf.Clamp(height + _height, minHeight, maxHeight);
            float yPos = Mathf.Lerp(transform.position.y, _height, Time.deltaTime / heightSmoothness);
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        }

        #endregion
    }
}