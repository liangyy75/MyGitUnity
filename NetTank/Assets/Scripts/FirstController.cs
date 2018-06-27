using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, SceneController
{
    bool isGameOver = false;

    public float GetPlayerHp()
    {
        return Singleton<PlayerController>.Instance.GetHp();
    }

    public float GetScore()
    {
        return Singleton<PlayerController>.Instance.GetScore();
    }

    public void LoadResources()
    {
        // 场地
        GameObject scene = Instantiate(Resources.Load("Prefabs/Scene", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
        scene.transform.parent = transform;
    }

    void Awake()
    {
        Director.GetInstance().sceneController = this;
        LoadResources();
    }

    public void ReStartGame()
    {
        Singleton<PlayerController>.Instance.ReSetScore();
        Singleton<PlayerController>.Instance.RpcReset();
    }

    public void GameOver()
    {
        isGameOver = true;
    }

    public bool IsContinue()
    {
        return !isGameOver;
    }
}
