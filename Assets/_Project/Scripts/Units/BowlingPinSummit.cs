using System;
using UnityEngine;

namespace Korven.GamePlay.Units.BowlingPin.BowlingPinSummit
{
    public sealed class BowlingPinSummit : MonoBehaviour
    {
        public event Action OnFlourCollisionEnteredEvent;

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
            {
                OnFlourCollisionEnteredEvent?.Invoke();
            }
        }
    }
}