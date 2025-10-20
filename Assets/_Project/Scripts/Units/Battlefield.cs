using Necro.World.Board.Cell;
using UnityEngine;

namespace Necro.World.Battlefield
    {
        public class Battlefield : MonoBehaviour
        {
            private Cell[] _cells;

            private void Awake()
            {
                
            }
            private void Start()
            {
                _cells = FindObjectsOfType<Cell>();
            }
        }
    }