using UnityEngine;
using UnityEngine.InputSystem;

namespace Korven.GamePlay.Player
{
    public sealed class BuildingGrid : MonoBehaviour
    {
        [SerializeField]
        private Vector2Int gridSize = new Vector2Int(10, 10);
        private Building[,] _gird;
        private Building _currentBuilding;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _gird = new Building[gridSize.x, gridSize.y];
        }

        private void Update()
        {
            ChoosePlace();
        }

        public void StartPlacingBuildings(Building buildingPrefab)
        {
            if (_currentBuilding != null)
            {
                Destroy(_currentBuilding.gameObject);
            }

            _currentBuilding = Instantiate(buildingPrefab);
        }

        private void ChoosePlace()
        {
            if (_currentBuilding != null)
            {
                var groundPlane = new Plane(Vector3.up, Vector3.zero);
                var mouse = Mouse.current;

                Vector2 mousePos = mouse.position.ReadValue();
                Ray ray = _mainCamera.ScreenPointToRay(mousePos);

                if (groundPlane.Raycast(ray, out float position))
                {
                    Vector3 worldPosition = ray.GetPoint(position);

                    int x = Mathf.RoundToInt(worldPosition.x);
                    int z = Mathf.RoundToInt(worldPosition.z);

                    var isPlaceAvailable = true;

                    if (x < 0 || x > gridSize.x - _currentBuilding.Size.x || z < 0 ||
                        z > gridSize.y - _currentBuilding.Size.y)
                    {
                        isPlaceAvailable = false;
                    }
                    else
                    {
                        isPlaceAvailable = true;
                    }

                    if (isPlaceAvailable && IsPlaceTaken(x, z))
                    {
                        isPlaceAvailable = false;
                    }

                    _currentBuilding.transform.position = new Vector3(x, worldPosition.y, z);
                    _currentBuilding.SetTransparent(isPlaceAvailable);

                    if (mouse.leftButton.wasPressedThisFrame && isPlaceAvailable)
                    {
                        PlaceCurrentBuilding(x, z);
                    }
                }
            }
        }

        private bool IsPlaceTaken(int placeX, int placeZ)
        {
            for (int x = 0; x < _currentBuilding.Size.x; x++)
            {
                for (int z = 0; z < _currentBuilding.Size.y; z++)
                {
                    if (_gird[placeX + x, placeZ + z] != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void PlaceCurrentBuilding(int placeX, int placeZ)
        {
            for (int x = 0; x < _currentBuilding.Size.x; x++)
            {
                for (int z = 0; z < _currentBuilding.Size.y; z++)
                {
                    _gird[placeX + x, placeZ + z] = _currentBuilding;
                }
            }

            _currentBuilding.SetNormalColor();
            _currentBuilding = null;
        }
    }
}