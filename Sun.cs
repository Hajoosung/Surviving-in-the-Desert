using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    [SerializeField]
    private float secondPerRealTimeSecond;

    public GameObject BigBlueMonster;
    public GameObject BigBlueMonster1;

    public GameObject BigBlueMonster2;
    public GameObject BigBlueMonster3;
    public GameObject BigBlueMonster4;
    public GameObject BigBlueMonster5;


    public GameObject warning;


    [SerializeField]
    private float fogDensityCalc;

    [SerializeField]
    private float nightFogDensity;


    private float dayFogDensity;
    private float currentFogDensity;


   

    // Start is called before the first frame update
    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;

        BigBlueMonster.SetActive(false);
        BigBlueMonster1.SetActive(false);

        BigBlueMonster2.SetActive(false);
        BigBlueMonster3.SetActive(false);
        BigBlueMonster4.SetActive(false);
        BigBlueMonster5.SetActive(false);


        warning.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        if (transform.eulerAngles.x >= 180)
        {
            GameManager.isNight = true;
            
            BigBlueMonster.SetActive(true);
            BigBlueMonster1.SetActive(true);

            BigBlueMonster2.SetActive(true);
            BigBlueMonster3.SetActive(true);
            BigBlueMonster4.SetActive(true);
            BigBlueMonster5.SetActive(true);


            warning.SetActive(true);


        }
        else if (transform.eulerAngles.x <= 0)

        {
            GameManager.isNight = false;
            BigBlueMonster.SetActive(false);
                        BigBlueMonster1.SetActive(false);

            BigBlueMonster2.SetActive(false);
            BigBlueMonster3.SetActive(false);
            BigBlueMonster4.SetActive(false);
            BigBlueMonster5.SetActive(false);


            warning.SetActive(false);

        }


        if (GameManager.isNight)
        {
            if (currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {  
            if (currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        } 
    }
}
