using System.Collections.Generic;
using Korven.GamePlay.Units.UnitSelection;
using UnityEngine;

namespace Korven.GamePlay.Units.MarkersView
{
    public sealed class SelectionMarkersViewAdapter : MonoBehaviour
    {
        [SerializeField]
        private SelectionMarkerViewPool pool;
        [SerializeField, Header("Stack")]
        private SelectedUnitsStack stack;

        private readonly Dictionary<GameObject, SelectionMarkerView> _markers = new();

        private void OnEnable()
        {
            stack.OnUnitsChanged += OnUnitsChanged;
        }

        private void OnDisable()
        {
            stack.OnUnitsChanged -= OnUnitsChanged;
        }

        private void OnUnitsChanged(IEnumerable<GameObject> units)
        {
            DestroyMarkers();
            SpawnMarkers(units);
        }

        private void SpawnMarkers(IEnumerable<GameObject> units)
        {
            foreach (var unit in units)
            {
                var marker = pool.Get();
                marker.SetPosition(unit.transform.position);
                _markers.Add(unit, marker);
            }
        }

        private void DestroyMarkers()
        {
            foreach (var marker in _markers.Values)
            {
                pool.Release(marker);
            }

            _markers.Clear();
        }
    }
}