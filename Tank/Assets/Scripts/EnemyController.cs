using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // 前进的速度、后退的速度、转变方向的速度
    public float forwardSpeed;
    public float backwardSpeed;
    public float rotateSpeed;
    // 刚体
    Rigidbody _rigidbody;
    // 子弹时间间隔
    public float timeLimit = 0.1f;
    public float timePointer = 0;
    // 子弹目标
    private Vector3 target;

    void Start ()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        forwardSpeed = 15;
        backwardSpeed = 10;
        rotateSpeed = 1.5f;
        StartCoroutine(Shoot());
    }
	
	void Update ()
    {
        if (GameEventManager.isContinue)
        {
            target = Director.GetInstance().sceneController.GetPlayerPos();
            if (GetComponent<Health>().GetHp() <= 0)
            {
                ParticleSystem tps = Singleton<TankFactory>.Instance.GetPs(false);
                tps.transform.position = transform.position;
                tps.Play();
                Singleton<GameEventManager>.Instance.ChangeScore(1);
                Singleton<TankFactory>.Instance.ReCycleTank(gameObject);
            }
            else
                GetComponent<NavMeshAgent>().SetDestination(target);
        }
        else
        {
            GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            GetComponent<NavMeshAgent>().ResetPath();
        }
        _rigidbody.velocity = Vector3.zero;

    }

    IEnumerator Shoot()
    {
        while (GameEventManager.isContinue)
        {
            for (float i = 1; i > 0; i -= Time.deltaTime)
                yield return 0;
            if(Vector3.Distance(transform.position, target) < 20)
            {
                GameObject bullet = Singleton<TankFactory>.Instance.GetBullet();
                bullet.GetComponent<BulletController>().SetBullet(gameObject, 20);
            }
        }
    }

    public void Reset()
    {
        StartCoroutine(Shoot());
    }
}
