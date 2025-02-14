public struct Square
{
    public GameManager.Side side { get; private set; }
    public bool isEmpty { get; private set; }

    public void Init()
    {
        isEmpty = true;
    }

    public void SetSide(GameManager.Side newSide)
    {
        side = newSide;
        isEmpty = false;
    }

    public char GetChar()
    {
        if(isEmpty)
        {
            return '.';
        }

        if(side == GameManager.Side.X)
        {
            return 'X';
        }

        return 'O';
    }

    public bool IsSide(GameManager.Side sideToCheck)
    {
        if(isEmpty)
        {
            return false;
        }

        return side == sideToCheck;
    }
}