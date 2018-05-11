using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour {
    // 游戏结束
    public delegate void AttackPlayer();
    public static event AttackPlayer AttackPlayerEvent;
    // 计分
    public delegate void Score();
    public static event Score ScoreEvent;
    // 开始游戏
    public delegate void StartGame();
    public static event StartGame StartGameEvent;

    // 游戏是否在继续
    public static bool isContinue = false;

    public void PatrolGameOver()
    {
        isContinue = false;
        if (AttackPlayerEvent != null)
            AttackPlayerEvent();
    }

    public void ChangeScore()
    {
        if (isContinue && ScoreEvent != null)
            ScoreEvent();
    }

    public void GameBegin()
    {
        if (StartGameEvent != null)
            StartGameEvent();
        isContinue = true;
    }
}
