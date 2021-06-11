using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Water : MonoBehaviour
{
  

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

    [SerializeField]
    private StatusController thePlayerStatus;

    [SerializeField] private Color waterNightColor;
    [SerializeField] private float waterNightFogDensity;

    [SerializeField] private string sound_WaterOut;
    [SerializeField] private string sound_WaterIn;
    [SerializeField] private string sound_Breathe;

    [SerializeField] private float breatheTime;
    private float currentBreatheTime;

    [SerializeField] private float totalOxyzen;
    private float currentOxyzen;
    private float temp;

    [SerializeField] private GameObject go_BaseUi;
    [SerializeField] private Text text_totalOxyzen;
    [SerializeField] private Text text_currentOxyzen;
    [SerializeField] private Image image_guage;


    private StatusController thePlayerStat;

    private Color originColor;
    //원래 색깔
    private float originFogDesity;


    [SerializeField]
    private Color originNightColor;
    [SerializeField]
    private float originNightFogDensity;
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

        thePlayerStat = FindObjectOfType<StatusController>();
        currentOxyzen = totalOxyzen;

        text_totalOxyzen.text = totalOxyzen.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isWater)
        {

            thePlayerStat.IncreaseThirsty(1);

            currentBreatheTime += Time.deltaTime;
            if (currentBreatheTime >= breatheTime)
            {
                SoundManager.instance.PlaySE(sound_Breathe);
                currentBreatheTime = 0;



            }
        }
        DecreaseOxyzen();

    }
    private void DecreaseOxyzen()
    {
        if(GameManager.isWater)
        {
            currentOxyzen -= Time.deltaTime;
            text_currentOxyzen.text = Mathf.RoundToInt(currentOxyzen).ToString();
            //반올림
            image_guage.fillAmount = currentOxyzen / totalOxyzen;

            if(currentOxyzen <= 0)
            {
                temp += Time.deltaTime;
                if(temp >= 1)
                {
                    thePlayerStat.DecreaseHP(1);
                    temp = 0;

                }
            }
        }
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
        SoundManager.instance.PlaySE(sound_WaterIn);

        go_BaseUi.SetActive(true);
        GameManager.isWater = true;
        //isWater를 true로 변경
        _player.transform.GetComponent<Rigidbody>().drag = waterDrag;
        //플레이어의 중력을 waterDrag 값으로 변경

        if(!GameManager.isWater)
        {
            RenderSettings.fogColor = watercolor;
            RenderSettings.fogDensity = waterFogDesity;
        }
        else
        {
            RenderSettings.fogColor = waterNightColor;
            RenderSettings.fogDensity = waterNightFogDensity ;
        }

        

    }


    private void GetOutWater(Collider _player)

    {
        go_BaseUi.SetActive(false);

        currentOxyzen = totalOxyzen;
        SoundManager.instance.PlaySE(sound_WaterOut);

        if (GameManager.isWater)
        //만약 isWater이 true라면
        {
            GameManager.isWater = false;
            //isWater를 false로 변경

            _player.transform.GetComponent<Rigidbody>().drag = originDrag;
            //플레이어의 중력을 originDrag 값으로 변경

            if(!GameManager.isNight)
            {
                RenderSettings.fogColor = originColor;
                //색을 원래 색으로 변경
                RenderSettings.fogDensity = originFogDesity;
                //탁함정도를 원래 탁함으로 변경

            }
            else
            {
                RenderSettings.fogColor = originNightColor;
                //색을 원래 색으로 변경
                RenderSettings.fogDensity = originNightFogDensity;
                //탁함정도를 원래 탁함으로 변경
            }
        }
    }

}
