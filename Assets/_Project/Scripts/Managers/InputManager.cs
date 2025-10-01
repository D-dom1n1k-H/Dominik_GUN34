using System;
using Korven.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Korven.Managers
{
    public class InputManager : MonoBehaviour
    {
        [Inject]
        private Controls _controls; //Injected
        [Inject]
        private SceneController _sceneController; //Injected

        private bool _isPressed = false;

        [SerializeField]
        private Image restartFr;
        [SerializeField]
        private Image restartingBg;

        private void Awake()
        {
            ValidateDependencies();

            restartFr.enabled = false;

            restartingBg.enabled = false;
            restartingBg.fillAmount = 0f;
        }

        private void Update()
        {
            if (_isPressed == true)
            {
                restartFr.fillAmount += Time.deltaTime / 3f;
                if (restartFr.fillAmount == 1f)
                {
                    restartFr.fillAmount = 0f;
                }
            }
        }

        private void OnEnable()
        {
            _controls.Game.Enable();
            _controls.Game.Restart.started += Restart_started;
            _controls.Game.Restart.performed += Restart_performed;
            _controls.Game.Restart.canceled += Restart_canceled;
        }

        private void OnDestroy()
        {
            _controls.Game.Restart.started -= Restart_started;
            _controls.Game.Restart.performed -= Restart_performed;
            _controls.Game.Restart.canceled -= Restart_canceled;
        }


        private void Restart_started(InputAction.CallbackContext context)
        {
            restartFr.enabled = true;
            restartingBg.enabled = true;
            _isPressed = true;
        }

        private void Restart_canceled(InputAction.CallbackContext context)
        {
            restartFr.enabled = false;
            restartFr.fillAmount = 0f;
            restartingBg.enabled = false;
            _isPressed = false;
        }

        private void Restart_performed(InputAction.CallbackContext context)
        {
            _sceneController.OpenGameScene();
        }

        private bool ValidateDependencies()
        {
            if (_controls == null)
                throw new NullReferenceException("[InputManager] Controls could not be injected!");


            if (_sceneController == null)
                throw new NullReferenceException("[InputManager] SceneController could not be injected!");

            if (restartFr == null)
                throw new NullReferenceException("[InputManager] Restart image is missing!");

            if (restartingBg == null)
                throw new NullReferenceException("[InputManager] Restarting image is missing!");

            return true;
        }
    }
}