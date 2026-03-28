using UnityEngine;
using UnityEngine.UI;

namespace Korven.GamePlay.Player.RectSelection
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public sealed class RectSelectionView : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Image _image;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
        }

        public void SetPositions(Rect rect)
        {
            _rectTransform.sizeDelta = rect.size;
            _rectTransform.anchoredPosition = new Vector2(rect.x, rect.y);
        }

        public void SetVisible(bool isVisible)
        {
            _image.enabled = isVisible;
        }
    }
}