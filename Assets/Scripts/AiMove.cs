using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMove
{
    public Board board { get; private set; }
    public int x { get; private set; }
    public int y { get; private set; }
    public GameManager.Side side { get; private set; }

    public float score { get; private set; }

    public AiMove(Board board, int x, int y, GameManager.Side side)
    {
        this.board = board;
        this.x = x;
        this.y = y;
        this.side = side;
    }

    public void Print()
    {
        Debug.Log("X: " + x + " Y: " + y);
        board.PrintValues();
    }

    public void SetScore(float score)
    {
        this.score = score;
    }
}
