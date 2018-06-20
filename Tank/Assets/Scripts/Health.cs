using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float _hp;

    // 控制血量
    public void SetHp(float hp)
    {
        _hp = hp;
    }

    // 得到血量
    public float GetHp()
    {
        return _hp;
    }
}
