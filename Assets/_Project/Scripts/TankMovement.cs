using UnityEngine;
using UnityEngine.InputSystem;

namespace Necro.GamePlay.Player.TankMovement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TankMovement : MonoBehaviour
    {
        [SerializeField, Range(0.001f, 1f)]
        private float moveSpeed = 0.01f;
        [SerializeField, Range(0, 10)]
        private int stabilizeMotionSpeed = 1;

        private Vector2 _moveInput;

        private Rigidbody2D _rb;
        private Keyboard _keyboard;

        private bool _isMoveKeyPressed;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _keyboard = Keyboard.current;
            if (_keyboard == null) return;

            _isMoveKeyPressed = false;

            #region MotionLogic

            // Straight line motion

            if (_keyboard.wKey.isPressed && !_keyboard.aKey.isPressed && !_keyboard.dKey.isPressed)
            {
                _moveInput.y += 1;
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                _isMoveKeyPressed = true;
            }

            if (_keyboard.aKey.isPressed)
            {
                _moveInput.x -= 1;
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                _isMoveKeyPressed = true;
            }

            if (_keyboard.sKey.isPressed && !_keyboard.aKey.isPressed && !_keyboard.dKey.isPressed)
            {
                _moveInput.y -= 1;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                _isMoveKeyPressed = true;
            }

            if (_keyboard.dKey.isPressed)
            {
                _moveInput.x += 1;
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                _isMoveKeyPressed = true;
            }

            // Diagonal motion

            if (_keyboard.wKey.isPressed && _keyboard.aKey.isPressed)
            {
                _moveInput.y += 1;
                _moveInput.x -= 1;
                transform.rotation = Quaternion.Euler(0f, 0f, -135f);
                _isMoveKeyPressed = true;
            }

            if (_keyboard.wKey.isPressed && _keyboard.dKey.isPressed)
            {
                _moveInput.y += 1;
                _moveInput.x += 1;
                transform.rotation = Quaternion.Euler(0f, 0f, 135f);
                _isMoveKeyPressed = true;
            }

            if (_keyboard.sKey.isPressed && _keyboard.aKey.isPressed)
            {
                _moveInput.y -= 1;
                _moveInput.x -= 1;
                transform.rotation = Quaternion.Euler(0f, 0f, -45f);
                _isMoveKeyPressed = true;
            }

            if (_keyboard.sKey.isPressed && _keyboard.dKey.isPressed)
            {
                _moveInput.y -= 1;
                _moveInput.x += 1;
                transform.rotation = Quaternion.Euler(0f, 0f, 45f);
                _isMoveKeyPressed = true;
            }

            if (!_isMoveKeyPressed)
            {
                StabilizeMovement();
            }

            // _moveInput.Normalize();

            #endregion
        }

        private void FixedUpdate()
        {
            _rb.velocity = _moveInput * moveSpeed;
        }

        private void StabilizeMovement()
        {
            switch (_moveInput.x)
            {
                case < 0:
                    _moveInput.x += stabilizeMotionSpeed;
                    break;
                case > 0:
                    _moveInput.x -= stabilizeMotionSpeed;
                    break;
            }

            switch (_moveInput.y)
            {
                case < 0:
                    _moveInput.y += stabilizeMotionSpeed;
                    break;
                case > 0:
                    _moveInput.y -= stabilizeMotionSpeed;
                    break;
            }
        }
    }
}