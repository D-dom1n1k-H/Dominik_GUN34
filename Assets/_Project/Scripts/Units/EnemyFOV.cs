using System.Collections;
using Korven.GamePlay.Units.Enemys;
using UnityEngine;

namespace Korven.GamePlay.Units.EnemysFOV
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyFOV : MonoBehaviour
    {
        [SerializeField]
        private float radius = 8f;
        [SerializeField, Header("ReadOnly, (to change this field, you need to change some code)")]
        private Vector3 origin;
        [SerializeField]
        private LayerMask obstacleMask;

        private Transform _target;
        private Enemy _enemy;
        private bool _canSeePlayer;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();

            _target = _enemy.GetTarget();
        }

        private void Start()
        {
            StartCoroutine(CheckFieldOfViewCoroutine());
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;

            origin = transform.position;
            Gizmos.DrawWireSphere(origin, radius);

            if (_canSeePlayer)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(origin, _target.position);
            }
        }

        public bool GetCanSeePlayer() => _canSeePlayer;

        private IEnumerator CheckFieldOfViewCoroutine()
        {
            WaitForSeconds wait = new WaitForSeconds(0.2f);

            while (true)
            {
                CheckForPlayer();
                yield return wait;
            }
        }

        private void CheckForPlayer()
        {
            Vector3 directionToTarget = (_target.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, _target.position);

            if (distanceToTarget > radius)
            {
                _canSeePlayer = false;
            }
            else
            {
                if (!Physics.Raycast(origin, directionToTarget, distanceToTarget, obstacleMask))
                {
                    _canSeePlayer = true;
                }
                else
                {
                    _canSeePlayer = false;
                }
            }
        }
    }
}