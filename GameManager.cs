using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public string labelText = "                      마을에 있는 몬스터를  소탕해주세요!";
    public bool showWindow = false;
    public bool showScoreWindow = false;

    private GameManager gameManager;



    public static bool canPlayMove = true; //플레이어의 움직임 제어
    public static bool isOpenInventory = false; // 인벤토리 활성화

    public static bool isNight = false;
    public static bool isWater = false;

    public static bool isPause = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isOpenInventory || isPause)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canPlayMove = false;

        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canPlayMove = true;
        }

    }


    private int _Score = 0;

    public int Score
    {
        get { return _Score; }
        set
        {
            _Score = value;

            if (_Score >= 30)
            {
                Cursor.lockState = CursorLockMode.None;

                Cursor.visible = true;

                SceneManager.LoadScene("GameClear");



            }
        }
    }


    void OnGUI()
    {
      
        GUI.Box(new Rect(20, 50, 150, 25), "잡은 횟수 : " + _Score + " / 30");
        GUI.Label(new Rect(Screen.width / 2 - 220, Screen.height - 80, 500, 50), labelText);


       
    }
}
