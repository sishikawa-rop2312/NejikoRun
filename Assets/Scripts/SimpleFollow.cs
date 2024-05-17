using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    Vector3 diff;

    public GameObject target;
    public float followSpeed;

    void Start()
    {
        diff = target.transform.position - transform.position;
    }

    // 全てのupdate()が終了したら実行されるのがLateUpdate関数
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            target.transform.position - diff,
            Time.deltaTime * followSpeed
        );
    }
}
