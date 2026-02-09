using UnityEngine;

namespace Korven.GamePlay.Units.BawlingBall
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class BawlingBall : MonoBehaviour
    {
        [SerializeField, Range(1f, 1000f)]
        private float throwForce;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void ThrowMe(Vector3 direction)
        {
            direction.Normalize();
            _rigidbody.AddForce(direction * throwForce, ForceMode.Impulse);
        }
    }
}