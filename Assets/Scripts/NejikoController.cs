using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour
{
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1.0f;

    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;
    int targetLane;

    public float gravity;
    public float speedZ;
    public float speedX;
    public float speedJump;
    public float accelerationZ;

    void Start()
    {
        // 必要なコンポーネントを自動取得
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //デバッグ用
        if (Input.GetKeyDown("left")) MoveToLeft();
        if (Input.GetKeyDown("right")) MoveToRight();
        if (Input.GetKeyDown("space")) Jump();

        // 徐々に加速しZ方向に常に前進させる
        float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
        moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);

        // x方向は目標のポジションまでの差分の割合で速度を計算
        float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
        moveDirection.x = ratioX * speedX;

        // // キャラが接地しているか
        // if (controller.isGrounded)
        // {
        //     // ↑キーを押下
        //     if (Input.GetAxis("Vertical") > 0.0f)
        //     {
        //         moveDirection.z = Input.GetAxis("Vertical") * speedZ;
        //     }
        //     // ↓キーを押下
        //     else
        //     {
        //         moveDirection.z = 0;
        //     }

        //     // ← or → キー押下
        //     transform.Rotate(0, Input.GetAxis("Horizontal") * 3, 0);

        //     // スペースキー押下
        //     if (Input.GetButton("Jump"))
        //     {
        //         moveDirection.y = speedJump;
        //         // アニメータの「jump」を実行
        //         animator.SetTrigger("jump");
        //     }
        // }

        // 重力分の力を毎フレーム追加
        moveDirection.y -= gravity * Time.deltaTime;

        // 移動実行
        // transform.TransformDirectionで変換しないとキャラの方向関係なくz軸に進んでしまう。
        // 変換するとキャラが向いている方向に進んでくれる
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        // 移動後接地してたらY方向の速度はリセットする
        // 重力が毎フレーム追加されるのでリセットしないと道から落ちた時に早く落ちる（ジャンプしたらspeedJumpの値に書き換えれられるので影響はない）
        if (controller.isGrounded) moveDirection.y = 0;

        // 速度が0以上なら走っているフラグをtrueにする
        animator.SetBool("run", moveDirection.z > 0.0f);
    }

    // 左のレーンに移動を開始
    public void MoveToLeft()
    {
        if (controller.isGrounded && targetLane > MinLane) targetLane--;
    }

    // 右のレーンに移動を開始
    public void MoveToRight()
    {
        if (controller.isGrounded && targetLane < MaxLane) targetLane++;
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            moveDirection.y = speedJump;

            // ジャンプトリガーを設定
            animator.SetTrigger("jump");
        }
    }
}
