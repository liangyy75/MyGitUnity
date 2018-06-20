using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // 子弹的伤害范围
    public float explosionRadius = 3f;
    // 发射出子弹的坦克类型
    private string from;

    public void SetBullet(GameObject _gameObject, int power)
    {
        from = _gameObject.tag;
        transform.position = new Vector3(_gameObject.transform.position.x, 1.5f, _gameObject.transform.position.z)
            + _gameObject.transform.forward * 2;  // 设置子弹位置
        transform.forward = _gameObject.transform.forward;   // 设置子弹方向
        GetComponent<Rigidbody>().AddForce(transform.forward * power, ForceMode.Impulse);  // 发射子弹
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explosion();
    }

    private void Explosion()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);//获取爆炸范围内的所有碰撞体
        for (int i = 0; i < colliders.Length; i++)
            if (colliders[i].tag == "Player" && from == "Enemy" || colliders[i].tag == "Factory" && from == "Player"
                || colliders[i].tag == "Enemy" && from == "Player")
            {
                float distance = Vector3.Distance(colliders[i].transform.position, transform.position);
                float currentHp = colliders[i].gameObject.GetComponent<Health>().GetHp();
                colliders[i].gameObject.GetComponent<Health>().SetHp(currentHp - 100 / distance);
            }
        TankFactory tankFactory = Singleton<TankFactory>.Instance;
        ParticleSystem bps = tankFactory.GetPs(true);
        bps.transform.position = transform.position;
        bps.Play();
        if (gameObject.activeSelf)
            tankFactory.ReCycleBullet(gameObject);
    }

    void Update()
    {
        // 超出边界的也要回收(越界、碰地)
        Vector3 pos = transform.position;
        if (pos.x > 50 || pos.z > 50 || pos.x < -50 || pos.z < -50 || pos.y <= 0)
            Explosion();
    }
}
