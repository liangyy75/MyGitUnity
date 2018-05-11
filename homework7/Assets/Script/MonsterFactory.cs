using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour {
    // 怪物预制
    public GameObject monster;
    // 生产的怪物
    private List<GameObject> monsters;

    // 怪物位置限制
    private float min_X = -3.75f;
    private float min_Y = -3.75f;
    private float max_X = 3.75f;
    private float max_Y = 3.75f;
    // 墙的厚度
    private float wallThick = 0.05f;
    // 格子长度
    private float cellLength = 2.5f;
    // 地图的行数与列数
    private int row = 3;
    private int col = 3;
    // 每个格子怪物的数量
    private int monsterNumEveryCell = 2;
    // 是否是第一次start game
    private bool flag = true;

    private void Awake()
    {
        monster = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/monster"), new Vector3(0, 100, 0), Quaternion.identity);
        monsters = new List<GameObject>();
    }

    // 得到所有的怪物
    public List<GameObject> GetMonsters()
    {
        int num = row * col * monsterNumEveryCell;
        for(int i = 0; i < num; i++) {
            GameObject monsterClone = Instantiate(monster, Vector3.zero, Quaternion.identity);
            monsterClone.gameObject.AddComponent<MonsterController>();
            monsters.Add(monsterClone);
        }
        SetMonstersPostion();
        return monsters;
    }

    public void Reset()
    {
        if (flag)
            flag = false;
        else
            SetMonstersPostion();
        int num = row * col * monsterNumEveryCell;
        for (int i = 0; i < num; i++)
            monsters[i].GetComponent<MonsterController>().Reset();
    }

    private void SetMonstersPostion()
    {
        int index = 0;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                // 每个格子的大致范围
                float minX = min_X + i * cellLength + wallThick;
                float minY = min_Y + j * cellLength + wallThick;
                float maxX = min_X + (i + 1) * cellLength - wallThick;
                float maxY = min_Y + (j + 1) * cellLength - wallThick;
                List<Vector3> allLocations = new List<Vector3>();
                // 在格子内怪物设置怪物的位置
                for (int l = 0; l < monsterNumEveryCell; l++)
                {
                    float x = Random.Range(minX, maxX);
                    float y = Random.Range(minY, maxY);
                    // 防止怪物生成在玩家附近
                    if (x < 0.5 && x > -0.5)
                        x = Random.Range(minX, maxX);
                    if (y < 0.5 && y > -0.5)
                        y = Random.Range(minY, maxY);
                    // 防止怪物生成在同一个地方
                    Vector3 location = new Vector3(x, 0, y);
                    foreach (var loc in allLocations)
                        if(Vector3.Distance(loc, location) < 0.01)
                        {
                            l--;
                            continue;
                        }
                    monsters[index++].gameObject.transform.position = location;
                }
            }
        }
    }
}
