using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button btnPlant;
    public Button btnMove;
    public Button btnDelete;
    public Button btnChangeType;
    public Button btnSave;
    public Button btnLoad;
    public GameController gameController;

    public enum eTreeStates
    {
        plant,
        move,
        delete,
        changeType
    }

    // Start is called before the first frame update
    void Start()
    {
        btnPlant.onClick.AddListener(onclickPlants);
        btnMove.onClick.AddListener(onclickMove);
        btnDelete.onClick.AddListener(onclickDelete);
        btnChangeType.onClick.AddListener(onclickChangeType);
        btnSave.onClick.AddListener(onclickSave);
        btnLoad.onClick.AddListener(onclickLoad);
    }

    public void onclickPlants()
    {
        Debug.Log("plant tree");
        gameController.state = GameController.eTreeStates.plant;
    }
    public void onclickMove()
    {
        Debug.Log("move tree");
        gameController.state = GameController.eTreeStates.move;
    }
    public void onclickDelete()
    {
        Debug.Log("delete tree");
        gameController.state = GameController.eTreeStates.delete;
    }
    public void onclickChangeType()
    {
        Debug.Log("change type tree");        
        gameController.changeTypeAction();
        gameController.state = GameController.eTreeStates.plant;
    }
    public void onclickSave()
    {
        Debug.Log("save trees");
        gameController.saveAction();
    }
    public void onclickLoad()
    {
        Debug.Log("load trees");
        gameController.loadAction();
    }
}
