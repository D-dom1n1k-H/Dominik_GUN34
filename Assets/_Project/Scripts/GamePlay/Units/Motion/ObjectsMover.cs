using System.Collections.Generic;
using Korven.GamePlay.Units.Motion.Managers;
using UnityEngine;

namespace Korven.GamePlay.Units.Motion
{
    public sealed class ObjectsMover : MonoBehaviour
    {
        [SerializeField]
        private MovingGroupManager movingGroupManager;

        public void MoveObjects(IEnumerable<GameObject> objects, Vector3 destination)
        {
        }
    }
}