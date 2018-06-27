using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletController : NetworkBehaviour
{
    PlayerController playerController;

    public void SetBullet(GameObject player)
    {
        transform.position = new Vector3(player.transform.position.x, 1.5f, player.transform.position.z) + player.transform.forward * 2;
        transform.forward = player.transform.forward;   // 设置子弹方向
        GetComponent<Rigidbody>().velocity = player.transform.forward * 15;  // 发射子弹
        playerController = player.GetComponent<PlayerController>();
    }

    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var hitPlayer = hit.GetComponent<PlayerController>();

        if (hitPlayer != null)
        {
            if (gameObject.activeSelf && playerController != null)
                playerController.ReCycleBullet(gameObject);
            playerController.BeginShellExplosion(gameObject.transform.position);
            float distance = Vector3.Distance(hit.transform.position, transform.position);
            if (hitPlayer.GetHp() - 100 / distance < 0)
                playerController.AddScore();
            hitPlayer.Damage(100 / distance);
        }
    }

    void Update()
    {
        Vector3 pos = transform.position;
        if (pos.x > 50 || pos.z > 50 || pos.x < -50 || pos.z < -50 || pos.y <= 0 && playerController != null && gameObject.activeSelf)
        {
            playerController.BeginShellExplosion(gameObject.transform.position);
            playerController.ReCycleBullet(gameObject);
        }
    }
}
