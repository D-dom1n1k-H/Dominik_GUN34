using UnityEngine;

namespace Korven.GamePlay.Units.BowlingPin
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class BowlingPin : MonoBehaviour
    {
        [SerializeField]
        private GameObject origin;
        [SerializeField]
        private BowlingPinSummit.BowlingPinSummit summit;
        private PointScore.PointScore _pointScore;

        private bool _isFallen = false;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _pointScore = FindObjectOfType<PointScore.PointScore>();
        }

        private void OnEnable()
        {
            summit.OnFlourCollisionEnteredEvent += OnFlourCollisionEntered;
        }

        private void OnDisable()
        {
            summit.OnFlourCollisionEnteredEvent -= OnFlourCollisionEntered;
        }

        private void OnFlourCollisionEntered()
        {
            if (!_isFallen)
            {
                _pointScore.AddScore(1);
                _isFallen = true;
            }
        }
        
        public bool GetIsFallen() => _isFallen;
    }
}