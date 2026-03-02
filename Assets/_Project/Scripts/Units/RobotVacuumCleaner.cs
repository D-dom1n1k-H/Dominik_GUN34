using UnityEngine;

namespace Korven.GamePlay.Units
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class RobotVacuumCleaner : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 1f), Space(10f), Header("========== RaycastSettings ==========")]
        private float rayLenght = 0.2f;
        [SerializeField]
        private LayerMask obstacleLayerMask;

        [SerializeField, Range(0.01f, 1f), Space(10f), Header("========== MotionSettings ==========")]
        private float speed = 0.5f;
        [SerializeField, Range(1f, 100f)]
        private float rotationSpeed = 50f;
        private Quaternion _startRotation;
        private Quaternion _targetRotation;
        private float _rotationProgress;
        private bool _isMoving = true;
        private bool _isRotating;

        private RaycastHit _raycastHit;
        private Ray _frontRay;
        private Ray _rightRay;
        private Ray _leftRay;

        [SerializeField, Header("=========== AudioSettings ==========")]
        private AudioSource robotVacuumCleanerAudioSource;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            SetAndDrawRays();

            if (Physics.Raycast(_frontRay, out var hit, rayLenght, obstacleLayerMask))
            {
                if (!(Physics.Raycast(_rightRay, rayLenght,
                        obstacleLayerMask) && !Physics.Raycast(_leftRay, rayLenght, obstacleLayerMask)))
                {
                    SetRandomDirection();
                }

                if (!Physics.Raycast(_rightRay, rayLenght, obstacleLayerMask))
                {
                    SetLeftDirection();
                }
                else if (!Physics.Raycast(_leftRay, rayLenght, obstacleLayerMask))
                {
                    SetRightDirection();
                }

                Debug.Log(hit.collider.gameObject.name);
            }

            if (_isMoving)
                MoveForward();

            if (_isRotating)
                Rotate();
        }

        //       private void OnCollisionEnter(Collision collision) RobotVacuumCleaner это empty game object без коллайдера 
        //      {
        //          if (collision.collider.gameObject.layer == obstacleLayerMask)
        //         {
        //              robotVacuumCleanerAudioSource.Play();
        //         }
        //      }


        private void MoveForward()
        {
            _rb.MovePosition(_rb.position + transform.forward * (speed * Time.deltaTime));
        }

        private void SetNewDirection(float value)
        {
            if (_isRotating) return;

            _isRotating = true;
            _isMoving = false;

            _rotationProgress = 0f;
            _startRotation = _rb.rotation;
            _targetRotation = _startRotation * Quaternion.Euler(0f, value, 0f);
        }

        private void SetRightDirection() => SetNewDirection(-90f);
        private void SetLeftDirection() => SetNewDirection(90f);
        private void SetRandomDirection() => SetNewDirection(Random.Range(-180f, 180));

        private void SetAndDrawRays()
        {
            _frontRay = new Ray(transform.position + new Vector3(0f, 0.02f, 0f), transform.forward);
            _rightRay = new Ray(transform.position + new Vector3(0f, 0.02f, 0f), transform.right);
            _leftRay = new Ray(transform.position + new Vector3(0f, 0.02f, 0f), -transform.right);
            Debug.DrawRay(_frontRay.origin, _frontRay.direction * rayLenght, Color.green);
            Debug.DrawRay(_rightRay.origin, _rightRay.direction * rayLenght, Color.cyan);
            Debug.DrawRay(_leftRay.origin, _leftRay.direction * rayLenght, Color.cyan);
        }


        private void Rotate()
        {
            _rotationProgress += rotationSpeed * Time.deltaTime;

            float t = _rotationProgress / 90f; //конвертация в %, для интерполяции 

            Quaternion newRotation = Quaternion.Slerp(_startRotation, _targetRotation, t);

            _rb.MoveRotation(newRotation);
            if (t >= 1f)
            {
                _rb.MoveRotation(_targetRotation);

                _isRotating = false;
                _isMoving = true;
            }
        }
    }
}