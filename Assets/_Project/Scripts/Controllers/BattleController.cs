using System;
using Necro.AutoGen.Controls;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Necro.GamePlay.Controllers
{
    public class BattleController : MonoBehaviour
    {
        private Controls _controls; //injected

        public event Action OnMovePreformedEvent;

        private void Awake()
        {
            ValidateDependencies();
        }

        private void OnEnable()
        {
            _controls.Player.Enable();

            _controls.Player.Cancel.performed += OnPlayerCancel_performed;
            _controls.Player.Confirm.performed += OnPlayerConfirm_performed;
            _controls.Player.Select.performed += OnPlayerSelect_performed;
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
            _controls.Player.Cancel.performed -= OnPlayerCancel_performed;
            _controls.Player.Confirm.performed -= OnPlayerConfirm_performed;
            _controls.Player.Select.performed -= OnPlayerSelect_performed;
        }

        private void OnPlayerSelect_performed(InputAction.CallbackContext obj)
        {
        }

        private void OnPlayerConfirm_performed(InputAction.CallbackContext obj)
        {
            OnMovePreformedEvent?.Invoke();
            Debug.Log("[BattleController] OnMovePreformedEvent was invoked");
        }

        private void OnPlayerCancel_performed(InputAction.CallbackContext obj)
        {
        }
        
        
        [Inject]
        private void Construct(Controls controls)
        {
            _controls = controls;
        }
        private void ValidateDependencies()
        {
            if (_controls == null)
                throw new NullReferenceException("[BattleController] _controls could not be injected!");
        }
    }
}