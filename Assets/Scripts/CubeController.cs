using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CubeController : MonoBehaviour
{
    public Image img;
    public Transform lookAtTarget;

    void Start()
    {
        //     transform.DOLocalMove(new Vector3(10f, 0, 0), 1f)
        // .SetEase(Ease.Linear);

        //     img.DOColor(new Color(1f, 0, 0), 1.5f);

        // transform.DOLocalRotate(new Vector3(0, 180f, 0), 2f);

        transform.DOLookAt(lookAtTarget.localPosition, 1f).SetDelay(1f).SetLoops(10, LoopType.Yoyo);

        img.DOFade(0.3f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
