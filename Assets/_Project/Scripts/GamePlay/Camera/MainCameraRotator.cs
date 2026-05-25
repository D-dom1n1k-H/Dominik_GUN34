using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Korven.Gameplay.Camera
{
    public sealed class MainCameraRotator : MonoBehaviour
    {
        [SerializeField]
        private Quaternion startRotation;
        [SerializeField]
        private Vector3 endRotation;
        [SerializeField]
        private float duration;


        private void Update()
        {
            var keyboard = Keyboard.current;

            if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame)
            {
                transform.rotation = startRotation;

                transform.DORotate(endRotation, duration)
                    .SetEase(Ease.OutCubic);
            }
        }
    }
}