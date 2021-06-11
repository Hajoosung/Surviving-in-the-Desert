using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;




[System.Serializable]

public class SaveData
{
    public Vector3 playerPos;
    public Vector3 playerRot;

    public List<int> invenArrayNumber = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNumber = new List<int>();

}


public class SaveNLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();

    private string Save_DATA_DIRECTORY;
    private string Save_FILENAME = "/SaveFile.txt";

    private PlayerController thePlayer;
    private Inventory theInven;

    // Start is called before the first frame update
    void Start()
    {
        Save_DATA_DIRECTORY = Application.dataPath + "/Saves/";

        if (Directory.Exists(Save_DATA_DIRECTORY))
            Directory.CreateDirectory(Save_DATA_DIRECTORY);
    }

   public void SaveData()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        theInven = FindObjectOfType<Inventory>();

        saveData.playerPos = thePlayer.transform.position;
        saveData.playerRot = thePlayer.transform.eulerAngles;

        Slot[] slots = theInven.GetSlots();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                saveData.invenArrayNumber.Add(i);
                saveData.invenItemName.Add(slots[i].item.itemName);
                saveData.invenItemNumber.Add(slots[i].itemCount);

            }
        }
        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(Save_DATA_DIRECTORY + Save_FILENAME, json);

        Debug.Log("json");
    }

    public void LoadData()
    {
        if (File.Exists(Save_DATA_DIRECTORY + Save_FILENAME))
        {
            string loadJson = File.ReadAllText(Save_DATA_DIRECTORY + Save_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            thePlayer = FindObjectOfType<PlayerController>();
            theInven = FindObjectOfType<Inventory>();


            thePlayer.transform.position = saveData.playerPos;
            thePlayer.transform.eulerAngles = saveData.playerRot;

            for (int i = 0; i < saveData.invenItemName.Count; i++)
            {
                theInven.LoadToInven(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenItemNumber[i]);
            }

        }
        else
            Debug.Log("세이브 파일 없음");
    }
}
