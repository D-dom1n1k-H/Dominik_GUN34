using UnityEngine;
using UnityEngine.InputSystem;

namespace Necro.GamePlay.Player.FrogMovement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FrogMovement : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 30f)]
        private float moveSpeed = 4f;
        [SerializeField, Range(0f, 50f)]
        private float jumpForce = 14f;
        [SerializeField, Range(0.1f, 10f)]
        private float gravityForce = 3f;
        [SerializeField]
        private GameObject ground;
        [SerializeField, Header("Animation Settings")]
        private Animator animator;

        private Rigidbody2D _rb;
        private Keyboard _keyboard;
        private Vector2 _velocity;
        private bool _isOnTheGround;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = gravityForce;
        }

        private void Update()
        {
            HandleJump();
        }

        private void FixedUpdate()
        {
            _keyboard = Keyboard.current;
            HandleMovement();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject == ground)
            {
                _isOnTheGround = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject == ground)
            {
                _isOnTheGround = false;
            }
        }

        private void HandleMovement()
        {
            if (_keyboard.leftArrowKey.isPressed)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                animator.SetBool("isRunning", true);
                _rb.velocity = new Vector2(-moveSpeed, _rb.velocity.y);
            }
            else if (_keyboard.rightArrowKey.isPressed)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                animator.SetBool("isRunning", true);
                _rb.velocity = new Vector2(moveSpeed, _rb.velocity.y);
            }
            else if (!_keyboard.leftArrowKey.isPressed || !_keyboard.rightArrowKey.isPressed)
            {
                animator.SetBool("isRunning", false);
            }
        }

        private void HandleJump()
        {
            if (_isOnTheGround && _keyboard.spaceKey.wasPressedThisFrame)
            {
                animator.SetBool("isRunning", false);
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            }
        }
    }
}