using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IState
{
    public void doProcess(GameController controller)
    {
        Debug.Log("move state");
        controller.moveAction();
    }
}
