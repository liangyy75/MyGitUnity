using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankFactory : MonoBehaviour
{
    // 敌方坦克预制体
    public GameObject enemy;
    // 玩家坦克预制体
    public GameObject player;
    // 子弹预制体
    public GameObject bullet;
    // 子弹爆炸粒子系统
    public ParticleSystem bulletPS;
    // 坦克爆炸粒子系统
    public ParticleSystem tankPS;

    // 敌方坦克初始位置
    private List<Vector3> poss;
    // 敌方坦克
    private Dictionary<int, GameObject> usingEnemys;    // 正在使用中的
    private Dictionary<int, GameObject> freeEnemys;     // 回收的
    // 弹药
    private Dictionary<int, GameObject> usingBullets;   // 正在使用中的
    private Dictionary<int, GameObject> freeBullets;    // 回收的
    // 不是正在使用中的特效集合
    private List<ParticleSystem> bpsContainer;
    private List<ParticleSystem> tpsContainer;

    // 定时产生坦克的时间间隔
    private float timeLimit = 3;
    // 现在的时间指针
    private float timePointer = 0;

    void Awake()
    {
        // 预制体
        enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"), new Vector3(0, -10, 0), Quaternion.identity);
        bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"), new Vector3(0, 1.5f, 0), Quaternion.identity);
        bullet.AddComponent<BulletController>();
        bullet.gameObject.tag = "Bullet";
        bulletPS = Instantiate(Resources.Load<ParticleSystem>("Prefabs/CompleteShellExplosion"), new Vector3(0, 100, 0), Quaternion.identity);
        tankPS = Instantiate(Resources.Load<ParticleSystem>("Prefabs/CompleteTankExplosion"), new Vector3(0, 100, 0), Quaternion.identity);
        // 初始化
        poss = new List<Vector3>() { new Vector3(0, 0, 12), new Vector3(0, 0, 26), new Vector3(10, 0, 12), new Vector3(10, 0, 26),
            new Vector3(-10, 0, 12), new Vector3(-10, 0, 26), new Vector3(10, 0, -10), new Vector3(0, 0, -10), new Vector3(-10, 0, -10),
            new Vector3(10, 0, -30), new Vector3(0, 0, -30), new Vector3(-10, 0, -30)};
        usingEnemys = new Dictionary<int, GameObject>();
        freeEnemys = new Dictionary<int, GameObject>();
        usingBullets = new Dictionary<int, GameObject>();
        freeBullets = new Dictionary<int, GameObject>();
        bpsContainer = new List<ParticleSystem>();
        tpsContainer = new List<ParticleSystem>();
    }

    // 获得玩家坦克
    public GameObject GetPlayer()
    {
        player = Instantiate(Resources.Load<GameObject>("Prefabs/Tank"), Vector3.zero, Quaternion.identity);
        player.name = "player";
        player.tag = "Player";
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.transform.parent = player.transform;
        camera.transform.position = player.transform.position + new Vector3(0, 20, -5);
        camera.transform.Rotate(75, 0, 0);
        player.AddComponent<PlayerController>();
        player.AddComponent<Health>();
        player.GetComponent<Health>().SetHp(1000);
        return player;
    }

    // 获得敌方坦克 5 0 -18
    public GameObject GetEnemy(bool first = false, int index = 0)
    {
        GameObject newEnemy = null;
        if (freeEnemys.Count == 0)
        {
            newEnemy = Instantiate(enemy, first ? poss[index] : new Vector3(5, 0, -18), Quaternion.identity);
            newEnemy.AddComponent<EnemyController>();
            newEnemy.AddComponent<Health>();
            newEnemy.GetComponent<Health>().SetHp(500);
            newEnemy.gameObject.tag = "Enemy";
        }
        foreach(KeyValuePair<int, GameObject> pair in freeEnemys)
        {
            newEnemy = pair.Value;
            freeEnemys.Remove(pair.Key);
            newEnemy.SetActive(true);
            newEnemy.GetComponent<EnemyController>().Reset();
            newEnemy.transform.position = first ? poss[index] : new Vector3(5, 0, -18);
            break;
        }
        usingEnemys.Add(newEnemy.GetInstanceID(), newEnemy);
        return newEnemy;
    }

    // 获得子弹
    public GameObject GetBullet()
    {
        GameObject newBullet = null;
        if (freeBullets.Count == 0)
            newBullet = Instantiate(bullet, new Vector3(0, 1.5f, 0), Quaternion.identity);
        foreach(KeyValuePair<int, GameObject> pair in freeBullets)
        {
            newBullet = pair.Value;
            newBullet.SetActive(true);
            freeBullets.Remove(pair.Key);
            break;
        }
        newBullet.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        usingBullets.Add(newBullet.GetInstanceID(), newBullet);
        return newBullet;
    }

    // 获得爆炸特效：子弹的与坦克的
    public ParticleSystem GetPs(bool flag)
    {
        List<ParticleSystem> particles = flag ? bpsContainer : tpsContainer;
        foreach (ParticleSystem _particle in particles)
            if (!_particle.isPlaying)
                return _particle;
        ParticleSystem particle = Instantiate(flag ? bulletPS : tankPS);
        particles.Add(particle);
        return particle;
    }

    // 游戏刚开始时产生的地方坦克
    public void GetOrigialEnemys()
    {
        for (int i = 0; i < poss.Count; i++)
            GetEnemy(true, i);
    }

    // 定时产生敌方坦克
    private void Update()
    {
        if (!GameEventManager.isContinue)
            return;
        if(gameObject.GetComponent<Health>().GetHp() < 0)
        {
            ParticleSystem tps = Singleton<TankFactory>.Instance.GetPs(false);
            tps.transform.position = transform.position;
            tps.Play();
            Singleton<GameEventManager>.Instance.ChangeScore(10);
            Singleton<GameEventManager>.Instance.GameOver();
            gameObject.SetActive(false);
        }
        timePointer += Time.deltaTime;
        if(timePointer > timeLimit)
        {
            GetEnemy();
            timePointer = 0;
        }
    }

    // 回收坦克
    public void ReCycleTank(GameObject enemy)
    {
        usingEnemys.Remove(enemy.GetInstanceID());
        freeEnemys.Add(enemy.GetInstanceID(), enemy);
        enemy.transform.position = new Vector3(5, 0, -18);
        enemy.transform.rotation = Quaternion.identity;
        enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
        enemy.GetComponent<Health>().SetHp(500);
        enemy.SetActive(false);
    }

    // 回收子弹
    public void ReCycleBullet(GameObject bullet)
    {
        usingBullets.Remove(bullet.GetInstanceID());
        freeBullets.Add(bullet.GetInstanceID(), bullet);
        bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bullet.SetActive(false);
    }

    // 重新开始
    public void Reset()
    {
        // 玩家
        player.SetActive(true);
        player.transform.rotation = Quaternion.identity;
        player.transform.position = Vector3.zero;
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.transform.parent = player.transform;
        camera.transform.position = player.transform.position + new Vector3(0, 20, -5);
        camera.transform.rotation = Quaternion.identity;
        camera.transform.Rotate(75, 0, 0);
        player.GetComponent<Health>().SetHp(1000);
        // 敌军
        foreach (KeyValuePair<int, GameObject> pair in usingEnemys)
        {
            freeEnemys.Add(pair.Key, pair.Value);
            pair.Value.GetComponent<Rigidbody>().velocity = Vector3.zero;
            pair.Value.GetComponent<Health>().SetHp(500);
            pair.Value.transform.position = new Vector3(5, 0, -18);
            pair.Value.transform.rotation = Quaternion.identity;
            pair.Value.SetActive(false);
        }
        usingEnemys.Clear();
        // 子弹
        foreach(KeyValuePair<int, GameObject> pair in usingBullets)
        {
            freeBullets.Add(pair.Key, pair.Value);
            pair.Value.GetComponent<Rigidbody>().velocity = Vector3.zero;
            pair.Value.SetActive(false);
        }
        usingBullets.Clear();
        // 游戏初始敌军
        GetOrigialEnemys();
        timePointer = 0;
    }
}
