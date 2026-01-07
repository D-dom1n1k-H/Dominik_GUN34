using Korven.GamePlay.Units.EnemysFOV;
using Korven.GamePlay.Units.EnemysMotion;
using UnityEngine;

namespace Korven.GamePlay.Units.Enemys
{
    [RequireComponent(typeof(EnemyFOV))]
    [RequireComponent(typeof(EnemyMotion))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        private EnemyFOV _enemyFOV;
        private EnemyMotion _enemyMotion;

        private void Awake()
        {
            _enemyFOV = GetComponent<EnemyFOV>();
            _enemyMotion = GetComponent<EnemyMotion>();
        }

        public Transform GetTarget() => target;
        
        ///*
        /// Main class for enemies
        ///*
    }
}