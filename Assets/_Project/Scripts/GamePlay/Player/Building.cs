using UnityEngine;

namespace Korven.GamePlay.Player
{
    public sealed class Building : MonoBehaviour
    {
        [SerializeField]
        private Vector2Int size = Vector2Int.one;
        private MeshRenderer _meshRenderer;
        public Vector2Int Size => size;

        private void Awake()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        private void OnDrawGizmos()
        {
            for (int x = 0; x < Size.x; x++)
            {
                for (int z = 0; z < Size.y; z++)
                {
                    if ((x + z) % 2 == 0)
                    {
                        Gizmos.color = new Color(0.88f, 1, 0.3f);
                    }
                    else
                    {
                        Gizmos.color = new Color(1f, 0.68f, 0.3f);
                    }

                    Gizmos.DrawCube(transform.position + new Vector3(x, 0f, z), new Vector3(1f, 0.1f, 1f));
                }
            }
        }

        public void SetTransparent(bool isAvailable)
        {
            if (isAvailable)
            {
                _meshRenderer.material.color = Color.green;
            }
            else
            {
                _meshRenderer.material.color = Color.red;
            }
        }

        public void SetNormalColor()
        {
            _meshRenderer.material.color = Color.white;
        }
    }
}