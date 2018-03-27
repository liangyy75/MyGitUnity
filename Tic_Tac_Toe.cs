using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://blog.csdn.net/shendw818/article/details/79689656
// https://blog.csdn.net/zzj051319/article/details/62040762
// https://blog.csdn.net/alva112358/article/details/79675793

public class Tic_Tac_Toe : MonoBehaviour {
    private int[,] chessboard = new int[3, 3];
    int state = 1;

    private void Start()
    {
        ResetChessBoard();
    }

    private void OnGUI()
    {
        int midWidth = Screen.width / 2;
        int midHeight = Screen.height / 2;
        int buttonEdge = Screen.height / 5;

        GUIStyle redStyle = new GUIStyle();
        redStyle.fontSize = buttonEdge / 2;
        redStyle.fontStyle = FontStyle.Bold;
        redStyle.normal.textColor = Color.red;

        GUIStyle blueStyle = new GUIStyle();
        blueStyle.fontSize = buttonEdge / 2;
        blueStyle.fontStyle = FontStyle.Bold;
        blueStyle.normal.textColor = Color.blue;

        GUIStyle blackStyle = new GUIStyle();
        blackStyle.fontSize = buttonEdge / 2;
        blackStyle.fontStyle = FontStyle.Bold;
        blackStyle.normal.textColor = Color.black;

        GUIStyle anotherBlue = new GUIStyle(blueStyle);
        anotherBlue.fontSize = buttonEdge;

        GUIStyle anotherRed = new GUIStyle(redStyle);
        anotherRed.fontSize = buttonEdge;

        int winner = WhetherWin();
        if (winner == 1)
        {
            if (GUI.Button(new Rect(midWidth - buttonEdge * 1.5f, 0.5f * buttonEdge, 3 * buttonEdge, 4.5f * buttonEdge), "Blue Win", blackStyle))
                ResetChessBoard();
        }
        else if (winner == 2)
        {
            if (GUI.Button(new Rect(midWidth - buttonEdge * 1.5f, 0.5f * buttonEdge, 3 * buttonEdge, 4.5f * buttonEdge), "Red Win", redStyle))
                ResetChessBoard();
        }
        else if (winner == 0)
        {
            GUI.Label(new Rect(midWidth - buttonEdge * 1.5f, 0.5f * buttonEdge, 3 * buttonEdge, 4.5f * buttonEdge), "Welcome!", blackStyle);
        }
        else if (winner == 3)
        {
            if (GUI.Button(new Rect(midWidth - buttonEdge * 1.5f, 0.5f * buttonEdge, 3 * buttonEdge, 4.5f * buttonEdge), "Tied", blackStyle))
                ResetChessBoard();
        }

        if (GUI.Button(new Rect(midWidth - 0.5f * buttonEdge, midHeight + 1.75f * buttonEdge, buttonEdge, 0.5f * buttonEdge), "Reset"))
            ResetChessBoard();

        for (int i = 0; i < 3; i++)
            for(int j = 0; j < 3; j++)
            {
                float x = midWidth - 1.5f * buttonEdge + j * buttonEdge;
                float y = midHeight - 1.5f * buttonEdge + i * buttonEdge;
                if (chessboard[i, j] == 1)
                    GUI.Button(new Rect(x, y, buttonEdge, buttonEdge), "O", anotherBlue);
                else if (chessboard[i, j] == 2)
                    GUI.Button(new Rect(x, y, buttonEdge, buttonEdge), "X", anotherRed);
                if(GUI.Button(new Rect(x, y, buttonEdge, buttonEdge), ""))
                {
                    if (state == 0)
                        chessboard[i, j] = 1;
                    else if (state == 1)
                        chessboard[i, j] = 2;
                    state = 1 - state;
                }
            }
    }

    // 重置棋盘
    private void ResetChessBoard()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                chessboard[i, j] = 0;
    }

    // 判断谁赢了
    private int WhetherWin()
    {
        int count = 0;
        for(int i = 0; i < 3; i++)
        {
            if (chessboard[i, 0] != 0 && chessboard[i, 0] == chessboard[i, 1] && chessboard[i, 0] == chessboard[i, 2])
                return chessboard[i, 0];
            if (chessboard[0, i] != 0 && chessboard[0, i] == chessboard[1, i] && chessboard[0, i] == chessboard[2, i])
                return chessboard[0, i];
            for (int j = 0; j < 3; j++)
                if (chessboard[i, j] != 0)
                    count++;
        }
        if (chessboard[1, 1] != 0 && chessboard[0, 0] == chessboard[1, 1] && chessboard[1, 1] == chessboard[2, 2])
            return chessboard[0, 0];
        if (chessboard[1, 1] != 0 && chessboard[2, 0] == chessboard[1, 1] && chessboard[0, 2] == chessboard[1, 1])
            return chessboard[0, 2];
        if (count == 9)
            return 3;
        return 0;
    }
}
