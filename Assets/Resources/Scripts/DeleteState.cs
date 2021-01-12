using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteState : IState
{
    public void doProcess(GameController controller)
    {
        Debug.Log("delete state");
        controller.deleteAction();
    }
}
