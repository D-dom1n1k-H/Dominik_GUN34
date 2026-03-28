using System;
using UnityEngine;

namespace Korven.Core.Services
{
    public sealed class UIService : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private Camera mainCamera;

        private RectTransform _canvasTransform;

        private void Awake()
        {
            _canvasTransform = canvas.GetComponent<RectTransform>();
        }

        public Vector2 GetUIPointByScreenPoint(Vector2 screenPoint)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasTransform,
                screenPoint, mainCamera,
                out var result);

            return result;
        }

        public Rect GetUIRectByScreenPoints(Vector2 startScreenPoint, Vector2 endScreenPoint)
        {
            startScreenPoint = GetUIPointByScreenPoint(startScreenPoint);
            endScreenPoint = GetUIPointByScreenPoint(endScreenPoint);

            Vector2 center = (startScreenPoint + endScreenPoint) / 2;
            Vector2 vector = endScreenPoint - startScreenPoint;
            Vector2 size = new Vector2(MathF.Abs(vector.x), MathF.Abs(vector.y));
            return new Rect(center, size);
        }
    }
}