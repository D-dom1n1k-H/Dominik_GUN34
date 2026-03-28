using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Korven.GamePlay.Player.RectSelection
{
    public sealed class RectSelectionInput : MonoBehaviour
    {
        private Vector2 _startPosition;
        private Vector2 _endPosition;
        private bool _isSelecting;

        public event Action OnStarted;
        public event Action OnFinished;

        public Vector2 StartPoint => this._startPosition;

        public Vector2 EndPoint => this._endPosition;

        public bool IsSelecting => this._isSelecting;

        private void Update()
        {
            var mouse = Mouse.current;
            if (mouse == null) return;

            if (mouse.leftButton.wasPressedThisFrame)
            {
                this._isSelecting = true;
                this._startPosition = mouse.position.ReadValue(); // mouse position
                this._endPosition = this._startPosition;
                this.OnStarted?.Invoke();
            }
            else if (mouse.leftButton.isPressed)
            {
                this._endPosition = mouse.position.ReadValue();
            }
            else if (mouse.leftButton.wasReleasedThisFrame)
            {
                this._isSelecting = false;
                this._endPosition = mouse.position.ReadValue();
                this.OnFinished?.Invoke();
            }
        }
    }
}