using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    // 游戏结束
    public delegate void AttackPlayer();
    public static event AttackPlayer AttackPlayerEvent;
    // 计分
    public delegate void Score(int change);
    public static event Score ScoreEvent;
    // 开始游戏
    public delegate void StartGame();
    public static event StartGame StartGameEvent;

    // 游戏是否在继续
    public static bool isContinue = true;

    public void GameOver()
    {
        isContinue = false;
        if (AttackPlayerEvent != null)
            AttackPlayerEvent();
    }

    public void ChangeScore(int change)
    {
        if (isContinue && ScoreEvent != null)
            ScoreEvent(change);
    }

    public void GameBegin()
    {
        if (StartGameEvent != null)
            StartGameEvent();
        isContinue = true;
    }
}
