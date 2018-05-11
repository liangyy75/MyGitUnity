using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    private Animator animator;
    private CapsuleCollider capsule;
    private Rigidbody _rigidbody;

    public float animatorSpeed;
    public float forwardSpeed = 0.7f;
    public float backwardSpeed = 0.2f;
    public float rotateSpeed = 2.0f;
    public float jumpPower = 1.5f;

    private AnimatorStateInfo stateInfo;
    private Vector3 velocity;

    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int locoState = Animator.StringToHash("Base Layer.Locomotion");
    static int jumpState = Animator.StringToHash("Base Layer.Jump");
    static int restState = Animator.StringToHash("Base Layer.Rest");

    // Use this for initialization
    void Start () {
        animator = gameObject.GetComponent<Animator>();
        capsule = gameObject.GetComponent<CapsuleCollider>();
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        animatorSpeed = 1;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (!GameEventManager.isContinue)
        {
            animator.SetFloat("Speed", 0);
            animator.SetFloat("Direction", 0);
            animator.SetBool("Jump", false);
            return;
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        animator.SetFloat("Speed", v);
        animator.SetFloat("Direction", h);
        animator.speed = animatorSpeed;
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        velocity = this.transform.TransformDirection(new Vector3(0, 0, v));
        if (v > 0.1)
            velocity *= forwardSpeed;
        else if (v < -0.1)
            velocity *= backwardSpeed;

        // 按下空格
        if (Input.GetButtonDown("Jump"))
        {
            // 如果处于运动状态，且并非过渡时期
            if (stateInfo.fullPathHash == locoState && !animator.IsInTransition(0))
            {
                _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                animator.SetBool("Jump", true);
            }
        }

        // 移动 和 改变方向
        transform.localPosition += velocity * Time.fixedDeltaTime;
        transform.Rotate(0, h * rotateSpeed, 0);

        // 重置
        if (stateInfo.fullPathHash == jumpState && !animator.IsInTransition(0))
        {
            animator.SetBool("Jump", false);
            _rigidbody.isKinematic = false;
        }
    }

    public void Reset()
    {
        transform.position = new Vector3(0, 0, -1f);
        transform.rotation = Quaternion.identity;
        animator.SetFloat("Speed", 0);
        animator.SetFloat("Direction", 0);
        animator.SetBool("Jump", false);
        _rigidbody.isKinematic = false;
        velocity = Vector3.zero;
    }
}
