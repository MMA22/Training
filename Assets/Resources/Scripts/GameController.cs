using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    public Button btnPlant;
    public Button btnDelete;
    public Button btnMove;
    public Button btnChangeType;
    public Button btnSave;
    public Button btnLoad;
    private enum eTrees
    {
        AppleTree,
        OrangeTree,
        BanaTree
    }

    private IState state;
    private eTrees tree;
    private GameObject objTree;
    private bool isMovable;
    private List<GameObject> lstSaveTree;
    private List<GameObject> lstTree = new List<GameObject>();
    private GameObject appleTree;
    private GameObject orangeTree;
    private GameObject bananaTree;
    private GameObject menu;

    // Start is called before the first frame update
    void Start()
    {   
        tree = eTrees.AppleTree;
        appleTree = Resources.Load<GameObject>("Models/AppleTree"); 
        orangeTree = Resources.Load<GameObject>("Models/OrangeTree");
        bananaTree = Resources.Load<GameObject>("Models/BananaTree");
        objTree = appleTree;
        lstSaveTree = new List<GameObject>();        
        initLoad();

        btnPlant.onClick.AddListener(onclickPlants);
        btnDelete.onClick.AddListener(onclickDelete);
        btnChangeType.onClick.AddListener(onclickChangeType);
        btnMove.onClick.AddListener(onclickMove);
        btnSave.onClick.AddListener(onclickSave);
        btnLoad.onClick.AddListener(onclickLoad);
        
        isMovable = false;
        Camera.main.GetComponent<MovingTrees>().enabled = isMovable;
    }

    // Update is called once per frame
    void Update()
    {
        if (state != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    state.doProcess(this);
                } else
                {                    
                    if (state.GetType() == typeof(LoadState))
                    {
                        Debug.Log("Enter load");
                        state.doProcess(this);
                        state = null;
                    }
                }
                    
            }
        }
    }    

    public void onclickChangeType()
    {
        Debug.Log("change type");
        state = new ChangePlantState();
        state.doProcess(this);
        state = new PlantState();
    }

    public void onclickDelete()
    {
        Debug.Log("delete tree");
        state = new DeleteState();
    }

    public void onclickMove()
    {
        Debug.Log("move tree");
        state = new MoveState();
    }

    public void onclickPlants()
    {
        Debug.Log("plant tree");
        state = new PlantState();
    }
    
    public void onclickSave()
    {
        saveAction();
    }

    public void onclickLoad()
    {
        Debug.Log("load tree");
        state = new LoadState();
    }

    public void plantAction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray))
        Instantiate(objTree, new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z), transform.rotation);
        lstTree.Add(objTree);
    }

    public void moveAction()
    {
        if (!isMovable)
        {
            isMovable = true;
            Camera.main.GetComponent<MovingTrees>().enabled = isMovable;
        }
    }

    public void deleteAction()
    {
        RaycastHit hitInfo;
        GameObject target = ReturnClickedObject(out hitInfo);
        if (target != null && !target.name.Equals("Floor"))
        {            
            GameObject.Destroy(target);           
        }
    }

    public void changeTypeAction()
    {
        if (tree.Equals(eTrees.AppleTree))
        {
            tree = eTrees.BanaTree;
            objTree = bananaTree;
            return;
        }
        if (tree.Equals(eTrees.BanaTree))
        {
            tree = eTrees.OrangeTree;
            objTree = orangeTree;
            return;
        }
        if (tree.Equals(eTrees.OrangeTree))
        {
            tree = eTrees.AppleTree;
            objTree = appleTree;
            return;
        }
    }

    public void saveAction() {
        Debug.Log("Saving tree");
        SaveData save = new SaveData();
        lstSaveTree.Clear();
        lstSaveTree.AddRange(lstTree);

        foreach (GameObject tree in lstSaveTree)
        {
            save.lstType.Add(tree.name);
            save.lstPosition.Add(tree.transform.position);
        }
        string json = JsonUtility.ToJson(save);       
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

    public void loadAction()
    {
        Debug.Log("Loading from json file....");
        TextAsset jsonData = Resources.Load<TextAsset>("Text/SaveData");
        
        if (jsonData != null)
        {
            SaveData data = JsonUtility.FromJson<SaveData>(jsonData.text);
            if (data != null && data.lstPosition != null && data.lstType != null)
            {               
                Debug.Log("save tree count " + lstSaveTree.Count);                
                for (int i = 0; i < lstSaveTree.Count; i++)
                {
                   lstSaveTree[i].transform.Translate(data.lstPosition[i]);
                }              
            }
        }
    }

    public void initLoad()
    {
        Debug.Log("init load");
        TextAsset jsonData = Resources.Load<TextAsset>("Text/SaveData");
        if (jsonData != null)
        {
            SaveData data = JsonUtility.FromJson<SaveData>(jsonData.text);
            if (data != null && data.lstPosition.Count != 0)
            {
                for (int i = 0; i < data.lstPosition.Count; i++)
                {
                    if (data.lstType[i].Contains("AppleTree"))
                    {
                        objTree = appleTree;
                    }
                    if (data.lstType[i].Contains("OrangeTree"))
                    {
                        objTree = orangeTree;
                    }
                    if (data.lstType[i].Contains("BananaTree"))
                    {
                        objTree = bananaTree;
                    }
                    Instantiate(objTree, data.lstPosition[i], transform.rotation);
                    lstTree.Add(objTree);
                }
            }
        }           
    }

    GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {            
            target = hit.collider.gameObject;
        }
        return target;
    }

}
