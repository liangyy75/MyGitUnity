using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SceneController
{
    // 加载资源
    void LoadResources();
    // 获得工厂血量
    float GetFactoryHp();
    // 获得玩家血量
    float GetPlayerHp();
    // 获得玩家位置
    Vector3 GetPlayerPos();
    // 获得分数
    float GetScore();
}