using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class MonsterController : MonoBehaviour {
    // 怪物巡逻路径
    public List<Vector3> path = new List<Vector3>();
    // 怪物在路径上那个节点
    private int currentNode = 0;
    // 怪物行进的速度
    private int speed = 4;

    private Animator animator;
    private Rigidbody _rigidbody;
    private AnimatorStateInfo stateInfo;

    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int walkState = Animator.StringToHash("Base Layer.Walk");
    static int runState = Animator.StringToHash("Base Layer.Run");
    static int attackState = Animator.StringToHash("Base Layer.Attack");

    private void Awake()
    {
        transform.GetChild(3).gameObject.AddComponent<AreaController>();
    }

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        SetPath();
    }

    void FixedUpdate()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!GameEventManager.isContinue)
        {
            animator.SetBool("isAttack", false);
            animator.SetBool("isWalk", false);
            animator.SetBool("isIdle", true);
            return;
        }
        // 查看四周后继续巡逻
        if (stateInfo.fullPathHash == idleState && !animator.IsInTransition(0))
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalk", true);
        }
        // 追踪玩家
        else if (stateInfo.fullPathHash == runState && !animator.IsInTransition(0))
        {
            MoveToTarget(path[currentNode], 4);
        }
        // 巡逻
        else if (stateInfo.fullPathHash == walkState && !animator.IsInTransition(0))
        {
            Vector3 targetPos = path[currentNode];
            animator.SetBool("isWalk", true);
            MoveToTarget(targetPos, 8);
            if (Vector3.Distance(transform.position, targetPos) < 0.01)
            {
                // 巡逻一会后查看四周
                currentNode = (currentNode + 1) % path.Count;
                animator.SetBool("isIdle", true);
                animator.SetBool("isWalk", false);
            }
        }
    }

    // 玩家进入怪物领域
    public void Hit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            path.Clear();
            path.Add(other.gameObject.transform.position);
            animator.SetBool("isWalk", false);
            animator.SetBool("isIdle", false);
            animator.SetBool("isRun", true);
            currentNode = 0;
        }
    }

    // 玩家脱离怪物领域
    public void Miss(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            SetPath();
            animator.SetBool("isRun", false);
            Singleton<GameEventManager>.Instance.ChangeScore();
        }
    }

    // 碰撞到墙或者其他怪物的对策
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Monster")
            currentNode = (currentNode + 1) % path.Count;
        if (collision.gameObject.tag == "Wall")
            SetPath();
    }

    // 进行袭击动作
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            animator.SetBool("isAttack", true);
            Singleton<GameEventManager>.Instance.PatrolGameOver();
        }
    }

    // 移动到目标位置
    public void MoveToTarget(Vector3 targetPos, int speedNum)
    {
        if (Vector3.Distance(transform.position, targetPos) > 0.01)
        {
            Vector3 direction = targetPos - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = rotation;
            direction = direction.normalized;
            transform.localPosition += direction * Time.fixedDeltaTime / speedNum;
        }
    }

    // 得到巡逻方位
    private void SetPath()
    {
        path.Clear();
        float length = Random.Range(1f, 2f);
        Vector3 right = new Vector3(transform.right.x, 0, transform.right.z).normalized;
        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        Vector3 pos = transform.position - forward * 2;
        path.Add(pos);
        path.Add(pos + right * length);
        path.Add(pos - forward * length + right * length);
        path.Add(pos - forward * length);
        currentNode = 0;
    }

    // 重置
    public void Reset()
    {
        SetPath();
        animator.SetBool("isWalk", false);
        animator.SetBool("isRun", false);
        animator.SetBool("isAttack", false);
        animator.SetBool("isIdle", true);
    }
}
