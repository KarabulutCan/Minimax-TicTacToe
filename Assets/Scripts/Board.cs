using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    private Square[,] squares;

    public Board()
    {
        squares = new Square[3, 3];

        int i, j;
        for(i = 0; i < 3; i++)
        {
            for(j = 0; j < 3; j++)
            {
                squares[i, j].Init();
            }
        }
    }

    public void SetSquareSideById(int squareId, GameManager.Side side)
    {
        int x = squareId % 3;
        int y = squareId / 3;
        SetSquareSide(x, y, side);
    }

    public Square GetSquare(int x, int y)
    {
        return squares[x, y];
    }

    public void SetSquareSide(int x, int y, GameManager.Side side)
    {
        squares[x, y].SetSide(side);
        //PrintValues();
    }

    public int ImportantLinesAmount(GameManager.Side side)
    {
        int importantLineAmount = 0;
        int x, y;

        int emptySquareAmount = 0;
        int squaresWithOurSide = 0;

        for (x = 0; x < 3; x++)
        {
            emptySquareAmount = 0;
            squaresWithOurSide = 0;

            for(y = 0; y < 3; y++)
            {
                if(squares[x, y].isEmpty)
                {
                    emptySquareAmount++;
                }
                else if(squares[x, y].IsSide(side))
                {
                    squaresWithOurSide++;
                }
            }

            if (squaresWithOurSide == 2 && emptySquareAmount == 1)
            {
                importantLineAmount++;
            }
        }

        for (y = 0; y < 3; y++)
        {
            emptySquareAmount = 0;
            squaresWithOurSide = 0;

            for (x = 0; x < 3; x++)
            {
                if (squares[x, y].isEmpty)
                {
                    emptySquareAmount++;
                }
                else if (squares[x, y].IsSide(side))
                {
                    squaresWithOurSide++;
                }
            }

            if (squaresWithOurSide == 2 && emptySquareAmount == 1)
            {
                importantLineAmount++;
            }
        }

        emptySquareAmount = 0;
        squaresWithOurSide = 0;

        for (x = 0; x < 3; x++)
        {
            if (squares[x, x].isEmpty)
            {
                emptySquareAmount++;
            }
            else if (squares[x, x].IsSide(side))
            {
                squaresWithOurSide++;
            }
        }

        if (squaresWithOurSide == 2 && emptySquareAmount == 1)
        {
            importantLineAmount++;
        }

        emptySquareAmount = 0;
        squaresWithOurSide = 0;

        for (x = 0; x < 3; x++)
        {
            if (squares[x, 2 - x].isEmpty)
            {
                emptySquareAmount++;
            }
            else if (squares[x, 2 - x].IsSide(side))
            {
                squaresWithOurSide++;
            }
        }

        if (squaresWithOurSide == 2 && emptySquareAmount == 1)
        {
            importantLineAmount++;
        }

        return importantLineAmount;
    }

    public bool DoesSideWinMatch(GameManager.Side side)
    {
        int x, y;

        for(x = 0; x < 3; x++)
        {
            if(squares[x, 0].IsSide(side) && squares[x, 1].IsSide(side) && squares[x, 2].IsSide(side))
            {
                return true;
            }
        }

        for (y = 0; y < 3; y++)
        {
            if (squares[0, y].IsSide(side) && squares[1, y].IsSide(side) && squares[2, y].IsSide(side))
            {
                return true;
            }
        }

        if(squares[0,0].IsSide(side) && squares[1,1].IsSide(side) && squares[2,2].IsSide(side))
        {
            return true;
        }

        if (squares[2, 0].IsSide(side) && squares[1, 1].IsSide(side) && squares[0, 2].IsSide(side))
        {
            return true;
        }

        return false;
    }

    public bool IsDraw()
    {
        int i, j;
        for (i = 0; i < 3; i++)
        {
            for (j = 0; j < 3; j++)
            {
                if(squares[i, j].isEmpty)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void PrintValues()
    {
        int x, y;
        for (y = 0; y < 3; y++)
        {
            string line = "";
            for (x = 0; x < 3; x++)
            {
                line += squares[x, y].GetChar();
            }

            Debug.Log(line);
        }
    }

    public Board Duplicate()
    {
        Board duplicateBoard = new Board();

        int x, y;
        for (y = 0; y < 3; y++)
        {
            for (x = 0; x < 3; x++)
            {
                if(!squares[x, y].isEmpty)
                {
                    duplicateBoard.SetSquareSide(x, y, squares[x, y].side);
                }
            }
        }

        return duplicateBoard;
    }
}
