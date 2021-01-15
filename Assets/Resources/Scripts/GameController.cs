using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    
    public enum eTreeStates
    {
        noState,
        plant,
        move,
        delete
    }
    public eTreeStates state;    
    public GameObject cvMenu;
    public MoveTree movetree;


    private enum eTrees
    {
        AppleTree,
        OrangeTree,
        BanaTree
    }
    private GameObject appleTree;
    private GameObject orangeTree;
    private GameObject bananaTree;
    private SaveData save;
    private bool isLoad;
    private int id;
    private eTrees tree;
    private GameObject currentTree;
    private List<GameObject> lstTreeGO = new List<GameObject>();
    

    // Start is called before the first frame update
    void Start()
    {
        id = 0;
        isLoad = false;
        state = eTreeStates.noState;
        loadPrefabs();
        tree = eTrees.AppleTree;
        currentTree = appleTree;                
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                 switch (state)
                 {
                    case eTreeStates.noState:                        
                        break;
                    case eTreeStates.plant:                        
                        movetree.enabled = false;
                        plantAction();
                        break;
                    case eTreeStates.move:                        
                        movetree.enabled = true;
                        break;
                    case eTreeStates.delete:                        
                        movetree.enabled = false;
                        deleteAction();
                        break;                 
                 }
            }            
        }

        if (isLoad)
        {
            reposition();
            isLoad = false;
        }
    }

    public void plantAction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray))
        {
            GameObject objTree = Instantiate(currentTree, new Vector3(Input.mousePosition.x, 0, Input.mousePosition.z), transform.rotation);
            id += 1;
            objTree.GetComponent<Tree>().id = id;
            lstTreeGO.Add(objTree);
        }

    }

    public void deleteAction()
    {
        RaycastHit hitInfo;
        GameObject target = ReturnClickedObject(out hitInfo);
        if (target != null && !target.name.Equals("Floor"))
        {
            removeTree(target.GetComponent<Tree>().id);
            GameObject.Destroy(target);           
        }
    }

    public void changeTypeAction()
    {        
        switch (tree)
        {
            case eTrees.AppleTree:
                tree = eTrees.BanaTree;
                currentTree = bananaTree;                
                return;
            case eTrees.BanaTree:
                tree = eTrees.OrangeTree;
                currentTree = orangeTree;
                return;
            case eTrees.OrangeTree:
                tree = eTrees.AppleTree;
                currentTree = appleTree;
                return;
        }
    }

    public void saveAction() {
        SaveData saveData = new SaveData();
        saveData.lstTreeData.Clear();
        saveData.lstTreeData.AddRange(getTreeInfoFromTreeGameObject());
        SaveLoad.saveTreeData(saveData);
        state = eTreeStates.noState;
    }

    public void loadAction()
    {
        save = SaveLoad.loadTreeData(lstTreeGO);        
        isLoad = true;
        state = eTreeStates.noState;
    }

    private void loadPrefabs()
    {
        appleTree = Resources.Load("Prefabs/Tree/AppleTree") as GameObject;
        orangeTree = Resources.Load("Prefabs/Tree/OrangeTree") as GameObject;
        bananaTree = Resources.Load("Prefabs/Tree/BananaTree") as GameObject;
    }

    private GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {            
            target = hit.collider.gameObject;
        }
        return target;
    }

    private void removeTree(int id)
    {
        foreach (GameObject tree in lstTreeGO)
        {
            if (tree.GetComponent<Tree>().id == id)
            {                
                lstTreeGO.Remove(tree);
                return;
            }
        }
    }

    private List<SaveTreeInfo> getTreeInfoFromTreeGameObject()
    {
        List<SaveTreeInfo> lstTree = new List<SaveTreeInfo>();
        SaveTreeInfo saveTreeInfo; 
        foreach (GameObject tree in lstTreeGO)
        {
            saveTreeInfo = new SaveTreeInfo();
            saveTreeInfo.id = tree.GetComponent<Tree>().id;
            saveTreeInfo.position = tree.transform.position; 
            lstTree.Add(saveTreeInfo);
        }
        return lstTree;
    }

    private void reposition()
    {
        if (save != null && save.lstTreeData != null) 
        {
            foreach (GameObject t in lstTreeGO)
            {
                foreach (SaveTreeInfo saveTreeInfo in save.lstTreeData)
                {
                    if (t.GetComponent<Tree>().id == saveTreeInfo.id) 
                    {
                        Debug.Log("reposition " + saveTreeInfo.position.x + " , " + saveTreeInfo.position.y + " , " + saveTreeInfo.position.z);
                        t.transform.position = saveTreeInfo.position;
                        break;
                    }                        
                }
            }
        }
    }



}
