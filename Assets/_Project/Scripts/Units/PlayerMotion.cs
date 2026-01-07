using System.Collections;
using Korven.GamePlay.Units.Payers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Korven.GamePlay.Units.PayerMotion
{
    [RequireComponent(typeof(Player))]
    public class PlayerMotion : MonoBehaviour
    {
        [SerializeField]
        private float speed = 10f;

        private Player _player;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _player = GetComponent<Player>();
            _rigidbody = GetComponent<Rigidbody>();

            if (_rigidbody == null) Debug.LogError($"<b>[{nameof(PlayerMotion)}]</b> {nameof(_rigidbody)} is null");
        }

        private void Start()
        {
            StartCoroutine(MovementCoroutine());
        }

        private IEnumerator MovementCoroutine()
        {
            WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

            while (true)
            {
                HoldMotion();
                yield return waitForFixedUpdate;
            }
        }

        private void HoldMotion()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            if (keyboard.wKey.isPressed)
            {
                _rigidbody.AddForce(Vector3.forward.normalized * speed);
            }

            if (keyboard.aKey.isPressed)
            {
                _rigidbody.AddForce(Vector3.left.normalized * speed);
            }

            if (keyboard.sKey.isPressed)
            {
                _rigidbody.AddForce(Vector3.back.normalized * speed);
            }

            if (keyboard.dKey.isPressed)
            {
                _rigidbody.AddForce(Vector3.right.normalized * speed);
            }
        }
    }
}