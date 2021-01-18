using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<SaveTreeInfo> lstTreeData = new List<SaveTreeInfo>();    
}

[System.Serializable]
public class SaveTreeInfo
{
    public int id;
    public string type;
    public Vector3 position; 
}
