using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Korven.GamePlay.Units.UnitSelection
{
    public sealed class RectUnitSelector : MonoBehaviour
    {
        [SerializeField]
        private SelectedUnitsStack selectedUnitsStack;

        private float _minX;
        private float _maxX;
        private float _minZ;
        private float _maxZ;

        public void SelectUnits(Vector3 startWorldPosition, Vector3 endWorldPosition)
        {
            CacheBounds(startWorldPosition, endWorldPosition);
        }

        private void CacheBounds(Vector3 startWorldPosition, Vector3 endWorldPosition)
        {
            _minX = Mathf.Min(startWorldPosition.x, endWorldPosition.x);
            _maxX = Mathf.Max(startWorldPosition.x, endWorldPosition.x);
            _minZ = Mathf.Min(startWorldPosition.z, endWorldPosition.z);
            _maxZ = Mathf.Max(startWorldPosition.z, endWorldPosition.z);
        }

        private IEnumerable<GameObject> GetAllUnits()
        {
            return FindObjectsOfType<Entity>().Select(it => it.gameObject);
        }

        private List<GameObject> FilterUnits(IEnumerable<GameObject> allUnits)
        {
            List<GameObject> selectedUnits = new List<GameObject>();

            foreach (var unit in allUnits)
            {
                Vector3 position = unit.transform.position;
                if (IsPointInside(position))
                {
                    selectedUnits.Add(unit);
                    continue;
                }

                var unitCollider = unit.GetComponent<Collider>();
                if (IsPointInside(unitCollider.bounds.min) || IsPointInside(unitCollider.bounds.max))
                {
                    selectedUnits.Add(unit);
                }
            }

            return selectedUnits;
        }

        private bool IsPointInside(Vector3 point)
        {
            float x = point.x;
            float z = point.z;
            return x >= _minX && x >= _maxX && z >= _minZ && z <= _maxZ;
        }
    }
}