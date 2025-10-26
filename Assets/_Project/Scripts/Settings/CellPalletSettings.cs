using UnityEngine;

namespace Necro.Config.CellPalleteSettings
{
    [CreateAssetMenu(
        fileName = "NewCellPalletSettings",
        menuName = "Assets/_Project/Scripts/Settings/CellPalletSettings",
        order = 52)]
    public class CellPalletSettings : ScriptableObject
    {
        [field: SerializeField, Space(20f), Tooltip("Клетка под выбранным юнитом")]
        public Material SelectCellMaterial { get; private set; }

        [field: SerializeField, Tooltip("Клетка доступна для передвижения")]
        public Material MoveCellMaterial { get; private set; }

        [field: SerializeField, Tooltip("Клетка доступна для атаки")]
        public Material AttackCellMaterial { get; private set; }

        private void Awake()
        {
            ValidateDependencies();
        }

        private void ValidateDependencies()
        {
            if (SelectCellMaterial == null)
                Debug.LogError("[CellPalletSettings] SelectCellMaterial is null!");

            if (MoveCellMaterial == null)
                Debug.LogError("[CellPalletSettings] MoveCellMaterial is null!");

            if (AttackCellMaterial == null)
                Debug.LogError("[SelectCellMaterial] AttackCellMaterial is null!");
        }
    }
}