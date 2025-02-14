using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai : MonoBehaviour
{
    public int maxDepth = 3;
    public static Ai instance;

    List<AiMove> rootMoves;
    static Board currentBoard;

    const float maxValue = 10.0f;
    const float minValue = -maxValue;
    const float decay = 0.99f;
    const float waitTime = 0.1f;

    GameManager.Side aiSide = GameManager.Side.O; //This is set at Start, getting player side from GameManager
    GameManager.Side enemySide; //This is also set when AI starts

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemySide = GameManager.GetPlayerSide();
        aiSide = GameManager.GetOpponentSide(enemySide);

        print("aiSide: " + aiSide.ToString());
    }

    public static void StartExecution()
    {
        instance.StartCoroutine("Execute");
    }

    private float GetMaxOfList(List<float> list)
    {
        int i;
        float maxValue = float.MinValue;

        for(i = 0; i < list.Count; i++)
        {
            if(list[i] > maxValue)
            {
                maxValue = list[i];
            }
        }

        return maxValue;
    }

    private float GetMinOfList(List<float> list)
    {
        int i;
        float minValue = float.MaxValue;

        for (i = 0; i < list.Count; i++)
        {
            if (list[i] < minValue)
            {
                minValue = list[i];
            }
        }

        return minValue;
    }

    private float Minimax(int depth, Board board, GameManager.Side sideWhoseScoreIsCalculated, float alpha, float beta)
    {
        bool isMaximizing = sideWhoseScoreIsCalculated == aiSide;
        //("Minimaxing board:");
        //board.PrintValues();

        if (board.DoesSideWinMatch(aiSide))
        {
            return maxValue * Mathf.Pow(decay, depth);
        }

        if (board.DoesSideWinMatch(enemySide))
        {
            return minValue * Mathf.Pow(decay, depth);
        }

        if(depth >= maxDepth)
        {
            return CalculateBoardScore(board, sideWhoseScoreIsCalculated);
        }

        List<AiMove> possibleMoves = GetPossibleMoves(board, sideWhoseScoreIsCalculated);

        //DRAW
        if(possibleMoves.Count == 0)
        {
            return 0;
        }

        List<float> scores = new List<float>();
        int i;

        GameManager.Side oppositeSide = GameManager.GetOpponentSide(sideWhoseScoreIsCalculated);

        float best = (isMaximizing) ? float.MinValue : float.MaxValue;

        for (i = 0; i < possibleMoves.Count; i++)
        {
            float score = Minimax(depth + 1, possibleMoves[i].board, oppositeSide, alpha, beta);
            possibleMoves[i].SetScore(score);
            scores.Add(possibleMoves[i].score);

            if (depth == 0)
            {
                rootMoves.Add(possibleMoves[i]);
            }

            if (isMaximizing)
            {
                best = Mathf.Max(score, best);
                alpha = Mathf.Max(alpha, best);

                if (beta <= alpha)
                {
                    break;
                }
            }
            else
            {
                best = Mathf.Min(best, score);
                beta = Mathf.Min(beta, best);

                // Alpha Beta Pruning 
                if (beta <= alpha)
                {
                    break;
                }
            }
        }

        return (isMaximizing) ? GetMaxOfList(scores) : GetMinOfList(scores);

    }

    public IEnumerator Execute()
    {
        print("Executing AI");
        currentBoard = GameManager.currentBoard;
        yield return new WaitForSeconds(waitTime);

        float timeWhenAiStarted = Time.realtimeSinceStartup;

        //List<AiMove> possibleMoves = GetPossibleMoves(currentBoard);
        rootMoves = new List<AiMove>();
        //AiMove rootMove = new AiMove()
        float alpha = float.MinValue;
        float beta = float.MaxValue;

        Minimax(0, currentBoard, aiSide, alpha, beta);

        int i;
        int bestMoveId = -1;
        float bestMoveScore = float.MinValue;

        for(i = 0; i < rootMoves.Count; i++)
        {
            if (rootMoves[i].score > bestMoveScore)
            {
                bestMoveId = i;
                bestMoveScore = rootMoves[i].score;
            }

            //rootMoves[i].Print();
        }

        AiMove bestMove = rootMoves[bestMoveId];
        ChooseSquare(bestMove.x, bestMove.y);

        float calculationTime = Time.realtimeSinceStartup - timeWhenAiStarted;
        print("Calculation time: " + calculationTime);

        yield break;
    }


    public float CalculateBoardScore(Board board, GameManager.Side sideWhoseScoreIsCalculated)
    {
        int importantLineAmount = board.ImportantLinesAmount(sideWhoseScoreIsCalculated);

        if(importantLineAmount > 0)
        {
            //Debug.LogWarning("Important line amount > 0");
            //board.PrintValues();
        }
        
        float score = importantLineAmount * 0.2f;

        return (sideWhoseScoreIsCalculated == aiSide) ? score : -score;
    }


    static void ChooseSquare(int x, int y)
    {
        int buttonId = GameManager.GetButtonIdByCoordinates(x, y);
        
        GameUi.MarkButtonAsPressed(buttonId);
        GameManager.MakeMove(buttonId);
    }

    public static List<AiMove> GetPossibleMoves(Board baseBoard, GameManager.Side moveSide)
    {
        List<AiMove> possibleMoves = new List<AiMove>();

        int x, y;
        for (y = 0; y < 3; y++)
        {
            for (x = 0; x < 3; x++)
            {
                Square square = baseBoard.GetSquare(x, y);
                if (square.isEmpty)
                {
                    Board possibleBoard = baseBoard.Duplicate();
                    possibleBoard.SetSquareSide(x, y, moveSide);
                    AiMove possibleMove = new AiMove(possibleBoard, x, y, moveSide);
                    //possibleMove.SetScore(instance.CalculateMoveScore(possibleMove));
                    possibleMoves.Add(possibleMove);
                    //possibleBoards.Add(possibleBoard);
                }
            }
        }

        return possibleMoves;
    }
}
