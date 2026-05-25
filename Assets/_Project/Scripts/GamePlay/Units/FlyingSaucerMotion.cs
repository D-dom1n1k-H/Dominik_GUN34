using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Korven.Gameplay.Units
{
    public sealed class FlyingSaucerMotion : MonoBehaviour
    {
        [SerializeField]
        private Vector3 startPosition;
        [SerializeField]
        private Vector3 initialLocalScale;
        [SerializeField]
        private Vector3 lastWayPoint;
        [SerializeField]
        private Vector3[] wayPoints;
        [SerializeField]
        private Vector3 endRotation;
        [SerializeField]
        private float duration;

        private void Update()
        {
            var keyboard = Keyboard.current;

            if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame)
            {
                transform.position = startPosition;
                transform.localScale = initialLocalScale;

                transform.DOPath(wayPoints, duration, PathType.CatmullRom)
                    .OnComplete(GoToTheLastWaypoint);

                transform.DORotate(endRotation, duration, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear)
                    .SetLoops(15, LoopType.Restart);
            }
        }

        private void GoToTheLastWaypoint()
        {
            transform.DOMove(lastWayPoint, duration / 4).SetEase(Ease.InOutSine);
            transform.DOScale(Vector3.zero, duration / 4).SetEase(Ease.InOutSine);
        }
    }
}