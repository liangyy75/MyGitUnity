using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour {
    private MonsterController monsterController = null;

    private void Start()
    {
        monsterController = this.transform.parent.GetComponent<MonsterController>();
    }

    // 玩家进入自己的区域
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "player(Clone)")
            monsterController.Hit(other);
    }

    // 玩家脱离区域
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "player(Clone)")
            monsterController.Miss(other);
    }
}
