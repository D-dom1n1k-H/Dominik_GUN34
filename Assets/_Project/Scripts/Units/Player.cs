using Korven.GamePlay.Units.PayerMotion;
using UnityEngine;

namespace Korven.GamePlay.Units.Payers
{
    [RequireComponent(typeof(PlayerMotion))]
    public class Player : MonoBehaviour
    {
        private PlayerMotion _playerMotion;

        private void Awake()
        {
            _playerMotion = GetComponent<PlayerMotion>();
        }
        
        ///*
        /// Main class for player
        ///*
    }
}