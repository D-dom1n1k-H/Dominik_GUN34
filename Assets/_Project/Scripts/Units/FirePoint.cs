using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Korven.GamePlay.Units.FirePoint
{
    public sealed class FirePoint : MonoBehaviour
    {
        [SerializeField, Header("=== Bowling Balls Prefabs ===")]
        private GameObject bowlingBlueBall;
        [SerializeField]
        private GameObject bowlingGreenBall;
        [SerializeField]
        private GameObject bowlingYellowBall;

        [SerializeField, Space(20f), Header("=== Aim Settings ===")]
        private GameObject aim;
        [SerializeField, Range(1f, 100f)]
        private float aimRotationSpeed = 50f;
        [SerializeField, Range(0f, 45f)]
        private float aimMaxAndMinRotationYAngle = 30f;
        private float _currentAimYRotationAngle;

        private GameObject _currentBawlingBall;
        private BawlingBall.BawlingBall[] _bawlingBalls;

        private int _launchCount;
        private bool _isCurrentTurnFinished;

        private void Start()
        {
            _currentAimYRotationAngle = transform.localEulerAngles.y;
            if (_currentAimYRotationAngle > 180) _currentAimYRotationAngle -= 360; // 0 -360 -> -180 180

            _isCurrentTurnFinished = true;

            aim.SetActive(false);

            StartCoroutine(BowlingBallChooserCoroutine());
            StartCoroutine(BawlingBallSenderCoroutine());
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<BowlingPin.BowlingPin>())
            {
                collision.rigidbody.velocity = (Vector3.up + Vector3.right) * 10f;
                print(collision.gameObject.name);
            }

            print("Collision");
        }

        private IEnumerator BowlingBallChooserCoroutine()
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();

            while (true)
            {
                if (_isCurrentTurnFinished)
                {
                    var keyboard = Keyboard.current;
                    if (keyboard == null) yield break;

                    aim.SetActive(false);

                    if (keyboard.digit1Key.wasPressedThisFrame)
                    {
                        _currentBawlingBall = Instantiate(bowlingBlueBall, this.transform.position,
                            this.transform.rotation);
                        _isCurrentTurnFinished = false;
                    }

                    if (keyboard.digit2Key.wasPressedThisFrame)
                    {
                        _currentBawlingBall = Instantiate(bowlingGreenBall, this.transform.position,
                            this.transform.rotation);
                        _isCurrentTurnFinished = false;
                    }

                    if (keyboard.digit3Key.wasPressedThisFrame)
                    {
                        _currentBawlingBall = Instantiate(bowlingYellowBall, this.transform.position,
                            this.transform.rotation);
                        _isCurrentTurnFinished = false;
                    }

                    yield return waitForEndOfFrame;
                }

                yield return waitForEndOfFrame;
            }
        }

        private IEnumerator BawlingBallSenderCoroutine()
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();

            while (true)
            {
                if (!_isCurrentTurnFinished)
                {
                    var keyboard = Keyboard.current;
                    var mouse = Mouse.current;
                    if (keyboard == null) yield break;
                    if (mouse == null) yield break;

                    aim.SetActive(true);

                    if (keyboard.spaceKey.wasPressedThisFrame)
                    {
                        _bawlingBalls = FindObjectsOfType<BawlingBall.BawlingBall>();

                        if (_bawlingBalls.Length > 1)
                        {
                            for (int i = 0; i < _bawlingBalls.Length - 1; i++)
                            {
                                Destroy(_bawlingBalls[i + 1].gameObject); // удаляет мяч который дальше, костыль
                            }
                        }

                        _currentBawlingBall.GetComponent<BawlingBall.BawlingBall>().ThrowMe(this.transform.right);
                        _isCurrentTurnFinished = true;
                        _launchCount++;
                    }
                    else
                    {
                        // FirePoint is "Pivot" for aim object 

                        if (keyboard.rightArrowKey.isPressed)
                        {
                            _currentAimYRotationAngle += aimRotationSpeed * Time.deltaTime;
                            _currentAimYRotationAngle = Mathf.Clamp(_currentAimYRotationAngle,
                                -aimMaxAndMinRotationYAngle, aimMaxAndMinRotationYAngle);
                            transform.localEulerAngles = new Vector3(0, _currentAimYRotationAngle, 0);
                        }

                        if (keyboard.leftArrowKey.isPressed)
                        {
                            _currentAimYRotationAngle += -aimRotationSpeed * Time.deltaTime;
                            _currentAimYRotationAngle = Mathf.Clamp(_currentAimYRotationAngle,
                                -aimMaxAndMinRotationYAngle, aimMaxAndMinRotationYAngle);
                            transform.localEulerAngles = new Vector3(0, _currentAimYRotationAngle, 0);
                        }
                    }

                    yield return waitForEndOfFrame;
                }

                yield return waitForEndOfFrame;
            }
        }

        public int GetLaunchCount() => _launchCount;
    }
}