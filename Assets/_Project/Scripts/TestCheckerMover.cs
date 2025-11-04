using Necro.World.Board.Cell;
using UnityEngine;

public class TestCheckerMover : MonoBehaviour
{
    [SerializeField]
    private Cell currentCell;
    
    [SerializeField]
    private Cell targetCell;

    private void Update()
    {
        if (currentCell.CurrentUnit != null)
        {
            currentCell.CurrentUnit.Move(targetCell);
        }
    }
}
