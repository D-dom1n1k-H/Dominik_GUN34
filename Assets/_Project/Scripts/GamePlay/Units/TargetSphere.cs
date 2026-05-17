using System;
using Korven.GamePlay.NCPs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Korven.GamePlay.Units
{
    public sealed class TargetSphere : MonoBehaviour
    {
        [SerializeField]
        private CharacterAI characterAI;
        [SerializeField]
        private Vector3[] randomPositions;

        private void Awake()
        {
            if (randomPositions.Length == 0)
            {
                throw new NullReferenceException("randomPositions array is empty!");
            }
        }

        private void Start()
        {
            GetRandomPosition();
        }

        private void OnEnable()
        {
            characterAI.OnTargetSphereCollectedEvent += TeleportToRandomPosition;
        }

        private void OnDisable()
        {
            characterAI.OnTargetSphereCollectedEvent -= TeleportToRandomPosition;
        }

        private void TeleportToRandomPosition()
        {
            Debug.Log("<color=yellow>TeleportToRandomPosition method was called</color>");
            GetRandomPosition();
        }

        private void GetRandomPosition()
        {
            var index = Random.Range(0, randomPositions.Length);

            transform.position = randomPositions[index];
        }
    }
}