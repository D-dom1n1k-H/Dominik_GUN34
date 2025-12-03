using System;
using UnityEngine;

namespace Necro.Interfaces.ConfirmMovementTextController
{
    public class ConfirmMovementTextController : MonoBehaviour
    {
        [SerializeField]
        private GameObject confirmMovementText;

        private void Awake()
        {
            ValidateDependencies();
        }

        public void ShowConfirmMovementText() => confirmMovementText.SetActive(true);
        public void HideConfirmMovementText() => confirmMovementText.SetActive(false);

        private void ValidateDependencies()
        {
            if (confirmMovementText == null)
                throw new NullReferenceException("<b>[ConfirmMovementTextController]</b> confirmMovementText is null!");
        }
    }
}