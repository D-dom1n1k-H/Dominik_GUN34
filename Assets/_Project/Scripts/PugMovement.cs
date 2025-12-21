using UnityEngine;
using UnityEngine.InputSystem;

namespace Necro.GamePlay.Player.PugMovement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PugMovement : MonoBehaviour
    {
        [SerializeField, Range(0.001f, 60f)]
        private float moveSpeed = 2f;

        private Vector2 _moveInput;

        private Rigidbody2D _rb;
        private Keyboard _keyboard;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _keyboard = Keyboard.current;
            if (_keyboard == null) return;

            #region MotionLogic

            _moveInput = Vector2.zero;

            if (_keyboard.upArrowKey.isPressed)
            {
                _moveInput.y += 1;
            }

            if (_keyboard.leftArrowKey.isPressed)
            {
                _moveInput.x -= 1;
            }

            if (_keyboard.downArrowKey.isPressed)
            {
                _moveInput.y -= 1;
            }

            if (_keyboard.rightArrowKey.isPressed)
            {
                _moveInput.x += 1;
            }

            _moveInput.Normalize();

            #endregion
        }

        private void FixedUpdate()
        {
            _rb.velocity = _moveInput * moveSpeed;
        }
    }
}