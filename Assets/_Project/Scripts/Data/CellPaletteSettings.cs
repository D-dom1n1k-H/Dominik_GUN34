using UnityEngine;

namespace Korven.Data.Settings
{
    [CreateAssetMenu(fileName = "NewCellPalletSettings", menuName = "Assets/_Project/Data/CellPaletteSettings")]
    public class CellPaletteSettings : ScriptableObject
    {
        [field: SerializeField, Space(20f)]
        [field: Tooltip("Клетка под выбранным юнитом")]
        public Material SelectCellMaterial { get; private set; }

        [field: SerializeField]
        [Tooltip("Клетка доступна для передвижения")]
        public Material MoveCellMaterial { get; private set; }

        [field: SerializeField]
        [Tooltip("Клетка доступна для атаки")]
        public Material AttackCellMaterial { get; private set; }

        [field: SerializeField]
        [field: Tooltip("Клетка доступна и для атаки и для передвижения")]
        public Material MoveAndAttackCellMaterial { get; private set; }
    }
}