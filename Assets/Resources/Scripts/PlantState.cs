using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantState : IState 
{
    public void doProcess(GameController controller)
    {
        Debug.Log("plant state");
        controller.plantAction();  
    }
}
    