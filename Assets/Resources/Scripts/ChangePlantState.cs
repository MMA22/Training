using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlantState : IState
{
    public void doProcess(GameController controller)
    {
        controller.changeTypeAction();           
    }    
}
