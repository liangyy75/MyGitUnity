using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, SceneController, IUserAction {
    // 玩家预制体
    private GameObject player;
    // 所有的怪物
    private List<GameObject> monsters;
    // 工厂
    private MonsterFactory monsterFactory;
    // 游戏是否结束
    private bool isGameOver = false;
    // 得分
    private int score = 0;

    public void LoadResources()
    {
        // 场地
        GameObject scene = Instantiate(Resources.Load("Prefabs/Scene", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
        scene.gameObject.transform.localScale = new Vector3(0.5f, 0.2f, 0.5f);
        // 玩家
        player = Instantiate(Resources.Load("Prefabs/player", typeof(GameObject)), new Vector3(0, 0, -1f), Quaternion.identity, null) as GameObject;
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.transform.parent = player.transform;
        camera.transform.position = player.transform.position + new Vector3(0, 0.2f, -0.4f);
        player.AddComponent<PlayerController>();
        // 工厂
        gameObject.AddComponent<MonsterFactory>();
        monsterFactory = gameObject.GetComponent<MonsterFactory>();
        // 怪物
        monsters = monsterFactory.GetMonsters();
        // 游戏事件的管理者
        gameObject.AddComponent<GameEventManager>();
    }

    public void GameOver()
    {
        player.GetComponent<Rigidbody>().isKinematic = true;
        isGameOver = true;
    }

    public void AddScore()
    {
        if(GameEventManager.isContinue)
            score++;
        if(score == 30)
            Singleton<GameEventManager>.Instance.PatrolGameOver();
    }

    public void ReStartGame()
    {
        player.GetComponent<PlayerController>().Reset();
        monsterFactory.Reset();
        score = 0;
    }

    public int GetScore()
    {
        return score;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    private void Awake()
    {
        Director director = Director.GetInstance();
        director.sceneController = this;
        LoadResources();
        // 注册游戏结束的事件
        GameEventManager.AttackPlayerEvent += GameOver;
        // 注册加分的事件
        GameEventManager.ScoreEvent += AddScore;
        // 开始游戏
        GameEventManager.StartGameEvent += ReStartGame;
    }

    private void OnDisable()
    {
        GameEventManager.AttackPlayerEvent -= GameOver;
        GameEventManager.ScoreEvent -= AddScore;
    }
}
