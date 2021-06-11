using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{

    // 기존 위치.
    private Vector3 originPos;

    // 현재 위치.
    private Vector3 currentPos;

    // sway 한계.
    [SerializeField]
    private Vector3 limitPos;


    // 부드러운 움직임 정도.
    [SerializeField]
    private Vector3 smoothSway;


    // 필요한 컴포넌트/
    [SerializeField]
    private GunController theGunController;


    // Use this for initialization
    void Start()
    {
        originPos = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.canPlayMove)
        {
            TrySway();
        }
    }


    private void TrySway()
    {
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
            Swaying();
        else
            BackToOriginPos();
    }

    private void Swaying()
    {
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

       
        
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(originPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x),
               Mathf.Clamp(Mathf.Lerp(originPos.y, -_moveY, smoothSway.x), -limitPos.y, limitPos.y),
               originPos.z);
       
        
        transform.localPosition = originPos;
    }

    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(originPos, originPos, smoothSway.x);
        transform.localPosition = originPos;
    }
}
