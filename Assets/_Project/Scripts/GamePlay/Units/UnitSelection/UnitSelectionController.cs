using Korven.GamePlay.Player.RectSelection;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Korven.GamePlay.Units.UnitSelection
{
    public sealed class UnitSelectionController : MonoBehaviour
    {
        private static Plane _groundPlane = new Plane(Vector3.up, Vector3.zero);

        [SerializeField]
        private RectSelectionInput rectSelectionInput;
        [SerializeField]
        private RectUnitSelector rectUnitSelector;

        private void OnEnable()
        {
            rectSelectionInput.OnFinished += OnSelectionFinished;
        }

        private void OnDisable()
        {
            rectSelectionInput.OnFinished -= OnSelectionFinished;
        }

        private void OnSelectionFinished()
        {
            Vector2 startScreenPoint = rectSelectionInput.StartPoint;
            Vector2 endScreenPoint = rectSelectionInput.EndPoint;

            Vector3 startWorldPoint = GetGroundPosition(startScreenPoint);
            Vector3 endWorldPoint = GetGroundPosition(endScreenPoint);
            rectUnitSelector.SelectUnits(startWorldPoint, endWorldPoint);
        }

        private Vector3 GetGroundPosition(Vector2 screenPoint)
        {
            Debug.Assert(Camera.main != null, "Camera.main != null");
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
            _groundPlane.Raycast(ray, out var distance);
            Vector3 groundPoint = ray.GetPoint(distance);
            return groundPoint;
        }
    }
}