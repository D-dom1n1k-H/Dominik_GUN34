using Korven.Core.Services;
using UnityEngine;

namespace Korven.GamePlay.Player.RectSelection
{
    public sealed class RectSelectionController : MonoBehaviour
    {
        [SerializeField]
        private RectSelectionInput rectSelectionInput;
        [SerializeField]
        private RectSelectionView rectSelectionView;
        [SerializeField]
        private UIService uiService;

        private void Update()
        {
            if (rectSelectionInput.IsSelecting)
            {
                Rect rect = uiService.GetUIRectByScreenPoints(rectSelectionInput.StartPoint,
                    rectSelectionInput.EndPoint);
                rectSelectionView.SetPositions(rect);
                rectSelectionView.SetVisible(true);
            }
            else
            {
                rectSelectionView.SetVisible(false);
            }
        }
    }
}