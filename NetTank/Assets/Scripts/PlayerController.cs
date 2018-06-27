using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    // 血量
    [SyncVar]
    private float health;

    public void Damage(float a)
    {
        if (!isServer)
            return;
        health -= a;
        // 血量减少至0
        if (health < 0)
        {
            pos = transform.position;
            flag = 2;
            health = 500;
            RpcReset();
        }
    }

    public float GetHp()
    {
        return health;
    }

    // 得分
    [SyncVar]
    private int score;

    public void AddScore()
    {
        score++;
        if (score == 3)
        {
            Director.GetInstance().sceneController.GameOver();
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void ReSetScore()
    {
        score = 0;
    }

    // 子弹预制体
    public GameObject bullet;
    // 子弹爆炸粒子系统
    public GameObject bulletPS;
    // 坦克爆炸粒子系统
    public GameObject tankPS;
    // 弹药
    private Dictionary<int, GameObject> usingBullets;   // 正在使用中的
    private Dictionary<int, GameObject> freeBullets;    // 回收的
    // 不是正在使用中的特效集合
    private List<GameObject> bpsContainer;
    private List<GameObject> tpsContainer;

    // 子弹时间间隔
    public float timeLimit = 0.1f;
    public float timePointer = 0;
    // 摄像机
    public GameObject _camera;

    // 爆炸位置与爆炸类型
    [SyncVar]
    public Vector3 pos;
    [SyncVar(hook = "CmdExplosion")]
    public int flag = 0;
    
    public override void OnStartLocalPlayer()
    {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRenderer in meshRenderers)
            meshRenderer.material.color = Color.red;
    }

    private void Awake()
    {
        bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"), new Vector3(50, 0, 50), Quaternion.identity);
        bulletPS = Instantiate(Resources.Load<GameObject>("Prefabs/CompleteShellExplosion"), new Vector3(50, 0, 50), Quaternion.identity);
        tankPS = Instantiate(Resources.Load<GameObject>("Prefabs/CompleteTankExplosion"), new Vector3(50, 0, 50), Quaternion.identity);
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
        // 初始化
        usingBullets = new Dictionary<int, GameObject>();
        freeBullets = new Dictionary<int, GameObject>();
        bpsContainer = new List<GameObject>();
        tpsContainer = new List<GameObject>();
        health = 500;
    }
	
	void Update ()
    {
        if (Director.GetInstance().sceneController.IsContinue() == false)
            return;
        if (!isLocalPlayer)
            return;
        // 摄像机追踪
        _camera.transform.rotation = transform.localRotation;
        _camera.transform.Rotate(90, 0, 0);
        _camera.transform.position = gameObject.transform.position + new Vector3(0, 20, 0);
        // 前后移动
        transform.Translate(0, 0, Input.GetAxis("Vertical") / 2);
        // 改变方向
        transform.Rotate(0, Input.GetAxis("Horizontal"), 0);
        // 发射子弹
        timePointer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && timePointer > timeLimit)    // Input.GetAxis("Fire1")
        {
            timePointer = 0;
            CmdFile();
        }
    }

    [Command]
    void CmdFile()
    {
        GameObject newBullet = GetBullet();
        newBullet.GetComponent<BulletController>().SetBullet(gameObject);
        NetworkServer.Spawn(newBullet);
    }

    // [Command]
    public void CmdExplosion(int newflag)
    {
        flag = newflag;
        if (flag == 0)
            return;
        GameObject bps = GetPs(flag == 1);
        bps.transform.position = pos;
        bps.GetComponent<ParticleSystem>().Play();
        flag = 0;
        // NetworkServer.Spawn(bps);
    }

    public void BeginShellExplosion(Vector3 newpos)
    {
        pos = newpos;
        flag = 1;
    }

    // 获得子弹
    public GameObject GetBullet()
    {
        GameObject newBullet = null;
        if (freeBullets.Count == 0)
        {
            newBullet = Instantiate(bullet);
            newBullet.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        foreach (KeyValuePair<int, GameObject> pair in freeBullets)
        {
            newBullet = pair.Value;
            newBullet.SetActive(true);
            freeBullets.Remove(pair.Key);
            break;
        }
        usingBullets.Add(newBullet.GetInstanceID(), newBullet);
        return newBullet;
    }

    // 获得爆炸特效：子弹的与坦克的
    public GameObject GetPs(bool flag)
    {
        List<GameObject> particles = flag ? bpsContainer : tpsContainer;
        foreach (GameObject _particle in particles)
            if (!_particle.GetComponent<ParticleSystem>().isPlaying)
            {
                // NetworkServer.UnSpawn(_particle.gameObject);
                return _particle;
            }
        GameObject particle = Instantiate(flag ? bulletPS : tankPS);
        particles.Add(particle);
        return particle;
    }

    // 回收子弹
    public void ReCycleBullet(GameObject bullet)
    {
        usingBullets.Remove(bullet.GetInstanceID());
        freeBullets.Add(bullet.GetInstanceID(), bullet);
        bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bullet.SetActive(false);
        NetworkServer.UnSpawn(bullet);
    }


    [ClientRpc]
    public void RpcReset()
    {
        if (!isLocalPlayer)
            return;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.position = Vector3.zero;
    }
}
