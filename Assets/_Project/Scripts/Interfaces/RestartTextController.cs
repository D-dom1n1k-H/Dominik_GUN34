using UnityEngine;
using UnityEngine.UI;
using System;
using Necro.GamePlay.Controllers;
using UnityEngine.InputSystem;
using Zenject;

namespace Necro.Interfaces.RestartTextController
{
    public class RestartTextController : MonoBehaviour
    {
        private SceneController _sceneController; //injected

        [SerializeField]
        private Image restartFr;
        [SerializeField]
        private Image restartingBg;

        private void Awake()
        {
            ValidateDependencies();
        }

        private void Start()
        {
            restartFr.enabled = false;
            restartingBg.enabled = false;
            restartingBg.fillAmount = 0f;
        }

        private void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            RestartFunction(keyboard);
        }

        private void RestartFunction(Keyboard keyboard)
        {
            if (keyboard == null)
                return;

            if (keyboard.tabKey.isPressed)
            {
                restartFr.enabled = true;
                restartingBg.enabled = true;

                restartingBg.fillAmount += Time.deltaTime / 2f;
                if (restartingBg.fillAmount >= 1f)
                {
                    restartFr.enabled = false;
                    restartingBg.enabled = false;
                    restartingBg.fillAmount = 0f;

                    _sceneController.OpenGameScene();
                }
            }
            else
            {
                restartFr.enabled = false;
                restartingBg.fillAmount = 0f;
                restartingBg.enabled = false;
            }
        }

        [Inject]
        private void Construct(SceneController sceneController)
        {
            _sceneController = sceneController;
        }

        private void ValidateDependencies()
        {
            if (restartFr == null)
                throw new ArgumentNullException(nameof(restartFr),
                    "<b>[RestartTextController]</b> restartFr is not assigned!");


            if (restartingBg == null)
                throw new ArgumentNullException(nameof(restartFr),
                    "<b>[RestartTextController]</b> restartingBg is not assigned!");
        }
    }
}