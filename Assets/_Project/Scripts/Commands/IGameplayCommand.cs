using Necro.World.Board.Cell;

namespace Necro.Commands.IGameplayCommand
{
    public interface IGameplayCommand
    {
        public void Interact(Cell cell);
    }
}