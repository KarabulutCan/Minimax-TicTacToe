using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUi : MonoBehaviour
{
    public GameObject gameEndScreen;
    public Text gameEndText;
    public Text whoseTurnText;

    public XOX_Button[] xoxButtons;

    static GameUi instance;

    private void Awake()
    {
        instance = this;
        gameEndScreen.SetActive(false);
    }

    public static void OpenGameEndScren()
    {
        instance.gameEndScreen.SetActive(true);
        //instance.whoseTurnImage.enabled = false;
        instance.whoseTurnText.enabled = false;
    }

    public static void ShowWinningSide(GameManager.Side winningSide)
    {
        OpenGameEndScren();
        instance.gameEndText.text = (winningSide == GameManager.Side.O) ? "O" : "X";
        instance.gameEndText.text += " WINS";
    }

    public static void ShowDraw()
    {
        OpenGameEndScren();
        instance.gameEndText.text = "DRAW";
    }

    public static void ShowWhoseTurn(GameManager.Side side)
    {
        instance.whoseTurnText.text = side == GameManager.Side.X ? "X" : "O";
        instance.whoseTurnText.text += "'s turn";
    }

    public static void MarkButtonAsPressed(int buttonId)
    {
        instance.xoxButtons[buttonId].MarkButtonAsPressed();
    }
}
