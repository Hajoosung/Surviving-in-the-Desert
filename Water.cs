using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public static bool isWater = false;
    //isWater 상태 false로 둠

    [SerializeField]
    private float waterDrag;
    //물속 중력 변수 설정
    private float originDrag;
    //원래 중력 변수 설정

    [SerializeField]
    private Color watercolor;
    //물속 색
    [SerializeField]
    private float waterFogDesity;
    // 물속 탁함 정도

    private Color originColor;
    //원래 색깔
    private float originFogDesity;
    //원래 탁함 정도

    // Start is called before the first frame update
    void Start()
    {
        originColor = RenderSettings.fogColor;
        //originColor는 위의 것을 가져옴
        originFogDesity = RenderSettings.fogDensity;
        //originFogDesity는 위의 것을 가져옴

        originDrag = 0;
        // 현재 중력 = 0

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
            //만약 플레이어 태그된 오브젝트가 들어오면
        {
            GetWater(other);
            //함수 호출
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        //만약 플레이어 태그된 오브젝트가 나가면

        {
            GetOutWater(other);
            //함수 호출


        }

    }

    private void GetWater(Collider _player)
    {
        isWater = true;
        //isWater를 true로 변경
        _player.transform.GetComponent<Rigidbody>().drag = waterDrag;
        //플레이어의 중력을 waterDrag 값으로 변경

        RenderSettings.fogColor = watercolor;
        //색을 물속 색으로 변경
        RenderSettings.fogDensity = waterFogDesity;
        //탁함정도를 물속 탁함으로 변경

    }


    private void GetOutWater(Collider _player)

    {
        if (isWater)
        //만약 isWater이 true라면
        {
            isWater = false;
            //isWater를 false로 변경

            _player.transform.GetComponent<Rigidbody>().drag = originDrag;
            //플레이어의 중력을 originDrag 값으로 변경


            RenderSettings.fogColor = originColor;
            //색을 원래 색으로 변경
            RenderSettings.fogDensity = originFogDesity;
            //탁함정도를 원래 탁함으로 변경

        }
    }

}
