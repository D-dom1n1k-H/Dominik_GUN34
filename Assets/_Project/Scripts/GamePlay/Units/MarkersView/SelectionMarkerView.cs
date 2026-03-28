using UnityEngine;

namespace Korven.GamePlay.Units.MarkersView
{
    public sealed class SelectionMarkerView : MonoBehaviour
    {
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}