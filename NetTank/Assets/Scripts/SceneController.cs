using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SceneController
{
    // 加载资源
    void LoadResources();
    // 获得玩家血量
    float GetPlayerHp();
    // 获得分数
    float GetScore();
    // 游戏结束
    void GameOver();
    // 是否继续
    bool IsContinue();
    // 继续
    void ReStartGame();
}
