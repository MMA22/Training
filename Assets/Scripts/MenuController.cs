using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button btnPlant;
    public Button btnMove;
    public Button btnDelete;
    public Button btnChangeType;
    public Button btnChangeView;
    public Button btnSave;
    public Button btnLoad;
    public Text txtInfo;
    public GameController gameController;

    public enum eTreeStates
    {
        plant,
        move,
        delete,
        changeType
    }

    private bool isPlantView;
    private Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        getNormalView();        
        btnPlant.onClick.AddListener(onclickPlants);
        btnMove.onClick.AddListener(onclickMove);
        btnDelete.onClick.AddListener(onclickDelete);
        btnChangeType.onClick.AddListener(onclickChangeType);
        btnChangeView.onClick.AddListener(onclickChangeView);
        btnSave.onClick.AddListener(onclickSave);
        btnLoad.onClick.AddListener(onclickLoad);
    }

    public void onclickPlants()
    {
        txtInfo.text = "Plant Tree";
        gameController.state = GameController.eTreeStates.plant;
        
    }
    public void onclickMove()
    {
        txtInfo.text = "Move Tree";
        gameController.state = GameController.eTreeStates.move;
    }
    public void onclickDelete()
    {
        txtInfo.text = "Delete Tree";
        gameController.state = GameController.eTreeStates.delete;
    }
    public void onclickChangeType()
    {
        txtInfo.text = "Change Tree type";
        gameController.changeTypeAction();
        gameController.state = GameController.eTreeStates.noState;
    }
    public void onclickChangeView()
    {
        
        if (isPlantView)
        {
            getPlantView();
        } else
        {
            getNormalView();
        }
    }
    public void onclickSave()
    {
        txtInfo.text = "Save Tree info";
        gameController.saveAction();
    }
    public void onclickLoad()
    {
        txtInfo.text = "Load Tree info";
        gameController.loadAction();
    }

    private void getNormalView ()
    {
        txtInfo.text = "Normal View";
        btnPlant.enabled = false;
        gameController.state = GameController.eTreeStates.noState;
        isPlantView = true;
        mainCam.transform.position = new Vector3(45, 30, -100);
        mainCam.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void getPlantView()
    {
        txtInfo.text = "Plant View";
        btnPlant.enabled = true;
        isPlantView = false;
        mainCam.transform.position = new Vector3(45, 60, -50);
        mainCam.transform.rotation = Quaternion.Euler(50, 0, 0);
    }
}
