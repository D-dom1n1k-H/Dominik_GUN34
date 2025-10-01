namespace Korven.Extra
{
    public enum Team
    {
        None = 0,
        Player1 = 1,
        Player2 = 2,
    }

    [System.Flags]
    public enum NeighbourType
    {
        None = 0,
        Left = 1 << 0,
        Right = 1 << 1,
        Up = 1 << 2,
        Down = 1 << 3,
        UpLeft = 1 << 4,
        UpRight = 1 << 5,
        DownLeft = 1 << 6,
        DownRight = 1 << 7,
    }
}