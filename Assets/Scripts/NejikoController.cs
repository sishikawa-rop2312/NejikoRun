using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour
{
    const int MinLane = -2;  // 左側に移動できるMAXレーン数
    const int MaxLane = 2;  // 右側に移動できるMAXレーン数
    const float LaneWidth = 1.0f;   // 1レーンサイズ
    const int DefaultLife = 3;  // ライフ
    const float StunDuration = 0.5f;   // 気絶時間

    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;
    int targetLane;
    int life = DefaultLife;
    float recoverTime = 0.0f;

    public float gravity;
    public float speedZ;
    public float speedX;
    public float speedJump;
    public float accelerationZ; // 前進加速度のパラメータ

    public int Life()
    {
        return life;
    }

    bool IsStun()
    {
        return recoverTime > 0.0f || life <= 0;
    }

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

        if (IsStun())
        {
            // 動きをとm気絶状態からの復帰カウントを進める
            moveDirection.x = 0.0f;
            moveDirection.z = 0.0f;
            recoverTime -= Time.deltaTime;
        }
        else
        {
            // 徐々に加速しZ方向に常に前進させる
            float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
            moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ); // ある一定のスピードに達したらそれ以上スピードUPしない

            // x方向は目標のポジションまでの差分の割合で速度を計算
            float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
            moveDirection.x = ratioX * speedX;
        }


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
        if (IsStun()) return;
        if (controller.isGrounded && targetLane > MinLane) targetLane--;
    }

    // 右のレーンに移動を開始
    public void MoveToRight()
    {
        if (IsStun()) return;
        if (controller.isGrounded && targetLane < MaxLane) targetLane++;
    }

    public void Jump()
    {
        if (IsStun()) return;
        if (controller.isGrounded)
        {
            moveDirection.y = speedJump;

            // ジャンプトリガーを設定
            animator.SetTrigger("jump");
        }
    }

    // CharacterCOntrollerに衝突判定が生じたときの処理
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsStun()) return;

        if (hit.gameObject.CompareTag("Robo"))
        {
            // ライフを減らして気絶状態に移行
            life--;
            recoverTime = StunDuration;

            // ダメージトリガーを設定
            animator.SetTrigger("damage");

            // ヒットしたオブジェクトを削除
            Destroy(hit.gameObject);
        }
    }
}
