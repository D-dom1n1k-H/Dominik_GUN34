using System.Collections;
using Korven.GamePlay.Units.Enemys;
using Korven.GamePlay.Units.EnemysFOV;
using UnityEngine;

namespace Korven.GamePlay.Units.EnemysMotion
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyMotion : MonoBehaviour
    {
        [SerializeField]
        private float speed = 8f;

        private Transform _target;

        private Rigidbody _rigidbody;
        private Enemy _enemy;
        private EnemyFOV _enemyFOV;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _enemy = GetComponent<Enemy>();
            _enemyFOV = GetComponent<EnemyFOV>();
            _target = _enemy.GetTarget();

            if (_rigidbody == null) Debug.LogError($"<b>[{nameof(EnemyMotion)}]</b> {nameof(_rigidbody)} is null");
        }

        private void Start()
        {
            StartCoroutine(CheckFieldOfViewCoroutine());
        }

        private IEnumerator CheckFieldOfViewCoroutine()
        {
            WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

            while (true)
            {
                HoldMotion();
                yield return waitForFixedUpdate;
            }
        }

        private void HoldMotion()
        {
            if (_enemyFOV.GetCanSeePlayer())
            {
                Vector3 direction = (_target.position - transform.position).normalized;
                _rigidbody.AddForce(direction * speed);
            }
        }
    }
}