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
    public enum eTrees
    {
        AppleTree,
        OrangeTree,
        BananaTree
    }
    public eTrees tree;
    public GameObject cvMenu;

    private GameObject appleTree;
    private GameObject orangeTree;
    private GameObject bananaTree;
    private SaveData save;
    private bool isLoad;
    private bool isMovable;
    private int id;

    private string treeType;
    private GameObject currentTree;
    private List<GameObject> lstTreeGO;
    private GameObject moveTarget = null;
    private bool isMouseDragging;

    // Start is called before the first frame update
    void Start()
    {
        id = 0;
        isLoad = false;
        isMovable = false;
        state = eTreeStates.noState;
        loadData();
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
                        isMovable = false;
                        plantAction();
                        break;
                    case eTreeStates.move:                        
                        isMovable = true;
                        moveAction();
                        break;
                    case eTreeStates.delete:
                        isMovable = false;
                        deleteAction();
                        break;                 
                 }
            }            
        }

        if (Input.GetMouseButtonUp(0) && isMovable)
        {
            isMouseDragging = false;
            if (moveTarget != null)
            {
                moveTarget.GetComponent<Collider>().enabled = true;
            }            
        }

        if (isMouseDragging && isMovable)
        {
            if (moveTarget != null)
            {
                moveTarget.GetComponent<Collider>().enabled = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 200))
                {
                    isMouseDragging = true;
                    moveTarget.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
                }                
            }
        }

        if (isLoad)
        {
            loadTreePlant();
            isLoad = false;
        }
    }

    public void plantAction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200))
        {
            if (hit.collider.tag.Equals("ground"))
            {
                GameObject objTree = Instantiate(currentTree, hit.point, Quaternion.identity);
                id += 1;
                objTree.GetComponent<Tree>().id = id;
                objTree.GetComponent<Tree>().type = tree.ToString("g");
                lstTreeGO.Add(objTree);
            }
        }
    }

    public void moveAction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200))
        {
            if (hit.collider.tag.Equals("Tree"))
            {
                isMouseDragging = true;                
                moveTarget = hit.collider.gameObject;                
            }
        }
    }

    public void deleteAction()
    {
        RaycastHit hitInfo;
        GameObject target = ReturnClickedObject(out hitInfo);
        if (target != null && !target.tag.Equals("ground"))
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
                tree = eTrees.BananaTree;
                currentTree = bananaTree;                
                return;
            case eTrees.BananaTree:
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
        save = SaveLoad.loadTreeData();
        isLoad = true;
        state = eTreeStates.noState;
    }
    private void loadData()
    {
        appleTree = Resources.Load("Prefabs/Tree/AppleTree") as GameObject;
        orangeTree = Resources.Load("Prefabs/Tree/OrangeTree") as GameObject;
        bananaTree = Resources.Load("Prefabs/Tree/BananaTree") as GameObject;
        lstTreeGO = new List<GameObject>();
        save = SaveLoad.loadTreeData();
        loadTreePlant();
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
                Destroy(tree);
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
            saveTreeInfo.type = tree.GetComponent<Tree>().type;
            saveTreeInfo.position = tree.transform.position; 
            lstTree.Add(saveTreeInfo);
        }
        return lstTree;
    }

    private void loadTreePlant()
    {
        foreach (GameObject tree in lstTreeGO)
        {
            Destroy(tree);
        }
        lstTreeGO.Clear();

        if (save != null && save.lstTreeData != null) 
        {
            GameObject plantTree = null;
            foreach (SaveTreeInfo saveTreeInfo in save.lstTreeData)
            {
                if (saveTreeInfo.type.Equals("AppleTree")) {
                    plantTree = appleTree;
                }
                if (saveTreeInfo.type.Equals("OrangeTree")) {
                    plantTree = orangeTree;
                }
                if (saveTreeInfo.type.Equals("BananaTree")) {
                    plantTree = bananaTree;
                }
                GameObject objTree = Instantiate(plantTree, saveTreeInfo.position, Quaternion.identity);
                id += 1;
                objTree.GetComponent<Tree>().id = id;
                objTree.GetComponent<Tree>().type = saveTreeInfo.type;
                lstTreeGO.Add(objTree);                
            }
        }
    }



}
