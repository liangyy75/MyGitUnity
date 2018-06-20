using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
    
    void Start ()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        forwardSpeed = 20;
        backwardSpeed = 15;
        rotateSpeed = 2;
    }
	
	void Update ()
    {
        if (!GameEventManager.isContinue)
            return;
        // 前后移动
        // if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        //     _rigidbody.velocity = transform.forward * forwardSpeed;
        // else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        //     _rigidbody.velocity = transform.forward * (0 - backwardSpeed);
        // else
        //     _rigidbody.velocity = Vector3.zero;

        float v = Input.GetAxis("Vertical");
        Vector3 velocity = transform.TransformDirection(new Vector3(0, 0, v));
        if (v > 0.1)
            velocity *= forwardSpeed;
        else if (v < -0.1)
            velocity *= backwardSpeed;
        transform.localPosition += velocity * Time.fixedDeltaTime;

        // 改变方向
        // if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        //     transform.Rotate(-transform.up * Time.deltaTime * 70 * rotateSpeed);
        // else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        //     transform.Rotate(transform.up * Time.deltaTime * 70 * rotateSpeed);

        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);

        _rigidbody.velocity = Vector3.zero;

        // 发射子弹
        timePointer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && timePointer > timeLimit)    // Input.GetAxis("Fire1")
        {
            timePointer = 0;
            GameObject bullet = Singleton<TankFactory>.Instance.GetBullet();
            bullet.GetComponent<BulletController>().SetBullet(gameObject, 25);
        }

        // 如果血量归0就死亡
        if (GetComponent<Health>().GetHp() <= 0)
        {
            Singleton<GameEventManager>.Instance.GameOver();
            ParticleSystem tps = Singleton<TankFactory>.Instance.GetPs(false);
            tps.transform.position = transform.position;
            tps.Play();
            GameObject.FindGameObjectWithTag("MainCamera").transform.parent = null;
            gameObject.SetActive(false);
            timePointer = 0;
        }
    }
}
