using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoad
{
    public static void saveTreeData(SaveData saveData)
    {
        Debug.Log("Saving tree");
        string json = JsonUtility.ToJson(saveData);
        Debug.Log(Application.dataPath);
        string fileName = Application.dataPath + "/Resources/Text/SaveData.text";
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
        Debug.Log("Save " + json);
        File.WriteAllText(fileName, json);               
    }

    public static SaveData loadTreeData()
    {
        Debug.Log("Loading from json file....");
        string fileName = Application.dataPath + "/Resources/Text/SaveData.text";
        if (File.Exists(fileName))
        {
            string saveString = File.ReadAllText(fileName);
            Debug.Log("load " + saveString);
            SaveData data = JsonUtility.FromJson<SaveData>(saveString);
            foreach (SaveTreeInfo stree in data.lstTreeData)
            {
                Debug.Log("id " + stree.id + " , type " + stree.type + " , position " + stree.position);
            }
            return data;
        }
        return null;   
    }
    
}
