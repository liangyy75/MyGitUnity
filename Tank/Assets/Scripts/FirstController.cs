using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, SceneController
{
    // 工厂对象
    public GameObject factory;
    // 工厂
    public TankFactory tankFactory;
    // 玩家对象
    private GameObject player;

    // 是否游戏结束
    bool isGameOver = false;
    // 得分
    int score = 0;

    // 获得工厂血量
    public float GetFactoryHp()
    {
        return factory.GetComponent<Health>().GetHp();
    }

    // 获得玩家血量
    public float GetPlayerHp()
    {
        return player.GetComponent<Health>().GetHp();
    }

    // 获得玩家位置
    public Vector3 GetPlayerPos()
    {
        return player.transform.position;
    }

    public void GameOver()
    {
        isGameOver = true;
    }

    public void AddScore(int _score)
    {
        if (GameEventManager.isContinue)
            score += _score;
        if (score == 100)
            Singleton<GameEventManager>.Instance.GameOver();
    }

    public float GetScore()
    {
        return score;
    }

    public void ReStartGame()
    {
        factory.SetActive(true);
        factory.GetComponent<Health>().SetHp(3000);
        tankFactory.Reset();
        score = 0;
    }

    public void LoadResources()
    {
        // 场地
        GameObject scene = Instantiate(Resources.Load("Prefabs/Scene", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
        // 坦克工厂
        factory = Instantiate(Resources.Load<GameObject>("Prefabs/Factory"), new Vector3(0, 0, -18), Quaternion.identity);
        factory.AddComponent<TankFactory>();
        factory.AddComponent<Health>();
        factory.GetComponent<Health>().SetHp(3000);
        factory.gameObject.tag = "Factory";
        tankFactory = factory.GetComponent<TankFactory>();
        // 玩家
        player = tankFactory.GetPlayer();
        // 产生初始敌军
        tankFactory.GetOrigialEnemys();
        // 游戏事件管理者
        gameObject.AddComponent<GameEventManager>();
    }

    void Awake()
    {
        Director.GetInstance().sceneController = this;
        LoadResources();
        // 注册游戏结束的事件
        GameEventManager.AttackPlayerEvent += GameOver;
        // 注册加分的事件
        GameEventManager.ScoreEvent += AddScore;
        // 开始游戏
        GameEventManager.StartGameEvent += ReStartGame;
    }
	
	void OnDisable()
    {
        GameEventManager.AttackPlayerEvent -= GameOver;
        GameEventManager.ScoreEvent -= AddScore;
        GameEventManager.StartGameEvent -= ReStartGame;
    }
}
