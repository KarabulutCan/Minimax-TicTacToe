using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Sprite xSprite, oSprite;
    public enum Side
    {
        X,
        O
    }

    public static Side whoseTurn = Side.X;

    public enum GameState
    {
        WAITING_FOR_PLAYER_INPUT,
        EXECUTING_AI,
        GAME_END
    }

    public static Board currentBoard { get; private set; }

    public Side playerSide = Side.X;

    private static GameState state = GameState.WAITING_FOR_PLAYER_INPUT;
    private Ai ai;
    private static GameManager instance;
    private float timeWhenGameEnded = 0; //Uses Time.timeSinceLevelLoad.

    private void Awake()
    {
        instance = this;
        currentBoard = new Board();
        ai = FindObjectOfType<Ai>();
        state = GameState.WAITING_FOR_PLAYER_INPUT;
        whoseTurn = Side.X;
    }

    public static Side GetOpponentSide(Side side)
    {
        if(side == Side.O)
        {
            return Side.X;
        }

        return Side.O;
    }

    public static Side GetPlayerSide()
    {
        return instance.playerSide;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameUi.ShowWhoseTurn(whoseTurn);

        if(playerSide != Side.X)
        {
            state = GameState.EXECUTING_AI;
            Ai.StartExecution();
        }
    }

    public static bool IsInputAllowed()
    {
        return state == GameState.WAITING_FOR_PLAYER_INPUT; 
    }

    // Update is called once per frame
    void Update()
    {
        if(state == GameState.GAME_END && Input.anyKeyDown && Time.timeSinceLevelLoad - timeWhenGameEnded > 0.4f)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }

    public static void EndGame()
    {
        state = GameState.GAME_END;
        instance.timeWhenGameEnded = Time.timeSinceLevelLoad;
    }

    public static void MakeMove(int x, int y)
    {
        int buttonId = GetButtonIdByCoordinates(x, y);
        MakeMove(buttonId);
    }

    public static int GetButtonIdByCoordinates(int x, int y)
    {
        return x + (y * 3);
    }

    public static void MakeMove(int buttonId)
    {
        currentBoard.SetSquareSideById(buttonId, whoseTurn);

        if(currentBoard.DoesSideWinMatch(whoseTurn))
        {
            Debug.LogWarning(whoseTurn + " wins");
            EndGame();
            GameUi.ShowWinningSide(whoseTurn);
            return;
        }

        if(currentBoard.IsDraw())
        {
            Debug.LogWarning("Draw");
            EndGame();
            GameUi.ShowDraw();
            return;
        }

        if(whoseTurn == instance.playerSide)
        {
            state = GameState.EXECUTING_AI;
        }
        else
        {
            state = GameState.WAITING_FOR_PLAYER_INPUT;
        }

        whoseTurn = GetOpponentSide(whoseTurn);

        GameUi.ShowWhoseTurn(whoseTurn);

        if(state == GameState.EXECUTING_AI)
        {
            //print("It's turn of  " + whoseTurn);
            Ai.StartExecution();
        }

    }

    public static Sprite GetSideSprite()
    {
        return (whoseTurn == Side.X) ? instance.xSprite : instance.oSprite;
    }
}
