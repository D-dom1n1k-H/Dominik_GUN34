using System.Collections.Generic;
using UnityEngine;

namespace Korven.GamePlay.Units.MarkersView
{
    public sealed class SelectionMarkerViewPool : MonoBehaviour
    {
        [SerializeField]
        private SelectionMarkerView prefab;
        [SerializeField]
        private Transform activeContainer;
        [SerializeField]
        private Transform inactiveContainer;
        [SerializeField]
        private int initialSize = 32;

        private readonly Queue<SelectionMarkerView> _markers = new();

        private void Awake()
        {
            for (int i = 0; i < initialSize; i++)
            {
                var marker = Instantiate(prefab, inactiveContainer);
                _markers.Enqueue(marker);
            }
        }

        public SelectionMarkerView Get()
        {
            if (_markers.TryDequeue(out var marker))
            {
                marker.transform.SetParent(activeContainer);
                return marker;
            }

            return Instantiate(prefab, activeContainer);
        }

        public void Release(SelectionMarkerView marker)
        {
            marker.transform.SetParent(inactiveContainer);
            _markers.Enqueue(marker);
        }
    }
}