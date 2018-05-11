using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction {
    // 得到分数
    int GetScore();
    // 游戏是否结束
    bool IsGameOver();
}
