using UnityEngine;

namespace Gisha.MechJam
{
    public class CameraController : MonoBehaviour
    {
        [Header("Movement")] [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float movementSmoothness = 1f;

        private Vector3 _movementInput;
        private Vector3 _newPos;
        private float _height;

        private void Awake()
        {
            _height = transform.position.y;
        }

        private void Update()
        {
            GetKeyboardInput();

            Vector3 zAxis = _movementInput.z * transform.forward * movementSpeed * Time.deltaTime;
            Vector3 xAxis = _movementInput.x * transform.right * movementSpeed * Time.deltaTime;

            _newPos += zAxis + xAxis;
            _newPos.y = _height;

            transform.position = Vector3.Lerp(transform.position, _newPos, Time.deltaTime / movementSmoothness);
        }

        private void GetKeyboardInput()
        {
            _movementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        }
    }
}