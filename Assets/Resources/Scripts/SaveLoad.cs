using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoad
{
    public static void saveTreeData(SaveData saveData)
    {
        Debug.Log("Saving tree");
        string json = JsonUtility.ToJson(saveData);
        string fileName = @"Assets/Resources/Text/SaveData.json";
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
        using (StreamWriter sw = File.CreateText(fileName))
        {
            Debug.Log(json);
            sw.WriteLine(json);
        }
    }

    public static SaveData loadTreeData(List<GameObject> lstTreeGO)
    {
        Debug.Log("Loading from json file....");
        TextAsset jsonData = Resources.Load<TextAsset>("Text/SaveData");

        if (jsonData != null)
        {
            SaveData data = JsonUtility.FromJson<SaveData>(jsonData.text);            
            return data;           
        }
        return null;
    }
    
}
