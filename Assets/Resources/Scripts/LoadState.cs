using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadState : IState
{
    public void doProcess(GameController controller)
    {
        Debug.Log("load state");
        controller.loadAction();
    }
}
